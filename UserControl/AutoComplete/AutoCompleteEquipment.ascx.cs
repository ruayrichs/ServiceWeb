using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserControl.AutoComplete
{
    public partial class AutoCompleteEquipment : System.Web.UI.UserControl
    {        
        public string CssClass
        {            
            set { tbEquipment.CssClass = value; }
        }

        public string placeholder 
        {
            set { tbEquipment.Attributes["placeholder"] = value; }
        }

        public string SelectedValue
        {
            get { return hdfEquipmentCode.Value; }
            set { hdfEquipmentCode.Value = value; ClientService.DoJavascript("setDataAutoCompleteEquipment"+ClientID+"('" + value + "')"); }
        }

        public string SelectedText
        {
            get { return hdfEquipmentName.Value; }
        }

        public string SelectedDisplay
        {
            get { return tbEquipment.Text; }
            set 
            {
                string xValue = Convert.ToString(value);
                string[] Arr = xValue.Split(':');
                if (Arr.Length > 1)
                {
                    hdfEquipmentCode.Value = Arr[0].Trim();
                    hdfEquipmentName.Value = Arr[1].Trim();
                }
                else 
                {
                    hdfEquipmentCode.Value = Arr[0].Trim();
                    hdfEquipmentName.Value = Arr[0].Trim();
                }
                tbEquipment.Text = xValue;
            }
        }

        public string BoxSearchID
        {
            get { return tbEquipment.ClientID; }
        }

        public string AfterSelectedChange { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ResetValue()
        {
            tbEquipment.Text = "";
            hdfEquipmentCode.Value = "";
            hdfEquipmentName.Value = "";
        }


        public void RebindAutoComplete()
        {
            ClientService.DoJavascript("loadEquipmentWithoutCondition" + ClientID + "();");
        }
    }
}