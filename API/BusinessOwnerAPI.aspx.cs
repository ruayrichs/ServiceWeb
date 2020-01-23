using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API
{
    public partial class BusinessOwnerAPI : System.Web.UI.Page
    {
        private string _SEQ { get; set; }
        private string _SID { get; set; }
        private string _CompanyCode { get; set; }

        private string SEQ { get
            {
                if (string.IsNullOrEmpty(_SEQ))
                {
                    _SEQ = Request.QueryString["SEQ"];
                }
                return _SEQ;
            } }
        private string SID { get
            {
                if (string.IsNullOrEmpty(_SID))
                {
                    _SID = ERPWAuthentication.SID;
                }
                return _SID;
            } }
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                {
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                }
                return _CompanyCode;
            }
        }

        private BusinessOwnerLib businessOwnerLib = new BusinessOwnerLib();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsCallback)
            {
                init();
            }
        }

        protected void init()
        {
            List<BusinessOwnerEN> lists = businessOwnerLib.readBySEQ(SID,CompanyCode,SEQ);
            var json = new JavaScriptSerializer().Serialize(lists);
            Response.Write(json);
        }
    }
}