using agape.lib.web.configuration.utils;
using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class CRMAccountabilityService
    {
        DBService db = new DBService();

        public void DeleteParticipantsByCompanyStructureCode(string SID, string CompanyCode,
           string CompanyStructureCode, string WorkGroupCode)
        {
            List<string> sqllist = new List<string>();
            string sqlMaster = "";
            string sqlDetail = "";

            string sqlSelect = @"select * from LINK_ACCOUNTABILITY_PARTICIPANTS_MASTER 
                            where SID = '" + SID + @"' 
                             AND CompanyCode = '" + CompanyCode + @"' 
                             and WorkGroupCode = '" + WorkGroupCode + @"'
                             AND CompanyStructureCode = '" + CompanyStructureCode + "'";

            DataTable dt = db.selectDataFocusone(sqlSelect);

            if (dt.Rows.Count > 0)
            {
                sqlMaster = @"delete from LINK_ACCOUNTABILITY_PARTICIPANTS_MASTER 
                            WHERE SID = '" + SID + @"'
                              AND CompanyCode = '" + CompanyCode + @"' 
                              and WorkGroupCode = '" + WorkGroupCode + @"'
                              AND CompanyStructureCode = '" + CompanyStructureCode + @"'";
                foreach (DataRow dr in dt.Rows)
                {
                    sqlDetail = @"delete from LINK_ACCOUNTABILITY_PARTICIPANTS_MASTER_DETAIL 
                            WHERE SID = '" + SID + @"'
                                AND CompanyCode = '" + CompanyCode + @"' 
                                and WorkGroupCode = '" + WorkGroupCode + @"'
                                AND ParticipantsCode = '" + dr["ParticipantsCode"].ToString() + "'";
                    sqllist.Add(sqlDetail);
                }
                sqllist.Add(sqlMaster);

                db.executeSQLForFocusone(sqllist);
            }
        }

        public void DeleteInitiativeOwner(string SID, string CompanyCode, string StructureCode, string EmpCode, string WorkGroupCode)
        {
            string sql = @"delete from LINK_ACCOUNTABILITY_INITIATIVE_OWNER_MASTER 
                        WHERE SID = '" + SID + @"'
                          AND CompanyCode = '" + CompanyCode + @"' 
                          and WorkGroupCode = '" + WorkGroupCode + @"'
                          AND CompanyStructureCode = '" + StructureCode + @"'
                          " + (string.IsNullOrEmpty(EmpCode) ? "" : "AND EmployeeCode = '" + EmpCode + @"'");

            db.executeSQLForFocusone(sql);
        }

        public string getStructure(string SID, string StructureCode, string WorkGroupCode)
        {
            string structure = "";
            string sql = @"select * from LINK_PROJECT_COMPANY_STRUCTURE_ITEM 
                        where SID = '" + SID + @"' 
                        and WorkGroupCode = '" + WorkGroupCode + @"'
                        and StructureCode = '" + StructureCode + "'";
            DataTable dt = db.selectDataFocusone(sql);
            string DataStructure = "";
            if (dt.Rows.Count > 0)
            {
                DataStructure = dt.Rows[0]["StructureName"].ToString() + "/" + structure;

                if (!string.IsNullOrEmpty(dt.Rows[0]["NodeParentCode"].ToString()))
                {
                    structure += getStructure(SID, dt.Rows[0]["NodeParentCode"].ToString(), WorkGroupCode);
                }
                DataStructure = structure + DataStructure;
            }

            return DataStructure;
        }

        public DataTable getDataInitiativeContact(string SID, string StructureCode, string WorkGroupCode, string ContactCurrent)
        {
            if (string.IsNullOrEmpty(StructureCode))
                return new DataTable();

            string sql = "";
            if (string.IsNullOrEmpty(ContactCurrent))
            {
                sql += @"select conmaster.BPCODE,initcon.ContactCode,initcon.CompanyStructureCode,contact.NAME1 + ' ' + contact.NAME2 as FullName
                            ,ISNULL(ctypemaster.ContactTypeName,'ไม่ระบุประเภท') as ContactTypeName
                            ,contact_email.EMAIL,contact_phone.phone
                            ,case when ISNULL(contact.POSITION, '') = '' then 'ไม่ระบุตำแหน่ง' else contact.POSITION END  as POSITION
                            ,OtherInfo.facebook, OtherInfo.instagram, OtherInfo.tweeter, OtherInfo.LineID
                            from LINK_ACCOUNTABILITY_INITIATIVE_CONTACT initcon
                            left outer join CONTACT_DETAILS contact
                            on initcon.SID = contact.SID and initcon.ContactCode = contact.BOBJECTLINK
                            left outer join CONTACT_MASTER conmaster on conmaster.SID = contact.SID and conmaster.AOBJECTLINK = contact.AOBJECTLINK
                            left join CONTACT_CRM_TYPE ctype
                            on contact.SID = ctype.sid 
                            and contact.BOBJECTLINK= ctype.BOBJECTLINK
                            left outer join CONTACT_CRM_TYPE_MASTER ctypemaster
                            on ctype.sid = ctypemaster.sid and ctype.ContacttypeCode = ctypemaster.ContacttypeCode
                            left join CONTACT_DETAILS_OTHERINFO OtherInfo
                            on OtherInfo.BOBJECTLINK = contact.BOBJECTLINK
                            and OtherInfo.SID = contact.SID

                            left join (
                                 SELECT  sid, BOBJECTLINK
                                 , STUFF(
                                    (SELECT ', ' + isnull(EMAIL, '')
                                     FROM  CONTACT_EMAIL b
                                     where a.BOBJECTLINK = b.BOBJECTLINK
                                     FOR XML PATH(''))  ,1,1,'') as EMAIL

                                 FROM  CONTACT_EMAIL a
                                 group by sid, BOBJECTLINK
                            ) contact_email on  contact_email.sid = contact.sid and  contact_email.BOBJECTLINK = contact.bobjectlink
                            left join (
                                SELECT  sid, BOBJECTLINK
                                , STUFF(
                                    (SELECT ', ' + isnull(PHONENUMBER,'') + isnull(EXT,'')   
                                     FROM CONTACT_PHONE b
                                     where a.BOBJECTLINK = b.BOBJECTLINK
                                     FOR XML PATH(''))  ,1,1,'') as phone

                                FROM CONTACT_PHONE a 
                                 group by  sid, BOBJECTLINK
                            ) contact_phone on  contact_phone.sid = contact.sid and  contact_phone.BOBJECTLINK = contact.bobjectlink
                            where initcon.SID = '" + SID + @"'
                            and initcon.WorkGroupCode = '" + WorkGroupCode + @"'";

                sql += " and initcon.CompanyStructureCode = '" + StructureCode + "' ";
            }

            else
            {
                string[] ContactCurrentArray = ContactCurrent.Split(',');

                string sqlIn = string.Join(",", ContactCurrentArray.Select(a => "'" + a + "'"));

                sql += @" select contact.BOBJECTLINK as ContactCode,contact.NAME1 + ' ' + contact.NAME2 as FullName,ISNULL(ctypemaster.ContactTypeName,'ไม่ระบุประเภท') as ContactTypeName
                        ,contact_email.EMAIL,contact_phone.phone
                        ,case when ISNULL(contact.POSITION, '') = '' then 'ไม่ระบุตำแหน่ง' else contact.POSITION END  as POSITION
                        ,OtherInfo.facebook, OtherInfo.instagram, OtherInfo.tweeter, OtherInfo.LineID from CONTACT_DETAILS contact                    
                        left join CONTACT_CRM_TYPE ctype
                        on contact.SID = ctype.sid 
                        and contact.BOBJECTLINK= ctype.BOBJECTLINK
                        left outer join CONTACT_CRM_TYPE_MASTER ctypemaster
                        on ctype.sid = ctypemaster.sid and ctype.ContacttypeCode = ctypemaster.ContacttypeCode
                        left join CONTACT_DETAILS_OTHERINFO OtherInfo
                        on OtherInfo.BOBJECTLINK = contact.BOBJECTLINK
                        and OtherInfo.SID = contact.SID
                        left join (
                                SELECT  sid, BOBJECTLINK
                                , STUFF(
                                (SELECT ', ' + isnull(EMAIL, '')
                                    FROM  CONTACT_EMAIL b
                                    where a.BOBJECTLINK = b.BOBJECTLINK
                                    FOR XML PATH(''))  ,1,1,'') as EMAIL

                                FROM  CONTACT_EMAIL a
                                group by sid, BOBJECTLINK
                        ) contact_email on  contact_email.sid = contact.sid and  contact_email.BOBJECTLINK = contact.bobjectlink
                        left join (
                            SELECT  sid, BOBJECTLINK
                            , STUFF(
                                (SELECT ', ' + isnull(PHONENUMBER,'') + isnull(EXT,'')   
                                    FROM CONTACT_PHONE b
                                    where a.BOBJECTLINK = b.BOBJECTLINK
                                    FOR XML PATH(''))  ,1,1,'') as phone

                            FROM CONTACT_PHONE a 
                                group by  sid, BOBJECTLINK
                        ) contact_phone on  contact_phone.sid = contact.sid and  contact_phone.BOBJECTLINK = contact.bobjectlink
                        where contact.SID='" + SID + "' and contact.BOBJECTLINK in(" + sqlIn +")";
            }

            return db.selectDataFocusone(sql);
        }

        public DataTable getDataInitiativeContactV2(string SNAID, string SID, string StructureCode, string WorkGroupCode)
        {
            if (string.IsNullOrEmpty(StructureCode))
                return new DataTable();
            
            string sql = @"select conmaster.BPCODE,initcon.ContactCode,initcon.CompanyStructureCode,contact.NAME1 + ' ' + contact.NAME2 as FullName
                        ,ISNULL(ctypemaster.ContactTypeName,'ไม่ระบุประเภท') as ContactTypeName
                        ,contact_email.EMAIL,contact_phone.phone
                        ,case when ISNULL(contact.POSITION, '') = '' then 'ไม่ระบุตำแหน่ง' else contact.POSITION END  as POSITION
                        ,OtherInfo.facebook, OtherInfo.instagram, OtherInfo.tweeter, OtherInfo.LineID
                        from LINK_ACCOUNTABILITY_INITIATIVE_CONTACT initcon
                        left outer join CONTACT_DETAILS contact
                        on initcon.SID = contact.SID and initcon.ContactCode = contact.BOBJECTLINK
                        left outer join CONTACT_MASTER conmaster on conmaster.SID = contact.SID and conmaster.AOBJECTLINK = contact.AOBJECTLINK
                        inner join " + WebConfigHelper.getDatabaseSNAName() + ".dbo.SNA_" + SNAID + @"_TIMEATTENDANCE act on conmaster.BPCODE = act.DOCNUMBER and act.ItemType = 'CRM_CONT'
                        left join CONTACT_CRM_TYPE ctype
                        on contact.SID = ctype.sid 
                        and contact.BOBJECTLINK= ctype.BOBJECTLINK
                        left outer join CONTACT_CRM_TYPE_MASTER ctypemaster
                        on ctype.sid = ctypemaster.sid and ctype.ContacttypeCode = ctypemaster.ContacttypeCode
                        left join CONTACT_DETAILS_OTHERINFO OtherInfo
                        on OtherInfo.BOBJECTLINK = contact.BOBJECTLINK
                        and OtherInfo.SID = contact.SID

                        left join (
                                SELECT  sid, BOBJECTLINK
                                , STUFF(
                                (SELECT ', ' + isnull(EMAIL, '')
                                    FROM  CONTACT_EMAIL b
                                    where a.BOBJECTLINK = b.BOBJECTLINK
                                    FOR XML PATH(''))  ,1,1,'') as EMAIL

                                FROM  CONTACT_EMAIL a
                                group by sid, BOBJECTLINK
                        ) contact_email on  contact_email.sid = contact.sid and  contact_email.BOBJECTLINK = contact.bobjectlink
                        left join (
                            SELECT  sid, BOBJECTLINK
                            , STUFF(
                                (SELECT ', ' + isnull(PHONENUMBER,'') + isnull(EXT,'')   
                                    FROM CONTACT_PHONE b
                                    where a.BOBJECTLINK = b.BOBJECTLINK
                                    FOR XML PATH(''))  ,1,1,'') as phone

                            FROM CONTACT_PHONE a 
                                group by  sid, BOBJECTLINK
                        ) contact_phone on  contact_phone.sid = contact.sid and  contact_phone.BOBJECTLINK = contact.bobjectlink
                        where initcon.SID = '" + SID + @"'
                        and initcon.WorkGroupCode = '" + WorkGroupCode + @"'";

            sql += " and initcon.CompanyStructureCode = '" + StructureCode + "' ";
            
            return db.selectDataFocusone(sql);
        }

        public DataTable getDataInitiativeCustomerV2(string SNAID, string SID, string StructureCode, string WorkGroupCode)
        {
            if (string.IsNullOrEmpty(StructureCode))
                return new DataTable();

            string sql = @"select conmaster.BPCODE,initcon.CustomerCode,initcon.CompanyStructureCode,contact.NAME1 + ' ' + contact.NAME2 as FullName
                              ,ISNULL(ctypemaster.ContactTypeName,'ไม่ระบุประเภท') as ContactTypeName
                              ,contact_email.EMAIL,contact_phone.phone
                              ,case when ISNULL(contact.POSITION, '') = '' then 'ไม่ระบุตำแหน่ง' else contact.POSITION END  as POSITION
                              ,OtherInfo.facebook, OtherInfo.instagram, OtherInfo.tweeter, OtherInfo.LineID

                            from LINK_ACCOUNTABILITY_INITIATIVE_CUSTOMER initcon
                            left outer join CONTACT_MASTER conmaster 
                              on conmaster.SID = initcon.SID 
                              and conmaster.BPCODE = initcon.CustomerCode
                            left outer join CONTACT_DETAILS contact
                              on initcon.SID = contact.SID 
                              and conmaster.AOBJECTLINK = contact.AOBJECTLINK
                            inner join " + WebConfigHelper.getDatabaseSNAName() + ".dbo.SNA_" + SNAID + @"_TIMEATTENDANCE act 
                              on conmaster.BPCODE = act.DOCNUMBER
                              and act.ItemType = 'CRM_CONT'
                            left join CONTACT_CRM_TYPE ctype
                              on contact.SID = ctype.sid 
                              and contact.BOBJECTLINK= ctype.BOBJECTLINK
                            left outer join CONTACT_CRM_TYPE_MASTER ctypemaster
                              on ctype.sid = ctypemaster.sid 
                              and ctype.ContacttypeCode = ctypemaster.ContacttypeCode
                            left join CONTACT_DETAILS_OTHERINFO OtherInfo
                              on OtherInfo.BOBJECTLINK = contact.BOBJECTLINK
                              and OtherInfo.SID = contact.SID

                            left join 
                            (
                              SELECT  sid, BOBJECTLINK
                              , STUFF(
                              (SELECT ', ' + isnull(EMAIL, '')
                                  FROM  CONTACT_EMAIL b
                                  where a.BOBJECTLINK = b.BOBJECTLINK
                                  FOR XML PATH(''))  ,1,1,'') as EMAIL

                              FROM  CONTACT_EMAIL a
                              group by sid, BOBJECTLINK
                            ) contact_email 
                              on  contact_email.sid = contact.sid 
                              and  contact_email.BOBJECTLINK = contact.bobjectlink
  
                            left join 
                            (
                                SELECT  sid, BOBJECTLINK
                                , STUFF(
                                    (
                                      SELECT ', ' + isnull(PHONENUMBER,'') + isnull(EXT,'')   
                                      FROM CONTACT_PHONE b
                                      where a.BOBJECTLINK = b.BOBJECTLINK
                                      FOR XML PATH('')
                                    )  ,1,1,''
                                  ) as phone
                                FROM CONTACT_PHONE a 
                                group by  sid, BOBJECTLINK
                            ) contact_phone 
                              on  contact_phone.sid = contact.sid 
                              and  contact_phone.BOBJECTLINK = contact.bobjectlink
                            where initcon.SID = '" + SID + @"'
                              and initcon.WorkGroupCode = '" + WorkGroupCode + @"' 
                              and initcon.CompanyStructureCode = '" + StructureCode + "' ";

            return db.selectDataFocusone(sql);
        }

        public DataTable getCRMContactType(string SID)
        {
            string sql = @"select ContactTypeCode,ContactTypeName from CONTACT_CRM_TYPE_MASTER where SID='" + SID + "'";

            return db.selectDataFocusone(sql);
        }

        public DataTable getCRMContact(string SID, string CompanyCode, string WorkGroupCode, string StructureCode, string ContactType, string SearchText, string ContactCode)
        {
            string sql = @"SELECT distinct contactdetail.NAME1 + ' ' + contactdetail.NAME2 as FullName,contactdetail.NickName
                            ,ISNULL(ctypemaster.ContactTypeName,'ไม่ระบุประเภท') as ContactTypeName
                            ,case when ISNULL(contactdetail.POSITION, '') = '' then 'ไม่ระบุตำแหน่ง' else contactdetail.POSITION END  as POSITION
                            ,contact_email.EMAIL,contact_phone.phone
                            ,contact.BPCODE,contact.BPNAME1,contact.AOBJECTLINK
                            ,contactdetail.BOBJECTLINK , contact_structurename.StructureName
                            FROM CONTACT_MASTER contact
                            inner join CONTACT_DETAILS contactdetail
                            on contact.SID = contactdetail.SID AND contact.AOBJECTLINK = contactdetail.AOBJECTLINK
                            left join CONTACT_CRM_TYPE ctype
                            on contactdetail.SID = ctype.sid 
                            and contactdetail.BOBJECTLINK= ctype.BOBJECTLINK
                            left outer join CONTACT_CRM_TYPE_MASTER ctypemaster
                            on ctype.sid = ctypemaster.sid and ctype.ContacttypeCode = ctypemaster.ContacttypeCode
                            left join (
                                 SELECT  sid, BOBJECTLINK
                                 , STUFF(
                                    (SELECT ', ' + isnull(EMAIL, '')
                                     FROM  CONTACT_EMAIL b
                                     where a.BOBJECTLINK = b.BOBJECTLINK
                                     FOR XML PATH(''))  ,1,1,'') as EMAIL

                                 FROM  CONTACT_EMAIL a
                                 group by sid, BOBJECTLINK
                            ) contact_email on  contact_email.sid = contactdetail.sid and  contact_email.BOBJECTLINK = contactdetail.bobjectlink
                            left join (
                                SELECT  sid, BOBJECTLINK
                                , STUFF(
                                    (SELECT ', ' + isnull(PHONENUMBER,'') + isnull(EXT,'')   
                                     FROM CONTACT_PHONE b
                                     where a.BOBJECTLINK = b.BOBJECTLINK
                                     FOR XML PATH(''))  ,1,1,'') as phone

                                FROM CONTACT_PHONE a 
                                 group by  sid, BOBJECTLINK
                            ) contact_phone on  contact_phone.sid = contactdetail.sid and  contact_phone.BOBJECTLINK = contactdetail.bobjectlink
                            inner join (
	                            SELECT SID,ContactCode
                                , STUFF(
                                    (SELECT ', ' + isnull(StructureName,'')   
                                        FROM (select a.ContactCode,b.StructureName from LINK_ACCOUNTABILITY_INITIATIVE_CONTACT a
			                            left outer join LINK_PROJECT_COMPANY_STRUCTURE_ITEM b on a.SID = b.SID and a.WorkGroupCode = b.WorkGroupCode 
			                            and a.CompanyStructureCode = b.StructureCode ) b
                                        where a.ContactCode = b.ContactCode
                                        FOR XML PATH(''))  ,1,1,'') as StructureName

                                FROM (select a.SID,a.ContactCode,b.StructureName from LINK_ACCOUNTABILITY_INITIATIVE_CONTACT a
			                            left outer join LINK_PROJECT_COMPANY_STRUCTURE_ITEM b on a.SID = b.SID and a.WorkGroupCode = b.WorkGroupCode 
			                            and a.CompanyStructureCode = b.StructureCode ) a 
                                    group by  SID,ContactCode, StructureName
                            ) contact_structurename 
                            on contact_structurename.SID = contactdetail.SID and contact_structurename.ContactCode = contactdetail.BOBJECTLINK
                            
                            where contact.SID='" + SID + "' and contact.CompanyCode='" + CompanyCode + @"'";

            if (!string.IsNullOrEmpty(ContactCode))
            {
                string[] ContactCodeArray = ContactCode.Split(',');
                string sqlIn = string.Join(",", ContactCodeArray.Select(a => "'" + a + "'"));

                sql += " and contactdetail.bobjectlink not in(" + sqlIn + ") ";
            }
            if (!string.IsNullOrEmpty(ContactType))
            {
                sql += " and ctypemaster.ContactTypeCode='" + ContactType + "'";
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                sql += @" and (
                            contactdetail.NickName like '%" + SearchText + @"%'
                            or contactdetail.NAME1 like '%" + SearchText + @"%'
                            or contactdetail.NAME2 like '%" + SearchText + @"%'
                            or contact_email.EMAIL like '%" + SearchText + @"%'
                        )";
            }

            return db.selectDataFocusone(sql);
        }

        public string getProjectMappingStructure(string SID, string WorkGroupCode, string StructureCode)
        {
            string Projectcode = "";
            string sql = @"select * from LINK_PROJECT_COMPANY_STRUCTURE_MAPPING_PROJECT_DETAIL 
                            where SID='" + SID + "' and WorkGroupCode='" + WorkGroupCode + @"'
                            and StructureCode='" + StructureCode + "'";

            DataTable dt = db.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                Projectcode = dt.Rows[0]["ProjectCode"].ToString();
            }

            return Projectcode;
        }

        public string getProjectMappingStructureUniversalService(string SID, 
            string WorkGroupCode, string StructureCode)
        {
            string Projectcode = "";
            string sql = @"select * from LINK_PROJECT_COMPANY_STRUCTURE_MAPPING_UNIVERSAL_SERVICE 
                            where SID='" + SID + "' and WorkGroupCode='" + WorkGroupCode + @"'
                            and StructureCode='" + StructureCode + "'";

            DataTable dt = db.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                Projectcode = dt.Rows[0]["ProjectCode"].ToString();
            }

            return Projectcode;
        }

        public DataTable getUniversalDetail(string SID, string CompanyCode, string ProjectCode)
        {
            string sql = @"select project.ProjectName,tem.* from LINK_PROJECT_MASTER_CUSTOMIZE_TEMPLATE tem 
                            left outer join LINK_PROJECT_MASTER project on tem.SID = project.SID and tem.CompanyCode = project.CompanyCode and tem.ProjectCode = project.ProjectCode
                            where tem.SID='" + SID + "' and tem.CompanyCode='" + CompanyCode + "' and tem.ProjectCode='" + ProjectCode + "'";

            return db.selectDataFocusone(sql);
        }

        public DataTable getUniversalServiceDetail(string SID, string CompanyCode, string ProjectCode)
        {
            string sql = @"select project.ProjectName,tem.* 
                            from LINK_PROJECT_MASTER_UNIVERSAL_SERVICE tem 
                            left outer join LINK_PROJECT_MASTER project 
                                on tem.SID = project.SID 
                                and tem.CompanyCode = project.CompanyCode 
                                and tem.ProjectCode = project.ProjectCode
                            where tem.SID='" + SID + @"' 
                                and tem.CompanyCode='" + CompanyCode + @"' 
                                and tem.ProjectCode='" + ProjectCode + "'";
            return db.selectDataFocusone(sql);
        }

        public string getSaleTeam(string SID, string CompanyCode, string ProjectCode)
        {
            string SaleTeam = "";
            string sql = @"select * from LINK_PROJECT_MASTER_CONTACT where SID='" + SID + "' and CompanyCode='" + CompanyCode + "' and ProjectCode='" + ProjectCode + "'";

            DataTable dt = db.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(SaleTeam))
                    {
                        SaleTeam = dr["EmployeeCode"].ToString();
                    }
                    else
                    {
                        SaleTeam += "," + dr["EmployeeCode"].ToString();
                    }
                }
            }

            return SaleTeam;
        }

        #region
        public DataTable getListCustomerMapping(string SID, string CompanyCode,
            string WorkGroupCode, string WorkStructureCode)
        {
            string sql = @"select Customer.* 
                            from dbo.master_customer Customer
                            left join LINK_ACCOUNTABILITY_INITIATIVE_CUSTOMER Mapping
                              on Customer.SID = Mapping.SID 
                              and Customer.CompanyCode = Mapping.CompanyCode
                              and Customer.CustomerCode = Mapping.CustomerCode
                              and Mapping.CompanyStructureCode = '" + WorkStructureCode + @"' 
                              and Mapping.WorkGroupCode = '" + WorkGroupCode + @"'
                            where Customer.SID = '" + SID + @"' 
                              AND Customer.CompanyCode = '" + CompanyCode + @"'
                              AND Mapping.SID is null";

            return db.selectDataFocusone(sql);
        }

        public DataTable getContactMappingInStructure(string SID, string CompanyCode, 
            string WorkGroupCode, string CompanyStructureCode, string CustomerCode)
        {
            string sql = @"";
            if (string.IsNullOrEmpty(CustomerCode))
            {
                sql = @"select InitCus.*, Master.CustomerName, Master.CustomerGroup
                            , CusType.[Description], ConMaster.AOBJECTLINK
                        from LINK_ACCOUNTABILITY_INITIATIVE_CUSTOMER InitCus
                        left join master_customer Master
                            on Master.SID = InitCus.SID
                            and Master.CompanyCode = InitCus.CompanyCode
                            and Master.CustomerCode = InitCus.CustomerCode
                        left join master_config_customer_doctype CusType
                            on CusType.Companycode = InitCus.CompanyCode
                            and CusType.SID = InitCus.SID
                            and CusType.CustomerGroupCode = Master.CustomerGroup
                        left join CONTACT_MASTER ConMaster
                            on ConMaster.BPCODE = InitCus.CustomerCode
                            and ConMaster.SID = ConMaster.SID
                            and ConMaster.COMPANYCODE = InitCus.COMPANYCODE

                        where InitCus.SID = '" + SID + @"' 
                            AND InitCus.CompanyCode = '" + CompanyCode + @"' 
                            AND InitCus.WorkGroupCode = '" + WorkGroupCode + @"' 
                            AND InitCus.CompanyStructureCode = '" + CompanyStructureCode + @"'";
            }
            else
            {
                string[] CustomerCodeArray = CustomerCode.Split(',');

                string sqlIn = string.Join(",", CustomerCodeArray.Select(a => "'" + a + "'"));

                sql = @"select CusMaster.CustomerCode, CusMaster.CustomerGroup
                          , CusMaster.CustomerName, ConMaster.AOBJECTLINK
                          , CusType.[Description]
                        from master_customer CusMaster
                        left join master_config_customer_doctype CusType
                          on CusType.Companycode = CusMaster.CompanyCode
                          and CusType.SID = CusMaster.SID
                          and CusType.CustomerGroupCode = CusMaster.CustomerGroup
                        left join CONTACT_MASTER ConMaster
                          on ConMaster.BPCODE = CusMaster.CustomerCode
                          and ConMaster.SID = CusMaster.SID
                          and ConMaster.COMPANYCODE = CusMaster.COMPANYCODE

                        where CusMaster.SID = '001' 
                          AND CusMaster.CompanyCode = '1000' 
                          and CusMaster.CustomerCode in (" + sqlIn + ")";
            }

            return db.selectDataFocusone(sql);
        }

        public DataTable getListContactFormCustomerCode(string SID, string CompanyCode, string CustomerCode)
        {
            string sql = @"select Customer.SID, Customer.CompanyCode, Customer.CustomerCode
                              , Customer.CustomerName, Customer.CustomerNameTH
                              , Contact.BPTYPE, Contact.BPNAME1, Contact.BPNAME2, Contact.AOBJECTLINK
                              , ConDetail.ITEMNO, ConDetail.NAME1, ConDetail.NAME2, ConDetail.POSITION
                              , ConDetail.REMARK1, ConDetail.REMARK2, ConDetail.BOBJECTLINK
                              , ConInfo.person_height, ConInfo.person_weight, ConInfo.facebook
                              , ConInfo.instagram, ConInfo.tweeter, ConInfo.LineID
                              , TypeMaster.ContactTypeName, Phone.PHONENUMBER, Email.EMAIL
                          from master_customer Customer
                          left join CONTACT_MASTER Contact
                              on customer.CustomerCode = Contact.BPCODE
                              and Customer.SID = Contact.SID
                              and Customer.CompanyCode = Contact.CompanyCode
                          left join CONTACT_DETAILS ConDetail
                              on Contact.AOBJECTLINK = ConDetail.AOBJECTLINK
                              and Contact.SID = ConDetail.SID
                          left join CONTACT_DETAILS_OTHERINFO ConInfo
                              on ConDetail.BOBJECTLINK = ConInfo.BOBJECTLINK
                              and ConDetail.SID = ConInfo.SID
                          left join CONTACT_CRM_TYPE ConType
                              on ConType.BOBJECTLINK = ConDetail.BOBJECTLINK
                              and ConType.SID = Customer.SID
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

                          where Customer.SID = '" + SID + @"' 
                              AND Customer.CompanyCode = '" + CompanyCode + @"' 
                              AND Customer.CustomerCode = '" + CustomerCode + @"'";

            return db.selectDataFocusone(sql);
        }
        #endregion
    }
}