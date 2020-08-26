using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Focusone.ICMWcfService;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using ERPW.Lib.Service;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service.Workflow;
using Newtonsoft.Json;
using System.Web.Configuration;
using ServiceWeb.API.Model.Respond;
using Link.Lib.Model.Model.Timeline;
using ERPW.Lib.Service.Entity;
using agape.proxy.data.dataset;
using ERPW.Lib.F1WebService.ICMUtils;
using System.Web.SessionState;

namespace ServiceWeb.Service
{
    public class AfterSaleService
    {
        string ThisPage = "AfterSaleService";
        private DBService dbService = new DBService();
        private LookupICMService lookupICMService = new LookupICMService();
        private F1LinkReference.F1LinkReference lc_lib = new F1LinkReference.F1LinkReference();
        private EquipmentService ServiceEquipment = new EquipmentService();

        private static AfterSaleService _instance;
        public static AfterSaleService getInstance()
        {
            if (_instance == null)
            {
                _instance = new AfterSaleService();
            }
            return _instance;
        }

        public DataTable getCountAttachFile(string sid, string companycode, string callid)
        {
            string sql = "SELECT count(b.doc_number)as totalfile,a.CallerID,a.Fiscalyear FROM cs_servicecall_header a  WITH (NOLOCK) " +
                         "inner join km_attach_list b WITH (NOLOCK)  on a.SID = b.sid and a.CallerID = b.doc_number and a.Fiscalyear = b.doc_year  " +
                         " where a.SID='" + sid + "'" +
                         " and CompanyCode='" + companycode + "'";
            if (!string.IsNullOrEmpty(callid))
                sql += " and a.CallerID='" + callid + "'";
            sql += " group by a.CallerID,a.Fiscalyear";
            return dbService.selectDataFocusone(sql);
        }


        public DataTable getDataServiceTicketHeader(string sid, string companycode, string callid)
        {
            string sql = @"select *
                            from cs_servicecall_header WITH (NOLOCK) 
                            where SID = '" + sid + @"' 
                            AND CompanyCode = '" + companycode + @"' 
                            AND CallerID = '" + callid + "'";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getServiceCall(String sid, String companycode, String customercode,
         String fiscalyear, String doctype, String Priority, String docdateFrom, String docdateTo, String contactname,
            String status, String docnumber, String AssignTo, String condition)
        {
            //String sqlTop = ""//(String.IsNullOrEmpty(optioncode)) ? "" : "TOP(50)";
            StringBuilder st = new StringBuilder();
            st.AppendLine("SELECT TOP(100) a.CallerID AS CallerID,a.UPDATED_BY AS UPDATED_BY,a.CREATED_ON AS CREATED_ON, ");
            st.AppendLine("a.UPDATED_ON AS UPDATED_ON,a.DOCDATE AS DOCDATE,B.EquipmentNo AS EquipmentNo, ");
            st.AppendLine("C.Description AS Description,a.CustomerCode AS CustomerCode,a.CompanyCode AS CompanyCode, ");
            st.AppendLine("a.Doctype AS Doctype,B.ObjectID AS ObjectID,B.xLineNo AS xLineNo, ");
            st.AppendLine("a.ContractPersonName AS ContractPersonName,a.ContractPersonTel AS ContractPersonTel, ");
            st.AppendLine("a.Email AS Email,a.Address AS Address,a.HeaderText AS HeaderText,a.Priority AS Priority, ");
            st.AppendLine("D.Description AS issuestatus,B.BObjectID AS BObjectID,a.Fiscalyear AS Fiscalyear, ");
            st.AppendLine("B.AssignDate AS AssignDate,a.CallStatus AS CallStatus,E.Description AS CallStatusName, a.Docstatus, ");
            st.AppendLine("a.CREATED_BY AS CREATED_BY,a.CREATED_BY+':'+F.FirstName+' '+F.LastName AS OpenName, ");
            st.AppendLine("B.TechnicianCode AS TechnicianCode,B.TechnicianCode+':'+G.FirstName+' '+G.LastName AS TechnicianName, ");
            st.AppendLine("B.AssignCode AS AssignCode,B.AssignCode+':'+H.FirstName+' '+H.LastName AS AssignName, ");
            st.AppendLine("B.ResponseBy AS ResponseBy,B.ResponseBy+':'+I.FirstName+' '+I.LastName AS ResponseName, ");
            st.AppendLine("B.ResolutionBy AS ResolutionBy,B.ResolutionBy+':'+J.FirstName+' '+J.LastName AS ResolutionName, ");
            st.AppendLine("B.CloseBy AS CloseBy,B.CloseBy+':'+K.FirstName+' '+K.LastName AS CloseName, ");
            st.AppendLine("a.CancelBy AS CancelBy,a.CancelBy+':'+L.FirstName+' '+L.LastName AS CancelName,M.CustomerName AS CustomerName ");
            st.AppendLine(",a.ProjectCode,pj.Description as projectname ");
            st.AppendLine("From cs_servicecall_header AS a WITH (NOLOCK)  INNER JOIN cs_servicecall_item AS B WITH (NOLOCK)  ");
            st.AppendLine("ON A.SID = B.SID AND A.ObjectID = B.ObjectID  LEFT OUTER JOIN master_equipment AS C WITH (NOLOCK)  ");
            st.AppendLine("ON B.SID = C.SID AND B.EquipmentNo = C.EquipmentCode  left JOIN master_problemtype AS D WITH (NOLOCK)  ");
            st.AppendLine("ON B.ProblemTypeCode = D.Name AND B.SID = D.SID  INNER JOIN master_callstatus AS E WITH (NOLOCK)  ");
            st.AppendLine("ON A.SID = E.SID AND A.CallStatus = E.Name  LEFT OUTER JOIN master_employee AS F WITH (NOLOCK)  ");
            st.AppendLine("ON A.SID = F.SID AND A.CompanyCode = F.CompanyCode AND ");
            st.AppendLine("A.CREATED_BY = F.EmployeeCode  LEFT OUTER JOIN master_employee AS G WITH (NOLOCK)  ");
            st.AppendLine("ON B.SID = G.SID AND B.CompanyCode = G.CompanyCode AND ");
            st.AppendLine("B.TechnicianCode = G.EmployeeCode  LEFT OUTER JOIN master_employee AS H WITH (NOLOCK)  ");
            st.AppendLine("ON B.SID = H.SID AND B.CompanyCode = H.CompanyCode AND ");
            st.AppendLine("B.AssignCode = H.EmployeeCode  LEFT OUTER JOIN master_employee AS I WITH (NOLOCK)  ");
            st.AppendLine("ON B.SID = I.SID AND B.CompanyCode = I.CompanyCode AND ");
            st.AppendLine("B.ResponseBy = I.EmployeeCode  LEFT OUTER JOIN master_employee AS J WITH (NOLOCK)  ");
            st.AppendLine("ON B.SID = J.SID AND B.CompanyCode = J.CompanyCode AND ");
            st.AppendLine("B.ResolutionBy = J.EmployeeCode  LEFT OUTER JOIN master_employee AS K WITH (NOLOCK)  ");
            st.AppendLine("ON B.SID = K.SID AND B.CompanyCode = K.CompanyCode AND ");
            st.AppendLine("B.CloseBy = K.EmployeeCode  LEFT OUTER JOIN master_employee AS L WITH (NOLOCK)  ");
            st.AppendLine("ON A.SID = L.SID AND A.CompanyCode = L.CompanyCode AND A.CancelBy = L.EmployeeCode ");
            st.AppendLine("LEFT JOIN master_customer AS M  WITH (NOLOCK) ON A.SID = M.SID AND A.CompanyCode = M.CompanyCode AND A.CustomerCode = M.CustomerCode");
            st.AppendLine("left join master_conf_project pj WITH (NOLOCK)  on A.SID = pj.SID and A.ProjectCode = pj.ProjectCode");
            st.AppendLine(" where 1=1 and A.SID='" + sid + "' ");
            if (!string.IsNullOrEmpty(fiscalyear))
                st.AppendLine("and a.fiscalyear='" + fiscalyear + "'");
            if (!string.IsNullOrEmpty(companycode))
                st.AppendLine("and a.CompanyCode='" + companycode + "'");
            if (!string.IsNullOrEmpty(doctype))
                st.AppendLine("and a.Doctype='" + doctype + "'");
            if (!string.IsNullOrEmpty(Priority))
                st.AppendLine("and a.Priority='" + Priority + "'");
            if (!string.IsNullOrEmpty(customercode))
                st.AppendLine("and a.CustomerCode='" + customercode + "'");
            if (!string.IsNullOrEmpty(contactname))
                st.AppendLine("and a.ContractPersonName like'%" + contactname + "%'");
            if (!string.IsNullOrEmpty(docdateFrom) && !string.IsNullOrEmpty(docdateTo))
                st.AppendLine("and (a.DOCDATE >='" + docdateFrom + "' and a.DOCDATE<='" + docdateTo + "')");
            else if (!string.IsNullOrEmpty(docdateFrom))
                st.AppendLine("and a.DOCDATE ='" + docdateFrom + "'");
            if (!string.IsNullOrEmpty(status))
                st.AppendLine("and a.Docstatus = '" + status + "'");
            if (!string.IsNullOrEmpty(docnumber))
                st.AppendLine("and a.CallerID like'%" + docnumber + "%'");
            if (!string.IsNullOrEmpty(AssignTo))
                st.AppendLine(" and B.AssignCode='" + AssignTo + "'");
            if (!string.IsNullOrEmpty(condition))
                st.AppendLine(" " + condition);

            st.AppendLine(" order by a.DOCDATE desc ,a.CallerID desc");
            DataTable dt = dbService.selectDataFocusone(st.ToString());
            return dt;
        }

        public DataTable getServicecallAssignForYou(String sid, String companycode, String assignto)
        {
            string where = " and B.CloseStatus='01' and A.CallStatus='01' and (B.ResolutionBy is null or B.ResolutionBy ='') ";
            return getServiceCall(sid, companycode, "", "", "", "", "", "", "", "", "", assignto, where);
        }

        public DataTable getServicecallWaitForClose(String sid, String companycode, String open_by)
        {
            string where = " and B.ResponseBy<>'' and B.ResolutionBy<>''  and B.CloseStatus='01' ";

            if (!string.IsNullOrEmpty(open_by))
                where += " and A.CREATED_BY='" + open_by + "'";
            return getServiceCall(sid, companycode, "", "", "", "", "", "", "", "", "", "", where);
        }

        public DataTable countMessageChatter(string groupcode)
        {
            StringBuilder st = new StringBuilder();
            st.AppendLine(" select count(ISNULL(gHead.a_obj_link,0)) as hCount,");
            st.AppendLine("  ISNULL(gItem.iCount,0) as iCount,");
            st.AppendLine("  count(ISNULL(gHead.a_obj_link,0)) + ISNULL(gItem.iCount,0) as sumMessage ");
            st.AppendLine("  from km_message_head gHead  WITH (NOLOCK) ");
            st.AppendLine("  left outer join ");
            st.AppendLine("      (select i.root_obj, count(i.a_obj_link) as iCount from km_message_head h WITH (NOLOCK)  ");
            st.AppendLine("        left outer join km_message_follow i WITH (NOLOCK)  ");
            st.AppendLine("        on h.sid = i.sid and h.a_obj_link = i.root_obj ");
            st.AppendLine("        where h.sid='001' and h.RCGROUP='" + groupcode + "' ");
            st.AppendLine("       group by i.root_obj) gItem ");
            st.AppendLine("  on gHead.a_obj_link = gItem.root_obj ");
            st.AppendLine("  where gHead.sid='001' and gHead.RCGROUP='" + groupcode + "' ");
            st.AppendLine("  group by gItem.iCount ");

            return dbService.selectDataFocusone(st.ToString());
        }

        private string GenerateCodeAndDesc(string code, string desc)
        {
            string res = code.Trim();

            if (!string.IsNullOrEmpty(desc))
            {
                res += " : " + desc.Trim();
            }

            return res;
        }

        public DataTable getSearchDoctype(string doctype, string comcode)
        {
            return getSearchDoctype(doctype, comcode, false, false, new List<string>() { }, ERPWAuthentication.SID);
        }

        public DataTable getSearchDoctype(string doctype, string comcode, List<string> listBusinessObject)
        {
            return getSearchDoctype(doctype, comcode, false, false, listBusinessObject, ERPWAuthentication.SID);
        }
        public DataTable getSearchDoctype(string doctype, string comcode, string BusinessObject)
        {
            return getSearchDoctype(doctype, comcode, false, false, new List<string>() { BusinessObject }, ERPWAuthentication.SID);
        }

        public DataTable getSearchDoctype(string doctype, string comcode, bool IsExclude, bool IsPageChangeChange)
        {
            return getSearchDoctype(doctype, comcode, IsExclude, IsPageChangeChange, new List<string>() { }, ERPWAuthentication.SID);
        }
        public DataTable getSearchDoctype(string doctype, string comcode, string BusinessObject, string SID)
        {
            return getSearchDoctype(doctype, comcode, false, false, new List<string>() { BusinessObject }, SID);
        }
        private DataTable getSearchDoctype(string doctype, string comcode, bool IsExclude, bool IsPageChangeChange,
            List<string> listBusinessObject, string SID)
        {
            List<string> listTicketType = new List<string>();

            string OwnerGroupCode = "";
            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);
            if (FilterOwner)
            {
                OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
            }

