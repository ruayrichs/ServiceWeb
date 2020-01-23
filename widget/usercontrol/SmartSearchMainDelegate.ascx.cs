using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.widget.usercontrol
{
    public partial class SmartSearchMainDelegate : System.Web.UI.UserControl
    {
        private F1LinkReference.F1LinkReference lc_lib = new F1LinkReference.F1LinkReference();
        EmployeeService serEmployee = new EmployeeService();

        public string SelectedCode
        {
            get
            {
                return txtResultCode.Text;
            }
            set
            {
                txtResultCode.Text = value;
                udpData.Update();
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
                udpData.Update();
            }
        }

        private bool _isOnlyFriend = true;

        public bool isOnlyFriend
        {
            get
            {
                return _isOnlyFriend;
            }
            set
            {
                _isOnlyFriend = value;
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
                txtResultCode.CssClass = "input-group-main-delegate-code-" + ID;
                txtResultValue.CssClass = "input-group-main-delegate-value-" + ID;

            }

            //ClientService.DoJavascript("bindSmartSearchMainDelegate" + ID + "();");
        }

        private void bindData()
        {
            DataTable dt = new DataTable();
            if (isOnlyFriend)
            {
                dt = lc_lib.SearchEmployeeRelation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.EmployeeCode, true, "");
            }
            else
            {
                dt = serEmployee.searchEmployee(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
            }
            rptSearchMainDelegate.DataSource = dt;
            rptSearchMainDelegate.DataBind();
        }

        public void rebindSmartSearch()
        {
            DataTable dt = new DataTable();
            if (isOnlyFriend)
            {
                dt = lc_lib.GetEmployeeRelation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.EmployeeCode, true);
            }
            else
            {
                dt = serEmployee.searchEmployee(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
            }
            rptSearchMainDelegate.DataSource = dt;
            rptSearchMainDelegate.DataBind();
            ClientService.DoJavascript("bindSmartSearchMainDelegate" + ID + "();");
        }

        protected void udpnRepeaterMainDelegate_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["__EVENTTARGET"]) && Request["__EVENTTARGET"] == udpnRepeaterMainDelegate.ClientID)
                {
                    string param = Request["__EVENTARGUMENT"];

                    DataTable dt = serEmployee.searchEmployee(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
                    rptSearchMainDelegate.DataSource = dt;
                    rptSearchMainDelegate.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        ClientService.DoJavascript("$('.input-group-main-delegate-" + ID + "').smartSearchKeyUp();");
                        ClientService.DoJavascript("bindSmartSearchMainDelegate" + ID + "();");
                    }
                    else
                    {
                        ClientService.DoJavascript("$('.input-group-main-delegate-" + ID + "').noDataMatch();");
                    }

                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        public string GetImage(Object EmployeeCode, Object UpdatedOn)
        {
            try
            {
                string path = "/images/profile/128/" + ERPWAuthentication.SID + "_" + ERPWAuthentication.CompanyCode + "_"
                    + EmployeeCode + ".png";
                string serverPath = Server.MapPath("~" + path);
                return path + "?vs=" + UpdatedOn;

                //if (File.Exists(serverPath))
                //    return path + "?vs=" + UpdatedOn;
                //else
                //    return "/images/user.png";

                //throw new Exception("no image");
            }
            catch
            {
                return "/images/user.png";
            }
        }
    }
}