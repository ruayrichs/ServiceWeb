using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.WebConfig;
using Link.Lib.Model.Model.Timeline;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.API.Model.Respond;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



//namespace ServiceWeb.API.v1
//{
//    public partial class TicketCommentAPI : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}


namespace ServiceWeb.API.v1
{
    public partial class TicketCommentAPI : System.Web.UI.Page
    {
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private AppClientLibrary libAppClient = AppClientLibrary.GetInstance();

        private string _SID;
        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = !string.IsNullOrEmpty(ERPWAuthentication.SID) ? ERPWAuthentication.SID : ERPWebConfig.GetSID(); // "555";
                return _SID;
            }
        }

        private string _CompanyCode;
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = !string.IsNullOrEmpty(ERPWAuthentication.CompanyCode) ? ERPWAuthentication.CompanyCode : ERPWebConfig.GetCompany(); // "INET";
                return _CompanyCode;
            }
        }

        private string _UserName;
        private string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_UserName))
                    _UserName = !string.IsNullOrEmpty(ERPWAuthentication.UserName) ? ERPWAuthentication.UserName : ""; // "focusone";
                return _UserName;
            }
        }

        private string _EmployeeCode;
        private string EmployeeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_EmployeeCode))
                    _EmployeeCode = !string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode) ? ERPWAuthentication.EmployeeCode : ""; // "focusone";
                return _EmployeeCode;
            }
        }

        private string _FullNameTH;
        private string FullNameTH
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameTH))
                    _FullNameTH = !string.IsNullOrEmpty(ERPWAuthentication.FullNameTH) ? ERPWAuthentication.FullNameTH : ""; // "focusone";
                return _FullNameTH;
            }
        }

        private string _FullNameEN;
        private string FullNameEN
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameEN))
                    _FullNameEN = !string.IsNullOrEmpty(ERPWAuthentication.FullNameEN) ? ERPWAuthentication.FullNameEN : ""; // "focusone";
                return _FullNameEN;
            }
        }

        private string _OwnerCode;
        private string OwnerCode
        {
            get
            {
                if (string.IsNullOrEmpty(_OwnerCode))
                    _OwnerCode = !string.IsNullOrEmpty(ERPWAuthentication.Permission.OwnerGroupCode) ? ERPWAuthentication.Permission.OwnerGroupCode : ""; // "focusone";
                return _OwnerCode;
            }
        }

        public UserAuthenticationEntities UserAuthentication
        {
            get
            {

                return new UserAuthenticationEntities
                {
                    SID = SID,
                    CompanyCode = CompanyCode,
                    SNAID = "",
                    FullNameTH = FullNameTH,
                    FullNameEN = FullNameEN,
                    EmployeeCode = EmployeeCode,
                    LinkID = EmployeeCode
                };
            }
        }

        public class UserAuthenticationEntities
        {
            public string SID { get; set; }
            public string CompanyCode { get; set; }
            public string SNAID { get; set; }
            public string FullNameTH { get; set; }
            public string FullNameEN { get; set; }
            public string EmployeeCode { get; set; }
            public string LinkID { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TicketCommentResponseModel response = new TicketCommentResponseModel();
            if (!checkPermission())
            {
                response.resultCode = "Error";
                response.message = "No Permission.";
                //response.Datas = new DataSet();
            }
            else
            {
                response = TicketTrackingDetail();
            }
            Response.Write(JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        //private string getAobjectLinkByTicketNumber(string TicketNumber)
        //{
        //    //string TicketNumber = !string.IsNullOrEmpty(Request["TicketNumber"]) ? Request["TicketNumber"] : Request.Headers["TicketNumber"];
        //    //if (string.IsNullOrEmpty(TicketNumber))
        //    //{
        //    //    throw new Exception("access denied.");
        //    //}
        //    string aobjectlink = "";
        //    aobjectlink = ServiceLibrary.LookUpTable(
        //        "AOBJECTLINK", 
        //        "CRM_SERVICECALL_MAPPING_ACTIVITY",
        //        "where SNAID = 'INET' AND ServiceDocNo = '" + TicketNumber + @"' order by Tier desc"
        //    );

        //    return aobjectlink;
        //}

        private TicketCommentResponseModel TicketTrackingDetail()
        {
            TicketCommentResponseModel response = new TicketCommentResponseModel();
            try
            {
                Response.ContentType = "application/json";
                string JSON = "";

                string TicketNumber = !string.IsNullOrEmpty(Request["TicketNumber"]) ? Request["TicketNumber"] : Request.Headers["TicketNumber"];
                string TierZeroID = !string.IsNullOrEmpty(Request["TierZeroID"]) ? Request["TierZeroID"] : Request.Headers["TierZeroID"];
                //if (string.IsNullOrEmpty(TicketNumber))
                //{
                //    throw new Exception("access denied.");
                //}


                if (!string.IsNullOrEmpty(TicketNumber))
                {
                    string aobjectlink = AfterSaleService.getInstance().getAobjectLinkByTicketNumber(TicketNumber);
                    if (string.IsNullOrEmpty(aobjectlink))
                    {
                        throw new Exception("access denied.");
                    }

                    if (Request["q"].ToString() == "taskremark-lazyload")
                    {
                        response.data = new TicketCommentModel();

                        TicketCommentModel datas = taskRemarkLazyLoad(aobjectlink);
                        response.data.Remarks = datas.Remarks;
                        response.data.RemarkTicketReplys = datas.RemarkTicketReplys;
                        response.data.TotalRemark = datas.TotalRemark;

                        response.resultCode = "Success";
                        response.message = "Search success.";
                    }
                    else if (Request["q"].ToString() == "postremark")
                    {
                        JSON = postremark(aobjectlink);
                        response.resultCode = "Success";
                        response.message = "Post success.";
                    }
                    else
                    {
                        throw new Exception("access denied.");
                    }
                }
                else if (!string.IsNullOrEmpty(TierZeroID))
                {
                    TierZeroEn dataTierZero = TierZeroLibrary.GetInstance().getTierZeroDetail(
                        SID, CompanyCode, TierZeroID, "", false
                    );
                    if (string.IsNullOrEmpty(dataTierZero.SEQ))
                    {
                        throw new Exception("access denied.");
                    }

                    if (Request["q"].ToString() == "postremark")
                    {
                        if (!string.IsNullOrEmpty(dataTierZero.TicketNumber))
                        {
                            string aobjectlink = AfterSaleService.getInstance().getAobjectLinkByTicketNumber(dataTierZero.TicketNumber);
                            JSON = postremark(aobjectlink);
                            PostTierZeroOtherRemark(dataTierZero.SEQ);
                        }
                        else
                        {
                            PostTierZeroOtherRemark(dataTierZero.SEQ);
                        }

                        response.resultCode = "Success";
                        response.message = "Post success.";
                    }

                }
                else
                {
                    throw new Exception("access denied.");
                }

                //Response.Write(JSON);
            }
            catch (Exception ex)
            {
                response.resultCode = "Error";
                response.message = ex.Message;
                //response.Datas = new DataSet();
            }

            return response;
        }

        private void PostTierZeroOtherRemark(string TierZeroID)
        {
            string Comment = !string.IsNullOrEmpty(Request["remarkMessage"]) ? Request["remarkMessage"] : Request.Headers["remarkMessage"]; // Request["remarkMessage"];
            string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
            string RemarkType = !string.IsNullOrEmpty(Request["remarkType"]) ? Request["remarkType"] : Request.Headers["remarkType"]; // Request["remarkType"];

            TierZeroLibrary.GetInstance().SaveTierZeroPostOtherRemark(
                SID,
                CompanyCode,
                TierZeroID,
                Channel,
                RemarkType,
                Comment,
                EmployeeCode
            );
        }


        private bool checkPermission()
        {
            bool HasPermission = false;

            try
            {
                string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                if (Channel != TierZeroLibrary.TIER_ZERO_CHANNEL_APPCLIENT)
                {
                    ERPW_API_Permission_Token_Key_DAO libPermission = new ERPW_API_Permission_Token_Key_DAO();
                    string PermissionKey = !string.IsNullOrEmpty(Request["PermissionKey"]) ? Request["PermissionKey"] : Request.Headers["PermissionKey"];
                    DataTable dtPermission = libPermission.getOneByKey(PermissionKey);

                    if (dtPermission.Rows.Count > 0)
                    {
                        _SID = dtPermission.Rows[0]["SID"].ToString();
                        _UserName = dtPermission.Rows[0]["UserName"].ToString();
                        _EmployeeCode = dtPermission.Rows[0]["EmployeeCode"].ToString();
                        _OwnerCode = dtPermission.Rows[0]["OwnerService"].ToString();
                    }

                    if (!string.IsNullOrEmpty(_UserName))
                    {
                        HasPermission = true;
                        SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");
                        ERPWAutoLoginService loginService = new ERPWAutoLoginService(_SID, _UserName, mode);
                    }
                    else
                    {
                        HasPermission = false;
                    }
                }
                else
                {
                    string CorpoKey = !string.IsNullOrEmpty(Request["CorporatePermissionKey"]) ? Request["CorporatePermissionKey"] : Request.Headers["CorporatePermissionKey"];
                    string AppKey = !string.IsNullOrEmpty(Request["ApplicationPermissionKey"]) ? Request["ApplicationPermissionKey"] : Request.Headers["ApplicationPermissionKey"];
                    string AppID = !string.IsNullOrEmpty(Request["ApplicationID"]) ? Request["ApplicationID"] : Request.Headers["ApplicationID"];
                    //libAppClient.chec
                    if (libAppClient.checkAuthenCreatedTicket(SID, CompanyCode, CorpoKey, AppKey, AppID))
                    {
                        HasPermission = true;
                    }
                    else
                    {
                        HasPermission = false;
                    }
                }

            }
            catch (Exception)
            {
                HasPermission = false;
            }

            return HasPermission;
        }

        private TicketCommentModel taskRemarkLazyLoad(string aobjectlink)
        {
            ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();

            bool isFutureMode = false; // HttpContext.Current.Request["isLoadNewMessage"] == null ? false : Convert.ToBoolean(HttpContext.Current.Request["isLoadNewMessage"]);

            List<ActivityRemark> ActivityRemark = libService.GetActivityRemarkLazyLoad(
                UserAuthentication.SID,
                UserAuthentication.CompanyCode,
                UserAuthentication.LinkID,
                aobjectlink,
                999, //5,
                isFutureMode,
                "", //HttpContext.Current.Request["seq"],
                false
            );

            int Totals = libService.GetActivityRemarkTotalRows(
                UserAuthentication.SID,
                UserAuthentication.CompanyCode,
                aobjectlink
            );

            TicketCommentModel Lazy = new TicketCommentModel
            {
                Remarks = ActivityRemark,
                TotalRemark = Totals
            };
            return Lazy;
            //return JSONUtil.GetJson(Lazy);
        }

        private string postremark(string aobjectlink)
        {
            ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();

            string comment = Request["remarkMessage"];
            string type = Request["remarkType"];
            string attachFileKey = Request["attachFileKey"];
            string RefCode = Request["RefCode"];
            //if (!string.IsNullOrEmpty(RefCode))
            //{
            //    type = "Reply";
            //}

            bool sendMail = Convert.ToBoolean(Request["sendMail"]);
            bool isQuote = Convert.ToBoolean(Request["isQuote"]);

            try
            {
                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                string messageQuote = "";
                string quoteType = "";

                if (isQuote)
                {
                    messageQuote = Request["quoteMessage"];
                    if (!string.IsNullOrEmpty(messageQuote))
                    {
                        messageQuote = messageQuote.Replace("'", "''");
                    }
                    quoteType = Request["quoteType"];
                }

                string UploadType = !string.IsNullOrEmpty(Request["uploadType"]) ? Request["uploadType"] : Request.Headers["uploadType"];
                attachFileKey = PostUploadFile(aobjectlink, UploadType, comment);

                libService.SaveActivityDetail(
                        UserAuthentication.SID,
                        UserAuthentication.CompanyCode,
                        UserAuthentication.SNAID,
                        UserAuthentication.EmployeeCode,
                        UserAuthentication.LinkID,
                        UserAuthentication.FullNameEN,
                        aobjectlink,
                        "",
                        comment,
                        "",
                        curDateTime,
                        type,
                        quoteType,
                        messageQuote,
                        attachFileKey,
                        RefCode
                    );

                //string UploadType = !string.IsNullOrEmpty(Request["uploadType"]) ? Request["uploadType"] : Request.Headers["uploadType"];
                //PostUploadFile(aobjectlink, UploadType, comment);

                JObject obj = new JObject();
                obj.Add("message", "S");
                return obj.ToString();
            }
            catch (Exception ex)
            {
                JObject obj = new JObject();
                obj.Add("message", ObjectUtil.Err(ex.Message));
                return obj.ToString();
            }
        }

        #region Attach Files

        private string PostUploadFile(string aobjectLink, string uploadType, string Message)
        {
            if (string.IsNullOrEmpty(aobjectLink))
            {
                return "";
            }
            string TimeLineKey = "";
            try
            {
                HttpFileCollection Files = Request.Files;
                if (Files != null && Files.Count > 0)
                {
                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                            (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    string UploadFilePath = Server.MapPath("~/managefile/" + SID + "/kmfile/assets/");
                    if (!Directory.Exists(UploadFilePath))
                    {
                        Directory.CreateDirectory(UploadFilePath);
                    }
                    string UploadFileUrl = Domain + "/managefile/" + SID + "/kmfile/assets/";

                    //string aobjectLink = Request["aobj"];
                    //string uploadType = Request["uploadType"];
                    //string Message = Request["message"];
                    string dateTime = Validation.getCurrentServerStringDateTime();
                    TimeLineKey = aobjectLink + "_" + uploadType + "_" + dateTime;


                    int type = 0;
                    int assetType = 0;
                    if (uploadType.Equals("IMAGE"))
                    {
                        type = Timeline.TYPE_IMAGE;
                        assetType = TimelineAsset.TYPE_IMAGE;
                    }
                    else //FILE
                    {
                        type = Timeline.TYPE_ATTACH_FILE;
                        assetType = TimelineAsset.TYPE_FILE;
                    }

                    Timeline timeLine = new Timeline();
                    timeLine.SID = SID;
                    timeLine.CompanyCode = CompanyCode;
                    timeLine.ObjectLink = aobjectLink;
                    timeLine.TimelineKey = TimeLineKey;
                    timeLine.Type = type.ToString();
                    timeLine.Message = Message;
                    timeLine.ContentUri = "";
                    timeLine.ContentUrl = "";
                    timeLine.Status = "";
                    timeLine.Latitude = "";
                    timeLine.Longitude = "";
                    timeLine.Address = "";
                    timeLine.CreatorId = EmployeeCode;
                    timeLine.CreatorName = FullNameEN;
                    timeLine.LinkId = EmployeeCode;
                    timeLine.EmployeeCode = EmployeeCode;
                    timeLine.CreatedOn = dateTime;

                    ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
                    libService.AddTimeline(SID, CompanyCode, timeLine);
                    PostAssetAndSaveFile(Files, TimeLineKey, uploadType, assetType, UploadFilePath, UploadFileUrl);

                    //libService.UpdateAttachFileKeyToRemark(SID, CompanyCode, aobjectLink, TimeLineKey);
                }

                return TimeLineKey;
            }
            catch
            {
                return "";
            }
        }

        private void PostAssetAndSaveFile(HttpFileCollection Files, string timeLineKey, string uploadType, int assetType
            , string UploadFilePath, string UploadFileUrl)
        {
            for (int i = 0; i < Files.Count; i++)
            {
                string Filekey = uploadType + "_" + Validation.getCurrentServerStringDateTime() + i.ToString();
                string savedFileName = SaveFile(Files[i], Filekey, UploadFilePath);
                if (!string.IsNullOrEmpty(savedFileName))
                {
                    TimelineAsset asset = new TimelineAsset();
                    asset.SID = SID;
                    asset.CompanyCode = CompanyCode;
                    asset.ObjectLink = timeLineKey;
                    asset.AssetKey = Filekey;
                    asset.Type = assetType.ToString();
                    asset.ContentUri = UploadFilePath + savedFileName;
                    asset.ContentUrl = UploadFileUrl + savedFileName;
                    asset.Latitude = "";
                    asset.Longitude = "";
                    asset.Address = "";
                    asset.CreatedBy = EmployeeCode;
                    asset.CreatedOn = Validation.getCurrentServerStringDateTime();

                    ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
                    libService.AddTimelineAsset(SID, CompanyCode, asset);
                }
            }
        }

        private string SaveFile(HttpPostedFile file, string FileKey, string UploadFilePath)
        {
            try
            {
                string savePath = UploadFilePath;
                string extension = System.IO.Path.GetExtension(file.FileName);
                string realName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                string fileName = ReplaceInvalidFilenameChar(realName) + "_" + Validation.getCurrentServerStringDateTimeMillisecond() + extension;

                savePath += fileName;
                file.SaveAs(savePath);

                if (Request["createThumb"] != null)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(savePath);
                    System.Drawing.Image thumb = image.GetThumbnailImage(24, 24, () => false, IntPtr.Zero);
                    string tempFileName = fileName.Replace(extension, "");
                    savePath = savePath.Replace(fileName, tempFileName + "_thumb" + extension);
                    thumb.Save(Path.ChangeExtension(savePath, "png"));
                }

                return fileName;
            }
            catch
            {
                return "";
            }
        }

        private string ReplaceInvalidFilenameChar(string input)
        {
            string illegal = input;
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            illegal = r.Replace(illegal, "");
            illegal = Regex.Replace(input, @"[^\w\d]", "");
            return illegal;
        }

        #endregion

        //protected class CommonResponse
        //{
        //    public string ResultCode { get; set; }
        //    public string Message { get; set; }
        //    //public DataSet Datas { get; set; }
        //}
    }
}