using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService.ICMUtils;
using Newtonsoft.Json;
using SNA.Lib.POS.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class CustomerMappingCIAPI : System.Web.UI.Page
    {
        ServiceWeb.Service.EquipmentService lib = new ServiceWeb.Service.EquipmentService();

        private ICMUtils icmUtil = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private DataTable dtEquipmentStatus_;
        private DataTable dtEquipmentStatus
        {
            get
            {
                if (dtEquipmentStatus_ == null)
                {
                    Object[] objParam = new Object[] { "0800066", POSDocumentHelper.getSessionId(SID, UserName) };
                    DataSet[] ds = new DataSet[] { };
                    DataSet objReturn = icmUtil.ICMDataSetInvoke(objParam, ds);
                    if (objReturn.Tables.Count > 0)
                    {
                        dtEquipmentStatus_ = objReturn.Tables[0].DefaultView.ToTable(true, "StatusCode", "StatusName");
                    }
                    else
                    {
                        dtEquipmentStatus_ = new DataTable();
                    }
                }

                return dtEquipmentStatus_;
            }
        }

        private string _SID;
        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = ERPWAuthentication.SID;
                return _SID;
            }
        }

        private string _CompanyCode;
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                return _CompanyCode;
            }
        }

        private string _EmployeeCode;
        private string EmployeeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_EmployeeCode))
                    _EmployeeCode = ERPWAuthentication.EmployeeCode;
                return _EmployeeCode;
            }
        }

        private string _UserName;
        private string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_UserName))
                    _UserName = ERPWAuthentication.UserName;
                return _UserName;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string CustomerCode = Request["CustomerCode"];


            DataTable dt = lib.getListCIOfCustomer(SID, CompanyCode, CustomerCode, "01");
            dt.Columns.Add("StatusDesc");
            dt.Columns.Add("CategoryDesc");
            foreach (DataRow dtr in dt.Rows)
            {

                dtr["StatusDesc"] = TranslaterEMStatus(dtr["Status"].ToString());

                dtr["CategoryDesc"] = TranslaterEMCategory(dtr["CategoryCode"].ToString());
            }

            Response.Write(JsonConvert.SerializeObject(dt));
        }

        private string TranslaterEMStatus(string status)
        {
            DataRow[] dtr = dtEquipmentStatus.Select("StatusCode = '" + status + "'");
            if (dtr.Count() > 0)
            {
                return dtr[0]["StatusName"].ToString();
            }
            return "";
        }


        private string TranslaterEMCategory(string code)
        {
            if (code == "00")
            {
                return "Main Configuration Item";
            }
            //if (code == "01") {
            //    return "Sub Configuration Item";
            //}
            if (code == "02")
            {
                return "Virtual Configuration Item";
            }
            return code;
        }
    }
}