using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Framework.ag_activity_remark.API
{
    public partial class SystemMessageFormAPI : System.Web.UI.Page
    {
        private F1LinkReference.F1LinkReference lc_lib = new F1LinkReference.F1LinkReference();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";

            string RequestType = Request["type"];
            string JSON = "";
            try
            {
                if (RequestType.Equals("getrefemail"))
                {
                    JSON = GetRefEmail();
                }
            }
            catch
            {
                JSON = "[]";
            }

            Response.Write(JSON);
        }

        private string GetRefEmail()
        {
            string aobj = Request["aobj"];
            string refEmailCode = Request["refEmailCode"];
            bool includeEmailBody = Convert.ToBoolean(Request["includeEmailBody"]);

            JArray result = ServiceTicketLibrary.GetInstance().getActivityEmailWithAttachment(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                aobj,
                refEmailCode,
                true,
                true
            );

            foreach (JObject content in result.Children<JObject>())
            {
                string refCode = content.GetValue("message_id").ToString();
                string Aobjectlink = content.GetValue("AOBJECTLINK").ToString();

                JObject key = new JObject();
                key.Add("aobj", Aobjectlink);
                key.Add("refEmailCode", refCode);
                key.Add("includeEmailBody", includeEmailBody);
                key.Add("type", Request["type"]);

                JObject getRefEmailContent = new JObject();
                getRefEmailContent.Add("key", key);
                getRefEmailContent.Add("url", "/widget/SystemMessageFormAPI.aspx");
                content.Add("getRefEmailContent", getRefEmailContent);
            }

            return JsonConvert.SerializeObject(result).ToString();
        }
    }
}