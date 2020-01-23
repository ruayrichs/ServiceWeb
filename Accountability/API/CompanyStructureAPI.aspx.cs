using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Accountability.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service.Workflow;

namespace ServiceWeb.Accountability.API
{
    public partial class CompanyStructureAPI1 : System.Web.UI.Page
    {
        DBService db = new DBService();
        AccountabilityService accountService = new AccountabilityService();
        private string WorkGroupCode
        {
            get
            {
                return Request["WorkGroupCode"];
            }
        }

        private const string REQUEST_ACTION_HIERARCHY = "get_hierarchy";
        private const string REQUEST_ACTION_MOVENODE = "movenode";
        private const string REQUEST_ACTION_RENAME = "rename";
        private const string REQUEST_ACTION_NEWFOLDER = "newfolder";
        private const string REQUEST_ACTION_DELETE = "delete";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            string requestAction = Request["request"];
            string ResponseJSON = "";

            if (requestAction.Equals(REQUEST_ACTION_HIERARCHY))
            {
                ResponseJSON = GetHierarchy();
            }
            if (requestAction.Equals(REQUEST_ACTION_MOVENODE))
            {
                MoveNode();
            }
            if (requestAction.Equals(REQUEST_ACTION_RENAME))
            {
                RenameNode();
            }
            if (requestAction.Equals(REQUEST_ACTION_NEWFOLDER))
            {
                NewFolder();
            }
            if (requestAction.Equals(REQUEST_ACTION_DELETE))
            {
                DeleteFolder();
            }


            Response.Write(ResponseJSON);
        }

        #region Manage Hierarchy

        public class ProductHierarchyManagerEntity
        {
            public string type { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public int index { get; set; }
            public string countdata { get; set; }
            public List<ProductHierarchyManagerEntity> tree { get; set; }
        }

        private List<CompanyStructureModel> _ListProductCategory = null;
        private List<CompanyStructureModel> ListProductCategory
        {
            get
            {
                return _ListProductCategory == null ? CompanyStructureModel.GetProductCategory(ERPWAuthentication.SID, WorkGroupCode) : _ListProductCategory;
            }
        }
        private string GetHierarchy()
        {
            List<ProductHierarchyManagerEntity> RootHierarchy = ManageHierarchyManagerAllProject(0, "");
            JArray returnJSON = ManageHierarchyManagerJSON(RootHierarchy);
            return JsonConvert.SerializeObject(returnJSON);
        }
        private List<ProductHierarchyManagerEntity> ManageHierarchyManagerAllProject(int NodeLevel, string ParentNodeCode)
        {
            var Hierarchy = ListProductCategory.Where(
                a =>
                a.NodeLevel == NodeLevel &&
                a.NodeParentCode.Equals(ParentNodeCode)
            ).OrderBy(o => o.StructureName).ToList();

            List<ProductHierarchyManagerEntity> RootHierarchy = new List<ProductHierarchyManagerEntity>();
            foreach (var h in Hierarchy)
            {
                List<ProductHierarchyManagerEntity> innerList = ManageHierarchyManagerAllProject((int)h.NodeLevel + 1, h.StructureCode);

                int CountItem = h.CountItem + innerList.Sum(a => Convert.ToInt32(a.countdata));
                ProductHierarchyManagerEntity pm = new ProductHierarchyManagerEntity
                {
                    id = h.StructureCode,
                    name = h.StructureName,
                    type = "f",
                    countdata = CountItem.ToString(),
                    tree = innerList
                };

                RootHierarchy.Add(pm);
            }
            return RootHierarchy.ToList();
        }
        private JArray ManageHierarchyManagerJSON(List<ProductHierarchyManagerEntity> inputList)
        {
            JArray returnJSON = new JArray();
            foreach (var List in inputList)
            {
                JObject obj = new JObject();
                obj.Add("id", List.id);
                obj.Add("name", List.name);
                obj.Add("type", List.type);
                obj.Add("countdata", List.countdata);
                JArray InnerTree = List.tree.Count() == 0 ? new JArray() : ManageHierarchyManagerJSON(List.tree);
                obj.Add("tree", InnerTree);
                returnJSON.Add(obj);
            }
            return returnJSON;
        }


        #endregion

