using ERPW.Lib.Authentication;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.widget.usercontrol
{
    public partial class SmartSearchEmployee : System.Web.UI.UserControl
    {
        public string SelectedCode
        {
            get
            {
                return txtResultCode.Text;
            }
            set
            {
                txtResultCode.Text = value;
            }
        }
        public string SelectedValue
        {
            get
            {
                return txtResultValue.Text;
            }
            set
            {
                txtResultValue.Text = value;
            }
        }

        private bool _unLoadOnPostBack;
        public bool unLoadOnPostBack
        {
            get { return _unLoadOnPostBack; }
            set { _unLoadOnPostBack = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!_unLoadOnPostBack)
                {
                    bindData();
                }

                txtResultCode.CssClass = "input-group-main-employee-code-" + ID;
                txtResultValue.CssClass = "input-group-main-employee-value-" + ID;
            }
        }
        private void bindData()
        {
            DataTable dt = AssetService.getInstance().SearchEmployee(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode, 
                ""
            );
            rptSearchMainDelegate.DataSource = dt;
            rptSearchMainDelegate.DataBind();

        }

        public void reBindData()
        {
            DataTable dt = AssetService.getInstance().getEmployee(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode
            );

            rptSearchMainDelegate.DataSource = dt;
            rptSearchMainDelegate.DataBind();
            ClientService.DoJavascript("rebind" + ID + "();");
        }

        protected void udpnRepeaterEmploy_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["__EVENTTARGET"]) && Request["__EVENTTARGET"] == udpnRepeaterEmploy.ClientID)
                {
                    string param = Request["__EVENTARGUMENT"];

                    DataTable dt = AssetService.getInstance().SearchEmployee(
                        ERPWAuthentication.SID, 
                        ERPWAuthentication.CompanyCode, 
                        param
                    );

                    rptSearchMainDelegate.DataSource = dt;
                    rptSearchMainDelegate.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        ClientService.DoJavascript("$('.input-group-main-employee-" + ID + "').smartSearchKeyUp();");
                        ClientService.DoJavascript("rebind" + ID + "();");

                    }
                    else
                    {
                        ClientService.DoJavascript("$('.input-group-main-employee-" + ID + "').noDataMatch();");
                    }
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }
    }
}