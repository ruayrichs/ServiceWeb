using Agape.FocusOne.Utilities;
using Agape.Lib.Link.Mobile.Model;
using ERPW.Lib.Service.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.Service;


namespace POSWeb.Framework.ag_activity_remark.API
{
    public partial class Default : System.Web.UI.Page
    {
        public UserAuthenticationEntities UserAuthentication
        {
            get
            {
                string SID = ERPWAuthentication.SID;
                string CompanyCode = ERPWAuthentication.CompanyCode;
                string SNAID = ERPWAuthentication.CompanyCode;
                string FullNameTH = ERPWAuthentication.FullNameTH;
                string FullNameEn = ERPWAuthentication.FullNameEN;
                string EmployeeCode = ERPWAuthentication.EmployeeCode;
                string LinkID = ERPWAuthentication.EmployeeCode;

                return new UserAuthenticationEntities
                {
                    SID = SID,
                    CompanyCode = CompanyCode,
                    SNAID = SNAID,
                    FullNameTH = FullNameTH,
                    FullNameEN = FullNameEn,
                    EmployeeCode = EmployeeCode,
                    LinkID = LinkID
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

        public class ActivityRemarkLazyload
        {
            public int TotalRemark { get; set; }
            public List<ActivityRemark> Remarks { get; set; }
            public List<ActivityRemarkTicketReply> RemarkTicketReplys { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "application/json";
                string JSON = "";
                //if (Request["q"].ToString() == "taskremark")
                //{
                //    JSON = taskRemark();
                //}
                //else if (Request["q"].ToString() == "favorite")
                //{
                //    JSON = favorite();
                //}
                //else if (Request["q"].ToString() == "getonlineform")
                //{
                //    JSON = getonlineform();
                //}
                if (Request["q"].ToString() == "taskremark-lazyload")
                {
                    JSON = taskRemarkLazyLoad();
                }
                else if (Request["q"].ToString() == "postremark")
                {
                    string aobj = Request["aobj"];
                    JSON = postremark();
                    TriggerService.GetInstance().CommentTicket(aobj);
                }
                else if (Request["q"].ToString() == "editremark")
                {
                    JSON = editremark();
                }
                else if (Request["q"].ToString() == "get-reply")
                {
                    JSON = taskRemarkReply();
                }
                else if (Request["q"].ToString() == "get-reply-in-bar") 
                {
                    JSON = taskReplyComment();
                }
                else if (Request["q"].ToString() == "validate-current-tier")
                {
                    string tkno = Request["tkno"];
                    string aobj = Request["aobj"];
                    bool isCurrentTier = AfterSaleService.CheckCurrentTier(UserAuthentication.CompanyCode, tkno, aobj);
                    JSON = isCurrentTier.ToString().ToLower();
                }
                else
                {
                    throw new Exception("access denied.");
                }

                Response.Write(JSON);
            }
            catch (Exception ex)
            {
                JObject response = new JObject();
                AGResponse.generateError(response, ex);
                AGResponse.generate(response, HttpStatusCode.Unauthorized);
                Response.Write(response);
            }
        }

        //private string taskRemark()
        //{
        //    //ReturnDataType = "ActivityRemark";
        //    string mode = Request["mode"];
        //    mode = mode == null || string.IsNullOrEmpty(mode) ? "activity" : mode;

        //    List<ActivityServiceEntity.ActivityRemark> ActivityRemark;
        //    if (mode.Equals("favorite"))
        //    {
        //        string CourseID = Request["courseId"];
        //        string ClassID = Request["classId"];
        //        CourseID = CourseID == null || string.IsNullOrEmpty(CourseID) ? "" : CourseID;
        //        ClassID = ClassID == null || string.IsNullOrEmpty(ClassID) ? "" : ClassID;

        //        ActivityRemark = ActivityServiceEntity.GetActivityRemarkFavorite(
        //            UserAuthentication.SID,
        //            UserAuthentication.CompanyCode,
        //            UserAuthentication.LinkID,
        //            true,
        //            CourseID,
        //            ClassID
        //        );
        //    }
        //    else
        //    {

        //        ActivityRemark = ActivityServiceEntity.GetActivityRemark(
        //            UserAuthentication.SID,
        //            UserAuthentication.CompanyCode,
        //            UserAuthentication.LinkID,
        //            Request["obj"],
        //            true
        //        );
        //    }

        //    foreach (var rem in ActivityRemark)
        //    {
        //        rem.Image = PublicAuthentication.FocusOneLinkProfileImageByEmployeeCode(rem.CreatorEmployeeCode);
        //    }

        //    return JSONUtil.GetJson(ActivityRemark);
        //}

        //private string favorite()
        //{
        //    try
        //    {
        //        string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
        //        string aobjectLink = Request["aobjectlink"];
        //        string classID = Request["classID"];
        //        string courseID = Request["courseID"];
        //        string messageSeq = Request["messageSeq"];

        //        ActivityServiceEntity.PostFavoritRemark_AreaP21(
        //            UserAuthentication.SID,
        //            UserAuthentication.EmployeeCode,
        //            UserAuthentication.LinkID,
        //            messageSeq,
        //            aobjectLink,
        //            courseID,
        //            classID,
        //            curDateTime
        //        );

        //        JObject obj = new JObject();
        //        obj.Add("message", "S");
        //        obj.Add("seq", messageSeq);
        //        obj.Add("count", ActivityServiceEntity.CountFavoritRemark_AreaP21(
        //            messageSeq,
        //            aobjectLink
        //        ));
        //        return obj.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        JObject obj = new JObject();
        //        obj.Add("message", ObjectUtil.Err(ex.Message));
        //        return obj.ToString();
        //    }
        //}

        //private string getonlineform()
        //{
        //    DataTable dt = ActivityService.getInstance().GetActivityOnlineForm(
        //                            UserAuthentication.SID,
        //                            UserAuthentication.CompanyCode,
        //                            UserAuthentication.SNAID,
        //                            HttpContext.Current.Request["aobj"],
        //                            UserAuthentication.EmployeeCode
        //                        );

        //    return JSONUtil.GetJson(dt);
        //}

        private string taskRemarkLazyLoad()
        {
            string aobjectlink = HttpContext.Current.Request["obj"];

            ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();

            bool isFutureMode = HttpContext.Current.Request["isLoadNewMessage"] == null ? false : Convert.ToBoolean(HttpContext.Current.Request["isLoadNewMessage"]);

            List<ActivityRemark> ActivityRemark = libService.GetActivityRemarkLazyLoad(
                UserAuthentication.SID,
                UserAuthentication.CompanyCode,
                UserAuthentication.LinkID,
                aobjectlink,
                5,
                isFutureMode,
                HttpContext.Current.Request["seq"],
                true
            );                                  

            int Totals = libService.GetActivityRemarkTotalRows(
                UserAuthentication.SID,
                UserAuthentication.CompanyCode,
                HttpContext.Current.Request["obj"]
            );

            ActivityRemarkLazyload Lazy = new ActivityRemarkLazyload
            {
                Remarks = ActivityRemark,
                TotalRemark = Totals
            };

            return JSONUtil.GetJson(Lazy);
        }

        private string taskReplyComment() {
            ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
            DataTable ActivityRemark = libService.GetActivityRemarkReplyComment_V2(UserAuthentication.SID,
                UserAuthentication.CompanyCode,
                UserAuthentication.LinkID
            );

             //ActivityRemarkLazyload Lazy = new ActivityRemarkLazyload
             //{
             //    RemarkTicketReplys = ActivityRemark,
             //    TotalRemark = ActivityRemark.Count
             //};

            return JSONUtil.GetJson(ActivityRemark);
        }

        private string taskRemarkReply()
        {
            string aobjectlink = HttpContext.Current.Request["obj"];
            ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();

            DataTable ActivityRemark = libService.GetActivityRemarkReply_V2(
                UserAuthentication.SID,
                UserAuthentication.CompanyCode,
                UserAuthentication.LinkID,
                aobjectlink
            );            

            //ActivityRemarkLazyload Lazy = new ActivityRemarkLazyload
            //{
            //    Remarks = ActivityRemark,
            //    TotalRemark = ActivityRemark.Count
            //};

            return JSONUtil.GetJson(ActivityRemark);
        }      

        private string postremark()
        {
            ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();

            string comment = Request["remarkMessage"];
            string aobj = Request["aobj"];           
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

                libService.SaveActivityDetail(
                        UserAuthentication.SID,
                        UserAuthentication.CompanyCode,
                        UserAuthentication.SNAID,
                        UserAuthentication.EmployeeCode,
                        UserAuthentication.LinkID,
                        UserAuthentication.FullNameEN,
                        aobj,
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

        private string editremark()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Result");
            dt.Columns.Add("Stamp");
            dt.Columns.Add("File");
            string rStamp = Validation.getCurrentServerStringDateTimeMillisecond();
            try
            {
                string rSEQ = Request["remarkKey"];
                string rText = Request["remarkMessage"];
                string RemoveFileJSON = Request["removeFileKey"];
                string EitAttachFile = Request["editAttachFile"];

                ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();

                libService.UpdateActivityRemark(
                    UserAuthentication.SID,
                    UserAuthentication.CompanyCode,
                    rSEQ,
                    rText,
                    rStamp,
                    RemoveFileJSON
                );

                string AttachFileKey = libService.MoveRemarkTimelineAsset(
                                        UserAuthentication.SID,
                                        UserAuthentication.CompanyCode,
                                        rSEQ,
                                        EitAttachFile);

                string FileJSON = JsonConvert.SerializeObject(
                                        libService.GetActivityRemarkAttachFiles(
                                            UserAuthentication.SID,
                                            UserAuthentication.CompanyCode,
                                            AttachFileKey,
                                            true));

                dt.Rows.Add("S", Validation.Convert2DateTimeDisplay(rStamp), FileJSON);
            }
            catch (Exception ex)
            {
                dt.Rows.Add("E", ObjectUtil.Err(ex.Message));
            }

            return JSONUtil.GetJson(dt);
        }        

    }
}