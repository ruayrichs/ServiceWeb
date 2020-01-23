using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserControl.AutoComplete.General
{
    public partial class AutoCompleteGeneral : System.Web.UI.UserControl
    {
        public string ActionCase //***
        {
            get { return hdfActionCase.Text; }
            set { hdfActionCase.Text = value; }
        }
        public bool NotAutoBindComplete
        {
            get {
                bool NotBind = false;
                bool.TryParse(hdfNotAutoBindComplete.Text, out NotBind);
                return NotBind; 
            }
            set { hdfNotAutoBindComplete.Text = Convert.ToString(value); }
        }

        public string CssClass
        {
            set { tbDescription.CssClass = "Description " + value; }
        }
        
        public string SelectedValue
        {
            get { return hdfCode.Text; }
        }

        public string SelectedText
        {
            get { return hdfName.Text; }
        }

        public string SelectedDisplay
        {
            get { return tbDescription.Text; }
            set
            {
                string xValue = Convert.ToString(value);
                string[] Arr = xValue.Split(':');
                if (Arr.Length > 1)
                {
                    hdfCode.Text = Arr[0].Trim();
                    hdfName.Text = Arr[1].Trim();
                }
                else
                {
                    hdfCode.Text = Arr[0].Trim();
                    hdfName.Text = "";
                }
                tbDescription.Text = xValue;

                ClientService.DoJavascript("bindAutocompleteGeneral('" + ClientID + "',[],'" + tbDescription.Text + "');");
            }
        }

        public string AfterSelectedChange { get { return hdfAfterSelectedChange.Text; } set { hdfAfterSelectedChange.Text = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!NotAutoBindComplete)
                {
                    ClientService.DoJavascript("bindAutocompleteGeneral('" + ClientID + "',[],'" + tbDescription.Text + "');");
                }
            }
        }

        public bool Enabled
        {
            get { return tbDescription.Enabled; }
            set{ tbDescription.Enabled =  value;}
        }

    }
}