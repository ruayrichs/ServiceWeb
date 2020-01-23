using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ERPW.Lib.Authentication;
using Focusone.ICMWcfService;
using Agape.FocusOne.Utilities;
using System.Text;
using agape.lib.web.configuration.utils;
using SNA.Lib.crm.entity;
using ServiceWeb.Util;
using ERPW.Lib.F1WebService.ICMUtils;
using agape.proxy.data.dataset;

namespace ServiceWeb.Service
{
    public class CustomerService
    {
        public static string CONTACT_PROPERTIES_CODE_CONTACTTYPE = "90";
        public static string CONTACT_PROPERTIES_CODE_BEHAVIOR = "91";
        LookupICMService lookupICMService = new LookupICMService();
        DBService dbService = new DBService();
        private static CustomerService _instance = null;
        public static CustomerService getInstance()
        {
            if (_instance == null)
                _instance = new CustomerService();
            return _instance;
        }

        public DataTable getPaymentTerms(string sid)
        {
            string sql = "SELECT * FROM master_payment_terms WHERE SID='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getSearchCustomerType(string customertype)
        {
            DataTable dt = new DataTable();
            string where = "#where 1=1  and Module ='CU'";
            //ICMService.icm
            dt = lookupICMService.GetSearchData(ERPWAuthentication.SID,
                    "SHM000314", where);
            return dt;
        }
        public DataTable getSearchCustomerTypeV2(string customertype)
        {
            DataTable dt = new DataTable();
            string where = "#where 1=1  and Module ='CU'";
            if (!string.IsNullOrEmpty(customertype))
            {
                where += " and BusinessCode='" + customertype + "'";
            }
            dt = lookupICMService.GetSearchData(ERPWAuthentication.SID,
                    "SHM000314", where);
            return dt;
        }
        public DataTable getSearchCriteria(string companycode, string customercode, string customername, string customertype, string customergroup
            , string active)
        {
            DataTable dt;
            string where = "";
            if (!String.IsNullOrEmpty(companycode))
                where += " and A.CompanyCode='" + companycode + "'";
            if (!String.IsNullOrEmpty(customercode))
                where += " and A.CustomerCode like '%" + customercode + "%'";
            if (!String.IsNullOrEmpty(customername))
                where += " and A.CustomerName like '%" + customername + "%'";
            if (!String.IsNullOrEmpty(customertype))
                where += " and F.PostingType='" + customertype + "'";
            if (!String.IsNullOrEmpty(customergroup))
                where += " and A.CustomerGroup='" + customergroup + "'";
            if (!string.IsNullOrEmpty(active))
                where += " and B.Active ='" + active + "'";

            dt = lookupICMService.GetSearchData(ERPWAuthentication.SID,
                    "Cus_master_header", "#where A.SID='"
                        + ERPWAuthentication.SID
                        + "' " + where);
            return dt;
        }
        public DataTable getSearchCustomerGroupCode(string customergroupcode, string postingtype, string companycode, string groupcodeIn)
        {
            DataTable dt = new DataTable();
            string where = "";
            if (customergroupcode != "")
            {
                where += " and A.CustomerGroupCode='" + customergroupcode + "'";
            }
            if (postingtype != "")
            {
                where += " and PostingType='" + postingtype + "'";
            }
            if (companycode != "")
            {
                where += " and A.Companycode='" + companycode + "'";
            }
            if (!string.IsNullOrEmpty(groupcodeIn))
            {
                where += " and A.CustomerGroupCode in (" + groupcodeIn + ")";
            }
            dt = lookupICMService.GetSearchData(ERPWAuthentication.SID,
                    "SHM000189", "#where A.SID='"
                        + ERPWAuthentication.SID
                        + "' " + where);
            return dt;
        }
        public DataTable getSearchCriteria_V2(string companycode, string customercode, string customername, string customertype, string customergroup
            , string active, string pricelist, string pricegroup, string membercode, string foreignname)
        {
            DataTable dt;
            string where = "";
            where += " and A.CompanyCode='" + companycode + "'";
            if (!String.IsNullOrEmpty(customercode))
                where += " and A.CustomerCode like '%" + customercode + "%'";
            if (!String.IsNullOrEmpty(customername))
                where += " and A.CustomerName like '%" + customername + "%'";
            if (!String.IsNullOrEmpty(customertype))
                where += " and F.PostingType='" + customertype + "'";
            if (!String.IsNullOrEmpty(customergroup))
                where += " and A.CustomerGroup='" + customergroup + "'";
            if (!string.IsNullOrEmpty(active))
                where += " and B.Active ='" + active + "'";
            if (!string.IsNullOrEmpty(pricelist))
                where += " and A.PriceList='" + pricelist + "'";
            if (!string.IsNullOrEmpty(pricegroup))
                where += " and A.PriceGroup='" + pricegroup + "'";
            if (!string.IsNullOrEmpty(foreignname))
                where += " and A.ForeignName like '%" + foreignname + "%'";
            if (!string.IsNullOrEmpty(membercode))
                where += " and H.MEMBERCARD_ID like '%" + membercode + "%'";

            dt = lookupICMService.GetSearchData(ERPWAuthentication.SID,
                    "Cus_master_header", "#where A.SID='"
                        + ERPWAuthentication.SID
                        + "' " + where);
            return dt;
        }
        public DataTable getSearchCriteriaByOption_V2(string optionvalue, string companycode, string customercode, string customername,
            string customertype, string customergroup, string active, string pricelist, string pricegroup, string membercode, string foreignname)
        {
            string where = "";
            if ("shAll".Equals(optionvalue))
                where = "";
            else if ("op_a".Equals(optionvalue))
                where = " and PostingType='LCUST' "; //ลูกค้า Lead 20 รายล่าสุด order by a.DOCDATE desc
            else if ("op_b".Equals(optionvalue))
                where = " and PostingType='NORCUS' "; //ลูกค้าใหม่ 20 รายล่าสุด  order by a.DOCDATE desc
            if (!String.IsNullOrEmpty(companycode))
                where += " and A.CompanyCode='" + companycode + "'";
            if (!String.IsNullOrEmpty(customercode))
                where += " and A.CustomerCode like '%" + customercode + "%'";
            if (!String.IsNullOrEmpty(customername))
                where += " and A.CustomerName like '%" + customername + "%'";
            if (!String.IsNullOrEmpty(customertype))
                where += " and F.PostingType='" + customertype + "'";
            if (!String.IsNullOrEmpty(customergroup))
                where += " and A.CustomerGroup='" + customergroup + "'";
            if (!string.IsNullOrEmpty(active))
                where += " and B.Active ='" + active + "'";
            if (!string.IsNullOrEmpty(pricelist))
                where += " and A.PriceList='" + pricelist + "'";
            if (!string.IsNullOrEmpty(pricegroup))
                where += " and A.PriceGroup='" + pricegroup + "'";
            if (!string.IsNullOrEmpty(foreignname))
                where += " and A.ForeignName like '%" + foreignname + "%'";
            if (!string.IsNullOrEmpty(membercode))
                where += " and H.MEMBERCARD_ID like '%" + membercode + "%'";

            string sql = "SELECT TOP(20) A.CustomerCode AS CustomerCode,A.CustomerName AS CustomerName, " +
                "A.CustomerGroup AS CustomerGroup,A.FederalTaxID AS FederalTaxID,A.ForeignName AS ForeignName, " +
                "A.CompanyCode AS CompanyCode,A.PriceList AS PriceList,A.PriceGroup AS PriceGroup, " +
                "B.Active AS Active,B.ActiveFrom AS ActiveFrom,B.ActiveTo AS ActiveTo,C.Description AS Description, " +
                "D.Name AS Name,E.PictureFile AS PictureFile,F.PostingType AS PostingType,G.Description AS Description2 " +
                "From master_customer AS A LEFT OUTER JOIN master_customer_general AS B ON A.SID = B.SID and A.CustomerCode = B.CustomerCode " +
                "AND A.CompanyCode = B.CompanyCode LEFT OUTER JOIN master_config_customer_doctype AS C ON A.SID = C.SID and A.CustomerGroup = C.CustomerGroupCode " +
                "AND A.CompanyCode = C.Companycode LEFT OUTER JOIN master_company AS D ON A.SID = D.SID and A.CompanyCode = D.ID " +
                "LEFT OUTER JOIN master_customer_details AS E ON A.SID = E.SID and A.CustomerCode = E.CustomerCode AND A.CompanyCode = E.CompanyCode " +
                "LEFT OUTER JOIN master_config_customer_doctype_docdetail AS F ON A.SID = F.SID and A.CustomerGroup = F.CustomerGroupCode AND A.CompanyCode = F.companyCode " +
                "LEFT OUTER JOIN master_config_business AS G ON F.PostingType = G.BusinessCode " +
                "LEFT OUTER JOIN master_customer_pricelist AS H ON A.SID = H.SID AND A.CompanyCode = H.CompanyCode AND A.CustomerCode = H.CustomerCode where A.SID='"
                    + ERPWAuthentication.SID
                    + "' ";
            sql += where;
            sql += " order by A.created_on desc";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public DataTable getSearchTitleName(string titlename)
        {
            DataTable dt;
            string where = "";
            if (titlename != "")
            {
                where += " and TitleCode='" + titlename + "'";
            }
            dt = lookupICMService.GetSearchData(ERPWAuthentication.SID,
                    "SHM000257", "#where A.SID='"
                        + ERPWAuthentication.SID
                        + "' " + where);
            return dt;
        }
        public DataTable GetConfigMemberCustomerGroupV2(string p_sid, string p_companycode, string p_customergroup)
        {
            string sql = "select * from f1_longtail_pos_conf_member where SID = '" + p_sid + "' and CompanyCode = '" + p_companycode + "'";

            if (!string.IsNullOrEmpty(p_customergroup))
                sql += " and CUSTOMERGROUP = '" + p_customergroup + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }
        public DataTable getSelectedValue(string p_sid, string p_code)
        {
            string sql = "select a.SID,a.Code,a.Description,a.CREATED_BY,a.UPDATED_BY,a.CREATED_ON,a.UPDATED_ON " +
                ",b.xValue as xValue,b.DetailCode as DetailCode from master_conf_selectedvalue a inner join  " +
                " master_conf_selectedvalue_detail b " +
                " on a.SID = b.SID and a.Code = b.Code " +
                " where a.SID='" + p_sid + "'";

            //string sql = "select * from longtail_doctype_mapping where sid='" + sid + "' and xActive='True' ";
            string where = "";

            if (p_code != "" && p_code != "*")
            {
                where += " and a.Code='" + p_code + "'";
            }

            sql += where + " order by b.DetailCode ";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public DataTable getSearchContactMaster(string CustomerCode, string CompanyCode)
        {
            string where = "";
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                where += " and A.CompanyCode = '" + CompanyCode + "' ";
            }
            if (!string.IsNullOrEmpty(CustomerCode))
            {
                where += " and A.CustomerCode = '" + CustomerCode + "' ";
            }
            DataTable dt = lookupICMService.GetSearchData(ERPWAuthentication.SID,
                    "SHM000121", "#where A.SID='"
                        + ERPWAuthentication.SID
                        + "' " + where);
            return dt;
        }
        public DataTable SearchCustomer(String name, string mobile, string email,
                                    string status, string type, string expire, string customer_from, string customer_to, string branch, string memberid)
        {

            DataTable _dt = new DataTable();
            string sid = ERPWAuthentication.SID;
            string companycode = ERPWAuthentication.CompanyCode;

            List<string> sql_arr = new List<string>();
            sql_arr.Add("SELECT c.[CustomerCode] ,c.[CustomerName] ,c.[CustomerGroup] ,c.[Currency] ,c.[FederalTaxID] ,c.[ForeignName] ,c.[CompanyCode] ,c.[SID] ,c.[ChangeCurrency] ,c.[TitleCode] ,p.[CREATED_BY] ,p.[UPDATED_BY] ,p.[CREATED_ON] ,p.[UPDATED_ON] ,c.[CustomerNameTH] ,");
            sql_arr.Add("p.[PriceList], p.[PRICEGROUP] ,p.[MEMBERCARD_ID] ,p.[VALID_FROM] ,p.[VALID_TO] ,p.[ACTIVE] ,p.[Remark] ,p.Birthday, ");
            sql_arr.Add("q.Description,r.TelNo1,r.Mobile,r.EMail,s.BRANCHNAME_TH,t.FirstName_TH,t.LastName_TH,ISNULL(z.PriceGroupDescription,'*') as PriceGroupDescription, ");
            sql_arr.Add("p.FirstName as MemberFName,p.LastName as MemberLName,p.Phone as MemberPhone,p.Mobile as MemberMobile,p.Mail as MemberMail,p.TitleCode as MemberTitle ");

            sql_arr.Add("FROM [master_customer] c WITH (NOLOCK) ");
            sql_arr.Add("inner join [master_customer_pricelist] p WITH (NOLOCK)  on c.sid = p.sid and c.companycode = p.companycode and c.customercode = p.customercode");
            sql_arr.Add("left join [master_conf_pricelist] q WITH (NOLOCK)  on p.SID = q.SID and p.PriceList = q.PriceListCode");
            sql_arr.Add("left join MASTER_CONFIG_PRICEGROUP z  WITH (NOLOCK) on p.SID = z.SID and p.PriceGroup = z.PriceGroupCode");
            sql_arr.Add("left join master_customer_general r WITH (NOLOCK)  on c.sid = r.sid and c.CompanyCode=r.CompanyCode and c.CustomerCode=r.CustomerCode");
            sql_arr.Add("left join dbo.BRANCH_MASTER_GENERAL s WITH (NOLOCK)  on p.SID = s.SID and p.CompanyCode=s.COMPANYCODE and p.BranchCode = s.BRANCHCODE");
            sql_arr.Add("left join dbo.master_employee t WITH (NOLOCK)  on c.SID = t.SID and c.CompanyCode=t.CompanyCode and c.CREATED_BY = t.EmployeeCode");


            //GET LATEST CUSTOMER TRANSACTION
            sql_arr.Add("join (SELECT [SID], [CompanyCode], [CustomerCode], [PriceGroup],[PriceList] , [MEMBERCARD_ID], MAX([CREATED_ON]) AS [CREATED_ON]");
            sql_arr.Add("FROM [master_customer_pricelist] WITH (NOLOCK) ");
            sql_arr.Add("GROUP BY [SID], [CompanyCode], [CustomerCode], [PriceGroup],[PriceList] , [MEMBERCARD_ID]");
            sql_arr.Add(") x on x.SID = p.SID and x.CompanyCode = p.CompanyCode and x.CustomerCode = p.CustomerCode");
            sql_arr.Add("and x.PriceGroup = p.PriceGroup and x.PriceList = p.PriceList and x.MEMBERCARD_ID = p.MEMBERCARD_ID and x.CREATED_ON = p.CREATED_ON");


            sql_arr.Add("where c.sid='" + sid + "' and UPPER(r.active) = 'TRUE' and c.companycode = '" + companycode + "' ");

            if (name != null && !string.IsNullOrEmpty(name))
            {
                sql_arr.Add("and c.[CustomerName] LIKE '%" + name + "%'");
            }
            if (mobile != null && !string.IsNullOrEmpty(mobile))
            {
                sql_arr.Add("and (r.Mobile LIKE '%" + mobile + "%' or r.TelNo1  LIKE '%" + mobile + "%') ");
            }
            if (email != null && !string.IsNullOrEmpty(email))
            {
                sql_arr.Add("and r.EMail LIKE '%" + email + "%'");
            }
            if (status != null && !string.IsNullOrEmpty(status))
            {
                sql_arr.Add("and p.[ACTIVE] = '" + status + "'");
            }
            if (type != null && !string.IsNullOrEmpty(type))
            {
                sql_arr.Add("and p.[PriceList] = '" + type + "'");
            }
            if (expire != null && !string.IsNullOrEmpty(expire))
            {
                sql_arr.Add("and p.[VALID_TO] = '" + expire + "'");
            }
            if (!string.IsNullOrEmpty(memberid))
            {
                sql_arr.Add("and p.[MEMBERCARD_ID] like '%" + memberid + "%'");
            }


            if (customer_from != null && !string.IsNullOrEmpty(customer_from))
            {
                if (customer_to != null && !string.IsNullOrEmpty(customer_to))
                {
                    sql_arr.Add("and c.[CustomerCode] >= '" + customer_from + "' and c.[CustomerCode] <= '" + customer_to + "'");
                }
                else
                {
                    sql_arr.Add("and c.[CustomerCode] = '" + customer_from + "'");
                }
            }

            if (branch != null && !string.IsNullOrEmpty(branch))
            {
                sql_arr.Add("and s.BRANCHCODE = '" + branch + "'");
            }

            sql_arr.Add("order by c.[CustomerCode], p.[PriceList],p.[MEMBERCARD_ID],p.[VALID_FROM]");

            _dt = dbService.selectDataFocusone(string.Join(" ", sql_arr.ToArray()));

            DataTable _dt_new = _dt.Clone();
            _dt_new.Columns.Add("_status", typeof(string));
            _dt_new.Columns.Add("_valid_from", typeof(string));
            _dt_new.Columns.Add("_valid_to", typeof(string));
            _dt_new.Columns.Add("_valid_total", typeof(string));
            _dt_new.Columns.Add("_created_by", typeof(string));
            _dt_new.Columns.Add("_date", typeof(string));
            _dt_new.Columns.Add("_time", typeof(string));

            foreach (DataRow _dr in _dt.Rows)
            {
                DataRow _dr_new = _dt_new.NewRow();

                _dr_new["CustomerCode"] = _dr["CustomerCode"];
                _dr_new["CustomerName"] = _dr["CustomerName"];
                _dr_new["CustomerGroup"] = _dr["CustomerGroup"];
                _dr_new["Currency"] = _dr["Currency"];
                _dr_new["FederalTaxID"] = _dr["FederalTaxID"];
                _dr_new["ForeignName"] = _dr["ForeignName"];
                _dr_new["CompanyCode"] = _dr["CompanyCode"];
                _dr_new["SID"] = _dr["SID"];
                _dr_new["ChangeCurrency"] = _dr["ChangeCurrency"];
                _dr_new["TitleCode"] = _dr["TitleCode"];
                _dr_new["CREATED_BY"] = _dr["CREATED_BY"];
                _dr_new["UPDATED_BY"] = _dr["UPDATED_BY"];
                _dr_new["CREATED_ON"] = _dr["CREATED_ON"];
                _dr_new["UPDATED_ON"] = _dr["UPDATED_ON"];
                _dr_new["CustomerNameTH"] = _dr["CustomerNameTH"];
                _dr_new["PriceList"] = _dr["PriceList"];
                _dr_new["PRICEGROUP"] = _dr["PRICEGROUP"];
                _dr_new["MEMBERCARD_ID"] = _dr["MEMBERCARD_ID"];
                _dr_new["VALID_FROM"] = _dr["VALID_FROM"];
                _dr_new["VALID_TO"] = _dr["VALID_TO"];
                _dr_new["ACTIVE"] = _dr["ACTIVE"];
                _dr_new["Description"] = _dr["Description"];
                _dr_new["TelNo1"] = _dr["TelNo1"];
                _dr_new["Mobile"] = _dr["Mobile"];
                _dr_new["EMail"] = _dr["EMail"];
                _dr_new["BRANCHNAME_TH"] = _dr["BRANCHNAME_TH"];
                _dr_new["FirstName_TH"] = _dr["FirstName_TH"];
                _dr_new["LastName_TH"] = _dr["LastName_TH"];
                _dr_new["MemberFName"] = _dr["MemberFName"];
                _dr_new["MemberLName"] = _dr["MemberLName"];
                _dr_new["MemberPhone"] = _dr["MemberPhone"];
                _dr_new["MemberMobile"] = _dr["MemberMobile"];
                _dr_new["MemberMail"] = _dr["MemberMail"];
                _dr_new["MemberTitle"] = _dr["MemberTitle"];
                _dr_new["Birthday"] = _dr["Birthday"];

                _dr_new["_status"] = "True".Equals(_dr["ACTIVE"].ToString()) ? "ใช้งานได้" : "หมดอายุ";
                _dr_new["_valid_from"] = Validation.Convert2DateDisplay(_dr["VALID_FROM"].ToString());
                _dr_new["_valid_to"] = Validation.Convert2DateDisplay(_dr["VALID_TO"].ToString());

                DateTime exp = DateTime.ParseExact(_dr["VALID_TO"].ToString().Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                DateDifference diff = new DateDifference(DateTime.Now, exp);
                _dr_new["_valid_total"] = diff.ToString();
                _dr_new["_created_by"] = _dr["FirstName_TH"].ToString() + " " + _dr["LastName_TH"].ToString();
                _dr_new["_date"] = Validation.Convert2DateDisplay(_dr["CREATED_ON"].ToString());
                _dr_new["_time"] = Validation.Convert2TimeDisplay(_dr["CREATED_ON"].ToString().Substring(8, 6));

                _dt_new.Rows.Add(_dr_new);
            }

            return _dt_new;
        }
        public DataTable SearchCustomer(string status, string type, string expire, string customer_from, string customer_to, string branch)
        {
            return SearchCustomer("", "", "", status, type, expire, customer_from, customer_to, branch, "");
        }
        public DataTable SearchCustomer(String name, string mobile, string email, string memberid)
        {
            return SearchCustomer(name, mobile, email, "", "", "", "", "", "", memberid);
        }
        public DataTable getNormalCustomerGroup(String Sid, String Companycode)
        {
            string sql = "select A.CustomerGroupCode,A.Description,B.NumberRangeCode,D.PrefixCode, " +
                "D.xStart,D.xEnd,D.SuffixCode,D.xCurrent,D.ExternalOrNot from master_config_customer_doctype as A  WITH (NOLOCK) " +
                "left join master_config_customer_doctype_docdetail as B  WITH (NOLOCK) " +
                "on A.SID = B.SID and A.CompanyCode = B.companyCode and A.CustomerGroupCode = B.CustomerGroupCode " +
                "left join master_config_customer_nr_mapping as C  WITH (NOLOCK) " +
                "on B.SID = C.SID and B.CustomerGroupCode = C.CustomerGroupCode and B.NumberRangeCode = C.NumberRangeCode and B.companyCode = C.CompanyCode " +
                "left join master_config_customer_nr as D  WITH (NOLOCK) " +
                "on C.SID = D.SID and C.NumberRangeCode = D.NumberRangeCode and C.CompanyCode = D.CompanyCode " +
                "where A.SID='" + Sid + "' and A.CompanyCode='" + Companycode + "' and PostingType='NORCUS'";
            DataSet ds = lookupICMService.selectData(sql);
            DataTable dt = ds.Tables[0];
            return dt;
        }
        public DataTable getLeadCustomerGroup(String Sid, String Companycode)
        {
            string sql = "select A.CustomerGroupCode,A.Description,B.NumberRangeCode,D.PrefixCode, " +
                "D.xStart,D.xEnd,D.SuffixCode,D.xCurrent,D.ExternalOrNot from master_config_customer_doctype as A " +
                "left join master_config_customer_doctype_docdetail as B " +
                "on A.SID = B.SID and A.CompanyCode = B.companyCode and A.CustomerGroupCode = B.CustomerGroupCode " +
                "left join master_config_customer_nr_mapping as C " +
                "on B.SID = C.SID and B.CustomerGroupCode = C.CustomerGroupCode and B.NumberRangeCode = C.NumberRangeCode and B.companyCode = C.CompanyCode " +
                "left join master_config_customer_nr as D " +
                "on C.SID = D.SID and C.NumberRangeCode = D.NumberRangeCode and C.CompanyCode = D.CompanyCode " +
                "where A.SID='" + Sid + "' and A.CompanyCode='" + Companycode + "' and PostingType='LCUST'";
            DataSet ds = lookupICMService.selectData(sql);
            DataTable dt = ds.Tables[0];
            return dt;
        }


        public DataTable getCustomerArea(string p_sessionid, string plantType, string p_customername, string p_custTaxId, string p_status)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT A.CustomerName, A.FederalTaxId,B.* FROM master_customer A");
            sql.AppendLine("INNER JOIN master_customer_farm B");
            sql.AppendLine("ON A.SID = B.SID AND A.CompanyCode = B.CompanyCode AND A.CustomerCode = B.CustomerCode");
            sql.AppendLine("LEFT OUTER JOIN master_config_customer_doctype_docdetail C");
            sql.AppendLine("ON A.SID = C.SID AND A.CompanyCode = C.companyCode AND A.CustomerGroup = C.CustomerGroupCode");
            sql.AppendLine("WHERE 1=1 ");
            sql.AppendLine("AND A.CompanyCode='" + ERPWAuthentication.CompanyCode + "'");
            sql.AppendLine(Validation.CreateString(p_status, "B.Status"));
            sql.AppendLine(Validation.CreateString(plantType, "B.Plant"));

            // sql.AppendLine(Validation.CreateString(p_customergroup, "A.CustomerGroup"));
            sql.AppendLine(Validation.CreateString(p_custTaxId, "A.FederalTaxId"));

            if (!ERPWAuthentication.SID.Equals("401"))
            {
                sql.AppendLine(" AND A.SID='" + ERPWAuthentication.SID + "' ");
            }

            if (!String.IsNullOrEmpty(p_customername))
            {
                sql.AppendLine(" and A.customername like '%" + p_customername + "%' ");
            }


            sql.AppendLine("ORDER BY A.CustomerCode,B.ItemNo");

            return lookupICMService.selectData(sql.ToString()).Tables[0];
        }

        public DataTable getAreaHeaderStructure()
        {
            string sql = "SELECT TOP(1) * FROM master_customer_farm_header";

            DataTable dt = lookupICMService.selectData(sql).Tables[0].Clone();

            dt.TableName = "master_customer_farm_header";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["CompanyCode"], dt.Columns["CustomerCode"], dt.Columns["ItemNo"]
                , dt.Columns["ActivityType"], dt.Columns["ActivityCode"] };

            return dt;
        }

        public DataTable getAreaDetailStructure()
        {
            string sql = "SELECT TOP(1) * FROM master_customer_farm_detail";

            DataTable dt = lookupICMService.selectData(sql).Tables[0].Clone();

            dt.TableName = "master_customer_farm_detail";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["CompanyCode"], dt.Columns["CustomerCode"], dt.Columns["ItemLink"], dt.Columns["ItemNo"]
                , dt.Columns["ActivityType"], dt.Columns["ActivityCode"] };
            dt.Columns["Qty"].DefaultValue = 0;
            dt.Columns["GoodsRate"].DefaultValue = 0;
            dt.Columns["Effect"].DefaultValue = 0;
            dt.Columns["GoodsQty"].DefaultValue = 0;
            dt.Columns["RequestRecoveredQty"].DefaultValue = 0;
            dt.Columns["RecoveredQty"].DefaultValue = 0;
            dt.Columns["NoneRecoveredQty"].DefaultValue = 0;

            return dt;
        }

