
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.widget.usercontrol
{
    public partial class AutoCompleteControl : System.Web.UI.UserControl
    {
        public string SelectText
        {
            get
            {
                if (string.IsNullOrEmpty(textAutoComplete.Text))
                {
                    return "";
                }
                else
                {
                    return hddDataText.Value;
                }
            }
            set
            {
                hddDataText.Value = value;
                udpBindAutoComplete.Update();
            }
        }
        public string SelectValue
        {
            get
            {
                if (string.IsNullOrEmpty(textAutoComplete.Text))
                {
                    return "";
                }
                else
                {
                    return hddDataValue.Value;
                }
            }
            set
            {
                hddDataValue.Value = value;
                udpBindAutoComplete.Update();
            }
        }

        public string BoxSearchID
        {
            get { return textAutoComplete.ClientID; }
        }

        #region Set Valu Data To Screen
        public string SetValue
        {
            set
            {
                SetValueDataToScreen(value, true);
            }
        }

        public string SetValueFromName
        {
            set
            {
                SetValueDataToScreen(value, false);
            }
        }

        private void SetValueDataToScreen(string value, bool isValueOrName)
        {
            if (string.IsNullOrEmpty(strJson))
            {
                return;
            }
            List<DataAutoComplete> En = JsonConvert.DeserializeObject<List<DataAutoComplete>>(strJson);
            if (value != null && En.Count > 0)
            {
                List<DataAutoComplete> EnSelect = new List<DataAutoComplete>();
                if (isValueOrName)
                {
                    EnSelect = En.Where(w => w.value.Equals(value)).ToList();
                }
                else
                {
                    EnSelect = En.Where(w => w.label.Equals(value)).ToList();
                }

                if (EnSelect.Count > 0)
                {
                    textAutoComplete.Text = string.IsNullOrEmpty(mSetName) ? EnSelect[0].label : textAutoComplete.Text;
                    textAutoComplete.ToolTip = EnSelect[0].shortname;
                    hddDataValue.Value = EnSelect[0].value;
                    hddDataText.Value = string.IsNullOrEmpty(mSetName) ? EnSelect[0].label : hddDataText.Value;
                }
                else
                {
                    textAutoComplete.Text = "";
                    textAutoComplete.ToolTip = "";
                    hddDataValue.Value = "";
                    hddDataText.Value = "";
                }
            }
            else
            {
                textAutoComplete.Text = "";
                textAutoComplete.ToolTip = "";
                hddDataValue.Value = "";
                hddDataText.Value = "";
            }
            udpBindAutoComplete.Update();
            ClientService.DoJavascript("$('.autocomplete-control-data" + ClientID + "').AG_RenderAutoComplete();");
        }

        #endregion

        private string mSetName { get; set; }
        public string SetName
        {
            set
            {
                textAutoComplete.Text = hddDataValue.Value + " - " + value;
                hddDataText.Value = value;
                mSetName = value;
                udpBindAutoComplete.Update();
                ClientService.DoJavascript("$('.autocomplete-control-data" + ClientID + "').AG_RenderAutoComplete();");
            }
        }

        public bool IsRename { get; set; }
        public string TODO_FunctionJS { get; set; }
        public string PlaceHolder { get; set; }
        public string CssClass { get; set; }

        public string AddClassCustom{ get; set; }
        
        public bool Enabled
        {
            get
            {
                return textAutoComplete.Enabled;
            }
            set
            {
                textAutoComplete.Enabled = value;
                udpBindAutoComplete.Update();
            }
        }
        public bool NotAutoSearch { get; set; }

        public bool CustomizeDisplay { get; set; }

        public string CustomViewCode { get; set; }

        public string ControlData
        {
            get
            {
                AutoCompleteControlData dataEn =  new AutoCompleteControlData();

                dataEn.ControlID = ClientID;
                dataEn.AutoCompleteID  = textAutoComplete.ClientID;
                dataEn.ValueID  = hddDataValue.ClientID;
                dataEn.TextID  = hddDataText.ClientID;
                dataEn.RenameID  = txtRenameData.ClientID;
                dataEn.IsCustomizeDisplay  = CustomizeDisplay;
                dataEn.IsNotAutoSearch  = NotAutoSearch;
                dataEn.IsEnabled  = Enabled;
                dataEn.IsRename  = IsRename;
                dataEn.callBack  = TODO_FunctionJS;
                dataEn.CustomViewCode = CustomViewCode;

                return JsonConvert.SerializeObject(dataEn);
            }
        }

        public string _strJson;
        public string strJson
        {
            get 
            {
                if (string.IsNullOrEmpty(_strJson))
                {
                    _strJson = (string)Session["AutoCompleteControl.strJson" + ClientID]; 
                }
                return _strJson;
            }
            set 
            { 
                Session["AutoCompleteControl.strJson" + ClientID] = value;
                if (_strJson != null)
                {
                    _strJson = value;
                }
                udpBindAutoComplete.Update(); 
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string _cssClass = CssClass;
                textAutoComplete.CssClass = _cssClass;
               
                textAutoComplete.Attributes.Add("placeholder", PlaceHolder);
            }
            ClientService.DoJavascript("$('.autocomplete-control-data" + ClientID + "').AG_RenderAutoComplete();");
        }


        public void initialDataAutoComplete(DataTable dt, string ColumnValue, string ColumnText)
        {
            initialDataAutoComplete(dt, ColumnValue, ColumnText, false);
        }

        public void initialDataAutoComplete(DataTable dt, string ColumnValue, string ColumnText, bool IsSumKeyDes)
        {
            initialDataAutoComplete(dt, ColumnValue, ColumnText, IsSumKeyDes,"", "", "");
        }

        //public void initialDataAutoComplete(DataTable dt, string ColumnValue, string ColumnText, bool IsSumKeyDes, string columnOther1, string columnOther2)
        //{
        //    initialDataAutoComplete(dt, ColumnValue, ColumnText, IsSumKeyDes, "", columnOther1, columnOther2);
        //}
        public void initialDataAutoComplete(DataTable dt, string ColumnValue, string ColumnText, bool IsSumKeyDes,string shortName, string columnOther1, string columnOther2)
        {
            string _cssClass = textAutoComplete.CssClass;
            if (!string.IsNullOrEmpty(AddClassCustom))
            {
                if (_cssClass.IndexOf(AddClassCustom) == -1)
                {
                    _cssClass += (" " + AddClassCustom);
                }
            }
            else
            {
                _cssClass = CssClass;
            }
            textAutoComplete.CssClass = _cssClass;
            textAutoComplete.Attributes.Add("placeholder", PlaceHolder);

            List<DataAutoComplete> En = new List<DataAutoComplete>();

            foreach (DataRow dr in dt.Rows)
            {
                En.Add(new DataAutoComplete
                {
                    value = dr[ColumnValue].ToString(),
                    label = IsSumKeyDes ? dr[ColumnValue] + " - " + dr[ColumnText] : dr[ColumnText].ToString(),
                    shortname = shortName == "" ? "" : dr[shortName].ToString(),
                    other1 = columnOther1 == "" ? "" : dr[columnOther1].ToString(),
                    other2 = columnOther2 == "" ? "" : dr[columnOther2].ToString()
                });
            }

            strJson = JsonConvert.SerializeObject(En);
            if (En.Count == 1)
            {
                SetValue = En[0].value;
            }
            else
            {
                SetValue = "";
            }

            udpBindAutoComplete.Update();

            ClientService.DoJavascript("$('.autocomplete-control-data" + ClientID + "').AG_RenderAutoComplete();");
        }

        public class DataAutoComplete
        {
            public string value { get; set; }
            public string label { get; set; }
            public string other1 { get; set; }
            public string other2 { get; set; }
            public string shortname { get; set; }
        }

        private class AutoCompleteControlData
        {
            public string ControlID { get; set; }
            public string AutoCompleteID { get; set; }
            public string ValueID { get; set; }
            public string TextID { get; set; }
            public string RenameID { get; set; }
            public bool IsCustomizeDisplay { get; set; }
            public bool IsNotAutoSearch { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsRename { get; set; }
            public string callBack { get; set; }

            public string CustomViewCode { get; set; }
        }
    }
}