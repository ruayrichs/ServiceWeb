using Agape.FocusOne.Utilities;
using Link.Lib.Model.Model.Timeline;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.widget
{
    public partial class AJAXFileUploadAPI : System.Web.UI.Page
    {
        private string UploadFilePath = "";
        private string UploadFileUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                            (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
            UploadFilePath = Server.MapPath("~/managefile/" + ERPWAuthentication.SID + "/kmfile/assets/");
            if (!Directory.Exists(UploadFilePath))
            {
                Directory.CreateDirectory(UploadFilePath);
            }
            UploadFileUrl = Domain + "/managefile/" + ERPWAuthentication.SID + "/kmfile/assets/";
            Response.Write(PostUploadFile());
        }

        private string PostUploadFile()
        {
            string TimeLineKey = "";
            try
            {
                HttpFileCollection Files = Request.Files;
                if (Files.Count > 0)
                {
                    string aobjectLink = Request["aobj"];
                    string uploadType = Request["uploadType"];
                    string Message = Request["message"];
                    string dateTime = Validation.getCurrentServerStringDateTime();
                    TimeLineKey = aobjectLink + "_" + uploadType + "_" + dateTime;


                    int type = 0;
                    int assetType = 0;
                    if (uploadType.Equals("IMAGE"))
                    {
                        type = Timeline.TYPE_IMAGE;
                        assetType = TimelineAsset.TYPE_IMAGE;
                    }
                    else if (uploadType.Equals("FILE"))
                    {
                        type = Timeline.TYPE_ATTACH_FILE;
                        assetType = TimelineAsset.TYPE_FILE;
                    }
                    else
                    {
                        type = Timeline.TYPE_ATTACH_FILE;
                        assetType = TimelineAsset.TYPE_FILE;
                    }

                    if (!string.IsNullOrEmpty(Message))
                    {
                        Message = Message.Replace("'", "''");
                    }

                    Timeline timeLine = new Timeline();
                    timeLine.SID = ERPWAuthentication.SID;
                    timeLine.CompanyCode = ERPWAuthentication.CompanyCode;
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
                    timeLine.CreatorId = ERPWAuthentication.EmployeeCode;
                    timeLine.CreatorName = ERPWAuthentication.FullNameTH;
                    timeLine.LinkId = ERPWAuthentication.EmployeeCode;
                    timeLine.EmployeeCode = ERPWAuthentication.EmployeeCode;
                    timeLine.CreatedOn = dateTime;

                    ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
                    libService.AddTimeline(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, timeLine);
                    PostAssetAndSaveFile(Files, TimeLineKey, uploadType, assetType);
                }

                return TimeLineKey;
            }
            catch
            {
                return "";
            }
        }

        private void PostAssetAndSaveFile(HttpFileCollection Files, string timeLineKey, string uploadType, int assetType)
        {
            for (int i = 0; i < Files.Count; i++)
            {
                string Filekey = uploadType + "_" + Validation.getCurrentServerStringDateTime() + i.ToString();
                string savedFileName = SaveFile(Files[i], Filekey);
                if (!string.IsNullOrEmpty(savedFileName))
                {
                    TimelineAsset asset = new TimelineAsset();
                    asset.SID = ERPWAuthentication.SID;
                    asset.CompanyCode = ERPWAuthentication.CompanyCode;
                    asset.ObjectLink = timeLineKey;
                    asset.AssetKey = Filekey;
                    asset.Type = assetType.ToString();
                    asset.ContentUri = UploadFilePath + savedFileName;
                    asset.ContentUrl = UploadFileUrl + savedFileName;
                    asset.Latitude = "";
                    asset.Longitude = "";
                    asset.Address = "";
                    asset.CreatedBy = ERPWAuthentication.EmployeeCode;
                    asset.CreatedOn = Validation.getCurrentServerStringDateTime();

                    ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
                    libService.AddTimelineAsset(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, asset);
                }
            }
        }

        private string SaveFile(HttpPostedFile file, string FileKey)
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
    }
}