        #region Move Node
        private void MoveNode()
        {
            string id = Request["itemNode"];
            string newParent = Request["newParentNode"];
            MoveMatHierachyCatalog(ERPWAuthentication.SID, id, newParent, ERPWAuthentication.EmployeeCode);
        }

        public void MoveMatHierachyCatalog(string sid, string NodeID, string NewParentNodeID, string UpdateBy)
        {
            var CurrentNode = ListProductCategory.Where(a => a.StructureCode.Equals(NodeID)).First();
            int NextLevel = GetNextNodeLavel(NewParentNodeID);
            string sql = getStrSqlMoveNode(CurrentNode, NewParentNodeID, NextLevel);
            db.executeSQLForFocusone(sql);
        }

        private string getStrSqlMoveNode(CompanyStructureModel node, string NewParentNodeID, int NextLevel)
        {
            var nodeStruc = GetNewNodeStructure(NewParentNodeID);
            string NewNodeHierarchyCode = nodeStruc.NodeHierarchyCode;
            string NewParentCode = nodeStruc.NodeParentCode;


            string sql = "update ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE_ITEM set NodeHierarchyCode = '" + NewNodeHierarchyCode + "'";
            sql += ",NodeParentCode = '" + NewParentCode + "'";
            sql += ",NodeLevel = '" + NextLevel + "'";
            sql += " where sid = '" + node.SID + "' and StructureCode = '" + node.StructureCode + "' and WorkGroupCode = '" + WorkGroupCode + "';";

            var Hierarchy = ListProductCategory.Where(
                a =>
                a.NodeLevel == (node.NodeLevel + 1) &&
                a.NodeParentCode.Equals(node.StructureCode)
            ).ToList();

            node.NodeHierarchyCode = NewNodeHierarchyCode;
            node.NodeParentCode = NewParentCode;
            node.NodeLevel = NextLevel;

            foreach (var n in Hierarchy)
            {
                sql += getStrSqlMoveNode(n, node.StructureCode, NextLevel + 1);
            }

            return sql;
        }

        #endregion

        #region Rename | New Folder | Delete Folder

        private void RenameNode()
        {
            string id = Request["id"];
            string newName = Request["name"];
            string sql = "update ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE_ITEM set StructureName = '" + newName + "'";
            sql += " where sid = '" + ERPWAuthentication.SID + "' and StructureCode = '" + id + "' and WorkGroupCode = '" + WorkGroupCode + "';";
            db.executeSQLForFocusone(sql);
        }

