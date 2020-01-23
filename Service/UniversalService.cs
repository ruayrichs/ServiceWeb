using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class UniversalService
    {
        DBService dbService = new DBService();

        public DataTable getEquipmentType(string SID, string Companycode)
        {
//            string sql = @"select * 
//                            from master_config_material_doctype
//                            where SID = '" + SID + @"' 
//                            AND (Companycode = '" + Companycode + @"' or Companycode = '*')";

            return new EquipmentService().getConfigurationItemFamily(SID, Companycode);
        }

        public bool isAutoNumberRefEquipmentType(string SID, string CompanyCode, string NumberRangeCode)
        {
            string sql = @"SELECT ExternalOrNot,FreeDefine from master_config_material_nr
                            where SID='" + SID + @"' 
                            and (CompanyCode='" + CompanyCode + @"' or CompanyCode='*')
                            and NumberRangeCode='" + NumberRangeCode + @"'
                            and ExternalOrNot = 'False' and FreeDefine='False' ";
            DataTable dt = dbService.selectDataFocusone(sql);
            if (dt.Rows.Count <= 0)
            {
                return false;
            }
            return true;
        }

        public void MappingEquipment(string SID, string WorkgroupCode, string StructureCode,
            string EquipmentCode, string CustomerCode, string TierCode, string Created_By)
        {
            string Created_On = Validation.getCurrentServerStringDateTime();
            string sql = @"insert into CRM_Equipment_Mapping_Customer
                            (
                               SID
                              ,WorkgroupCode
                              ,StructureCode
                              ,EquipmentCode
                              ,CustomerCode
                              ,TierCode
                              ,Created_On
                              ,Created_By
                            ) VALUES (
                               '" + SID + @"'
                              ,'" + WorkgroupCode + @"'
                              ,'" + StructureCode + @"'
                              ,'" + EquipmentCode + @"'
                              ,'" + CustomerCode + @"'
                              ,'" + TierCode + @"'
                              ,'" + Created_On + @"'
                              ,'" + Created_By + @"'
                            )";

            dbService.executeSQLForFocusone(sql);
        }

        public DataTable getTierMaster(string SID, string WorkGroupCode)
        {
            string sql = @"select * from Link_Tier_Master 
                            where SID = '" + SID + @"' 
                              AND WorkGroupCode = '" + WorkGroupCode + @"'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getEquipmentbyCustomerCode(string SID, string WorkgroupCode,
            string CustomerCode)
        {
            return getEquipmentbyCustomerCode(SID, WorkgroupCode, CustomerCode, "");
        }
        public DataTable getEquipmentbyCustomerCode(string SID, string WorkgroupCode,
            string CustomerCode, string StructureCode)
        {
            string sql = @"select Mapping.WorkgroupCode, Mapping.EquipmentCode
                              , Mapping.CustomerCode, Mapping.TierCode , Tier.TierName
                              , equipment.[Description], equipment.EquipmentType
                            from CRM_Equipment_Mapping_Customer Mapping
                            left join master_equipment equipment
                              on Mapping.SID = equipment.SID
                              and Mapping.EquipmentCode = equipment.EquipmentCode
                            left join Link_Tier_Master Tier
                              on Tier.SID = equipment.SID
                              and Tier.TierCode = Mapping.TierCode

                            where Mapping.SID = '" + SID + @"'
                              AND Mapping.WorkgroupCode = '" + WorkgroupCode + @"' 
                              AND Mapping.CustomerCode = '" + CustomerCode + @"'";

            if (!string.IsNullOrEmpty(StructureCode))
            {
                sql += " AND Mapping.StructureCode = '" + StructureCode + @"'";
            }

            DataTable dt = dbService.selectDataFocusone(sql);
            dt.Columns.Add("CodeWithDesc", typeof(System.String));

            foreach (DataRow dr in dt.Rows)
            {
                string desc = dr["EquipmentCode"].ToString();

                if (!string.IsNullOrEmpty(dr["Description"].ToString()))
                {
                    desc += desc == "" ? "" : " : ";
                    desc += dr["Description"].ToString();
                }

                dr["CodeWithDesc"] = desc;
            }

            return dt;
        }

        public DataTable GetListEquipmentForMappingCustomer(string SID, string CompanyCode, string WorkgroupCode,
            string CustomerCode, string EquipmentCode, string EquipmentName, string EquipmentType)
        {
            string sql = @"select Equipment.*, DocType.[Description] as EquipmentTypeName
                           from master_equipment Equipment
                           inner join master_config_material_doctype DocType
                              ON Equipment.SID = DocType.SID
                              --AND Equipment.CompanyCode = DocType.Companycode
                              AND Equipment.EquipmentType = DocType.MaterialGroupCode
                           where Equipment.SID = '" + SID + @"'
                              AND Equipment.CompanyCode = '" + CompanyCode + "'" +
                              (string.IsNullOrEmpty(EquipmentCode) ? "" : " AND Equipment.EquipmentCode = '" + EquipmentCode + "'") +
                              (string.IsNullOrEmpty(EquipmentName) ? "" : " AND Equipment.[Description] like '%" + EquipmentName + "%'") +
                              (string.IsNullOrEmpty(EquipmentType) ? "" : " AND Equipment.EquipmentType = '" + EquipmentType + "'") +
                              @"AND Equipment.EquipmentCode not in 
                              (
                                select EquipmentCode
                                from CRM_Equipment_Mapping_Customer
                                where SID = '" + SID + @"'
                                AND WorkgroupCode = '" + WorkgroupCode + @"'
                                AND CustomerCode = '" + CustomerCode + @"'
                              )";

            return dbService.selectDataFocusone(sql);
        }

        public string getStrucyureCodeByProjectCode(string SID, string WorkGroupCode, string ProjectCode)
        {
            string StructureCode = "";
            string sql = @"select * from LINK_PROJECT_COMPANY_STRUCTURE_MAPPING_UNIVERSAL_SERVICE where SID='" + SID + "' and WorkGroupCode='" + WorkGroupCode + "' and ProjectCode='" + ProjectCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                StructureCode = dt.Rows[0]["StructureCode"].ToString();
            }

            return StructureCode;
        }

        public DataTable getListEquipmentExcludeByCustomer(string SID, string CompanyCode,
            string WorkgroupCode, string CustomerCode, string StructureCode)
        {
            string sql = @"select equipment.SID, equipment.CompanyCode, equipment.EquipmentCode
                              , equipment.CategoryCode, equipment.[Description]
                              , equipment.Valid_From, equipment.Valid_To

                            from master_equipment equipment
                            left join CRM_Equipment_Mapping_Customer Mapping
                              on equipment.EquipmentCode = Mapping.EquipmentCode
                              and Mapping.CustomerCode = '" + CustomerCode + @"' 
                              AND Mapping.WorkgroupCode = '" + WorkgroupCode + @"' 
                            where equipment.SID = '" + SID + @"' 
                              AND equipment.CompanyCode = '" + CompanyCode + @"'
                              AND Mapping.StructureCode = '" + StructureCode + @"'
                              AND Mapping.CustomerCode is null";

            return dbService.selectDataFocusone(sql);
        }

        #region Equipment Mapping Contact
        public DataTable getEquipmentMappingContact(string SID, string CompanyCode, string WorkGroupCode,
            string ProjectCode, string CustomerCode, string EquipmentCode)
        {
            string sql = @"select Mapping.*
                              , ConDetail.ITEMNO, ConDetail.NAME1, ConDetail.NAME2, ConDetail.POSITION
                              , ConDetail.REMARK1, ConDetail.REMARK2, ConDetail.BOBJECTLINK
                              , ConInfo.person_height, ConInfo.person_weight, ConInfo.facebook
                              , ConInfo.instagram, ConInfo.tweeter, ConInfo.LineID
                              , TypeMaster.ContactTypeName, Phone.PHONENUMBER, Email.EMAIL
                              , ISNULL(contact_address.address,'ไม่ระบุที่อยู่') as address  
                              ,contact_phone.phone as contact_phone,contact_email.EMAIL as contact_email

                            from Link_Equipment_Mapping_Contact Mapping
                            left join CONTACT_DETAILS ConDetail
                              on ConDetail.BOBJECTLINK = Mapping.ContactCode
                              and ConDetail.SID = Mapping.SID
                            left join CONTACT_DETAILS_OTHERINFO ConInfo
                                on ConDetail.BOBJECTLINK = ConInfo.BOBJECTLINK
                                and ConDetail.SID = ConInfo.SID
                            left join CONTACT_CRM_TYPE ConType
                                on ConType.BOBJECTLINK = ConDetail.BOBJECTLINK
                                and ConType.SID = ConDetail.SID
                            left join CONTACT_CRM_TYPE_MASTER TypeMaster
                                on TypeMaster.SID = ConType.SID
                                and TypeMaster.ContactTypeCode = ConType.ContactTypeCode
                            left join CONTACT_PHONE Phone
                                on Phone.BOBJECTLINK = ConDetail.BOBJECTLINK
                                and Phone.ITEMNO = ConDetail.ITEMNO
                                and Phone.SID = ConDetail.SID
                            left join CONTACT_EMAIL Email
                                on Email.BOBJECTLINK = ConDetail.BOBJECTLINK
                                and Email.ITEMNO = ConDetail.ITEMNO
                                and Email.SID = ConInfo.SID
                            left join (
                              select SID,AddressCode,CustomerCode
                              ,STUFF( 
                                (select ' ' + prop.Description + ' : ' + c_address.xValue from master_customer_address c_address
                                  inner join master_conf_properties prop on prop.SID = c_address.SID and prop.PropertiesCode = c_address.PropertiesCode
                                  and prop.xType='ADDRESS' 
                                  where c_address.SID = a.SID and c_address.CustomerCode = a.CustomerCode
                                  and c_address.AddressCode = a.AddressCode
                                  order by c_address.AddressCode,c_address.PropertiesCode
                                  FOR XML PATH('')) ,1,1,'') as address

                              from master_customer_address a 
                              WHERE AddressCode = '001'
                              group by sid, AddressCode,CustomerCode
                            ) contact_address 
                              on contact_address.sid = Mapping.SID
                              and contact_address.CustomerCode = Mapping.CustomerCode
                            left join (
                                SELECT  sid, BOBJECTLINK
                                , STUFF(
                                    (SELECT ', ' + isnull(PHONENUMBER,'') + isnull(EXT,'')   
                                     FROM CONTACT_PHONE b
                                     where a.BOBJECTLINK = b.BOBJECTLINK
                                     FOR XML PATH(''))  ,1,1,'') as phone

                                FROM CONTACT_PHONE a 
                                 group by  sid, BOBJECTLINK
                            ) contact_phone 
                              on contact_phone.sid = Mapping.sid 
                              and  contact_phone.BOBJECTLINK = ConDetail.BOBJECTLINK
                            left join (
                                 SELECT  sid, BOBJECTLINK
                                 , STUFF(
                                    (SELECT ', ' + isnull(EMAIL, '')
                                     FROM  CONTACT_EMAIL b
                                     where a.BOBJECTLINK = b.BOBJECTLINK
                                     FOR XML PATH(''))  ,1,1,'') as EMAIL

                                 FROM  CONTACT_EMAIL a                                 
                                 group by sid, BOBJECTLINK
                            ) contact_email 
                              on  contact_email.sid = Mapping.sid 
                              and  contact_email.BOBJECTLINK = ConDetail.BOBJECTLINK

                            where Mapping.SID = '" + SID + @"' 
                              AND Mapping.CompanyCode = '" + CompanyCode + @"' 
                              AND Mapping.WorkGroupCode = '" + WorkGroupCode + @"' 
                              AND Mapping.CustomerCode = '" + CustomerCode + @"' 
                              AND Mapping.EquipmentCode = '" + EquipmentCode + @"'";

            if (!string.IsNullOrEmpty(ProjectCode))
            {
                sql += " AND Mapping.ProjectCode = '" + ProjectCode + @"' ";

            }

            return dbService.selectDataFocusone(sql);
        }

        public void insertEquipmentMappingContact(string SID, string CompanyCode, string WorkGroupCode,
            string ProjectCode, string CustomerCode, string EquipmentCode, string ContactCode,
            string Remark, string Created_on)
        {
            string sqlValidate = @"select * from Link_Equipment_Mapping_Contact
                                    where SID = '" + SID + @"' 
                                      AND CompanyCode = '" + CompanyCode + @"' 
                                      AND WorkGroupCode = '" + WorkGroupCode + @"' 
                                      AND ProjectCode = '" + ProjectCode + @"' 
                                      AND CustomerCode = '" + CustomerCode + @"' 
                                      AND EquipmentCode = '" + EquipmentCode + @"' 
                                      AND ContactCode = '" + ContactCode + @"'";
            DataTable dt = dbService.selectDataFocusone(sqlValidate);
            if (dt.Rows.Count > 0)
            {
                throw new Exception("มีการผูกกับผู้ติดต่อรายนี้แล้ว");
            }

            string sql = @"insert into Link_Equipment_Mapping_Contact (
                           SID
                          ,CompanyCode
                          ,WorkGroupCode
                          ,ProjectCode
                          ,CustomerCode
                          ,EquipmentCode
                          ,ContactCode
                          ,Remark
                          ,Created_on
                          ,Created_by
                        ) VALUES (
                           '" + SID + @"'
                          ,'" + CompanyCode + @"'
                          ,'" + WorkGroupCode + @"'
                          ,'" + ProjectCode + @"'
                          ,'" + CustomerCode + @"'
                          ,'" + EquipmentCode + @"'
                          ,'" + ContactCode + @"'
                          ,'" + Remark + @"'
                          ,'" + Created_on + @"'
                          ,'" + Validation.getCurrentServerStringDateTime() + @"'
                        )";

            dbService.executeSQLForFocusone(sql);
        }

        public void deleteEquipmentMappingContact(string SID, string CompanyCode, string WorkGroupCode,
            string ProjectCode, string CustomerCode, string EquipmentCode, string ContactCode)
        {
            string sql = @"delete from Link_Equipment_Mapping_Contact 
                            WHERE SID = '" + SID + @"'
                              AND CompanyCode = '" + CompanyCode + @"'
                              AND WorkGroupCode = '" + WorkGroupCode + @"'
                              AND ProjectCode = '" + ProjectCode + @"'
                              AND CustomerCode = '" + CustomerCode + @"'
                              AND EquipmentCode = '" + EquipmentCode + @"'
                              AND ContactCode = '" + ContactCode + @"'";

            dbService.executeSQLForFocusone(sql);
        }
        #endregion 

        #region Service Contract

        public DataTable getContractNumberbyEquipmentCode(string SID, string CompanyCode, string EquipmentNo, string EquipmentType)
        {
            string sql = @"select top 1 b.ContractNo,b.ContractPerson,b.PhoneNo,b.Description,b.StartDate,b.EndDate,b.DocDate
                              ,c.Owner, emp.FirstName_TH + ' ' + emp.LastName_TH as FullName,c.Remark,a.Astatus,a.Xstatus,a.Hstatus
                              ,case when(a.Xstatus='T') THEN 'Terminate'  
                                 when(a.Astatus='I') THEN 'InActive' 
                                 when(a.Hstatus='H') THEN 'Hold' 
                                 ELSE 'Open'
                               END as StateStatus
                               ,case when(a.Xstatus='T') THEN '#E65041'  
                                 when(a.Astatus='I') THEN '#fdf59a' 
                                 when(a.Hstatus='H') THEN '#F1C513' 
                                 ELSE '#000'
                               END as StateStatusColor
                              ,coverage.Monday,coverage.Tuesday,coverage.Wenesday,coverage.Thursday 
                              ,coverage.Friday,coverage.Saturday,coverage.Sunday
                              ,coverage.MondayStartTime,coverage.MondayEndTime
                              ,coverage.TuesdayStartTime,coverage.TuesdayEndTime
                              ,coverage.WenesdayStartTime,coverage.WenesdayEndTime
                              ,coverage.ThursdayStartTime,coverage.ThursdayEndTime
                              ,coverage.FridayStartTime,coverage.FridayEndTime
                              ,coverage.SaturdayStartTime,coverage.SaturdayEndTime
                              ,coverage.SundayStartTime,coverage.SundayEndTime
                              ,coverage.Parts,coverage.Labour,coverage.Travel,coverage.IncludingHolidays
                            from master_customer_service_contract_item a 
                            inner join master_customer_service_contract b
                              on a.sid = b.SID 
                              and a.ContractNo = b.ContractNo 
                              and a.DocumentType = b.DocumentType
                              and a.CustomerCode = b.CustomerCode 
                              and a.Fiscalyear = b.Fiscalyear
                            left outer join master_customer_service_contract_general c
                              on a.sid = c.SID 
                              and a.ContractNo = c.ContractNo
                              and a.DocumentType = c.DocumentType
                              and a.CustomerCode = c.CustomerCode 
                              and a.Fiscalyear = c.Fiscalyear
                            left outer join master_customer_service_contract_converange coverage
                              on a.sid = coverage.SID 
                              and a.ContractNo = coverage.ContractNo 
                              and a.DocumentType = coverage.DocumentType
                              and a.CustomerCode = coverage.CustomerCode 
                              and a.Fiscalyear = coverage.Fiscalyear
                            left join master_employee emp
                              on emp.CompanyCode = a.CompanyCode
                              and emp.SID = a.SID
                              and emp.EmployeeCode = c.[Owner]

                            where a.SID='" + SID + @"' 
                              and a.CompanyCode='" + CompanyCode + @"'
                              and a.EquipmentNo='" + EquipmentNo + @"'
                              and a.EquipmentType='" + EquipmentType + @"'";

            return dbService.selectDataFocusone(sql);
        }

        #endregion 

        public void UpdateRemarkContact(string SID, string CompanyCode, string WorkGroupCode,
            string ProjectCode, string CustomerCode, string EquipmentCode, string ContactCode, string Remark)
        {
            string sql = @"update Link_Equipment_Mapping_Contact 
                            SET
                              Remark = '" + Remark + @"'
                            WHERE SID = '" + SID + @"'
                              AND CompanyCode = '" + CompanyCode + @"'
                              AND WorkGroupCode = '" + WorkGroupCode + @"'
                              AND ProjectCode = '" + ProjectCode + @"'
                              AND CustomerCode = '" + CustomerCode + @"'
                              AND EquipmentCode = '" + EquipmentCode + @"'
                              AND ContactCode = '" + ContactCode + @"'";

            dbService.executeSQLForFocusone(sql);
        }

        public DataTable GetEquipmentCustomerAssignment(string sid, string companyCode, string equipmentCode, string customerCode)
        {
            string sql = @"SELECT a.EquipmentCode
	                            ,a.Description AS EquipmentName
	                            ,a.EquipmentType
	                            ,b.OwnerCode
	                            ,c.CustomerName
	                            ,c.ForeignName	
                            FROM master_equipment a
                            INNER JOIN master_equipment_owner_assignment b ON a.SID = b.SID
	                            AND a.CompanyCode = b.CompanyCode
	                            AND a.EquipmentCode = b.EquipmentCode
                            INNER JOIN master_customer c ON b.SID = c.SID
                            	AND b.CompanyCode = c.CompanyCode
	                            AND b.OwnerCode = c.CustomerCode
                            WHERE a.SID = '"+ sid + @"'
	                            AND a.CompanyCode = '" + companyCode + @"'
	                            AND b.OwnerType = '01'
	                            AND b.ActiveStatus = 'True'";

            if (!string.IsNullOrEmpty(equipmentCode))
            {
                sql += " AND a.EquipmentCode = '" + equipmentCode + "'";
            }

            if (!string.IsNullOrEmpty(customerCode))
            {
                sql += " AND b.OwnerCode = '" + customerCode + "'";
            }

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }        

        public bool CheckOpenTicket(string sid, string companyCode, string docType, string customerCode, string equipmentCode)
        {
            string sql = @"SELECT a.CallerID
                           FROM cs_servicecall_header a
                           INNER JOIN cs_servicecall_item b ON a.SID = b.SID
	                           AND a.CompanyCode = b.CompanyCode
	                           AND a.ObjectID = b.ObjectID
                           WHERE a.SID = '" + sid + @"'
	                           AND a.CompanyCode = '" + companyCode + @"'
	                           AND a.Doctype = '" + docType + @"'
	                           AND a.CustomerCode = '" + customerCode + @"'
	                           AND a.CallStatus = '01'";

            if (!string.IsNullOrEmpty(equipmentCode))
            {
                sql += " AND b.EquipmentNo = '" + equipmentCode + "'";
            }

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt.Rows.Count > 0;
        }

        public void CheckDoctypeCreateTicket(string sid, string companyCode, string docType)
        {
            DateTime dateTime = DateTime.Now;
            string docYear = ServiceLibrary.LookUpTable(
                "Year", 
                "master_config_cs_nr",
                "where SID = '" + sid + "' AND NumberRangeCode = '" + docType + "' AND CompanyCode = '" + companyCode + "' AND Year = '" + dateTime.Year.ToString() + "'"
            );

            if (string.IsNullOrEmpty(docYear))
            {
                List<string> listSql = new List<string>();
                
                listSql.Add(
                    @"
                    insert into master_config_cs_nr (
                       SID
                      ,NumberRangeCode
                      ,CompanyCode
                      ,Year
                      ,xStart
                      ,xEnd
                      ,xCurrent
                      ,ExternalOrNot
                      ,CREATED_BY
                      ,UPDATED_BY
                      ,CREATED_ON
                      ,UPDATED_ON
                      ,FreeDefine
                      ,PrefixCode
                      ,SuffixCode
                    ) SELECT 
                       SID
                      ,NumberRangeCode
                      ,CompanyCode
                      ,'" + dateTime.Year.ToString() + @"'  AS [Year]
                      ,REPLACE(xCurrent, PrefixCode , '') AS xStart
                      ,xEnd
                      ,xCurrent
                      ,ExternalOrNot
                      ,CREATED_BY
                      ,UPDATED_BY
                      ,'" + dateTime.Year.ToString() + @"0101000000'  AS CREATED_ON
                      ,'" + dateTime.Year.ToString() + @"0101000000'  AS UPDATED_ON
                      ,FreeDefine
                      ,PrefixCode
                      ,SuffixCode
                    FROM master_config_cs_nr
                    where SID = '" + sid + @"' 
                      AND CompanyCode = '" + companyCode + @"' 
                      AND NumberRangeCode = '" + docType + @"' 
                      AND [Year] = '" + (dateTime.Year - 1).ToString() + @"'
                    "
                );

                listSql.Add(
                    @"
                    insert into master_config_cs_nr_detail (
                       SID
                      ,Year
                      ,CompanyCode
                      ,NumberRangeCode
                      ,prefixcode
                      ,xStart
                      ,xEnd
                      ,xCurrent
                      ,startdate
                      ,xActive
                      ,CREATED_BY
                      ,CREATED_ON
                      ,UPDATED_BY
                      ,UPDATED_ON
                      ,SuffixCode
                      ,xRunning
                    ) SELECT 
                       SID            
                      ,'" + dateTime.Year.ToString() + @"'  AS [Year]     
                      ,CompanyCode    
                      ,NumberRangeCode
                      ,prefixcode     
                      ,xRunning AS xStart    
                      ,xEnd          
                      ,xCurrent      
                      ,'" + dateTime.Year.ToString() + @"0101' AS startdate     
                      ,xActive       
                      ,CREATED_BY    
                      ,'" + dateTime.Year.ToString() + @"0101000000' AS CREATED_ON    
                      ,UPDATED_BY    
                      ,'" + dateTime.Year.ToString() + @"0101000000' AS UPDATED_ON    
                      ,SuffixCode    
                      ,xRunning      
                    FROM master_config_cs_nr_detail
                    where SID = '" + sid + @"'
                      AND CompanyCode = '" + companyCode + @"'
                      AND NumberRangeCode = '" + docType + @"'
                      AND [Year] = '" + (dateTime.Year - 1).ToString() + @"'
                    "
                );

                dbService.executeSQLForFocusone(listSql);
            }
        }
    }
}