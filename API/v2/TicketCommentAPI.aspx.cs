using agape.proxy.data.dataset;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
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
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v2
{
    public partial class TicketCommentAPI : AbstractWebAPI //System.Web.UI.Page
    {
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private AfterSaleService service = AfterSaleService.getInstance();

        public UserAuthenticationEntities UserAuthentication
        {
            get
            {

                return new UserAuthenticationEntities
                {
                    SID = _SID,
                    CompanyCode = _CompanyCode,
                    SNAID = "",
                    FullNameTH = _FullNameTH,
                    FullNameEN = _FullNameEN,
                    EmployeeCode = _EmployeeCode,
                    LinkID = _EmployeeCode
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
                string statusCodeNew = !string.IsNullOrEmpty(Request["statusCode"]) ? Request["statusCode"] : Request.Headers["statusCode"];
                //if (string.IsNullOrEmpty(TicketNumber))
                //{
                //    throw new Exception("access denied.");
                //}


                if (!string.IsNullOrEmpty(TicketNumber))
                {
                    string TiccketTemp = TicketNumber;
                    TicketNumber = service.ConvertToTicketDB(TicketNumber);

                    string aobjectlink = service.getAobjectLinkByTicketNumber(TicketNumber);
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
                        
                        string ticketType = "";
                        string ticketYear = "";
                        string ticketNo = "";
                        string docStatusOld = "";
                        //Get Key For Update
                        DataTable dt = GetTicketDetailByTicketNumber(_SID,_CompanyCode, TicketNumber);
                        if (dt.Rows.Count <= 0)
                        {
                            throw new Exception("ไม่พบหมายเลข ticket no : "+ TiccketTemp +" ในระบบ!");
                        }
                        ticketType = Convert.ToString(dt.Rows[0]["Doctype"]);
                        ticketYear = Convert.ToString(dt.Rows[0]["Fiscalyear"]);
                        ticketNo = Convert.ToString(dt.Rows[0]["CallerID"]);
                        docStatusOld = Convert.ToString(dt.Rows[0]["Docstatus"]);
                        validStatusNewUpdate(ticketType,statusCodeNew, docStatusOld);

                        JSON = postremark(aobjectlink, docStatusOld, statusCodeNew);
                        PostTierZeroUpdateStatus(ticketType, ticketYear, ticketNo, docStatusOld, statusCodeNew);//Update Status
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
                        _SID, _CompanyCode, TierZeroID, "", false
                    );
                    if (string.IsNullOrEmpty(dataTierZero.SEQ))
                    {
                        throw new Exception("access denied.");
                    }

                    if (Request["q"].ToString() == "postremark")
                    {
                        if (!string.IsNullOrEmpty(dataTierZero.TicketNumber))
                        {
                            validStatusNewUpdate(dataTierZero.TicketType, statusCodeNew, dataTierZero.Status);
                            string aobjectlink = service.getAobjectLinkByTicketNumber(dataTierZero.TicketNumber);
                            JSON = postremark(aobjectlink, dataTierZero.Ticket_DocStatus, statusCodeNew);
                            PostTierZeroUpdateStatus(dataTierZero.TicketType, dataTierZero.FiscalYear, dataTierZero.TicketNumber, dataTierZero.Ticket_DocStatus, statusCodeNew);//Update Status
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

        //Remark Message
        private void PostTierZeroOtherRemark(string TierZeroID)
        {
            string Comment = !string.IsNullOrEmpty(Request["remarkMessage"]) ? Request["remarkMessage"] : Request.Headers["remarkMessage"]; // Request["remarkMessage"];
            string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
            string RemarkType = !string.IsNullOrEmpty(Request["remarkType"]) ? Request["remarkType"] : Request.Headers["remarkType"]; // Request["remarkType"];

            TierZeroLibrary.GetInstance().SaveTierZeroPostOtherRemark(
                _SID,
                _CompanyCode,
                TierZeroID,
                Channel,
                RemarkType,
                Comment,
                _EmployeeCode
            );
        }

        #region //Status Code Update
        private void PostTierZeroUpdateStatus(string ticketType,string ticketYear,string ticketNo
            ,string docStatusOld,string statusNew)
        {
            string statusCodeNew = statusNew;
            string statusCodeOld = docStatusOld;
            if (!string.IsNullOrEmpty(statusCodeNew) && statusCodeNew != statusCodeOld)
            {
                string SERVICE_DOC_STATUS_RESOLVE = lib.GetTicketStatusFromEvent(_SID, _CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE);
                string SERVICE_DOC_STATUS_RESPONSECUSTOMER = lib.GetTicketStatusFromEvent(_SID, _CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESPONSECUSTOMER);

                //Update Status
                try
                {
                    service.UpdateStatus(_SID, _CompanyCode, statusCodeNew, ticketType, ticketYear, ticketNo,
                        _UserName, Validation.getCurrentServerStringDateTime());
                }
                catch (Exception ex)
                {

                }
              
                if (statusCodeOld == SERVICE_DOC_STATUS_RESOLVE)
                {
                    lib.reOpenTicketTask_SLA(_SID, _CompanyCode, ticketNo);
                }
                if (statusCodeNew == SERVICE_DOC_STATUS_RESPONSECUSTOMER)
                {
                    string ResponseDate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                    string ResponseTime = Validation.getCurrentServerDateTime().ToString("HHmmss");
                    string ResponseBy = _EmployeeCode;

                    saveTimetampResponseToCustomer(ResponseDate, ResponseTime, ResponseBy, statusCodeNew
                        , ticketType, ticketYear, ticketNo);
                }
                
                //NotificationLibrary.GetInstance().TicketAlertEvent(
                //    NotificationLibrary.EVENT_TYPE.TICKET_UPDATESTATUS,
                //    _SID,
                //    _CompanyCode,
                //    ticketNo,
                //    _EmployeeCode
                //);

            }
        }

        private void saveTimetampResponseToCustomer(string ResponseDate, string ResponseTime, string ResponseBy
            ,string statusCodeNew,string ticketType,string ticketYear,string ticketNo)
        {
            try
            {
                //Get CallEntity Bean
                tmpServiceCallDataSet serviceCallEntity = GetTicketBeanStandard(ticketType, ticketNo, ticketYear);
                
                foreach (DataRow dr in serviceCallEntity.cs_servicecall_item.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ResponseOnDate"])))
                    {
                        return;
                    }

                    dr["ResponseOnDate"] = ResponseDate;
                    dr["ResponseOnTime"] = ResponseTime;
                    dr["ResponseBy"] = ResponseBy;
                }

                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    dr["Docstatus"] = statusCodeNew;
                }

                saveAssignCancelCloseResponseResolution("1500545", "Response To Customer", ResponseBy, serviceCallEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private tmpServiceCallDataSet GetTicketBeanStandard(string doctype, string docnumber, string fiscalyear)
        {
            tmpServiceCallDataSet serviceTempCallEntity = new tmpServiceCallDataSet();
            Object[] objParam = new Object[] { "1500117",_SessionID,
                _CompanyCode,doctype,docnumber,fiscalyear};
            DataSet[] objDataSet = new DataSet[] { new tmpServiceCallDataSet() };
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            if (objReturn != null)
            {

                serviceTempCallEntity.Merge(objReturn.Copy());
            }
            return serviceTempCallEntity;
        }

        private void saveAssignCancelCloseResponseResolution(string ReflectionCode, string _Message, string empcode,
            tmpServiceCallDataSet serviceCallEntity)
        {
            try
            {
                if (!string.IsNullOrEmpty(ReflectionCode))
                {
                    DataSet objReturn = new DataSet();
                    string returnMessage = "";

                    string sessionid = (string)Session[_SessionID];
                    Object[] objParam = new Object[] { ReflectionCode, sessionid };
                    DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
                    objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);

                    returnMessage = (objReturn.Tables["MessageResult"] == null || objReturn.Tables["MessageResult"].Rows.Count <= 0)
                         ? "" : objReturn.Tables["MessageResult"].Rows[0]["Message"].ToString();

                    if (returnMessage.ToString() != "")
                    {
                        throw new Exception(_Message + " Error : " + returnMessage);
                    }
                }
                else
                {
                    throw new Exception("กรุณาทำการบันทึกเอกสารก่อนครับ/ค่ะ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetTicketDetailByTicketNumber(string sid,string companyCode,string ticketNumber)
        {
            string sql = @"
                select Fiscalyear, Doctype, CallerID, Docstatus from cs_servicecall_header with(nolock)
                 WHERE SID = '"+ sid + "' AND CompanyCode = '"+ companyCode + "' and CallerID = '"+ ticketNumber + @"'
                 order by CREATED_BY desc
            ";
            DataTable dt = new DBService().selectDataFocusone(sql);
            return dt;
        }

        private void validStatusNewUpdate(string ticketType,string statusCode,string statusOld)
        {
            if (!string.IsNullOrEmpty(statusCode))
            {
                //1. ดัก Status ที่ไม่สามารถเปลียนได้ตาม Event --> Start, Resolve, Closed, Cancel
                if (ServiceTicketLibrary.TICKET_STATUS_EVENT_START.Equals(statusOld, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("ไม่สามารถอัพเดตข้อมูลได้เนื่องจากเอกสารอยู่ในสถานะ Open!");
                }
                else if (ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE.Equals(statusOld, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("ไม่สามารถอัพเดตข้อมูลได้เนื่องจากเอกสารอยู่ในสถานะ Resolve!");
                }
                else if (ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED.Equals(statusOld, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("ไม่สามารถอัพเดตข้อมูลได้เนื่องจากเอกสารอยู่ในสถานะ Closed!");
                }
                else if (ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL.Equals(statusOld, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("ไม่สามารถอัพเดตข้อมูลได้เนื่องจากเอกสารอยู่ในสถานะ Cancel!");
                }

                //2. Doctype ของ Business Object Change  จะไม่สามารถเปลียน Status ได้
                string buObjectCode = lib.GetBusinessObjectFromTicketType(_SID, ticketType);
                if (ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE.Equals(buObjectCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("ไม่สามารถอัพเดตสถานะเอกสารประเภท " + ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE_DESC + " ได้!");
                }
                else if(string.IsNullOrEmpty(buObjectCode))
                {
                    throw new Exception("ไม่พบ ticket type : " + ticketType + " สำหรับการอัพเดตสถานะในระบบ!");
                }

                //3. ดัก Status ตาม Business Object ของ Doc Type
                List<string> EventCode = new List<string>();
                EventCode.Add(ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE);
                EventCode.Add(ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED);
                EventCode.Add(ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL);
                List<string> notStatus = new List<string>();
                notStatus.Add("");
                notStatus.Add("00");
                DataTable dt = lib.GetTicketStatus(_SID, _CompanyCode, buObjectCode, statusCode, EventCode, notStatus);
                if (dt.Rows.Count <= 0)
                {
                    throw new Exception("ไม่พบ status "+ statusCode + " ในเอกสาร ticket type : " + ticketType + " ในระบบ!");
                }
            }
        }

        #endregion

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

        private string postremark(string aobjectlink,string statusCodeOld,string statusCodeNew)
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

                if (!string.IsNullOrEmpty(statusCodeNew) && statusCodeNew != statusCodeOld)
                {
                    quoteType = "UPDATESTATUS";
                    messageQuote = @"Update status from "+"\""+ ServiceTicketLibrary.GetTicketDocStatusDesc(_SID, _CompanyCode, statusCodeOld) + "\""+ " to " + "\"" + ServiceTicketLibrary.GetTicketDocStatusDesc(_SID, _CompanyCode, statusCodeNew) + "\"";
                    //messageQuote = messageQuote.Replace("'", "\"");
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
                    string UploadFilePath = Server.MapPath("~/managefile/" + _SID + "/kmfile/assets/");
                    if (!Directory.Exists(UploadFilePath))
                    {
                        Directory.CreateDirectory(UploadFilePath);
                    }
                    string UploadFileUrl = Domain + "/managefile/" + _SID + "/kmfile/assets/";

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
                    timeLine.SID = _SID;
                    timeLine.CompanyCode = _CompanyCode;
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
                    timeLine.CreatorId = _EmployeeCode;
                    timeLine.CreatorName = _FullNameEN;
                    timeLine.LinkId = _EmployeeCode;
                    timeLine.EmployeeCode = _EmployeeCode;
                    timeLine.CreatedOn = dateTime;

                    ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
                    libService.AddTimeline(_SID, _CompanyCode, timeLine);
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
                    asset.SID = _SID;
                    asset.CompanyCode = _CompanyCode;
                    asset.ObjectLink = timeLineKey;
                    asset.AssetKey = Filekey;
                    asset.Type = assetType.ToString();
                    asset.ContentUri = UploadFilePath + savedFileName;
                    asset.ContentUrl = UploadFileUrl + savedFileName;
                    asset.Latitude = "";
                    asset.Longitude = "";
                    asset.Address = "";
                    asset.CreatedBy = _EmployeeCode;
                    asset.CreatedOn = Validation.getCurrentServerStringDateTime();

                    ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
                    libService.AddTimelineAsset(_SID, _CompanyCode, asset);
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

    }
}