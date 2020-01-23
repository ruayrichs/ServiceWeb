using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Report
{
    public partial class UserAnalystDetail : AbstractsSANWebpage
    {
        
        private string empIDLog 
        {
            get { return (string)Session["UserAnalystDetail_" + Request["id"]]; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadUserAnalystList();
            }
        }

        private void loadUserAnalystList()
        {
            #region Validate
            if (string.IsNullOrEmpty(empIDLog))
            {
                return;
            }
            #endregion
            List<UserLogEntity> ListLogData = new LogServiceLibrary().GetLogToTalRefUser(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, empIDLog);

            JArray data = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(ListLogData));
            upPanelProfileList.Update();
            ClientService.DoJavascript("afterSearch(" + data + ");");
            ClientService.DoJavascript("scrollToTable();");
        }


    }
}