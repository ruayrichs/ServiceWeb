using Agape.Lib.DBService;
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
using ERPW.Lib.Service.Workflow.Entity;

namespace ServiceWeb.Accountability.API
{
    public partial class HirearchyStructureAPI : System.Web.UI.Page
    {
        private static DBService dbService = new DBService();
        HierarchyService hierarchyservice = HierarchyService.getInstance();

        private List<HierarchyStructureModel> HierarchyList
        {
            get;
            set;
        }
        private string hierarchyType
        {
            get;
            set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            hierarchyType = Request["hierarchyType"];
            string RequestType = Request["request"];
            string response = "";
            try
            {
                if (!string.IsNullOrEmpty(RequestType))
                {
                    switch (RequestType)
                    {
                        case "list":
                            response = GetListHierarchyManager();
                            break;

                        case "userlist":
                            //response = GetListHierarchyManagerByUser(Request["username"]);
                            break;

                        case "newfolder":
                            response = NewFolder();
                            break;

                        case "rename":
                            Rename();
                            break;

                        case "deletefolder":
                            DeleteFolder();
                            break;

                        case "movenode":
                            MoveNode();
                            break;

                        case "sharevariant":
                            //ShareHierarchy();
                            break;
                    }
                }
            }
            catch
            {

            }
            Response.Write(response);
        }

        private void DeleteFolder()
        {
            string id = Request["id"];
            hierarchyservice.deleteMatHierachyCatalog(ERPWAuthentication.SID, hierarchyType, id);
        }
        private void MoveNode()
        {
            string id = Request["itemNode"];
            string newParent = Request["newParentNode"];
            hierarchyservice.moveMatHierachyCatalog(ERPWAuthentication.SID, hierarchyType, id, newParent, ERPWAuthentication.UserName);
        }
        private void Rename()
        {
            string id = Request["id"];
            string name = Request["name"];
            hierarchyservice.renameMatHierachyCatalog(ERPWAuthentication.SID, hierarchyType, id, name, ERPWAuthentication.UserName);
        }
        private string NewFolder()
        {
            string folderName = Request["folderName"];
            string folderParent = Request["folderParent"];
            string hierarchyGroup = Request["hierarchyGroup"];
            JObject obj = new JObject();

            bool status = chackLevel(folderParent, hierarchyGroup);
            if (status)
            {
                hierarchyservice.insertMatHierachyCatalog(ERPWAuthentication.SID, hierarchyGroup, hierarchyType, folderName, folderParent, ERPWAuthentication.UserName);
                obj.Add("status", "");
            }
            else
            {
                obj.Add("status", "กรุณาเพิ่ม Possible Entry ก่อน");
            }
            return obj.ToString();
        }
        private bool chackLevel(string folderParent, string hierarchyGroup)
        {
            bool result = false;

            string sqlGetParent = @"select * from ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY 
                where ObjectID = '" + folderParent + @"'
                AND HierarchyType = '" + hierarchyType + @"'
                AND HierarchyGroup = '" + hierarchyGroup + "'";


            string sqlGetMaxLevel = @"select * 
                from ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY_POSSIBLE_ENTRY 
                where SID = '" + ERPWAuthentication.SID + @"'
                AND CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'
                AND HierarchyTypeCode = '" + hierarchyType + @"'
                order by NodeLevel asc";
            DataTable dt = dbService.selectDataFocusone(sqlGetParent);
            int ParentLevel = 0;
            if (dt.Rows.Count > 0)
            {
                ParentLevel = Convert.ToInt32(dt.Rows[0]["xLevel"].ToString());
            }
            int MaxLevel = dbService.selectDataFocusone(sqlGetMaxLevel).Rows.Count;

            if (ParentLevel < MaxLevel)
            {
                result = true;
            }

            return result;
        }
        private string GetListHierarchyManager()
        {
            try
            {
                HierarchyList = getMatHierachyCatalogEntity(ERPWAuthentication.SID, hierarchyType);
                List<HierarchyManagerEntity> RootHierarchy = ManageHierarchyManager(0, "");

                JArray returnJSON = ManageHierarchyManagerJSON(RootHierarchy);
                return returnJSON.ToString();
            }
            catch
            {
                return "[]";
            }
        }

        private List<HierarchyManagerEntity> ManageHierarchyManager(int NodeLeval, string ParentNodeID)
        {
            var Hierarchy = HierarchyList.Where(
                a =>
                a.NodeLevel == NodeLeval &&
                a.NodeParentID.Equals(ParentNodeID)
            );

            List<HierarchyManagerEntity> RootHierarchy = new List<HierarchyManagerEntity>();
            foreach (var h in Hierarchy)
            {
                RootHierarchy.Add(new HierarchyManagerEntity
                {
                    id = h.NodeID,
                    name = h.NodeName,
                    type = "f",
                    countdata = h.CountData,
                    tree = ManageHierarchyManager((int)h.NodeLevel + 1, h.NodeID)
                });
            }
            return RootHierarchy.OrderBy(o => o.type).ThenBy(t => t.name).ToList();
        }
        private JArray ManageHierarchyManagerJSON(List<HierarchyManagerEntity> inputList)
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

        public static List<HierarchyStructureModel> getMatHierachyCatalogEntity(string sid, string p_hierarchyType)
        {
            DataTable dt = getMatHierachyCatalog(sid, p_hierarchyType, "");

            List<HierarchyStructureModel> ListResult = new List<HierarchyStructureModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ListResult.Add(new HierarchyStructureModel
                {
                    NodeID = dr["ObjectID"].ToString(),
                    NodeParentID = dr["HierarchyParent"].ToString(),
                    NodeLevel = Convert.ToInt32(dr["xLevel"].ToString()),
                    NodeName = dr["HierarchyDesc"].ToString(),
                    HierarchyCode = dr["HierarchyCode"].ToString(),
                    HierarchyType = dr["HierarchyType"].ToString(),
                    CountData = dr["xCount"].ToString()
                });
            }

            return ListResult.ToList();
        }

        private static DataTable getMatHierachyCatalog(string sid, string p_hierarchyType, string HierarchiObjectID)
        {
            string sql = @"select a.*, isNull(b.xCount,0) xCount from ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY  a
                left outer join 
                (
                select count(objectid) as xCount,PrimaryHierarchyCode,sid from km_header
                group by PrimaryHierarchyCode,sid 
                ) b
                on a.Sid = b.SID and a.ObjectID = b.PrimaryHierarchyCode where a.sid='" + sid + "'";

            //string sql = "select * from ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY where sid='" + sid + "' ";
            if (p_hierarchyType != "")
            {
                sql += " and a.HierarchyType='" + p_hierarchyType + "' ";
            }
            if (HierarchiObjectID != "")
            {
                sql += " and a.ObjectID='" + HierarchiObjectID + "' ";
            }

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public class HierarchyManagerEntity
        {
            public string type { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public int index { get; set; }
            public string countdata { get; set; }

            public List<HierarchyManagerEntity> tree { get; set; }
        }
    }
}