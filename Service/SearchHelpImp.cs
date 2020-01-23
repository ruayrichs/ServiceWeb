using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using agape.lib.constant;
using agape.lib.fobgp.service;
using Agape.FocusOne.Utilities;
using System.Text;
using System.Globalization;
using ERPW.Lib.Authentication;
using Agape.Lib.DBService;
using agape.lib.agcommonutils;
using ServiceWeb.Util;
using ERPW.Lib.F1WebService.ICMUtils;

namespace ServiceWeb.Service
{
    public class SearchHelpImp
    {
        #region Private variable        
        private ICMUtils icmUtil = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private fobgpService serviceFobgp = new fobgpService();
        private DBService DB = new DBService();
        #endregion

        #region String const
        public const string BUSINESS_GOODS_ISSUE = "GISSUE";
        public const string BUSINESS_GOODS_ISSUE_SO = "GISSUESO";
        public const string BUSINESS_REVERSE_GOODS_ISSUE = "REGISSUE";
        public const string BUSINESS_GOODS_ISSUE_Reservation = "GISSUERES";
        public const string BUSINESS_GOODS_ISSUE_DO = "GISSUEDO";
        public const string BUSINESS_DELIVERY_ORDER = "DO";
        public const string BUSINESS_REVERSEATION = "RESERVE";
        public const string BUSINESS_GOODS_ISSUE_SO_CONSIGNMENT_ISSUE = "GISOCI";
        public const string BUSINESS_GOODS_ISSUE_MAT_DAC = "GISSUEMD";
        public const string BUSINESS_SALE_ORDER = "SO";
        public const string BUSINESS_SALE_ORDER_CONSIGNMENTISSUE = "SOCI";
        public const string BUSINESS_GOODS_RECEIPT = "GRECEIPT";
        public const string BUSINESS_INCOMING = "INCOMING";
        public const string BUSINESS_CASHSALE = "CASHSALE";
        public const string FINANCIAL_DOCSTATUS_REVERSE = "R";
        public const string FINANCIAL_DOCSTATUS_REVERSE_PARK = "V";

        public const string STATUS_SO_CODE_APPROVE = "02";
        public const string STATUS_SO_CODE_CANCEL = "03";
        public const string STATUS_SO_CODE_COMPLETED = "07";
        public const string STATUS_DO_CODE_BLOCKED = "05";

        public const string PROPERTY_CODE_TRANSFORMATION = "TRANS";
        public const string PROPERTY_CODE_ADDITIONALTERM = "TERMS";

        #endregion

        public DataTable getSearchMat(string matCode)
        {
            string where = "";
            if (!string.IsNullOrEmpty(matCode))
            {
                where += " and A.ItmNumber LIKE '" + matCode + "%'";
            }

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, icmconstants.ICM_CONST_MM_GETMATERIAL, "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
            dt.Columns.Add("TempDescription", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                item["TempDescription"] = item["ItmDescription"].ToString();
                item["ItmDescription"] = item["ItmNumber"].ToString() + " - " + item["ItmDescription"].ToString();
            }

            return dt;
        }

        public DataTable getSearchSOPlant()
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000305", "#Where A.SID='" + ERPWAuthentication.SID + "'");
            foreach (DataRow item in dt.Rows)
            {
                item["PLANTNAME1"] = item["PLANTCODE"].ToString() + " - " + item["PLANTNAME1"].ToString();
            }

            return dt;
        }

        public DataTable getSearchStorage(string plantcode)
        {
            string where = "";
            if (plantcode != "")
            {
                where += " and A.PLANTCODE LIKE'" + plantcode + "%'";
            }

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000309", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
            foreach (DataRow item in dt.Rows)
            {
                item["StoreName"] = item["STORAGELOCCODE"].ToString() + " - " + item["StoreName"].ToString();
            }

            return dt;
        }

