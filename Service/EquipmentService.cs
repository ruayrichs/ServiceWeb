using agape.proxy.data.dataset;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using Newtonsoft.Json;
using ServiceWeb.LinkFlowChart.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceWeb.Service
{
    public class EquipmentService
    {
        private DBService dbService = new DBService();
        private LogServiceLibrary logservice = new LogServiceLibrary();
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

        #region Get search help
        public DataTable getWeightMaster(string SID)
        {
            string sql = @"select *, UCODE + ' : ' + UDESC as DetailDescription
                            from master_mm_weight_setup WITH (NOLOCK) 
                            where SID = '" + SID + @"'";

            return dbService.selectDataFocusone(sql);
        }

        //Edit 01/11/2561
        public DataTable getLocationMaster(string SID)
        {
            string sql = @"select *, CONCAT (LocationCode + ' : ',LocationName) as LocateName
                            from ERPW_Location_Equipment_Master WITH (NOLOCK) 
                            where SID = '" + SID + @"'";

            return dbService.selectDataFocusone(sql);
        }
        public DataTable getLocationMasterddl(string SID, string LocationCode)
        {
            string sql = @"select *
                            from ERPW_Location_Equipment_Master WITH (NOLOCK) 
                            where SID = '" + SID + @"' 
                            AND LocationCode = '" + LocationCode + "'";

            return dbService.selectDataFocusone(sql);
        }

        public void SaveCILocation(string SID, string CompanyCode, string LineNumber,
            string EquipmentCode, string LocationCode, string Created_By, string DateTime
            )//edit 06/11/2561
        {
            DataTable list = LogMapping(SID, EquipmentCode);
            int Range = 1;
            if (list.Rows.Count > 0)
            {
                Range = Convert.ToInt32(list.Select("LineNumber = max(LineNumber)")[0]["LineNumber"]) + 1;
            }

            string sql = @"insert into master_equipment_mapping_location (
                    SID
                    ,CompanyCode
                    ,LineNumber
                    ,EquipmentCode
                    ,LocationCode                              
                    ,Created_By
                    ,Created_On
                ) VALUES (
                    '" + SID + @"'
                    ,'" + CompanyCode + @"'
                    ,'" + Range.ToString().PadLeft(3, '0') + @"'
                    ,'" + EquipmentCode + @"'
                    ,'" + LocationCode + @"'
                    ,'" + Created_By + @"'
                    ,'" + Validation.getCurrentServerStringDateTime() + @"'
                )";
            System.Diagnostics.Debug.WriteLine(Range.ToString().PadLeft(3, '0'));
            dbService.executeSQLForFocusone(sql);
        }

        public DataTable LogMapping(string SID, string EquipmentCode) //edit 06/11/2561
        {
            string sql = @"select * from master_equipment_mapping_location WITH (NOLOCK) 
                            where SID = '" + SID + @"'
                            AND EquipmentCode = '" + EquipmentCode + @"'";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable LogMappingMaxOfCI(string SID,string CompanyCode, string EquipmentCode) //edit 06/11/2561
        {
            string sql = @"select top 1 * from master_equipment_mapping_location WITH (NOLOCK) 
                            where SID = '" + SID + @"'
                            AND CompanyCode = '"+CompanyCode+@"'
                            AND EquipmentCode = '" + EquipmentCode + @"' order by  LineNumber desc";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getMappingLineNumber(string SID, string CompanyCode, string EquipmentCode)//edit 06/11/2561
        {
            string sql = @"select master_equipment_mapping_location.EquipmentCode,
		                        master_equipment_mapping_location.LocationCode,
	                        	CAST(master_equipment_mapping_location.LineNumber as int) as LineNumber,
                                ERPW_Location_Equipment_Master.Plant,
								ERPW_Location_Equipment_Master.Location,
								ERPW_Location_Equipment_Master.LocationCategory,
								ERPW_Location_Equipment_Master.Room,
								ERPW_Location_Equipment_Master.Shelf,
								ERPW_Location_Equipment_Master.Slot,
								ERPW_Location_Equipment_Master.WorkCenter,
								ERPW_Location_Equipment_Master.AddressZipCode,
								ERPW_Location_Equipment_Master.AddressCity,
								ERPW_Location_Equipment_Master.AddressName1,
								ERPW_Location_Equipment_Master.AddressName2,
								ERPW_Location_Equipment_Master.AddressStreet,
								ERPW_Location_Equipment_Master.AddressTelephone,
								ERPW_Location_Equipment_Master.AddressFax
                                from master_equipment_mapping_location  WITH (NOLOCK) 
                                LEFT JOIN ERPW_Location_Equipment_Master  WITH (NOLOCK) 
								ON master_equipment_mapping_location.LocationCode = ERPW_Location_Equipment_Master.LocationCode
                                WHERE master_equipment_mapping_location.SID = '" + SID + @"' 
                                AND master_equipment_mapping_location.CompanyCode = '" + CompanyCode + @"'
								AND master_equipment_mapping_location.EquipmentCode = '" + EquipmentCode + @"'
								AND CAST(master_equipment_mapping_location.LineNumber as int) = (SELECT 
								MAX(CAST(master_equipment_mapping_location.LineNumber as int))FROM master_equipment_mapping_location)";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getPlantMaster(string SID)
        {
            string sql = @"select * , PLANTCODE + ' : ' + PLANTNAME1 as DetailDescription
                            from mm_conf_define_plant WITH (NOLOCK) 
                            where SID = '" + SID + @"'";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getStorageLocationMaster(string SID, string PlantCode)
        {
            string sql = @"select *, STORAGELOCCODE + ' : ' + StoreName as DetailDescription
                            from mm_conf_define_storagelocation WITH (NOLOCK) 
                            where SID = '" + SID + @"'";
            if (!string.IsNullOrEmpty(PlantCode))
            {
                sql += " and PLANTCODE = '" + PlantCode + @"' ";
            }
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getWorkCenterMaster(string SID)
        {
            string sql = @"select *, workCenter + ' : ' + Description as DetailDescription
                            from pp_master_workcenter_header WITH (NOLOCK) 
                            where SID = '" + SID + @"'";

            return dbService.selectDataFocusone(sql);
        }
        public DataTable getBusinessAreaMaster(string SID)
        {
            string sql = @"select *, BusinessAreaCode + ' : ' + Description as DetailDescription
                            from master_buarea WITH (NOLOCK) 
                            where SID = '" + SID + @"'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getDoctypeMaster(string SID, string CompanyCode)
        {
            string sql = @"select Doctype.DocumentTypeCode, Doctype.[description]
                              , Docdetail.PostingType, Business.Module
                              , Doctype.DocumentTypeCode + ' : ' + Doctype.[description] as DetailDescription

                            from master_config_lo_doctype Doctype WITH (NOLOCK) 
                            LEFT OUTER JOIN master_config_lo_doctype_docdetail Docdetail WITH (NOLOCK) 
                              ON Doctype.sid = Docdetail.sid 
                              AND Doctype.DocumentTypeCode = Docdetail.DocumentTypeCode
                              AND Doctype.CompanyCode = Docdetail.CompanyCode 
                            LEFT OUTER JOIN master_config_business Business
                              ON Docdetail.postingType = Business.BusinessCode

                            where Docdetail.PostingType = 'BILLING'
                              and Doctype.SID = '" + SID + @"'
                              and Doctype.CompanyCode = '" + CompanyCode + @"'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getBillingDocnumberByDoctype(string SID, string Doctype)
        {
            string sql = @"select * , SaleDocument + ' : ' + SaleDocument as DetailDescription
                            from bl_header
                            where SID = '" + SID + @"' 
                              AND Stypecode = '" + Doctype + @"'
                              and Stypecode in 
                              (
                                select DocumentTypeCode 
                                from master_config_lo_doctype_docdetail 
                                where sid = '" + SID + @"' 
                                and PostingType = 'BILLING'
                              )";

            return dbService.selectDataFocusone(sql);
        }
        public DataTable getSalseOrganizationMaster(string SID)
        {
            string sql = @"select *, SORGCODE + ' : ' + SORGNAME as DetailDescription
                            from sd_conf_define_sales_org
                            where SID = '" + SID + "'";

            return dbService.selectDataFocusone(sql);
        }
        public DataTable getDistChanalMaster(string SID)
        {
            string sql = @"select *, SCHANNELCODE + ' : ' + SHCANNELNAME as DetailDescription
                            from sd_conf_define_sales_channel
                            where SID = '" + SID + "'";

            return dbService.selectDataFocusone(sql);
        }
        public DataTable getDivisionMaster(string SID)
        {
            string sql = @"select * , SDIVCODE + ' : ' + SDIVNAME as DetailDescription
                            from sd_conf_define_sales_division
                            where SID = '" + SID + "'";

            return dbService.selectDataFocusone(sql);
        }
        public DataTable getSaleOfficeMaster(string SID)
        {
            string sql = @"select *, SOFFICECODE + ' : ' + SOFFICENAME as DetailDescription
                            from sd_conf_define_sales_office
                            where SID = '" + SID + "'";

            return dbService.selectDataFocusone(sql);
        }
        public DataTable getSaleGroupMaster(string SID)
        {
            string sql = @"select *, SGROUPCODE + ' : ' + SGROUPNAME as DetailDescription
                            from sd_conf_define_sales_group
                            where SID = '" + SID + "'";

            return dbService.selectDataFocusone(sql);
        }
        #endregion

        public List<EquipmentItemData> getListEquipment(string SID, string CompanyCode, string EquipmentCode,
            string EquipmentName, string EquipmentType, string Status)
        {
            return getListEquipment(SID, CompanyCode, EquipmentCode, EquipmentName, EquipmentType, Status, "", "", "", "", "", "", "");
        }

        public List<EquipmentItemData> getListEquipment(string SID, string CompanyCode, string EquipmentCode,
            string EquipmentName, string EquipmentType, string Status, string EquipmentClass, string Category,
            string OwnerGroupCode)
        {
            return getListEquipment(SID, CompanyCode, EquipmentCode, EquipmentName, EquipmentType, Status, EquipmentClass, Category, OwnerGroupCode, "", "", "", "");
        }

        public List<EquipmentItemData> getListEquipment(string SID, string CompanyCode, string EquipmentCode,
            string EquipmentName, string EquipmentType, string Status, string EquipmentClass, string Category,
            string OwnerGroupCode, string SerialNo, string btn, string TimeSendMail, string UserName)
        {
            StringBuilder Condition = new StringBuilder();

            if (!string.IsNullOrEmpty(EquipmentCode))
            {
                Condition.AppendLine("AND Equipment.EquipmentCode like '%" + EquipmentCode + "%'");
            }
            if (!string.IsNullOrEmpty(EquipmentName))
            {
                Condition.AppendLine("AND Equipment.[Description] like '%" + EquipmentName + "%'");
            }
            if (!string.IsNullOrEmpty(EquipmentType))
            {
                Condition.AppendLine("AND Equipment.EquipmentType = '" + EquipmentType + "'");
            }
            if (!string.IsNullOrEmpty(Status))
            {
                Condition.AppendLine("AND Equipment.Status = '" + Status + "'");
            }
            if (!string.IsNullOrEmpty(EquipmentClass))
            {
                Condition.AppendLine(" and ge.EquipmentClass = '" + EquipmentClass + "' ");
            }
            if (!string.IsNullOrEmpty(Category))
            {
                Condition.AppendLine(" and Equipment.CategoryCode = '" + Category + "' ");
            }
            if (!string.IsNullOrEmpty(OwnerGroupCode))
            {
                Condition.AppendLine(" and ge.EquipmentObjectType = '" + OwnerGroupCode + "' ");
            }
            if (!string.IsNullOrEmpty(SerialNo))
            {
                SerialNo = SerialNo.Replace('[', '%').Replace(']', '%').Replace('_', '%');
                Condition.AppendLine(" and ge.ManufacturerSerialNO like '%" + SerialNo + "%' ");
            }

            string sql;
            if (btn == "Search Send Mail")
            {
                sql = @" select Equipment.*                                                        
	                        from ERPW_master_equipment_warranty a
                            LEFT JOIN 
                                (SELECT Equipment.*
                                ,DocType.[Description] AS EquipmentTypeName
                                ,ge.EquipmentClass
                                ,class.ClassName AS EquipmentClassName
                                ,ge.EquipmentObjectType as OwnerGroupCode
                                ,ISNULL(Owner.OwnerGroupName,'') as OwnerGroupName
                                ,ELocation.LocationCode
                                ,ELocation.Location
                                ,ELocation.LocationName
                                ,ELocation.LocationCategory
                                ,ELocation.Room
                                ,ELocation.Shelf
                                FROM master_equipment Equipment WITH (NOLOCK) 
                                INNER JOIN master_config_material_doctype DocType  WITH (NOLOCK) 
                                    ON Equipment.SID = DocType.SID
                                    --AND Equipment.CompanyCode = DocType.Companycode
                                    AND Equipment.EquipmentType = DocType.MaterialGroupCode
                                INNER JOIN master_equipment_general ge WITH (NOLOCK)  ON ge.sid = Equipment.sid
	                                AND ge.companyCode = Equipment.companycode
	                                AND Equipment.EquipmentCode = ge.EquipmentCode
                                LEFT JOIN master_equipment_class class WITH (NOLOCK)  ON class.SID = ge.SID
	                                AND class.ClassCode = ge.EquipmentClass
                                LEFT JOIN ERPW_OWNER_GROUP AS [Owner] WITH (NOLOCK)  ON ge.SID = Owner.SID and ge.CompanyCode = Owner.CompanyCode
	                                AND ge.EquipmentObjectType = Owner.OwnerGroupCode

                                LEFT JOIN (
                                  select MapLocation.SID, MapLocation.EquipmentCode ,Max(MapLocation.LineNumber) as LineNumber --,MapLocation.LocationCode
                                  from master_equipment_mapping_location AS MapLocation  WITH (NOLOCK) 
                                  group by MapLocation.SID, MapLocation.EquipmentCode
                                ) MapLocationGroup
                                  ON  Equipment.SID = MapLocationGroup.SID
                                  AND Equipment.EquipmentCode = MapLocationGroup.EquipmentCode

                                LEFT JOIN master_equipment_mapping_location AS MapLocation WITH (NOLOCK) 
                                  ON  MapLocationGroup.SID = MapLocation.SID
                                  AND MapLocationGroup.EquipmentCode = MapLocation.EquipmentCode
                                  AND MapLocationGroup.LineNumber = MapLocation.LineNumber
  
                                LEFT JOIN ERPW_Location_Equipment_Master AS ELocation WITH (NOLOCK) 
                                  ON Equipment.SID =  ELocation.SID
                                  AND MapLocation.LocationCode = ELocation.LocationCode
  
                                WHERE Equipment.SID = '" + SID + @"' 
                                AND Equipment.CompanyCode = '" + CompanyCode + @"') Equipment
                            ON a.SID = Equipment.SID
	                        and a.CompanyCode = Equipment.CompanyCode
	                        and a.EquipmentCode = Equipment.EquipmentCode
	                        WHERE Equipment.EquipmentCode != '' and a.NextMaintenanceDate != ''
                            
                            " + Condition.ToString();
            }
            else if (btn == "Update Send Mail")
            {
                sql = @" select Equipment.*
                            , DATEDIFF(DAY, CURRENT_TIMESTAMP, DATEADD(DAY, -" + TimeSendMail + @", a.NextMaintenanceDate)) * 86400 AS TargetTime
                            , DATEDIFF(DAY, CURRENT_TIMESTAMP, (DATEADD(DAY, -" + TimeSendMail + @", a.NextMaintenanceDate))) AS NextDayToSendMail
                            , CURRENT_TIMESTAMP AS TODAY
                            , DATEADD(DAY, -" + TimeSendMail + @", a.NextMaintenanceDate) AS SendMailDate
                            , a.NextMaintenanceDate
                            , YEAR(CURRENT_TIMESTAMP) AS Year
	                        from ERPW_master_equipment_warranty a
                            LEFT JOIN 
                                (SELECT Equipment.*
                                ,DocType.[Description] AS EquipmentTypeName
                                ,ge.EquipmentClass
                                ,class.ClassName AS EquipmentClassName
                                ,ge.EquipmentObjectType as OwnerGroupCode
                                ,ISNULL(Owner.OwnerGroupName,'') as OwnerGroupName
                                ,ELocation.LocationCode
                                ,ELocation.Location
                                ,ELocation.LocationName
                                ,ELocation.LocationCategory
                                ,ELocation.Room
                                ,ELocation.Shelf
                                FROM master_equipment Equipment WITH (NOLOCK) 
                                INNER JOIN master_config_material_doctype DocType  WITH (NOLOCK) 
                                    ON Equipment.SID = DocType.SID
                                    --AND Equipment.CompanyCode = DocType.Companycode
                                    AND Equipment.EquipmentType = DocType.MaterialGroupCode
                                INNER JOIN master_equipment_general ge WITH (NOLOCK)  ON ge.sid = Equipment.sid
	                                AND ge.companyCode = Equipment.companycode
	                                AND Equipment.EquipmentCode = ge.EquipmentCode
                                LEFT JOIN master_equipment_class class WITH (NOLOCK)  ON class.SID = ge.SID
	                                AND class.ClassCode = ge.EquipmentClass
                                LEFT JOIN ERPW_OWNER_GROUP AS [Owner] WITH (NOLOCK)  ON ge.SID = Owner.SID and ge.CompanyCode = Owner.CompanyCode
	                                AND ge.EquipmentObjectType = Owner.OwnerGroupCode

                                LEFT JOIN (
                                  select MapLocation.SID, MapLocation.EquipmentCode ,Max(MapLocation.LineNumber) as LineNumber --,MapLocation.LocationCode
                                  from master_equipment_mapping_location AS MapLocation  WITH (NOLOCK) 
                                  group by MapLocation.SID, MapLocation.EquipmentCode
                                ) MapLocationGroup
                                  ON  Equipment.SID = MapLocationGroup.SID
                                  AND Equipment.EquipmentCode = MapLocationGroup.EquipmentCode

                                LEFT JOIN master_equipment_mapping_location AS MapLocation WITH (NOLOCK) 
                                  ON  MapLocationGroup.SID = MapLocation.SID
                                  AND MapLocationGroup.EquipmentCode = MapLocation.EquipmentCode
                                  AND MapLocationGroup.LineNumber = MapLocation.LineNumber
  
                                LEFT JOIN ERPW_Location_Equipment_Master AS ELocation WITH (NOLOCK) 
                                  ON Equipment.SID =  ELocation.SID
                                  AND MapLocation.LocationCode = ELocation.LocationCode
  
                                WHERE Equipment.SID = '" + SID + @"' 
                                AND Equipment.CompanyCode = '" + CompanyCode + @"') Equipment
                            ON a.SID = Equipment.SID
	                        and a.CompanyCode = Equipment.CompanyCode
	                        and a.EquipmentCode = Equipment.EquipmentCode
	                        WHERE Equipment.EquipmentCode != '' and a.NextMaintenanceDate != ''
                            
                            " + Condition.ToString();
                DataTable dtTable = dbService.selectDataFocusone(sql);
                foreach (DataRow dtRow in dtTable.Rows)
                {
                    var EquipmentNo = dtRow["EquipmentCode"].ToString();
                    var Year = dtRow["Year"].ToString();
                    var TimeSec = dtRow["TargetTime"].ToString();
                    String TransactionID = Guid.NewGuid().ToString();
                    TriggerService.GetInstance().EscalateTicket(
                    TransactionID, "CI", EquipmentNo, Year, TimeSec, UserName
                    );

                    TriggerService.GetInstance().CancelTriggerCI(
                    TransactionID, "CI", EquipmentNo
                    );
                }
            }
            else
            {
                sql = @"SELECT Equipment.*
                            ,DocType.[Description] AS EquipmentTypeName
                            ,ge.EquipmentClass
                            ,class.ClassName AS EquipmentClassName
                            ,ge.EquipmentObjectType as OwnerGroupCode
                            ,ISNULL(Owner.OwnerGroupName,'') as OwnerGroupName
                            ,ELocation.LocationCode
                            ,ELocation.Location
                            ,ELocation.LocationName
                            ,ELocation.LocationCategory
                            ,ELocation.Room
                            ,ELocation.Shelf
                            FROM master_equipment Equipment WITH (NOLOCK) 
                            INNER JOIN master_config_material_doctype DocType  WITH (NOLOCK) 
                                ON Equipment.SID = DocType.SID
                                --AND Equipment.CompanyCode = DocType.Companycode
                                AND Equipment.EquipmentType = DocType.MaterialGroupCode
                            INNER JOIN master_equipment_general ge WITH (NOLOCK)  ON ge.sid = Equipment.sid
	                            AND ge.companyCode = Equipment.companycode
	                            AND Equipment.EquipmentCode = ge.EquipmentCode
                            LEFT JOIN master_equipment_class class WITH (NOLOCK)  ON class.SID = ge.SID
	                            AND class.ClassCode = ge.EquipmentClass
                            LEFT JOIN ERPW_OWNER_GROUP AS [Owner] WITH (NOLOCK)  ON ge.SID = Owner.SID and ge.CompanyCode = Owner.CompanyCode
	                            AND ge.EquipmentObjectType = Owner.OwnerGroupCode

                            LEFT JOIN (
                              select MapLocation.SID, MapLocation.EquipmentCode ,Max(MapLocation.LineNumber) as LineNumber --,MapLocation.LocationCode
                              from master_equipment_mapping_location AS MapLocation  WITH (NOLOCK) 
                              group by MapLocation.SID, MapLocation.EquipmentCode
                            ) MapLocationGroup
                              ON  Equipment.SID = MapLocationGroup.SID
                              AND Equipment.EquipmentCode = MapLocationGroup.EquipmentCode

                            LEFT JOIN master_equipment_mapping_location AS MapLocation WITH (NOLOCK) 
                              ON  MapLocationGroup.SID = MapLocation.SID
                              AND MapLocationGroup.EquipmentCode = MapLocation.EquipmentCode
                              AND MapLocationGroup.LineNumber = MapLocation.LineNumber
  
                            LEFT JOIN ERPW_Location_Equipment_Master AS ELocation WITH (NOLOCK) 
                              ON Equipment.SID =  ELocation.SID
                              AND MapLocation.LocationCode = ELocation.LocationCode
  
                            WHERE Equipment.SID = '" + SID + @"' 
                    AND Equipment.CompanyCode = '" + CompanyCode + @"' 
                    " + Condition.ToString();
            }
            
            

            DataTable dt = dbService.selectDataFocusone(sql);
            string JsonStr = JsonConvert.SerializeObject(dt);

            List<EquipmentItemData> en = JsonConvert.DeserializeObject<List<EquipmentItemData>>(JsonStr);
            return en;
        }

        
        public DataTable getEquipmentDetail(string SID, string CompanyCode, List<string> listEquipmentCode)
        {
            if (listEquipmentCode == null)
            {
                return new DataTable();
            }

            string sql = @"SELECT a.EquipmentCode, a.[Description] as EquipmentName
	                        , a.EquipmentType
	                        , b.Description AS EquipmentTypeDesc
	                        , a.CategoryCode
	                        , CASE a.CategoryCode 
		                        WHEN '00' THEN 'Main Equipment'
		                        WHEN '01' THEN 'Sub Equipment'
		                        ELSE 'Virtual Equipment'
		                        END AS CategoryName
	                        , c.EquipmentClass
	                        , d.ClassName
                        FROM master_equipment a WITH (NOLOCK) 
                        INNER JOIN master_config_material_doctype b WITH (NOLOCK)  ON a.SID = b.SID
	                        --AND a.CompanyCode = b.Companycode
	                        AND a.EquipmentType = b.MaterialGroupCode
                        LEFT JOIN master_equipment_general c WITH (NOLOCK)  ON a.SID = c.SID
	                        AND a.CompanyCode = c.CompanyCode
	                        AND a.EquipmentCode = c.EquipmentCode
                        LEFT JOIN master_equipment_class d WITH (NOLOCK)  ON c.SID = d.SID
	                        AND c.EquipmentClass = d.ClassCode
                        WHERE a.SID = '" + SID + @"'
	                        AND a.CompanyCode = '" + CompanyCode + @"'
	                        AND a.EquipmentCode in ('" + string.Join("','", listEquipmentCode) + @"')";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public DataTable getListEquipmentByOwner(string SID, string CompanyCode, string OwnerType, string OwnerCode)
        {
            string sql = @"select owner_a.OwnerCode, owner_a.ActiveStatus, owner_a.BeginDate, owner_a.EndDate
                              , Equipment.*, DocType.[Description] as EquipmentTypeName, ge.EquipmentClass
  
                            from master_equipment_owner_assignment owner_a WITH (NOLOCK) 
                            inner join master_equipment Equipment  WITH (NOLOCK) 
                              on owner_a.SID = Equipment.SID
                              and owner_a.CompanyCode = Equipment.CompanyCode
                              and owner_a.EquipmentCode = Equipment.EquipmentCode
  
                            inner join master_config_material_doctype DocType WITH (NOLOCK) 
                              ON Equipment.SID = DocType.SID
                              --AND Equipment.CompanyCode = DocType.Companycode
                              AND Equipment.EquipmentType = DocType.MaterialGroupCode
  
                            inner join master_equipment_general ge WITH (NOLOCK) 
                              on ge.sid = Equipment.sid 
                              and ge.companyCode = Equipment.companycode
                              and Equipment.EquipmentCode = ge.EquipmentCode

                            where owner_a.SID = '" + SID + @"' 
                              AND owner_a.CompanyCode = '" + CompanyCode + @"'
                              AND owner_a.OwnerType = '" + OwnerType + @"'
                              AND owner_a.OwnerCode = '" + OwnerCode + @"'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public string getEquipmentMappingOwnerService(string sid, string CompanyCode, string EquipmentCode)
        {
            string OwnerService = "";
            string sql = @"select EquipmentObjectType, EquipmentCode
                            from master_equipment_general WITH (NOLOCK) 
                            where sid = '" + sid + @"'
                              and CompanyCode = '" + CompanyCode + @"'
                              and EquipmentCode = '" + EquipmentCode + @"'";
            DataTable dt = dbService.selectDataFocusone(sql);
            if (dt.Rows.Count > 0)
            {
                OwnerService = Convert.ToString(dt.Rows[0]["EquipmentObjectType"]);
            }

            return OwnerService;
        }

        public DataTable getCostcenterMaster(string SID)
        {
            string sql = @"select *, COSTCENTERCODE + ' : ' + COSTCENTERNAME as DetailDescription
                            from co_costcenter_master WITH (NOLOCK) 
                            where  SID = '" + SID + "'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getEMClass(string SID)
        {
            string sql = @"select * 
                            from master_equipment_class WITH (NOLOCK) 
                            where  SID = '" + SID + "'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getConfigClassRelationMater(string SID, string CompanyCode)
        {
            string sql = @"select * 
                            from master_equipment_class_relation_config WITH (NOLOCK) 
                            where SID = '" + SID + @"' 
                              AND CompanyCode = '" + CompanyCode + "'";

            return dbService.selectDataFocusone(sql);
        }

        public List<string> getEquipmentClassRelationConfig(string SID, string CompanyCode, string EquipmentCode)
        {
            List<EquipmentItemData> equipment = getListEquipment(SID, CompanyCode, EquipmentCode, "", "", "");
            if (equipment.Count == 0)
            {
                return new List<string>();
            }
            List<LinkFlowChartService.FlowChartRelation> enRelation = LinkFlowChartService.getDiagramRelation(
                SID,
                CompanyCode,
                equipment.First().EquipmentClass,
                LinkFlowChartService.ItemGroup_CLASS
            ).chindNode.Where(w => w.Level == 1).ToList();

            List<string> listClassCode = enRelation.Select(s => s.ItemCode).ToList();
            return listClassCode;
        }

        public List<EquipmentMappingFirstOwner> getListEquipmentMappingFirstOwner(string SID, string CompanyCode)
        {
            string sql = @"select a.EquipmentCode, a.[Description] as EquipmentName
                              , b.CustomerCode, c.CustomerName, b.SLAGroupCode
                            from master_equipment a WITH (NOLOCK) 
                            inner join 
                            (
                              select EquipmentCode, min(LineNumber) as LineNumber
                              , min(OwnerCode) as CustomerCode
                              , min(SLAGroupCode) as SLAGroupCode
                              from master_equipment_owner_assignment WITH (NOLOCK) 
                              where SID = '" + SID + @"' 
                                AND CompanyCode = '" + CompanyCode + @"' 
                                AND OwnerType = '01' 
                                AND ActiveStatus = 'True'  
                              group by EquipmentCode
                            ) b
                            on a.EquipmentCode = b.EquipmentCode
                            inner join master_customer c WITH (NOLOCK) 
                            on c.SID = a.SID 
                            AND c.CompanyCode = a.CompanyCode 
                            AND c.CustomerCode = b.CustomerCode

                            where a.SID = '" + SID + @"' 
                              AND a.CompanyCode = '" + CompanyCode + @"' ";

            DataTable dt = dbService.selectDataFocusone(sql);
            string JsonDatas = JsonConvert.SerializeObject(dt);
            List<EquipmentMappingFirstOwner> listDatas = JsonConvert.DeserializeObject<List<EquipmentMappingFirstOwner>>(JsonDatas);
            return listDatas;
        }

        #region Get SoldToParty ,ShipToParty ,BillToParty Ref Equipment

        public DataTable getCustomerSoldToPartyRefEquipment(string SID, string companyCode, string EquipmentCode)
        {
            return getCustomerRefSoForEquipment(SID, companyCode, "SoldToParty", EquipmentCode);
        }

        public DataTable getCustomerShipToPartyRefEquipment(string SID, string companyCode, string EquipmentCode)
        {
            return getCustomerRefSoForEquipment(SID, companyCode, "ShipToParty", EquipmentCode);
        }

        public DataTable getCustomerBillToPartyRefEquipment(string SID, string companyCode, string EquipmentCode)
        {
            return getCustomerRefSoForEquipment(SID, companyCode, "BillToParty", EquipmentCode);
        }

        private DataTable getCustomerRefSoForEquipment(string SID, string companyCode, string _modeTypeParty, string EquipmentCode)
        {
            string sql = @"
                        select distinct a." + _modeTypeParty + @" as CustomerCode
                        ,e.CustomerName as CustomerName
                        from sd_so_header a WITH (NOLOCK) 
                        inner join master_customer_service_contract_item_refdoc b WITH (NOLOCK) 
                        on a.SID= b.SID and a.companyCode = b.CompanyCode
                         and a.Stypecode = b.RefDoctype 
                         and a.FiscalYear = b.RefFiscalyear  
                         and a.SaleDocument = b.RefDocno 
                        inner join master_customer_service_contract_item c WITH (NOLOCK) 
                        on b.SID = c.SID and b.CompanyCode = c.CompanyCode
                         and b.DocumentType = c.DocumentType
                         and b.Fiscalyear = b.Fiscalyear
                         and b.ContractNo = c.ContractNo
                        left join master_customer e WITH (NOLOCK) 
                         on a.SID = e.SID 
	                        and a.companyCode = e.CompanyCode
	                        and  a." + _modeTypeParty + @" = e.CustomerCode
                        where a.SID='" + SID + @"' and a.companyCode='" + companyCode + @"'
                         and b.RefBusinessType='SO'
                         and ISNULL(e.CustomerName,'') <> ''
                         and c.EquipmentNo='" + EquipmentCode + @"' 
                        ";
            return dbService.selectDataFocusone(sql);
        }

        #endregion

        #region Warranty 

        public DataTable getWarrantyData(string SID, string CompanyCode, string EquipmentCode)
        {
            string sql = "select * from ERPW_master_equipment_warranty WITH (NOLOCK)  where SID='" + SID + "' and CompanyCode='" + CompanyCode + "' ";
            if (!string.IsNullOrEmpty(EquipmentCode))
            {
                sql += " and EquipmentCode='" + EquipmentCode + "' ";
            }
            DataTable dtWarranty = dbService.selectDataFocusone(sql);
            foreach (DataRow dr in dtWarranty.Rows)
            {
                dr["NextMaintenanceDate"] = getCurrentNextMaintenanceDate(dr["BeginGuarantee"].ToString(), dr["EndGuaranty"].ToString()
                    , dr["NextMaintenanceDate"].ToString(), dr["Period"].ToString());
            }
            return dtWarranty;
        }
        
        //public DataTable getNextMaintenanceTime()
        //{
        //    string sql = "select * from ERPW_master_equipment_warranty WHERE NextMaintenanceDate != ''";
        //    DataTable dtWarranty = dbService.selectDataFocusone(sql);
        //    return dtWarranty;
        //}



        public void saveWarrantyMaster(DataTable dtWarrantyForSave)
        {
            if (dtWarrantyForSave == null || dtWarrantyForSave.Rows.Count <= 0)
            {
                return;
            }
            List<Main_LogService> listEnLog = new List<Main_LogService>();
            DataRow dr = dtWarrantyForSave.Rows[0];
            string SID = dr["SID"].ToString();
            string CompanyCode = dr["CompanyCode"].ToString();
            string EquipmentCode = dr["EquipmentCode"].ToString();
            string EmployeeCode = !string.IsNullOrEmpty(dr["UPDATED_BY"].ToString()) ? dr["UPDATED_BY"].ToString() : dr["CREATED_BY"].ToString();
            string sql = "select * from ERPW_master_equipment_warranty WITH (NOLOCK)  where SID='" + SID + "' and CompanyCode='" + CompanyCode + "'  and EquipmentCode='" + EquipmentCode + "' ";
            DataTable dtWarranty = dbService.selectDataFocusone(sql);
            if (dtWarranty.Rows.Count <= 0)
            {
                DataRow drNew = dtWarranty.NewRow();
                drNew["SID"] = SID;
                drNew["CompanyCode"] = CompanyCode;
                drNew["EquipmentCode"] = EquipmentCode;
                dtWarranty.Rows.Add(drNew);
                listEnLog = prepareWarrantyForSaveLog(SID, CompanyCode, EmployeeCode, EquipmentCode, dtWarranty, dtWarrantyForSave);
                dtWarranty = dtWarrantyForSave;
            }
            else
            {
                listEnLog = prepareWarrantyForSaveLog(SID, CompanyCode, EmployeeCode, EquipmentCode, dtWarranty, dtWarrantyForSave);
                DataRow drupdate = dtWarranty.Rows[0];
                drupdate["CategoryCode"] = dr["CategoryCode"];
                drupdate["BeginGuarantee"] = dr["BeginGuarantee"];
                drupdate["EndGuaranty"] = dr["EndGuaranty"];
                drupdate["BeginWarrantee"] = dr["BeginWarrantee"];
                drupdate["EndWarrantee"] = dr["EndWarrantee"];
                drupdate["Period"] = dr["Period"];
                drupdate["NextMaintenanceDate"] = dr["NextMaintenanceDate"];
                drupdate["NextMaintenanceTime"] = dr["NextMaintenanceTime"];
                drupdate["LastMaintenanceDate"] = dr["LastMaintenanceDate"];
                drupdate["LastMaintenanceTime"] = dr["LastMaintenanceTime"];
                drupdate["UPDATED_BY"] = dr["UPDATED_BY"];
                drupdate["UPDATED_ON"] = dr["UPDATED_ON"];
            }
            dtWarranty.TableName = "ERPW_master_equipment_warranty";
            dtWarranty.PrimaryKey = new DataColumn[] { dtWarranty.Columns["SID"], dtWarranty.Columns["CompanyCode"], dtWarranty.Columns["EquipmentCode"] };
            dbService.SaveTransactionForFocusone(dtWarranty);
            if (listEnLog.Count > 0)
            {
                logservice.SaveLog(SID, LogServiceLibrary.PROGRAM_ID_EQUIPMENT, "M", listEnLog);
            }
        }

        public string getCurrentNextMaintenanceDate(string BeginGuarantee, string EndGuarantee
           , string NextMaintenanceDate
           , string Period)
        {
            DateTime dateCurrent = ObjectUtil.ConvertDateTimeDBToDateTime(Validation.getCurrentServerStringDateTime().Substring(0, 8) + "000000");
            return calculateCurrentNextMaintenanceDate(BeginGuarantee, EndGuarantee, NextMaintenanceDate, Period, dateCurrent);
        }
        private string calculateCurrentNextMaintenanceDate(string BeginGuarantee, string EndGuarantee
            , string NextMaintenanceDate
            , string Period
            , DateTime dateCurrent)
        {
            Int16 nPeriod = 0;
            Int16.TryParse(Period, out nPeriod);
            if (string.IsNullOrEmpty(BeginGuarantee) || nPeriod == 0)
            {
                return NextMaintenanceDate;
            }
            //get วันปัจจุบัน
            //DateTime dateCurrent = ObjectUtil.ConvertDateTimeDBToDateTime(Validation.getCurrentServerStringDateTime().Substring(0,8) + "000000");
            DateTime dateBeginGuarantee = ObjectUtil.ConvertDateTimeDBToDateTime(BeginGuarantee + "000000");
            //ถ้า END ไม่เป็นว่างให้คิด EndGuarantee ด้วย
            if (!string.IsNullOrEmpty(EndGuarantee))
            {
                DateTime dateEndGuarantee = ObjectUtil.ConvertDateTimeDBToDateTime(EndGuarantee + "000000");
                //วันปัจจุบันอยู่นอกขอบเขตที่ระบุ
                //ถ้าน้อยกว่า ขอบเขต
                if (dateCurrent < dateBeginGuarantee && !string.IsNullOrEmpty(NextMaintenanceDate))
                {
                    return NextMaintenanceDate;
                }
                //ถ้ามากกว่า ขอบเขต ให้เอารอบล่าสุด
                else if (dateCurrent > dateEndGuarantee && !string.IsNullOrEmpty(NextMaintenanceDate))
                {
                    return calculateCurrentNextMaintenanceDate(BeginGuarantee, EndGuarantee, NextMaintenanceDate, Period, dateEndGuarantee);
                }
            }
            else
            {
                if ((dateCurrent < dateBeginGuarantee) && !string.IsNullOrEmpty(NextMaintenanceDate))
                {
                    return NextMaintenanceDate;
                }
            }

            //หารอบปัจจุบัน และ ถัดไป
            DateTime dateCurrentRount = dateBeginGuarantee;
            while (dateCurrentRount.AddMonths(nPeriod) <= dateCurrent)
            {
                dateCurrentRount = dateCurrentRount.AddMonths(nPeriod);
            }
            DateTime dateNextMin = dateCurrentRount.AddMonths(nPeriod);
            DateTime dateNextMax = dateNextMin.AddMonths(nPeriod);

            //เช็ควันที่ได้ ตามรอบปัจจุบัน
            if (!string.IsNullOrEmpty(NextMaintenanceDate))
            {
                DateTime dateNextMaintenanceDate = ObjectUtil.ConvertDateTimeDBToDateTime(NextMaintenanceDate.Substring(0, 8) + "000000");
                if ((dateCurrent <= dateNextMaintenanceDate && dateCurrentRount < dateNextMaintenanceDate && dateNextMaintenanceDate <= dateNextMin)
                    || (dateCurrent <= dateNextMaintenanceDate && dateNextMin < dateNextMaintenanceDate && dateNextMaintenanceDate < dateNextMax))
                {
                    return NextMaintenanceDate;
                }
            }
            return Validation.Convert2DateDB(dateNextMin.ToString("dd/MM/yyyy"));
        }

        private List<Main_LogService> prepareWarrantyForSaveLog(string SID, string CompanyCode, string EmployeeCode
            , string EquipmentCode
            , DataTable dtOld, DataTable dtNew)
        {
            List<Main_LogService> listEnLog = new List<Main_LogService>();
            List<Detail_LogService> listMaster = new List<Detail_LogService>();
            string UserName = EmployeeService.GetInstance().GetUserNameRefEmployeeCode(SID, CompanyCode, EmployeeCode);
            string strDate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
            string strTime = Validation.getCurrentServerStringDateTime().Substring(8, 6);
            DataRow drOld = dtOld.Rows[0];
            DataRow drNew = dtNew.Rows[0];
            #region Prepare Data
            if (Convert.ToString(drOld["BeginGuarantee"]) != drNew["BeginGuarantee"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Maintenance Start Date", ItemNumber = "", NewValue = drNew["BeginGuarantee"].ToString(), OldValue = Convert.ToString(drOld["BeginGuarantee"]) });
            }
            if (Convert.ToString(drOld["EndGuaranty"]) != drNew["EndGuaranty"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Maintenance End Date", ItemNumber = "", NewValue = drNew["EndGuaranty"].ToString(), OldValue = Convert.ToString(drOld["EndGuaranty"]) });
            }

            if (Convert.ToString(drOld["BeginWarrantee"]) != drNew["BeginWarrantee"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Warranty start date", ItemNumber = "", NewValue = drNew["BeginWarrantee"].ToString(), OldValue = Convert.ToString(drOld["BeginWarrantee"]) });
            }
            if (Convert.ToString(drOld["EndWarrantee"]) != drNew["EndWarrantee"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Warranty end date", ItemNumber = "", NewValue = drNew["EndWarrantee"].ToString(), OldValue = Convert.ToString(drOld["EndWarrantee"]) });
            }

            if (Convert.ToString(drOld["CategoryCode"]) != drNew["CategoryCode"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Maintenance Type", ItemNumber = "", NewValue = drNew["CategoryCode"].ToString(), OldValue = Convert.ToString(drOld["CategoryCode"]) });
            }
            if (Convert.ToString(drOld["Period"]) != drNew["Period"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Period", ItemNumber = "", NewValue = drNew["Period"].ToString(), OldValue = Convert.ToString(drOld["Period"]) });
            }
            if (Convert.ToString(drOld["NextMaintenanceDate"]) != drNew["NextMaintenanceDate"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Next Maintenance Date", ItemNumber = "", NewValue = drNew["NextMaintenanceDate"].ToString(), OldValue = Convert.ToString(drOld["NextMaintenanceDate"]) });
            }
            if (Convert.ToString(drOld["NextMaintenanceTime"]) != drNew["NextMaintenanceTime"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Next Maintenance Time", ItemNumber = "", NewValue = drNew["NextMaintenanceTime"].ToString(), OldValue = Convert.ToString(drOld["NextMaintenanceTime"]) });
            }
            if (Convert.ToString(drOld["LastMaintenanceDate"]) != drNew["LastMaintenanceDate"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Last Maintenance Date", ItemNumber = "", NewValue = drNew["LastMaintenanceDate"].ToString(), OldValue = Convert.ToString(drOld["LastMaintenanceDate"]) });
            }
            if (Convert.ToString(drOld["LastMaintenanceTime"]) != drNew["LastMaintenanceTime"].ToString())
            {
                listMaster.Add(new Detail_LogService() { FieldName = "Last Maintenance Time", ItemNumber = "", NewValue = drNew["LastMaintenanceTime"].ToString(), OldValue = Convert.ToString(drOld["LastMaintenanceTime"]) });
            }
            #endregion

            if (listMaster.Count > 0)
            {
                listEnLog.Add(new Main_LogService
                {
                    LOGOBJCODE = "",
                    PROGOBJECT = LogServiceLibrary.PROGRAM_ID_EQUIPMENT + "|ERPW_master_equipment_warranty",
                    ACCESSCODE = LogServiceLibrary.AccessCode_Change,
                    OBJPKREC = SID + CompanyCode + EquipmentCode,
                    APPLTYPE = "M",
                    Access_By = UserName,
                    Access_Date = strDate,
                    Access_Time = strTime,
                    listDetail = listMaster
                });
            }



            return listEnLog;
        }
        #endregion


        [Serializable]
        public class EquipmentItemData
        {
            public string SID { get; set; }
            public string CompanyCode { get; set; }
            public string EquipmentCode { get; set; }
            public string CategoryCode { get; set; }
            public string Reference { get; set; }
            public string Material { get; set; }
            public string CREATED_BY { get; set; }
            public string UPDATED_BY { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public string Valid_From { get; set; }
            public string Valid_To { get; set; }
            public string EquipmentType { get; set; }
            public string CREATED_ON { get; set; }
            public string UPDATED_ON { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }
            public string PicturePart { get; set; }
            public string ObjectID { get; set; }
            public string ActiveBy { get; set; }
            public string ActiveDate { get; set; }
            public string ActiveTime { get; set; }
            public string EquipmentTypeName { get; set; }
            public string EquipmentClass { get; set; }
            public string EquipmentClassName { get; set; }

            public string OwnerGroupCode { get; set; }
            public string OwnerGroupName { get; set; }

            public string LineNumber { get; set; }
            public string LocationCode { get; set; }
            public string EmployeeCode { get; set; }

            //public string LocationCode { get; set; }
            public string Location { get; set; }
            public string LocationName { get; set; }
            public string LocationCategory { get; set; }
            public string Room { get; set; }
            public string Shelf { get; set; }
        }

        [Serializable]
        public class EquipmentMappingFirstOwner
        {
            public string EquipmentCode { get; set; }
            public string EquipmentName { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public string SLAGroupCode { get; set; }
        }

        #region CI
        public List<CI_Item> getListCI (string SID,string CIBatch,string SONumber,string shiptocode,string ShipToName, string MaterialCode, string MeterialName, string StartDate, string EndDate, string Status, string RequestType)
        {
            StringBuilder Condition = new StringBuilder();
            
            if (!string.IsNullOrEmpty(CIBatch))
            {
                Condition.AppendLine("AND CI.CIBatch like '%" + CIBatch + "%'");
            }
            if (!string.IsNullOrEmpty(SONumber))
            {
                Condition.AppendLine("AND CI.[SONumber] like '%" + SONumber + "%'");
            }
            if (!string.IsNullOrEmpty(shiptocode))
            {
                Condition.AppendLine("AND CI.shiptocode like '%" + shiptocode + "%'");
            }
            if (!string.IsNullOrEmpty(ShipToName))
            {
                Condition.AppendLine("AND CI.ShipToName like '%" + ShipToName + "%'");
            }
            if (!string.IsNullOrEmpty(MaterialCode))
            {
                Condition.AppendLine(" and CI.MaterialCode like '%" + MaterialCode + "%' ");
            }
            if (!string.IsNullOrEmpty(MeterialName))
            {
                Condition.AppendLine(" and CI.MeterialName like '%" + MeterialName + "%' ");
            }
            if (!string.IsNullOrEmpty(StartDate))
            {
                Condition.AppendLine(" and CI.StartDate = '" + StartDate + "' ");
            }
            if (!string.IsNullOrEmpty(EndDate))
            {
                Condition.AppendLine(" and CI.EndDate = '" + EndDate + "' ");
            }
            if (!string.IsNullOrEmpty(Status))
            {
                Condition.AppendLine(" and CI.Status = '" + Status + "' ");
            }
            if (!string.IsNullOrEmpty(RequestType))
            {
                Condition.AppendLine(" and CI.RequestType = '" + RequestType + "' ");
            }
            Condition.AppendLine(" and CI.status='open'");

            string sql = @"select * from SD_SO_CI_REQUEST CI
                           WHERE CI.SID = '" + SID + @"' " + Condition.ToString();

            DataTable dt = dbService.selectDataFocusone(sql);
            List<CI_Item> en = JsonConvert.DeserializeObject<List<CI_Item>>(JsonConvert.SerializeObject(dt));
            return en;
        }
        public void UpdateCIItem(string SID, string CIBatch, string SONumber, string SOType, string SOItemNo,string fiscalYear)
        {
            string sql = @"UPDATE SD_SO_CI_REQUEST
                            SET [Status] ='Created'
                            Where SID='"+ SID +@"'
                            and CIBatch='"+CIBatch+@"'
                            and SOType='"+SOType+@"'
                            and SONumber='"+SONumber+@"'
                            and FiscalYear ='"+fiscalYear+@"'
                            and SoItemNo ='"+SOItemNo+@"'";

            dbService.executeSQLForFocusone(sql);
        }

        public DataTable getListCIOfCustomer(string SID, string CompanyCode, string customerCode, string customerType)
        {

            string sql = @"select ISNULL(Owner.OwnerGroupName,'') as OwnerGroupName,owner_a.OwnerCode, owner_a.ActiveStatus, owner_a.BeginDate, owner_a.EndDate
                              , Equipment.*, DocType.[Description] as EquipmentTypeName, ge.EquipmentClass,class.ClassName AS EquipmentClassName
  
                            from master_equipment_owner_assignment owner_a
                            left join master_equipment Equipment 
                              on owner_a.SID = Equipment.SID
                              and owner_a.CompanyCode = Equipment.CompanyCode
                              and owner_a.EquipmentCode = Equipment.EquipmentCode
  
                            left join master_config_material_doctype DocType
                              ON Equipment.SID = DocType.SID
                              --AND Equipment.CompanyCode = DocType.Companycode
                              AND Equipment.EquipmentType = DocType.MaterialGroupCode
  
                            left join master_equipment_general ge
                              on ge.sid = Equipment.sid 
                              and ge.companyCode = Equipment.companycode
                              and Equipment.EquipmentCode = ge.EquipmentCode
                            LEFT JOIN ERPW_OWNER_GROUP AS [Owner] ON ge.SID = Owner.SID and ge.CompanyCode = Owner.CompanyCode
	                            AND ge.EquipmentObjectType = Owner.OwnerGroupCode
                            LEFT JOIN master_equipment_class class ON class.SID = ge.SID
	                            AND class.ClassCode = ge.EquipmentClass
                            where owner_a.SID = '" + SID + @"' 
                              and owner_a.CompanyCode = '" + CompanyCode + @"'
                              and owner_a.OwnerType = '" + customerType + @"'
                              AND owner_a.OwnerCode = '" + customerCode + @"'"; ;
                            

           return dbService.selectDataFocusone(sql);

        }
        #endregion

        #region CI_Class

        [Serializable]
        public class CI_Item
        {
            public string SID { get; set; }
            public string CIBatch { get; set; }
            public string SOType { get; set; }
            public string SONumber { get; set; }
            public string SoItemNo { get; set; }
            public string ShipToCode { get; set; }
            public string ShipToName { get; set; }
            public string MaterialCode { get; set; }
            public string MeterialName { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Status { get; set; }
            public string RequestType { get; set; }
            public string Fiscalyear { get; set; }

        }
        #endregion

        #region CI Fimily
        public DataTable getConfigurationItemFamily(String sid, String CompanyCode)
        {
            return getMaterialGroupNR(sid, CompanyCode, "EQ");
        }

        public void AddConfigurationItemFamily(string SID, string CompanyCode, string EmployeeCode, string sessionID,
            string CIGroupCode, string CIGroupDescription, string Prefix, string NR_Start, string NR_End,
            bool External, bool FreeDefine)
        {
            AddDataMaterialGroupNR(
                SID, CompanyCode, EmployeeCode, sessionID,
                CIGroupCode, CIGroupDescription, Prefix, NR_Start, NR_End,
                External, FreeDefine, "EQ"
             );
        }

        public void UpdateConfigurationItemFamily(string SID, string CompanyCode, string EmployeeCode, string sessionID,
            string CIGroupCode, string CIGroupDescription, string Prefix, string NR_Start, string NR_End,
            bool External, bool FreeDefine)
        {

            UpdateDataMaterialGroupNR(
                SID, CompanyCode, EmployeeCode, sessionID,
                CIGroupCode, CIGroupDescription, Prefix, NR_Start, NR_End,
                External, FreeDefine, "EQ"
             );
        }

        public void RemoveConfigurationItemFamily(string SID, string CompanyCode, string sessionID, string CIGroupCode)
        {
            RemoveDataMaterialGroupNR(SID, CompanyCode, sessionID, CIGroupCode);
        }


        private DataTable getMaterialGroupNR(String sid, String CompanyCode, String BusinessObject)
        {
            string sql = @"select A.MaterialGroupCode,A.Description,B.NumberRangeCode,D.PrefixCode
                                , D.xStart,D.xEnd,D.SuffixCode,D.xCurrent
                                , B.PostingType,D.ExternalOrNot,D.FreeDefine
                            from master_config_material_doctype as A  WITH (NOLOCK) 
                            left join master_config_materrial_doctype_docdetail as B  WITH (NOLOCK) 
                                on A.SID = B.SID 
                                and (A.CompanyCode = B.companyCode or B.CompanyCode ='*' or B.CompanyCode ='" + CompanyCode + @"')
                                and A.MaterialGroupCode = B.MaterialGroupCode 
                            left join master_config_material_nr_mapping as C  WITH (NOLOCK) 
                                on B.SID = C.SID 
                                and B.MaterialGroupCode = C.MaterialGroupCode 
                                and B.NumberRangeCode = C.NumberRangeCode 
                                and (B.companyCode = C.CompanyCode or C.CompanyCode ='*' or C.CompanyCode ='" + CompanyCode + @"')
                            left join master_config_material_nr as D  WITH (NOLOCK) 
                                on C.SID = D.SID 
                                and C.NumberRangeCode = D.NumberRangeCode
                                and (C.CompanyCode = D.CompanyCode  or D.CompanyCode ='*' or D.CompanyCode ='" + CompanyCode + @"')
                            where A.SID='" + sid + "' ";

            sql += " and (A.CompanyCode ='*' OR A.CompanyCode = '" + CompanyCode + "') ";
            if (!String.IsNullOrEmpty(BusinessObject))
                sql += " and B.PostingType='" + BusinessObject + "' ";
            sql += " order by A.MaterialGroupCode ";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        private void AddDataMaterialGroupNR(string SID, string CompanyCode, string EmployeeCode, string sessionID,
            string MaterialGroupCode, string Description, string Prefix, string NR_Start, string NR_End,
            bool External, bool FreeDefine, string PostingType)
        {
            MaterialGroupMainBean DSMaterialGroup = getMaterialGroupMainBean(SID, CompanyCode, sessionID);
            DataRow dr1 = DSMaterialGroup.master_config_material_doctype.NewRow();
            //string PostingType = "EQ";

            dr1["SID"] = SID;
            dr1["CompanyCode"] = CompanyCode;
            dr1["MaterialGroupCode"] = MaterialGroupCode;
            dr1["Description"] = Description;
            dr1["CREATED_BY"] = EmployeeCode;
            dr1["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            DSMaterialGroup.master_config_material_doctype.Rows.Add(dr1);

            DataRow dr2 = DSMaterialGroup.master_config_materrial_doctype_docdetail.NewRow();
            dr2["SID"] = SID;
            dr2["companyCode"] = CompanyCode;
            dr2["MaterialGroupCode"] = MaterialGroupCode;
            dr2["NumberRangeCode"] = MaterialGroupCode;
            dr2["PostingType"] = PostingType;//_tb_BO.Text;
            dr2["CREATED_BY"] = EmployeeCode;
            dr2["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr2["RequireSaleData"] = "False";
            dr2["RequirePurchaseData"] = "False";
            dr2["RequireWarehouseData"] = "False";

            DSMaterialGroup.master_config_materrial_doctype_docdetail.Rows.Add(dr2);

            DataRow dr3 = DSMaterialGroup.master_config_material_nr.NewRow();
            dr3["SID"] = SID;
            dr3["CompanyCode"] = CompanyCode;
            dr3["NumberRangeCode"] = MaterialGroupCode;
            dr3["Year"] = "*";
            dr3["ExternalOrNot"] = External.ToString();
            dr3["FreeDefine"] = FreeDefine.ToString();
            dr3["xStart"] = NR_Start;
            dr3["xEnd"] = NR_End;
            dr3["xCurrent"] = "";
            dr3["PrefixCode"] = Prefix;
            dr3["SuffixCode"] = "";
            dr3["CREATED_BY"] = EmployeeCode;
            dr3["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            DSMaterialGroup.master_config_material_nr.Rows.Add(dr3);

            DataRow dr4 = DSMaterialGroup.master_config_material_nr_mapping.NewRow();
            dr4["SID"] = SID;
            dr4["CompanyCode"] = CompanyCode;
            dr4["MaterialGroupCode"] = MaterialGroupCode;
            dr4["NumberRangeCode"] = MaterialGroupCode;
            dr4["Description"] = Description;
            dr4["CREATED_BY"] = EmployeeCode;
            dr4["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            DSMaterialGroup.master_config_material_nr_mapping.Rows.Add(dr4);

            foreach (DataRow dr in DSMaterialGroup.master_config_material_doctype.Rows)
            {
                string State = dr.RowState.ToString();
            }

            Object[] objParam = new Object[] { "LT00152", sessionID, CompanyCode, "False"  };
            DataSet[] objDataSet = new DataSet[] { DSMaterialGroup };
            Object objReturn = ICMService.ICMPrimitiveInvoke(objParam, objDataSet);
        }

        private void UpdateDataMaterialGroupNR(string SID, string CompanyCode, string EmployeeCode, string sessionID,
            string MaterialGroupCode, string Description, string Prefix, string NR_Start, string NR_End,
            bool External, bool FreeDefine, string PostingType)
        {
            MaterialGroupMainBean DSMaterialGroup = getMaterialGroupMainBean(SID, CompanyCode, sessionID);

            DataRow[] dr1 = DSMaterialGroup.master_config_material_doctype.Select("MaterialGroupCode = '" + MaterialGroupCode + "'");
            if (dr1.Length > 0)
            {
                DataRow dr = dr1[0];
                dr["Description"] = Description;
                dr["UPDATED_BY"] = EmployeeCode;
                dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            }

            DataRow[] dr2 = DSMaterialGroup.master_config_materrial_doctype_docdetail.Select("MaterialGroupCode = '" + MaterialGroupCode + "'");
            if (dr2.Length > 0)
            {
                DataRow dr = dr2[0];
                dr["PostingType"] = PostingType;
                dr["UPDATED_BY"] = EmployeeCode;
                dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
                //dr["RequireSaleData"] = item.RequireSaleData == "True" ? "True" : "False";
                //dr["RequirePurchaseData"] = item.RequirePurchaseData == "True" ? "True" : "False";
                //dr["RequireWarehouseData"] = item.RequireWarehouseData == "True" ? "True" : "False";
            }

            DataRow[] dr3 = DSMaterialGroup.master_config_material_nr_mapping.Select("MaterialGroupCode = '" + MaterialGroupCode + "'");
            if (dr3.Length > 0)
            {
                DataRow dr = dr3[0];
                dr["UPDATED_BY"] = EmployeeCode;
                dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            }

            DataRow[] dr4 = DSMaterialGroup.master_config_material_nr.Select("NumberRangeCode = '" + MaterialGroupCode + "'");
            if (dr4.Length > 0)
            {
                DataRow dr = dr4[0];
                dr["xStart"] = NR_Start;
                dr["xEnd"] = NR_End;
                dr["PrefixCode"] = Prefix;
                //dr["SuffixCode"] = item.Suffix;
                //dr["xCurrent"] = item.Current;
                dr["ExternalOrNot"] = External.ToString();
                dr["FreeDefine"] = FreeDefine.ToString();
                dr["UPDATED_BY"] = EmployeeCode;
                dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            }

            Object[] objParam = new Object[] { "LT00152", sessionID, CompanyCode, "False" };
            DataSet[] objDataSet = new DataSet[] { DSMaterialGroup };
            Object objReturn = ICMService.ICMPrimitiveInvoke(objParam, objDataSet);


        }

        private void RemoveDataMaterialGroupNR(string SID, string CompanyCode, string sessionID, string MaterialGroupCode)
        {
            MaterialGroupMainBean DSMaterialGroup = getMaterialGroupMainBean(SID, CompanyCode, sessionID);

            DataRow[] dr1 = DSMaterialGroup.master_config_material_doctype.Select("MaterialGroupCode = '" + MaterialGroupCode + "'");
            if (dr1.Length > 0)
            {
                dr1[0].Delete();
            }
            DataRow[] dr2 = DSMaterialGroup.master_config_materrial_doctype_docdetail.Select("MaterialGroupCode = '" + MaterialGroupCode + "'");
            if (dr2.Length > 0)
            {
                dr2[0].Delete();
            }
            DataRow[] dr3 = DSMaterialGroup.master_config_material_nr_mapping.Select("MaterialGroupCode = '" + MaterialGroupCode + "'");
            if (dr3.Length > 0)
            {
                dr3[0].Delete();
            }
            DataRow[] dr4 = DSMaterialGroup.master_config_material_nr.Select("NumberRangeCode = '" + MaterialGroupCode + "'");
            if (dr4.Length > 0)
            {
                dr4[0].Delete();
            }

            Object[] objParam = new Object[] { "LT00152", sessionID, CompanyCode, "False" };
            DataSet[] objDataSet = new DataSet[] { DSMaterialGroup };
            Object objReturn = ICMService.ICMPrimitiveInvoke(objParam, objDataSet);
        }

        private MaterialGroupMainBean getMaterialGroupMainBean(string SID, string CompanyCode, string sessionID)
        {
            MaterialGroupMainBean DSMaterialGroup = new MaterialGroupMainBean();

            Object[] objParam = new Object[] { "LT00151", sessionID, CompanyCode };
            DataSet[] objDataSet = new DataSet[] { DSMaterialGroup };
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            if (objReturn != null)
            {
                DSMaterialGroup.Clear();
                DSMaterialGroup.Merge(objReturn);
            }

            return DSMaterialGroup;
        }


        #endregion
    }
}