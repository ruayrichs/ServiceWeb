using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserControl.AutoComplete
{
    public partial class AutoCompleteEmployee : System.Web.UI.UserControl
    {
        public string placeholder
        {
            set { tbEmployee.Attributes["placeholder"] = value; }
        }
        public string CssClass
        {            
            set { tbEmployee.CssClass = value; }
        }

        public string SelectedValue
        {
            get { return hdfEmployeeCode.Value; }
            set
            {
                hdfEmployeeCode.Value = value;
            }
        }

        public string SelectedText
        {
            get { return hdfEmployeeName.Value; }
            set { hdfEmployeeName.Value = value;}
        }

        public string SelectedDisplay
        {
            get { return tbEmployee.Text; }
            set { tbEmployee.Text = value; }
        }

        /// <summary>
        /// param [code : name]
        /// </summary>
        public string SelectedValueRefresh
        {
            get { return hdfEmployeeCode.Value; }
            set
            {
                string Code_Name = string.IsNullOrEmpty(value) ? "" : value;
                ClientService.DoJavascript("RefreshValueAutoCompleteEmployee" + ClientID + "('" + Code_Name + "')");
            }
        }

        public string AfterSelectedChange
        {
            get { return hddAfterSelectedChange.Value; }
            set { hddAfterSelectedChange.Value = value; }
        }
        public bool Enabled
        {
            get
            {
                return Convert.ToBoolean(hddAutoCompleteEnabled.Value);
            }
            set
            {
                hddAutoCompleteEnabled.Value = value.ToString();
                ClientService.DoJavascript("$('#" + tbEmployee.ClientID + "').prop('disabled', " + (!value).ToString().ToLower() + ")");
                udpOption.Update();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientService.DoJavascript("$('#" + tbEmployee.ClientID + "').prop('disabled', " + (!Enabled).ToString().ToLower() + ")");
        }
        public void ResetValue()
        {
            tbEmployee.Text = "";
            hdfEmployeeCode.Value = "";
            hdfEmployeeName.Value = "";
        }

    }
}