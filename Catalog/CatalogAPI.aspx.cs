using Newtonsoft.Json.Linq;
using ERPW.Lib.Authentication;
using SNA.Lib.Catalog;
using SNA.Lib.Catalog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Catalog
{
    public partial class CatalogAPI : System.Web.UI.Page
    {
        private List<CatalogHierarchyEntity> HierarchyList
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
                            NewFolder();
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
            CatalogService.deleteMatHierachyCatalog(ERPWAuthentication.SID, hierarchyType, id);
        }
        private void MoveNode()
        {
            string id = Request["itemNode"];
            string newParent = Request["newParentNode"];
            CatalogService.moveMatHierachyCatalog(ERPWAuthentication.SID, hierarchyType, id, newParent, ERPWAuthentication.UserName);
        }
        private void Rename()
        {
            string id = Request["id"];
            string name = Request["name"];
            CatalogService.renameMatHierachyCatalog(ERPWAuthentication.SID, hierarchyType, id, name, ERPWAuthentication.UserName);
        }
        private void NewFolder()
        {
            string folderName = Request["folderName"];
            string folderParent = Request["folderParent"];
            string hierarchyGroup = Request["hierarchyGroup"];
            CatalogService.insertMatHierachyCatalog(ERPWAuthentication.SID, hierarchyGroup, hierarchyType, folderName, folderParent, ERPWAuthentication.UserName);
        }

        private string GetListHierarchyManager()
        {
            try
            {
                HierarchyList = CatalogService.getMatHierachyCatalogEntity(ERPWAuthentication.SID, hierarchyType);
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