            if (IsExclude)
            {
                List<string> BusinessObject = new List<string>();
                if (IsPageChangeChange)
                {
                    BusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE);
                }
                else
                {
                    BusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT);
                    BusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST);
                    BusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM);
                }

                listTicketType = ServiceTicketLibrary.GetInstance().getListTicketType(
                    SID,
                    BusinessObject,
                    OwnerGroupCode
                );
            }
            else
            {
                if (listBusinessObject.Count > 0)
                {
                    listTicketType = ServiceTicketLibrary.GetInstance().getListTicketType(
                        SID,
                        listBusinessObject,
                        OwnerGroupCode
                    );
                }
            }

            string where = " and b.PostingType ='SERVICCALL' and a.xActive = 'True'";

            if (!string.IsNullOrEmpty(doctype))
            {
                where += " and a.DocumentTypeCode='" + doctype + "'";
            }

            if (listTicketType.Count > 0)
            {
                where += " and a.DocumentTypeCode in ('" + string.Join("', '", listTicketType) + "')";
            }

            if (!string.IsNullOrEmpty(comcode))
            {
                where += " and a.CompanyCode='" + comcode + "'";
            }


            DataTable dt = lookupICMService.GetSearchData(
                SID,
                "SHService",
                "#where a.SID='" + SID + "' " + where
            );

            foreach (DataRow dr in dt.Rows)
            {
                dr["Description"] = GenerateCodeAndDesc(dr["DocumentTypeCode"].ToString(), dr["Description"].ToString());
            }

            return dt;
        }

        public DataTable getSearchCustomerCode(string customercode, string companycode)
        {
            //string where = "";
            //if (customercode != "")
            //{
            //    where += " and A.CustomerCode='" + customercode + "'";
            //}
            //if (companycode != "")
            //{
            //    where += " and A.CompanyCode='" + companycode + "'";
            //}

            //DataTable dt = lookupICMService.GetSearchData(
            //    ERPWAuthentication.SID,
            //        "Cus_master_header", "#where A.SID='"
            //            + ERPWAuthentication.SID
            //            + "' " + where);
            string sql = @"select CustomerName, CustomerCode
                            from master_customer WITH (NOLOCK) 
                            where CompanyCode = '" + companycode + @"'";

            if (!string.IsNullOrEmpty(customercode))
            {
                sql += @" AND CustomerCode = '" + customercode + @"' ";
            }

            DataTable dt = dbService.selectDataFocusone(sql);
            //new
            foreach (DataRow dr in dt.Rows)
            {
                dr["CustomerName"] = GenerateCodeAndDesc(dr["CustomerCode"].ToString(), dr["CustomerName"].ToString());
            }

            return dt;//CustomerCode,CustomerName
        }

        public DataTable GetCustomerWithForeignName()
        {
            DataTable dt = lookupICMService.GetSearchData(ERPWAuthentication.SID,
                   "Cus_master_header", "#WHERE A.SID= '" + ERPWAuthentication.SID + "' AND A.CompanyCode = '" + ERPWAuthentication.CompanyCode + "'");

            ERPW.Lib.Master.CustomerService libCustomer = new ERPW.Lib.Master.CustomerService();

            foreach (DataRow dr in dt.Rows)
            {
                string customerName = libCustomer.PrepareCustomerNameAndForeignName(dr["CustomerName"].ToString(), dr["ForeignName"].ToString());

                dr["CustomerName"] = customerName;
            }

            return dt;
        }

        public DataTable GetCustomerData()
        {
            string sql = @"SELECT a.CustomerCode, a.CustomerName, a.ForeignName
                            FROM master_customer a WITH (NOLOCK) 
                            INNER JOIN master_customer_general b WITH (NOLOCK) 
                                ON a.SID = b.SID 
                                AND a.CompanyCode = b.CompanyCode
                                AND a.CustomerCode = b.CustomerCode
                            WHERE a.SID = '" + ERPWAuthentication.SID + "' AND a.CompanyCode = '" + ERPWAuthentication.CompanyCode + @"' AND b.Active='True'";


            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                sql += @" and b.BPChannalCode = '" + ERPWAuthentication.Permission.OwnerGroupCode + "' ";
            }

            sql += " ORDER BY a.[CustomerName]";

            DataTable dt = dbService.selectDataFocusone(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string customerName = ERPW.Lib.Master.CustomerService.getInstance().PrepareCustomerNameAndForeignName(dr["CustomerName"].ToString(), dr["ForeignName"].ToString());

                dr["CustomerName"] = customerName;
            }
            return dt;
        }

        public DataTable getSearchVendor(string SID, string CompanyCode, string VendorCode)
        {
            string condition = "";

            if (!string.IsNullOrEmpty(VendorCode))
            {
                condition += " AND VendorCode = '' ";
            }
            string sql = @"select * 
                            from master_vendor WITH (NOLOCK) 
                            where SID = '" + SID + @"'
                              AND CompanyCode = '" + CompanyCode + @"' 
                            " + condition;


            return dbService.selectDataFocusone(sql);
        }

        public DataTable getContactPerson(string COMPANYCODE, string BPCODE, string ActiveStatus = null)
        {
            string sql = @"SELECT case ISNULL(b.NAME1,'') when '' then b.NickName else b.NAME1 end NAME1
                             , b.NAME2,b.POSITION,b.BOBJECTLINK 
                             , IsNull(d.email, '' ) as email, IsNull(e.phone, '' ) as phone
                             ,b.REMARK1 as REMARK
                          FROM CONTACT_MASTER a WITH (NOLOCK)  inner join CONTACT_DETAILS b WITH (NOLOCK)  
                            on a.SID = b.SID and a.AOBJECTLINK = b.AOBJECTLINK 
                        left join 
                            (
                                SELECT  sid, [BOBJECTLINK]
                                , STUFF(
                                    (SELECT ', ' + isnull([EMAIL], '')
                                    FROM  [CONTACT_EMAIL] b
                                    where a.[BOBJECTLINK] = b.[BOBJECTLINK]
                                    FOR XML PATH(''))  ,1,1,'') as [EMAIL]

                                FROM  [CONTACT_EMAIL] a
                                group by sid, [BOBJECTLINK] 
                            ) d 
                                on d.sid = a.sid 
                                and  d.BOBJECTLINK = b.bobjectlink
  
                            left join 
                            (
                                SELECT  sid, [BOBJECTLINK]
                                , STUFF(
                                    (SELECT ', ' + isnull([PHONENUMBER],'') + isnull(EXT,'')   
                                    FROM  [CONTACT_PHONE] b
                                    where a.[BOBJECTLINK] = b.[BOBJECTLINK]
                                    FOR XML PATH(''))  ,1,1,'') as phone

                                FROM  [CONTACT_PHONE] a 
                                group by  sid, [BOBJECTLINK]
                            ) e 
                                on e.sid = a.sid 
                                and e.BOBJECTLINK = b.bobjectlink
                            where a.SID='" + ERPWAuthentication.SID + "' ";
            if (!string.IsNullOrEmpty(COMPANYCODE))
                sql += " and a.COMPANYCODE='" + COMPANYCODE + "'";
            if (!string.IsNullOrEmpty(BPCODE))
                sql += " and a.BPCODE='" + BPCODE + "'";
            if (!string.IsNullOrEmpty(ActiveStatus))
                sql += " and b.ACTIVESTATUS='" + ActiveStatus + "'";
            DataTable dt = dbService.selectDataFocusone(sql);

            ERPW.Lib.Master.CustomerService lib = ERPW.Lib.Master.CustomerService.getInstance();

            foreach (DataRow dr in dt.Rows)
            {
                dr["NAME1"] = lib.PrepareCustomerNameAndForeignName(dr["NAME1"].ToString(), dr["NAME2"].ToString());
            }

            dt.DefaultView.RowFilter = "NAME1 <> ''";

            return dt.DefaultView.ToTable();//NAME1,BOBJECTLINK
        }

        public DataTable getContactPersonV2(string SID, string CompanyCode, string BpCode, string ContactCode)
        {
            string sql = @"SELECT b.BOBJECTLINK,(b.NAME1+' '+b.NAME2) as NAME1
                            ,case when ISNULL(b.POSITION, '') = '' then 'ไม่ระบุตำแหน่ง' else b.POSITION END  as POSITION
                            ,contact_phone.phone,contact_email.EMAIL,ISNULL(contact_address.address,'ไม่ระบุที่อยู่') as address,b.REMARK1 as Remark 
                            FROM CONTACT_MASTER a  WITH (NOLOCK) 
                            inner join CONTACT_DETAILS b  WITH (NOLOCK) 
                            on a.SID = b.SID and a.AOBJECTLINK = b.AOBJECTLINK
                            left join (
                                SELECT  sid, BOBJECTLINK
                                , STUFF(
                                    (SELECT ', ' + isnull(PHONENUMBER,'') + isnull(EXT,'')   
                                        FROM CONTACT_PHONE b
                                        where a.BOBJECTLINK = b.BOBJECTLINK
                                        FOR XML PATH(''))  ,1,1,'') as phone

                                FROM CONTACT_PHONE a 
                                    group by  sid, BOBJECTLINK
                            ) contact_phone on  contact_phone.sid = b.sid and  contact_phone.BOBJECTLINK = b.bobjectlink
                            left join (
                                    SELECT  sid, BOBJECTLINK
                                    , STUFF(
                                    (SELECT ', ' + isnull(EMAIL, '')
                                        FROM  CONTACT_EMAIL b
                                        where a.BOBJECTLINK = b.BOBJECTLINK
                                        FOR XML PATH(''))  ,1,1,'') as EMAIL

                                    FROM  CONTACT_EMAIL a
                                    group by sid, BOBJECTLINK
                            ) contact_email on  contact_email.sid = b.sid and  contact_email.BOBJECTLINK = b.bobjectlink
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
	                            group by sid, AddressCode,CustomerCode
                            ) contact_address on contact_address.sid = a.sid and contact_address.CustomerCode = a.BPCODE
                            where a.SID='" + SID + @"'
                            and a.COMPANYCODE='" + CompanyCode + @"'
                            and a.BPCODE='" + BpCode + @"'";

            if (!string.IsNullOrEmpty(ContactCode))
            {
                string[] ContactCodeArray = ContactCode.Split(',');
                string sqlIn = string.Join(",", ContactCodeArray.Select(a => "'" + a + "'"));

                sql += " and b.bobjectlink not in(" + sqlIn + ") ";
            }

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public DataTable getDataSaleCallContact(string SID, string ObjecID, string ContactCurrent, string mode)
        {
            string sql = "";
            if (string.IsNullOrEmpty(ContactCurrent) && mode != "delete")
            {
                sql = @"select servicecon.ItemNo,condetail.BOBJECTLINK,(condetail.NAME1+' '+condetail.NAME2) as NAME1
                            ,case when ISNULL(condetail.POSITION, '') = '' then 'ไม่ระบุตำแหน่ง' else condetail.POSITION END  as POSITION 
                            ,contact_phone.phone,contact_email.EMAIL,ISNULL(contact_address.address,'ไม่ระบุที่อยู่') as address,condetail.REMARK1 as Remark 
                            from cs_servicecall_contract servicecon WITH (NOLOCK) 
                            left outer join CONTACT_DETAILS condetail  WITH (NOLOCK) 
                            on servicecon.SID = condetail.SID and servicecon.PersonName = condetail.BOBJECTLINK
                            left outer join CONTACT_MASTER conmaster  WITH (NOLOCK) 
                            on conmaster.SID = condetail.SID and conmaster.AOBJECTLINK = condetail.AOBJECTLINK
                            left join (
                                SELECT  sid, BOBJECTLINK
                                , STUFF(
                                    (SELECT ', ' + isnull(PHONENUMBER,'') + isnull(EXT,'')   
                                     FROM CONTACT_PHONE b
                                     where a.BOBJECTLINK = b.BOBJECTLINK
                                     FOR XML PATH(''))  ,1,1,'') as phone

                                FROM CONTACT_PHONE a 
                                 group by  sid, BOBJECTLINK
                            ) contact_phone on  contact_phone.sid = condetail.sid and  contact_phone.BOBJECTLINK = condetail.bobjectlink
                            left join (
                                 SELECT  sid, BOBJECTLINK
                                 , STUFF(
                                    (SELECT ', ' + isnull(EMAIL, '')
                                     FROM  CONTACT_EMAIL b
                                     where a.BOBJECTLINK = b.BOBJECTLINK
                                     FOR XML PATH(''))  ,1,1,'') as EMAIL

                                 FROM  CONTACT_EMAIL a
                                 group by sid, BOBJECTLINK
                            ) contact_email on  contact_email.sid = condetail.sid and  contact_email.BOBJECTLINK = condetail.bobjectlink
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
	                            group by sid, AddressCode,CustomerCode
                            ) contact_address on contact_address.sid = servicecon.sid and contact_address.CustomerCode = conmaster.BPCODE
                            where servicecon.SID='" + SID + @"' and servicecon.ObjectID='" + ObjecID + @"'";
            }
            else
            {
                string[] ContactCurrentArray = ContactCurrent.Split(',');

                string sqlIn = string.Join(",", ContactCurrentArray.Select(a => "'" + a + "'"));

                sql += @"select '' as ItemNo,contact.BOBJECTLINK,contact.NAME1 + ' ' + contact.NAME2 as NAME1
                        ,case when ISNULL(contact.POSITION, '') = '' then 'ไม่ระบุตำแหน่ง' else contact.POSITION END  as POSITION
                        ,contact_phone.phone,contact_email.EMAIL
                        ,ISNULL(contact_address.address,'ไม่ระบุที่อยู่') as address,contact.REMARK1 as Remark 
                        from CONTACT_DETAILS contact WITH (NOLOCK) 
                        left outer join CONTACT_MASTER conmaster  WITH (NOLOCK) 
                        on conmaster.SID = contact.SID and conmaster.AOBJECTLINK = contact.AOBJECTLINK  
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
	                        group by sid, AddressCode,CustomerCode
                        ) contact_address on contact_address.sid = conmaster.SID and contact_address.CustomerCode = conmaster.BPCODE
                        where contact.SID='" + SID + "' and contact.BOBJECTLINK in(" + sqlIn + ")";
            }

            return dbService.selectDataFocusone(sql);
        }

        public DataTable GetPriority(string sid)
        {
            string sql = "select * from master_config_priority WITH (NOLOCK)  where SID='" + sid + "' and ProjectCode=''";

            DataTable dt = dbService.selectDataFocusone(sql);

            foreach (DataRow dr in dt.Rows)
            {
                dr["Description"] = GenerateCodeAndDesc(dr["PriorityCode"].ToString(), dr["Description"].ToString());
            }

            return dt;
        }

        public DataTable GetHaste(string sid)
        {
            string sql = "select * from master_config_haste WITH (NOLOCK)  where SID='" + sid + "' and ProjectCode=''";

            DataTable dt = dbService.selectDataFocusone(sql);

            foreach (DataRow dr in dt.Rows)
            {
                dr["Description"] = GenerateCodeAndDesc(dr["HasteCode"].ToString(), dr["Description"].ToString());
            }

            return dt;
        }
        #region Incident Area
        #region Incident Area Structure Master
        public DataTable GetDTProblemGroup(string sid, string businessObject)
        {
            List<string> sql_arr = new List<string>();
            sql_arr.Add("select * from ERPW_TICKET_AREA_PROBLEM_GROUP_MASTER WITH (NOLOCK)  where SID = '" + sid + "' and BUSINESSOBJECT='" + businessObject + "' ");
            DataTable dt = dbService.selectDataFocusone(string.Join(" ", sql_arr.ToArray()));

            dt.TableName = "ERPW_TICKET_AREA_PROBLEM_GROUP_MASTER";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["BUSINESSOBJECT"], dt.Columns["GROUPCODE"] };

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dr["GROUPNAME"] = GenerateCodeAndDesc(dr["GROUPCODE"].ToString(), dr["GROUPNAME"].ToString());
            //}

            return dt;//GROUPCODE,GROUPNAME
        }

        public DataTable GetDTProblemType(string SID, string BusinessObject, string ProblemGroup)
        {
            List<string> sql_arr = new List<string>();
            sql_arr.Add("select * from ERPW_TICKET_AREA_PROBLEM_TYPE_MASTER WITH (NOLOCK)  where SID = '" + SID + "' and BUSINESSOBJECT='" + BusinessObject + "' ");
            if (!string.IsNullOrEmpty(ProblemGroup))
                sql_arr.Add(" and GROUPCODE = '" + ProblemGroup + "'");
            DataTable dt = dbService.selectDataFocusone(string.Join(" ", sql_arr.ToArray()));

            dt.TableName = "ERPW_TICKET_AREA_PROBLEM_TYPE_MASTER";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["BUSINESSOBJECT"], dt.Columns["GROUPCODE"], dt.Columns["TYPECODE"] };

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dr["TYPENAME"] = GenerateCodeAndDesc(dr["TYPECODE"].ToString(), dr["TYPENAME"].ToString());
            //}

            return dt;//TYPECODE,TYPENAME
        }


        public DataTable DTProblemSource(string SID, string BusinessObject, string ProblemGroup, string IncidentType)
        {
            List<string> sql_arr = new List<string>();
            sql_arr.Add("select * from ERPW_TICKET_AREA_PROBLEM_SOURCE_MASTER WITH (NOLOCK)  where SID = '" + SID + "' and BUSINESSOBJECT='" + BusinessObject + "' ");
            if (!string.IsNullOrEmpty(ProblemGroup))
                sql_arr.Add(" and GROUPCODE = '" + ProblemGroup + "'");
            if (!string.IsNullOrEmpty(IncidentType))
                sql_arr.Add(" and TYPECODE = '" + IncidentType + "'");
            DataTable dt = dbService.selectDataFocusone(string.Join(" ", sql_arr.ToArray()));

            dt.TableName = "ERPW_TICKET_AREA_PROBLEM_SOURCE_MASTER";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["BUSINESSOBJECT"], dt.Columns["GROUPCODE"], dt.Columns["TYPECODE"], dt.Columns["SOURCECODE"] };

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dr["SOURCENAME"] = GenerateCodeAndDesc(dr["SOURCECODE"].ToString(), dr["SOURCENAME"].ToString());
            //}

            return dt;//SOURCECODE,SOURCENAME
        }

        public DataTable GetDTProblemCallType(string sid, string businessobject
            , string ProblemGroup, string IncidentType, string IncidentSource)
        {
            List<string> sql_arr = new List<string>();
            sql_arr.Add("select * from ERPW_TICKET_AREA_CONTACT_SOURCE_MASTER WITH (NOLOCK)  where SID = '" + ERPWAuthentication.SID + "' and BUSINESSOBJECT='" + businessobject + "'  ");
            if (!string.IsNullOrEmpty(ProblemGroup))
                sql_arr.Add(" and GROUPCODE = '" + ProblemGroup + "'");
            if (!string.IsNullOrEmpty(IncidentType))
                sql_arr.Add(" and TYPECODE = '" + IncidentType + "'");
            if (!string.IsNullOrEmpty(IncidentSource))
                sql_arr.Add(" and SOURCECODE = '" + IncidentSource + "'");

            sql_arr.Add(" order by AREACODE asc , AREANAME asc  ");
            DataTable dt = dbService.selectDataFocusone(string.Join(" ", sql_arr.ToArray()));

            dt.TableName = "ERPW_TICKET_AREA_CONTACT_SOURCE_MASTER";
            dt.PrimaryKey = new DataColumn[] {
                dt.Columns["SID"],
                dt.Columns["BUSINESSOBJECT"],
                dt.Columns["GROUPCODE"],
                dt.Columns["TYPECODE"],
                dt.Columns["SOURCECODE"],
                dt.Columns["AREACODE"]
            };

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dr["AREANAME"] = GenerateCodeAndDesc(dr["AREACODE"].ToString(), dr["AREANAME"].ToString());
            //}

            return dt;//AREACODE,AREANAME
        }

        #endregion

        #endregion

        public DataTable getProjectCode(string SID, string WorkgroupCode, string CustomerCode)
        {
            string sql = @"select project.ProjectCode,project.ProjectName as Description from LINK_ACCOUNTABILITY_INITIATIVE_CUSTOMER customer WITH (NOLOCK) 
                            left outer join LINK_PROJECT_COMPANY_STRUCTURE_MAPPING_UNIVERSAL_SERVICE structure   WITH (NOLOCK) 
                            on customer.SID = structure.SID and customer.WorkGroupCode = structure.WorkGroupCode 
                            and customer.CompanyStructureCode = structure.StructureCode
                            left outer join link_project_master project WITH (NOLOCK)  on project.SID = structure.SID and project.ProjectCode = structure.ProjectCode
                            where customer.SID='" + SID + "' and customer.WorkGroupCode='" + WorkgroupCode + "' and customer.CustomerCode='" + CustomerCode + @"'
                            order by project.ProjectName desc";
            DataTable dt = new DataTable();
            dt = dbService.selectDataFocusone(sql);

            return dt.DefaultView.ToTable();//ProjectCode,Description
        }

        public DataTable getProjectElement(string SID, string CompanyCode, string ProjectCode)
        {
            string sql = "select SubProjectCode as BOMID,Name as ELEMENTDESC from LINK_PROJECT_MASTER_SUB_PROJECT WITH (NOLOCK)  where SID='" + SID + "' and CompanyCode='" + CompanyCode + "' and ProjectCode='" + ProjectCode + "'";

            DataTable dt = new DataTable();
            dt = dbService.selectDataFocusone(sql);
            return dt;//BOMID,ELEMENTDESC
        }

        public DataTable getCallStatus(string statuscode)
        {
            string sql = "select * from master_callstatus WITH (NOLOCK)  where SID='" + ERPWAuthentication.SID + "' ";
            if (!string.IsNullOrEmpty(statuscode))
                sql += " and Name='" + statuscode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            foreach (DataRow dr in dt.Rows)
            {
                dr["Description"] = GenerateCodeAndDesc(dr["NAME"].ToString(), dr["Description"].ToString());
            }

            return dt;
        }

        public DataTable GetSevierity()
        {
            List<string> sql_arr = new List<string>();
            sql_arr.Add("select * from master_severity WITH (NOLOCK)  where SID = '" + ERPWAuthentication.SID + "'");
            DataTable dt = dbService.selectDataFocusone(string.Join(" ", sql_arr.ToArray()));

            dt.TableName = "master_severity";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["SeverityCode"] };
            return dt;//SeverityCode,Description
        }

        public DataTable getContactPhon(string BOBJECTLINK)
        {
            string sql = "select CONUNTRYCODE,(CONUNTRYCODE+' '+PHONENUMBER)as PHONENUMBER,EXT,PHONETYPE from CONTACT_PHONE WITH (NOLOCK)  where SID='" + ERPWAuthentication.SID + "' ";
            if (!string.IsNullOrEmpty(BOBJECTLINK))
                sql += " and BOBJECTLINK='" + BOBJECTLINK + "'";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;//PHONENUMBER
        }

        public DataTable getContactEmail(string BOBJECTLINK)
        {
            string sql = "select * from CONTACT_EMAIL WITH (NOLOCK)  where SID='" + ERPWAuthentication.SID + "' ";
            if (!string.IsNullOrEmpty(BOBJECTLINK))
                sql += " and BOBJECTLINK='" + BOBJECTLINK + "'";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;//EMAIL
        }

        public DataTable getContactAddress(string BOBJECTLINK)
        {
            string sql = "select * from CONTACT_ADDRESS WITH (NOLOCK)  where SID='" + ERPWAuthentication.SID + "' ";
            if (!string.IsNullOrEmpty(BOBJECTLINK))
                sql += " and BOBJECTLINK='" + BOBJECTLINK + "'";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;//ADDRESSTEXT
        }

        public DataTable getSearchEQInfo(string ownerCode, string equipmentCode)
        {
            return getSearchEQInfo(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ownerCode, equipmentCode);
        }

        public DataTable getSearchEQInfo(string sid, string companycode, string ownerCode, string equipmentCode)
        {
            string where = "";

            if (!string.IsNullOrEmpty(ownerCode))
            {
                where += " and G.OwnerCode='" + ownerCode + "'";
            }

            if (!string.IsNullOrEmpty(equipmentCode))
            {
                where += " and A.EquipmentCode='" + equipmentCode + "'";
            }

            DataTable dt = lookupICMService.GetSearchData(sid,
                    "SHJEQ001_V1", "#where A.SID='"
                        + sid + "' AND A.CompanyCode = '" + companycode + "' " + where);
            return dt;//EquipmentCode,EquipmentName
        }


        public DataTable getTierOperation(string SID, string TierCode, string ServiceDocNo)
        {
            string sql = @"SELECT item.SID
                            , item.WorkGroupCode
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.TierCode 
                              ELSE TierSelect.TierCode 
                            END AS TierCode 
                            , item.Tier
                            , item.TierDescription
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.[Role] 
                              ELSE TierSelect.[Role] 
                            END AS [Role] 
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.DefaultMain 
                              ELSE TierSelect.DefaultMain 
                            END AS DefaultMain 
                            , item.[sequence]
                            , item.Created_By
                            , item.Created_On
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.Resolution 
                              ELSE TierSelect.Resolution 
                            END AS Resolution 
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.Requester 
                              ELSE TierSelect.Requester 
                            END AS Requester 
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.HeadShift 
                              ELSE TierSelect.HeadShift 
                            END AS HeadShift 
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.AVPSale 
                              ELSE TierSelect.AVPSale 
                            END AS AVPSale 
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.SVPSale 
                              ELSE TierSelect.SVPSale 
                            END AS SVPSale 
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.StatusReporter 
                              ELSE TierSelect.StatusReporter 
                            END AS StatusReporter 
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN item.DynamicOwner 
                              ELSE TierSelect.DynamicOwner 
                            END AS DynamicOwner 
                            ,ISNULL(map.AOBJECTLINK, '') AS AOBJECTLINK
                            ,ISNULL(timeat.Subject, '') AS Subject
                            ,ISNULL(timeat.Remark, '') AS Remark	
                            ,ISNULL(timeat.StartDateTime, '') AS StartDateTime
                            ,ISNULL(timeat.EndDateTime, '') AS EndDateTime
                            ,timeat.Status as HeaderStatus
                            ,CountRemark.xCountRemark
                            ,CASE 
                              WHEN timeat.SLAGroupCode_Select is null or timeat.SLAGroupCode_Select = ''
                              THEN master.TierGroupCode 
                              ELSE timeat.SLAGroupCode_Select 
                            END AS TierGroupCode 
                            , timeat.OwnerService_Select
                            , timeat.SLAGroupCode_Select
    
                          from Link_Tier_Master_Item item  WITH (NOLOCK) 
                          inner join Link_Tier_Master master WITH (NOLOCK) 
                            on master.SID = item.SID
                            and master.WorkGroupCode = item.WorkGroupCode
                            and master.TierCode = item.TierCode
                          left outer join CRM_SERVICECALL_MAPPING_ACTIVITY map  WITH (NOLOCK) 
                            on item.Tier = map.Tier 
                            --and item.TierCode = map.TierCode 
                            and map.ServiceDocNo = '" + ServiceDocNo + @"'
                          left outer join ticket_service_header timeat  WITH (NOLOCK) 
                            on timeat.SID = item.SID
                            and timeat.CompanyCode = map.SNAID
                            and timeat.TicketCode = map.AOBJECTLINK

                        outer apply 
                        (
                          select b.*
                          from Link_Tier_Master a WITH (NOLOCK) 
                          inner join Link_Tier_Master_Item b WITH (NOLOCK) 
                            on a.SID = b.SID
                            and a.WorkGroupCode = b.WorkGroupCode
                            and a.TierCode = b.TierCode
                          inner join cs_servicecall_header c WITH (NOLOCK) 
                            on c.CallerID = '" + ServiceDocNo + @"'
                            and c.SID = a.SID
                          where a.SID = item.SID
                            and a.WorkGroupCode = item.WorkGroupCode
                            and a.TierGroupCode = timeat.SLAGroupCode_Select
                            and a.PriorityCode = c.Priority
                            and b.Tier = item.Tier
                        ) TierSelect

                          outer apply 
                          (
                            select Count(*) as xCountRemark
                            from SNA_ACTIVITY_REMARK WITH (NOLOCK) 
                            where OBJECTLINK = map.AOBJECTLINK
                          ) CountRemark

                          where item.SID='" + SID + @"'
                          and item.TierCode='" + TierCode + @"'
                          order by ABS(item.sequence), map.created_on asc";

            return dbService.selectDataFocusone(sql);
        }


        public DataTable getOwnerOperation(string CompanyCode, string DocYear, string ServiceDocNo)
        {
            string sql = @"select * from CRM_SERVICECALL_MAPPING_ACTIVITY map WITH (NOLOCK) 
                          outer apply 
                          (
                            select Count(*) as xCountRemark
                            from SNA_ACTIVITY_REMARK WITH (NOLOCK) 
                            where OBJECTLINK = map.AOBJECTLINK
                          ) CountRemark

                          where map.SNAID = '" + CompanyCode + @"'
	                        and map.DOCYEAR = '" + DocYear + @"'
	                        and map.ServiceDocNo = '" + ServiceDocNo + @"'";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getTierItem(string SID, string WorkGroupCode, string TierCode)
        {
            string sql = @"select * from dbo.Link_Tier_Master_Item WITH (NOLOCK)  where SID='" + SID + "' and WorkGroupCode='" + WorkGroupCode + @"' 
                            and TierCode='" + TierCode + "'";

            return dbService.selectDataFocusone(sql);
        }

        private DataTable GetTierEmployee(string sid, string companyCode, string tierCode, string tier, string ticketType,
            /*string incidentArea,*/ bool mainDelegate, string TicketCode, string OwnerCode)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("EmployeeCode", typeof(System.String)).DefaultValue = "";
            dtResult.Columns.Add("fullname", typeof(System.String)).DefaultValue = "";
            dtResult.Columns.Add("HierarchyDesc", typeof(System.String)).DefaultValue = "";
            dtResult.Columns.Add("AuthenTransferTask", typeof(System.String)).DefaultValue = "FALSE";
            dtResult.Columns.Add("AuthenCloseTask", typeof(System.String)).DefaultValue = "FALSE";

            string sql = @"SELECT Role, DefaultMain, DynamicOwner 
                           FROM Link_Tier_Master_Item WITH (NOLOCK)  WHERE SID = '" + sid + "' AND TierCode = '" + tierCode + "' AND Tier = '" + tier + "'";

            DataTable dtTier = dbService.selectDataFocusone(sql);

            if (dtTier.Rows.Count > 0)
            {
                string role = dtTier.Rows[0]["Role"].ToString().Trim();
                string defaultMain = dtTier.Rows[0]["DefaultMain"].ToString().Trim();

                bool dynamicOwner = false;
                bool.TryParse(dtTier.Rows[0]["DynamicOwner"].ToString(), out dynamicOwner);

                if (dynamicOwner)
                {
                    sql = @"SELECT Role FROM ERPW_TIER_OWNER_MAPPING_ROLE  WITH (NOLOCK) 
                                WHERE SID = '" + sid + "' AND TierCode = '" + tierCode + "' AND Tier = '" + tier + "' AND OwnerGroupCode = '" + OwnerCode + "'";
                    DataTable dtRoleMapping = dbService.selectDataFocusone(sql);
                    if (dtRoleMapping.Rows.Count > 0)
                    {
                        role = dtRoleMapping.Rows[0]["Role"].ToString();
                    }

                }
                else if (defaultMain != "")
                {
                    if (mainDelegate)
                    {
                        DataTable dtMainDelegate = getTierDefaultMain(sid, companyCode, crm.AfterSale.ServiceCallTransaction.WorkGroupCode, tierCode, tier);

                        if (dtMainDelegate.Rows.Count > 0)
                        {
                            dtResult.Rows.Add(
                                dtMainDelegate.Rows[0]["EmployeeCode"].ToString(),
                                dtMainDelegate.Rows[0]["fullname"].ToString(),
                                dtMainDelegate.Rows[0]["TierDescription"].ToString(),
                                dtMainDelegate.Rows[0]["AuthenTransferTask"].ToString(),
                                dtMainDelegate.Rows[0]["AuthenCloseTask"].ToString()
                            );

                            return dtResult;
                        }
                    }
                    else
                    {
                        DataTable dtParticipants = getTierDefaultParticipant(sid, companyCode, crm.AfterSale.ServiceCallTransaction.WorkGroupCode, tierCode, tier);

                        foreach (DataRow dr in dtParticipants.Rows)
                        {
                            dtResult.Rows.Add(
                                dr["EmployeeCode"].ToString(),
                                dr["fullname"].ToString(),
                                dr["HierarchyDesc"].ToString(),
                                dr["AuthenTransferTask"].ToString(),
                                dr["AuthenCloseTask"].ToString()
                            );
                        }

                        //TODO Add Other Delegate
                        return dtResult;
                    }
                }

                if (role != "")
                {
                    DataTable dt = new DataTable();
                    if (mainDelegate && !string.IsNullOrEmpty(TicketCode))
                    {
                        dt = getSLASetMain(sid, companyCode, role, TicketCode);
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt = CharacterService.getInstance().getRoleMappingEmployee(sid, companyCode, role, mainDelegate);
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtResult.Rows.Add(
                            dr["EmployeeCode"].ToString(),
                            dr["FullName_EN"].ToString(),
                            dr["RoleDescript"].ToString(),
                            dr["AuthenTransferTask"].ToString(),
                            dr["AuthenCloseTask"].ToString()
                        );
                    }
                }
            }

            if (!mainDelegate)
            {
                if (!string.IsNullOrEmpty(TicketCode))
                {
                    List<string> listEmp = new List<string>();
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        listEmp.Add(Convert.ToString(dr["EmployeeCode"]));
                    }

                    string sql_participants = @"select parti.EmployeeCode, emp.FirstName + ' ' + emp.LastName as fullname
	                                    , '(Transfer)' as HierarchyDesc , 'FALSE' as AuthenTransferTask, 'FALSE' as AuthenCloseTask
                                    from ticket_service_participants parti WITH (NOLOCK) 
                                    left join master_employee emp  WITH (NOLOCK) 
	                                    on parti.SID = emp.SID 
	                                    and emp.CompanyCode = '" + companyCode + @"'
	                                    and parti.EmployeeCode = emp.EmployeeCode
                                    where parti.SID = '" + sid + @"'
	                                    and parti.TicketCode = '" + TicketCode + @"'
	                                    and parti.EmployeeCode not in ('" + string.Join("', '", listEmp) + "')";

                    DataTable dt_participants = dbService.selectDataFocusone(sql_participants);
                    dtResult.Merge(dt_participants);
                }
            }


            return dtResult;
        }

        public DataTable GetTierMainDelegate(string sid, string companyCode, string tierCode, string tier, string ticketType,
            /*string incidentArea,*/ string TicketCode = "", string OwnerCode = "")
        {
            return GetTierEmployee(sid, companyCode, tierCode, tier, ticketType, /*incidentArea,*/ true, TicketCode, OwnerCode);
        }

        public DataTable GetTierParticipants(string sid, string companyCode, string tierCode, string tier, string ticketType
            , /*string incidentArea,*/ string TicketCode, string OwnerCode = "")
        {
            return GetTierEmployee(sid, companyCode, tierCode, tier, ticketType, /*incidentArea,*/ false, TicketCode, OwnerCode);
        }

        public DataTable getTierDefaultMain(string SID, string CompanyCode, string WorkGroupCode, string TierCode, string Tier)
        {
            string sql = @"select item.DefaultMain as EmployeeCode, hierar.HierarchyDesc
                                , emp.FirstName + ' ' + emp.LastName as fullname
                                , 'TRUE' as AuthenTransferTask, 'TRUE' as AuthenCloseTask
                                , item.TierDescription
                            from Link_Tier_Master_Item item WITH (NOLOCK) 
                            left outer join master_employee emp  WITH (NOLOCK) 
                                on item.SID = emp.SID and item.DefaultMain = emp.EmployeeCode                             
	                            and emp.CompanyCode = '" + CompanyCode + @"'
                            left outer join ep_master_conf_hierarchy hierar WITH (NOLOCK) 
                                on item.SID = hierar.Sid and item.Role = hierar.HierarchyCode
                            where item.SID='" + SID + "' and item.WorkGroupCode='" + WorkGroupCode + @"' 
                            and item.TierCode='" + TierCode + "' and item.Tier='" + Tier + "'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getTierDefaultParticipant(string SID, string CompanyCode, string WorkGroupCode, string TierCode, string Tier)
        {
            string sql = @"select par.DefaultParticipant as EmployeeCode, emp.FirstName + ' ' + emp.LastName as fullname,hierar.HierarchyDesc from Link_Tier_Master_Item_Participant par WITH (NOLOCK) 
                            left outer join master_employee emp WITH (NOLOCK)  
                                on par.SID = emp.SID 
                                and par.DefaultParticipant = emp.EmployeeCode                             
	                            and emp.CompanyCode = '" + CompanyCode + @"'
                            left outer join ep_master_conf_hierarchy hierar WITH (NOLOCK)  on par.SID = hierar.Sid and par.Role = hierar.HierarchyCode
                            where par.SID='" + SID + "' and par.WorkGroupCode='" + WorkGroupCode + @"' 
                            and par.TierCode='" + TierCode + "' and par.Tier='" + Tier + "'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getSLASetMain(string sid, string CompanyCode, string Role, string AobjLink)
        {
            string sql = @"select b.HierarchyCode
	                            , b.HierarchyDesc as RoleDescript
	                            , a.MainDelegate as EmployeeCode
	                            , 'TRUE' as IsMain
	                            , 'TRUE' as AuthenTransferTask
	                            , 'TRUE' as AuthenCloseTask
                                , c.FirstName + ' ' + c.LastName as FullName_EN
                                , c.FirstName_TH + ' ' + c.LastName_TH as FullName_TH
                                , c.UPDATED_ON as UpdateOnEmployee

                            from ticket_service_header a WITH (NOLOCK) 
                            left join ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY b WITH (NOLOCK) 
	                            on a.SID = b.Sid
	                            and b.HierarchyCode = '" + Role + @"'
                            inner join master_employee c WITH (NOLOCK) 
                                on a.Sid = c.SID
                                and a.CompanyCode = c.CompanyCode
                                and a.MainDelegate = c.EmployeeCode

                            where a.sid = '" + sid + @"'
	                            and a.CompanyCode = '" + CompanyCode + @"'	
	                            and a.TicketCode = '" + AobjLink + @"'
	                            and a.MainDelegate <> ''
	                            and a.MainDelegate is not null";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public void SaveServicecallMappingActivity(string SNAID, string AobjectLink, string Year, string ServiceDocNo, string EquipmentNo, string TierCode, string Tier
            , string ItemNo, string CreatedBy, string CreatedOn)
        {

            try
            {
                string sql = @"insert into CRM_SERVICECALL_MAPPING_ACTIVITY (SNAID,AOBJECTLINK,DOCYEAR,ServiceDocNo,EquipmentNo,TierCode,Tier,ItemNo,created_by,created_on) values ('" + SNAID + "','" + AobjectLink + @"'
                            ,'" + Year + "','" + ServiceDocNo + "','" + EquipmentNo + "','" + TierCode + "','" + Tier + "','" + ItemNo + "','" + CreatedBy + "','" + CreatedOn + "')";
                dbService.executeSQLForFocusone(sql);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().IndexOf("duplicate key") > -1)
                {
                    throw new Exception("There are changes in the tier, Please do the transaction again.");
                }
                else
                {
                    throw ex;
                }
            }
        }

        public DataTable getAobjectlinkByServiceDoc(string SNAID, string ServiceDocNo, string DocYear)
        {
            string sql = @"select top 1 * from CRM_SERVICECALL_MAPPING_ACTIVITY WITH (NOLOCK)  where SNAID='" + SNAID + "' and ServiceDocNo='" + ServiceDocNo + @"' 
                            and DOCYEAR='" + DocYear + @"'
                            order by created_on desc";

            return dbService.selectDataFocusone(sql);
        }

        public int countActivityServiceCall(string SNAID, string ServiceDocNo, string EquipmentNo, string ItemNo)
        {
            string sql = @"select * from CRM_SERVICECALL_MAPPING_ACTIVITY  WITH (NOLOCK) 
                            where SNAID='" + SNAID + @"' 
                            and ServiceDocNo='" + ServiceDocNo + @"' 
                            --and EquipmentNo='" + EquipmentNo + @"'
                            and ItemNo='" + ItemNo + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt.Rows.Count;
        }

        public int tierActivityServiceCall(string SNAID, string ServiceDocNo, string EquipmentNo, string ItemNo)
        {
            string sql = @"select top 1 STUFF(Tier, 1, 2, '') AS Activity from CRM_SERVICECALL_MAPPING_ACTIVITY  WITH (NOLOCK) 
                            where SNAID='" + SNAID + @"' 
                            and ServiceDocNo='" + ServiceDocNo + @"' 
                            --and EquipmentNo='" + EquipmentNo + @"'
                            and ItemNo='" + ItemNo + @"'
                            order by Activity desc";

            DataTable dt = dbService.selectDataFocusone(sql);
            var Activity = 0;
            foreach (DataRow dr in dt.Rows)
            {
                var num = dt.Rows[0]["Activity"].ToString();
                Activity = Int16.Parse(num);
            }

            return Activity;
        }

        public string getLastActivityServiceCall(string SNAID, string ServiceDocNo, string EquipmentNo, string ItemNo)
        {
            string sql = @"select top 1 * from CRM_SERVICECALL_MAPPING_ACTIVITY 
                            where SNAID= '" + SNAID + @"' 
                            and ServiceDocNo='" + ServiceDocNo + @"' ";

            //if (string.IsNullOrEmpty(EquipmentNo))
            //{
            //    sql += " and EquipmentNo='" + EquipmentNo + @"' ";
            //}
            if (!string.IsNullOrEmpty(ItemNo))
            {
                sql += " and ItemNo='" + ItemNo + "' ";
            }

            sql += " order by created_on DESC ";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dt.Rows[0]["AOBJECTLINK"].ToString();
            }
        }

        public DataTable GetTierGroup(string sid)
        {
            string sql = @"SELECT * FROM Link_TierGroup_Master WITH (NOLOCK)  WHERE SID = '" + sid + "'";


            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                sql += @" and OwnerService = '" + ERPWAuthentication.Permission.OwnerGroupCode + "' ";
            }

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }


        public string GetSLAGroupCode(string sid, string companyCode, string customerCode, string equipmentCode, string priorityCode)
        {
            string TierCode = "";
            DataTable dt = getDataTierServiceTicket(sid, companyCode, customerCode, equipmentCode, priorityCode);

            if (dt.Rows.Count > 0)
            {
                TierCode = dt.Rows[0]["TierGroupCode"].ToString();
            }

            return TierCode;
        }

        public string GetTierCode(string sid, string companyCode, string customerCode, string equipmentCode, string priorityCode)
        {
            string TierCode = "";
            DataTable dt = getDataTierServiceTicket(sid, companyCode, customerCode, equipmentCode, priorityCode);

            if (dt.Rows.Count > 0)
            {
                TierCode = dt.Rows[0]["TierCode"].ToString();
            }

            return TierCode;
        }
        public string GetTierCode(string sid, string companyCode, string equipmentCode, string priorityCode)
        {
            string TierCode = "";
            DataTable dt = getDataTierServiceTicket(sid, companyCode, equipmentCode, priorityCode);

            if (dt.Rows.Count > 0)
            {
                TierCode = dt.Rows[0]["TierCode"].ToString();
            }

            return TierCode;
        }
        private DataTable getDataTierServiceTicket(string sid, string companyCode, string equipmentCode, string priorityCode)
        {
            string sql = @"SELECT b.TierCode
                           	,b.TierName
	                           ,b.TierGroupCode
	                           ,b.PriorityCode
                           FROM master_equipment_owner_assignment a WITH (NOLOCK) 
                           INNER JOIN Link_Tier_Master b WITH (NOLOCK)  ON a.SID = b.SID
	                           AND a.SLAGroupCode = b.TierGroupCode
                           WHERE a.SID = '" + sid + @"'
	                           AND a.CompanyCode = '" + companyCode + @"'	                           
	                           AND a.OwnerType = '01'
	                           AND a.EquipmentCode = '" + equipmentCode + @"'
	                           AND b.PriorityCode = '" + priorityCode + @"'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        private DataTable getDataTierServiceTicket(string sid, string companyCode, string customerCode, string equipmentCode, string priorityCode)
        {
            string sql = @"SELECT b.TierCode
                           	,b.TierName
	                           ,b.TierGroupCode
	                           ,b.PriorityCode
                           FROM master_equipment_owner_assignment a WITH (NOLOCK) 
                           INNER JOIN Link_Tier_Master b WITH (NOLOCK)  ON a.SID = b.SID
	                           AND a.SLAGroupCode = b.TierGroupCode
                           WHERE a.SID = '" + sid + @"'
	                           AND a.CompanyCode = '" + companyCode + @"'	                           
	                           AND a.OwnerType = '01'
	                           AND a.OwnerCode = '" + customerCode + @"'
	                           AND a.EquipmentCode = '" + equipmentCode + @"'
	                           AND b.PriorityCode = '" + priorityCode + @"'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }



        public string getTierName(string SID, string WorkGroupCode, string TierCode)
        {
            string TierName = "";
            string sql = @"select * from Link_Tier_Master WITH (NOLOCK)  where SID='" + SID + "' and WorkGroupcode='" + WorkGroupCode + "' and TierCode='" + TierCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                TierName = dt.Rows[0]["TierName"].ToString();
            }

            return TierName;
        }

        public string CreatedTierServiceTicket(string sid, string companyCode, string subject, string remark,
            string mainDelegate, string[] participants, string createdBy, string startDateTime,
            double resolutionTimes, bool escalate)
        {
            return CreatedTierServiceTicket(sid, companyCode, subject, remark,
                 mainDelegate, participants, createdBy, startDateTime,
                 resolutionTimes, escalate, "", ""
            );
        }
        public string CreatedTierServiceTicket(string sid, string companyCode, string subject, string remark,
            string mainDelegate, string[] participants, string createdBy, string startDateTime,
            double resolutionTimes, bool escalate, string OwnerServiceCode, string SLAGroupCode)
        {
            List<string> listInsert = new List<string>();

            string ticketCode = Guid.NewGuid().ToString("N");
            string createdOn = Validation.getCurrentServerStringDateTime();

            if (string.IsNullOrEmpty(startDateTime))
            {
                startDateTime = Validation.getCurrentServerStringDateTime();
            }

            DateTime startDate = new DateTime(
                Convert.ToInt32(startDateTime.Substring(0, 4)),
                Convert.ToInt32(startDateTime.Substring(4, 2)),
                Convert.ToInt32(startDateTime.Substring(6, 2)),
                Convert.ToInt32(startDateTime.Substring(8, 2)),
                Convert.ToInt32(startDateTime.Substring(10, 2)),
                Convert.ToInt32(startDateTime.Substring(12, 2)));

            if (escalate)
            {
                startDate = startDate.AddSeconds(1);
            }

            DateTime endDate = startDate.AddSeconds(resolutionTimes);

            CultureInfo cul = new CultureInfo("en-US");

            string finalStartDate = Validation.Convert2DateTimeDB(startDate.ToString("dd/MM/yyyy HH:mm:ss", cul));
            string finalEndDate = Validation.Convert2DateTimeDB(endDate.ToString("dd/MM/yyyy HH:mm:ss", cul));

            string sqlHeader = @"INSERT INTO [ticket_service_header]
                                (
                                     [SID]
                                    ,[CompanyCode]
                                    ,[TicketCode]
                                    ,[Subject]
                                    ,[Remark]
                                    ,[MainDelegate]
                                    ,[ResolutionTime]
                                    ,[StartDateTime]
                                    ,[EndDateTime]
                                    ,[Status]
                                    ,[CREATED_BY]
                                    ,[CREATED_ON]
                                    ,[OwnerService_Select]
                                    ,[SLAGroupCode_Select]
                                )
                                VALUES
                                (
                                     '" + sid + @"'
                                    ,'" + companyCode + @"'
                                    ,'" + ticketCode + @"'
                                    ,'" + subject + @"'
                                    ,'" + remark + @"'
                                    ,'" + mainDelegate + @"'
                                    ,'" + resolutionTimes + @"'
                                    ,'" + finalStartDate + @"'
                                    ,'" + finalEndDate + @"'
                                    ,'Open'
                                    ,'" + createdBy + @"'
                                    ,'" + createdOn + @"'
                                    ,'" + OwnerServiceCode + @"'
                                    ,'" + SLAGroupCode + @"'
                                )";

            listInsert.Add(sqlHeader);

            if (participants != null && participants.Length > 0)
            {
                foreach (string emp in participants)
                {
                    string sqlItem = @"INSERT INTO [dbo].[ticket_service_participants]
                                            ([SID]
                                            ,[CompanyCode]
                                            ,[TicketCode]
                                            ,[EmployeeCode]
                                            ,[CREATED_BY]
                                            ,[CREATED_ON])
                                        VALUES
                                            ('" + sid + @"'
                                            ,'" + companyCode + @"'
                                            ,'" + ticketCode + @"'
                                            ,'" + emp + @"'
                                            ,'" + createdBy + @"'
                                            ,'" + createdOn + @"')";

                    listInsert.Add(sqlItem);
                }
            }

            if (listInsert.Count == 0)
            {
                throw new Exception("Insert list not found.");
            }

            dbService.executeSQLForFocusone(listInsert);

            return ticketCode;
        }

        public void CloseTierServiceTicket(string sid, string companyCode, string ticketCode)
        {
            string sql = @"UPDATE [dbo].[ticket_service_header] SET [Status] = 'Close'
                            where SID = '" + sid + @"'
	                            and CompanyCode = '" + companyCode + @"'
	                            and TicketCode = '" + ticketCode + @"'";

            dbService.executeSQLForFocusone(sql);
        }

        public DataTable getTimeofTicket(string ticketcode)
        {
            string sql = @"Select * FROM ticket_service_header WHERE TicketCode = '" + ticketcode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        #region Validation end work time
        //public double validationWorkTime(double resolutionTime)
        //{
        //    MasterWorkingTimeConfigLib workingtimeConfigLib = new MasterWorkingTimeConfigLib();
        //    List<WorkingTimeConfig> lst_wtEn = workingtimeConfigLib.GetWorkingTime_Config(
        //           ERPWAuthentication.SID,
        //           ERPWAuthentication.CompanyCode
        //    );

        //    if (lst_wtEn.Count > 0)
        //    {
        //        WorkingTimeConfig wtEn = lst_wtEn[0];

        //        DateTime timeNow = Validation.Convert2RadTimeDisplay(Validation.getCurrentServerTime());//เวลาสร้าง ticket
        //        DateTime dateTimeNow = Validation.getCurrentServerDateTime(); //วันและเวลาสร้าง ticket
        //        DateTime startTime = Validation.Convert2RadTimeDisplay(wtEn.StartTime);//เวลาเริ่มงาน
        //        DateTime endTime = Validation.Convert2RadTimeDisplay(wtEn.EndTime);//เวลาเลิกงาน
        //        DateTime overdueTime = timeNow.AddSeconds(resolutionTime);
        //        TimeSpan workTime = endTime - startTime;
        //        double oneDay = 24 * 60 * 60; //แปลงเวลา 1 วันเป็น วินาที
        //        double gapTime = oneDay - workTime.TotalSeconds; //หาระยะเวลาที่ไม่ได้ทำงาน

        //        if (checkHoliday(resolutionTime, dateTimeNow, wtEn.Workday) || timeNow >= endTime)//เปิด ticket หลังเลิกงาน และวันหยุด
        //        {
        //            TimeSpan gap = startTime.AddDays(1) - timeNow;
        //            resolutionTime += Convert.ToDouble((int)gap.TotalSeconds);
        //        }

        //        if (timeNow >= startTime && timeNow < endTime) { //ถ้าเปิด ticket ในเวลางาน ให้เข้ามาทำใน if
        //            if (overdueTime >= endTime)
        //            {
        //                resolutionTime += gapTime;
        //            }
        //        }
        //        else if (timeNow < startTime)
        //        {
        //            TimeSpan gapTimeBeforWorkTime = startTime - timeNow;
        //            resolutionTime += Convert.ToDouble((int)gapTimeBeforWorkTime.TotalSeconds);
        //        }
        //        resolutionTime = validationHoliday(resolutionTime, dateTimeNow, wtEn.Workday);//เช็ควันหยุดในอีกวันถัดไป
        //    }
        //    return resolutionTime;
        //}
        //public double validationHoliday(double resolutionTime, DateTime nowDate, string workDay)
        //{

        //    double gapTimeHoliday = 24 * 60 * 60; //แปลง 1 วัน เป็นวินาที
        //    while (checkHoliday(resolutionTime, nowDate, workDay))
        //    {
        //        resolutionTime += gapTimeHoliday;
        //    }
        //    return resolutionTime;
        //}
        //public bool checkHoliday(double resolutionTime, DateTime nowDate, string workDay)
        //{
        //    CultureInfo _cultureEnInfo = new CultureInfo("en-US");
        //    bool isHoliday = false;
        //    int indexDay = (int)nowDate.AddSeconds(resolutionTime).DayOfWeek;
        //    MasterWorkingTimeConfigLib lib = new MasterWorkingTimeConfigLib();
        //    List<string> list_holidays = lib.GetHolidaysByDate(
        //                                ERPWAuthentication.SID,
        //                                ERPWAuthentication.CompanyCode,
        //                                MasterWorkingTimeConfigLib.Holidays_TYPE,
        //                                Convert.ToDateTime(nowDate.AddSeconds(resolutionTime), _cultureEnInfo).ToString("yyyyMMdd", _cultureEnInfo));

        //    if (list_holidays.Count > 0 || workDay[indexDay] == '0')
        //    {
        //        isHoliday = true;
        //    }
        //    return isHoliday;
        //}
        //public string ConvertdbDateToDisplayDate(string dbDate)
        //{
        //    DateTime datetime_date = DateTime.ParseExact(dbDate,
        //                            "yyyyMMdd",
        //                             CultureInfo.CurrentCulture);
        //    string dispDate = String.Format("{0:yyyy/MM/dd}", datetime_date);
        //    return dispDate;
        //}

        public double CalculateNewResolutionTime(double resolutionTime)
        {
            TimeSpan tsResolutionTimeTemp = new TimeSpan(0, 0, Convert.ToInt32(resolutionTime));
            
            CultureInfo cul = new CultureInfo("en-US");
            DateTime dtCurrent = Validation.getCurrentServerDateTime();
            DateTime dueDateTime = dtCurrent;

            MasterWorkingTimeConfigLib workingtimeConfigLib = new MasterWorkingTimeConfigLib();
            List<WorkingTimeConfig> lst_wtEn = workingtimeConfigLib.GetWorkingTime_Config(
                   ERPWAuthentication.SID,
                   ERPWAuthentication.CompanyCode
            );

            if (lst_wtEn.Count > 0)
            {
                int hhStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(0, 2));
                int mmStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(2, 2));
                int ssStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(4, 2));
                int hhEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(0, 2));
                int mmEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(2, 2));
                int ssEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(4, 2));
                TimeSpan workdayStart = new TimeSpan(hhStart, mmStart, ssStart);
                TimeSpan workdayEnd = new TimeSpan(hhEnd, mmEnd, ssEnd);

                do
                {
                    List<string> list_holidays = workingtimeConfigLib.GetHolidaysByDate(
                                            ERPWAuthentication.SID,
                                            ERPWAuthentication.CompanyCode,
                                            MasterWorkingTimeConfigLib.Holidays_TYPE,
                                            Validation.Convert2DateTimeDB(dueDateTime.ToString("dd/MM/yyyy HH:mm:ss", cul)).Substring(0, 8));

                    if (lst_wtEn[0].Workday[(int)dueDateTime.DayOfWeek] == '0' || list_holidays.Contains(dueDateTime.ToString("yyyyMMdd", cul)))
                    {
                        DateTime dueDateTimeTemp = dueDateTime;

                        //jump to start of next day
                        dueDateTimeTemp = dueDateTimeTemp.AddDays(1);
                        dueDateTimeTemp = new DateTime(dueDateTimeTemp.Year, dueDateTimeTemp.Month, dueDateTimeTemp.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);

                        dueDateTime = dueDateTimeTemp;
                    }
                    else
                    {
                        DateTime startWorkDayTemp = new DateTime(dueDateTime.Year, dueDateTime.Month, dueDateTime.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);
                        DateTime endWorkDayTemp = new DateTime(dueDateTime.Year, dueDateTime.Month, dueDateTime.Day, workdayEnd.Hours, workdayEnd.Minutes, workdayEnd.Seconds);

                        if (dueDateTime > endWorkDayTemp) // เวลาเกินเลิกงาน
                        {
                            DateTime dueDateTimeTemp = dueDateTime;

                            //jump to start of next day
                            dueDateTimeTemp = dueDateTimeTemp.AddDays(1);
                            dueDateTimeTemp = new DateTime(dueDateTimeTemp.Year, dueDateTimeTemp.Month, dueDateTimeTemp.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);

                            dueDateTime = dueDateTimeTemp;
                        }
                        else
                        {
                            if (dueDateTime <= startWorkDayTemp)
                            {
                                dueDateTime = startWorkDayTemp;
                            }

                            TimeSpan tsDiff = endWorkDayTemp - dueDateTime;

                            if (tsResolutionTimeTemp > tsDiff)
                            {
                                tsResolutionTimeTemp = tsResolutionTimeTemp.Subtract(tsDiff); // ลบเวลาที่ทำไปแล้ว

                                DateTime dueDateTimeTemp = dueDateTime;
                                //jump to start of next day
                                dueDateTimeTemp = dueDateTimeTemp.AddDays(1);
                                dueDateTimeTemp = new DateTime(dueDateTimeTemp.Year, dueDateTimeTemp.Month, dueDateTimeTemp.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);

                                dueDateTime = dueDateTimeTemp;
                            }
                            else
                            {
                                dueDateTime = startWorkDayTemp.AddSeconds(tsResolutionTimeTemp.TotalSeconds);
                                tsResolutionTimeTemp = TimeSpan.Zero;
                            }
                        }

                    }
                } while (tsResolutionTimeTemp.Ticks > 0);
            } 
            else
            {
                dueDateTime = dueDateTime.AddSeconds(tsResolutionTimeTemp.TotalSeconds);
            }

            TimeSpan tsNewResolutionTime = dueDateTime - dtCurrent;

            return tsNewResolutionTime.TotalSeconds;

        }
        public double CalculateNewResolutionTime(double resolutionTime, DateTime startDatetime)
        {
            TimeSpan tsResolutionTimeTemp = new TimeSpan(0, 0, Convert.ToInt32(resolutionTime));

            CultureInfo cul = new CultureInfo("en-US");
            DateTime dtCurrent = startDatetime;
            DateTime dueDateTime = dtCurrent;

            MasterWorkingTimeConfigLib workingtimeConfigLib = new MasterWorkingTimeConfigLib();
            List<WorkingTimeConfig> lst_wtEn = workingtimeConfigLib.GetWorkingTime_Config(
                   ERPWAuthentication.SID,
                   ERPWAuthentication.CompanyCode
            );

            if (lst_wtEn.Count > 0)
            {
                int hhStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(0, 2));
                int mmStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(2, 2));
                int ssStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(4, 2));
                int hhEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(0, 2));
                int mmEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(2, 2));
                int ssEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(4, 2));
                TimeSpan workdayStart = new TimeSpan(hhStart, mmStart, ssStart);
                TimeSpan workdayEnd = new TimeSpan(hhEnd, mmEnd, ssEnd);

                do
                {
                    List<string> list_holidays = workingtimeConfigLib.GetHolidaysByDate(
                                            ERPWAuthentication.SID,
                                            ERPWAuthentication.CompanyCode,
                                            MasterWorkingTimeConfigLib.Holidays_TYPE,
                                            Validation.Convert2DateTimeDB(dueDateTime.ToString("dd/MM/yyyy HH:mm:ss", cul)).Substring(0, 8));

                    if (lst_wtEn[0].Workday[(int)dueDateTime.DayOfWeek] == '0' || list_holidays.Contains(dueDateTime.ToString("yyyyMMdd", cul)))
                    {
                        DateTime dueDateTimeTemp = dueDateTime;

                        //jump to start of next day
                        dueDateTimeTemp = dueDateTimeTemp.AddDays(1);
                        dueDateTimeTemp = new DateTime(dueDateTimeTemp.Year, dueDateTimeTemp.Month, dueDateTimeTemp.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);

                        dueDateTime = dueDateTimeTemp;
                    }
                    else
                    {
                        DateTime startWorkDayTemp = new DateTime(dueDateTime.Year, dueDateTime.Month, dueDateTime.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);
                        DateTime endWorkDayTemp = new DateTime(dueDateTime.Year, dueDateTime.Month, dueDateTime.Day, workdayEnd.Hours, workdayEnd.Minutes, workdayEnd.Seconds);

                        if (dueDateTime > endWorkDayTemp) // เวลาเกินเลิกงาน
                        {
                            DateTime dueDateTimeTemp = dueDateTime;

                            //jump to start of next day
                            dueDateTimeTemp = dueDateTimeTemp.AddDays(1);
                            dueDateTimeTemp = new DateTime(dueDateTimeTemp.Year, dueDateTimeTemp.Month, dueDateTimeTemp.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);

                            dueDateTime = dueDateTimeTemp;
                        }
                        else
                        {
                            if (dueDateTime <= startWorkDayTemp)
                            {
                                dueDateTime = startWorkDayTemp;
                            }

                            TimeSpan tsDiff = endWorkDayTemp - dueDateTime;

                            if (tsResolutionTimeTemp > tsDiff)
                            {
                                tsResolutionTimeTemp = tsResolutionTimeTemp.Subtract(tsDiff); // ลบเวลาที่ทำไปแล้ว

                                DateTime dueDateTimeTemp = dueDateTime;
                                //jump to start of next day
                                dueDateTimeTemp = dueDateTimeTemp.AddDays(1);
                                dueDateTimeTemp = new DateTime(dueDateTimeTemp.Year, dueDateTimeTemp.Month, dueDateTimeTemp.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);

                                dueDateTime = dueDateTimeTemp;
                            }
                            else
                            {
                                dueDateTime = startWorkDayTemp.AddSeconds(tsResolutionTimeTemp.TotalSeconds);
                                tsResolutionTimeTemp = TimeSpan.Zero;
                            }
                        }

                    }
                } while (tsResolutionTimeTemp.Ticks > 0);
            }
            else
            {
                dueDateTime = dueDateTime.AddSeconds(tsResolutionTimeTemp.TotalSeconds);
            }

            TimeSpan tsNewResolutionTime = dueDateTime - dtCurrent;

            return tsNewResolutionTime.TotalSeconds;

        }
        #endregion
        // Edit 16-12-2019
        public string StartTicket(string sid, string companyCode, string ticketType, string ticketNo, string ticketYear,
            string tierCode, string tierStart, string tierStartDescription, double resolutionTime, double requesterTime, string OwnerSevice,/*string incidentArea,*/
            string equipmentNo, string remark, string createdUserName, string createdEmployeeCode, string createdFullName,
            Boolean AutoTriggerEscalate, string SLAGroupCode,
            DataTable dtFile = null, string UploadFileUrl = "", string UploadFilePath = "")
        {


            //resolutionTime = validationWorkTime(resolutionTime);// test validationWorkTime 
            resolutionTime = CalculateNewResolutionTime(resolutionTime);
            ServiceTicketLibrary lib = new ServiceTicketLibrary();
            remark = remark.Replace("'", "''");

            string itemObjectID = sid.Trim() + ticketType.Trim() + ticketYear.Trim() + ticketNo.Trim();

            string startDateTime = Validation.getCurrentServerStringDateTime();

            //Update equipment and incident area
            lib.AutoUpdateTicketWhenAssignWork(
                sid,
                companyCode,
                itemObjectID,
                equipmentNo,
                ""//incidentArea
            );

            #region Create Activity     

            string MainDelegate = "";

            DataTable dtmain = GetTierMainDelegate(sid, companyCode, tierCode, tierStart, ticketType, "", OwnerSevice);

            if (dtmain.Rows.Count > 0)
            {
                foreach (DataRow drMain in dtmain.Rows)
                {
                    MainDelegate = drMain["EmployeeCode"].ToString();
                }
            }

            string participants = "";
            string[] participantsArray = new string[0];

            DataTable dtParticipants = GetTierParticipants(sid, companyCode, tierCode, tierStart, ticketType, "", OwnerSevice);

            if (dtParticipants.Rows.Count > 0)
            {
                foreach (DataRow drParticipant in dtParticipants.Rows)
                {
                    participants += participants == "" ? "" : ",";
                    participants += drParticipant["EmployeeCode"].ToString();
                }
            }

            if (participants != "")
            {
                participantsArray = participants.Split(',');
            }

            string displayTicketNo = GetTicketNoForDisplay(sid, companyCode, ticketType, ticketNo);

            string subject = "Ticket : " + displayTicketNo + " Assign To " + tierStartDescription;

            string ticketCode = CreatedTierServiceTicket(
                sid, companyCode,
                subject, remark, MainDelegate,
                participantsArray, createdUserName,
                startDateTime, resolutionTime, false,
                "", SLAGroupCode
            );

            SaveServicecallMappingActivity(companyCode, ticketCode, ticketYear, ticketNo
                , equipmentNo, tierCode, tierStart, "001", createdUserName, startDateTime);

            #endregion

            #region Trigger
            if (AutoTriggerEscalate)
            {
                // Convert minutes to seconds
                //double seconds = TimeSpan.FromSeconds(resolutionTime).TotalSeconds;
                double seconds = resolutionTime;

                SetTriggerBeforeOverdue(ticketCode, ticketType, ticketNo, ticketYear, resolutionTime, requesterTime, createdUserName);

                TriggerService.GetInstance().EscalateTicket(ticketCode, ticketType, ticketNo, ticketYear, seconds.ToString(), createdUserName);
            }
            #endregion

            #region Send Mail
            //NotificationLibrary.GetInstance().OpenAndEscalateTicket(sid, companyCode, ticketCode, createdEmployeeCode);

            #endregion


            //Update Assign Date Time
            string sql = @"UPDATE cs_servicecall_item 
                           SET AssignDate = '" + startDateTime.Substring(0, 8) + "', AssignTime = '" + startDateTime.Substring(8, 6) + @"' 
                           WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + "' AND ObjectID = '" + itemObjectID + "' AND xLineNo = '001'";

            dbService.executeSQLForFocusone(sql);

            string ticketTypeDesc = lib.GetDocumentTypeDesc(sid, companyCode, ticketType);

            string Description = ServiceTicketLibrary.LookUpTable("b.Remark",
                "cs_servicecall_header a inner join cs_servicecall_item b on a.SID = b.SID and a.CompanyCode = b.CompanyCode and a.ObjectID = b.ObjectID",
                "WHERE a.SID='" + sid + "' AND a.CompanyCode='" + companyCode + "' AND a.CallerID='" + ticketNo + "'");
            string DescriptionDisplay = "";
            if (Description != "") { DescriptionDisplay = " : " + Description; }

            string logMessage = "Create a new " + ticketTypeDesc + ". Ticket No. " + displayTicketNo + DescriptionDisplay;
            //Add comment
            string AttachFileKey = "";
            if (dtFile != null && dtFile.Rows.Count > 0)
            {
                AttachFileKey = SaveFileForCreatedTicket(ticketCode, logMessage, UploadFilePath, UploadFileUrl, dtFile);
            }
            lib.SaveActivityDetail(sid, companyCode, companyCode, createdEmployeeCode, createdEmployeeCode, createdFullName, ticketCode, "", logMessage, "",
                startDateTime, "Initial", "", "", AttachFileKey);

            return ticketCode;
        }

        public void SetTriggerBeforeOverdue(string ticketCode, string ticketType, string ticketNo, string ticketYear, double resolutionTime, double requesterTime, string createdUserName)
        {

            if (requesterTime != 0)
            {
                if (resolutionTime - requesterTime > 0)
                {
                    TriggerService.GetInstance().StartTicket(ticketCode + "befOverdue", ticketType, ticketNo, ticketYear, (resolutionTime - requesterTime).ToString(), createdUserName);
                }

            }
        }

        public void SetTriggerUpdateStatus(string ticketCode, string ticketType, string ticketNo, string ticketYear, double delayTime, string createdUserName, string statusbegin, string statustarget)
        {

            if (delayTime != 0)
            {
                TriggerService.GetInstance().UpdateTicketStatus(ticketCode + "updatestatus|" + statusbegin + "|" + statustarget, ticketType, ticketNo, ticketYear, delayTime.ToString(), createdUserName);
            }

        }

        public string StartTicketChange(string sid, string companyCode, string ticketType, string ticketNo,
            string ticketYear, string StartDescription, string MainDelegate, string[] participantsArray,
            string remark, string createdUserName, string createdEmployeeCode, string createdFullName,
            double resolutionTime, string startDateTime,
            DataTable dtFile = null, string UploadFileUrl = "", string UploadFilePath = ""
            )
        {
            ServiceTicketLibrary lib = new ServiceTicketLibrary();
            remark = remark.Replace("'", "''");

            string itemObjectID = sid.Trim() + ticketType.Trim() + ticketYear.Trim() + ticketNo.Trim();

            //string startDateTime = Validation.getCurrentServerStringDateTime();

            #region Create Activity
            string displayTicketNo = GetTicketNoForDisplay(sid, companyCode, ticketType, ticketNo);

            string subject = "Ticket : " + displayTicketNo + " Assign To " + StartDescription;

            string ticketServiceCode = CreatedTierServiceTicket(sid, companyCode,
                subject, remark, MainDelegate, participantsArray, createdUserName, startDateTime, resolutionTime, false);

            SaveServicecallMappingActivity(companyCode, ticketServiceCode, ticketYear, ticketNo
                , "", "", "", "", createdUserName, startDateTime);

            #endregion

            #region Send Mail
            //NotificationLibrary.GetInstance().OpenAndEscalateTicket(sid, companyCode, ticketServiceCode, createdEmployeeCode);
            #endregion

            //Update Assign Date Time
            string sql = @"UPDATE cs_servicecall_item 
                           SET AssignDate = '" + startDateTime.Substring(0, 8) + "', AssignTime = '" + startDateTime.Substring(8, 6) + @"' 
                           WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + "' AND ObjectID = '" + itemObjectID + "'";

            dbService.executeSQLForFocusone(sql);

            string ticketTypeDesc = lib.GetDocumentTypeDesc(sid, companyCode, ticketType);
            //Add comment
            string logMessage = "Create a new " + ticketTypeDesc + ". Ticket No. " + displayTicketNo;
            string AttachFileKey = "";
            if (dtFile != null && dtFile.Rows.Count > 0)
            {
                AttachFileKey = SaveFileForCreatedTicket(ticketServiceCode, logMessage, UploadFilePath, UploadFileUrl, dtFile);
            }
            lib.SaveActivityDetail(
                sid, companyCode, companyCode, createdEmployeeCode,
                createdEmployeeCode, createdFullName,
                ticketServiceCode, "", logMessage, "",
                startDateTime, "Initial", "", "", AttachFileKey
            );

            return ticketServiceCode;
        }


        public string EscalateTicket(string sid, string companyCode, string ticketType, string ticketNo, string ticketYear,
            string tierCode, string tierEscalate, string tierDescription, double resolutionTime,
            double requesterTime,
            string OwnerService, /*string incidentArea,*/ string equipmentNo, string remark, string escalateUserName, string escalateEmployeeCode,
            string escalateFullName, bool IsEscalateManual)
        {
            return EscalateTicket(sid, companyCode, ticketType, ticketNo, ticketYear,
                tierCode, tierEscalate, tierDescription, resolutionTime,
                requesterTime,
                OwnerService, /*incidentArea,*/ equipmentNo, remark, escalateUserName, escalateEmployeeCode,
                escalateFullName, IsEscalateManual, null, null, null, null
            );
        }

        public string EscalateTicket(string sid, string companyCode, string ticketType, string ticketNo, string ticketYear,
            string tierCode, string tierEscalate, string tierDescription, double resolutionTime, double requesterTime,
            string OwnerService, /*string incidentArea,*/ string equipmentNo, string remark, string escalateUserName, string escalateEmployeeCode,
            string escalateFullName, bool IsEscalateManual, string MainDelegate, string[] participantsArray, string OwnerServiceCode, string SLAGroupCode)
        {
            ServiceTicketLibrary lib = new ServiceTicketLibrary();

            remark = remark.Replace("'", "''");

            string oldTicket = "";
            string oldTierDesc = "";
            string endDateTime = "";
            //string MainDelegate = "";

            //get ข้อมูลว่าเอกสาร service นี้ใช้ activity ตัวไหนอยู่
            DataTable dt = getAobjectlinkByServiceDoc(companyCode, ticketNo, ticketYear);

            if (dt.Rows.Count > 0)
            {
                oldTicket = dt.Rows[0]["AOBJECTLINK"].ToString();

                oldTierDesc = ServiceTicketLibrary.LookUpTable("TierDescription", "Link_Tier_Master_Item", "WHERE SID='" + sid + "' AND TierCode='" + dt.Rows[0]["TierCode"] + "' AND Tier='" + dt.Rows[0]["Tier"] + "'");

                endDateTime = ServiceTicketLibrary.LookUpTable("EndDateTime", "ticket_service_header", "WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "' AND TicketCode='" + oldTicket + "'");

                //ปิดงานเก่า
                CloseTierServiceTicket(sid, companyCode, oldTicket);
            }

            if (IsEscalateManual)
            {
                endDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
            }

            #region Create Activity     
            if (string.IsNullOrEmpty(MainDelegate))
            {
                DataTable dtmain = GetTierMainDelegate(
                    sid,
                    companyCode,
                    tierCode,
                    tierEscalate,
                    ticketType,
                    "",
                    OwnerService
                );

                if (dtmain.Rows.Count > 0)
                {
                    foreach (DataRow drMain in dtmain.Rows)
                    {
                        MainDelegate = drMain["EmployeeCode"].ToString();
                    }
                }
            }

            if (participantsArray == null)
            {
                participantsArray = new string[0];
                string participants = "";

                DataTable dtParticipants = GetTierParticipants(sid, companyCode, tierCode, tierEscalate, ticketType, "", OwnerService);

                if (dtParticipants.Rows.Count > 0)
                {
                    foreach (DataRow drParticipant in dtParticipants.Rows)
                    {
                        participants += participants == "" ? "" : ",";
                        participants += drParticipant["EmployeeCode"].ToString();
                    }
                }

                if (participants != "")
                {
                    participantsArray = participants.Split(',');
                }
            }

            string displayTicketNo = GetTicketNoForDisplay(sid, companyCode, ticketType, ticketNo);

            string subject = "Ticket : " + displayTicketNo + " Assign To " + tierDescription;

            string ticketCode = CreatedTierServiceTicket(
                sid, companyCode,
                subject, remark, MainDelegate, participantsArray,
                escalateUserName, endDateTime, resolutionTime,
                true, OwnerServiceCode, SLAGroupCode
            );

            if (oldTicket != "" && ticketCode != "")
            {
                //Copy Remark
                TierService.getInStance().CopyRemark(oldTicket, ticketCode);
            }

            SaveServicecallMappingActivity(companyCode, ticketCode, ticketYear, ticketNo, equipmentNo,
                tierCode, tierEscalate, "001", escalateUserName, Validation.getCurrentServerStringDateTime());

            #endregion

            #region Trigger
            // Convert minutes to seconds
            //double seconds = TimeSpan.FromSeconds(resolutionTime).TotalSeconds;
            double seconds = resolutionTime;
            TriggerService.GetInstance().EscalateTicket(ticketCode, ticketType, ticketNo, ticketYear, seconds.ToString(), escalateUserName);

            SetTriggerBeforeOverdue(ticketCode, ticketType, ticketNo, ticketYear, resolutionTime, requesterTime, escalateUserName);
            #endregion

            #region Send Mail
            //NotificationLibrary.GetInstance().OpenAndEscalateTicket(sid, companyCode, ticketCode, escalateEmployeeCode);
            #endregion

            string logMessage = "Escalate group from\"" + escalateUserName + "\" \"" + oldTierDesc.Trim() + "\" to \"" + MainDelegate + "\" \"" + tierDescription.Trim() + "\"";

            #region Add Log
            //List<logValue_OldNew> enLog = new List<logValue_OldNew>();
            //enLog.Add(new logValue_OldNew
            //{
            //    Value_Old = "",
            //    Value_New = logMessage,
            //    TableName = "",
            //    FieldName = "",
            //    AccessCode = LogServiceLibrary.AccessCode_Change
            //});

            //SaveLogTicket(sid, ticketType, ticketYear, ticketNo, companyCode, escalateUserName, enLog);
            #endregion


            //Add comment
            lib.SaveActivityDetail(sid, companyCode, companyCode, escalateEmployeeCode,
                escalateEmployeeCode, escalateFullName, ticketCode, "", logMessage, "",
                endDateTime, "Escalate", "", "", "");

            NotificationLibrary.GetInstance().TicketAlertEvent(
                NotificationLibrary.EVENT_TYPE.TICKET_ESCALATE,
                sid,
                companyCode,
                ticketNo,
                escalateEmployeeCode,
                ThisPage
            );

            return ticketCode;
        }

        public void updateCurDateTimeActivityActionManual(string SID, string CompanyCode, string TicketCode)
        {
            string sql = @"
                update ticket_service_header 
                set EndDateTime = '" + Validation.getCurrentServerStringDateTime() + @"' 
                where SID = '" + SID + @"' 
                AND CompanyCode = '" + CompanyCode + @"' 
                AND TicketCode = '" + TicketCode + @"'
                ";
            dbService.executeSQLForFocusone(sql);
        }

        public string ResolveTicket(string sid, string companyCode, string ticketType, string ticketNo, string ticketYear,
            string tierResolveDescription, string resolveEmployeeCode, string resolveFullName)
        {
            ServiceTicketLibrary lib = new ServiceTicketLibrary();

            string itemObjectID = sid.Trim() + ticketType.Trim() + ticketYear.Trim() + ticketNo.Trim();

            //get ข้อมูลว่าเอกสาร service นี้ใช้ activity ตัวไหนอยู่
            DataTable dt = getAobjectlinkByServiceDoc(companyCode, ticketNo, ticketYear);

            string ticketCode = "";

            if (dt.Rows.Count > 0)
            {
                string resolveDateTime = Validation.getCurrentServerStringDateTime();

                ticketCode = dt.Rows[0]["AOBJECTLINK"].ToString();

                //ปิดงาน
                CloseTierServiceTicket(sid, companyCode, ticketCode);

                #region Send Mail
                //NotificationLibrary.GetInstance().OpenAndEscalateTicket(sid, companyCode, ticketCode, resolveEmployeeCode);
                #endregion

                #region Update Assign Date Time
                string sql = @"UPDATE cs_servicecall_item 
                              SET 
                                ResolutionBy = '" + resolveEmployeeCode + @"',
                                ResolutionOnDate = '" + resolveDateTime.Substring(0, 8) + @"',
                                ResolutionOnTime = '" + resolveDateTime.Substring(8, 6) + @"' 
                              WHERE SID = '" + sid + @"' 
                                AND CompanyCode = '" + companyCode + @"' 
                                AND ObjectID = '" + itemObjectID + @"' 
                                AND xLineNo = '001'";

                dbService.executeSQLForFocusone(sql);
                #endregion

                //Get status
                string where = @" WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + @"' 
                              AND Doctype = '" + ticketType + "' AND Fiscalyear = '" + ticketYear + "' AND CallerID = '" + ticketNo + "'";

                string currentStatus = ServiceTicketLibrary.LookUpTable("DocStatus", "cs_servicecall_header", where);
                string currentStatusDesc = ServiceTicketLibrary.GetTicketDocStatusDesc(sid, companyCode, currentStatus);
                string statusResolve = lib.GetTicketStatusFromEvent(sid, companyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE);
                string statusResolveDesc = ServiceTicketLibrary.GetTicketDocStatusDesc(sid, companyCode, statusResolve);

                #region Update Status                                                                                          
                sql = @"UPDATE cs_servicecall_header
                           SET Docstatus = '" + statusResolve + "'" + where;

                dbService.executeSQLForFocusone(sql);
                #endregion                


                string displayTicketNo = GetTicketNoForDisplay(sid, companyCode, ticketType, ticketNo);

                string logMessage = "Ticket No. " + displayTicketNo + " has been resolved by \"" + tierResolveDescription.Trim() + "\"";
                string quoteMessage = "Update status from \"" + currentStatusDesc.Trim() + "\" to \"" + statusResolveDesc.Trim() + "\"";

                //Add comment
                lib.SaveActivityDetail(sid, companyCode, companyCode, resolveEmployeeCode, resolveEmployeeCode, resolveFullName, ticketCode, "", logMessage, "",
                    resolveDateTime, "REMARK", "UPDATESTATUS", quoteMessage, "");
            }

            return ticketCode;
        }

        public void saveLogCanceledTicket(string sid, string companyCode, string ticketType, string ticketNo, string ticketYear,
            string EmployeeCode, string EmployeeFullName)
        {
            ServiceTicketLibrary lib = new ServiceTicketLibrary();

            //get ข้อมูลว่าเอกสาร service นี้ใช้ activity ตัวไหนอยู่
            DataTable dt = getAobjectlinkByServiceDoc(companyCode, ticketNo, ticketYear);

            string ticketCode = "";

            if (dt.Rows.Count > 0)
            {
                string cancelDateTime = Validation.getCurrentServerStringDateTime();

                ticketCode = dt.Rows[0]["AOBJECTLINK"].ToString();
                //ปิดงาน
                CloseTierServiceTicket(sid, companyCode, ticketCode);

                string where = @" WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + @"' 
                              AND Doctype = '" + ticketType + "' AND Fiscalyear = '" + ticketYear + "' AND CallerID = '" + ticketNo + "'";

                string currentStatus = ServiceTicketLibrary.LookUpTable("DocStatus", "cs_servicecall_header", where);
                string currentStatusDesc = ServiceTicketLibrary.GetTicketDocStatusDesc(sid, companyCode, currentStatus);
                string statusCancel = lib.GetTicketStatusFromEvent(sid, companyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL);
                string statusCancelDesc = ServiceTicketLibrary.GetTicketDocStatusDesc(sid, companyCode, statusCancel);

                string displayTicketNo = GetTicketNoForDisplay(sid, companyCode, ticketType, ticketNo);

                string logMessage = "Ticket No. " + displayTicketNo + " has been canceled.";
                string quoteMessage = "Update status from \"" + currentStatusDesc.Trim() + "\" to \"" + statusCancelDesc.Trim() + "\"";

                //Add comment
                lib.SaveActivityDetail(sid, companyCode, companyCode, EmployeeCode, EmployeeCode, EmployeeFullName, ticketCode, "", logMessage, "",
                    cancelDateTime, "REMARK", "UPDATESTATUS", quoteMessage, "");
            }
        }

        public void UpdateStatus(string sid, string companyCode, string statusCode, string ticketType, string ticketYear, string ticketNo, string updatedBy, string updatedOn)
        {
            string where = @" WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + @"' 
                              AND Doctype = '" + ticketType + "' AND Fiscalyear = '" + ticketYear + "' AND CallerID = '" + ticketNo + "'";

            string sql = @"UPDATE cs_servicecall_header
                           SET Docstatus = '" + statusCode + "', UPDATED_BY = '" + updatedBy + "', UPDATED_ON = '" + updatedOn + "'" + where;

            dbService.executeSQLForFocusone(sql);

            //Get event start / stop from status
            string stopTimerString = ServiceTicketLibrary.LookUpTable("StopTimer", "ERPW_TICKET_STATUS",
                "WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + "' AND TicketStatusCode = '" + statusCode + "'");

            bool stopTimer = false;
            bool.TryParse(stopTimerString, out stopTimer);

            string sqlCheckStop = "SELECT * FROM cs_servicecall_stop_timer WITH (NOLOCK)  " + where;

            DataTable dt = dbService.selectDataFocusone(sqlCheckStop);

            DataRow[] drr = dt.Select("StopDate <> '' AND RestartDate = ''");

            if (stopTimer)
            {
                if (drr.Length == 0) //not have stop record
                {
                    string itemNo = "001";

                    if (dt.Rows.Count > 0)
                    {
                        itemNo = Convert.ToString(Convert.ToInt32(dt.Compute("MAX(xLineNo)", "")) + 1).PadLeft(3, '0');
                    }

                    InsertStopTimer(sid, companyCode, ticketType, ticketYear, ticketNo, itemNo, updatedBy, updatedOn);

                    if (!IsOverTime(sid, companyCode, ticketNo))
                    {
                        PauseTriggerEscalate(sid, companyCode, statusCode, ticketNo);
                    }
                }
            }
            else
            {
                if (drr.Length > 0) //has stop record
                {
                    string sqlUpdate = @"UPDATE [dbo].[cs_servicecall_stop_timer] 
                                         SET [RestartDate] = '" + updatedOn.Substring(0, 8) + @"', 
                                             [RestartTime] = '" + updatedOn.Substring(8, 6) + @"'"
                                             + where + " AND [xLineNo] = '" + drr[0]["xLineNo"] + "'";

                    dbService.executeSQLForFocusone(sqlUpdate);

                    CalculateNewEndDateTime(sid, companyCode, statusCode, ticketNo, drr[0]["xLineNo"].ToString(), updatedOn);

                }
            }
        }

        private void InsertStopTimer(string sid, string companyCode, string ticketType, string ticketYear, string ticketNo, string itemNo, string createdBy, string createdOn)
        {
            string stopDate = createdOn.Substring(0, 8);
            string stopTime = createdOn.Substring(8, 6);

            string sql = @"INSERT INTO [dbo].[cs_servicecall_stop_timer]
                                      ([SID]
                                      ,[CompanyCode]
                                      ,[Doctype]
                                      ,[CallerID]
                                      ,[FiscalYear]
                                      ,[xLineNo]
                                      ,[StopDate]
                                      ,[StopTime]
                                      ,[RestartDate]
                                      ,[RestartTime]
                                      ,[CREATED_BY]
                                      ,[CREATED_ON])
                                VALUES
                                      ('" + sid + @"'
                                      ,'" + companyCode + @"'
                                      ,'" + ticketType + @"'
                                      ,'" + ticketNo + @"'
                                      ,'" + ticketYear + @"'
                                      ,'" + itemNo + @"'
                                      ,'" + stopDate + @"'
                                      ,'" + stopTime + @"'
                                      ,''
                                      ,''
                                      ,'" + createdBy + @"'
                                      ,'" + createdOn + "');";

            dbService.executeSQLForFocusone(sql);
        }

        public List<Main_LogService> SaveLogTicket(string sid, string ticketType, string fiscalYear, string ticketNo, string companyCode, string userName, List<logValue_OldNew> enLog)
        {
            string curDateTime = Validation.getCurrentServerStringDateTime();
            string curDate = curDateTime.Substring(0, 8);
            string curTime = curDateTime.Substring(8);

            List<Main_LogService> en = new List<Main_LogService>();

            enLog.Select(s => s.TableName).Distinct().ToList().ForEach(r =>
            {

                List<Detail_LogService> listDetail = new List<Detail_LogService>();
                enLog.Where(w => w.TableName.Equals(r)).ToList().ForEach(r_item =>
                {
                    if (r_item.Value_Old != r_item.Value_New)
                    {
                        listDetail.Add(new Detail_LogService
                        {
                            ItemNumber = "",
                            FieldName = r_item.FieldName,
                            OldValue = r_item.Value_Old.Replace("'", "''"),
                            NewValue = r_item.Value_New.Replace("'", "''")
                        });
                    }
                });

                if (listDetail.Count > 0)
                {
                    en.Add(new Main_LogService
                    {
                        LOGOBJCODE = LogServiceLibrary.PROGRAM_ID_SERVICE_CALL,
                        PROGOBJECT = LogServiceLibrary.PROGRAM_ID_SERVICE_CALL + "|" + r,
                        ACCESSCODE = LogServiceLibrary.AccessCode_Change,
                        OBJPKREC = sid + ticketType + fiscalYear + ticketNo + companyCode,
                        APPLTYPE = "T",
                        Access_By = userName,
                        Access_Date = curDate,
                        Access_Time = curTime,
                        listDetail = listDetail
                    });
                }
            });

            if (en.Count > 0)
            {
                LogServiceLibrary libLog = new LogServiceLibrary();
                libLog.SaveLog(sid, LogServiceLibrary.PROGRAM_ID_SERVICE_CALL, "T", en);
            }

            return en;
        }

        public string GetTicketNoForDisplay(string sid, string companyCode, string ticketType, string ticketNo)
        {
            DataTable dtPrefix = ServiceTicketLibrary.GetInstance().getDataPrefixDocType(sid, companyCode, new List<string> { ticketType });
            DataRow[] drr = dtPrefix.Select("'" + ticketNo + "' like PrefixCode + '%'");
            if (drr.Length > 0)
            {
                string prefix = drr[0]["PrefixCode"].ToString();
                string ticketNoDisplay = ticketNo;
                ticketNo = ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay);// prefix + Convert.ToInt32(ticketNoDisplay);
            }
            return ticketNo;
        }

        public DataTable GetTicketMaterial(string sid, string companyCode, string fiscalYear, string ticketType, string ticketNo)
        {
            string sql = @"SELECT *
                           FROM ERPW_TICKET_MATERIAL WITH (NOLOCK) 
                           WHERE SID = '" + sid + @"'
	                           AND CompanyCode = '" + companyCode + @"'
	                           AND FiscalYear = '" + fiscalYear + @"'
	                           AND TicketType = '" + ticketType + @"'
	                           AND TicketNo = '" + ticketNo + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            dt.TableName = "ERPW_TICKET_MATERIAL";
            dt.PrimaryKey = new DataColumn[] {
                dt.Columns["SID"],
                dt.Columns["CompanyCode"],
                dt.Columns["FiscalYear"],
                dt.Columns["TicketType"],
                dt.Columns["TicketNo"],
                dt.Columns["ItemNo"]
            };

            dt.Columns["SID"].DefaultValue = sid;
            dt.Columns["CompanyCode"].DefaultValue = companyCode;
            dt.Columns["FiscalYear"].DefaultValue = fiscalYear;
            dt.Columns["TicketType"].DefaultValue = ticketType;
            dt.Columns["TicketNo"].DefaultValue = ticketNo;
            dt.Columns["UnitPrice"].DefaultValue = 0;
            dt.Columns["Qty"].DefaultValue = 0;

            return dt;
        }

        // 05/01/2561 ฟังก์ชันดึงข้อมูล CI ใกล้หมดอายุภายใน 30 วัน || get data ci at time nearly up in 30 day (By Born kk)
        public List<Time_NearlyUp> GetCITimesNearlyUp(string sid, string companyCode) {

            DateTime startDate = DateTime.Now;
            DateTime expiryDate = startDate.AddDays(30);
            string dateNow = DateTime.Now.ToString("yyyyMMdd");
            string dateTo = expiryDate.ToString("yyyyMMdd");


            string sql = @"(select meoa.*,me.Description,mc.CustomerName as Name
                            from master_equipment_owner_assignment meoa WITH (NOLOCK) 
                            INNER JOIN master_equipment me WITH (NOLOCK)  ON meoa.EquipmentCode = me.EquipmentCode
                            INNER JOIN master_customer mc WITH (NOLOCK)  ON meoa.OwnerCode = mc.CustomerCode
                            where meoa.SID = '" + sid + @"'
                              AND meoa.CompanyCode = '" + companyCode + @"'
                              AND meoa.ActiveStatus = 'True'
                              AND meoa.OwnerType = '01'
                              AND meoa.EndDate >= '" + dateNow + @"' 
                              AND meoa.EndDate <= '" + dateTo + @"')
                             UNION
                                (select meoa.*,me.Description,mv.VendorName as Name
                            from master_equipment_owner_assignment meoa WITH (NOLOCK) 
                            INNER JOIN master_equipment me WITH (NOLOCK)  ON meoa.EquipmentCode = me.EquipmentCode
                            INNER JOIN master_vendor mv WITH (NOLOCK)  ON meoa.OwnerCode = mv.VendorCode
                            where meoa.SID = '" + sid + @"'
                              AND meoa.CompanyCode = '" + companyCode + @"'
                              AND meoa.ActiveStatus = 'True'
                              AND meoa.OwnerType = '02'
                              AND meoa.EndDate >= '" + dateNow + @"' 
                              AND meoa.EndDate <= '" + dateTo + @"') ORDER BY EndDate ASC";

            DataTable dtResult = dbService.selectDataFocusone(sql);
            List<Time_NearlyUp> listdata = new List<Time_NearlyUp>();
            string strJson = JsonConvert.SerializeObject(dtResult);
            listdata = JsonConvert.DeserializeObject<List<Time_NearlyUp>>(strJson);
            return listdata;
        }
        // 05/01/2561 model ข้อมูล CI ใกล้หมดอายุภายใน 30 วัน || model data ci at time nearly up in 30 day (By Born kk)
        #region Entity TimeNearlyUp
        public class Time_NearlyUp
        {
            public string CompanyCode { get; set; }
            public string EquipmentCode { get; set; }
            public string Categorycode { get; set; }
            public string SID { get; set; }
            public string LineNumber { get; set; }
            public string ActiveStatus { get; set; }
            public string BeginDate { get; set; }
            string _enddate;
            public string EndDate { get {
                    return Validation.Convert2DateDisplay(this._enddate);

                } set {
                    this._enddate = value;
                } }
            public string OwnerCode { get; set; }
            public string OwnerType { get; set; }
            public string CREATED_BY { get; set; }
            public string UPDATED_BY { get; set; }
            public string CREATED_ON { get; set; }
            public string UPDATED_ON { get; set; }
            public string SLAGroupCode { get; set; }
            public string TicketType { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

        }
        #endregion


        //04/01/2562 function ดึง ERPW property header (By born kk)
        public List<ERPWPropertyValueHeader> GetERPWPropertyHeader(string sid, string companyCode) {
            string sql = "select * from ERPW_Property_Value_Header pvh WITH (NOLOCK)  where  pvh.SID = '" + sid + @"' and pvh.CompanyCode = '" + companyCode + @"'";
            List<ERPWPropertyValueHeader> listpvh = new List<ERPWPropertyValueHeader>();
            DataTable result = dbService.selectDataFocusone(sql);
            string strJson = JsonConvert.SerializeObject(result);
            listpvh = JsonConvert.DeserializeObject<List<ERPWPropertyValueHeader>>(strJson);
            return listpvh;
        }

        #region ERPWPropertyValueHeader
        public class ERPWPropertyValueHeader {
            public string SID { get; set; }
            public string PostingType { get; set; }
            public string HeaderCode { get; set; }
            public string HeaderDescription { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedOn { get; set; }
            public string UpdateBy { get; set; }
            public string UpdateOn { get; set; }
        }
        #endregion

        public void addERPWPropertyValue(string sid, string companyCode, string fiscalYear, string documentType, string documentNo, DataTable dt, string createby, string postingtype, string headercode) {
            List<string> listInsert = new List<string>();
            string createdOn = Validation.getCurrentServerStringDateTime();

            string sqldelete = "delete from ERPW_Property_Value_Item where SID = '" + sid + @"' and CompanyCode = '" + companyCode + @"' and  RefFiscalYear = '" + fiscalYear + @"' and RefDocumentType = '" + documentType + @"' and  RefDocumentNo = '" + documentNo + @"' and  PostingType = '" + postingtype + @"' and  HeaderCode = '" + headercode + @"'";
            dbService.executeSQLForFocusone(sqldelete);

            if (dt.Rows.Count > 0) {
                foreach (DataRow row in dt.Rows)
                {
                    string sql = "insert into ERPW_Property_Value_Item(SID,CompanyCode,PostingType,RefFiscalYear,RefDocumentType,RefDocumentNo,HeaderCode,ItemCode,ItemDescription,xValue,Created_By,Created_On) values('" + sid + @"','" + companyCode + @"','" + row["PostingType"] + @"','" + fiscalYear + @"','" + documentType + @"','" + documentNo + @"','" + row["HeaderCode"] + @"','" + row["ItemCode"] + @"','" + row["ItemDesc"] + @"','" + row["Value"] + @"','" + createby + @"','" + createdOn + @"')";
                    listInsert.Add(sql);
                }
                dbService.executeSQLForFocusone(listInsert);
            }


        }

        public DataTable GetErpwPropertyValueItem(string sid, string companyCode, string fiscalYear, string documentType, string documentNo, string postingType, string headerCode) {
            string sql = "select ItemCode as ItemNo,PostingType,HeaderCode,ItemCode,ItemDescription as ItemDesc ,xValue as Value from ERPW_Property_Value_Item WITH (NOLOCK)  where SID = '" + sid + @"' and  CompanyCode = '" + companyCode + @"' and RefFiscalYear = '" + fiscalYear + @"' and RefDocumentType = '" + documentType + @"'  and RefDocumentNo = '" + documentNo + @"' and  PostingType = '" + postingType + @"' and HeaderCode = '" + headerCode + @"'";

            DataTable result = dbService.selectDataFocusone(sql);

            return result;
        }

        public string getAobjectLinkByTicketNumber(string TicketNumber)
        {
            string aobjectlink = "";
            aobjectlink = ServiceLibrary.LookUpTable(
                "AOBJECTLINK",
                "CRM_SERVICECALL_MAPPING_ACTIVITY",
                "where SNAID = 'INET' AND ServiceDocNo = '" + TicketNumber + @"' order by Tier desc"
            );

            return aobjectlink;
        }
        public string getTierByTicketNumber(string AobjectLink)
        {
            string Tier = "";
            Tier = ServiceLibrary.LookUpTable(
                "Tier",
                "CRM_SERVICECALL_MAPPING_ACTIVITY",
                "where SNAID = 'INET' AND AOBJECTLINK = '" + AobjectLink + @"' order by Tier desc"
            );

            return Tier;
        }

        #region Data KeysValue

        public List<KeyValue> GetTierGroupKeyValue(string sid)
        {
            string sql = @"SELECT  TierGroupCode as [Key], TierGroupDescription as [Value] 
                            FROM Link_TierGroup_Master WITH (NOLOCK) WHERE SID = '" + sid + "'";
            sql += " ORDER BY TierGroupDescription asc";
            DataTable dt = dbService.selectDataFocusone(sql);
            List<KeyValue> datas = JsonConvert.DeserializeObject<List<KeyValue>>(
                JsonConvert.SerializeObject(dt)
            );
            return datas;
        }

        public List<KeyValue> GetCustomerDataKeyValue(string SID, string CompanyCode)
        {
            string sql = @"SELECT a.CustomerCode as [Key], a.CustomerName as [Value] 
                            FROM master_customer a WITH (NOLOCK) 
                            INNER JOIN master_customer_general b WITH (NOLOCK) 
                                ON a.SID = b.SID 
                                AND a.CompanyCode = b.CompanyCode
                                AND a.CustomerCode = b.CustomerCode
                            WHERE a.SID = '" + SID + "' AND a.CompanyCode = '" + CompanyCode + @"' AND b.Active='True'";
            sql += " ORDER BY a.[CustomerName] asc";

            DataTable dt = dbService.selectDataFocusone(sql);
            List<KeyValue> datas = JsonConvert.DeserializeObject<List<KeyValue>>(
                JsonConvert.SerializeObject(dt)
            );
            return datas;
        }

        public List<KeyValue> getSearchVendorKeyValue(string SID, string CompanyCode)
        {
            string sql = @"select VendorCode as [Key], VendorName as [Value] 
                            from master_vendor WITH (NOLOCK) 
                            where SID = '" + SID + @"'
                              AND CompanyCode = '" + CompanyCode + @"' ";

            DataTable dt = dbService.selectDataFocusone(sql);
            List<KeyValue> datas = JsonConvert.DeserializeObject<List<KeyValue>>(
                JsonConvert.SerializeObject(dt)
            );
            return datas;
        }

        public List<KeyValue> getSearchEmployeeKeyValue(string sid, string companyCode)
        {
            string sql = @"select a.EmployeeCode as [Key], a.FirstName + ' ' + a.LastName as [Value]  
                            from master_employee a
                            inner join dbo.master_employee_branchandposition b
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.EmployeeCode = b.EmployeeCode
                              and b.Status = 'Active'
                            where a.SID = '" + sid + @"' 
                              AND a.CompanyCode = '" + companyCode + @"' ";

            sql += " order by a.FirstName asc ";

            DataTable dt = dbService.selectDataFocusone(sql);
            List<KeyValue> datas = JsonConvert.DeserializeObject<List<KeyValue>>(
                JsonConvert.SerializeObject(dt)
            );
            return datas;
        }

        #region Get SoldToParty ,ShipToParty ,BillToParty Ref Equipment

        public List<KeyValue> getCustomerSoldToPartyRefEquipment(string SID, string companyCode, string EquipmentCode)
        {
            DataTable dt = getCustomerRefSoForEquipment(SID, companyCode, "SoldToParty", EquipmentCode);
            List<KeyValue> datas = JsonConvert.DeserializeObject<List<KeyValue>>(
                JsonConvert.SerializeObject(dt)
            );
            return datas;
        }

        public List<KeyValue> getCustomerShipToPartyRefEquipment(string SID, string companyCode, string EquipmentCode)
        {
            DataTable dt = getCustomerRefSoForEquipment(SID, companyCode, "ShipToParty", EquipmentCode);
            List<KeyValue> datas = JsonConvert.DeserializeObject<List<KeyValue>>(
                JsonConvert.SerializeObject(dt)
            );
            return datas;
        }

        public List<KeyValue> getCustomerBillToPartyRefEquipment(string SID, string companyCode, string EquipmentCode)
        {
            DataTable dt = getCustomerRefSoForEquipment(SID, companyCode, "BillToParty", EquipmentCode);
            List<KeyValue> datas = JsonConvert.DeserializeObject<List<KeyValue>>(
                JsonConvert.SerializeObject(dt)
            );
            return datas;
        }

        private DataTable getCustomerRefSoForEquipment(string SID, string companyCode, string _modeTypeParty, string EquipmentCode)
        {
            string sql = @"
                        select distinct a." + _modeTypeParty + @" as [Key]
                        ,e.CustomerName as [Value] 
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

        public List<KeyValue> getAssetKeyValue(string SID, string CompanyCode, string AssetCode)
        {
            string sql = @"
                SELECT A.AssetCode AS [Key] 
                  ,B.AssetSubCodeDescription AS [Value]
                FROM am_master_asset_header AS A 
                INNER JOIN am_master_asset_subcode AS B
                  ON A.SID = B.SID 
                  AND A.CompanyCode = B.CompanyCode 
                  AND A.AssetCode = B.AssetCode  
                WHERE A.SID = '" + SID + @"' 
                  AND A.CompanyCode = '" + CompanyCode + @"' ";

            if (!string.IsNullOrEmpty(AssetCode))
            {
                sql += " AND A.AssetCode = '" + AssetCode + @"' ";
            }

            sql += " ORDER BY A.AssetCode ,B.AssetSubCodeDescription ";

            DataTable dt = dbService.selectDataFocusone(sql);
            List<KeyValue> datas = JsonConvert.DeserializeObject<List<KeyValue>>(
                JsonConvert.SerializeObject(dt)
            );
            return datas;
        }
        #endregion

        public static bool CheckCurrentTier(string CompanyCode, string TicketNumber, string ActivityCode)
        {
            bool isCurrentTier = false;
            string curActivityCode = ServiceLibrary.LookUpTable(
                "AOBJECTLINK",
                "CRM_SERVICECALL_MAPPING_ACTIVITY",
                "WHERE SNAID = '" + CompanyCode + "' AND ServiceDocNo = '" + TicketNumber + "' AND TierCode <> '' order by created_on desc"
            );

            if (!string.IsNullOrEmpty(curActivityCode))
            {
                if (curActivityCode == ActivityCode)
                {
                    isCurrentTier = true;
                }
                else
                {
                    isCurrentTier = false;
                }
            }
            else
            {
                isCurrentTier = true;
            }

            return isCurrentTier;
        }


        public static bool CheckResolveTier(string SID, string CompanyCode, string ActivityCode)
        {
            bool isResolve = false;
            string curStatusResolve = ServiceLibrary.LookUpTable(
                "Status",
                "ticket_service_header",
                "where SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' AND TicketCode = '" + ActivityCode + "'"
            );

            if (!string.IsNullOrEmpty(curStatusResolve))
            {
                if (curStatusResolve.ToUpper() == "OPEN")
                {
                    isResolve = false;
                }
                else
                {
                    isResolve = true;
                }
            }
            else
            {
                isResolve = false;
            }

            return isResolve;
        }

        public List<AssignToModel> getAssignTo_MainDelegate_ByAobjectLink(string SID, string CompanyCode, string AObjectLink)
        {
            string sql = @"select b.FirstName, b.LastName, 'true' as IsMainDelegate
                            from ticket_service_header a
                            inner join master_employee b
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.MainDelegate = b.EmployeeCode
                            where a.SID = '" + SID + @"' 
                              AND a.CompanyCode = '" + CompanyCode + @"' 
                              AND a.TicketCode = '" + AObjectLink + @"'";

            DataTable dt = dbService.selectDataFocusone(sql);
            List<AssignToModel> datas = JsonConvert.DeserializeObject<List<AssignToModel>>(JsonConvert.SerializeObject(dt));
            return datas;
        }

        public List<AssignToModel> getAssignTo_Participants_ByAobjectLink(string SID, string CompanyCode, string AObjectLink)
        {
            string sql = @"select b.FirstName, b.LastName, 'false' as IsMainDelegate
                            from ticket_service_participants a
                            inner join master_employee b
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.EmployeeCode = b.EmployeeCode
                            where a.SID = '" + SID + @"' 
                              AND a.CompanyCode = '" + CompanyCode + @"' 
                              AND a.TicketCode = '" + AObjectLink + @"'";

            DataTable dt = dbService.selectDataFocusone(sql);
            List<AssignToModel> datas = JsonConvert.DeserializeObject<List<AssignToModel>>(JsonConvert.SerializeObject(dt));
            return datas;
        }

        public string SaveFileForCreatedTicket(string aobjectLink, string Message, string UploadFilePath, string UploadFileUrl,
            DataTable dtFile)
        {
            string dateTime = Validation.getCurrentServerStringDateTime();
            string TimeLineKey = aobjectLink + "_" + "FILE" + "_" + dateTime;
            Message = Message.Replace("'", "''");

            Timeline timeLine = new Timeline();
            timeLine.SID = ERPWAuthentication.SID;
            timeLine.CompanyCode = ERPWAuthentication.CompanyCode;
            timeLine.ObjectLink = aobjectLink;
            timeLine.TimelineKey = TimeLineKey;
            timeLine.Type = Timeline.TYPE_ATTACH_FILE.ToString();
            timeLine.Message = Message;
            timeLine.ContentUri = "";
            timeLine.ContentUrl = "";
            timeLine.Status = "";
            timeLine.Latitude = "";
            timeLine.Longitude = "";
            timeLine.Address = "";
            timeLine.CreatorId = ERPWAuthentication.EmployeeCode;
            timeLine.CreatorName = ERPWAuthentication.FullNameTH;
            timeLine.LinkId = ERPWAuthentication.EmployeeCode;
            timeLine.EmployeeCode = ERPWAuthentication.EmployeeCode;
            timeLine.CreatedOn = dateTime;

            ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
            libService.AddTimeline(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, timeLine);

            //string UploadFilePath = Server.MapPath("~/managefile/" + ERPWAuthentication.SID + "/kmfile/assets/");
            //string UploadFileUrl = Domain + "/managefile/" + ERPWAuthentication.SID + "/kmfile/assets/";

            for (int i = 0; i < dtFile.Rows.Count; i++)
            {
                string Filekey = "FILE" + "_" + Validation.getCurrentServerStringDateTime() + i.ToString();
                string savedFileName = dtFile.Rows[i]["PhysicalFileName"].ToString(); //SaveFile(Files[i], Filekey);
                if (!string.IsNullOrEmpty(savedFileName))
                {
                    TimelineAsset asset = new TimelineAsset();
                    asset.SID = ERPWAuthentication.SID;
                    asset.CompanyCode = ERPWAuthentication.CompanyCode;
                    asset.ObjectLink = TimeLineKey;
                    asset.AssetKey = Filekey;
                    asset.Type = TimelineAsset.TYPE_FILE.ToString();
                    asset.ContentUri = UploadFilePath + savedFileName;
                    asset.ContentUrl = UploadFileUrl + savedFileName;
                    asset.Latitude = "";
                    asset.Longitude = "";
                    asset.Address = "";
                    asset.CreatedBy = ERPWAuthentication.EmployeeCode;
                    asset.CreatedOn = Validation.getCurrentServerStringDateTime();

                    libService.AddTimelineAsset(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, asset);
                }
            }


            return TimeLineKey;
        }

        public DataTable getStartDateTime(string objId)
        {
            string sql = "SELECT a.CreatedOnDate, a.CreatedOnTime, d.Subject, d.Remark FROM cs_servicecall_item a " +
                "JOIN cs_servicecall_header b ON a.ObjectID = b.ObjectID " +
                "JOIN CRM_SERVICECALL_MAPPING_ACTIVITY c ON b.CallerID = c.ServiceDocNo " +
                "JOIN ticket_service_header d ON c.AOBJECTLINK = d.TicketCode " +
                "WHERE a.ObjectID = '" + objId + @"'";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public DataTable getSlaTime(string docType, string priority)
        {
            string sql = "SELECT c.Resolution , b.TierCode " +
                 "From ERPW_BUSINESSOBJECT_MAPPING_TICKET_TYPE a " +
                 "JOIN Link_Tier_Master b ON a.Default_SLAGroup = b.TierGroupCode " +
                 "JOIN Link_Tier_Master_Item c ON c.TierCode = b.TierCode " +
                 "WHERE b.PriorityCode = '" + priority + @"' AND a.TicketType = '" + docType + @"'";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public void saveRating(string callerID, string employeeCode, string rating, string comment)
        {
            string dateStr = DateTime.Now.ToString("yyyyMMddHHmmss", new CultureInfo("en-US"));
            string sql = "INSERT INTO [dbo].[ticket_service_rating] " +
                        "([CallerID],[EmployeeCode],[Rating],[Comment],[CreateOn]) " +
                        "VALUES('" + callerID + @"', '" + employeeCode + @"', '" + rating + @"', '" + comment + @"', '" + dateStr + @"')";

            dbService.selectDataFocusone(sql);

        }
        public DataTable getRating(string callerID, string employeeCode)
        {

            string sql = "SELECT [Rating],[Comment] FROM [dbo].[ticket_service_rating] " +
                "WHERE [CallerID] = '" + callerID + @"' AND [EmployeeCode] = '" + employeeCode + @"'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        #region method for New EndDateTime [stop timer]

        private string[] ignoreStatusList = {
            ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL),
            ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED),
            ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE)
        };
        private DataTable getCurrentTierTicketServiceHeader(string sid, string companyCode, string serviceDocNo)
        {
            string sql = @" select h.*, m.TierCode, m.Tier, cs.Fiscalyear, cs.Doctype from ticket_service_header as h
	                        inner join CRM_SERVICECALL_MAPPING_ACTIVITY as m
	                        on h.TicketCode = m.AOBJECTLINK
	                        inner join (select SNAID, DOCYEAR, ServiceDocNo, max(Tier) as Tier
		                        from CRM_SERVICECALL_MAPPING_ACTIVITY
		                        where SNAID = '" + companyCode + @"' and ServiceDocNo = '" + serviceDocNo + @"'
		                        group by SNAID, DOCYEAR, ServiceDocNo) as currentTier
	                        on currentTier.SNAID = m.SNAID 
	                        and currentTier.DOCYEAR = m.DOCYEAR 
	                        and currentTier.ServiceDocNo = m.ServiceDocNo
	                        and currentTier.Tier = m.Tier
                            inner join cs_servicecall_header as cs
	                        on cs.SID = h.SID 
	                        and cs.CompanyCode = h.CompanyCode
	                        and cs.CallerID = currentTier.ServiceDocNo
	                        where h.SID = '" + sid + @"' and h.CompanyCode = '" + companyCode + "'";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public void PauseTriggerEscalate(string sid, string companyCode, string ticketStatusCode, string serviceDocNo)
        {       
            if (!ignoreStatusList.Contains(ticketStatusCode))
            {
                DataTable ticketServiceHeaderDT = getCurrentTierTicketServiceHeader(sid, companyCode, serviceDocNo);
                if (ticketServiceHeaderDT.Rows.Count > 0)
                {
                    string transactionID = ticketServiceHeaderDT.Rows[0]["TicketCode"].ToString();
                    TriggerService.GetInstance().updateDataTriggerEscalateAndBeforeOverdue(sid, companyCode, transactionID, TriggerService.TRIGGER_STATUS_PAUSE); // เปลี่ยน trigger status เป็น pause เพื่อให้ TriggerAPI ไม่สนใจการทำงานของ trigger ที่เพิ่มไว้ก่อนหน้า
                }
                else
                {
                    throw new Exception("TicketServiceHeader is not found DocNo:" + serviceDocNo);
                }
            }
        }
        private void CalculateNewEndDateTime(string sid, string companyCode, string ticketStatusCode, string serviceDocNo, string lastxLineNo, string reStartDateTimeStr)
        {
            if (!ignoreStatusList.Contains(ticketStatusCode))
            {
              
                DataTable ticketServiceHeaderDT = getCurrentTierTicketServiceHeader(sid, companyCode, serviceDocNo);
                if (ticketServiceHeaderDT.Rows.Count > 0)
                {
                    string ticketCode = ticketServiceHeaderDT.Rows[0]["TicketCode"].ToString();
                    DateTime startDateTime = ObjectUtil.ConvertDateTimeDBToDateTime(ticketServiceHeaderDT.Rows[0]["StartDateTime"].ToString());
                    if (CheckCurrentStopTimer(ticketCode) || CheckPauseBeforeStartTier(sid, companyCode, serviceDocNo, lastxLineNo, startDateTime)) // check ว่า pause อยู่ไหม จึงจะทำงานได้
                    {
                        TimeSpan tsWorkTime = new TimeSpan(0);

                        #region หาเวลาทำงานที่ใช้
                        string where = @" WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + @"' 
                              AND CallerID = '" + serviceDocNo + "'";
                        string sqlStop = "SELECT * FROM cs_servicecall_stop_timer " + where + " ORDER BY xLineNo ASC";
                        DataTable dtStop = dbService.selectDataFocusone(sqlStop);

                        DataRow[] resultdtStopQuery = (from DataRow myR in dtStop.Rows
                                                       where ObjectUtil.ConvertDateTimeDBToDateTime(myR["StopDate"].ToString() + myR["StopTime"].ToString()) >= startDateTime
                                                       select myR).ToArray();

                        if (resultdtStopQuery.Length > 0)
                        {
                            for (int i = 0; i < resultdtStopQuery.Length; i++)
                            {
                                DateTime stop = ObjectUtil.ConvertDateTimeDBToDateTime(resultdtStopQuery[i]["StopDate"].ToString() + resultdtStopQuery[i]["StopTime"].ToString());

                                if (stop >= startDateTime)
                                {
                                    if (i == 0)
                                    {
                                        tsWorkTime += (stop - startDateTime) - CalculateOtherTime(startDateTime, stop);
                                    }
                                    else
                                    {
                                        DateTime restartLowerLine = ObjectUtil.ConvertDateTimeDBToDateTime(resultdtStopQuery[i - 1]["RestartDate"].ToString() + resultdtStopQuery[i]["RestartTime"].ToString());
                                        tsWorkTime += (stop - restartLowerLine) - CalculateOtherTime(restartLowerLine, stop);
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Set Trigger
                        DataTable dtTier = AfterSaleService.getInstance().getTierOperation(sid, ticketServiceHeaderDT.Rows[0]["TierCode"].ToString(), serviceDocNo);
                        DataRow[] drr = dtTier.Select("Tier = '" + ticketServiceHeaderDT.Rows[0]["Tier"].ToString() + "'");

                        TimeSpan tsResolutionTimeOriginal = new TimeSpan(0, 0, Convert.ToInt32(drr[0]["Resolution"].ToString())); // เวลา SLA ดั้งเดิม ก่อนคำนวณวันหยุด

                        TimeSpan tsNewResolutionTime = tsResolutionTimeOriginal - (tsWorkTime.TotalSeconds >= TimeSpan.FromSeconds(0).TotalSeconds ? tsWorkTime : TimeSpan.FromSeconds(0)); // เวลา SLA ที่เหลือ
                        double tsNewResolutionTimeSeconds = tsNewResolutionTime.TotalSeconds;

                        DateTime restartCurrent = ObjectUtil.ConvertDateTimeDBToDateTime(reStartDateTimeStr);
                        double newResolutionTime = CalculateNewResolutionTime(tsNewResolutionTimeSeconds, restartCurrent); // เวลา SLA ที่เหลือหลังจากคำนวณวันหยุด

                        double seconds = newResolutionTime;        
                        string ticketType = ticketServiceHeaderDT.Rows[0]["Doctype"].ToString();
                        string ticketYear = ticketServiceHeaderDT.Rows[0]["Fiscalyear"].ToString();
                        double requesterTime = Convert.ToDouble(drr[0]["Requester"].ToString());

                        int newResolutionTimeInt = Convert.ToInt32(newResolutionTime);
                        TimeSpan newResolutionTimeTS = new TimeSpan(0, 0, 0, newResolutionTimeInt);
                        DateTime newEndDateTime = restartCurrent.Add(newResolutionTimeTS);
                        DateTime oldEndDateTime = ObjectUtil.ConvertDateTimeDBToDateTime(ticketServiceHeaderDT.Rows[0]["EndDateTime"].ToString());

                        RemoveTriggerEscalateAndbefOverdue(sid, companyCode, ticketCode); // ลบ Trigger เดิมออก

                        SetTriggerBeforeOverdue(ticketCode, ticketType, serviceDocNo, ticketYear, seconds, requesterTime, ERPWAuthentication.UserName); // เพิ่ม Trigger ที่คำนวณเวลาใหม่
                        TriggerService.GetInstance().EscalateTicket(ticketCode, ticketType, serviceDocNo, ticketYear, seconds.ToString(), ERPWAuthentication.UserName); // เพิ่ม Trigger ที่คำนวณเวลาใหม่

                        if (restartCurrent < oldEndDateTime)
                        {
                            TriggerService.GetInstance().updateDataTriggerEscalateAndBeforeOverdue(sid, companyCode, ticketCode, TriggerService.TRIGGER_STATUS_CONTINUE); // ระบุเพื่อให้ TriggerAPI รู้ว่าให้ทำงานต่อไปได้
                        }

                        CultureInfo cul = new CultureInfo("en-US");
                        string newEndDateTimeStr = Validation.Convert2DateTimeDB(newEndDateTime.ToString("dd/MM/yyyy HH:mm:ss", cul));
                        UpdateEndDateTimeToTicketServiceHeader(sid, companyCode, ticketCode, newEndDateTimeStr);
                        #endregion
                    }
                } else
                {
                    throw new Exception("TicketServiceHeader is not found DocNo:" + serviceDocNo);
                }
            }
        }
        private void UpdateEndDateTimeToTicketServiceHeader(string SID, string CompanyCode, string TicketCode, string newEndDateTime)
        {
            string sql = @"
                update ticket_service_header 
                set EndDateTime = '" + newEndDateTime + @"' 
                where SID = '" + SID + @"' 
                AND CompanyCode = '" + CompanyCode + @"' 
                AND TicketCode = '" + TicketCode + @"'
                ";
            dbService.executeSQLForFocusone(sql);
        }
        private void RemoveTriggerEscalateAndbefOverdue(string SID, string CompanyCode, string TicketCode)
        {
            string sql = @"
                delete from ERPW_TRIGGER_STATUS 
                where SID = '" + SID + @"' 
                AND CompanyCode = '" + CompanyCode + @"' 
                AND (TransactionID = '" + TicketCode + @"' or TransactionID = '" + TicketCode + "befOverdue')";
            dbService.executeSQLForFocusone(sql);
        }
        public bool CheckCurrentStopTimer(string aojectlink)
        {
            bool nowStop = false;

            if (!String.IsNullOrEmpty(aojectlink))
            {
                ERPW_TRIGGER_STATUS triggerData = TriggerService.GetInstance().GetTriggerData(aojectlink);

                if (triggerData.TriggerStatus == TriggerService.TRIGGER_STATUS_PAUSE)
                {
                    nowStop = true;
                }
            }
            return nowStop;
        }
        private TimeSpan CalculateOtherTime(DateTime startDateTime, DateTime endDateTime)
        {
            CultureInfo cul = new CultureInfo("en-US");
            MasterWorkingTimeConfigLib workingtimeConfigLib = new MasterWorkingTimeConfigLib();
            List<WorkingTimeConfig> lst_wtEn = workingtimeConfigLib.GetWorkingTime_Config(
                   ERPWAuthentication.SID,
                   ERPWAuthentication.CompanyCode
            );

            TimeSpan totalTimeOther = new TimeSpan(0);

            if (lst_wtEn.Count > 0)
            {
                int hhStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(0, 2));
                int mmStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(2, 2));
                int ssStart = Convert.ToInt32(lst_wtEn[0].StartTime.Substring(4, 2));
                int hhEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(0, 2));
                int mmEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(2, 2));
                int ssEnd = Convert.ToInt32(lst_wtEn[0].EndTime.Substring(4, 2));
                TimeSpan workdayStart = new TimeSpan(hhStart, mmStart, ssStart);
                TimeSpan workdayEnd = new TimeSpan(hhEnd, mmEnd, ssEnd);

                var dueStartDate = startDateTime;
                var totalTime = (endDateTime - startDateTime);

                do
                {
                    List<string> list_holidays = workingtimeConfigLib.GetHolidaysByDate(
                                            ERPWAuthentication.SID,
                                            ERPWAuthentication.CompanyCode,
                                            MasterWorkingTimeConfigLib.Holidays_TYPE,
                                            Validation.Convert2DateTimeDB(dueStartDate.ToString("dd/MM/yyyy HH:mm:ss", cul)).Substring(0, 8));

                    if (lst_wtEn[0].Workday[(int)dueStartDate.DayOfWeek] == '0' || list_holidays.Contains(dueStartDate.ToString("yyyyMMdd", cul)))
                    {
                        DateTime dueStartDateTemp = dueStartDate;

                        //jump to start of next day
                        dueStartDateTemp = dueStartDateTemp.AddDays(1);
                        dueStartDateTemp = new DateTime(dueStartDateTemp.Year, dueStartDateTemp.Month, dueStartDateTemp.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);

                        TimeSpan tsStop = dueStartDateTemp - dueStartDate;
                        totalTime = totalTime.Subtract(tsStop);
                        totalTimeOther = totalTimeOther.Add(tsStop);

                        dueStartDate = dueStartDateTemp;
                    }
                    else
                    {
                        DateTime startWorkDayTemp = new DateTime(dueStartDate.Year, dueStartDate.Month, dueStartDate.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);
                        DateTime endWorkDayTemp = new DateTime(dueStartDate.Year, dueStartDate.Month, dueStartDate.Day, workdayEnd.Hours, workdayEnd.Minutes, workdayEnd.Seconds);

                        if (endDateTime > endWorkDayTemp) // หยุดงานเกินเวลางานใน 1 วัน
                        {
                            DateTime dueStartDateTemp = dueStartDate;

                            //jump to start of next day
                            dueStartDateTemp = dueStartDateTemp.AddDays(1);
                            dueStartDateTemp = new DateTime(dueStartDateTemp.Year, dueStartDateTemp.Month, dueStartDateTemp.Day, workdayStart.Hours, workdayStart.Minutes, workdayStart.Seconds);

                            TimeSpan tsStop = dueStartDateTemp - dueStartDate;
                            totalTime = totalTime.Subtract(tsStop);

                            TimeSpan tsStopOther = dueStartDateTemp - endWorkDayTemp;
                            totalTimeOther = totalTimeOther.Add(tsStopOther);

                            dueStartDate = dueStartDateTemp;
                        }
                        else
                        {
                            if (dueStartDate <= startWorkDayTemp)
                            {
                                TimeSpan tsStopOther = (startWorkDayTemp - dueStartDate);
                                totalTimeOther = totalTimeOther.Add(tsStopOther);
                            }

                            TimeSpan tsStop = endDateTime - dueStartDate;
                            totalTime = totalTime.Subtract(tsStop);
                        }
                    }
                } while (totalTime.Ticks > 0);
            }

            return totalTimeOther;
        }
        private bool IsOverTime(string sid, string companyCode, string serviceDocNo)
        {
            bool overtime = false;
            DataTable ticketServiceHeaderDT = getCurrentTierTicketServiceHeader(sid, companyCode, serviceDocNo);
            if (ticketServiceHeaderDT.Rows.Count > 0)
            {
                //DataTable dtTier = AfterSaleService.getInstance().getTierOperation(sid, ticketServiceHeaderDT.Rows[0]["TierCode"].ToString(), serviceDocNo);
                //DataRow[] drr = dtTier.Select("Tier = '" + ticketServiceHeaderDT.Rows[0]["Tier"].ToString() + "'");

                //string currentSequence = drr[0]["sequence"].ToString();

                //drr = dtTier.Select("sequence > " + currentSequence, "sequence ASC");

                //if (drr.Length > 0) // Check has next tier
                //{
                //    overdue = false;
                //}

                DateTime endDateTime = ObjectUtil.ConvertDateTimeDBToDateTime(ticketServiceHeaderDT.Rows[0]["EndDateTime"].ToString());
                string currentDateTimeStr = Validation.Convert2DateTimeDB(Validation.getCurrentServerDateTime().ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US")));
                DateTime currentDateTime = ObjectUtil.ConvertDateTimeDBToDateTime(currentDateTimeStr);
                if (currentDateTime > endDateTime)
                {
                    overtime = true;
                }
            }
            return overtime;
        }
        private bool CheckPauseBeforeStartTier(string sid, string companyCode, string ticketNo, string xLineNo, DateTime startDateTime)
        {
            bool isBefore = false;
            string where = @" WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + @"' 
                              AND CallerID = '" + ticketNo + "' AND xLineNo = '" + xLineNo + "'";

            string stopDateString = ServiceTicketLibrary.LookUpTable("StopDate", "cs_servicecall_stop_timer",where);
            string stopTimeString = ServiceTicketLibrary.LookUpTable("StopTime", "cs_servicecall_stop_timer", where);

            DateTime stopDateTime = ObjectUtil.ConvertDateTimeDBToDateTime(stopDateString + stopTimeString);

            if (stopDateTime <= startDateTime)
            {
                isBefore = true;
            }

            return isBefore;
        }
        public bool IsTicketStatusStopTimer(string sid, string companyCode, string callid)
        {
            bool stopTimer = false;
            DataTable dt = getDataServiceTicketHeader(sid, companyCode, callid);
            if (dt.Rows.Count > 0)
            {
                string statusCode = dt.Rows[0]["Docstatus"].ToString();
                //Get event start / stop from status
                string stopTimerString = ServiceTicketLibrary.LookUpTable("StopTimer", "ERPW_TICKET_STATUS",
                    "WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + "' AND TicketStatusCode = '" + statusCode + "'");

                bool.TryParse(stopTimerString, out stopTimer);
            }
            return stopTimer;
        }
        #endregion

        #region Get & Update Ticket API

        public DataTable GetTicketDetailByTicketNumber(string sid, string companyCode, string ticketNumber)
        {
            string sql = @"
                select Fiscalyear, Doctype, CallerID, Docstatus, CustomerCode, HeaderText from cs_servicecall_header with(nolock)
                 WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + "' and CallerID = '" + ticketNumber + @"'
                 order by CREATED_BY desc
            ";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public string ConvertToTicketDB(string ticketNumber)
        {
            if (string.IsNullOrEmpty(ticketNumber))
            {
                return "";
            }
            if (ticketNumber.Split('-').Length == 2)
            {
                var _tk = ticketNumber.Split('-');
                ticketNumber = _tk[0] + "-" + _tk[1].PadLeft(10, '0');
            }
            return ticketNumber;
        }

        public tmpServiceCallDataSet GetTicketBeanStandard(string sessionid,string companyCode
            , string doctype, string docnumber, string fiscalyear)
        {
            tmpServiceCallDataSet serviceTempCallEntity = new tmpServiceCallDataSet();
            Object[] objParam = new Object[] { "1500117",sessionid, companyCode,doctype,docnumber,fiscalyear};
            DataSet[] objDataSet = new DataSet[] { new tmpServiceCallDataSet() };
            ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            if (objReturn != null)
            {

                serviceTempCallEntity.Merge(objReturn.Copy());
            }
            return serviceTempCallEntity;
        }

        public void UpdateTicketBeanStandard(string sessionid,string sid,string companyCode,
            string fiscalyear, string docType, string ticketNo, string UserName
            , tmpServiceCallDataSet serviceCallEntity, List<logValue_OldNew> enLog)
        {
            
            Object[] objParam = new Object[] { "1500141", sessionid };

            DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
            ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            try
            {
                if (enLog == null || enLog.Count <= 0)
                {
                    return;
                }
                List<Main_LogService> en = AfterSaleService.getInstance().SaveLogTicket(sid, docType, fiscalyear, ticketNo, companyCode,
                    UserName, enLog);
            }
            catch (Exception ex)
            {

            }
        }

        public DataTable GetEndDateTimelByTicketNumber(string sid, string companyCode, string ticketNumber)
        {
            string sql = @"
                SELECT * FROM (select xa.ServiceDocNo,  xb.EndDateTime,  xb.MainDelegate, xb.TicketCode
										from (
										  select SNAID, DOCYEAR, ServiceDocNo, max(Tier) as Tier
										  from CRM_SERVICECALL_MAPPING_ACTIVITY
										  where SNAID = '" + companyCode +  @"'
										  group by SNAID, DOCYEAR, ServiceDocNo
										) a
										inner join CRM_SERVICECALL_MAPPING_ACTIVITY xa
										  on  xa.SNAID = a.SNAID 
										  AND xa.DOCYEAR = a.DOCYEAR 
										  AND xa.ServiceDocNo = a.ServiceDocNo 
										  AND xa.Tier = a.Tier
  
										left join ticket_service_header xb
										  on xa.AOBJECTLINK = xb.TicketCode
										  and xb.SID = '"+ sid + @"' 
										  AND xb.CompanyCode = '"+ companyCode + @"'
				                          AND xb.EndDateTime != '') z								  
			WHERE ServiceDocNo = '" + ticketNumber + @"'
            ";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }



        #endregion


    }



    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }


}