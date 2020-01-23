using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Asset.API.Class;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Asset.API
{
    public partial class AssetStructureAPI : System.Web.UI.Page
    {
        DBService db = new DBService();
        AssetStructureModel assetStructureService = new AssetStructureModel();

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
            public bool IsNewItem { get; set; }
            public List<ProductHierarchyManagerEntity> tree { get; set; }
        }

        private List<AssetStructureModel.EnAssetStructureModel> _ListProductCategory = null;
        private List<AssetStructureModel.EnAssetStructureModel> ListProductCategory
        {
            get
            {
                return _ListProductCategory == null
                    ? AssetStructureModel.GetProductCategory(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode)
                    : _ListProductCategory;
            }
        }

        private string CodeNewItem { get; set; }

        private string GetHierarchy()
        {
            CodeNewItem = ListProductCategory.Where(w =>
                w.created_on.Equals(ListProductCategory.Max(m => m.created_on))
            ).First().StructureCode;

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
            ).OrderBy(o => o.AssetName).ToList();

            List<ProductHierarchyManagerEntity> RootHierarchy = new List<ProductHierarchyManagerEntity>();
            foreach (var h in Hierarchy)
            {
                List<ProductHierarchyManagerEntity> innerList = ManageHierarchyManagerAllProject((int)h.NodeLevel + 1, h.StructureCode);

                int CountItem = h.CountItem + innerList.Sum(a => Convert.ToInt32(a.countdata));
                ProductHierarchyManagerEntity pm = new ProductHierarchyManagerEntity
                {
                    id = h.StructureCode,
                    name = h.AssetName,
                    type = "f",
                    countdata = CountItem.ToString(),
                    tree = innerList,
                    IsNewItem = h.StructureCode.Equals(CodeNewItem)
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
                obj.Add("IsNewItem", List.IsNewItem);
                JArray InnerTree = List.tree.Count() == 0 ? new JArray() : ManageHierarchyManagerJSON(List.tree);
                obj.Add("tree", InnerTree);
                returnJSON.Add(obj);
            }
            return returnJSON;
        }
        #endregion

        #region Rename | New Folder | Delete Folder
        private void DeleteFolder()
        {
            string id = Request["id"];
            var CurrentNode = ListProductCategory.Where(a => a.StructureCode.Equals(id)).First();
            db.executeSQLForFocusone(GetSqlDeleteNode(CurrentNode));
        }

        private string GetSqlDeleteNode(AssetStructureModel.EnAssetStructureModel node)
        {
            string sql = @"delete from Link_Structure_Asset_Item
                            where SID = '" + ERPWAuthentication.SID + @"' 
                            and CompanyCode = '" + ERPWAuthentication.CompanyCode + @"' 
                            and StructureCode = '" + node.StructureCode + "';";

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
    }
}