        public DataTable getAreaStructure()
        {
            string sql = "SELECT TOP(1) * FROM master_customer_farm";

            DataTable dt = lookupICMService.selectData(sql).Tables[0].Clone();

            dt.TableName = "master_customer_farm";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["CompanyCode"], dt.Columns["CustomerCode"], dt.Columns["ItemNo"] };
            dt.Columns["Qty"].DefaultValue = 0;

            return dt;
        }

        public void SaveAreaManagement(DataTable p_dt)
        {
            if (p_dt.Columns.Contains("CustomerName"))
            {
                p_dt.Columns.Remove("CustomerName");
            }

            dbService.SaveTransactionForFocusone(p_dt);
        }

        public DataTable getAreaDetail(string p_customercode, string p_itemno)
        {
            string sql = "SELECT * FROM master_customer_farm_detail " +
                "WHERE SID='" + ERPWAuthentication.SID + "' " +
                "AND CompanyCode='" + ERPWAuthentication.CompanyCode + "' " +
                "AND CustomerCode='" + p_customercode + "' " +
                "AND ItemLink='" + p_itemno + "' ";

            return lookupICMService.selectData(sql).Tables[0];
        }

        public DataTable getAreaDetail(string p_customercode, string p_itemno, string p_activitycode)
        {
            string sql = "SELECT * FROM master_customer_farm_detail " +
                "WHERE SID='" + ERPWAuthentication.SID + "' " +
                "AND CompanyCode='" + ERPWAuthentication.CompanyCode + "' " +
                "AND CustomerCode='" + p_customercode + "' " +
                "AND ItemLink='" + p_itemno + "' " +
                //"AND ActivityType='" + p_activitytype + "'" +
                "AND ActivityCode='" + p_activitycode + "'";

            return lookupICMService.selectData(sql).Tables[0];
        }