        public DataTable getSearchRackbin()
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, icmconstants.ICM_CONST_SH_RACKBIN, "#where A.SID='" + ERPWAuthentication.SID + "'");
            foreach (DataRow item in dt.Rows)
            {
                item["Description"] = item["RACKBIN"].ToString() + " - " + item["Description"].ToString();
            }

            return dt;
        }

        public DataTable getSearchBatch()
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000391", "#where A.SID='" + ERPWAuthentication.SID + "'");
            foreach (DataRow item in dt.Rows)
            {
                item["description"] = item["batchnumber"].ToString() + " - " + item["description"].ToString();
            }

            return dt;
        }

        public DataTable getBranch(string sid, string companycode)
        {
            string sql = "select a.BRANCHCODE,a.BRANCHNAME_TH from BRANCH_MASTER_GENERAL as a " +
                         "where a.SID='" + sid + "'";
            if (!String.IsNullOrEmpty(companycode))
                sql += " and a.COMPANYCODE='" + companycode + "'";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable getMaterialBarcode(string sid, string barcodeid)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT TOP 1 BARCODEID, MATCODE, item.ItmDescription as  MATNAME, MATGROUPCODE, MATGROUPNAME, SALEUOM  ");
            sb.AppendLine("FROM Material_Barcode barcode ");
            sb.AppendLine(" left join  master_mm_items item on barcode.SID = item.SID and  barcode.MATCODE = item.ItmNumber ");
            sb.AppendLine("WHERE barcode.SID='" + sid + "' and barcode.BARCODEID='" + barcodeid.Trim() + "' ");


            DataSet ds = serviceFobgp.selectData(sb.ToString());
            DataTable dt = ds.Tables[0];
            return dt;
        }

        public DataTable GetSearch_TranHeader(string condition)
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHJCUSTOMTRN", "#Where A.SID='" + ERPWAuthentication.SID + "' " + condition);

            return dt;
        }

        public DataTable GetSearch_Doctype(string companycode, string businessgroup)
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000220", "#Where a.SID='" + ERPWAuthentication.SID + "' and C.BusinessCode in(" + businessgroup + ") and A.CompanyCode='" + companycode + "'");

            return dt;
        }

        public DataTable GetSearch_OrderReason(string companycode, string doctype_in)
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000510", "#Where A.SID='" + ERPWAuthentication.SID + "' and DocumentType in(" + doctype_in + ") and CompanyCode='" + companycode + "' order by OrderReasonCode asc ");
            foreach (DataRow item in dt.Rows)
            {
                item["Description"] = item["OrderReasonCode"].ToString() + " - " + item["Description"].ToString();
            }

            return dt;
        }

        public DataTable getDefinePlantTranfer(string plantFrom, string plantTo)
        {
            string sql = "select a.*,b.CustomerName from dbo.mm_conf_define_plant a left outer join master_customer b " +
                         " on a.sid = b.sid and a.CustomerCode=b.CustomerCode " +
                         " where a.SID='" + ERPWAuthentication.SID + "' and a.PLANTCODE in('" + plantFrom + "','" + plantTo + "')";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable GetSearch_TransferConsign(string condition)
        {
            return GetSearch_TransferConsign("", "", "", "", "", "", "", "", "", condition);
        }

        public DataTable GetSearch_TransferConsign(string doctype, string reason, string plant_to, string status, string createdby,
                                                   string docnumber, string docdate_from, string docdate_to, string fiscalyear, string xcontition)
        {
            string condition = "";

            if (!string.IsNullOrEmpty(doctype))
            {
                condition += " and A.DocType='" + doctype + "'";
            }
            if (!string.IsNullOrEmpty(reason))
            {
                condition += " and A.OrderReason='" + reason + "'";
            }
            if (!string.IsNullOrEmpty(plant_to))
            {
                condition += " and A.PlantTo='" + plant_to + "'";
            }
            if (status != "ALL" && !string.IsNullOrEmpty(status))
            {
                condition += " and A.StatusTransfer='" + status + "'";
            }
            if (status == "ALL")
            {
                condition += " and A.StatusTransfer in('','11','12','07','03','13','14')";
            }
            if (!string.IsNullOrEmpty(createdby))
            {
                condition += " and A.CREATED_BY='" + createdby + "'";
            }
            if (!string.IsNullOrEmpty(docnumber))
            {
                condition += " and A.DocNumber like'%" + docnumber + "%'";
            }
            if (!string.IsNullOrEmpty(docdate_from) && !string.IsNullOrEmpty(docdate_to))
            {
                condition += " and (A.DocDate>='" + docdate_from + "' and A.DocDate<='" + docdate_to + "')";
            }
            else if (!string.IsNullOrEmpty(docdate_from))
            {
                condition += " and A.DocDate='" + docdate_from + "'";
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                condition += " and A.FiscalYear='" + fiscalyear + "'";
            }

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID,
                                                  "GET_LIST_TRANSFER_CONSIGNMENT",
                                                  "#Where A.SID='" + ERPWAuthentication.SID + "'  and A.CompanyCode='" + ERPWAuthentication.CompanyCode + "' " + condition + " " + xcontition);

            dt.DefaultView.Sort = "DocNumber desc,DocDate desc";

            return dt.DefaultView.ToTable();
        }

        public IDictionary<String, decimal> searchAllSalePriceBySaleorgMaterial(String sid, DateTime searchDate)
        {
            IDictionary<String, decimal> idicData = new Dictionary<String, decimal>();
            String date = searchDate.ToString("yyyyMMdd");

            // base price [Saleorg + MaterialCode]
            string sql = "SELECT SID,[Sorgcode],[MeterialCode],[xVersion],[refBOM] " +
                         " ,[refPack],[xLineNo],[percentage],[amount],[CurrencyCode],[per] " +
                         " ,[UOM],[validfrom],[validto],[xTime],[xDate],[Qty] " +
                         " FROM [F_555_B001_005] WITH(NOLOCK) " +
                         " where sid='" + sid + "' " +
                         " and validfrom <= '" + date + "' " +
                         " and validto >= '" + date + "' ";

            DataSet ds = serviceFobgp.selectData(sql);
            if ((ds == null) || (ds.Tables.Count <= 0))
            {
                return idicData;
            }

            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String materialCode = Convert.ToString(dt.Rows[i]["MeterialCode"]);
                Decimal amount = Convert.ToDecimal(dt.Rows[i]["amount"]);

                if (idicData.ContainsKey(materialCode))
                {
                    continue;
                }
                idicData.Add(materialCode, amount);
            }

            return idicData;
        }

        public IDictionary<String, decimal> searchAllMarkDownByMatSaleorgMaterial(String sid, DateTime searchDate)
        {
            IDictionary<String, decimal> idicData = new Dictionary<String, decimal>();
            String date = searchDate.ToString("yyyyMMdd");

            // markdown [Saleorg + MatGroupCode1]
            string sql = "SELECT m.* " +
                         " , matgroupcode1.ItmNumber " +
                         " FROM [F_555_MD01_001] m WITH(NOLOCK) " +
                         " inner join  " +
                         " (  SELECT  [SID] " +
                         "    ,[ItmNumber],[MatGroupType],[MatGroupValue],[ValidDateFrom],[ValidDateTo] " +
                         "    FROM  [master_mm_item_matgroupcode] WITH(NOLOCK) " +
                         "    where MatGroupType = 'MATGROUPCODE1' " +
                         "    and [ValidDateFrom] <= '" + date + "' and [ValidDateTo] >= '" + date + "'  ) matgroupcode1  " +
                         " on m.SID = matgroupcode1.sid and m.[MatGroupCode1] = matgroupcode1.MatGroupValue " +
                         " where m.sid = '" + sid + "' " +
                         " and m.validfrom <= '" + date + "' " +
                         " and m.validto >= '" + date + "' ";

            DataSet ds = serviceFobgp.selectData(sql);
            if ((ds == null) || (ds.Tables.Count <= 0))
            {
                return idicData;
            }

            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String materialCode = Convert.ToString(dt.Rows[i]["ItmNumber"]);
                Decimal percentage = Convert.ToDecimal(dt.Rows[i]["percentage"]);

                if (idicData.ContainsKey(materialCode))
                {
                    continue;
                }
                idicData.Add(materialCode, percentage);
            }

            return idicData;
        }

        public IDictionary<String, decimal> searchAllOnePrice(String sid, DateTime searchDate)
        {
            IDictionary<String, decimal> idicData = new Dictionary<String, decimal>();
            String date = searchDate.ToString("yyyyMMdd");

            string sql = "SELECT m.* " +
                         " , matgroupcode1.ItmNumber " +
                         " FROM [F_555_B001_007] m WITH(NOLOCK) " +
                         " inner join  " +
                         " (  SELECT  [SID] " +
                         "    ,[ItmNumber],[MatGroupType],[MatGroupValue],[ValidDateFrom],[ValidDateTo] " +
                         "    FROM  [master_mm_item_matgroupcode] WITH(NOLOCK) " +
                         "    where MatGroupType = 'MATGROUPCODE1' " +
                         "    and [ValidDateFrom] <= '" + date + "' and [ValidDateTo] >= '" + date + "'  ) matgroupcode1  " +
                         " on m.SID = matgroupcode1.sid and m.[MatGroupCode1] = matgroupcode1.MatGroupValue and m.MeterialCode = matgroupcode1.ItmNumber " +
                         " where m.sid = '" + sid + "' " +
                         " and m.validfrom <= '" + date + "' " +
                         " and m.validto >= '" + date + "' " +
                         " and m.MatGroupCode1='ONEPRICE' ";

            DataSet ds = serviceFobgp.selectData(sql);
            if ((ds == null) || (ds.Tables.Count <= 0))
            {
                return idicData;
            }

            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String materialCode = Convert.ToString(dt.Rows[i]["MeterialCode"]);
                Decimal amount = Convert.ToDecimal(dt.Rows[i]["amount"]);

                if (idicData.ContainsKey(materialCode))
                {
                    continue;
                }
                idicData.Add(materialCode, amount);
            }

            return idicData;
        }

        public string CheckDefaultDoctype(string p_sid, string p_plantOut, string p_plantIn)
        {
            string _businesscode = "";
            bool _in = getConsign(p_sid, p_plantIn);
            bool _out = getConsign(p_sid, p_plantOut);

            if (_out == false && _in == true)
                _businesscode = "TCON";
            else if (_out == true && _in == false)
                _businesscode = "TCON2";
            else if (_out == true && _in == true)
                _businesscode = "TCON3";
            else if (_out == false && _in == false)
                _businesscode = "TCON4";

            return _businesscode;
        }

        public Boolean getConsign(string p_sid, string p_plant)
        {
            bool xConsign = false;

            string sql = "select XCONSIGNOR from mm_conf_define_plant where SID = '" + p_sid + "' and PLANTCODE = '" + p_plant + "'";

            DataTable dt = serviceFobgp.selectData(sql).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                xConsign = Convert.ToString(dr["XCONSIGNOR"]) == "" ? false : Convert.ToBoolean(dr["XCONSIGNOR"]);
            }

            return xConsign;
        }

        public DataTable GR_GetSearchDoctype2(string companycode, string p_where)
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000220",
                                                 "#Where a.SID='" + ERPWAuthentication.SID + "' and A.CompanyCode='" + companycode + "' " + p_where +
                                                 "  order by case when left(A.DocumentTypeCode,1) = 'T' then '0' + A.DocumentTypeCode " +
                                                 "  else A.DocumentTypeCode end ");

            foreach (DataRow item in dt.Rows)
            {
                item["description"] = item["documenttypecode"].ToString() + " - " + item["description"].ToString();
            }

            return dt;
        }

        public DataTable getLoBusinessObjectFromDocType(string p_companyCode, string p_docType)
        {
            string sql = "select * from master_config_lo_doctype_docdetail where sid='" + ERPWAuthentication.SID + "' and companyCode='" + p_companyCode + "' and DocumentTypeCode='" + p_docType + "' ";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable getDetailDoctype(string doctype)
        {
            string sql = "select * from dbo.master_config_lo_doctype_docdetail where SID='" + ERPWAuthentication.SID + "' and DocumentTypeCode='" + doctype + "'";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable getDetailMaterial(string matcode)
        {
            string sql = "select * from dbo.master_mm_items  where SID='" + ERPWAuthentication.SID + "'";

            if (!string.IsNullOrEmpty(matcode))
                sql += " and ItmNumber='" + matcode + "'";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable getSaleUom(string matcode)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(matcode))
            {
                condition = " and  ItmNumber='" + matcode + "'";
            }

            string sql = "select SalesUoM, salesuom as ucode, salesuom as udesc  FROM [master_mm_item_saledata] where 1=1" + condition + " group by [SalesUoM]";

            DataSet ds = serviceFobgp.selectData(sql);

            return ds.Tables[0];
        }

        public DataTable getSearchEmployeeCode(string companycode, string employeecode)
        {

            string where = "";

            if (employeecode != "")
            {
                where += " and A.EmployeeCode='" + employeecode + "'";
            }
            if (companycode != "")
            {
                where += " and A.CompanyCode='" + companycode + "'";
            }

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000728", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
            dt.Columns.Add("TempName", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                item["TempName"] = item["EmployeeCode"].ToString() + " - " + item["FirstName_TH"].ToString() + " " + item["LastName_TH"].ToString();
            }

            return dt;
        }

        public DataTable getReasonforRejectStatusWhereIn(string where)
        {
            string sql = "select * from dbo.master_conf_define_reasonforreject where SID='" + ERPWAuthentication.SID + "' " + where;

            DataSet ds = serviceFobgp.selectData(sql);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                item["Description"] = item["ReasonForRejectCode"] + " - " + item["Description"];
            }

            return ds.Tables[0];
        }

        public DataTable getMasterEmployee(string companycode, string employeecode)
        {
            string sql = "select *  from master_employee " +
                        "where SID='" + ERPWAuthentication.SID + "' and CompanyCode='" + companycode + "'";

            if (!string.IsNullOrEmpty(employeecode))
                sql += " and EmployeeCode='" + employeecode + "'";

            DataTable dt = serviceFobgp.selectData(sql).Tables[0];
            return dt;
        }

        public DataTable getSaleChannel()
        {
            string sql = @"select SCHANNELCODE as SDCode, SCHANNELCODE + ' - ' + SHCANNELNAME as SDDesc 
                           from sd_conf_define_sales_channel where SID='" + ERPWAuthentication.SID + "'";

            return serviceFobgp.selectData(sql).Tables[0];
        }

        public DataTable getDoctypeTransferReturn(string companycode, string doctype_trnsfer)
        {
            string sid = ERPWAuthentication.SID;
            string sql = "select * from f1_longtail_pos_doctrreturn_mapping where SID='" + sid + "' ";
            if (!string.IsNullOrEmpty(companycode))
                sql += " and CompanyCode='" + companycode + "'";
            if (!string.IsNullOrEmpty(doctype_trnsfer))
                sql += " and DOCTYPE_TR='" + doctype_trnsfer + "'";

            DataSet ds = serviceFobgp.selectData(sql);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        public DataTable getPlantEmployeeV2(String sid, string objectid)
        {
            string sql = @"select  * from dbo.BRANCH_MASTER_INVENTORYORG 
                           where SID='" + sid + @"' 
                           and OBJECTID='" + objectid + "'";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable GetSearch_POItem(string companycode, string planin)
        {
            string sid = ERPWAuthentication.SID;
            string sql = @"select A.* from dbo.goods_receipt_ref_po A
                          left join master_config_lo_doctype_docdetail B
                              on A.DocumentTypeCode = B.DocumentTypeCode
                              and B.PostingType in('PO','PODropShip')
                          where B.SID='" + sid + @"' 
                              and B.companyCode='" + companycode + @"'
                          order by A.FiscalYear desc , A.DocDate desc,A.PONumber desc ";


            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable getListDocType(string SID, string CompanyCode)
        {
            string sql = @"SELECT A.DocumentTypeCode AS DocumentTypeCode,A.PostingType AS PostingType,
                              B.Description AS Description, A.indRelease AS indRelease 
                           FROM master_config_lo_doctype_docdetail AS A 
                           INNER JOIN master_config_lo_doctype AS B
                              ON A.SID = B.SID
                              AND A.DocumentTypeCode = B.DocumentTypeCode
                              AND A.companyCode = B.CompanyCode  
                           Where a.SID='" + SID + @"'
                              and A.CompanyCode='" + CompanyCode + @"' 
                              and PostingType='GRECEIPTPO'";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }
        public DataTable GetSearchBusArea()
        {
            DataTable dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000135",
                "#Where a.SID='" + ERPWAuthentication.SID + "'"
            );
            return dt;
        }
        public DataTable GetSearch_Costcenter(String companycode)
        {
            DataTable dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000572",
                "#Where A.SID='" + ERPWAuthentication.SID + "' and A.CompanyCode='" + companycode + "'"
            );
            return dt;
        }
        public DataTable GetSearch_GLAccount()
        {
            DataTable dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000804",
                "#Where A.SID='" + ERPWAuthentication.SID + "'"
            );
            return dt;
        }
        public DataTable GetSearch_Budget(string companycode)
        {
            DataTable dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000607",
                "#Where A.SID='" + ERPWAuthentication.SID + "' and A.CompanyCode='" + companycode + "'");
            return dt;
        }
        public DataTable GetSearch_CostElement()
        {
            DataTable dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000573", "#Where A.SID='" + ERPWAuthentication.SID + "'");
            return dt;
        }
        public DataTable GetSearch_Project()
        {
            DataTable dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000629",
                "#Where A.SID='" + ERPWAuthentication.SID + "'");
            return dt;
        }
        public DataTable GetSearch_ProjectElement()
        {
            DataTable dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000630",
                "#Where A.SID='" + ERPWAuthentication.SID + "'");
            return dt;
        }
        public DataTable GetSearch_Channel()
        {
            DataTable dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000640",
                "#Where A.SID='" + ERPWAuthentication.SID + "'");
            return dt;
        }
        public DataTable getSearchUnit()
        {
            DataTable dt;
            dt = WSHelper.GetSearchData(
                ERPWAuthentication.SID,
                "SHM000057",
                "#where A.SID='" + ERPWAuthentication.SID + "' ");
            return dt;
        }

        public DataTable gtetGIGRNumberWithDocTransfer(string companycode, string doctransfer)
        {
            string sid = ERPWAuthentication.SID;

            string sql = "select DocumentNoGI,DocumentNoGR from dbo.mm_itemtranfer_head where MatDoc='" + doctransfer + "'";
            sql += " and CompanyCode='" + companycode + "'";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable getCustomer(string sid, string companycode, string where)
        {
            List<string> sql_arr = new List<string>();
            sql_arr.Add("select CustomerCode, CustomerName");
            sql_arr.Add("from master_customer");
            sql_arr.Add("where 1=1");

            if (sid != null && !string.IsNullOrEmpty(sid))
            {
                sql_arr.Add("and SID = '" + sid + "'");
            }
            if (companycode != null && !string.IsNullOrEmpty(companycode))
            {
                sql_arr.Add("and CompanyCode = '" + companycode + "'");
            }
            if (where != null && !string.IsNullOrEmpty(where))
            {
                sql_arr.Add(where);
            }
            sql_arr.Add("group by CustomerCode, CustomerName");
            sql_arr.Add("order by CustomerName");

            DataSet ds = serviceFobgp.selectData(string.Join(" ", sql_arr.ToArray()));
            return ds.Tables[0];
        }

        public DataTable SearchCustomer(string name, string mobile, string email, string memberid)
        {
            DataTable dt = new DataTable();

            dt = SearchCustomer(name, mobile, email, "", "", "", memberid, "", "", "");

            if (dt.Rows.Count > 0)
            {
                return dt;
            }

            return SearchCustomer(name, mobile, email, "", "", "", "", "", "", memberid);
        }

        public DataTable SearchCustomer(string status, string type, string expire, string customer_from, string customer_to, string branch)
        {
            return SearchCustomer("", "", "", status, type, expire, customer_from, customer_to, branch, "");
        }

        public DataTable SearchCustomer(string name, string mobile, string email, string status, string type, string expire,
                                        string customer_from, string customer_to, string branch, string memberid)
        {

            DataTable _dt = new DataTable();
            string sid = ERPWAuthentication.SID;
            string companycode = ERPWAuthentication.CompanyCode;

            List<string> sql_arr = new List<string>();
            sql_arr.Add("SELECT c.[CustomerCode] ,c.[CustomerName] ,c.[CustomerGroup] ,c.[Currency] ,c.[FederalTaxID] ,c.[ForeignName] ,c.[CompanyCode] ,c.[SID] ,c.[ChangeCurrency] ,c.[TitleCode] ,p.[CREATED_BY] ,p.[UPDATED_BY] ,p.[CREATED_ON] ,p.[UPDATED_ON] ,c.[CustomerNameTH] ,");
            sql_arr.Add("p.[PriceList], p.[PRICEGROUP] ,p.[MEMBERCARD_ID] ,p.[VALID_FROM] ,p.[VALID_TO] ,p.[ACTIVE] ,p.[Remark] ,p.Birthday, ");
            sql_arr.Add("q.Description,r.TelNo1,r.Mobile,r.EMail,s.BRANCHNAME_TH,t.FirstName_TH,t.LastName_TH,ISNULL(z.PriceGroupDescription,'*') as PriceGroupDescription, ");
            sql_arr.Add("p.FirstName as MemberFName,p.LastName as MemberLName,p.Phone as MemberPhone,p.Mobile as MemberMobile,p.Mail as MemberMail,p.TitleCode as MemberTitle,p.BranchCode ");

            sql_arr.Add("FROM [master_customer] c");
            sql_arr.Add("inner join [master_customer_pricelist] p on c.sid = p.sid and c.companycode = p.companycode and c.customercode = p.customercode");
            sql_arr.Add("left join [master_conf_pricelist] q on p.SID = q.SID and p.PriceList = q.PriceListCode");
            sql_arr.Add("left join MASTER_CONFIG_PRICEGROUP z on p.SID = z.SID and p.PriceGroup = z.PriceGroupCode");
            sql_arr.Add("left join master_customer_general r on c.sid = r.sid and c.CompanyCode=r.CompanyCode and c.CustomerCode=r.CustomerCode");
            sql_arr.Add("left join dbo.BRANCH_MASTER_GENERAL s on p.SID = s.SID and p.CompanyCode=s.COMPANYCODE and p.BranchCode = s.BRANCHCODE");
            sql_arr.Add("left join dbo.master_employee t on c.SID = t.SID and c.CompanyCode=t.CompanyCode and c.CREATED_BY = t.EmployeeCode");


            //GET LATEST CUSTOMER TRANSACTION
            sql_arr.Add("join (SELECT [SID], [CompanyCode], [CustomerCode], [PriceGroup],[PriceList] , [MEMBERCARD_ID], MAX([CREATED_ON]) AS [CREATED_ON]");
            sql_arr.Add("FROM [master_customer_pricelist]");
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

            DataSet ds = serviceFobgp.selectData(string.Join(" ", sql_arr.ToArray()));
            _dt = ds.Tables[0];

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
                _dr_new["BranchCode"] = _dr["BranchCode"];
                _dr_new["Remark"] = _dr["Remark"];

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
        public DataTable getCustomerByMemberType(String sid, string companycode, string pricelist)
        {
            List<string> sql_arr = new List<string>();

            sql_arr.Add("SELECT c.[CustomerCode] ,c.[CustomerName]");
            sql_arr.Add("FROM [master_customer] c");
            sql_arr.Add("inner join [master_customer_pricelist] p ");
            sql_arr.Add("on c.sid = p.sid and c.companycode = p.companycode and c.customercode = p.customercode");
            sql_arr.Add("WHERE c.sid = '" + sid + "' and c.companycode = '" + companycode + "'");

            if (!string.IsNullOrEmpty(pricelist))
                sql_arr.Add("and p.[PriceList] = '" + pricelist + "'");

            DataSet ds = serviceFobgp.selectData(string.Join(" ", sql_arr.ToArray()));
            return ds.Tables[0];
        }

        public DataTable getSearchPrefix(string titlecode)
        {
            string where = "";
            if (titlecode != "")
            {
                where += " and TitleCode='" + titlecode + "'";
            }

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000257", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);

            return dt;
        }

        public DataTable getSearchSalePriceList()
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000535", "#where A.SID='" + ERPWAuthentication.SID + "'");
            if (!dt.Columns.Contains("Code_Description"))
            {
                dt.Columns.Add("Code_Description");
            }
            foreach (DataRow dr in dt.Rows)
            {
                dr["Code_Description"] = dr["PriceListCode"] + " - " + dr["Description"];
            }
            return dt;
        }

        public DataTable prepareDTBranchDesc(DataTable _dt, string _col_in, string _col_out)
        {
            DataTable DTBranch = getBranch(ERPWAuthentication.CompanyCode);

            if (_dt.Columns.Contains(_col_in))
            {
                if (!_dt.Columns.Contains(_col_out))
                {
                    _dt.Columns.Add(_col_out, typeof(string));
                }
            }

            foreach (DataRow _dr in _dt.Rows)
            {
                if (_dr[_col_in] != null)
                {
                    DataRow[] _dr_find = DTBranch.Select("BRANCHCODE='" + _dr[_col_in].ToString() + "'");
                    if (_dr_find.Length > 0)
                    {
                        foreach (DataRow _dr_ele in _dr_find)
                        {
                            _dr[_col_out] = _dr_ele["BRANCHCODE"].ToString() + " - " + _dr_ele["BRANCHNAME_TH"].ToString();
                        }
                    }
                    else
                    {
                        _dr[_col_out] = _dr[_col_in].ToString();
                    }
                }
            }

            return _dt;
        }

        public DataTable getBranch(string companycode)
        {
            string sql = @"select a.BRANCHCODE,a.BRANCHNAME_TH
                        ,a.BRANCHCODE+' - '+a.BRANCHNAME_TH as Description
                      from BRANCH_MASTER_GENERAL as a " +
                         "where a.SID='" + ERPWAuthentication.SID + "'";
            if (!String.IsNullOrEmpty(companycode))
                sql += " and a.COMPANYCODE='" + companycode + "'";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable DTColumnTypeDouble(DataTable dt, List<string> doubleColumns)
        {
            DataTable dtNew = new DataTable();

            foreach (DataColumn dc in dt.Columns)
            {
                if (doubleColumns.Contains(dc.ColumnName))
                {
                    dtNew.Columns.Add(dc.ColumnName, typeof(double));
                }
                else
                {
                    dtNew.Columns.Add(dc.ColumnName, dc.DataType);
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtNew.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    object objValue = dr[dc.ColumnName];
                    if (doubleColumns.Contains(dc.ColumnName))
                    {
                        double dValue = 0;
                        if (objValue != null)
                        {
                            double.TryParse(objValue.ToString(), out dValue);
                        }
                        drNew[dc.ColumnName] = dValue;
                    }
                    else
                    {
                        drNew[dc.ColumnName] = objValue;
                    }
                }
                dtNew.Rows.Add(drNew);
            }

            return dtNew;
        }

        public DataTable DTColumnTypeDate(DataTable dt, List<string> dateColumns, string dateFormate)
        {
            DataTable dtNew = new DataTable();

            foreach (DataColumn dc in dt.Columns)
            {
                if (dateColumns.Contains(dc.ColumnName))
                {
                    dtNew.Columns.Add(dc.ColumnName, typeof(DateTime));
                }
                else
                {
                    dtNew.Columns.Add(dc.ColumnName, dc.DataType);
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtNew.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    object objValue = dr[dc.ColumnName];
                    if (dateColumns.Contains(dc.ColumnName))
                    {
                        DateTime dValue = DateTime.Now;

                        if (objValue != null)
                        {
                            DateTime.TryParseExact(objValue.ToString(), dateFormate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dValue);
                        }
                        drNew[dc.ColumnName] = dValue;
                    }
                    else
                    {
                        drNew[dc.ColumnName] = objValue;
                    }
                }
                dtNew.Rows.Add(drNew);
            }

            return dtNew;
        }

        public DataTable getSearchOrdertype(string companycode, string PostingType, string BranchCode)
        {
            string where = "";
            if (companycode != "")
            {
                where += " and A.companyCode='" + companycode + "'";
            }
            if (PostingType != "")
            {
                where += " and A.PostingType='" + PostingType + "'";
            }
            if (BranchCode != "")
            {
                where += " and A.BranchCode='" + BranchCode + "'";
            }

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000220", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
            if (dt.Rows.Count == 0)
            {
                where = "";
                if (companycode != "")
                {
                    where += " and A.companyCode='" + companycode + "'";
                }
                if (PostingType != "")
                {
                    where += " and A.PostingType='" + PostingType + "'";
                }
                dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000220", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
            }

            dt.Columns.Add("Description2", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                item["Description2"] = item["DocumentTypeCode"].ToString() + " - " + item["Description"].ToString();
            }

            return dt;
        }

        public DataTable getSearchSODocnumber(string companycode, string ordertype)
        {
            string where = "";
            if (companycode != "")
            {
                where += " and A.companyCode='" + companycode + "'";
            }
            if (ordertype != "")
            {
                where += " and A.Stypecode='" + ordertype + "'";
            }

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "XQT01", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);

            return dt;
        }

        public DataTable getSearchSODocnumber(string companycode, string ordertype, string postingtype)
        {
            string where = "";
            if (companycode != "")
            {
                where += " and A.companyCode='" + companycode + "'";
            }
            if (ordertype != "")
            {
                where += " and A.Stypecode='" + ordertype + "'";
            }
            if (postingtype != "")
            {
                where += " and B.Postingtype='" + postingtype + "'";
            }

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "XQT01", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);

            return dt;
        }

        public DataTable getCampaignCode(string SID, string CompanyCode)
        {
            string sql = "select a.CampaignCode as pro_code,a.CampaignDesc as pro_desc " +
                         "from sd_campaigns_scheme as a " +
                         "where a.SID='" + SID + "'";

            if (!string.IsNullOrEmpty(CompanyCode))
                sql += " and a.CompanyCode='" + CompanyCode + "'";

            DataSet ds = serviceFobgp.selectData(sql);

            return ds.Tables[0];
        }

        #region  ReportRegisterCustomersOfBranch

        public DataTable ReportRegisterCustomersOfBranch(String sid, string companycode, string branch, string registerDateFrom, string registerDateTo, string statusActive)
        {
            //string sql = "select *,C.Description as CustomerGroupName from master_customer A inner join master_customer_general B " +
            //                "on A.SID=B.SID and A.CompanyCode = B.CompanyCode and A.CustomerCode = B.CustomerCode " +
            //                "left join master_config_customer_doctype C "+
            //                "on A.SID=C.SID and A.CompanyCode = C.Companycode and A.CustomerGroup = C.CustomerGroupCode "+
            //                "inner join master_customer_pricelist P on P.SID = A.SID and P.CompanyCode = C.Companycode and P.CustomerCode = A.CustomerCode " +
            //                "where A.SID='" + sid + "'";

            StringBuilder str = new StringBuilder();
            str.AppendLine(" select cus.CustomerCode,cus.CustomerName,cus.CustomerNameTH,cus.ForeignName ");
            str.AppendLine(" , a.xValue as building, b.xValue as roomno, c.xValue as floorno, d.xValue as village, ");
            str.AppendLine(" e.xValue as company,  f.xValue as houseno,  g.xValue as moo,  h.xValue as soi, ");
            str.AppendLine(" i.xValue as street,  j.xValue as tumbon,  k.xValue as amphur,  l.xValue as province, ");
            str.AppendLine(" m.xValue as postcode,  n.xValue as country ");
            str.AppendLine(" ,P.Birthday ");
            str.AppendLine(@",case ISNULL(P.Birthday,'') when '' then '' 
                                else CONVERT(VARCHAR(10), CAST(P.Birthday AS DATETIME), 103) end BirthdayDes ");
            str.AppendLine(",P.Mail,P.Phone,P.Mobile ");
            str.AppendLine(",P.VALID_FROM,P.VALID_TO ");
            str.AppendLine(@"
                             ,case ISNULL(P.VALID_FROM,'')
	                             when '' then ''
	                             else CONVERT(VARCHAR(10), CAST(P.VALID_FROM AS DATETIME), 103)
	                            end VALID_FROM_DES
                             ,case ISNULL(P.VALID_TO,'')
	                             when '' then ''
	                             else CONVERT(VARCHAR(10), CAST(P.VALID_TO AS DATETIME), 103)
	                             end VALID_TO_DES 
                        ");
            str.AppendLine(",P.MEMBERCARD_ID,P.PriceList,P.BranchCode ");
            str.AppendLine(" ,pl.Description as PricelistDesc,P.BranchCode,branch.BRANCHNAME_TH,P.ACTIVE ");
            str.AppendLine(" from [dbo].[master_customer] cus ");
            str.AppendLine(" inner join master_customer_pricelist P ");
            str.AppendLine(" on P.SID = cus.SID and P.CompanyCode = cus.Companycode and P.CustomerCode = cus.CustomerCode ");
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

            str.AppendLine(" where cus.sid ='" + sid + "'   ");

            if (!string.IsNullOrEmpty(companycode))
            {
                str.AppendLine("  and  cus.CompanyCode='" + companycode + "' ");
            }
            if (!string.IsNullOrEmpty(branch))
            {
                str.AppendLine("  and P.BranchCode='" + branch + "' ");
            }
            if (!string.IsNullOrEmpty(registerDateFrom) && !string.IsNullOrEmpty(registerDateTo))
            {
                str.AppendLine("  and (cus.CREATED_ON >='" + registerDateFrom + "000000" + "' and cus.CREATED_ON<='" + registerDateTo + "999999" + "')");
            }
            else if (!string.IsNullOrEmpty(registerDateFrom))
            {
                str.AppendLine("  and (cus.CREATED_ON >='" + registerDateFrom + "000000" + "' and cus.CREATED_ON<='" + registerDateFrom + "999999" + "')");
            }
            if (!string.IsNullOrEmpty(statusActive))
            {
                str.AppendLine("  and P.ACTIVE ='" + statusActive + "'");
            }
            str.AppendLine("  order by cus.CREATED_ON desc");
            DataSet ds = serviceFobgp.selectData(str.ToString());
            DataTable dt = ds.Tables[0];
            return dt;
        }
        #endregion

        #region Goods Issue
        public bool checkMovementTypeActive(string SID, string CompanyCode, string MVT)
        {
            string sql = @"SELECT * FROM master_config_lo_doctype 
                           WHERE 1=1  AND SID = '" + SID + "' AND xActive='" + ApplicationConstants.TRUE_STRING + @"'
                           AND companyCode = '" + CompanyCode + "' AND DocumentTypeCode = '" + MVT + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public DataTable getGIOrderReason(string sid, string companycode, string doctype, string reasoncode)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(companycode))
            {
                condition += " AND CompanyCode ='" + companycode + "'";
            }
            if (!string.IsNullOrEmpty(doctype))
            {
                condition += " AND DocumentType ='" + doctype + "'";
            }
            if (!string.IsNullOrEmpty(reasoncode))
            {
                condition += " AND OrderReasonCode ='" + reasoncode + "'";
            }

            string sql = @"SELECT *,OrderReasonCode + ' : ' + Description + 
                           CASE GLAccount WHEN '' THEN CASE CostCenter WHEN '' THEN '' ELSE ' (ศูนย์ต้นทุน : ' + CostCenter + ')' END
                           ELSE CASE CostCenter WHEN '' THEN ' (เลขที่บัญชี : ' + GLAccount + ')'
                           ELSE ' (เลขที่บัญชี : ' + GLAccount + ', ศูนย์ต้นทุน : ' + CostCenter + ')'END END AS DisplayReason
                           FROM master_conf_define_orderreason
                           WHERE sid = '" + sid + "' " + condition + " ORDER BY OrderReasonCode";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getGIRejectReason(string sid, string doctype, string rejectcode)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(doctype))
            {
                condition += " AND DocumentType ='" + doctype + "'";
            }
            if (!string.IsNullOrEmpty(rejectcode))
            {
                condition += " AND ReasonForRejectCode ='" + rejectcode + "'";
            }

            string sql = @"SELECT *,ReasonForRejectCode + ' : ' + Description AS DisplayReason
                           FROM master_conf_define_reasonforreject
                           WHERE sid = '" + sid + "'" + condition + " ORDER BY ReasonForRejectCode";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getGIDocumentType(string SID, string CompanyCode)
        {
            string sql = @"select *
                           ,master_config_lo_doctype.DocumentTypeCode+' - '+ master_config_lo_doctype.Description as DocumentTypeCodeDes
                           from master_config_lo_doctype 
                           left outer join master_config_lo_doctype_docdetail 
                           on master_config_lo_doctype.sid = master_config_lo_doctype_docdetail.sid 
                           and master_config_lo_doctype.DocumentTypeCode = master_config_lo_doctype_docdetail.DocumentTypeCode
                           and master_config_lo_doctype.CompanyCode = master_config_lo_doctype_docdetail.CompanyCode
                           left outer join master_config_business on master_config_lo_doctype_docdetail.postingType = master_config_business.BusinessCode
                           where master_config_lo_doctype.SID = '" + SID + @"'
                           and master_config_business.BusinessGroup in ('" + BUSINESS_GOODS_ISSUE + "','" + BUSINESS_REVERSE_GOODS_ISSUE + @"') 
                           and master_config_lo_doctype.companyCode='" + CompanyCode + @"'";
            //and master_config_lo_doctype_docdetail.PostingType IN('" + BUSINESS_GOODS_ISSUE_DO + "','" + BUSINESS_GOODS_ISSUE_SO + "') ";


            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getGIDocumentNoList(string sid, string companycode, string documenttype, string fiscalyear, string fdocdate, string tdocdate, string pfdate, string ptdate)
        {
            try
            {
                string condition = "WHERE A.SID = '" + sid + "' ";
                condition += Validation.CreateString(companycode, "A.CompanyCode");
                condition += Validation.CreateString(fiscalyear, "A.FisicalYear");
                condition += Validation.CreateString(documenttype, "A.DocType");
                if (!string.IsNullOrEmpty(fdocdate) && !string.IsNullOrEmpty(tdocdate))
                {
                    condition += " AND A.DocumentDate >= '" + fdocdate + "' AND A.DocumentDate <= '" + tdocdate + "'";
                }
                else if (!string.IsNullOrEmpty(fdocdate))
                {
                    condition += " AND A.DocumentDate = '" + fdocdate + "'";
                }
                if (!string.IsNullOrEmpty(pfdate) && !string.IsNullOrEmpty(ptdate))
                {
                    condition += " AND A.PostingDate >= '" + pfdate + "' AND A.PostingDate <= '" + ptdate + "'";
                }
                else if (!string.IsNullOrEmpty(pfdate))
                {
                    condition += " AND A.PostingDate = '" + pfdate + "'";
                }

                if (string.IsNullOrEmpty(documenttype))
                {
                    string where = " AND C.BusinessGroup IN ('" + BUSINESS_GOODS_ISSUE + "','" + BUSINESS_REVERSE_GOODS_ISSUE + "')";
                    DataTable dtPostingType = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHM000220", "#WHERE A.SID='" + ERPWAuthentication.SID + "' " + where);
                    where = "";
                    foreach (DataRow dr in dtPostingType.Rows)
                    {
                        where += "'" + dr["DocumentTypeCode"].ToString() + "',";
                    }

                    if (!string.IsNullOrEmpty(where))
                    {
                        condition += " AND A.DocType IN (" + where.Substring(0, where.Length - 1) + ")";
                    }
                }

                string sql = @"SELECT A.MatDoc AS DocumentNumber,A.DocumentDate AS DocumentDate,A.PostingDate,A.GrGiSlipNo AS GrGiSlipNo,
                                    C.Description AS Description,A.DocHeader AS DocHeader,D.DESCRIPTION AS DocumentStatus,A.DocType AS DocumentType,
                                    F.Description AS DocumentTypeDesc 
                                    FROM mm_migo_head AS A 
                                    LEFT OUTER JOIN master_config_lo_doctype_docdetail AS B
                                    ON A.SID = B.SID AND A.CompanyCode = B.companyCode AND A.DocType = B.DocumentTypeCode  
                                    LEFT OUTER JOIN master_conf_define_orderreason AS C
                                    ON A.SID = C.SID AND A.CompanyCode = C.CompanyCode AND A.OrderReasonCode = C.OrderReasonCode AND B.PostingType = C.DocumentType  
                                    LEFT OUTER JOIN master_conf_document_status AS D
                                    ON A.SID = D.SID AND A.Status = D.STATUSCODE AND B.PostingType = D.BUSINESSCODE  
                                    INNER JOIN mm_migo_detail AS E
                                    ON A.SID = E.SID AND A.CompanyCode = E.CompanyCode AND A.FisicalYear = E.FisicalYear AND A.DocType = E.MovementType AND A.MatDoc = E.MatDoc  
                                    LEFT OUTER JOIN master_config_lo_doctype AS F
                                    ON A.SID = F.SID AND A.CompanyCode = F.CompanyCode AND A.DocType = F.DocumentTypeCode  
                                    LEFT OUTER JOIN master_config_business AS G
                                    ON B.PostingType = G.BusinessCode " + condition + @" 
                                    GROUP BY A.MatDoc,A.DocumentDate,A.PostingDate,A.GrGiSlipNo,C.Description,A.DocHeader,D.DESCRIPTION,A.DocType,F.Description 
                                    ORDER BY A.MatDoc ";

                DataTable dt = DB.selectDataFocusone(sql);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getGIRefDocumentType(string sid, string companycode, string postingtype)
        {
            string sql = @"select master_config_lo_doctype.DocumentTypeCode + ' : ' + master_config_lo_doctype.Description as Description2,*
                           from master_config_lo_doctype 
                           LEFT OUTER JOIN master_config_lo_doctype_docdetail
                           ON master_config_lo_doctype.sid = master_config_lo_doctype_docdetail.sid 
                           AND master_config_lo_doctype.DocumentTypeCode = master_config_lo_doctype_docdetail.DocumentTypeCode
                           AND master_config_lo_doctype.CompanyCode = master_config_lo_doctype_docdetail.CompanyCode
                           LEFT OUTER JOIN master_config_business ON master_config_lo_doctype_docdetail.postingType = master_config_business.BusinessCode where 1=1 ";

            sql += Validation.CreateString(sid, "master_config_lo_doctype.SID");
            sql += Validation.CreateString(companycode, "master_config_lo_doctype.companyCode");

            switch (postingtype)
            {
                case BUSINESS_GOODS_ISSUE_DO:
                    sql += " AND master_config_lo_doctype_docdetail.PostingType = '" + BUSINESS_DELIVERY_ORDER + "'";
                    break;
                case BUSINESS_GOODS_ISSUE_Reservation:
                    sql += " AND master_config_lo_doctype_docdetail.PostingType = '" + BUSINESS_REVERSEATION + "'";
                    break;
                case BUSINESS_GOODS_ISSUE_SO:
                    sql += @" AND master_config_lo_doctype_docdetail.PostingType IN ('" + BUSINESS_SALE_ORDER + "','" + BUSINESS_SALE_ORDER_CONSIGNMENTISSUE + @"')
                              AND master_config_lo_doctype_docdetail.UseProcessDO = 'False'";
                    break;
                case BUSINESS_GOODS_ISSUE_SO_CONSIGNMENT_ISSUE:
                    sql += " AND master_config_lo_doctype_docdetail.PostingType = '" + BUSINESS_SALE_ORDER_CONSIGNMENTISSUE + "'";
                    break;
                case BUSINESS_GOODS_ISSUE_MAT_DAC:
                    sql += " AND master_config_business.BusinessGroup = '" + BUSINESS_GOODS_RECEIPT + "'";
                    break;
                default: break;
            }

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getGIRefDocumentNo(string sid, string companycode, string gidoctype, string refdoctype, string refcustomer, string refyear)
        {
            string where = "";
            string postingtype = "";

            DataTable dtType = getLoBusinessObjectFromDocType(companycode, gidoctype);
            if (dtType != null && dtType.Rows.Count > 0)
            {
                postingtype = dtType.Rows[0]["PostingType"].ToString();
            }

            DataTable dt = new DataTable();
            switch (postingtype)
            {
                #region Goods Issue Ref SO
                case BUSINESS_GOODS_ISSUE_SO:
                    where = " AND (A.status='" + STATUS_SO_CODE_APPROVE + "' OR A.status='" + STATUS_SO_CODE_COMPLETED + "') and I.OrderQty-I.StkUn > 0 ";
                    where += Validation.CreateString(companycode, "A.companyCode");
                    where += Validation.CreateString(refdoctype, "A.Stypecode");
                    where += Validation.CreateString(refcustomer, "A.ShipToParty");
                    where += Validation.CreateString(refyear, "A.FiscalYear");
                    where += " AND F.CompleteDeliv = '" + ApplicationConstants.FALSE_STRING + "' and A.UseProcessDO='" + ApplicationConstants.FALSE_STRING + "'";
                    where += " AND B.CloseStatus='False' AND A.StypeCode in (select DocumentTypeCode from master_config_lo_doctype_docdetail where PostingType in('" + BUSINESS_SALE_ORDER + "','" + BUSINESS_SALE_ORDER_CONSIGNMENTISSUE + "')) ";

                    dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHJGISO001", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
                    break;
                #endregion
                #region Goods Issue SO Consignment Issue
                case BUSINESS_GOODS_ISSUE_SO_CONSIGNMENT_ISSUE:
                    where = " AND (A.status='" + STATUS_SO_CODE_APPROVE + "'" +
                            " OR A.status='" + STATUS_SO_CODE_COMPLETED + "') AND I.OrderQty-I.StkUn > 0 ";
                    where += Validation.CreateString(companycode, "A.companyCode");
                    where += Validation.CreateString(refdoctype, "A.Stypecode");
                    where += Validation.CreateString(refcustomer, "A.ShipToParty");
                    where += Validation.CreateString(refyear, "A.FiscalYear");
                    where += " AND F.CompleteDeliv = '" + ApplicationConstants.FALSE_STRING + "' AND A.UseProcessDO='" + ApplicationConstants.FALSE_STRING + "'";
                    where += " AND A.StypeCode in (SELECT DocumentTypeCode FROM master_config_lo_doctype_docdetail WHERE PostingType IN ('" + BUSINESS_SALE_ORDER_CONSIGNMENTISSUE + "')) ";

                    dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHJGISO001", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
                    break;
                #endregion
                #region Goods Issue Ref Reservation
                case BUSINESS_GOODS_ISSUE_Reservation:

                    where += Validation.CreateString(companycode, "A.companyCode");
                    where += Validation.CreateString(refdoctype, "A.DocumentType");
                    where += Validation.CreateString(refyear, "A.FiscalYear");

                    dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHJRESFORGI", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
                    break;
                #endregion
                #region Goods Issue Ref DO
                case BUSINESS_GOODS_ISSUE_DO:

                    where = " AND (A.Status NOT IN ('" + STATUS_DO_CODE_BLOCKED + "','" + STATUS_SO_CODE_CANCEL + "'))";
                    where += Validation.CreateString(companycode, "A.companyCode");
                    where += Validation.CreateString(refdoctype, "A.DocType");
                    where += Validation.CreateString(refcustomer, "A.ShipToParty");
                    where += Validation.CreateString(refyear, "A.FiscalYear");
                    where += " AND CASE B.UseBatchQty WHEN '" + ApplicationConstants.TRUE_STRING + "' THEN B.OrderQty ELSE F.StkUn END < B.ConfirmedQty ";

                    dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHJGIDO001", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);


                    break;
                #endregion
                #region Goods Issue Ref Mat Doc
                case BUSINESS_GOODS_ISSUE_MAT_DAC:

                    where += Validation.CreateString(companycode, "A.CompanyCode");
                    where += Validation.CreateString(refdoctype, "A.DocType");
                    where += Validation.CreateString(refyear, "A.FisicalYear");

                    dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHJGIMATDOC", "#where A.SID='" + ERPWAuthentication.SID + "' " + where);
                    break;
                #endregion
                default: break;
            }

            return dt;
        }

        public DataTable getGIAccountAssignment(string sid)
        {
            string sql = "SELECT * FROM mm_master_accountassigment where SID='" + sid + "' and Active='" + ApplicationConstants.TRUE_STRING + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }
        #endregion

        #region Incoming Invoice
        public DataTable getIncomingDocumentType(string sid, string companycode)
        {
            string sql = @"SELECT master_config_fi_doctype.DocumentTypeCode + ' : ' + master_config_fi_doctype.Description as Description2,* 
                           FROM master_config_fi_doctype 
                           LEFT OUTER JOIN master_config_fi_doctype_docdetail 
                           ON master_config_fi_doctype.sid = master_config_fi_doctype_docdetail.sid 
                           AND master_config_fi_doctype.DocumentTypeCode = master_config_fi_doctype_docdetail.DocumentTypeCode
                           AND master_config_fi_doctype.Companycode = master_config_fi_doctype_docdetail.companyCode
                           LEFT OUTER JOIN master_config_business 
                           ON master_config_fi_doctype_docdetail.postingType = master_config_business.BusinessCode
                           WHERE 1=1 
                           AND master_config_fi_doctype.SID ='" + sid + @"'
                           AND postingType IN ('" + BUSINESS_INCOMING + "','" + BUSINESS_CASHSALE + @"')
                           AND master_config_fi_doctype.companycode='" + companycode + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getIncomingBranchCode(string sid, string companycode)
        {
            string sql = @"SELECT A.BRANCHCODE AS BRANCHCODE,A.BRANCHNAME_TH AS BRANCHNAME_TH,A.BRANCHCODE + ' : ' + A.BRANCHNAME_TH as Description2,
                           A.COMPANYCODE AS COMPANYCODE,A.BranchID AS BranchTax 
                           FROM BRANCH_MASTER_GENERAL AS A
                           WHERE A.SID = '" + sid + "' AND A.COMPANYCODE = '" + companycode + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getIncomingBUArea(string sid)
        {
            string sql = @"SELECT a.BusinessAreaCode + ' : ' + a.Description as Description2,a.* 
                           FROM master_buarea as a
                           WHERE A.SID = '" + sid + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getIncomingCurrency(string sid)
        {
            string sql = @"SELECT a.CurrencyCode+ ' : ' + a.Description as Description2 ,a.*
                           FROM master_currencytype as a
                           WHERE A.SID = '" + sid + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getIncomingSpecialGL(string sid, string chartAccount, string condition)
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "SHJ000060", "#WHERE A.SID = '" + sid + "' AND A.chartcode = '" + chartAccount + "' " + condition);

            return dt;
        }

        public DataTable getIncomingCustomerBranch(string sid, string companycode, string bpcode)
        {
            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "master_customer_branch", "#WHERE A.SID = '" + sid + "' AND A.CompanyCode = '" + companycode + "' AND A.CustomerCode = '" + bpcode + "'");

            return dt;
        }

        public DataTable getIncomingCustomer(string sid, string companycode, string customercode)
        {
            string sql = @"SELECT * FROM master_customer 
                           WHERE A.SID = '" + sid + "' AND A.CompanyCode = '" + companycode + "' AND A.CustomerCode = '" + customercode + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getIncomingTaxData(string sid, string companycode, string fiscalyear, string documenttype, string docnumber, string lineItem)
        {
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.AppendLine("select a.*,b.* from t_fitax a with(nolock)");
            sqlcmd.AppendLine("inner join (");
            sqlcmd.AppendLine("	select aa.SID,aa.refCompany,aa.refFiscalYear,aa.refDocType,aa.refDocNumber,aa.refLineItem");
            sqlcmd.AppendLine("	,SUM(aa.taxamountloc) as cleartaxamountloc,SUM(aa.taxamountdoc) as cleartaxamountdoc");
            sqlcmd.AppendLine("	,SUM(aa.DocBaseAmtIndTax) as clearDocBaseAmtIndTax,SUM(aa.LocBaseAmtIndTax) as clearLocBaseAmtIndTax");
            sqlcmd.AppendLine("	,SUM(aa.taxbaseamountloc) as cleartaxbaseamountloc,SUM(aa.taxbaseamountdoc) as cleartaxbaseamountdoc");
            sqlcmd.AppendLine("	from t_fitax aa with(nolock)");
            sqlcmd.AppendLine("	where aa.DocStatus NOT IN ('" + FINANCIAL_DOCSTATUS_REVERSE + "','" + FINANCIAL_DOCSTATUS_REVERSE_PARK + "')");
            sqlcmd.AppendLine("and aa.refDocNumber='" + docnumber + "'");
            sqlcmd.AppendLine("and aa.refCompany='" + companycode + "'");
            sqlcmd.AppendLine("and aa.refFiscalYear='" + fiscalyear + "'");
            sqlcmd.AppendLine("and aa.refDocType='" + documenttype + "'");
            sqlcmd.AppendLine("	group by aa.SID,aa.refCompany,aa.refFiscalYear,aa.refDocType,aa.refDocNumber,aa.refLineItem");
            sqlcmd.AppendLine(") b on a.SID=b.SID and a.CompanyCode=b.refCompany and a.fiscalyear=b.refFiscalYear and a.documenttype=b.refDocType and a.AccountingDocNumber=b.refDocNumber and a.baseitemnumber=b.refLineItem");
            sqlcmd.AppendLine("where a.SID='" + sid + "'");
            sqlcmd.AppendLine("and a.AccountingDocNumber='" + docnumber + "'");
            sqlcmd.AppendLine("and a.CompanyCode='" + companycode + "'");
            sqlcmd.AppendLine("and a.fiscalyear='" + fiscalyear + "'");
            sqlcmd.AppendLine("and a.documenttype='" + documenttype + "'");
            sqlcmd.AppendLine("and a.baseitemnumber='" + lineItem + "'");
            sqlcmd.AppendLine("and a.DocStatus NOT IN ('" + FINANCIAL_DOCSTATUS_REVERSE + "','" + FINANCIAL_DOCSTATUS_REVERSE_PARK + "')");

            DataTable dt = DB.selectDataFocusone(sqlcmd.ToString());

            return dt;
        }

        public DataTable getPaymentMethodCash(string sid, string companycode)
        {
            string where = "#WHERE A.SID = '" + sid + "' AND A.FiType = '" + BUSINESS_INCOMING + "' AND A.CompanyCode='" + companycode + "'";

            DataTable dt = WSHelper.GetSearchData(ERPWAuthentication.SID, "master_payment_method_cash", where);

            return dt;
        }

        public DataTable getPaymentMethodBankTransfer(string sid, string companycode)
        {
            string where = "#WHERE A.SID = '" + sid + "' AND A.FiType = '" + BUSINESS_INCOMING + "' AND A.CompanyCode='" + companycode + "'";

            DataTable dt = WSHelper.GetSearchData(sid, "master_payment_method_transfer", where);

            return dt;
        }

        public string getChartOfAccount(string sid, string companycode)
        {
            DataTable dt = DB.selectDataFocusone("select * from master_company_chart where 1=1 and SID='" + sid + "' and companycode='" + companycode + "'");

            string result = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["chartAccts"].ToString();
            }

            return result;
        }

        public string getCurrencyByCompany(string sid, string companycode)
        {
            DataTable dt = DB.selectDataFocusone("select * from master_company_detail_basic_initial where 1=1 and SID='" + sid + "' and companycode='" + companycode + "'");

            string result = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["CurrencyCode"].ToString();
            }

            return result;
        }

        public DataTable getIncomingDocumentNo(string sid, string companycode, string doctype, string year, string accdoc, string cleardoc)
        {
            string condition = "";

            if (!string.IsNullOrEmpty(accdoc))
            {
                condition += " AND AccountingDocNumber LIKE '" + accdoc + "%'";
            }
            if (!string.IsNullOrEmpty(doctype))
            {
                condition += " AND DocumentType = '" + doctype + "'";
            }
            if (!string.IsNullOrEmpty(cleardoc))
            {
                condition += " AND ClearingDocNumber LIKE '" + cleardoc + "%'";
            }


            string sql = @"SELECT * FROM t_fiarclear 
                           WHERE SID = '" + sid + @"' 
                           AND CompanyCode = '" + companycode + @"' 
                           AND fiscalyear = '" + year + "' " + condition;

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public bool isCashSale(string sid, string companycode, string doctype)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(doctype))
            {
                condition = " AND DocumentTypeCode = '" + doctype + "'";
            }


            string sql = @"SELECT * FROM master_config_fi_doctype_docdetail 
                           WHERE SID = '" + sid + @"' 
                           AND CompanyCode = '" + companycode + @"' " + condition;

            DataTable dt = DB.selectDataFocusone(sql);

            bool result = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["isCashSale"].ToString().Equals("True"))
                {
                    result = true;
                }
            }

            return result;
        }

        public DataTable getBillingDepositDocument(string sid, string companycode, string sodocnumber, string fiscalyear)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                condition = " AND a.FiscalYear = '" + fiscalyear + "'";
            }
            if (!string.IsNullOrEmpty(sodocnumber))
            {
                condition = " AND a.SaleDocument = '" + sodocnumber + "'";
            }

            string sql = @"SELECT DISTINCT a.SaleDocument,b.SaleDocument AS BillingNumber,e.AccountingDocNumber AS IncomingNumber
                           FROM sd_so_header a
                           INNER JOIN bl_item b
                           ON a.SID = b.SID AND a.FiscalYear = b.FiscalYearRef AND a.Stypecode = b.RefDocType AND a.SaleDocument = b.SaleQtRef
                           INNER JOIN bl_header c
                           ON b.SID = c.SID AND b.FiscalYear = c.FiscalYear AND b.SaleDocument = c.SaleDocument
                           INNER JOIN master_config_lo_doctype_docdetail d
                           ON c.SID = d.SID AND c.CompanyCode = d.CompanyCode AND c.Stypecode = d.DocumentTypeCode
                           INNER JOIN t_fiarclear e
                           ON c.SID = e.SID AND c.ARCompany = e.InvCompany AND c.ARFiscalYear = e.InvFiscalYear 
                           AND c.ARDocumentType = e.InvDocType AND c.AccountingDoc = e.ClearingDocNumber
                           WHERE a.SID = '" + sid + "' AND a.companyCode = '" + companycode + "' AND d.PostingType = 'BILLDEPOSIT'" + condition;

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getIncomingRefSODocument(string sid, string companycode, string sodocnumber, string fiscalyear, string doctype)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(sodocnumber))
            {
                condition = " AND refdocument = '" + sodocnumber + "'";
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                condition += " AND RefFiscalYear = '" + fiscalyear + "'";
            }
            if (!string.IsNullOrEmpty(doctype))
            {
                condition += " AND RefDocType = '" + doctype + "'";
            }

            string sql = @"select refdocument,RefDocType,RefCompany,RefFiscalYear,* 
                           from bank_incomingdoc_header 
                           where SID ='" + sid + "' AND RefCompany='" + companycode + "' AND docstatus <> 'R' " + condition;

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getFIBusinessObjectFromDocType(string p_companyCode, string p_docType)
        {
            string sql = @"select * from master_config_fi_doctype_docdetail 
                           where sid='" + ERPWAuthentication.SID + "' and companyCode='" + p_companyCode + "' and DocumentTypeCode='" + p_docType + "'";

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public DataTable getSOMappingIncoming(string sid, string companycode, string sonumber, string sodoctype, string soyear, string incdocno, string incdoctype, string incfiscalyear)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(sonumber))
            {
                condition += " AND SONumber='" + sonumber + "'";
            }
            if (!string.IsNullOrEmpty(sodoctype))
            {
                condition += " AND SODocType='" + sodoctype + "'";
            }
            if (!string.IsNullOrEmpty(soyear))
            {
                condition += " AND SOYear='" + soyear + "'";
            }
            if (!string.IsNullOrEmpty(incdocno))
            {
                condition += " AND IncomingNumber='" + incdocno + "'";
            }
            if (!string.IsNullOrEmpty(incdoctype))
            {
                condition += " AND IncomingType='" + incdoctype + "'";
            }
            if (!string.IsNullOrEmpty(incfiscalyear))
            {
                condition += " AND IncomingYear='" + incfiscalyear + "'";
            }

            string sql = @"SELECT * FROM LIKE_CRM_SO_MAPPING_INCOMING 
                           WHERE SID='" + sid + @"' AND CompanyCode='" + companycode + @"' " + condition;

            DataSet ds = serviceFobgp.selectData(sql);
            return ds.Tables[0];
        }

        public void deleteSOMappingIncoming(string sid, string companycode, string sonumber, string sodoctype, string soyear, string incdocno, string incdoctype, string incfiscalyear)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(sonumber))
            {
                condition += " AND SONumber='" + sonumber + "'";
            }
            if (!string.IsNullOrEmpty(sodoctype))
            {
                condition += " AND SODocType='" + sodoctype + "'";
            }
            if (!string.IsNullOrEmpty(soyear))
            {
                condition += " AND SOYear='" + soyear + "'";
            }
            if (!string.IsNullOrEmpty(incdocno))
            {
                condition += " AND IncomingNumber='" + incdocno + "'";
            }
            if (!string.IsNullOrEmpty(incdoctype))
            {
                condition += " AND IncomingType='" + incdoctype + "'";
            }
            if (!string.IsNullOrEmpty(incfiscalyear))
            {
                condition += " AND IncomingYear='" + incfiscalyear + "'";
            }

            string sql = @"DELETE LIKE_CRM_SO_MAPPING_INCOMING 
                           WHERE SID='" + sid + @"' AND CompanyCode='" + companycode + @"' " + condition;

            DB.executeSQLForFocusone(sql);

            string sqlBlock = @"update b set b.IsBlock='True', b.UnBlockBy='', b.UnBlockDate='', b.UnBlockTime=''
                                from sd_so_header a
                                inner join sd_so_header_block b
                                on a.sid =  b.sid and a.Objectid= b.Objectid
                                where a.SID = '" + sid + "' and a.SaleDocument='" + sonumber + @"' 
                                and a.FiscalYear='" + soyear + @"' 
                                and b.blockCode ='11' and b.IsBlock='Flase';";

            sqlBlock += @"update sd_so_header set
                          Status = '05', updated_By = '" + ERPWAuthentication.EmployeeCode + "', updated_on = '" + Validation.getCurrentServerStringDateTime() + @"'
                          where SID ='" + sid + "' and saleDocument = '" + sonumber + @"'
                          and FiscalYear = '" + soyear + "' and CompanyCode = '" + companycode + @"';";

            DB.executeSQLForFocusone(sqlBlock);
        }

        public DataTable getDOQtyBySO(string sid, string sonumber, string soyear)
        {
            string sql = @"SELECT SaleDocument, FiscalYear, SUM(ISNULL(DOQty, 0)) as DOQty
                           FROM sd_so_item WHERE SID='" + sid + "' AND SaleDocument = '" + sonumber + "' and FiscalYear = '" + soyear + @"'
                           GROUP BY SaleDocument, FiscalYear";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }
        #endregion

        public DataTable getMaterialCode(string sid, string companycode, string matcode)
        {
            string sql = @"SELECT a.MaterialCode,a.MaterialDesc,a.Plant,a.PlantDesc,a.Storage,a.StorageDesc,a.Rackbin,
                            a.RackbinDesc,a.QOH,a.UOM,a.UomDesc,a.CompanyCode
                            FROM (
                            SELECT     SID, ISNULL(CompanyCode, '') AS CompanyCode, ItmNumber AS MaterialCode, ItmDescription AS MaterialDesc, ISNULL(PLANT, '') AS PLANT, ISNULL(PLANTNAME1, '') AS PlantDesc,
                            ISNULL(STORAGELOCATION, '') AS Storage, ISNULL(StoreName, '') AS StorageDesc, ISNULL(RACKBIN, '') AS RACKBIN, ISNULL(Description, '') AS RackbinDesc, SUM(Q_avaliable) AS QOH, 
                            ISNULL(UOM, '') AS UOM, ISNULL(UDESC, '') AS UomDesc, ISNULL(Name, '') AS CompanyDesc, ItmType, ItmGroup, IsQCPlant
                            FROM         (SELECT     items.SID, plantCom.COMPANYCODE AS CompanyCode, items.ItmNumber, CASE ISNULL(a_3.UOM, '') WHEN '' THEN ISNULL(itemgen.BaseUOM, '') ELSE ISNULL(a_3.UOM, '')
                            END AS UOM, matorg.PLANTCODE AS PLANT, matorg.STORAGELOCCODE AS STORAGELOCATION, matorg.RACKBIN, items.ItmDescription, plant.PLANTNAME1, stor.StoreName,
                            rack.Description, CASE ISNULL(uom.UDESC, '') WHEN '' THEN ISNULL(uom2.UDESC, '') ELSE ISNULL(uom.UDESC, '') END AS UDESC,
                            ISNULL(CASE a_3.DCINDICATOR WHEN 'Credit' THEN (a_3.Q_avaliable * - 1) ELSE a_3.Q_avaliable END, 0) AS Q_avaliable, com.Name, items.ItmType, items.ItmGroup,
                            plant.IsQCPlant
                            FROM          dbo.master_mm_items AS items INNER JOIN
                            dbo.mm_master_material_org AS matorg ON items.SID = matorg.SID AND items.ItmNumber = matorg.MATNR INNER JOIN
                            dbo.master_mm_item_general AS itemgen ON items.SID = itemgen.SID AND items.ItmNumber = itemgen.ItmNumber LEFT OUTER JOIN
                            dbo.mm_conf_define_plant AS plant ON matorg.SID = plant.SID AND matorg.PLANTCODE = plant.PLANTCODE LEFT OUTER JOIN
                            dbo.mm_conf_assign_plant_to_company AS plantCom ON plant.SID = plantCom.SID AND plant.PLANTCODE = plantCom.PLANTCODE LEFT OUTER JOIN
                            dbo.mm_conf_define_storagelocation AS stor ON matorg.SID = stor.SID AND matorg.PLANTCODE = stor.PLANTCODE AND
                            matorg.STORAGELOCCODE = stor.STORAGELOCCODE LEFT OUTER JOIN
                            dbo.mm_conf_define_rackbin AS rack ON matorg.SID = rack.SID AND matorg.PLANTCODE = rack.PLANTCODE AND matorg.STORAGELOCCODE = rack.STORAGELOCCODE AND
                            matorg.RACKBIN = rack.RACKBIN LEFT OUTER JOIN
                            (SELECT     SID, ITMNUMBER, PLANT, STORAGELOCATION, RACKBIN, UOM, DCINDICATOR, SUM(Q_AVALIABLE) AS Q_avaliable
                            FROM          dbo.mlt0_available
                            GROUP BY SID, ITMNUMBER, PLANT, STORAGELOCATION, RACKBIN, UOM, DCINDICATOR) AS a_3 ON a_3.SID = matorg.SID AND a_3.ITMNUMBER = matorg.MATNR AND
                            a_3.PLANT = matorg.PLANTCODE AND a_3.STORAGELOCATION = matorg.STORAGELOCCODE AND a_3.RACKBIN = matorg.RACKBIN AND a_3.UOM = itemgen.BaseUOM LEFT OUTER JOIN
                            dbo.master_mm_weight_setup AS uom ON a_3.SID = uom.SID AND a_3.UOM = uom.UCODE LEFT OUTER JOIN
                            dbo.master_mm_weight_setup AS uom2 ON itemgen.SID = uom2.SID AND itemgen.BaseUOM = uom2.UCODE LEFT OUTER JOIN
                            dbo.master_company AS com ON plantCom.SID = com.SID AND plantCom.COMPANYCODE = com.ID
                            UNION ALL
                            SELECT     items.SID, plantCom.COMPANYCODE AS CompanyCode, items.ItmNumber, CASE ISNULL(a_2.UOM, '') WHEN '' THEN ISNULL(itemgen.BaseUOM, '') ELSE ISNULL(a_2.UOM, '')
                            END AS UOM, matorg.PLANTCODE AS PLANT, matorg.STORAGELOCCODE AS STORAGELOCATION, matorg.RACKBIN, items.ItmDescription, plant.PLANTNAME1, stor.StoreName,
                            rack.Description, CASE ISNULL(uom.UDESC, '') WHEN '' THEN ISNULL(uom2.UDESC, '') ELSE ISNULL(uom.UDESC, '') END AS UDESC,
                            ISNULL(CASE a_2.DCINDICATOR WHEN 'Credit' THEN (a_2.Q_avaliable * - 1) ELSE a_2.Q_avaliable END, 0) AS Q_avaliable, com.Name, items.ItmType, items.ItmGroup,
                            plant.IsQCPlant
                            FROM         dbo.master_mm_items AS items INNER JOIN
                            dbo.mm_master_material_org AS matorg ON items.SID = matorg.SID AND items.ItmNumber = matorg.MATNR INNER JOIN
                            dbo.master_mm_item_general AS itemgen ON items.SID = itemgen.SID AND items.ItmNumber = itemgen.ItmNumber LEFT OUTER JOIN
                            dbo.mm_conf_define_plant AS plant ON matorg.SID = plant.SID AND matorg.PLANTCODE = plant.PLANTCODE LEFT OUTER JOIN
                            dbo.mm_conf_assign_plant_to_company AS plantCom ON plant.SID = plantCom.SID AND plant.PLANTCODE = plantCom.PLANTCODE LEFT OUTER JOIN
                            dbo.mm_conf_define_storagelocation AS stor ON matorg.SID = stor.SID AND matorg.PLANTCODE = stor.PLANTCODE AND
                            matorg.STORAGELOCCODE = stor.STORAGELOCCODE LEFT OUTER JOIN
                            dbo.mm_conf_define_rackbin AS rack ON matorg.SID = rack.SID AND matorg.PLANTCODE = rack.PLANTCODE AND matorg.STORAGELOCCODE = rack.STORAGELOCCODE AND
                            matorg.RACKBIN = rack.RACKBIN LEFT OUTER JOIN
                            (SELECT     b.SID, b.ITMNUMBER, b.PLANT, b.STORAGELOCATION, b.RACKBIN, b.UOM, b.DCINDICATOR, SUM(b.Q_AVALIABLE) AS Q_avaliable
                            FROM          dbo.buffer_work_process AS a INNER JOIN
                            dbo.BF_MLT0_AVAILABLE AS b ON a.BufferID = b.BufferID AND a.ProcessIndex = b.ProcessIndex
                            WHERE      (a.CompleateT = 'X') AND (a.DeleteT = '')
                            GROUP BY b.SID, b.ITMNUMBER, b.PLANT, b.STORAGELOCATION, b.RACKBIN, b.UOM, b.DCINDICATOR) AS a_2 ON a_2.SID = matorg.SID AND a_2.ITMNUMBER = matorg.MATNR AND
                            a_2.PLANT = matorg.PLANTCODE AND a_2.STORAGELOCATION = matorg.STORAGELOCCODE AND a_2.RACKBIN = matorg.RACKBIN AND a_2.UOM = itemgen.BaseUOM LEFT OUTER JOIN
                            dbo.master_mm_weight_setup AS uom ON a_2.SID = uom.SID AND a_2.UOM = uom.UCODE LEFT OUTER JOIN
                            dbo.master_mm_weight_setup AS uom2 ON itemgen.SID = uom2.SID AND itemgen.BaseUOM = uom2.UCODE LEFT OUTER JOIN
                            dbo.master_company AS com ON plantCom.SID = com.SID AND plantCom.COMPANYCODE = com.ID) AS a_1
                            GROUP BY SID, CompanyCode, ItmNumber, ItmDescription, PLANT, PLANTNAME1, STORAGELOCATION, StoreName, RACKBIN, Description, UOM, UDESC, Name, ItmType, ItmGroup, IsQCPlant
                            ) a
                             WHERE SID = '" + sid + "' AND MaterialCode LIKE '" + matcode.Trim() + @"%' AND CompanyCode='" + companycode + @"'
                            ORDER BY a.CompanyCode,a.MaterialCode,a.Plant,a.Storage,a.Rackbin";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAdditionalType(string SID, string CompanyCode)
        {
            return getAdditionalType(SID, CompanyCode, "");
        }

        public DataTable getAdditionalType(string SID, string CompanyCode, string AdditionalType)
        {
            string sql = @"select * from fi_mapping_additional_incoming_gl where SID='" + SID + "' and CompanyCode='" + CompanyCode + "'";

            if (!string.IsNullOrEmpty(AdditionalType))
            {
                sql += " and AdditionalType='" + AdditionalType + "'";
            }

            return DB.selectDataFocusone(sql);
        }

        public DataTable getAdditionalDocCurrency(string SID)
        {
            string sql = @"select * from master_currencytype where SID='" + SID + "'";

            return DB.selectDataFocusone(sql);
        }

        public DataTable getOrderTypeSaleReturn(string SID, string CompanyCode)
        {
            string sql = @"SELECT A.documenttypecode AS documenttypecode,A.documenttypecode + ' : ' + A.description AS description,B.PostingType AS PostingType,C.Module AS Module 
                            FROM master_config_lo_doctype AS A 
                            LEFT OUTER JOIN master_config_lo_doctype_docdetail AS B
                            ON A.SID = B.SID AND A.DocumentTypeCode = B.DocumentTypeCode AND A.CompanyCode = B.companyCode  
                            LEFT OUTER JOIN master_config_business AS C
                            ON B.PostingType = C.BusinessCode 
                            where A.SID='" + SID + "' and B.PostingType = 'SR' and (B.CompanyCode='" + CompanyCode + "' or B.CompanyCode='*')";

            return DB.selectDataFocusone(sql);
        }

        public DataTable getSaleReturnDocNumber(string SID, string CompanyCode, string FiscalYear, string DocType)
        {
            string sql = @"SELECT a.SaleDocument AS SaleDocument,a.Stypecode AS STypeCode,a.FiscalYear AS FiscalYear
                            ,a.companyCode AS CompanyCode,a.ShipToParty AS ShipToParty,a.CustomerName AS ShipToDesc
                            ,a.BPBranch AS BPBranch,a.BPBranchName AS BPBranchName,a.Sorgcode AS Sorgcode,a.SORGNAME AS SORGNAME
                            ,a.SDCode AS SDCode,a.SHCANNELNAME AS SHCANNELNAME,a.Sdivcode AS Sdivcode,a.SDIVNAME AS SDIVNAME
                            ,a.Soffcode AS Soffcode,a.SOFFICENAME AS SOFFICENAME,a.Sgroupcode AS Sgroupcode,a.SGROUPNAME AS SGROUPNAME
                            ,a.Status AS Status,a.Description AS Description,a.DocDate AS DocDate,a.SoldToParty AS SoldToParty
                            ,a.CustomerName AS CustomerName,a.ShipTo AS ShipTo,a.NetValue AS NetValue,a.CurrencyCode AS CurrencyCode 
                            FROM V_GET_LIST_ORDER AS a 
                            where a.CompanyCode='" + CompanyCode + "' and a.Stypecode in (select DocumentTypeCode from master_config_lo_doctype_docdetail where sid = '" + SID + @"' and PostingType = 'SR') ";

            if (!string.IsNullOrEmpty(FiscalYear))
            {
                sql += " and a.FiscalYear ='" + FiscalYear + "' ";
            }

            if (!string.IsNullOrEmpty(DocType))
            {
                sql += " and a.Stypecode ='" + DocType + "'";
            }

            sql += " ORDER BY a.companyCode ,a.FiscalYear ,a.Stypecode ,a.SaleDocument ";

            return DB.selectDataFocusone(sql);
        }

        public DataTable getSaleOrder(string SID, string FiscalYear)
        {
            string sql = @"SELECT A.Stypecode AS SaleDocumentTypeCode,B.Description AS SaleDocumentTypeName,A.SaleDocument AS SaleOrderNumber,A.DocDate AS DocumentDate,D.RegDelivDate AS DelivDate,C.Description AS DocumentStatus,A.SoldToDesc AS SoldToDescription,A.ShipToDesc AS ShipToDescription,E.Description AS ShipToBranchName,A.BillToDesc AS BillToDescription,A.NetValue AS NetValue,A.CurrencyCode AS CurrencyCode,D.PmntTermCode AS PaymentTerm,D.SaleRepresent AS SaleRepresentative,A.HeaderText AS HeaderText,A.FiscalYear AS FiscalYear 
                            FROM sd_so_header AS A 
                            LEFT OUTER JOIN master_config_lo_doctype AS B
                            ON A.SID = B.SID AND A.Stypecode = B.DocumentTypeCode AND A.companyCode = B.companyCode  
                            LEFT OUTER JOIN sd_master_status AS C
                            ON A.SID = C.SID AND A.Status = C.Code  
                            INNER JOIN sd_so_header_sale AS D
                            ON A.SID = D.SID AND A.ObjectID = D.ObjectID  
                            LEFT OUTER JOIN master_customer_branch AS E
                            ON A.SID = E.sid AND A.companyCode = E.CompanyCode AND A.ShipToParty = E.CustomerCode 

                            Where 1=1 and a.FiscalYear = '" + FiscalYear + @"'  and ( a.Status='02' or a.Status='07' ) 
                            and a.Stypecode in (select DocumentTypeCode from master_config_lo_doctype_docdetail where sid = '" + SID + @"' 
                            and PostingType = 'SO') 
                            and a.SaleDocument in (

                            select b.saleDocument  from sd_so_item b 
                            inner join sd_so_item_quantity c  
                            on b.SID = c.SID and b.ObjectID = c.ObjectID 
                            inner join sd_so_header d 
                            on b.SID = d.SID 
                            and b.SaleDocument = d.SaleDocument 
                            and b.FiscalYear = d.FiscalYear 
                            inner join master_config_lo_doctype_docdetail e 
                            on d.SID = e.SID and d.companyCode = e.companyCode 
                            and d.Stypecode = e.DocumentTypeCode 
                            where b.sid='" + SID + @"' and (c.StkUn - b.ReturnQty) > 0
                            and isnull(b.RejectCode,'')='' 
                            and  1=
                            case  when e.SaleReturnWithoutGI='True' then 1 else 
                            case when  c.StkUn > 0 then 1 else 0 end
                             end 
                             )
 
                            ORDER BY A.FiscalYear ,A.Stypecode ,A.SaleDocument";

            return DB.selectDataFocusone(sql);
        }

        public DataTable getSaleOrderItem(string SID, string FiscalYear)
        {
            string sql = @"SELECT A.Stypecode AS SaleDocumentTypeCode,B.Description AS SaleDocumentTypeName,A.SaleDocument AS SaleOrderNumber,A.DocDate AS DocumentDate,D.RegDelivDate AS DelivDate,C.Description AS DocumentStatus,A.SoldToDesc AS SoldToDescription,A.ShipToDesc AS ShipToDescription,E.Description AS ShipToBranchName,A.BillToDesc AS BillToDescription,A.NetValue AS NetValue,A.CurrencyCode AS CurrencyCode,D.PmntTermCode AS PaymentTerm,D.SaleRepresent AS SaleRepresentative,A.HeaderText AS HeaderText,A.FiscalYear AS FiscalYear 
                            ,soItem.MeterialCode as MaterialCode,soItem.Description as MaterialName,soItem.OrderQty,soItem.SaleUnit
                            FROM sd_so_header AS A 
                            INNER JOIN sd_so_item soItem
							on A.SID = soItem.SID and A.SaleDocument = soItem.SaleDocument and A.FiscalYear = soItem.FiscalYear 
                            LEFT OUTER JOIN master_config_lo_doctype AS B
                            ON A.SID = B.SID AND A.Stypecode = B.DocumentTypeCode AND A.companyCode = B.companyCode  
                            LEFT OUTER JOIN sd_master_status AS C
                            ON A.SID = C.SID AND A.Status = C.Code  
                            INNER JOIN sd_so_header_sale AS D
                            ON A.SID = D.SID AND A.ObjectID = D.ObjectID  
                            LEFT OUTER JOIN master_customer_branch AS E
                            ON A.SID = E.sid AND A.companyCode = E.CompanyCode AND A.ShipToParty = E.CustomerCode 

                            Where 1=1 and a.FiscalYear = '" + FiscalYear + @"'  and ( a.Status='02' or a.Status='07' ) 
                            and a.Stypecode in (select DocumentTypeCode from master_config_lo_doctype_docdetail where sid = '" + SID + @"' 
                            and PostingType = 'SO') 
                            and a.SaleDocument in (

                            select b.saleDocument  from sd_so_item b 
                            inner join sd_so_item_quantity c  
                            on b.SID = c.SID and b.ObjectID = c.ObjectID 
                            inner join sd_so_header d 
                            on b.SID = d.SID 
                            and b.SaleDocument = d.SaleDocument 
                            and b.FiscalYear = d.FiscalYear 
                            inner join master_config_lo_doctype_docdetail e 
                            on d.SID = e.SID and d.companyCode = e.companyCode 
                            and d.Stypecode = e.DocumentTypeCode 
                            where b.sid='" + SID + @"' and (c.StkUn - b.ReturnQty) > 0
                            and isnull(b.RejectCode,'')='' 
                            and  1=
                            case  when e.SaleReturnWithoutGI='True' then 1 else 
                            case when  c.StkUn > 0 then 1 else 0 end
                             end 
                             )
 
                            ORDER BY A.FiscalYear ,A.Stypecode ,A.SaleDocument";

            return DB.selectDataFocusone(sql);
        }

        public string LookUpTable(string columnName, string tableName, string whereCondition)
        {
            string sql = "SELECT " + columnName + " FROM " + tableName + " " + whereCondition;

            DataTable dt = DB.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][columnName].ToString();
            }

            return "";
        }

        public bool GetExrateTypeManual(string exchangeRateType)
        {
            string isManual = LookUpTable("ismanual", "master_exchangeratetype", "WHERE SID='" + ERPWAuthentication.SID + "' AND ExchangeRateType='" + exchangeRateType + "'");

            if (isManual != "")
            {
                return Convert.ToBoolean(isManual);
            }

            return false;
        }

        public double GetExchangeRateValue(string p_sessionid, string p_exratedate, string p_doccurency, string p_loccurrency, string p_exratetype)
        {
            double res = 1;

            if (p_exratedate.Length > 8)
            {
                p_exratedate = Validation.Convert2DateDB(p_exratedate);
            }

            Object[] objParam = new Object[] { "2900297", p_sessionid, p_exratedate, p_doccurency, p_loccurrency, p_exratetype };
            DataSet[] objDataset = new DataSet[] { };
            Object objReturn = icmUtil.ICMPrimitiveInvoke(objParam, objDataset);

            if (objReturn != null && objReturn.ToString() != "")
            {
                double.TryParse(objReturn.ToString(), out res);
            }

            if (res == 0)
            {
                res = 1;
            }

            return res;
        }

        public DataTable GetExchangeRateType(string sid)
        {
            string sql = "SELECT * FROM master_exchangeratetype WHERE SID='" + sid + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable GetSelectedValueDetail(string sid, string code)
        {
            string sql = "select * from master_conf_selectedvalue_detail where SID='" + sid + "' and Code='" + code + "' order by DetailCode";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public bool IsRefBilling(string sid, string docNumber, string fiscalYear)
        {
            string sql = "select * from bl_item where SID='" + sid + "' and SaleQtRef='" + docNumber + "' and FiscalYearRef='" + fiscalYear + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool CheckDORefSO(string sid, string docType, string docNumber, string fiscalYear)
        {
            string sql = "select * from sd_do_item where SID='" + sid + "' and SODocType='" + docType + "' and SONumber='" + docNumber + "' and SOFiscalYear='" + fiscalYear + "'";

            DataTable dt = DB.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }       

        private DataTable GetSOPropertySelectedValue(string sid, string propertyCode)
        {
            string sql = @"select c.*
                           from master_conf_properties a
                           inner join master_conf_selectedvalue b
                           on a.SID = b.SID and a.SelectedCode = b.Code
                           inner join master_conf_selectedvalue_detail c
                           on b.SID = c.SID and b.Code = c.Code
                           where a.SID='" + sid + "' and a.xType='" + BUSINESS_SALE_ORDER + "' and a.PropertiesCode='" + propertyCode + @"'
                           order by c.DetailCode";

            DataTable dt = DB.selectDataFocusone(sql);

            return dt;
        }

        public DataTable GetSaleOrderTransformation(string sid)
        {
            return GetSOPropertySelectedValue(sid, PROPERTY_CODE_TRANSFORMATION);
        }

        public DataTable GetSaleOrderAdditionalTerms(string sid)
        {
            return GetSOPropertySelectedValue(sid, PROPERTY_CODE_ADDITIONALTERM);
        }
    }
}