        private string NewFolder()
        {
            string parentID = Request["parentid"];
            string name = Request["name"];

            string result = "";

            var nodeStruc = GetNewNodeStructure(parentID);

            String sqlNodeLevel = @"SELECT Top 1 [SID],[CompanyCode],[StructureCode],[Name],[Description],[NodeLevel],[created_by],[created_on] 
                                    FROM [ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE] 
                                    where SID = '" + ERPWAuthentication.SID + @"'
                                        and CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'
                                        and WorkGroupCode = '" + WorkGroupCode + @"' 
                                    ORDER BY [NodeLevel] desc";

            DataTable dataTableLinkProjectCompanyStructure = db.selectDataFocusone(sqlNodeLevel);

            if (dataTableLinkProjectCompanyStructure.Rows.Count > 0)
            {
                if (Convert.ToInt32(nodeStruc.NodeLevel) <= Convert.ToInt32(dataTableLinkProjectCompanyStructure.Rows[0]["NodeLevel"]))
                {
                    string sql = @"insert into ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE_ITEM 
                    (SID,WorkGroupCode,StructureCode,StructureName,NodeHierarchyCode,NodeParentCode,NodeLevel,Description,created_on,created_by) values(";
                    sql += "'" + ERPWAuthentication.SID + "'";
                    sql += ",'" + WorkGroupCode + "'";
                    sql += ",'" + GetGenerateNextID("ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE_ITEM", "StructureCode", "CT", 10) + "'";
                    sql += ",'" + name + "'";
                    sql += ",'" + nodeStruc.NodeHierarchyCode + "'";
                    sql += ",'" + nodeStruc.NodeParentCode + "'";
                    sql += ",'" + nodeStruc.NodeLevel + "'";
                    sql += ",''";
                    sql += ",'" + Validation.getCurrentServerStringDateTime() + "'";
                    sql += ",'" + ERPWAuthentication.EmployeeCode + "'";
                    sql += ");";

                    db.executeSQLForFocusone(sql);
                    result = "สร้างสำเร็จ";
                }
                else
                {
                    result = "Node Level ต้องไม่เกิน " + dataTableLinkProjectCompanyStructure.Rows[0]["NodeLevel"];
                    //ClientService.AGMessage("Node Level ต้องไม่เกิน " + dataTableLinkProjectCompanyStructure.Rows[0]["NodeLevel"]);
                }
            }
            return result;
        }

        private void DeleteFolder()
        {
            string id = Request["id"];
            var CurrentNode = ListProductCategory.Where(a => a.StructureCode.Equals(id)).First();
            db.executeSQLForFocusone(GetSqlDeleteNode(CurrentNode));
            accountService.DeleteParticipantsByCompanyStructureCode(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                id,
                WorkGroupCode);
            //accountService.DeleteInitiativeOwner(
            //    ERPWAuthentication.SID,
            //    ERPWAuthentication.CompanyCode,
            //    id,
            //    "",
            //    WorkGroupCode);
        }
        private string GetSqlDeleteNode(CompanyStructureModel node)
        {
            string sql = "delete from ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE_ITEM ";
            sql += " where sid = '" + ERPWAuthentication.SID + "' and StructureCode = '" + node.StructureCode + "' and WorkGroupCode = '" + WorkGroupCode + "';";
            //sql += " update product set StructureCode = '' where sid = '" + ERPWAuthentication.SID + "' and StructureCode = '" + node.StructureCode + "';";

            var Hierarchy = ListProductCategory.Where(
                a =>
                a.NodeLevel == (node.NodeLevel + 1) &&
                a.NodeParentCode.Equals(node.StructureCode)
            ).ToList();

            foreach (var n in Hierarchy)
            {
                sql += GetSqlDeleteNode(n);
            }

            return sql;
        }

        #endregion

        private CompanyStructureModel GetNewNodeStructure(string ParentNodeCode)
        {
            var NewParentNode = ListProductCategory.Where(a => a.StructureCode.Equals(ParentNodeCode));
            string NewNodeHierarchyCode = "";
            string NewParentCode = "";
            int NewLevel = 0;
            if (NewParentNode.Count() > 0)
            {
                var parent = NewParentNode.First();
                if (!string.IsNullOrEmpty(parent.NodeHierarchyCode))
                {
                    NewNodeHierarchyCode += parent.NodeHierarchyCode;
                }
                if (!string.IsNullOrEmpty(NewNodeHierarchyCode))
                {
                    NewNodeHierarchyCode += "|";
                }
                NewNodeHierarchyCode += parent.StructureCode;
                NewParentCode = parent.StructureCode;
                NewLevel = parent.NodeLevel + 1;
            }

            return new CompanyStructureModel
            {
                NodeHierarchyCode = NewNodeHierarchyCode,
                NodeParentCode = NewParentCode,
                NodeLevel = NewLevel
            };
        }
        private int GetNextNodeLavel(string parentNodeID)
        {
            if (string.IsNullOrEmpty(parentNodeID))
            {
                return 0;
            }
            else
            {
                return ListProductCategory.Where(a => a.StructureCode.Equals(parentNodeID)).First().NodeLevel + 1;
            }
        }

        public string GetGenerateNextID(string TableName, string ColumnName, string Prefix, int TotalLength)
        {
            return GetGenerateNextIDExecute(TableName, ColumnName, Prefix, TotalLength, "");
        }

        private string GetGenerateNextIDExecute(string TableName, string ColumnName, string Prefix, int TotalLength, string WhereCause)
        {
            string sql = "";
            sql += "SELECT TOP 1 " + ColumnName + " as Value ";
            sql += " from " + TableName + " where SID = '" + ERPWAuthentication.SID + "' " + WhereCause + " and WorkGroupCode = '" + WorkGroupCode + "' order by " + ColumnName + " desc";

            int SuffixPlace = TotalLength - Prefix.Length;
            int Next;
            try
            {
                string getTop = db.selectDataFocusone(sql).Rows[0]["Value"].ToString();
                Next = Convert.ToInt32(getTop.Substring(Prefix.Length, SuffixPlace)) + 1;
            }
            catch
            {
                Next = 1;
            }
            return Prefix + Next.ToString().PadLeft(SuffixPlace, '0');
        }

    }
}