        public DataTable getContactType(string sid)
        {
            string sql = @"select b.DetailCode as Code,b.xValue as Description from dbo.master_conf_properties a
                inner join master_conf_selectedvalue_detail b 
                on a.SID = b.SID and a.SelectedCode=b.Code
                 where a.sid='" + sid + "' and a.xType='CRMCONTACT' and a.PropertiesCode='" + CONTACT_PROPERTIES_CODE_CONTACTTYPE + "'";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getContactBehavior(string sid)
        {
            string sql = @"select b.DetailCode as Code,b.xValue as Description from dbo.master_conf_properties a
                inner join master_conf_selectedvalue_detail b 
                on a.SID = b.SID and a.SelectedCode=b.Code
                 where a.sid='" + sid + "' and a.xType='CRMCONTACT' and a.PropertiesCode='" + CONTACT_PROPERTIES_CODE_BEHAVIOR + "'";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getContactMasterPropertiesData(string sid, string PropertiesCode)
        {
            string sql = @" select * from dbo.master_conf_properties 
                 where sid='" + sid + "' and xType='CRMCONTACT' and PropertiesCode='" + PropertiesCode + "'";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getCustomerGroupNr(string sid, string companycode, string customerGrouoCode)
        {
            string sql = @" select b.* from dbo.master_config_customer_nr_mapping a
inner join master_config_customer_nr b
on a.SID = b.SID and a.CompanyCode = b.CompanyCode and a.NumberRangeCode = b.NumberRangeCode
where a.SID = '" + sid + "' and a.CompanyCode='" + companycode + "' and a.CustomerGroupCode='" + customerGrouoCode + "'";
            return dbService.selectDataFocusone(sql);

        }

        public DataTable searchContact(string sid, string companyCode, string customerCode)
        {
            string _sql = @"SELECT A.BPCODE,C.CustomerName,A.AOBJECTLINK,B.BOBJECTLINK,B.NAME1 FROM CONTACT_MASTER A WITH (NOLOCK) 
                            INNER JOIN CONTACT_DETAILS B WITH (NOLOCK) 
                            ON A.SID = B.SID AND A.AOBJECTLINK = B.AOBJECTLINK
                            INNER JOIN master_customer C WITH (NOLOCK) 
                            ON A.SID = C.SID AND A.COMPANYCODE = C.CompanyCode AND A.BPCODE = C.CustomerCode
                            WHERE A.SID='" + sid + "' AND A.COMPANYCODE='" + companyCode + "'";

            _sql += Validation.CreateString(customerCode, "A.BPCODE");

            DataTable dt = dbService.selectDataFocusone(_sql);

            return dt;
        }

        public DataTable searchContactWithoutCustomer(string sid, string companyCode)
        {
//            string _sql = @"SELECT A.BPCODE,C.CustomerName,A.AOBJECTLINK,B.BOBJECTLINK,B.NAME1 FROM CONTACT_MASTER A
//                            INNER JOIN CONTACT_DETAILS B
//                            ON A.SID = B.SID AND A.AOBJECTLINK = B.AOBJECTLINK
//                            INNER JOIN master_customer C
//                            ON A.SID = C.SID AND A.COMPANYCODE = C.CompanyCode AND A.BPCODE = C.CustomerCode
//                            WHERE A.SID='" + sid + "' AND A.COMPANYCODE='" + companyCode + "'";

            string _sql = @"select TOP 100 A.BPCODE,ISNULL(b.NAME1,'') +' ' + ISNULL(b.NAME2,'') as CustomerName,A.AOBJECTLINK,B.BOBJECTLINK,B.NAME1 from CONTACT_MASTER A WITH (NOLOCK) 
                            INNER JOIN CONTACT_DETAILS B WITH (NOLOCK) 
                             ON A.SID = B.SID AND A.AOBJECTLINK = B.AOBJECTLINK
                                 WHERE A.SID='" + sid + "' AND A.COMPANYCODE='" + companyCode + "'";


            DataTable dt = dbService.selectDataFocusone(_sql);

            return dt;
        }

        public DataTable searchContactEmail(string sid, string companycode, string customerCode, string searchText)
        {
            string _sql = @"SELECT A.BPCODE,C.CustomerName,A.AOBJECTLINK,B.BOBJECTLINK,B.NAME1,D.EMAIL FROM CONTACT_MASTER A WITH (NOLOCK) 
                            INNER JOIN CONTACT_DETAILS B WITH (NOLOCK) 
                            ON A.SID = B.SID AND A.AOBJECTLINK = B.AOBJECTLINK
                            INNER JOIN master_customer C WITH (NOLOCK) 
                            ON A.SID = C.SID AND A.COMPANYCODE = C.CompanyCode AND A.BPCODE = C.CustomerCode
                            inner JOIN CONTACT_EMAIL D WITH (NOLOCK) 
                            ON A.SID = D.SID AND B.BOBJECTLINK = D.BOBJECTLINK
                            WHERE A.SID='" + sid + "' AND A.COMPANYCODE='" + companycode + "'";

            _sql += Validation.CreateString(customerCode, "A.BPCODE");

            if (!string.IsNullOrEmpty(searchText))
            {
                _sql += " AND (A.BPCODE LIKE '%" + searchText + "%'";
                _sql += " OR B.NAME1 LIKE '%" + searchText + "%'";
                _sql += " OR C.CustomerName LIKE '%" + searchText + "%')";
            }

            DataTable dt = dbService.selectDataFocusone(_sql);

            return dt;
        }

        public DataTable smartSearchCustomer(string sid, string companyCode, string searchText)
        {
            string _sql = @"SELECT TOP(100) A.CustomerCode,A.CustomerName 
                            FROM master_customer A
                            INNER JOIN master_customer_general B
                            ON A.SID = B.SID AND A.CompanyCode = B.CompanyCode AND A.CustomerCode = B.CustomerCode
                            WHERE A.SID='" + sid + "' AND A.CompanyCode='" + companyCode + "' AND B.Active='True'";

            if (!string.IsNullOrEmpty(searchText))
            {
                _sql += " AND (A.CustomerCode LIKE '%" + searchText + "%'";
                _sql += " OR A.CustomerName LIKE '%" + searchText + "%')";
            }

            DataTable dt = dbService.selectDataFocusone(_sql);

            return dt;
        }

        public DataTable smartSearchContact(string sid, string companyCode, string customerCode, string searchText)
        {
            string _sql = @"SELECT TOP(100) A.BPCODE,C.CustomerName,A.AOBJECTLINK,B.BOBJECTLINK,B.NAME1 FROM CONTACT_MASTER A
                            INNER JOIN CONTACT_DETAILS B
                            ON A.SID = B.SID AND A.AOBJECTLINK = B.AOBJECTLINK
                            INNER JOIN master_customer C
                            ON A.SID = C.SID AND A.COMPANYCODE = C.CompanyCode AND A.BPCODE = C.CustomerCode
                            WHERE A.SID='" + sid + "' AND A.COMPANYCODE='" + companyCode + "'";

            _sql += Validation.CreateString(customerCode, "A.BPCODE");

            if (!string.IsNullOrEmpty(searchText))
            {
                _sql += " AND (A.BPCODE LIKE '%" + searchText + "%'";
                _sql += " OR B.NAME1 LIKE '%" + searchText + "%'";
                _sql += " OR C.CustomerName LIKE '%" + searchText + "%')";
            }

            DataTable dt = dbService.selectDataFocusone(_sql);

            return dt;
        }

        public DataTable smartSearchContactWithoutCustomer(string sid, string companyCode, string searchText)
        {

            string _sql = @"select TOP(100) A.BPCODE,ISNULL(b.NAME1,'') +' ' + ISNULL(b.NAME2,'') as CustomerName,A.AOBJECTLINK,B.BOBJECTLINK,B.NAME1 from CONTACT_MASTER A
                            INNER JOIN CONTACT_DETAILS B
                             ON A.SID = B.SID AND A.AOBJECTLINK = B.AOBJECTLINK
                                 WHERE A.SID='" + sid + "' AND A.COMPANYCODE='" + companyCode + "'";
            if (!string.IsNullOrEmpty(searchText))
            {
                _sql += " AND (A.BPCODE LIKE '%" + searchText + "%'";
                _sql += " OR B.NAME1 LIKE '%" + searchText + "%'";
                _sql += " OR B.NAME2 LIKE '%" + searchText + "%')";
            }

            DataTable dt = dbService.selectDataFocusone(_sql);

            return dt;
        }

        public string getCustomerName(string sid, string companyCode, string customerCode)
        {
            string sql = "SELECT CustomerName, ForeignName FROM master_customer  WITH (NOLOCK) " +
                "WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "' AND CustomerCode='" + customerCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["CustomerName"].ToString();
            }

            return "";
        }

        public string CopyCustomerToContact(string p_sessionid, string p_sid, string p_compaycode, string p_employeecode, dsVenCus CustomerDataSet, string p_customercode)
        {
            contactsdataset ContactDataSet = new contactsdataset();

            string msg = "";
            string sid = p_sid;
            string companycode = p_compaycode;
            string bptype = "C";
            string created_by = p_employeecode;
            string created_on = Validation.getCurrentServerStringDateTime();
            string bpcode = "", name1 = "", active = "", startdate = "",
                enddate = "", email = "", mobile = "";

            #region "Copy"
            if (CustomerDataSet.master_customer.Rows.Count > 0)
            {
                foreach (DataRow dr in CustomerDataSet.master_customer.Rows)
                {
                    bpcode = p_customercode.Trim();
                    name1 = Convert.ToString(dr["CustomerName"]);
                }
            }
            if (CustomerDataSet.master_customer_general.Rows.Count > 0)
            {
                foreach (DataRow dr in CustomerDataSet.master_customer_general.Rows)
                {
                    active = Convert.ToString(dr["Active"]);
                    startdate = Convert.ToString(dr["ActiveDateFrom"]);
                    enddate = Convert.ToString(dr["ActiveDateTo"]);
                    email = Convert.ToString(dr["EMail"]);
                    mobile = Convert.ToString(dr["Mobile"]);
                }
            }
            #endregion
            #region "Paste"
            DataRow drMaster = ContactDataSet.CONTACT_MASTER.NewRow();
            drMaster["SID"] = sid;
            drMaster["COMPANYCODE"] = companycode;
            drMaster["BPTYPE"] = bptype;
            drMaster["BPCODE"] = bpcode;
            drMaster["BPNAME1"] = name1;
            drMaster["BPNAME2"] = "";
            drMaster["AOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim();
            drMaster["CREATED_BY"] = created_by;
            drMaster["CREATED_ON"] = created_on;
            ContactDataSet.CONTACT_MASTER.Rows.Add(drMaster);
            DataRow drDetails = ContactDataSet.CONTACT_DETAILS.NewRow();
            drDetails["SID"] = sid;
            drDetails["AOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim();
            drDetails["ITEMNO"] = "001";
            drDetails["NAME1"] = name1;
            drDetails["NAME2"] = "";
            drDetails["REMARK1"] = "";
            drDetails["REMARK2"] = "";
            drDetails["PersonalID"] = "";
            drDetails["IMAGELINK"] = "";
            drDetails["ACTIVESTATUS"] = active;
            drDetails["STARTDATE"] = startdate;
            drDetails["ENDDATE"] = enddate;
            drDetails["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + "001";
            drDetails["CREATED_BY"] = created_by;
            drDetails["CREATED_ON"] = created_on;
            drDetails["MEMBERCARD_ID"] = "";
            drDetails["BirthDay"] = "";


            ContactDataSet.CONTACT_DETAILS.Rows.Add(drDetails);
            if (!string.IsNullOrEmpty(email))
            {
                DataRow drEmail = ContactDataSet.CONTACT_EMAIL.NewRow();
                drEmail["SID"] = sid;
                drEmail["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + "001";
                drEmail["ITEMNO"] = "001";
                drEmail["MAILTYPE"] = "E";
                drEmail["EMAIL"] = email;
                drEmail["CREATED_BY"] = created_by;
                drEmail["CREATED_ON"] = created_on;
                ContactDataSet.CONTACT_EMAIL.Rows.Add(drEmail);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                DataRow drMobile = ContactDataSet.CONTACT_PHONE.NewRow();
                drMobile["SID"] = sid;
                drMobile["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + "001";
                drMobile["ITEMNO"] = "001";
                drMobile["CONUNTRYCODE"] = "";
                drMobile["PHONETYPE"] = "M";
                drMobile["PHONENUMBER"] = mobile;
                drMobile["EXT"] = "";
                drMobile["REMARKS"] = "";
                drMobile["CREATED_BY"] = created_by;
                drMobile["CREATED_ON"] = created_on;
                ContactDataSet.CONTACT_PHONE.Rows.Add(drMobile);
            }
            #endregion

            if (ContactDataSet.CONTACT_MASTER.Rows.Count > 0)
            {                
                ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

                Object[] objParam = new Object[] { "1Z00002", p_sessionid };
                DataSet[] objDataSet = new DataSet[] { ContactDataSet };
                DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
                if (objReturn != null)
                {
                    if (objReturn.Tables[0].Rows.Count > 0)
                    {
                        if (objReturn.Tables[0].Rows[0]["Message"].Equals("True"))
                        {
                            msg = "Copy Customer To Contact Success.\n";
                        }
                        else
                        {
                            msg = "Copy Customer To Contact Unsuccess. !!!\n";
                        }
                    }
                }
            }

            return msg;
        }

        public string getInviteCustomerGroup(string sid, string companyCode)
        {
            string sql = "SELECT * FROM link_mapping_business_document WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "' AND Business='INVITE_CUSTOMER'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DocumentTypeCode"].ToString();
            }

            return "";
        }

        public void deleteCustomerMappingLinkID(string sid, string companyCode, string customerCode)
        {
            string sql = "DELETE FROM customer_mapping_linkid WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "' AND CustomerCode='" + customerCode + "'";

            dbService.executeSQLForFocusone(sql);
        }

        public void saveCustomerMappingLinkID(string sid, string companyCode, string customerGroupCode, string customerCode, string linkId)
        {
            string sql = "SELECT * FROM customer_mapping_linkid " +
                " WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "' AND LinkID='" + linkId + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            dt.TableName = "customer_mapping_linkid";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["CompanyCode"], dt.Columns["LinkID"] };

            if (dt.Rows.Count > 0)
            {
                throw new Exception("ไม่สามารถทำรายการได้ เนื่องจาก Link ID " + linkId + " ได้ถูกกำหนดไว้กับรหัสลูกค้า " + dt.Rows[0]["CustomerCode"] + " แล้ว");
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["SID"] = sid;
                dr["CompanyCode"] = companyCode;
                dr["LinkID"] = linkId;
                dr["CustomerGroupCode"] = customerGroupCode;
                dr["CustomerCode"] = customerCode;
                dt.Rows.Add(dr);
            }

            dbService.SaveTransactionForFocusone(dt);
        }

        public DataTable searchVendor(string sid, string companyCode, string vendor)
        {
            string _sql = "select TOP(30) * from master_vendor A where SID='" + sid + "' and CompanyCode='" + companyCode + "'";

            if (!string.IsNullOrEmpty(vendor))
            {
                _sql += " AND (A.VendorCode LIKE '%" + vendor + "%'";
                _sql += " OR A.VendorName LIKE '%" + vendor + "%')";
            }

            DataTable dt = dbService.selectDataFocusone(_sql);

            return dt;
        }

        public DataTable getSearchCustomerQuotationDocumenttype(string sid)
        {
            string sql = "select cus.*,cusdoc.Description as CustomerGroupName,lodoc.Description as DocumentTypeName,sarea.SAREADESC as SaleAreaName" +
                         ",office.SOFFICENAME as SaleOfficeName,sgroup.SGROUPNAME as SaleGroupName,plant.PLANTNAME1 as PlantName,srg.StoreName from customergroup_mapping_quotation_documenttype cus " +
                         "left outer join master_config_customer_doctype cusdoc on cus.SID = cusdoc.SID and cus.CustomerGroupCode = cusdoc.CustomerGroupCode " +
                         "left outer join master_config_lo_doctype lodoc on cus.SID = lodoc.SID and cus.DocumentTypeCode = lodoc.DocumentTypeCode " +
                         "left outer join mm_conf_define_plant plant on cus.SID = plant.SID and cus.Plant = plant.PLANTCODE " +
                         "left outer join mm_conf_define_storagelocation srg on cus.SID = srg.SID and cus.Plant = srg.PLANTCODE and cus.Storage = srg.STORAGELOCCODE " +
                         "left outer join sd_conf_define_sales_area sarea on cus.SID = sarea.SID and cus.SaleArea = sarea.SAREACODE " +
                         "left outer join sd_conf_define_sales_office office on cus.SID = office.SID and cus.SaleOffice = office.SOFFICECODE " +
                         "left outer join sd_conf_define_sales_group sgroup on cus.SID = sgroup.SID and cus.SaleGroup = sgroup.SGROUPCODE " +
                         "where cus.SID='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public void addCustomerQuotationDocumenttype(string sid, string companycode, string customergroupcode, string documenttypecode, string salearea, string saleorg, string saledivision, string salechannel, string saleoffice, string salegroup, string plant, string storage, string rackbin)
        {
            string sql = "insert into customergroup_mapping_quotation_documenttype (SID, CompanyCode, CustomerGroupCode, DocumentTypeCode, SaleArea, SaleOrg," +
                         " SaleDivision, SaleChannel, SaleOffice, SaleGroup, Plant, Storage, Rackbin) VALUES ('" + sid + "','" + companycode + "','" + customergroupcode + "'" +
                         ",'" + documenttypecode + "','" + salearea + "','" + saleorg + "','" + saledivision + "','" + salechannel + "','" + saleoffice + "','" + salegroup + "'" +
                         ",'" + plant + "','" + storage + "','" + rackbin + "')";

            dbService.executeSQLForFocusone(sql);
        }

        public void editCustomerQuotationDocumenttype(string sid, string companycode, string customergroupcode, string documenttypecode, string salearea, string saleorg, string saledivision, string salechannel, string saleoffice, string salegroup, string plant, string storage, string rackbin)
        {
            string sql = "UPDATE [customergroup_mapping_quotation_documenttype] SET " +
                         "[DocumentTypeCode] = '" + documenttypecode + "' " +
                         ",[SaleArea] = '" + salearea + "' " +
                         ",[SaleOrg] = '" + saleorg + "' " +
                         ",[SaleDivision] = '" + saledivision + "' " +
                         ",[SaleChannel] = '" + salechannel + "' " +
                         ",[SaleOffice] = '" + saleoffice + "' " +
                         ",[SaleGroup] = '" + salegroup + "' " +
                         ",[Plant] = '" + plant + "' " +
                         ",[Storage] = '" + storage + "' " +
                         ",[Rackbin] = '" + rackbin + "' " +

                         "where SID='" + sid + "' and CompanyCode='" + companycode + "' and CustomerGroupCode='" + customergroupcode + "'";

            dbService.executeSQLForFocusone(sql);
        }

        public void deleteCustomerQuotationDocumenttype(string sid, string companycode, string customergroupcode)
        {
            string sql = "delete customergroup_mapping_quotation_documenttype where sid='" + sid + "' and CompanyCode='" + companycode + "' and CustomerGroupCode='" + customergroupcode + "'";

            dbService.executeSQLForFocusone(sql);
        }

        public DataTable getSaleArea(string sid, string salearea)
        {
            string sql = "select * from sd_conf_define_sales_area where sid='" + sid + "' ";
            if (salearea != "")
            {
                sql += "and SAREACODE='" + salearea + "'";
            }

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public DataTable getOfficeArea(string sid)
        {
            string sql = "select a.*,b.SOFFICENAME from sd_conf_assign_office_to_area a " +
                         "left outer join sd_conf_define_sales_office b on a.SID=b.SID and a.SOFFICECODE = b.SOFFICECODE " +
                         "where a.SID='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public DataTable getGroupOffice(string sid)
        {
            string sql = "select a.*,b.SGROUPNAME from sd_conf_assign_group_to_office a " +
                         "left outer join sd_conf_define_sales_group b on a.SID=b.SID and a.SGROUPCODE = b.SGROUPCODE " +
                         "where a.SID='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public DataTable GetActivityServices(string SNAID, string SID, string CompanyCode, string CustomerCode)
        {
            string sql = @"select Contact.AOBJECTLINK, Contact.CustomerCode, Contact.ContactCode,
                              CusDetail.CustomerName, TimeAt.EMPCODE, TimeAt.DATEIN, TimeAt.DATEOUT, TimeAt.SEQ,
                              TimeAt.EMPNAME, TimeAt.EMPSURNAME, TimeAt.[DAY], TimeAt.TIMEIN, TimeAt.TIMEEND, 
                              TimeAt.TIMEOUT, TimeAt.XSTATUS, TimeAt.PROJECTCODE, TimeAt.SUBPROJECT, 
                              TimeAt.JOBDESCRIPTION, TimeAt.REMARKS, TimeAt.ItemType, TimeAt.TLID,
                              TimeAt.checkIn_Date, TimeAt.checkIn_Time, TimeAt.checkOut_Date,
                              TimeAt.checkOut_Time, TimeAt.CompleteDate, TimeAt.ResponeDate, TimeAt.Priority,
                              TimeAt.Haste, TimeAt.ProblemType, TimeAt.Cause, TimeAt.Solution, TimeAt.PointScore, 
                              TimeAt.TaskStatus
                              ,case when '" + Validation.getCurrentServerStringDateTime() + @"' > TimeAt.DATEOUT+timeat.TIMEOUT 
                                then 'TRUE' 
                                else 'FALSE' 
                              end as IsLate 
                              ,case 
                                when TimeAt.CompleteDate <> '' 
                                  then 'SUCCESS' 
                                when TimeAt.ResponeDate <> '' 
                                  then 'RESPONSED'
                                else 'NONSESPONSE' 
                              end as ResponsiblePersonStatus
                              ,case 
                                when timeat.XSTATUS = 'CANCELED' 
                                  then 'CANCELED' 
                                when Checkout_Date is not null and Checkout_Date <> ''
                                  then 'COMPLETED'
                                else 'WORKING'  
                              end as ActivityStatus
    
                            from SNA_" + SNAID + @"_TIMEATTENDANCE TimeAt
                            left join SNA_ACTIVITY_CONTACT Contact
                              on TimeAt.AOBJECTLINK = Contact.AOBJECTLINK
                            left join " + WebConfigHelper.getDatabaseF1Name() + @".dbo.master_customer CusDetail
                              on CusDetail.SID = Contact.SID
                              and CusDetail.CompanyCode = Contact.CompanyCode
                              and CusDetail.CustomerCode = Contact.CustomerCode
  
                            where TimeAt.ItemType = 'SERVICE'
                              and Contact.CustomerCode is not null
                              and Contact.SID = '" + SID + @"'
                              and Contact.CompanyCode = '" + CompanyCode + @"'";
            if (!string.IsNullOrEmpty(CustomerCode))
            {
                sql += " and Contact.CustomerCode = '" + CustomerCode + "'";
            }
            return dbService.selectData(sql);
        }
        public DataTable getParticipantbyAobjectLink(string SNAID, string SID, string AobjLink)
        {
            string sql = @"select a.AOBJECTLINK, a.OBJECT, a.OBJECTTYPE, a.XSTATUS, a.ResponeDate, a.CompleteDate, a.Recall,
                            c.FirstName_TH + ' ' + c.LastName_TH as fullThai,
                            c.FirstName + ' ' + c.LastName as fullEng

                            from [" + WebConfigHelper.getDatabaseSNAName() + @"].[dbo].SNA_" + SNAID + @"_TIMEATTENDANCE_PARTICIPANT a
                            left join SNA_LINK_REFERENCE b
                            on a.OBJECT = b.LINKID 
                            and a.OBJECTTYPE = b.RTYPE
                            and b.SID = '" + SID + @"'
                            left join master_employee c
                            on b.REFOBJ = c.EmployeeCode 
                            and c.SID = '" + SID + @"'
                            where a.AOBJECTLINK = '" + AobjLink + "'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable GetBranch(string p_sid, string p_companycode)
        {
            string sql = @"select a.BRANCHCODE,a.BRANCHNAME_TH,b.SALEDIVISION
                            ,a.BRANCHCODE+' - '+a.BRANCHNAME_TH as Description
                            from BRANCH_MASTER_GENERAL as a
                           inner join BRANCH_MASTER_SALEORG as b on a.SID = b.SID and a.OBJECTID = b.OBJECTID
                           where a.SID='" + p_sid + "' and a.COMPANYCODE='" + p_companycode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable GetCustomerGroup(string p_sid, string p_companycode)
        {
            string sql = @"select conf.*,doc.Description from f1_longtail_pos_conf_member conf
                            left outer join master_config_customer_doctype doc on conf.SID= doc.SID and conf.CompanyCode = doc.Companycode
                            and conf.CUSTOMERGROUP = doc.CustomerGroupCode
                            where conf.SID='" + p_sid + "' and conf.COMPANYCODE='" + p_companycode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public string CreateCustomerAndCreatePricelist(string p_sessionid, string p_sid, string p_companycode, MemberEntity p_member, bool p_createcontact)
        {
            dsVenCus CustomerDataSet = new dsVenCus();
            MemberEntity Member = p_member;
            if (Member.MemberCardID == null || string.IsNullOrEmpty(Member.MemberCardID))
            {
                Member.MemberCardID = "TMP" + Validation.getCurrentServerStringDateTime();
            }

            string msg = "", TMPCustomerCardID = "TMP" + Validation.getCurrentServerStringDateTime();

            string xCustomerGroup = "", CharOfAccount = "", AccountReceivable = "";
            DataTable dtpos_config = GetConfigMemberCustomerGroupV2(p_sid, p_companycode, Member.CustomerGroup);
            if (dtpos_config != null)
            {
                foreach (DataRow drconfig in dtpos_config.Rows)
                {
                    xCustomerGroup = drconfig["CUSTOMERGROUP"].ToString();
                    CharOfAccount = drconfig["CharOfAccount"].ToString();
                    AccountReceivable = drconfig["AccountReceivable"].ToString();
                }
            }

            if (string.IsNullOrEmpty(xCustomerGroup))
                throw new Exception("- Please config customer group " + Member.CustomerGroup + " on table f1_longtail_pos_conf_member");

            DataTable dtSaleData = GetConfigMemberSaleData(p_sid, p_companycode, xCustomerGroup);
            if ((dtSaleData == null) || (dtSaleData.Rows.Count <= 0))
                throw new Exception("- Please config default customer sale data on table f1_longtail_pos_conf_member");

            DataRow drHeader = CustomerDataSet.master_customer.NewRow();
            drHeader["SID"] = p_sid;
            drHeader["CompanyCode"] = p_companycode;
            drHeader["CustomerCode"] = "";
            drHeader["CustomerGroup"] = xCustomerGroup;
            drHeader["CustomerName"] = Member.FirstName + " " + Member.LastName;
            drHeader["PriceList"] = Member.PriceList;
            drHeader["PriceGroup"] = dtSaleData.Rows[0]["PRICEGROUP"].ToString();
            drHeader["TitleCode"] = Member.TitleName == "" ? "001" : Member.TitleName;
            drHeader["Currency"] = "THB";
            drHeader["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            drHeader["CREATED_BY"] = Member.CREATED_BY;
            CustomerDataSet.master_customer.Rows.Add(drHeader);

            DataRow drGeneral = CustomerDataSet.master_customer_general.NewRow();
            drGeneral["SID"] = p_sid;
            drGeneral["CompanyCode"] = p_companycode;
            drGeneral["CustomerCode"] = "";
            drGeneral["TelNo1"] = Member.Phone;
            drGeneral["Mobile"] = Member.Mobile;
            drGeneral["EMail"] = Member.Mail;
            drGeneral["ActiveDateFrom"] = Member.ValidFrom;
            drGeneral["ActiveDateTo"] = "99991231";
            drGeneral["Active"] = "True";
            CustomerDataSet.master_customer_general.Rows.Add(drGeneral);

            foreach (DataRow drSale in dtSaleData.Rows)
            {
                DataRow drSaleData = CustomerDataSet.master_customer_sales_data.NewRow();
                drSaleData["SID"] = p_sid;
                drSaleData["CompanyCode"] = p_companycode;
                drSaleData["customerCode"] = "";

                string xLine = "01";
                if (CustomerDataSet.master_customer_sales_data.Rows.Count > 0)
                {
                    int xCount = Convert.ToInt32(CustomerDataSet.master_customer_sales_data.Compute("MAX(LineItem)", "")) + 1;
                    xLine = xCount.ToString().PadLeft(2, '0');
                }

                drSaleData["LineItem"] = xLine;
                drSaleData["SdistrictCode"] = drSale["SAREACODE"].ToString();
                drSaleData["Sorgcode"] = drSale["SORGCODE"].ToString();
                drSaleData["SDCode"] = drSale["SCHANNELCODE"].ToString();
                drSaleData["Sdivcode"] = drSale["SDIVCODE"].ToString();
                drSaleData["Sofficecode"] = "*";
                drSaleData["Sgroupcode"] = "*";
                drSaleData["CREATED_BY"] = Member.CREATED_BY;
                drSaleData["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
                CustomerDataSet.master_customer_sales_data.Rows.Add(drSaleData);
            }

            #region - accounting_general -
            DataRow drAcc_gen = CustomerDataSet.master_customer_accounting_general.NewRow();
            drAcc_gen["SID"] = p_sid;
            drAcc_gen["CompanyCode"] = p_companycode;
            drAcc_gen["customerCode"] = "";
            drAcc_gen["AccountReceivable"] = AccountReceivable;
            drAcc_gen["BlockDunningLetters"] = "False";
            drAcc_gen["CharOfAccount"] = CharOfAccount;
            drAcc_gen["CONSIGNMENTACCOUNT"] = "";
            drAcc_gen["DeliConsolidation"] = "False";
            drAcc_gen["DownPmtAcct"] = "";
            drAcc_gen["DunningDate"] = "";
            drAcc_gen["DunningLevel"] = "";
            drAcc_gen["PDConsolidation"] = "True";
            drAcc_gen["SpecialGLCode"] = "";
            drAcc_gen["ToleranceGroup"] = "";
            drAcc_gen["VendorCode"] = "";
            CustomerDataSet.master_customer_accounting_general.Rows.Add(drAcc_gen);
            #endregion

            ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

            Object[] objParam = new Object[] { agape.lib.constant.icmconstants.ICM_CONST_BP_CREATECUSTOMERMASTER, p_sessionid };
            DataSet[] objDataSet = new DataSet[] { CustomerDataSet };
            Object objReturn = ICMService.ICMPrimitiveInvoke(objParam, objDataSet);

            if (objReturn != null && objReturn.ToString() != "")
            {
                msg += objReturn.ToString() + "|";
                msg += "Create Customer : " + objReturn.ToString() + " Success.</br>";

                if (p_createcontact)
                {
                    msg += CopyCustomerToContact(p_sessionid, p_sid, p_companycode, Member.CREATED_BY, CustomerDataSet, Member, objReturn.ToString());
                }

                string sql = "select top(1) * from master_customer_pricelist";
                DataTable dtPriceList = dbService.selectDataFocusone(sql).Clone();
                dtPriceList.TableName = "master_customer_pricelist";
                dtPriceList.PrimaryKey = new DataColumn[] { dtPriceList.Columns["SID"], dtPriceList.Columns["CompanyCode"], dtPriceList.Columns["CustomerCode"],
                                                        dtPriceList.Columns["PriceList"], dtPriceList.Columns["PRICEGROUP"], dtPriceList.Columns["VALID_FROM"] };

                DataRow drPriceList = dtPriceList.NewRow();
                drPriceList["SID"] = p_sid;
                drPriceList["CompanyCode"] = p_companycode;
                drPriceList["CustomerCode"] = objReturn.ToString().Trim();
                drPriceList["MEMBERCARD_ID"] = Member.MemberCardID;
                drPriceList["PriceList"] = Member.PriceList;
                drPriceList["PRICEGROUP"] = dtSaleData.Rows[0]["PRICEGROUP"].ToString();
                drPriceList["BRANCHCODE"] = Member.Branch;
                drPriceList["VALID_FROM"] = Member.ValidFrom;
                drPriceList["VALID_TO"] = CalculateExpiredDateFromMonthInput(Member.ValidFrom, Member.ExpiredInMonth);
                drPriceList["ACTIVE"] = "True";
                drPriceList["CREATED_BY"] = Member.CREATED_BY;
                drPriceList["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
                drPriceList["UPDATED_BY"] = Member.CREATED_BY;
                drPriceList["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
                drPriceList["Remark"] = Member.Remark;
                drPriceList["TitleCode"] = Member.TitleName;
                drPriceList["FirstName"] = Member.FirstName;
                drPriceList["LastName"] = Member.LastName;
                drPriceList["Phone"] = Member.Phone;
                drPriceList["Mobile"] = Member.Mobile;
                drPriceList["Mail"] = Member.Mail;
                drPriceList["Birthday"] = Member.Birthday;
                dtPriceList.Rows.Add(drPriceList);

                dtPriceList = dbService.SaveTransactionForFocusone(dtPriceList);

                msg += "Create Member Card : " + Member.MemberCardID + " Success.";
            }
            else
                msg = "Create Unsuccess. !!!";

            return msg;
        }

        public DataTable GetConfigMemberSaleData(string p_sid, string p_companycode, string p_customergroup)
        {
            string sql = "select a.PRICEGROUP,b.* from f1_longtail_pos_conf_member  as a " +
                         "inner join sd_conf_define_sales_area as b on a.SID = b.SID and a.SAREACODE = b.SAREACODE " +
                         "where a.SID = '" + p_sid + "' and a.CompanyCode = '" + p_companycode + "'";

            if (!string.IsNullOrEmpty(p_customergroup))
            {
                sql += " and a.CUSTOMERGROUP = '" + p_customergroup + "'";
            }

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public string CopyCustomerToContact(string p_sessionid, string p_sid, string p_compaycode, string p_employeecode, dsVenCus CustomerDataSet, MemberEntity p_member, string p_customercode)
        {
            contactsdataset ContactDataSet = new contactsdataset();

            string msg = "";
            string sid = p_sid;
            string companycode = p_compaycode;
            string bptype = "C";
            string created_by = p_employeecode;
            string created_on = Validation.getCurrentServerStringDateTime();
            string bpcode = "", name1 = "", active = "", startdate = "",
                enddate = "", email = "", mobile = "";

            #region "Copy"
            if (CustomerDataSet.master_customer.Rows.Count > 0)
            {
                foreach (DataRow dr in CustomerDataSet.master_customer.Rows)
                {
                    bpcode = p_customercode.Trim();
                    name1 = Convert.ToString(dr["CustomerName"]);
                }
            }
            if (CustomerDataSet.master_customer_general.Rows.Count > 0)
            {
                foreach (DataRow dr in CustomerDataSet.master_customer_general.Rows)
                {
                    active = Convert.ToString(dr["Active"]);
                    startdate = Convert.ToString(dr["ActiveDateFrom"]);
                    enddate = Convert.ToString(dr["ActiveDateTo"]);
                    email = Convert.ToString(dr["EMail"]);
                    mobile = Convert.ToString(dr["Mobile"]);
                }
            }
            #endregion

            #region "Paste"
            DataRow drMaster = ContactDataSet.CONTACT_MASTER.NewRow();
            drMaster["SID"] = sid;
            drMaster["COMPANYCODE"] = companycode;
            drMaster["BPTYPE"] = bptype;
            drMaster["BPCODE"] = bpcode;
            drMaster["BPNAME1"] = name1;
            drMaster["BPNAME2"] = "";
            drMaster["AOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim();
            drMaster["CREATED_BY"] = created_by;
            drMaster["CREATED_ON"] = created_on;
            ContactDataSet.CONTACT_MASTER.Rows.Add(drMaster);

            DataRow drDetails = ContactDataSet.CONTACT_DETAILS.NewRow();
            drDetails["SID"] = sid;
            drDetails["AOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim();
            drDetails["ITEMNO"] = "001";
            drDetails["NAME1"] = name1;
            drDetails["NAME2"] = "";
            drDetails["REMARK1"] = "";
            drDetails["REMARK2"] = "";
            drDetails["PersonalID"] = "";
            drDetails["IMAGELINK"] = "";
            drDetails["ACTIVESTATUS"] = active;
            drDetails["STARTDATE"] = startdate;
            drDetails["ENDDATE"] = enddate;
            drDetails["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + "001";
            drDetails["CREATED_BY"] = created_by;
            drDetails["CREATED_ON"] = created_on;
            drDetails["MEMBERCARD_ID"] = p_member.MemberCardID;
            drDetails["BirthDay"] = p_member.Birthday;
            ContactDataSet.CONTACT_DETAILS.Rows.Add(drDetails);

            if (!string.IsNullOrEmpty(email))
            {
                DataRow drEmail = ContactDataSet.CONTACT_EMAIL.NewRow();
                drEmail["SID"] = sid;
                drEmail["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + "001";
                drEmail["ITEMNO"] = "001";
                drEmail["MAILTYPE"] = "E";
                drEmail["EMAIL"] = email;
                drEmail["CREATED_BY"] = created_by;
                drEmail["CREATED_ON"] = created_on;
                ContactDataSet.CONTACT_EMAIL.Rows.Add(drEmail);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                DataRow drMobile = ContactDataSet.CONTACT_PHONE.NewRow();
                drMobile["SID"] = sid;
                drMobile["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + "001";
                drMobile["ITEMNO"] = "001";
                drMobile["CONUNTRYCODE"] = "";
                drMobile["PHONETYPE"] = "M";
                drMobile["PHONENUMBER"] = mobile;
                drMobile["EXT"] = "";
                drMobile["REMARKS"] = "";
                drMobile["CREATED_BY"] = created_by;
                drMobile["CREATED_ON"] = created_on;
                ContactDataSet.CONTACT_PHONE.Rows.Add(drMobile);
            }
            #endregion

            if (ContactDataSet.CONTACT_MASTER.Rows.Count > 0)
            {
                ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

                Object[] objParam = new Object[] { "1Z00002", p_sessionid };
                DataSet[] objDataSet = new DataSet[] { ContactDataSet };
                DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
                if (objReturn != null)
                {
                    if (objReturn.Tables[0].Rows.Count > 0)
                    {
                        if (objReturn.Tables[0].Rows[0]["Message"].Equals("True"))
                        {
                            msg = "Copy Customer To Contact Success.</br>";
                        }
                        else
                        {
                            msg = "Copy Customer To Contact Unsuccess. !!!</br>";
                        }
                    }
                }
            }

            return msg;
        }

        public string CalculateExpiredDateFromMonthInput(string p_datefrom, int p_month)
        {
            DateTime _from = Validation.Convert2RadDateDisplay(p_datefrom);
            DateTime _expired = _from.AddMonths(p_month);

            return Validation.Convert2DateDB(_expired);
        }

        public string UpdateCustomerAndMember(string p_sessionid, string p_sid, string p_companycode, string p_customercode, string p_membercardid, MemberEntity p_member, bool p_updatecustomer)
        {
            string msg = "";

            dsVenCus CustomerDataSet = new dsVenCus();

            MemberEntity Member = p_member;

            ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

            Object[] objParam = new Object[] { agape.lib.constant.icmconstants.ICM_CONST_BP_GETCUSTOMERMASTER, p_sessionid, p_sid, p_companycode, p_customercode };
            DataSet[] objDataSet = new DataSet[] { CustomerDataSet };
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            if (objReturn != null)
            {
                CustomerDataSet = new dsVenCus();
                CustomerDataSet.Merge(objReturn);
            }

            if (p_updatecustomer)
            {
                if (objReturn != null)
                {

                    foreach (DataRow drHeader in CustomerDataSet.master_customer.Rows)
                    {
                        drHeader["CustomerName"] = Member.FirstName + " " + Member.LastName;
                        drHeader["TitleCode"] = Member.TitleName == "" ? "001" : Member.TitleName;
                        drHeader["UPDATED_BY"] = Member.CREATED_BY;
                        drHeader["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
                    }

                    foreach (DataRow drGeneral in CustomerDataSet.master_customer_general.Rows)
                    {
                        drGeneral["TelNo1"] = Member.Phone;
                        drGeneral["Mobile"] = Member.Mobile;
                        drGeneral["EMail"] = Member.Mail;
                    }
                   
                    Object[] objParam2 = new Object[] { agape.lib.constant.icmconstants.ICM_CONST_BP_CHANGECUSTOMERMASTER, p_sessionid };
                    DataSet[] objDataSet2 = new DataSet[] { CustomerDataSet };
                    Object objReturn2 = ICMService.ICMPrimitiveInvoke(objParam2, objDataSet2);

                    msg += "Update Customer :  " + CustomerDataSet.master_customer.Rows[0]["CustomerCode"].ToString() + " Success.</br>";
                }
            }


            //UPDATE CUSTOMER CONTACT
            msg += updateCustomerContact(p_sessionid, p_sid, p_companycode, p_member.CREATED_BY, p_customercode, p_membercardid, CustomerDataSet, p_member);

            DataTable dt = GetMemberCard(p_sid, p_companycode, p_customercode, Member.PriceList, Member.PriceGroup, Member.ValidFrom, Member.Branch);

            if (Member.MemberCardID == null || string.IsNullOrEmpty(Member.MemberCardID))
            {
                Member.MemberCardID = p_membercardid;
            }

            foreach (DataRow dr in dt.Rows)
            {
                dr["MEMBERCARD_ID"] = Member.MemberCardID;
                dr["TitleCode"] = Member.TitleName;
                dr["FirstName"] = Member.FirstName;
                dr["LastName"] = Member.LastName;
                dr["Phone"] = Member.Phone;
                dr["Mobile"] = Member.Mobile;
                dr["Mail"] = Member.Mail;
                dr["Remark"] = Member.Remark;
                dr["Birthday"] = Member.Birthday;

                dt = dbService.SaveTransactionForFocusone(dt);

                msg += "Update Member Card : " + dr["MEMBERCARD_ID"].ToString() + " Success.";
            }

            return msg;
        }

        private string updateCustomerContact(string p_sessionid, string p_sid, string p_companycode, string p_employeecode, string p_customercode,
                                             string p_member_card_id, dsVenCus CustomerDataSet, MemberEntity p_member)
        {
            string msg = "";
            string sid = p_sid;
            string companycode = p_companycode;
            string bptype = "C";
            string created_by = p_employeecode;
            string created_on = Validation.getCurrentServerStringDateTime();
            string bpcode = "", name1 = "", active = "", startdate = "",
                enddate = "", email = "", mobile = "";


            if (CustomerDataSet.master_customer.Rows.Count > 0)
            {
                foreach (DataRow dr in CustomerDataSet.master_customer.Rows)
                {
                    bpcode = p_customercode.Trim();
                    name1 = Convert.ToString(dr["CustomerName"]);
                }
            }
            if (CustomerDataSet.master_customer_general.Rows.Count > 0)
            {
                foreach (DataRow dr in CustomerDataSet.master_customer_general.Rows)
                {
                    active = Convert.ToString(dr["Active"]);
                    startdate = Convert.ToString(dr["ActiveDateFrom"]);
                    enddate = Convert.ToString(dr["ActiveDateTo"]);
                    email = Convert.ToString(dr["EMail"]);
                    mobile = Convert.ToString(dr["Mobile"]);
                }
            }

            ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

            contactsdataset ContactDataSet = new contactsdataset();
            Object[] contactOBJParam = new Object[] {
                    "CR00014",
                    p_sessionid,
                    p_companycode,
                    p_customercode};

            DataSet[] contactOBJDataset = new DataSet[] { ContactDataSet };
            DataSet contactOBJReturn = ICMService.ICMDataSetInvoke(contactOBJParam, contactOBJDataset);
            if (contactOBJReturn != null)
            {
                ContactDataSet = new contactsdataset();
                ContactDataSet.Merge(contactOBJReturn);
            }

            DataRow[] _contactDataRowFind = ContactDataSet.CONTACT_DETAILS.Select("MEMBERCARD_ID = '" + p_member_card_id + "'");

            if (_contactDataRowFind.Length > 0)
            {
                DataRow DRCONTACT_DETAILS = _contactDataRowFind[0];
                DRCONTACT_DETAILS["NAME1"] = p_member.FirstName + " " + p_member.LastName;
                DRCONTACT_DETAILS["ACTIVESTATUS"] = active;

                if (p_member.MemberCardID == null || string.IsNullOrEmpty(p_member.MemberCardID))
                {
                    DRCONTACT_DETAILS["MEMBERCARD_ID"] = p_member_card_id;
                }
                else
                {
                    DRCONTACT_DETAILS["MEMBERCARD_ID"] = p_member.MemberCardID;
                }

                //UPDATE EMAIL
                DataRow[] _emailDataRowFind = ContactDataSet.CONTACT_EMAIL.Select("BOBJECTLINK = '" + DRCONTACT_DETAILS["BOBJECTLINK"].ToString() + "'");

                if (_emailDataRowFind.Length > 0)
                {
                    DataRow DRCONTACT_EMAIL = _emailDataRowFind[0];
                    DRCONTACT_EMAIL["EMAIL"] = p_member.Mail;
                }
                else
                {
                    DataRow drEmail = ContactDataSet.CONTACT_EMAIL.NewRow();
                    drEmail["SID"] = sid;
                    drEmail["BOBJECTLINK"] = DRCONTACT_DETAILS["BOBJECTLINK"].ToString();
                    drEmail["ITEMNO"] = "001";
                    drEmail["MAILTYPE"] = "E";
                    drEmail["EMAIL"] = p_member.Mail;
                    drEmail["CREATED_BY"] = created_by;
                    drEmail["CREATED_ON"] = created_on;
                    ContactDataSet.CONTACT_EMAIL.Rows.Add(drEmail);
                }

                //UPDATE PHONE
                DataRow[] _phoneDataRowFind = ContactDataSet.CONTACT_PHONE.Select("BOBJECTLINK = '" + DRCONTACT_DETAILS["BOBJECTLINK"].ToString() + "'");
                if (_phoneDataRowFind.Length > 0)
                {
                    DataRow DRCONTACT_PHONE = _phoneDataRowFind[0];
                    DRCONTACT_PHONE["PHONENUMBER"] = p_member.Phone;
                }
                else
                {
                    DataRow drMobile = ContactDataSet.CONTACT_PHONE.NewRow();
                    drMobile["SID"] = sid;
                    drMobile["BOBJECTLINK"] = DRCONTACT_DETAILS["BOBJECTLINK"].ToString();
                    drMobile["ITEMNO"] = "001";
                    drMobile["CONUNTRYCODE"] = "";
                    drMobile["PHONETYPE"] = "M";
                    drMobile["PHONENUMBER"] = p_member.Mobile;
                    drMobile["EXT"] = "";
                    drMobile["REMARKS"] = "";
                    drMobile["CREATED_BY"] = created_by;
                    drMobile["CREATED_ON"] = created_on;
                    ContactDataSet.CONTACT_PHONE.Rows.Add(drMobile);
                }
            }
            else
            {
                #region "Copy"
                if (CustomerDataSet.master_customer.Rows.Count > 0)
                {
                    foreach (DataRow dr in CustomerDataSet.master_customer.Rows)
                    {
                        bpcode = p_customercode.Trim();
                        name1 = Convert.ToString(dr["CustomerName"]);
                    }
                }
                if (CustomerDataSet.master_customer_general.Rows.Count > 0)
                {
                    foreach (DataRow dr in CustomerDataSet.master_customer_general.Rows)
                    {
                        active = Convert.ToString(dr["Active"]);
                        startdate = Convert.ToString(dr["ActiveDateFrom"]);
                        enddate = Convert.ToString(dr["ActiveDateTo"]);
                        email = Convert.ToString(dr["EMail"]);
                        mobile = Convert.ToString(dr["Mobile"]);
                    }
                }
                #endregion
                #region "Paste"
                if (ContactDataSet.CONTACT_MASTER.Rows.Count <= 0)
                {
                    DataRow drMaster = ContactDataSet.CONTACT_MASTER.NewRow();
                    drMaster["SID"] = sid;
                    drMaster["COMPANYCODE"] = companycode;
                    drMaster["BPTYPE"] = bptype;
                    drMaster["BPCODE"] = bpcode;
                    drMaster["BPNAME1"] = name1;
                    drMaster["BPNAME2"] = "";
                    drMaster["AOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim();
                    drMaster["CREATED_BY"] = created_by;
                    drMaster["CREATED_ON"] = created_on;
                    ContactDataSet.CONTACT_MASTER.Rows.Add(drMaster);
                }

                string itemno = "001";

                if (ContactDataSet.CONTACT_DETAILS.Rows.Count > 0)
                {
                    int max = Convert.ToInt16(ContactDataSet.CONTACT_DETAILS.Compute("MAX(ITEMNO)", "AOBJECTLINK = '" + sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + "'")) + 1;

                    itemno = max.ToString().PadLeft(3, '0');
                }

                DataRow drDetails = ContactDataSet.CONTACT_DETAILS.NewRow();
                drDetails["SID"] = sid;
                drDetails["AOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim();
                drDetails["ITEMNO"] = itemno;
                drDetails["NAME1"] = name1;
                drDetails["NAME2"] = "";
                drDetails["REMARK1"] = "";
                drDetails["REMARK2"] = "";
                drDetails["PersonalID"] = "";
                drDetails["IMAGELINK"] = "";
                drDetails["ACTIVESTATUS"] = active;
                drDetails["STARTDATE"] = startdate;
                drDetails["ENDDATE"] = enddate;
                drDetails["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + itemno;
                drDetails["CREATED_BY"] = created_by;
                drDetails["CREATED_ON"] = created_on;

                if (string.IsNullOrEmpty(p_member.MemberCardID))
                {
                    drDetails["MEMBERCARD_ID"] = p_member_card_id;

                }
                else
                {
                    drDetails["MEMBERCARD_ID"] = p_member.MemberCardID;
                }


                ContactDataSet.CONTACT_DETAILS.Rows.Add(drDetails);
                if (!string.IsNullOrEmpty(email))
                {
                    DataRow drEmail = ContactDataSet.CONTACT_EMAIL.NewRow();
                    drEmail["SID"] = sid;
                    drEmail["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + itemno;
                    drEmail["ITEMNO"] = "001";
                    drEmail["MAILTYPE"] = "E";
                    drEmail["EMAIL"] = email;
                    drEmail["CREATED_BY"] = created_by;
                    drEmail["CREATED_ON"] = created_on;
                    ContactDataSet.CONTACT_EMAIL.Rows.Add(drEmail);
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    DataRow drMobile = ContactDataSet.CONTACT_PHONE.NewRow();
                    drMobile["SID"] = sid;
                    drMobile["BOBJECTLINK"] = sid.Trim() + companycode.Trim() + bptype.Trim() + bpcode.Trim() + itemno;
                    drMobile["ITEMNO"] = "001";
                    drMobile["CONUNTRYCODE"] = "";
                    drMobile["PHONETYPE"] = "M";
                    drMobile["PHONENUMBER"] = mobile;
                    drMobile["EXT"] = "";
                    drMobile["REMARKS"] = "";
                    drMobile["CREATED_BY"] = created_by;
                    drMobile["CREATED_ON"] = created_on;
                    ContactDataSet.CONTACT_PHONE.Rows.Add(drMobile);
                }
                #endregion
            }


            if (ContactDataSet.CONTACT_MASTER.Rows.Count > 0)
            {
                Object[] objParam = new Object[] { "1Z00002", p_sessionid };
                DataSet[] objDataSet = new DataSet[] { ContactDataSet };
                DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
                if (objReturn != null)
                {
                    if (objReturn.Tables[0].Rows.Count > 0)
                    {
                        if (objReturn.Tables[0].Rows[0]["Message"].Equals("True"))
                        {
                            msg = "Copy Customer To Contact Success.</br>";
                        }
                        else
                        {
                            msg = "Copy Customer To Contact Unsuccess. !!!</br>";
                        }
                    }
                }
            }

            return msg;
        }

        public DataTable GetMemberCard(string p_sid, string p_companycode, string p_customercode,
                                       string p_pricelist, string p_pricegroup, string p_validfrom, string p_branch)
        {
            string sql = "select * from master_customer_pricelist " +
                         "where SID='" + p_sid + "' and CompanyCode='" + p_companycode + "' and CustomerCode='" + p_customercode + "' " +
                         "and VALID_FROM='" + p_validfrom + "' and PriceList='" + p_pricelist + "' and PriceGroup='" + p_pricegroup + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            dt.TableName = "master_customer_pricelist";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["CompanyCode"], dt.Columns["CustomerCode"],
                                               dt.Columns["PriceList"], dt.Columns["PRICEGROUP"], dt.Columns["VALID_FROM"] };

            return dt;
        }

        public string CancelMember(string p_sid, string p_companycode, string p_customercode, string p_pricelist,
                                   string p_pricegroup, string p_membercard_id, string p_validfrom)
        {
            string sql = "select * from master_customer_pricelist where SID='" + p_sid + "' and CompanyCode='" + p_companycode + "' " +
                         "and CustomerCode='" + p_customercode + "' and PriceList='" + p_pricelist + "' and PRICEGROUP='" + p_pricegroup + "' " +
                         "and MEMBERCARD_ID='" + p_membercard_id + "' and VALID_FROM='" + p_validfrom + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            dt.TableName = "master_customer_pricelist";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["CompanyCode"], dt.Columns["CustomerCode"],
                                               dt.Columns["PriceList"], dt.Columns["PRICEGROUP"], dt.Columns["VALID_FROM"] };

            string msg = "";

            if (dt.Rows.Count > 0)
            {
                string validto = "";

                foreach (DataRow dr in dt.Rows)
                {
                    dr["ACTIVE"] = "False";

                    validto = Validation.Convert2DateDisplay(dr["VALID_TO"].ToString());
                }

                dbService.SaveTransactionForFocusone(dt);

                msg = "Cancel Member Card : " + p_membercard_id + "<br />" +
                      "Member Card Type : " + GetPriceListDescription(p_sid, p_pricelist) + " - " + GetPriceGroupDescription(p_sid, p_pricegroup) + "<br />" +
                      "Valid From : " + Validation.Convert2DateDisplay(p_validfrom) + "<br />" +
                      "Valid To : " + validto + " Success.";
            }
            else
            {
                msg = "Cancel Member Card : " + p_membercard_id + "<br />" +
                      "Member Card Type : " + GetPriceListDescription(p_sid, p_pricelist) + " - " + GetPriceGroupDescription(p_sid, p_pricegroup) + "<br />" +
                      "Valid From : " + Validation.Convert2DateDisplay(p_validfrom) + " Unsuccess.";
            }

            return msg;
        }

        public string GetPriceListDescription(string p_sid, string p_pricelistcode)
        {
            string sql = "select * from master_conf_pricelist where SID='" + p_sid + "' and PriceListCode='" + p_pricelistcode + "'";

            string desc = "";

            DataTable dt = dbService.selectDataFocusone(sql);

            foreach (DataRow dr in dt.Rows)
            {
                desc = dr["Description"].ToString();
            }

            return desc;
        }

        public string GetPriceGroupDescription(string p_sid, string p_pricegroupcode)
        {
            string sql = "select * from MASTER_CONFIG_PRICEGROUP where SID='" + p_sid + "' and PriceGroupCode='" + p_pricegroupcode + "'";

            string desc = "";

            DataTable dt = dbService.selectDataFocusone(sql);

            foreach (DataRow dr in dt.Rows)
            {
                desc = dr["PriceGroupDescription"].ToString();
            }

            return desc;
        }

        public DataTable getNewMemberReport(string p_sid, string p_companycode, string p_createdfrom, string p_createdto, string p_active, string p_pricelist, string p_branch)
        {
            string wherePriceList = "";

            if (!string.IsNullOrEmpty(p_pricelist))
            {
                wherePriceList = " and a.Sdivcode='" + p_pricelist + "' ";
            }

            DataTable dt = new DataTable();
            StringBuilder str = new StringBuilder();
            str.AppendLine(" select distinct cus.CustomerCode,cus.CustomerName,cus.CustomerNameTH,cus.ForeignName ");
            str.AppendLine(" , a.xValue as building, b.xValue as roomno, c.xValue as floorno, d.xValue as village, ");
            str.AppendLine(" e.xValue as company,  f.xValue as houseno,  g.xValue as moo,  h.xValue as soi, ");
            str.AppendLine(" i.xValue as street,  j.xValue as tumbon,  k.xValue as amphur,  l.xValue as province, ");
            str.AppendLine(" m.xValue as postcode,  n.xValue as country,cus.TitleCode,title.TitleName ");
            str.AppendLine(" ,case when P.Birthday='' then contd.BirthDay else P.Birthday end as Birthday ");
            str.AppendLine(" ,P.Mail,cusgen.TelNo1 as Phone,cusgen.Mobile,P.VALID_FROM,P.VALID_TO,P.MEMBERCARD_ID,P.PriceList,P.Remark ");
            str.AppendLine(" ,pl.Description as PricelistDesc,P.BranchCode,branch.BRANCHNAME_TH,P.ACTIVE,ISNULL(sot.TotalInMonth,0) as TotalPay ");
            str.AppendLine(" from [dbo].[master_customer] cus ");
            str.AppendLine(" inner join master_customer_pricelist P ");
            str.AppendLine(" on P.SID = cus.SID and P.CompanyCode = cus.Companycode and P.CustomerCode = cus.CustomerCode ");
            str.AppendLine(" left join master_customer_general cusgen");
            str.AppendLine(" on cus.SID = cusgen.SID and cus.CompanyCode = cusgen.CompanyCode and cus.CustomerCode = cusgen.CustomerCode");
            str.AppendLine(" left join CONTACT_MASTER cont on cus.SID = cont.SID and cus.CompanyCode = cont.COMPANYCODE and cus.CustomerCode = cont.BPCODE ");
            str.AppendLine(" left join CONTACT_DETAILS contd on cont.SID = contd.SID and cont.AOBJECTLINK = contd.AOBJECTLINK ");
            str.AppendLine(" left join master_titlename as title on cus.SID = title.SID and cus.TitleCode = title.TitleCode ");
            str.AppendLine(" left join dbo.master_conf_pricelist pl ");
            str.AppendLine(" on P.SID = pl.SID and P.PriceList = pl.PriceListCode ");
            str.AppendLine(" left join dbo.BRANCH_MASTER_GENERAL branch ");
            str.AppendLine(" on P.SID = branch.SID and P.CompanyCode = branch.COMPANYCODE and P.BranchCode = branch.BRANCHCODE ");
            str.AppendLine(" left join [dbo].[master_customer_address] a ");
            str.AppendLine(" on cus.SID=a.SID and cus.customercode = a.customercode and a.PropertiesCode='01' ");
            str.AppendLine(" left join [dbo].[master_customer_address] b ");
            str.AppendLine(" on cus.SID=b.SID and cus.customercode = b.customercode and b.PropertiesCode='02' ");
            str.AppendLine(" left join [dbo].[master_customer_address] c ");
            str.AppendLine(" on cus.SID=c.SID and cus.customercode = c.customercode and c.PropertiesCode='03' ");
            str.AppendLine(" left join [dbo].[master_customer_address] d ");
            str.AppendLine(" on cus.SID=d.SID and cus.customercode = d.customercode and d.PropertiesCode='04' ");
            str.AppendLine(" left join [dbo].[master_customer_address] e ");
            str.AppendLine(" on cus.SID=e.SID and cus.customercode = e.customercode and e.PropertiesCode='05' ");
            str.AppendLine(" left join [dbo].[master_customer_address] f ");
            str.AppendLine(" on cus.SID=f.SID and cus.customercode = f.customercode and f.PropertiesCode='06' ");
            str.AppendLine(" left join [dbo].[master_customer_address] g ");
            str.AppendLine(" on cus.SID=g.SID and cus.customercode = g.customercode and g.PropertiesCode='07' ");
            str.AppendLine(" left join [dbo].[master_customer_address] h ");
            str.AppendLine(" on cus.SID=h.SID and cus.customercode = h.customercode and h.PropertiesCode='08' ");
            str.AppendLine(" left join [dbo].[master_customer_address] i ");
            str.AppendLine(" on cus.SID=i.SID and cus.customercode = i.customercode and i.PropertiesCode='09' ");
            str.AppendLine(" left join [dbo].[master_customer_address] j ");
            str.AppendLine(" on cus.SID=j.SID and cus.customercode = j.customercode and j.PropertiesCode='10' ");
            str.AppendLine(" left join [dbo].[master_customer_address] k ");
            str.AppendLine(" on cus.SID=k.SID and cus.customercode = k.customercode and k.PropertiesCode='11' ");
            str.AppendLine(" left join [dbo].[master_customer_address] l ");
            str.AppendLine(" on cus.SID=l.SID and cus.customercode = l.customercode and l.PropertiesCode='12' ");
            str.AppendLine(" left join [dbo].[master_customer_address] m ");
            str.AppendLine(" on cus.SID=m.SID and cus.customercode = m.customercode and m.PropertiesCode='13' ");
            str.AppendLine(" left join [dbo].[master_customer_address] n ");
            str.AppendLine(" on cus.SID=n.SID and cus.customercode = n.customercode and n.PropertiesCode='14' ");
            str.AppendLine(" left join (select ISNULL(SUM(a.NetValue * case when dob.PostingType='SR' then -1 else 1 end),0) as TotalInMonth");
            str.AppendLine(" ,a.SoldToParty,a.companyCode,a.SID");
            str.AppendLine(" from sd_so_header a left outer join master_config_lo_doctype_docdetail dob WITH(NOLOCK) on a.Stypecode=dob.DocumentTypeCode and a.SID=dob.SID");
            str.AppendLine(" left join dbo.sys_so_doctype_mapping_auto_gibl sys_so on a.SID = sys_so.SID and a.companycode = sys_so.COMPANYCODE");
            str.AppendLine(" and (a.Stypecode=sys_so.SODOCTYPE  or a.Stypecode=sys_so.SR_DOCTYPE) where a.Status = '07' " + wherePriceList + " group by a.SoldToParty,a.companyCode,a.SID) as sot");
            str.AppendLine(" on cus.SID = sot.SID and cus.CompanyCode = sot.companyCode and cus.CustomerCode = sot.SoldToParty");
            str.AppendLine(" where cus.sid ='" + p_sid + "' and cus.CustomerGroup <> 'C05'");
            str.AppendLine(Validation.CreateString(p_companycode, "cus.CompanyCode"));
            str.AppendLine(Validation.CreateString(p_active, "P.ACTIVE"));
            str.AppendLine(Validation.CreateString(p_pricelist, "P.PriceList"));
            str.AppendLine(Validation.CreateString(p_branch, "P.BranchCode"));


            if (!string.IsNullOrEmpty(p_createdfrom) && string.IsNullOrEmpty(p_createdto))
                str.AppendLine("and P.VALID_FROM like '" + p_createdfrom + "%'");
            else if (!string.IsNullOrEmpty(p_createdfrom) && !string.IsNullOrEmpty(p_createdto))
                str.AppendLine("and P.VALID_FROM >= '" + p_createdfrom + "01' and P.VALID_FROM <= '" + p_createdto + "31'");

            str.AppendLine("order by P.ACTIVE desc");

            dt = dbService.selectDataFocusone(str.ToString());

            dt.Columns.Add("age", typeof(System.String)).DefaultValue = "";


            if (p_active.ToUpper() == "FALSE")
            {
                string sqlMemberActive = "select CustomerCode from master_customer_pricelist " +
                                         "where SID='" + p_sid + "' and CompanyCode='" + p_companycode + "' and ACTIVE='True'";

                if (!string.IsNullOrEmpty(p_pricelist))
                {
                    sqlMemberActive += " AND PriceList='" + p_pricelist + "'";
                }

                DataTable dtMemberActive = dbService.selectDataFocusone(sqlMemberActive);

                DataTable dtResult = dt.Clone();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dtMemberActive.Select("CustomerCode='" + dr["CustomerCode"] + "'").Length > 0)
                    {
                        continue;
                    }

                    dtResult.ImportRow(dr);
                }

                return dtResult;
            }
            
            return dt;
        }

        #region Report customer
        public DataTable getReportCustomer(string sid, string companycode, string customercode, string customername, string telephone
           , string mobile, string email,String active)
        {
            string sql = @"select 
                            D.*,A.*,B.TelNo1,B.Mobile,B.EMail,B.Active,B.ActiveDateFrom,B.ActiveDateTo,C.TitleName 
                           ,CASE B.Active WHEN 'true' THEN 'ใช้งาน' ELSE 'ยกเลิกการใช้งาน'END as statusActive 
                           ,F.BRANCHNAME_TH, ISNULL(contd.Birthday, '') as ContactBirthday 
                           ,isnull(D.Birthday,'') as BirthdayUse
                           from master_customer_pricelist D 
                           left join master_customer A  
                           on D.SID = A.SID and D.CompanyCode = A.CompanyCode and A.CustomerCode = D.CustomerCode 
                           left join dbo.master_customer_general B 
                           on A.SID = B.SID and A.CompanyCode = B.CompanyCode and A.CustomerCode = B.CustomerCode 
                           left join master_titlename C on A.SID = C.SID and A.TitleCode = C.TitleCode 
                           left join BRANCH_MASTER_GENERAL  F on D.SID = F.SID and D.CompanyCode = F.COMPANYCODE and D.BranchCode=F.BRANCHCODE 
                           left join CONTACT_MASTER cont on A.SID = cont.SID and A.CompanyCode = cont.COMPANYCODE and A.CustomerCode = cont.BPCODE 
                           left join CONTACT_DETAILS contd on cont.SID = contd.SID and cont.AOBJECTLINK = contd.AOBJECTLINK 
                                                              and D.MEMBERCARD_ID = contd.MEMBERCARD_ID
               where A.SID='" + sid + "' and A.CompanyCode='" + companycode + "' ";
            if (!string.IsNullOrEmpty(customercode))
            {
                sql += " and D.CustomerCode like'%" + customercode + "%'";
            }
            if (!string.IsNullOrEmpty(customername))
            {
                sql += " and A.CustomerName like'%" + customername + "%'";
            }

            if (!string.IsNullOrEmpty(telephone))
            {
                sql += " and B.TelNo1 like'%" + telephone + "%'";
            }

            if (!string.IsNullOrEmpty(mobile))
            {
                sql += " and B.Mobile like'%" + mobile + "%'";
            }

            if (!string.IsNullOrEmpty(email))
            {
                sql += " and B.EMail like'%" + email + "%'";
            }
            if(!String.IsNullOrEmpty(active))
            {
                sql += " and B.Active='"+active+@"' ";
            }
            
            sql += " order by A.CustomerCode ";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable getAddressCustomer(string sid, string companycode, string customercode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine(" select a.CustomerCode,b.Description as code,a.xValue as description,c.xValue as description2   ");
            str.AppendLine(" from dbo.master_customer_address a    ");
            str.AppendLine(" left join dbo.master_conf_properties b   ");
            str.AppendLine(" on a.SID = b.SID and a.PropertiesCode = b.PropertiesCode and xType='ADDRESS'   ");
            str.AppendLine(" left join dbo.master_conf_selectedvalue_detail c    ");
            str.AppendLine(" on b.SID = c.SID and b.SelectedCode = c.Code and a.xValue = c.DetailCode   ");

            str.AppendLine("  where a.SID='" + sid + "'  ");
            if (!string.IsNullOrEmpty(companycode))
                str.AppendLine(" and a.CompanyCode ='" + companycode + "'");
            if (!string.IsNullOrEmpty(customercode))
                str.AppendLine(" and a.CustomerCode ='" + customercode + "'");

           return dbService.selectDataFocusone(str.ToString());
        }
        #endregion

        public DataTable getMemberExpireReport(string p_sid, string p_companycode, string p_createdon, string p_active, string p_year
            , string p_pricelist, string p_branchcode, string p_searchlike)
        {
            if (string.IsNullOrEmpty(p_year))
            {
                p_year = Validation.getCurrentServerStringDateTime().Substring(0, 4);
            }

            string twoyearago = Convert.ToString(Convert.ToDecimal(p_year) - 2);
            string lastyear = Convert.ToString(Convert.ToDecimal(p_year) - 1);

            string wherePriceList = "";

            if (!string.IsNullOrEmpty(p_pricelist))
            {
                wherePriceList = " and a.Sdivcode='" + p_pricelist + "' ";
            }

            DataTable dt = new DataTable();
            StringBuilder str = new StringBuilder();
            str.AppendLine(" select cus.CustomerCode,cus.CustomerName,cus.CustomerNameTH,cus.ForeignName ");
            str.AppendLine(" , a.xValue as building, b.xValue as roomno, c.xValue as floorno, d.xValue as village, ");
            str.AppendLine(" e.xValue as company,  f.xValue as houseno,  g.xValue as moo,  h.xValue as soi, ");
            str.AppendLine(" i.xValue as street,  j.xValue as tumbon,  k.xValue as amphur,  l.xValue as province, ");
            str.AppendLine(" m.xValue as postcode,  n.xValue as country,cus.TitleCode,title.TitleName ");
            str.AppendLine(" ,P.Birthday,P.Mail,P.Phone,P.Mobile,P.VALID_FROM,P.VALID_TO,P.MEMBERCARD_ID,P.PriceList,P.PriceGroup,P.BranchCode,P.Remark ");
            str.AppendLine(" ,pl.Description as PricelistDesc,P.BranchCode,branch.BRANCHNAME_TH,P.ACTIVE, ");
            str.AppendLine(" ISNULL(sot.TotalInMonth,0) as TotalBeforeLastYear,ISNULL(sot2.TotalInMonth,0) as TotalLastYear,");
            str.AppendLine(" ISNULL(sot3.TotalInMonth,0) as TotalThisYear");
            str.AppendLine(" from [dbo].[master_customer] cus ");
            str.AppendLine(" inner join master_customer_pricelist P ");
            str.AppendLine(" on P.SID = cus.SID and P.CompanyCode = cus.Companycode and P.CustomerCode = cus.CustomerCode ");
            str.AppendLine(" left join master_titlename as title on cus.SID = title.SID and cus.TitleCode = title.TitleCode ");
            str.AppendLine(" left join dbo.master_conf_pricelist pl ");
            str.AppendLine(" on P.SID = pl.SID and P.PriceList = pl.PriceListCode ");
            str.AppendLine(" left join dbo.BRANCH_MASTER_GENERAL branch ");
            str.AppendLine(" on P.SID = branch.SID and P.CompanyCode = branch.COMPANYCODE and P.BranchCode = branch.BRANCHCODE ");
            str.AppendLine(" left join [dbo].[master_customer_address] a ");
            str.AppendLine(" on cus.SID=a.SID and cus.customercode = a.customercode and a.PropertiesCode='01' ");
            str.AppendLine(" left join [dbo].[master_customer_address] b ");
            str.AppendLine(" on cus.SID=b.SID and cus.customercode = b.customercode and b.PropertiesCode='02' ");
            str.AppendLine(" left join [dbo].[master_customer_address] c ");
            str.AppendLine(" on cus.SID=c.SID and cus.customercode = c.customercode and c.PropertiesCode='03' ");
            str.AppendLine(" left join [dbo].[master_customer_address] d ");
            str.AppendLine(" on cus.SID=d.SID and cus.customercode = d.customercode and d.PropertiesCode='04' ");
            str.AppendLine(" left join [dbo].[master_customer_address] e ");
            str.AppendLine(" on cus.SID=e.SID and cus.customercode = e.customercode and e.PropertiesCode='05' ");
            str.AppendLine(" left join [dbo].[master_customer_address] f ");
            str.AppendLine(" on cus.SID=f.SID and cus.customercode = f.customercode and f.PropertiesCode='06' ");
            str.AppendLine(" left join [dbo].[master_customer_address] g ");
            str.AppendLine(" on cus.SID=g.SID and cus.customercode = g.customercode and g.PropertiesCode='07' ");
            str.AppendLine(" left join [dbo].[master_customer_address] h ");
            str.AppendLine(" on cus.SID=h.SID and cus.customercode = h.customercode and h.PropertiesCode='08' ");
            str.AppendLine(" left join [dbo].[master_customer_address] i ");
            str.AppendLine(" on cus.SID=i.SID and cus.customercode = i.customercode and i.PropertiesCode='09' ");
            str.AppendLine(" left join [dbo].[master_customer_address] j ");
            str.AppendLine(" on cus.SID=j.SID and cus.customercode = j.customercode and j.PropertiesCode='10' ");
            str.AppendLine(" left join [dbo].[master_customer_address] k ");
            str.AppendLine(" on cus.SID=k.SID and cus.customercode = k.customercode and k.PropertiesCode='11' ");
            str.AppendLine(" left join [dbo].[master_customer_address] l ");
            str.AppendLine(" on cus.SID=l.SID and cus.customercode = l.customercode and l.PropertiesCode='12' ");
            str.AppendLine(" left join [dbo].[master_customer_address] m ");
            str.AppendLine(" on cus.SID=m.SID and cus.customercode = m.customercode and m.PropertiesCode='13' ");
            str.AppendLine(" left join [dbo].[master_customer_address] n ");
            str.AppendLine(" on cus.SID=n.SID and cus.customercode = n.customercode and n.PropertiesCode='14' ");
            str.AppendLine(" left join (select ISNULL(SUM(a.NetValue * case when dob.PostingType='SR' then -1 else 1 end),0) as TotalInMonth");
            str.AppendLine(" ,a.SoldToParty,a.companyCode,a.SID");
            str.AppendLine(" from sd_so_header a left outer join master_config_lo_doctype_docdetail dob WITH(NOLOCK) on a.Stypecode=dob.DocumentTypeCode and a.SID=dob.SID");
            str.AppendLine(" left join dbo.sys_so_doctype_mapping_auto_gibl sys_so on a.SID = sys_so.SID and a.companycode = sys_so.COMPANYCODE");
            str.AppendLine(" and (a.Stypecode=sys_so.SODOCTYPE  or a.Stypecode=sys_so.SR_DOCTYPE) where a.Status = '07' and");
            str.AppendLine(" a.DocDate >= '" + twoyearago + "0101' and a.DocDate <= '" + twoyearago + "1231' " + wherePriceList + " group by a.SoldToParty,a.companyCode,a.SID) as sot");
            str.AppendLine(" on cus.SID = sot.SID and cus.CompanyCode = sot.companyCode and cus.CustomerCode = sot.SoldToParty");
            str.AppendLine(" left join (select ISNULL(SUM(a.NetValue * case when dob.PostingType='SR' then -1 else 1 end),0) as TotalInMonth");
            str.AppendLine(" ,a.SoldToParty,a.companyCode,a.SID");
            str.AppendLine(" from sd_so_header a left outer join master_config_lo_doctype_docdetail dob WITH(NOLOCK) on a.Stypecode=dob.DocumentTypeCode and a.SID=dob.SID");
            str.AppendLine(" left join dbo.sys_so_doctype_mapping_auto_gibl sys_so on a.SID = sys_so.SID and a.companycode = sys_so.COMPANYCODE");
            str.AppendLine(" and (a.Stypecode=sys_so.SODOCTYPE  or a.Stypecode=sys_so.SR_DOCTYPE) where a.Status = '07' and");
            str.AppendLine(" a.DocDate >= '" + lastyear + "0101' and a.DocDate <= '" + lastyear + "1231' " + wherePriceList + " group by a.SoldToParty,a.companyCode,a.SID) as sot2");
            str.AppendLine(" on cus.SID = sot2.SID and cus.CompanyCode = sot2.companyCode and cus.CustomerCode = sot2.SoldToParty");
            str.AppendLine(" left join (select ISNULL(SUM(a.NetValue * case when dob.PostingType='SR' then -1 else 1 end),0) as TotalInMonth");
            str.AppendLine(" ,a.SoldToParty,a.companyCode,a.SID");
            str.AppendLine(" from sd_so_header a left outer join master_config_lo_doctype_docdetail dob WITH(NOLOCK) on a.Stypecode=dob.DocumentTypeCode and a.SID=dob.SID");
            str.AppendLine(" left join dbo.sys_so_doctype_mapping_auto_gibl sys_so on a.SID = sys_so.SID and a.companycode = sys_so.COMPANYCODE");
            str.AppendLine(" and (a.Stypecode=sys_so.SODOCTYPE  or a.Stypecode=sys_so.SR_DOCTYPE) where a.Status = '07' and");
            str.AppendLine(" a.DocDate >= '" + p_year + "0101' and a.DocDate <= '" + p_year + "1231' " + wherePriceList + " group by a.SoldToParty,a.companyCode,a.SID) as sot3");
            str.AppendLine(" on cus.SID = sot3.SID and cus.CompanyCode = sot3.companyCode and cus.CustomerCode = sot3.SoldToParty");
            str.AppendLine(" where cus.sid ='" + p_sid + "' and cus.CustomerGroup <> 'C05'");
            str.AppendLine(Validation.CreateString(p_companycode, "cus.CompanyCode"));
            str.AppendLine(Validation.CreateString(p_active, "P.ACTIVE"));

            if (!string.IsNullOrEmpty(p_pricelist))
            {
                str.AppendLine("and P.PriceList = '" + p_pricelist + "'");
            }
            if (!string.IsNullOrEmpty(p_searchlike))
            {
                str.AppendLine("and (");
                str.AppendLine("cus.CustomerCode LIKE '%" + p_searchlike + "%' OR");
                str.AppendLine("cus.CustomerName LIKE '%" + p_searchlike + "%' OR");
                str.AppendLine("cus.ForeignName LIKE '%" + p_searchlike + "%' OR");
                str.AppendLine("P.MEMBERCARD_ID LIKE '%" + p_searchlike + "%')");
            }

            string[] branchArr = p_branchcode.Split(';');
            List<string> branchPrepared = new List<string>();
            foreach (string branchEle in branchArr)
            {
                if (branchEle != null && !string.IsNullOrEmpty(branchEle))
                {
                    branchPrepared.Add("'" + branchEle + "'");
                }
            }
            if (branchPrepared.Count > 0)
            {
                str.AppendLine(" and P.BranchCode in (" + string.Join(",", branchPrepared.ToArray()) + ")");
            }
            if (!string.IsNullOrEmpty(p_createdon))
            {
                str.AppendLine("and P.VALID_TO like '" + p_createdon + "%'");
            }


            dt = dbService.selectDataFocusone(str.ToString());

            dt.Columns.Add("age", typeof(System.String)).DefaultValue = "";

            if (p_active.ToUpper() == "FALSE")
            {
                string sqlMemberActive = "select CustomerCode from master_customer_pricelist " +
                                         "where SID='" + p_sid + "' and CompanyCode='" + p_companycode + "' and ACTIVE='True'";

                if (!string.IsNullOrEmpty(p_pricelist))
                {
                    sqlMemberActive += " AND PriceList='" + p_pricelist + "'";
                }

                DataTable dtMemberActive = dbService.selectDataFocusone((sqlMemberActive));

                DataTable dtResult = dt.Clone();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dtMemberActive.Select("CustomerCode='" + dr["CustomerCode"] + "'").Length > 0)
                    {
                        continue;
                    }

                    dtResult.ImportRow(dr);
                }

                return dtResult;
            }

            return dt;
        }

        public DataTable getBirthDayMemberReport(string p_sid, string p_companycode, string p_month, string p_tomonth, string p_active, string p_pricelist)
        {
            DataTable dt = new DataTable();
            StringBuilder str = new StringBuilder();
            str.AppendLine(" select cus.CustomerCode,cus.CustomerName,cus.CustomerNameTH,cus.ForeignName ");
            str.AppendLine(" , a.xValue as building, b.xValue as roomno, c.xValue as floorno, d.xValue as village, ");
            str.AppendLine(" e.xValue as company,  f.xValue as houseno,  g.xValue as moo,  h.xValue as soi, ");
            str.AppendLine(" i.xValue as street,  j.xValue as tumbon,  k.xValue as amphur,  l.xValue as province, ");
            str.AppendLine(" m.xValue as postcode,  n.xValue as country,cus.TitleCode,title.TitleName ");
            str.AppendLine(" ,P.Birthday,P.Mail,P.Phone,P.Mobile,P.VALID_FROM,P.VALID_TO,P.MEMBERCARD_ID,P.PriceList,P.BranchCode,P.Remark ");
            str.AppendLine(" ,pl.Description as PricelistDesc,P.BranchCode,branch.BRANCHNAME_TH,P.ACTIVE,ISNULL(sot.TotalInMonth,0) as TotalPay ");
            str.AppendLine(" from [dbo].[master_customer] cus ");
            str.AppendLine(" inner join master_customer_pricelist P ");
            str.AppendLine(" on P.SID = cus.SID and P.CompanyCode = cus.Companycode and P.CustomerCode = cus.CustomerCode ");
            str.AppendLine(" left join master_titlename as title on cus.SID = title.SID and cus.TitleCode = title.TitleCode ");
            str.AppendLine(" left join dbo.master_conf_pricelist pl ");
            str.AppendLine(" on P.SID = pl.SID and P.PriceList = pl.PriceListCode ");
            str.AppendLine(" left join dbo.BRANCH_MASTER_GENERAL branch ");
            str.AppendLine(" on P.SID = branch.SID and P.CompanyCode = branch.COMPANYCODE and P.BranchCode = branch.BRANCHCODE ");
            str.AppendLine(" left join [dbo].[master_customer_address] a ");
            str.AppendLine(" on cus.SID=a.SID and cus.customercode = a.customercode and a.PropertiesCode='01' ");
            str.AppendLine(" left join [dbo].[master_customer_address] b ");
            str.AppendLine(" on cus.SID=b.SID and cus.customercode = b.customercode and b.PropertiesCode='02' ");
            str.AppendLine(" left join [dbo].[master_customer_address] c ");
            str.AppendLine(" on cus.SID=c.SID and cus.customercode = c.customercode and c.PropertiesCode='03' ");
            str.AppendLine(" left join [dbo].[master_customer_address] d ");
            str.AppendLine(" on cus.SID=d.SID and cus.customercode = d.customercode and d.PropertiesCode='04' ");
            str.AppendLine(" left join [dbo].[master_customer_address] e ");
            str.AppendLine(" on cus.SID=e.SID and cus.customercode = e.customercode and e.PropertiesCode='05' ");
            str.AppendLine(" left join [dbo].[master_customer_address] f ");
            str.AppendLine(" on cus.SID=f.SID and cus.customercode = f.customercode and f.PropertiesCode='06' ");
            str.AppendLine(" left join [dbo].[master_customer_address] g ");
            str.AppendLine(" on cus.SID=g.SID and cus.customercode = g.customercode and g.PropertiesCode='07' ");
            str.AppendLine(" left join [dbo].[master_customer_address] h ");
            str.AppendLine(" on cus.SID=h.SID and cus.customercode = h.customercode and h.PropertiesCode='08' ");
            str.AppendLine(" left join [dbo].[master_customer_address] i ");
            str.AppendLine(" on cus.SID=i.SID and cus.customercode = i.customercode and i.PropertiesCode='09' ");
            str.AppendLine(" left join [dbo].[master_customer_address] j ");
            str.AppendLine(" on cus.SID=j.SID and cus.customercode = j.customercode and j.PropertiesCode='10' ");
            str.AppendLine(" left join [dbo].[master_customer_address] k ");
            str.AppendLine(" on cus.SID=k.SID and cus.customercode = k.customercode and k.PropertiesCode='11' ");
            str.AppendLine(" left join [dbo].[master_customer_address] l ");
            str.AppendLine(" on cus.SID=l.SID and cus.customercode = l.customercode and l.PropertiesCode='12' ");
            str.AppendLine(" left join [dbo].[master_customer_address] m ");
            str.AppendLine(" on cus.SID=m.SID and cus.customercode = m.customercode and m.PropertiesCode='13' ");
            str.AppendLine(" left join [dbo].[master_customer_address] n ");
            str.AppendLine(" on cus.SID=n.SID and cus.customercode = n.customercode and n.PropertiesCode='14' ");
            str.AppendLine(" left join (select ISNULL(SUM(a.NetValue * case when dob.PostingType='SR' then -1 else 1 end),0) as TotalInMonth");
            str.AppendLine(" ,a.SoldToParty,a.companyCode,a.SID");
            str.AppendLine(" from sd_so_header a left outer join master_config_lo_doctype_docdetail dob WITH(NOLOCK) on a.Stypecode=dob.DocumentTypeCode and a.SID=dob.SID");
            str.AppendLine(" left join dbo.sys_so_doctype_mapping_auto_gibl sys_so on a.SID = sys_so.SID and a.companycode = sys_so.COMPANYCODE");
            str.AppendLine(" and (a.Stypecode=sys_so.SODOCTYPE  or a.Stypecode=sys_so.SR_DOCTYPE) where a.Status = '07'");
            str.AppendLine(" group by a.SoldToParty,a.companyCode,a.SID) as sot");
            str.AppendLine(" on cus.SID = sot.SID and cus.CompanyCode = sot.companyCode and cus.CustomerCode = sot.SoldToParty");
            str.AppendLine(" where cus.sid ='" + p_sid + "' and cus.CustomerGroup <> 'C05'");
            str.AppendLine(Validation.CreateString(p_companycode, "cus.CompanyCode"));
            str.AppendLine(Validation.CreateString(p_active, "P.ACTIVE"));

            //if (!string.IsNullOrEmpty(p_month))
            //    str.AppendLine("and SUBSTRING(P.Birthday,5,2) = '" + p_month + "'");
            if (!string.IsNullOrEmpty(p_pricelist))
                str.AppendLine("and P.PriceList = '" + p_pricelist + "'");

            if (!string.IsNullOrEmpty(p_month) && string.IsNullOrEmpty(p_tomonth))
            {
                str.AppendLine("and SUBSTRING(P.Birthday,5,2) = '" + p_month + "'");
            }
            else if (!string.IsNullOrEmpty(p_month) && !string.IsNullOrEmpty(p_tomonth))
            {
                //str.AppendLine("and SUBSTRING(P.Birthday,5,2) in ('" + p_month + "','" + p_tomonth + "')");
                str.AppendLine("and SUBSTRING(P.Birthday,5,2) >= '"+p_month+"' ");
                str.AppendLine("and SUBSTRING(P.Birthday,5,2) <= '" + p_tomonth + "' ");
            }
               


            dt = dbService.selectDataFocusone(str.ToString());

            dt.Columns.Add("age", typeof(System.String)).DefaultValue = "";

            return dt;
        }
    }
}