using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using SNA.Lib.Catalog.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNA.Lib.Catalog
{
    public class CatalogService
    {
        private static DBService dbService = new DBService();

        #region Hierarchy
        private static List<CatalogHierarchyEntity> HierarchyList
        {
            get;
            set;
        }
        private static DataTable getMatHierachyCatalog(string sid, string p_hierarchyType,string HierarchiObjectID)
        {
            string sql = @"select a.*, isNull(b.xCount,0) xCount from ep_master_conf_hierarchy  a
left outer join 
(
select count(objectid) as xCount,PrimaryHierarchyCode,sid from km_header
group by PrimaryHierarchyCode,sid 
) b
on a.Sid = b.SID and a.ObjectID = b.PrimaryHierarchyCode where a.sid='" + sid + "'";

            //string sql = "select * from ep_master_conf_hierarchy where sid='" + sid + "' ";
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
        private static string getStrSqlMatHierachyCatalogEntity(CatalogHierarchyEntity node, string sid, string hierarchyType)
        {
            string sql = "delete from ep_master_conf_hierarchy";
            sql += " where sid = '" + sid + "' and HierarchyType='" + hierarchyType + "' and ObjectID = '" + node.NodeID + "';";

            var Hierarchy = HierarchyList.Where(
                a =>
                a.NodeLevel == (node.NodeLevel + 1) &&
                a.NodeParentID.Equals(node.NodeID)
            );

            foreach (var n in Hierarchy)
            {
                sql += getStrSqlMatHierachyCatalogEntity(n, sid, hierarchyType);
            }
            return sql;
        }
        private static string getStrSqlMoveMatHierachyCatalog(CatalogHierarchyEntity node, int NodeParentLevel, string NewParentID, string sid, string hierarchyType)
        {
            string NewObjectID = NewParentID;
            NewObjectID += string.IsNullOrEmpty(NewObjectID) ? node.HierarchyCode : ("|" + node.HierarchyCode);

            string sql = "update ep_master_conf_hierarchy set ObjectID = '" + NewObjectID + "'";
            sql += ",HierarchyParent = '" + NewParentID + "'";
            sql += ",xLevel = '" + NodeParentLevel + "'";
            sql += " where sid = '" + sid + "' and HierarchyType='" + hierarchyType + "' and ObjectID = '" + node.NodeID + "';";

            var Hierarchy = HierarchyList.Where(
                a =>
                a.NodeLevel == (node.NodeLevel + 1) &&
                a.NodeParentID.Equals(node.NodeID)
            );

            foreach (var n in Hierarchy)
            {
                sql += getStrSqlMoveMatHierachyCatalog(n, NodeParentLevel + 1, NewObjectID, sid, hierarchyType);
            }

            return sql;
        }
        public static void renameMatHierachyCatalog(string sid, string hierarchyType, string NodeID, string NodeName, string UpdateBy)
        {
            string sql = "update ep_master_conf_hierarchy set HierarchyDesc = '" + NodeName + "'";
            sql += ",Updated_By = '" + UpdateBy + "'";
            sql += ",Updated_On = '" + Validation.getCurrentServerStringDateTime() + "'";
            sql += "where sid = '" + sid + "' and HierarchyType='" + hierarchyType + "' and ObjectID = '" + NodeID + "'";
            dbService.executeSQLForFocusone(sql);
        }
        public static void deleteMatHierachyCatalog(string sid, string hierarchyType, string NodeID)
        {
            HierarchyList = getMatHierachyCatalogEntity(sid, hierarchyType);
            var CurrentNode = HierarchyList.Where(a => a.NodeID.Equals(NodeID)).First();
            string sql = getStrSqlMatHierachyCatalogEntity(CurrentNode, sid, hierarchyType);
            dbService.executeSQLForFocusone(sql);
        }
        public static void moveMatHierachyCatalog(string sid, string hierarchyType, string NodeID, string NewParentNodeID, string UpdateBy)
        {
            HierarchyList = getMatHierachyCatalogEntity(sid, hierarchyType);
            var CurrentNode = HierarchyList.Where(a => a.NodeID.Equals(NodeID)).First();
            int parentLevel = GetNextNodeLavel(NewParentNodeID, HierarchyList);
            string sql = getStrSqlMoveMatHierachyCatalog(CurrentNode, parentLevel, NewParentNodeID, sid, hierarchyType);
            dbService.executeSQLForFocusone(sql);
        }
        public static void insertMatHierachyCatalog(string sid, string hierarchyGroup, string hierarchyType, string NodeName, string parentNodeID, string CreatedBy)
        {
            string GenerateID = Validation.getCurrentServerStringDateTimeMillisecond();
            string ObjectID = parentNodeID;
            ObjectID += string.IsNullOrEmpty(ObjectID) ? GenerateID : ("|" + GenerateID);

            string sql = @"insert into ep_master_conf_hierarchy 
            (
                [Sid]
                ,[ObjectID]
                ,[HierarchyType]
                ,[HierarchyGroup]
                ,[HierarchyCode]
                ,[HierarchyDesc]
                ,[xLevel]
                ,[HierarchyParent]
                ,[Created_By]
                ,[Created_On]
            ) values(";

            sql += "'" + sid + "',";
            sql += "'" + ObjectID + "',";
            sql += "'" + hierarchyType + "',";
            sql += "'" + hierarchyGroup + "',";
            sql += "'" + GenerateID + "',";
            sql += "'" + NodeName + "',";
            sql += "'" + GetNextNodeLavel(parentNodeID, getMatHierachyCatalogEntity(sid, hierarchyType)) + "',";
            sql += "'" + parentNodeID + "',";
            sql += "'" + CreatedBy + "',";
            sql += "'" + Validation.getCurrentServerStringDateTime() + "'";
            sql += ");";

            dbService.executeSQLForFocusone(sql);
        }
        public static string GetHierarchyCode(string sid, string p_hierarchyType, string NodeID)
        {
            try
            {
                return getMatHierachyCatalogEntity(sid, p_hierarchyType).Where(a => a.NodeID.Equals(NodeID)).First().HierarchyCode;
            }
            catch
            {
                return "";
            }
        }
        public static int GetNextNodeLavel(string parentNodeID, List<CatalogHierarchyEntity> HierarchyList)
        {
            if (string.IsNullOrEmpty(parentNodeID))
            {
                return 0;
            }
            else
            {
                return HierarchyList.Where(a => a.NodeID.Equals(parentNodeID)).First().NodeLevel + 1;
            }
        }
        public static List<CatalogHierarchyEntity> getMatHierachyCatalogEntity(string sid, string p_hierarchyType)
        {
            DataTable dt = getMatHierachyCatalog(sid, p_hierarchyType,"");

            List<CatalogHierarchyEntity> ListResult = new List<CatalogHierarchyEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                ListResult.Add(new CatalogHierarchyEntity
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
        public static CatalogHierarchyEntity getMatHierachyCatalogEntity(string sid, string p_hierarchyType,string HierarchyObjectID)
        {
            DataTable dt = getMatHierachyCatalog(sid, p_hierarchyType, HierarchyObjectID);

            foreach (DataRow dr in dt.Rows)
            {
                CatalogHierarchyEntity Result = new CatalogHierarchyEntity
                {
                    NodeID = dr["ObjectID"].ToString(),
                    NodeParentID = dr["HierarchyParent"].ToString(),
                    NodeLevel = Convert.ToInt32(dr["xLevel"].ToString()),
                    NodeName = dr["HierarchyDesc"].ToString(),
                    HierarchyCode = dr["HierarchyCode"].ToString(),
                    HierarchyType = dr["HierarchyType"].ToString()
                };
                return Result;
            }

            return new CatalogHierarchyEntity();
        }
        public static List<string> GetHierarchyNameList(string sid, string p_hierarchyType, string HierarchyObjectCode)
        {
            List<string> strList = new List<string>();
            string[] strArr = HierarchyObjectCode.Split('|');
            HierarchyList = getMatHierachyCatalogEntity(sid, p_hierarchyType);
            foreach (var str in strArr)
            {
                try
                {
                    strList.Add(HierarchyList.Where(a => a.HierarchyCode.Equals(str)).First().NodeName);
                }
                catch { }
            }

            return strList;
        }
        public static DataTable GetHierarchyType(string p_hierarchygroup)
        {
            string sql = "select b.HIERARCHYTYPECODE,b.HIERARCHYTYPENAME from [SYS_HIERARCHYGROUP_MASTER] as a " +
                "inner join [SYS_HIERARCHYTYPE_MASTER] as b on a.HIERARCHYGROUPCODE = b.HIERARCHYGROUPCODE";

            if (!string.IsNullOrEmpty(p_hierarchygroup))
                sql += " where b.HIERARCHYGROUPCODE = '" + p_hierarchygroup + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        #endregion

        #region Material
        public static DataTable GetListCatalog(string p_sid, string p_catalog, string p_matcode, string p_matdesc, string p_matgroup, string p_hierarchy_type, string p_hierarchy, string p_pricetable, string p_company_code)
        {
            DataTable catalog = new DataTable();

            string xDate = Validation.Convert2DateDB(Validation.getCurrentServerDate());
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select itm.SID, itm.ItmNumber, itm.ItmDescription, itm.DescForeign, pic.PictureFile ");
            sb.AppendLine(",itm.ItmGroup,itm.MatHierarchy, itm_cate.HierarchyType ");
            sb.AppendLine(",0 as amount,itm_detail.NameCatalog,itm_detail.SubNameCatalog,itm_detail.Language");
            sb.AppendLine(",itm_detail.MarketPrice,itm_detail.Currency,itm_detail.UOM");
            if (p_pricetable != "")
            {
                sb.AppendLine(",ISNULL(price.amount,0) as amount, price.CurrencyCode,price.UOM,uom.UDESC ");
            }
            else
            {
                sb.AppendLine(",0 as amount,'THB' as CurrencyCode,'' as UOM,'' as UDESC");
            }
            sb.AppendLine("from master_mm_items itm left outer join ");
            sb.AppendLine("(select SID, ItemNumber, PictureFile from master_mm_item_picture where SID='" + p_sid + "') pic ");
            sb.AppendLine("on itm.SID = pic.SID and itm.ItmNumber = pic.ItemNumber ");
            if (p_pricetable != "")
            {
                sb.AppendLine("left outer join " + p_pricetable + " price ");
                sb.AppendLine("on itm.SID = price.SID and itm.ItmNumber = price.MeterialCode ");
                sb.AppendLine("and (price.validfrom <= '" + xDate + "' and price.validto >= '" + xDate + "') ");
                sb.AppendLine("left outer join master_mm_weight_setup uom ");
                sb.AppendLine("on price.SID = uom.SID and price.UOM = uom.UCODE ");
            }
            sb.AppendLine("left outer join master_mm_items_catalog itm_cate ");
            sb.AppendLine("on itm.SID = itm_cate.SID and itm.ItmNumber = itm_cate.ItemNumber ");
            //if (p_company_code != "")
            //{
            //    sb.AppendLine("inner join master_mm_item_saledata itm_sale");
            //    sb.AppendLine("on itm.SID = itm_sale.SID and itm.ItmNumber = itm_sale.ItmNumber and itm_sale.CompanyCode = '" + p_company_code + "'");
            //}

            sb.AppendLine("join ep_master_conf_hierarchy hry on itm_cate.SID = hry.Sid and itm_cate.HierarchyCode = hry.HierarchyCode");
            sb.AppendLine("join master_mm_items_catalog_detail itm_detail on itm_detail.sid = itm_cate.SID and itm_cate.ItemNumber = itm_detail.ItemNumber and itm_cate.LineItem = itm_detail.LineItem");

            sb.AppendLine("where itm.SID='" + p_sid + "' ");
            if (!string.IsNullOrEmpty(p_matcode)) sb.AppendLine(" and itm.ItmNumber='" + p_matcode + "' ");
            if (!string.IsNullOrEmpty(p_matdesc)) sb.AppendLine(" and itm.ItmDescription like '%" + p_matdesc + "%' ");
            if (!string.IsNullOrEmpty(p_matgroup)) sb.AppendLine(" and itm.ItmGroup='" + p_matgroup + "' ");
            if (!string.IsNullOrEmpty(p_hierarchy_type)) sb.AppendLine(" and itm_cate.HierarchyType='" + p_hierarchy_type + "' ");
            if (!string.IsNullOrEmpty(p_hierarchy)) sb.AppendLine(" and hry.ObjectID='" + p_hierarchy + "' ");
            catalog = dbService.selectDataFocusone(sb.ToString());

            catalog.Columns.Add("ImageByte", typeof(byte[]));

            foreach (DataRow dr in catalog.Rows)
            {
                dr["ImageByte"] = new byte[] { };
            }
            return catalog;
        }

        public static List<CatalogEntity> GetListCatalogEntity(string p_sid, string p_catalog, string p_matcode, string p_matdesc, string p_matgroup, string p_hierarchy_type, string p_hierarchy, string p_pricetable, string p_company_code)
        {
            List<CatalogEntity> ListResult = new List<CatalogEntity>();

            DataTable dt = GetListCatalog(p_sid, p_catalog, p_matcode, p_matdesc, p_matgroup, p_hierarchy_type, p_hierarchy, p_pricetable, p_company_code);

            foreach (DataRow dr in dt.Rows)
            {
                ListResult.Add(new CatalogEntity
                {
                    amount = Convert.ToInt32(dr["amount"]),
                    Currency = dr["Currency"].ToString(),
                    DescForeign = dr["DescForeign"].ToString(),
                    HierarchyType = dr["HierarchyType"].ToString(),
                    ItmDescription = dr["ItmDescription"].ToString(),
                    ItmGroup = dr["ItmGroup"].ToString(),
                    ItmNumber = dr["ItmNumber"].ToString(),
                    Language = dr["Language"].ToString(),
                    MarketPrice = Convert.ToDecimal(dr["MarketPrice"].ToString()),
                    MatHierarchy = dr["MatHierarchy"].ToString(),
                    NameCatalog = dr["NameCatalog"].ToString(),
                    PictureFile = dr["PictureFile"].ToString(),
                    SID = dr["SID"].ToString(),
                    SubNameCatalog = dr["SubNameCatalog"].ToString(),
                    UOM = dr["UOM"].ToString()
                });
            }

            return ListResult;
        }

        #endregion
    }

}
