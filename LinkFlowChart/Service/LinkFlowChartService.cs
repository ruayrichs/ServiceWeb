using Agape.Lib.DBService;
using Newtonsoft.Json;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Service;

namespace ServiceWeb.LinkFlowChart.Service
{
    public class LinkFlowChartService
    {

        #region Constant
        public static LinkFlowChartItemGroup ItemGroup_ASSET = new LinkFlowChartItemGroup("ASSET");

        public static LinkFlowChartItemGroup ItemGroup_EQUIPMENT = new LinkFlowChartItemGroup("EQUIPMENT");

        public static LinkFlowChartItemGroup ItemGroup_CLASS = new LinkFlowChartItemGroup("CLASS");

        public static LinkFlowChartItemGroup ItemGroup_TICKET = new LinkFlowChartItemGroup("TICKET");
        #endregion

        #region Entities
        public class LinkFlowChartItemGroup
        {
            public string Value = "";
            public LinkFlowChartItemGroup(string itemGroup)
            {
                Value = itemGroup;
            }
        }
        public class LinkFlowChartGetDatas
        {
            public List<LinkFlowChartPalette> Palette { get; set; }
            public List<LinkFlowChartItem> Item { get; set; }
            public List<LinkFlowChartConnector> Connector { get; set; }
        }
        public class LinkFlowChartPalette
        {
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }

            [JsonProperty(PropertyName = "code")]
            public string Code { get; set; }
        }
        public class LinkFlowChartItem
        {
            [JsonProperty(PropertyName = "key")]
            public string ItemKey { get; set; }

            [JsonProperty(PropertyName = "code")]
            public string ItemCode { get; set; }

            [JsonProperty(PropertyName = "text")]
            public string ItemText { get; set; }

            [JsonProperty(PropertyName = "category")]
            public string NodeCategory { get; set; }

            [JsonProperty(PropertyName = "loc")]
            public string Location { get; set; }
        }
        public class LinkFlowChartConnector
        {
            [JsonProperty(PropertyName = "from")]
            public string FromKey { get; set; }

            [JsonProperty(PropertyName = "to")]
            public string ToKey { get; set; }

            [JsonProperty(PropertyName = "fromPort")]
            public string FromPort { get; set; }

            [JsonProperty(PropertyName = "toPort")]
            public string ToPort { get; set; }

            [JsonProperty(PropertyName = "points")]
            public List<Double> Point { get; set; }

            [JsonProperty(PropertyName = "text")]
            public string TextDescription { get; set; }
        }

        public class LinkFlowChartRelationDetail
        {
            public string ItemGroup { get; set; }
            public string ItemKey { get; set; }
            public string ItemCode { get; set; }
            public string ParentKey { get; set; }
            public string parentCode { get; set; }
            public string RelationType { get; set; }
        }

        #endregion

        private static DBService db = new DBService();

        public static void SaveLinkFlowChart(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup, List<LinkFlowChartItem> FlowChartItem, List<LinkFlowChartConnector> FlowChartConnector)
        {
            List<string> ListSQL = new List<string>();
            ListSQL.Add("delete from LINK_FLOWCHART_ITEM where SID = '" + SID + "' and CompanyCode = '" + CompanyCode + "' and ItemGroup = '" + ItemGroup.Value + "'");
            ListSQL.Add("delete from LINK_FLOWCHART_CONNECTOR where SID = '" + SID + "' and CompanyCode = '" + CompanyCode + "' and ItemGroup = '" + ItemGroup.Value + "'");

            foreach (var item in FlowChartItem)
            {
                string SQLInsert = @"
                        INSERT INTO [dbo].[LINK_FLOWCHART_ITEM]
                                   ([SID]
                                   ,[CompanyCode]
                                   ,[ItemGroup]
                                   ,[ItemKey]
                                   ,[ItemCode]
                                   ,[Location])
                             VALUES
                                   ('" + SID + @"'
                                   ,'" + CompanyCode + @"'
                                   ,'" + ItemGroup.Value + @"'
                                   ,'" + item.ItemKey + @"'
                                   ,'" + item.ItemCode + @"'
                                   ,'" + item.Location + @"'
                             )";

                ListSQL.Add(SQLInsert);
            }

            foreach (var item in FlowChartConnector)
            {

                string points = string.Join(",",item.Point);
                string SQLInsert = @"
                        INSERT INTO [dbo].[LINK_FLOWCHART_CONNECTOR]
                                   ([SID]
                                   ,[CompanyCode]
                                   ,[ItemGroup]
                                   ,[FromKey]
                                   ,[ToKey]
                                   ,[FromPort]
                                   ,[ToPort]
                                   ,[Point]
                                   ,[RelationType]
                             )
                             VALUES
                                   ('" + SID + @"'
                                   ,'" + CompanyCode + @"'
                                   ,'" + ItemGroup.Value + @"'
                                   ,'" + item.FromKey + @"'
                                   ,'" + item.ToKey + @"'
                                   ,'" + item.FromPort + @"'
                                   ,'" + item.ToPort + @"'
                                   ,'" + points + @"'
                                   ,'" + item.TextDescription + @"'
                             )";

                ListSQL.Add(SQLInsert);
            }


            db.executeSQLForFocusone(ListSQL);
        }

