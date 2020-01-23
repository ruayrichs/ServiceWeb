﻿using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service;

namespace ServiceWeb.API.ServiceTicketAPI
{
    public partial class ReferFromTicketAPI : System.Web.UI.Page
    {
        DBService db = new DBService();

        private string CustomerCode
        {
            get
            {
                return Request["CustomerCode"];
            }
        }
        private string TicketNo
        {
            get
            {
                return Request["TicketNo"];
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
            public string status { get; set; }
            public string iscloseticket { get; set; }
            public string fiscalyear { get; set; }
            public string doctype { get; set; }
            public List<ProductHierarchyManagerEntity> tree { get; set; }
        }

        private List<ServiceTicketStructureModel> _ListProductCategory;
        private List<ServiceTicketStructureModel> ListProductCategory
        {
            get
            {
                if (_ListProductCategory == null)
                {
                    string businessObject = ServiceTicketLibrary.GetInstance().GetBusinessObjectFromTicketType(ERPWAuthentication.SID, Request["TicketType"]);

                    if (businessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM)
                    {
                        _ListProductCategory = ServiceTicketStructureModel.GetTicketStructure(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, TicketNo, null);
                    }
                    else
                    {
                        _ListProductCategory = ServiceTicketStructureModel.GetTicketStructure(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, TicketNo, CustomerCode);
                    }
                }
                return _ListProductCategory;
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
                    tree = innerList,
                    status = h.CallStatus,
                    iscloseticket = h.IsCloseTicket.ToString().ToLower(),
                    fiscalyear = h.Fiscalyear,
                    doctype = h.Doctype
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
                obj.Add("status", List.status);
                obj.Add("iscloseticket", List.iscloseticket);
                obj.Add("fiscalyear", List.fiscalyear);
                obj.Add("doctype", List.doctype);
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

        private string getStrSqlMoveNode(ServiceTicketStructureModel node, string NewParentNodeID, int NextLevel)
        {
            var nodeStruc = GetNewNodeStructure(NewParentNodeID);
            string NewNodeHierarchyCode = nodeStruc.NodeHierarchyCode;
            string NewParentCode = nodeStruc.NodeParentCode;


            string sql = "update ERPW_REFERENT_FROM_TICKET_STRUCTURE_ITEM set NodeHierarchyCode = '" + NewNodeHierarchyCode + "'";
            sql += ",NodeParentCode = '" + NewParentCode + "'";
            sql += ",NodeLevel = '" + NextLevel + "'";
            sql += " where sid = '" + node.SID + "' and StructureCode = '" + node.StructureCode + "' and CustomerCode = '" + CustomerCode + "';";

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
            string sql = "update ERPW_REFERENT_FROM_TICKET_STRUCTURE_ITEM set StructureName = '" + newName + "'";
            sql += " where sid = '" + ERPWAuthentication.SID + "' and StructureCode = '" + id + "' and CustomerCode = '" + CustomerCode + "';";
            db.executeSQLForFocusone(sql);
        }

        private string NewFolder()
        {
            string parentID = Request["parentid"];
            string name = Request["name"];
            string tk_ref = Request["tk_ref"];
            var list_ref = ListProductCategory.Where(w => w.TicketCode.Equals(tk_ref)).ToList();
            if (list_ref.Count > 0)
            {
               parentID = list_ref.First().StructureCode;
            }

            string result = "";

            var nodeStruc = GetNewNodeStructure(parentID);

            string sql = @"insert into ERPW_REFERENT_FROM_TICKET_STRUCTURE_ITEM 
                    (SID,CustomerCode,StructureCode,StructureName,NodeHierarchyCode,NodeParentCode,NodeLevel,Description,created_on,created_by) values(";
            sql += "'" + ERPWAuthentication.SID + "'";
            sql += ",'" + CustomerCode + "'";
            sql += ",'" + GetGenerateNextID("ERPW_REFERENT_FROM_TICKET_STRUCTURE_ITEM", "StructureCode", "CT", 10) + "'";
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

            return result;
        }

        private void DeleteFolder()
        {
            string id = Request["id"];
            var CurrentNode = ListProductCategory.Where(a => a.StructureCode.Equals(id)).First();
            db.executeSQLForFocusone(GetSqlDeleteNode(CurrentNode));
            //accountService.DeleteParticipantsByCompanyStructureCode(
            //    ERPWAuthentication.SID,
            //    ERPWAuthentication.CompanyCode,
            //    id,
            //    CustomerCode
            //);
        }
        private string GetSqlDeleteNode(ServiceTicketStructureModel node)
        {
            string sql = "delete from ERPW_REFERENT_FROM_TICKET_STRUCTURE_ITEM ";
            sql += " where sid = '" + ERPWAuthentication.SID + "' and StructureCode = '" + node.StructureCode + "' and CustomerCode = '" + CustomerCode + "';";

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

        private ServiceTicketStructureModel GetNewNodeStructure(string ParentNodeCode)
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

            return new ServiceTicketStructureModel
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
            sql += " from " + TableName + " where SID = '" + ERPWAuthentication.SID + "' " + WhereCause + " and CustomerCode = '" + CustomerCode + "' order by " + ColumnName + " desc";

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