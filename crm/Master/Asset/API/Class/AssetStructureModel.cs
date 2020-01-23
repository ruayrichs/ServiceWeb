using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Asset.API.Class
{
    public class AssetStructureModel
    {
        DBService dbService = new DBService();
        public static List<AssetStructureModel.EnAssetStructureModel> GetProductCategory(string SID, string CompanyCode)
        {
            string sql = @"select struct.* , asset.AssetSubCodeDescription as AssetName
                            from ERPW_Structure_Asset_Item struct
                            left join am_master_asset_subcode asset
                              on asset.SID = struct.SID
                              AND asset.CompanyCode = struct.CompanyCode 
                              AND asset.AssetCode = struct.AssetCode
                              AND asset.AssetSubCode = struct.AssetSubCode
                            where struct.SID = '" + SID + @"' 
                              AND struct.CompanyCode = '" + CompanyCode + @"'";

            DataTable dt = new DBService().selectDataFocusone(sql);
            return JsonConvert.DeserializeObject<List<AssetStructureModel.EnAssetStructureModel>>(JsonConvert.SerializeObject(dt));
        }

        public void AddAssetStructure(string SID, string CompanyCode, string AssetCode, string AssetSubCode
            , string NodeHierarchyCode, string NodeParentCode, int NodeLevel, string Description, string created_by)
        {
            string sql = @"insert into ERPW_Structure_Asset_Item (
                               SID
                              ,CompanyCode
                              ,StructureCode
                              ,AssetCode 
                              ,AssetSubCode
                              ,NodeHierarchyCode
                              ,NodeParentCode
                              ,NodeLevel
                              ,[Description]
                              ,created_by
                              ,created_on
                            ) VALUES (
                               '" + SID + @"'
                              ,'" + CompanyCode + @"'
                              ,'" + AssetCode + AssetSubCode + @"'
                              ,'" + AssetCode + @"'
                              ,'" + AssetSubCode + @"'
                              ,'" + NodeHierarchyCode + @"'
                              ,'" + NodeParentCode + @"'
                              ," + NodeLevel + @"
                              ,'" + Description + @"'
                              ,'" + created_by + @"'
                              ,'" + Validation.getCurrentServerStringDateTime() + @"'
                            )";
            dbService.executeSQLForFocusone(sql);
        }

        public DataTable GetAssetRelation(string SID, string CompanyCode, string StructureCode)
        {
            string sql = @"select struct.SID, struct.CompanyCode, struct.StructureCode
                            , struct.AssetCode, asset.AssetSubCodeDescription as AssetName
                            , struct.AssetSubCode, struct.NodeHierarchyCode, struct.NodeParentCode
                            , struct.NodeLevel, struct.[Description]
                              , case 
                                when StructureCode = '" + StructureCode + @"' then 'This'
                                when NodeParentCode = '" + StructureCode + @"' then 'Child'
                                else 'Parent'
                                end Relation

                            from ERPW_Structure_Asset_Item struct
                            left join am_master_asset_subcode asset
                              on asset.SID = struct.SID
                              AND asset.CompanyCode = struct.CompanyCode 
                              AND asset.AssetCode = struct.AssetCode
                              AND asset.AssetSubCode = struct.AssetSubCode
                            where struct.SID = '" + SID + @"' 
                              AND struct.CompanyCode = '" + CompanyCode + @"' 
                              AND 
                              (
                                struct.StructureCode = '" + StructureCode + @"' 
                                or struct.NodeParentCode = '" + StructureCode + @"'
                                or struct.StructureCode in 
                                (
                                  select NodeParentCode
                                  from ERPW_Structure_Asset_Item
                                  where SID = '" + SID + @"' 
                                    AND CompanyCode = '" + CompanyCode + @"' 
                                    AND StructureCode = '" + StructureCode + @"'
                                )
                              )
                            order by NodeLevel asc";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getListDataAsset(string SID, string CompanyCode)
        {
            string sql = @"select c.AssetGroup,e.Description as AssetGroupName,c.AssetType
                              ,f.GroupName as AssetTypeName,a.AssetCode,b.AssetSubCode
                              ,b.AssetSubCodeDescription,b.NetValue,c.CURRENCYCODE
                              ,d.AssetOwner,g.FirstName_TH,g.LastName_TH,a.AssetStatus
                            from am_master_asset_header a
                            inner join am_master_asset_subcode b
                              on a.SID = b.SID 
                              and a.CompanyCode = b.CompanyCode 
                              and a.AssetCode = b.AssetCode
                            inner join am_master_asset_general1 c
                              on b.SID = c.SID 
                              and b.CompanyCode = c.CompanyCode 
                              and b.AssetCode = c.AssetCode 
                              and b.AssetSubCode = c.AssetSubCode
                            inner join am_master_asset_general2 d
                              on b.SID = d.SID 
                              and b.CompanyCode = d.CompanyCode 
                              and b.AssetCode = d.AssetCode 
                              and b.AssetSubCode = d.AssetSubCode
                            left join am_define_assetgroup e
                              on c.SID = e.SID 
                              and c.CompanyCode = e.CompanyCode 
                              and c.AssetGroup = e.AssetGroup
                            left join am_define_assettype f
                              on c.SID = f.SID 
                              and c.CompanyCode = f.CompanyCode 
                              and c.AssetGroup = f.AssetGroup 
                              and c.AssetType = f.GroupCode
                            left join master_employee g
                              on d.SID = g.SID 
                              and d.CompanyCode = g.CompanyCode 
                              and d.AssetOwner = g.EmployeeCode
                            where a.SID='" + SID + @"' 
                              and a.CompanyCode='" + CompanyCode + @"'";

            return dbService.selectDataFocusone(sql);
        }

        public class EnAssetStructureModel { 
            public string SID { get; set; }
            public string CompanyCode { get; set; }
            public string StructureCode { get; set; }
            public string AssetCode { get; set; }
            public string AssetSubCode { get; set; }
            public string AssetName { get; set; }
            public string NodeHierarchyCode { get; set; }
            public string NodeParentCode { get; set; }
            public int NodeLevel { get; set; }
            public string Description { get; set; }
            public string created_by { get; set; }
            public string created_on { get; set; }
            public string updated_by { get; set; }
            public string updated_on { get; set; }
            public int CountItem { get; set; }
        }
    }
}