        public static LinkFlowChartGetDatas GetLinkFlowChart(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup)
        {
            List<LinkFlowChartPalette> PaletteList = new List<LinkFlowChartPalette>();

            // Mode : ASSET
            if (ItemGroup_ASSET.Value.Equals(ItemGroup.Value))
            {
                //DataTable dtAsset = new AssetStructureModel().getListDataAsset(
                //    ERPWAuthentication.SID,
                //    ERPWAuthentication.CompanyCode
                //);

                //foreach (DataRow dr in dtAsset.Rows)
                //{
                //    PaletteList.Add(new LinkFlowChartPalette
                //    {
                //        Code = Convert.ToString(dr["AssetCode"]),
                //        Text = Convert.ToString(dr["AssetSubCodeDescription"])
                //    });
                //}
            }

            // Mode : EQUIPMENT
            else if (ItemGroup_EQUIPMENT.Value.Equals(ItemGroup.Value))
            {
                List<EquipmentService.EquipmentItemData> listEquipmentItem = new EquipmentService().getListEquipment(
                    SID,
                    CompanyCode,
                    "",
                    "",
                    "",
                    ""
                );

                foreach (EquipmentService.EquipmentItemData item in listEquipmentItem)
                {
                    string desc = "";

                    if (!string.IsNullOrEmpty(item.EquipmentClass))
                    {
                        desc = item.EquipmentClassName + " : " + item.Description;
                    }
                    else if (!string.IsNullOrEmpty(item.EquipmentTypeName))
                    {
                        desc = item.EquipmentTypeName + " : " + item.Description;
                    }
                    else
                    {
                        desc = item.Description;
                    }

                    PaletteList.Add(new LinkFlowChartPalette
                    {
                        Code = item.EquipmentCode,
                        Text = desc
                    });
                }
            }

            // Mode : CLASS
            else if (ItemGroup_CLASS.Value.Equals(ItemGroup.Value))
            {
                DataTable dtEquipment = new EquipmentService().getEMClass(SID);

                foreach (DataRow dr in dtEquipment.Rows)
                {
                    PaletteList.Add(new LinkFlowChartPalette
                    {
                        Code = Convert.ToString(dr["ClassCode"]),
                        Text = Convert.ToString(dr["ClassName"])
                    });
                }
            }

            List<LinkFlowChartItem> DataItemList = GetDataItemList(
                SID,
                CompanyCode,
                ItemGroup
            );
            List<LinkFlowChartConnector> ConnectorList = GetConnectorList(
                SID,
                CompanyCode,
                ItemGroup
            );

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["relationNode"]))
            {
                string relationNode = HttpContext.Current.Request["relationNode"];
                var selectedNode = DataItemList.Where(a => a.ItemCode.Equals(relationNode));
                if (selectedNode.Count() > 0)
                {
                    string nodeKey = selectedNode.First().ItemKey;
                    List<string> ListReloationKey = new List<string> { nodeKey };
                    foreach (var item in ConnectorList.Where(a => a.FromKey.Equals(nodeKey) || a.ToKey.Equals(nodeKey)))
                    {
                        ListReloationKey.Add(item.FromKey);
                        ListReloationKey.Add(item.ToKey);
                    }
                    DataItemList = DataItemList.Where(a => ListReloationKey.Contains(a.ItemKey)).ToList();
                }
                else
                {
                    var emptyItem = new LinkFlowChartItem
                    {
                        ItemKey = "-1",
                        ItemCode = relationNode,
                        ItemText = "Not in diagram",
                        NodeCategory = "active"
                    };
                    DataItemList = new List<LinkFlowChartItem> { emptyItem };
                }
            }

            PaletteList = PaletteList.Where(p => !DataItemList.Any(x => x.ItemCode.Equals(p.Code))).ToList();

            return new LinkFlowChartGetDatas
            {
                Palette = PaletteList,
                Item = DataItemList,
                Connector = ConnectorList
            };
        }

        private static List<LinkFlowChartItem> GetDataItemList(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup)
        {
            string sql = "";

            if (ItemGroup_ASSET.Value.Equals(ItemGroup.Value))
            {
                sql = @"select a.*,b.AssetSubCodeDescription as ItemText 
                from LINK_FLOWCHART_ITEM a
                inner join am_master_asset_subcode b
                on a.SID = b.SID 
                and a.CompanyCode = b.CompanyCode 
                and a.ItemCode = b.AssetCode
                where a.SID = '" + SID + "' and a.CompanyCode = '" + CompanyCode + @"' and 
                a.ItemGroup = '" + ItemGroup.Value + "'";
            }
            else if (ItemGroup_EQUIPMENT.Value.Equals(ItemGroup.Value))
            {
                sql = @"SELECT a.*
                          ,CASE ge.EquipmentClass 
                            WHEN '' THEN DocType.Description 
                            ELSE class.ClassName 
                          END + ' : ' + Equipment.[Description] AS ItemText

                        FROM LINK_FLOWCHART_ITEM a
                        INNER JOIN master_equipment Equipment ON a.SID = Equipment.SID
	                        AND a.CompanyCode = Equipment.Companycode
	                        AND a.ItemCode = Equipment.EquipmentCode
                        INNER JOIN master_config_material_doctype DocType ON Equipment.SID = DocType.SID
	                        --AND Equipment.CompanyCode = DocType.Companycode
	                        AND Equipment.EquipmentType = DocType.MaterialGroupCode
                        INNER JOIN master_equipment_general ge ON ge.sid = a.sid
	                        AND ge.companyCode = a.companycode
	                        AND a.ItemCode = ge.EquipmentCode
                        left join master_equipment_class class
                          on class.SID = a.SID
                          and class.ClassCode = ge.EquipmentClass

                        WHERE a.SID = '" + SID + "' AND a.CompanyCode = '" + CompanyCode + @"' 
                        AND a.ItemGroup = '" + ItemGroup.Value + "'";
            }
            else if (ItemGroup_CLASS.Value.Equals(ItemGroup.Value))
            {
                sql = @"select a.*,ge.ClassName as ItemText 
                from LINK_FLOWCHART_ITEM a
                inner join master_equipment_class ge
                  on ge.sid = a.sid
                  and a.ItemCode = ge.ClassCode
                where a.SID = '" + SID + @"' 
                  and a.CompanyCode = '" + CompanyCode + @"'
                  and a.ItemGroup = '" + ItemGroup.Value + "'";
            }

            DataTable dt = db.selectDataFocusone(sql);

            List<LinkFlowChartItem> Result = new List<LinkFlowChartItem>();
            foreach (DataRow dr in dt.Rows)
	        {
                Result.Add(new LinkFlowChartItem
                {
                    ItemCode = Convert.ToString(dr["ItemCode"]),
                    ItemKey = Convert.ToString(dr["ItemKey"]),
                    ItemText = Convert.ToString(dr["ItemText"]),
                    Location = Convert.ToString(dr["Location"]),
                    NodeCategory = !string.IsNullOrEmpty(HttpContext.Current.Request["relationNode"])
                                    && HttpContext.Current.Request["relationNode"].Equals(Convert.ToString(dr["ItemCode"]))
                                    ? "active" : ""
                });
	        }
            return Result;
        }
        private static List<LinkFlowChartConnector> GetConnectorList(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup)
        {
            string dataSelect = " a.* ";
            if (ItemGroup.Value == ItemGroup_CLASS.Value || ItemGroup.Value == ItemGroup_EQUIPMENT.Value)
            {
                dataSelect = @" a.SID, a.CompanyCode, a.ItemGroup, a.FromKey, a.ToKey
                              , a.FromPort, a.ToPort, a.Point
                              , a.RelationType + ' : ' + b.RelationDescript as RelationType ";
            }

            string sql = @"select " + dataSelect + @"
  
                            from LINK_FLOWCHART_CONNECTOR a
                            left join master_equipment_class_relation_config b
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.RelationType = b.RelationCode

                            where a.SID = '" + SID + @"' 
                              and a.CompanyCode = '" + CompanyCode + @"' 
                              and a.ItemGroup = '" + ItemGroup.Value + @"'";

            DataTable dt = db.selectDataFocusone(sql);
            List<LinkFlowChartConnector> Result = new List<LinkFlowChartConnector>();
            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(new LinkFlowChartConnector
                {
                    FromKey = Convert.ToString(dr["FromKey"]),
                    FromPort = Convert.ToString(dr["FromPort"]),
                    ToKey = Convert.ToString(dr["ToKey"]),
                    ToPort = Convert.ToString(dr["ToPort"]),
                    Point = Convert.ToString(dr["Point"]).Replace("[", "").Replace("]", "").Split(',').ToList().Select(double.Parse).ToList(),
                    TextDescription = Convert.ToString(dr["RelationType"])
                });
            }
            return Result;
        }

        public static List<LinkFlowChartRelationDetail> getFlowChartRelation(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup)
        {
            string condition = "";
            if (ItemGroup != null)
            {
                condition = " and a.ItemGroup = '" + ItemGroup.Value + @"' ";
            }

            string sql = @"select a.ItemGroup, a.ItemKey, a.ItemCode
                              , Isnull(b.FromKey, '') as ParentKey
                              , Isnull(c.ItemCode, '') as parentCode
                              , Isnull(b.RelationType, '') + ' : ' + Isnull(d.RelationDescript, '') as RelationType

                            from LINK_FLOWCHART_ITEM a
                            left join LINK_FLOWCHART_CONNECTOR b
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.ItemGroup = b.ItemGroup
                              and a.ItemKey = b.ToKey
                            left join LINK_FLOWCHART_ITEM c
                              on b.SID = c.SID
                              and b.CompanyCode = c.CompanyCode
                              and b.ItemGroup = c.ItemGroup
                              and b.FromKey = c.ItemKey
                            left join master_equipment_class_relation_config d
                              on a.SID = d.SID
                              and a.CompanyCode = d.CompanyCode
                              and b.RelationType = d.RelationCode

                            where a.SID = '" + SID + @"'
                              and a.CompanyCode = '" + CompanyCode + @"' " + condition;

            DataTable dt = db.selectDataFocusone(sql);
            string JsonStr = JsonConvert.SerializeObject(dt);
            List<LinkFlowChartRelationDetail> en = JsonConvert.DeserializeObject<List<LinkFlowChartRelationDetail>>(JsonStr);
            return en;
        }

        #region New Flow chart diagram
        public static DiagramRelation getDiagramRelation(string SID, string CompanyCode, string ItemCode,
            LinkFlowChartItemGroup ItemGroup, string OtherKey = null)
        {
            DiagramRelation relation = new DiagramRelation();
            List<FlowChartRelation> resultParentList = new List<FlowChartRelation>();
            List<FlowChartRelation> resultChildList = new List<FlowChartRelation>();

            string ItemDescription = getItemDescription(SID, CompanyCode, ItemCode, ItemGroup);

            //if (ItemGroup.Value == "")
            //{
            //    List<ServiceWeb.Service.EquipmentService.EquipmentItemData> equipment = new EquipmentService().getListEquipment(
            //        SID, CompanyCode, ItemCode, "", "", ""
            //    );

            //}
            List<FlowChartRelation> relationList = getDiagramRelationList(SID, CompanyCode, ItemGroup, OtherKey);

            #region Parent
            // ------------- //
            resultParentList.Add(new FlowChartRelation
            {
                ItemCode = ItemCode,
                ItemDescription = ItemDescription,
                ItemGroup = "EQUIPMENT",
                ParentItemCode = "",
                RelationCode = "",
                RelationDesc = "",
                Level = 0
            });

            List<FlowChartRelation> itemSelect = relationList.Where(w => w.ItemCode.Equals(ItemCode)).ToList();
            if (itemSelect.Count > 0)
            {
                foreach (FlowChartRelation item in itemSelect)
                {
                    resultParentList.AddRange(getParentRelation(relationList, item.ItemCode, item.ParentItemCode, 1));

                    resultParentList.Add(new FlowChartRelation
                    {
                        ItemCode = item.ParentItemCode,
                        ItemDescription = item.ParentItemDescription,
                        ItemGroup = "EQUIPMENT",
                        ParentItemCode = item.ItemCode,
                        RelationCode = item.RelationCode,
                        RelationDesc = item.RelationDesc,
                        Level = 1
                    });
                }

            }
            resultParentList = resultParentList.OrderBy(o => o.Level).ToList();
            #endregion

            #region Child
            // ------------- //
            resultChildList.Add(new FlowChartRelation
            {
                ItemCode = ItemCode,
                ItemDescription = ItemDescription,
                ItemGroup = "EQUIPMENT",
                ParentItemCode = "",
                RelationCode = "",
                Level = 0
            });
            resultChildList.AddRange(getChildRelation(relationList, ItemCode, 1));
            resultChildList = resultChildList.OrderBy(o => o.Level).ToList();
            int minLevel = resultChildList.First().Level * -1;
            resultChildList.ForEach(w =>
            {
                w.Level = w.Level + minLevel;
                if (w.Level == 0)
                {
                    w.ParentItemCode = "";
                }
            });
            relation.chindNode = resultChildList;
            #endregion

            relation.parentNode = resultParentList;
            return relation;
        }

        private static List<FlowChartRelation> getParentRelation(List<FlowChartRelation> relationList, string ItemCode, string ParentItemCode, int Level) 
        {
            List<FlowChartRelation> resultList = new List<FlowChartRelation>();
            List<FlowChartRelation> itemSelect = relationList.Where(w => w.ItemCode.Equals(ParentItemCode)).ToList();

            if (itemSelect.Count > 0)
            {
                foreach (FlowChartRelation item in itemSelect)
                {
                    List<FlowChartRelation> itemParent = relationList.Where(w => w.ItemCode.Equals(item.ParentItemCode)).ToList();
                    if (itemParent.Count > 0)
                    {
                        resultList.AddRange(getParentRelation(relationList, item.ItemCode, item.ParentItemCode, Level + 1));
                    }

                    resultList.Add(new FlowChartRelation
                    {
                        ItemCode = item.ParentItemCode,
                        ItemDescription = item.ParentItemDescription,
                        ItemGroup = "EQUIPMENT",
                        ParentItemCode = item.ItemCode,
                        RelationCode = item.RelationCode,
                        RelationDesc = item.RelationDesc,
                        Level = Level + 1
                    });
                    
                    //item.Level = Level;
                    //resultList.Add(item);
                }
            }


            return resultList;
        
        }
        private static List<FlowChartRelation> getChildRelation(List<FlowChartRelation> relationList, string ItemCode, int Level)
        {
            List<FlowChartRelation> resultList = new List<FlowChartRelation>();
            List<FlowChartRelation> itemSelect = relationList.Where(w => w.ParentItemCode.Equals(ItemCode)).ToList();

            foreach (FlowChartRelation item in itemSelect)
            {
                List<FlowChartRelation> itemChild = relationList.Where(w => w.ParentItemCode.Equals(item.ItemCode)).ToList();
                if (itemChild.Count > 0)
                {
                    resultList.AddRange(getChildRelation(relationList, item.ItemCode, Level + 1));
                }

                item.Level = Level;
                resultList.Add(item);
            }

            return resultList;
        }

        public static List<FlowChartRelation> getDiagramRelationList(string SID, string CompanyCode, 
            LinkFlowChartItemGroup ItemGroup, string OtherKey = null)
        {
            string sql = @"select a.ItemGroup, a.ItemCode, a.ItemCode as ItemDescription, a.ParentItemCode
                              , a.RelationCode, b.RelationDescript as RelationDesc
                            from ERPW_DIAGRAM_RELATION a
                            left join master_equipment_class_relation_config b
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.RelationCode = b.RelationCode
                            where a.SID = '" + SID + @"' 
                              AND a.CompanyCode = '" + CompanyCode + @"'
                              AND a.ItemGroup = '" + ItemGroup.Value + @"'";

            if (ItemGroup_EQUIPMENT.Value.Equals(ItemGroup.Value))
            {
                sql = @"select a.ItemGroup, a.ItemCode, c.[Description] as ItemDescription
                            , a.ParentItemCode, d.[Description] as ParentItemDescription
                            , a.RelationCode, b.RelationDescript as RelationDesc
                        from ERPW_DIAGRAM_RELATION a

                        left join master_equipment_class_relation_config b
                            on a.SID = b.SID
                            and a.CompanyCode = b.CompanyCode
                            and a.RelationCode = b.RelationCode

                        inner join master_equipment c
	                        on a.ItemCode = c.EquipmentCode
	                        and a.SID = c.SID
	                        and a.CompanyCode = c.CompanyCode

                        inner join master_equipment d
	                        on a.ParentItemCode = d.EquipmentCode
	                        and a.SID = d.SID
	                        and a.CompanyCode = d.CompanyCode

                        where a.SID = '" + SID + @"' 
                            AND a.CompanyCode = '" + CompanyCode + @"'
                            AND a.ItemGroup = '" + ItemGroup.Value + @"'";
            }
            else if (ItemGroup_CLASS.Value.Equals(ItemGroup.Value))
            {
                sql = @"select a.ItemGroup, a.ItemCode, c.ClassName as ItemDescription
                            , a.ParentItemCode, d.ClassName as ParentItemDescription
                            , a.RelationCode, b.RelationDescript as RelationDesc
                        from ERPW_DIAGRAM_RELATION a

                        left join master_equipment_class_relation_config b
                            on a.SID = b.SID
                            and a.CompanyCode = b.CompanyCode
                            and a.RelationCode = b.RelationCode

                        inner join master_equipment_class c
	                        on a.ItemCode = c.ClassCode
	                        and a.SID = c.SID

                        inner join master_equipment_class d
	                        on a.ParentItemCode = d.ClassCode
	                        and a.SID = d.SID

                        where a.SID = '" + SID + @"' 
                            AND a.CompanyCode = '" + CompanyCode + @"'
                            AND a.ItemGroup = '" + ItemGroup.Value + @"'";
            }
            else if (ItemGroup_TICKET.Value.Equals(ItemGroup.Value))
            {
                sql = @"select a.ItemGroup, a.ItemCode, a.ParentItemCode
	                        , b.HeaderText as ItemDescription
	                        , c.HeaderText as ParentItemDescription
                            , a.RelationCode, '' as RelationDesc
                        from ERPW_DIAGRAM_RELATION a
                        inner join cs_servicecall_header b
	                        on a.SID = b.SID
	                        and a.CompanyCode = b.CompanyCode
	                        and a.ItemCode = b.CallerID
                        inner join cs_servicecall_header c
	                        on a.SID = c.SID
	                        and a.CompanyCode = c.CompanyCode
	                        and a.ParentItemCode = c.CallerID

                        where a.SID = '" + SID + @"' 
                            AND a.CompanyCode = '" + CompanyCode + @"'
                            AND a.ItemGroup = '" + ItemGroup.Value + @"'";

                if (OtherKey != null)
                {
                    //sql += " AND a.OtherKey = '" + OtherKey + @"' ";
                }
            }


            DataTable dt = db.selectDataFocusone(sql);
            string JsonSTR = JsonConvert.SerializeObject(dt);
            List<FlowChartRelation> enList = JsonConvert.DeserializeObject<List<FlowChartRelation>>(JsonSTR);
            return enList;
        }

        private static string getItemDescription(string SID, string CompanyCode, string ItemCode, LinkFlowChartItemGroup ItemGroup)
        {
            EquipmentService ServiceEquipment = new EquipmentService();
            if (ItemGroup_EQUIPMENT.Equals(ItemGroup))
            {
                List<EquipmentService.EquipmentItemData> listEquipmentItem = ServiceEquipment.getListEquipment(
                    SID,
                    CompanyCode,
                    ItemCode,
                    "",
                    "",
                    ""
                );

                var data = listEquipmentItem.Where(w => w.EquipmentCode.Equals(ItemCode));
                return (data.Count() > 0) ? data.First().Description : ItemCode;
            }
            else if (ItemGroup_CLASS.Equals(ItemGroup))
            {
                DataTable dt = ServiceEquipment.getEMClass(SID);
                var data = dt.Select("ClassCode = '" + ItemCode + "'");
                return (data.Count() > 0) ? data[0]["ClassName"].ToString() : ItemCode;
            }
            else if (ItemGroup_TICKET.Equals(ItemGroup))
            {

                DataTable dtDocType = new DBService().selectDataFocusone(
                    @"select Doctype from cs_servicecall_header where CallerID in ('" + ItemCode + "')"
                );
                List<string> listDocType = dtDocType.AsEnumerable().Select(s => Convert.ToString(s["Doctype"])).ToList();

                DataTable dtPrefix = ServiceTicketLibrary.GetInstance().getDataPrefixDocType(SID, CompanyCode, listDocType);
                DataRow[] drr = dtPrefix.Select("'" + ItemCode + "' like PrefixCode + '%'");
                if (drr.Length > 0)
                {
                    string prefix = drr[0]["PrefixCode"].ToString();

                    string ticketNoDisplay = ItemCode;
                    //for (int i = 0; i < prefix.Length; i++)
                    //{
                    //    ticketNoDisplay = ticketNoDisplay.Replace(prefix[i].ToString(), "");
                    //}

                    return ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay);// prefix + Convert.ToInt32(ticketNoDisplay);
                }
                else
                {
                    return ItemCode;
                }
            }

            return ItemCode;
        }
        #endregion

        #region Update Data Diagram
        public static void insertNewItemRelation(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup,
            string ItemCode, string ParentItemCode, string RelationCode, string Created_By, string Created_On, string OtherKey)
        {
            db.executeSQLForFocusone(
                sqlInsertNewDataRelation(
                    SID, CompanyCode, ItemGroup, ItemCode, ParentItemCode, RelationCode, Created_By, Created_On, OtherKey
                )
            );
        }

        public static void updateDataDiagram(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup, string ItemCode,
            List<FlowChartRelation> flowChartRelation, string EMPCode, string OtherKey = null)
        {
            List<string> listSQL = new List<string>();
            string curentDataTime = Validation.getCurrentServerStringDateTime();
            listSQL.Add(sqlDeleteDataRelation(
                SID,
                CompanyCode,
                ItemGroup,
                ItemCode,
                OtherKey
            ));

            flowChartRelation.ForEach(r =>
            {
                listSQL.Add(sqlInsertNewDataRelation(
                    SID,
                    CompanyCode,
                    ItemGroup,
                    r.ItemCode,
                    ItemCode,
                    r.RelationCode,
                    EMPCode,
                    curentDataTime,
                    OtherKey
                ));

            });

            db.executeSQLForFocusone(listSQL);
        }

        private static string sqlDeleteDataRelation(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup, string ParentItemCode, string OtherKey)
        {
            string sql = @"delete ERPW_DIAGRAM_RELATION 
                            where SID = '" + SID + @"' 
                              AND CompanyCode = '" + CompanyCode + @"'
                              AND ItemGroup = '" + ItemGroup.Value + @"'
                              AND ParentItemCode = '" + ParentItemCode + @"'";

            if (OtherKey != null)
            {
                sql += " and OtherKey = '" + OtherKey + "' ";
            }

            return sql;
        }

        private static string sqlInsertNewDataRelation(string SID, string CompanyCode, LinkFlowChartItemGroup ItemGroup,
            string ItemCode, string ParentItemCode, string RelationCode, string Created_By, string Created_On, string OtherKey)
        {
            string sql = @"insert into ERPW_DIAGRAM_RELATION (
                               SID
                              ,CompanyCode
                              ,ItemGroup
                              ,ItemCode
                              ,ParentItemCode
                              ,RelationCode
                              ,Created_On
                              ,Created_By
                              ,OtherKey
                            ) VALUES (
                               '" + SID + @"'
                              ,'" + CompanyCode + @"'
                              ,'" + ItemGroup.Value + @"'
                              ,'" + ItemCode + @"'
                              ,'" + ParentItemCode + @"'
                              ,'" + RelationCode + @"'
                              ,'" + Created_On + @"'
                              ,'" + Created_By + @"'
                              ,'" + OtherKey + @"'
                            )";
            return sql;
        }
        #endregion

        #region New Entities
        public class DiagramRelation
        {
            public List<FlowChartRelation> parentNode { get; set; }
            public List<FlowChartRelation> chindNode { get; set; }
        }

        public class FlowChartRelation
        {
            public string ItemGroup { get; set; }
            public string ItemCode { get; set; }
            public string ItemDescription { get; set; }
            public string ParentItemCode { get; set; }
            public string ParentItemDescription { get; set; }
            public string RelationCode { get; set; }
            public string RelationDesc { get; set; }
            public int Level { get; set; }
        }
        #endregion
    }
}