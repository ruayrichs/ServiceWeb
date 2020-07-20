using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using System.Net;
using ERPW.Lib.Service;
using ERPW.Lib.Authentication;

namespace ServiceWeb.API
{
    public partial class LineLoginAPI : System.Web.UI.Page
    {
        private UserProfileService userProfileService = UserProfileService.getInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GetData();
            } catch (Exception ex)
            {
                Console.WriteLine("Line err: " + ex.Message);
                Response.Redirect(Page.ResolveUrl("~/UserProfile/UserProfile.aspx"));
            }
        }

        private void GetData()
        {
            string CODE = Request.QueryString["code"];
            string EMPCODE = Request.QueryString["state"];
            string CLIENT_ID = ConfigurationManager.AppSettings["LINE_LOGIN_CHANNEL_ID"];
            string CLIENT_SECRET = ConfigurationManager.AppSettings["LINE_LOGIN_CHANNEL_SECRET"];
            string API_URL = "https://api.line.me/oauth2/v2.1/token";
            string CONTENT_TYPE_CONFIG = "application/x-www-form-urlencoded";


            Uri originalUrl = Request.Url;
            string domain = originalUrl.Host;

            Dictionary<string, string> dictPostData = new Dictionary<string, string>()
{
                {"grant_type", "authorization_code"},
                {"code", CODE},
                {"redirect_uri", ConfigurationManager.AppSettings["LINE_LOGIN_API_RE_DIRECT"]},
                {"client_id", CLIENT_ID},
                {"client_secret", CLIENT_SECRET}
            };

            string postData = "";

            foreach (string key in dictPostData.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(dictPostData[key]) + "&";
            }

            byte[] data = Encoding.ASCII.GetBytes(postData);


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(API_URL);
            httpWebRequest.ContentType = CONTENT_TYPE_CONFIG;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = data.Length;

            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            var result = "";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            
            if ((int)httpResponse.StatusCode == 200 )
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();

                }
                LineTokenModel obj = JsonConvert.DeserializeObject<LineTokenModel>(result);

                string tokenString = obj.id_token;

                var handler = new JwtSecurityTokenHandler();

                var token = handler.ReadJwtToken(tokenString);
                var Keys = "sub";
                var lineUidlst = token.Payload.Where(a => a.Key.Equals(Keys)).ToList();
                var lineUid = lineUidlst.First().Value;

                userProfileService.saveAboutMe(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, EMPCODE, lineUid.ToString());

                Response.Redirect(Page.ResolveUrl("~/UserProfile/UserProfile.aspx"));
            }
            else
            {
                Console.WriteLine("Line err: " + (int)httpResponse.StatusCode);
                Response.Redirect(Page.ResolveUrl("~/UserProfile/UserProfile.aspx"));
            }
          
        }
    }

    public class LineTokenModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public Int64 expires_in { get; set; }
        public string scope { get; set; }
        public string id_token { get; set; }

    }

    //public class LineDataModel
    //{
    //    public List<EventModel> events { get; set; }
    //    public string destination { get; set; }
    //}
    //public class EventModel
    //{
    //    public string type { get; set; }
    //    public string replyToken { get; set; }
    //    public SourceModel source { get; set; }
    //    public Int64 timestamp { get; set; }
    //    public string mode { get; set; }
    //    public MessageModel message { get; set; }
    //}
    //public class SourceModel
    //{
    //    public string userId { get; set; }
    //    public string type { get; set; }
    //}
    //public class MessageModel
    //{
    //    public string type { get; set; }
    //    public string id { get; set; }
    //    public string text { get; set; }
    //}
}