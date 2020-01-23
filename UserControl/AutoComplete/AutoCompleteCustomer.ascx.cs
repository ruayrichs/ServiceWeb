using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserControl.AutoComplete
{
    public partial class AutoCompleteCustomer : System.Web.UI.UserControl
    {        
        public string CssClass
        {            
            set { tbCustomer.CssClass = value; }
        }
        public bool NotAutoBindComplete
        {
            get;
            set;
        }

        public string SelectedValue
        {
            get { return hdfCustomerCode.Value; }
            set { hdfCustomerCode.Value = value; ClientService.DoJavascript("setDataAutoCompleteCustomer"+ ClientID +"('" + value + "')"); }
        }

        public string SelectedText
        {
            get { return hdfCustomerName.Value; }
            set { hdfCustomerName.Value = value; }
        }

        public string SelectedDisplay
        {
            get { return tbCustomer.Text; }
            set 
            {
                string xValue = Convert.ToString(value);
                string[] Arr = xValue.Split(':');
                if (Arr.Length > 1)
                {
                    hdfCustomerCode.Value = Arr[0].Trim();
                    hdfCustomerName.Value = Arr[1].Trim();
                }
                else 
                {
                    hdfCustomerCode.Value = Arr[0].Trim();
                    hdfCustomerName.Value = Arr[0].Trim();
                }
                tbCustomer.Text = xValue;
            }
        }

        public string AfterSelectedChange { get; set; }

        public string BoxSearchID
        {
            get { return tbCustomer.ClientID; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ResetValue()
        {
            tbCustomer.Text = "";
            hdfCustomerCode.Value = "";
            hdfCustomerName.Value = "";
        }


    }
}