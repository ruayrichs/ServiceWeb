using agape.entity;
using agape.entity.km;
using Agape.FocusOne.Utilities;
using Agape.Lib.KMV2.Core.service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserControl.AGapeGallery.UploadGallery
{
    public partial class UploadGallery : System.Web.UI.UserControl
    {
        static CultureInfo culture_EN_US = new CultureInfo("en-GB", false);

        public string PreviewWidth
        {
            get
            {
                return txtConfig_PreviewWidth.Text;
            }
            set
            {
                txtConfig_PreviewWidth.Text = value;
            }
        }
        public string PreviewHeight
        {
            get
            {
                return txtConfig_PreviewHeight.Text;
            }
            set
            {
                txtConfig_PreviewHeight.Text = value;
            }
        }
        public string AcceptType
        {
            get
            {
                return txtConfig_AcceptType.Text;
            }
            set
            {
                txtConfig_AcceptType.Text = value;
            }
        }
        public string AcceptTypeIcon
        {
            get
            {
                if (AcceptType == null)
                {
                    return "fa-upload";
                }
                else if (AcceptType.ToLower().Equals("image"))
                {
                    return "fa-camera";
                }
                else if (AcceptType.ToLower().Equals("video"))
                {
                    return "fa-video-camera";
                }
                return "fa-upload";
            }
        }
        public bool DisableUpload
        {
            get
            {
                return Convert.ToBoolean(txtConfig_DisableUpload.Text);
            }
            set
            {
                txtConfig_DisableUpload.Text = value.ToString();
            }
        }
        public bool AutoUploadKM
        {
            get
            {
                return Convert.ToBoolean(txtConfig_AutoUploadKM.Text);
            }
            set
            {
                txtConfig_AutoUploadKM.Text = value.ToString();
            }
        }
        public bool MultipleMode
        {
            get
            {
                return Convert.ToBoolean(txtConfig_MultipleMode.Text);
            }
            set
            {
                txtConfig_MultipleMode.Text = value.ToString();
            }
        }
        public bool PreviewCertificate
        {
            get
            {
                return Convert.ToBoolean(txtConfig_PreviewCertificate.Text);
            }
            set
            {
                txtConfig_PreviewCertificate.Text = value.ToString();
            }
            //imgLogo
        }
        public string EventType
        {
            get;
            set;
        }
        public class FilesModel
        {
            public string guid { get; set; }
            public string dataUrl { get; set; }
            public string name { get; set; }
        }
        private class SaveFilesResult
        {
            public string PhysicalFileName { get; set; }
            public string PhysicalFilePath { get; set; }
        }

        public class UploadedFiled
        {
            public string name { get; set; }
            public string path { get; set; }
            public string guid { get; set; }
        }
        public List<UploadedFiled> ListUploadedFile
        {
            get
            {
                return JsonConvert.DeserializeObject<List<UploadedFiled>>(txtUploadedJSON.Text);
            }
            set
            {
                txtUploadedJSON.Text = JsonConvert.SerializeObject(value);
                udpBackup.Update();
            }
        }

        private string _SavePath = "/managefile/" + ERPWAuthentication.SID + "/upload_files/";
        public string SavePath
        {
            get
            {
                return _SavePath;
            }
            set
            {
                _SavePath = value;
            }
        }
        public string SessionJSONContainer
        {
            get
            {
                return Session["SessionJSONContainer" + ClientID] == null || string.IsNullOrEmpty((string)Session["SessionJSONContainer" + ClientID]) ? "[]" : (string)Session["SessionJSONContainer" + ClientID];
            }
            set
            {
                Session["SessionJSONContainer" + ClientID] = value;
            }
        }
        public List<string> SaveDeleteFile()
        {
            
            List<string> FileDelete = GetSessionDeleteFileQue();
            var uploadedFile = ListUploadedFile.Where(a => FileDelete.Any(x => x.Equals(a.guid)));
            foreach (var item in uploadedFile)
            {
                string serverPath = Server.MapPath("~" + item.path);
                if (File.Exists(serverPath))
                {
                    File.Delete(serverPath);
                }
            }
            return FileDelete;
        }
        public DataTable SaveFiles()
        {
            return GenericSaveFiles(true, false, false);
        }
        public DataTable SaveFilesNoRenameFixedFileName()
        {
            return GenericSaveFiles(false, true, true);
        }
        private DataTable GenericSaveFiles(Boolean renameInputFile, Boolean removeIfExist, Boolean fixedFileName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FileName");
            dt.Columns.Add("FileExtension");
            dt.Columns.Add("PhysicalFileName");
            dt.Columns.Add("PhysicalFilePath");
            if (!string.IsNullOrEmpty(SessionJSONContainer))
            {
                List<FilesModel> Files = JsonConvert.DeserializeObject<List<FilesModel>>(SessionJSONContainer);
                foreach (var file in Files)
                {
                    SaveFilesResult FileDesc = SaveFileProcess(file.dataUrl, SavePath, file.name, renameInputFile, removeIfExist, fixedFileName);
                    if (FileDesc != null)
                    {
                        dt.Rows.Add(
                            file.name,
                            Path.GetExtension(FileDesc.PhysicalFileName).Replace(".", ""),
                            FileDesc.PhysicalFileName,
                            FileDesc.PhysicalFilePath
                        );
                    }
                }
            }

            return dt;
        }
        public void Clear()
        {
            ClientService.DoJavascript("ClearAllFiles" + ClientID + "();");
            hddEditFilesContainer.Value = "";
            ClearSessionDeleteFileQue();
            SessionJSONContainer = null;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SessionJSONContainer = null;
            }
        }
        protected void btnTriggerUpload_Click(object sender, EventArgs e)
        {
            SessionJSONContainer = hddEditFilesContainer.Value;
            hddEditFilesContainer.Value = "";
            ClientService.AGLoading(false);

            if (AutoUploadKM)
            {
                SaveFilesWithKMObject();
            }
        }
        private SaveFilesResult SaveFileProcess(string data, string targetPath, string inputFiileName, Boolean renameInputFile, Boolean removeIfExist, Boolean fixedFileName)
        {
            try
            {
                SaveFilesResult Result = new SaveFilesResult();
                string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                var fileName = (renameInputFile) ? inputFiileName.Substring(0, (inputFiileName.Count()) - (inputFiileName.Split('.').Last().Count()) - 1) 
                                + "_" 
                                + timestamp + Path.GetExtension(inputFiileName) : inputFiileName;

                if (fixedFileName)
                {
                    targetPath = HttpContext.Current.Server.MapPath(targetPath);

                    String onlyPath = targetPath.Substring(0, targetPath.LastIndexOf('\\'));
                    if (!Directory.Exists(onlyPath))
                    {
                        Directory.CreateDirectory(onlyPath);
                    }
                    Result.PhysicalFileName = targetPath.Substring(targetPath.LastIndexOf('\\') + 1);
                    Result.PhysicalFilePath = targetPath;
                }
                else
                {
                    Result.PhysicalFileName = fileName;
                    Result.PhysicalFilePath = targetPath + fileName;
                    targetPath = HttpContext.Current.Server.MapPath("~" + targetPath);
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    targetPath = targetPath + fileName;
                }

                if (removeIfExist && File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }

                File.Copy(Server.MapPath("~" + data), targetPath);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InitialUploadedFile(DataTable InputData, string DataPathField, string DataNameField, string DataIDField)
        {
            List<UploadedFiled> listFiles = new List<UploadedFiled>();
            foreach (DataRow dr in InputData.Rows)
            {
                string path = Convert.ToString(dr[DataPathField]);

                listFiles.Add(new UploadedFiled
                {
                    name = Convert.ToString(dr[DataNameField]),
                    path = path,
                    guid = Convert.ToString(dr[DataIDField])
                });
            }
            ListUploadedFile = listFiles;
            ClientService.DoJavascript("appendUploadedFiles" + ClientID + "(" + JsonConvert.SerializeObject(listFiles) + ",'" + AcceptType + "');");
        }

        private List<string> GetSessionDeleteFileQue()
        {
            List<string> result = hddDeleteUploadedFileQue.Text.Split(',').ToList();
            result = result.Where(a => !string.IsNullOrEmpty(a)).ToList();
            return result;
        }

        private void AddSessionDeleteFileQue(string seq)
        {
            List<string> que = GetSessionDeleteFileQue();
            que.Add(seq);
            hddDeleteUploadedFileQue.Text = string.Join(",", que);
        }

        private void ClearSessionDeleteFileQue()
        {
            hddDeleteUploadedFileQue.Text = "";
        }
        protected void btnDeleteUploadedFile_Click(object sender, EventArgs e)
        {
            string FileKey = hddDeleteUploadedFile.Text;
            if (!string.IsNullOrEmpty(Convert.ToString(KMObjectID)))
            {
                DeleteUploadedKMFile(FileKey);
            }
            else
            {
                AddSessionDeleteFileQue(FileKey);
            }

            ClientService.AGLoading(false);
            ClientService.DoJavascript("ToggleAdder" + ClientID + "();");
        }

        protected void btnEditDescUploadedFile_Click(object sender, EventArgs e)
        {
            string FileKey = hddEditDescUploadedFileKey.Text;
            string FileDesc = hddEditDescUploadedFileDesc.Text;
            if (String.IsNullOrEmpty(FileKey))
            {
                return;
            }

            try
            {
                UploadGalleryService.getInstance().EditKMDesc(
                     ERPWAuthentication.SID,
                     FileKey,
                     FileDesc
                 );


                ClientService.DoJavascript("AGSuccess('บันทึกสำเร็จ');location.reload();");
            }
            catch (Exception ex)
            {
                ClientService.AGError("Edit Error[" + ex.Message + "]");
            }
            finally
            {
                ClientService.AGLoading(false);

            }
        }

        #region KM Object        
        public string KMUploadType
        {
            get
            {
                if (AcceptType == null || AcceptType.ToLower().Equals("file"))
                {
                    return "FILE";
                }
                else if (AcceptType.ToLower().Equals("image"))
                {
                    return "IMAGE";
                }
                else if (AcceptType.ToLower().Equals("video"))
                {
                    return "VIDEO";
                }

                return "FILE";
            }
        }
        public string KMUploadFileMessage
        {
            get
            {
                return txtConfig_KMUploadFileMessage.Text;
            }
            set
            {
                txtConfig_KMUploadFileMessage.Text = value;
            }
        }
        public string KMObjectID
        {
            get
            {
                return txtConfig_KMObjectID.Text;
            }
            set
            {
                txtConfig_KMObjectID.Text = value;
            }
        }
        public string KMBusinessType
        {
            get
            {
                return txtConfig_KMBusinessType.Text;
            }
            set
            {
                txtConfig_KMBusinessType.Text = value;
            }
        }
        public string KMDocumentType
        {
            get
            {
                return txtConfig_KMDocumentType.Text;
            }
            set
            {
                txtConfig_KMDocumentType.Text = value;
            }
        }
        public string KMFiscalYear
        {
            get
            {
                return txtConfig_KMFiscalYear.Text;
            }
            set
            {
                txtConfig_KMFiscalYear.Text = value;
            }
        }
        public int BindKMFileUploaded()
        {
            if (string.IsNullOrEmpty(Convert.ToString(KMObjectID)))
            {
                throw new Exception("KM Object is null");
            }
            DataTable dt = UploadGalleryService.getInstance().GetKMFile(
                ERPWAuthentication.SID,
                KMObjectID,
                KMUploadType
            );

            JArray arr = new JArray();
            foreach (DataRow dr in dt.Rows)
            {
                string path = "/managefile/" + ERPWAuthentication.SID + dr["file_path"].ToString().Replace("\\", "/");
                JObject obj = new JObject();
                obj.Add("name", dr["file_name"].ToString());
                obj.Add("path", path);
                obj.Add("guid", dr["key_object_link"].ToString());
                obj.Add("description", Convert.ToString(dr["description"]));

                arr.Add(obj);
            }
            ClientService.DoJavascript("appendUploadedFiles" + ClientID + "(" + arr.ToString() + ",'" + AcceptType + "');");
            return dt.Rows.Count;
        }
        private void DeleteUploadedKMFile(string fileKey)
        {
            string path = UploadGalleryService.getInstance().GetPathKMFile(
                ERPWAuthentication.SID,
                KMObjectID,
                fileKey
            );


            if (!string.IsNullOrEmpty(path))
            {
                string serverPath = "~/managefile/" + ERPWAuthentication.SID + path;
                serverPath = Server.MapPath(serverPath);

                UploadGalleryService.getInstance().DeleteKMFile(
                    ERPWAuthentication.SID,
                    KMObjectID,
                    fileKey,
                    serverPath
                );
            }


        }
        public void SaveFilesWithKMObject()
        {
            if (string.IsNullOrEmpty(Convert.ToString(KMObjectID)))
            {
                throw new Exception("KM Object is null");
            }
            PostUploadKMFile();
            BindKMFileUploaded();
        }
        private string PostUploadKMFile()
        {
            string result = "";

            try
            {
                if (!string.IsNullOrEmpty(SessionJSONContainer))
                {
                    List<FilesModel> Files = JsonConvert.DeserializeObject<List<FilesModel>>(SessionJSONContainer);

                    string aobjectLink = KMObjectID;
                    string businessType = KMBusinessType;
                    string documentType = KMDocumentType;
                    string fiscalYear = KMFiscalYear;
                    string uploadType = KMUploadType;
                    string message = KMUploadFileMessage;


                    IList ListOfBufferSave = new ArrayList();
                    IList ListOfAttatchment = new ArrayList();

                    string sid = ERPWAuthentication.SID;
                    string business_type = !string.IsNullOrEmpty(businessType) ? businessType : "KM";
                    string doc_year = !string.IsNullOrEmpty(fiscalYear) ? fiscalYear : "";
                    string doc_type = !string.IsNullOrEmpty(documentType) ? documentType : "KM";
                    string description = "";
                    string doc_number = aobjectLink;
                    string item_no = UploadGalleryService.getInstance().getKMMaxItemNo(sid, business_type, doc_type, doc_number, doc_year);

                    int maxItemNo = Convert.ToInt32(item_no);

                    foreach (var file in Files)
                    {
                        maxItemNo++;

                        item_no = maxItemNo.ToString().PadLeft(3, '0');

                        string key_object_link = Guid.NewGuid().ToString();
                        byte[] bytes = File.ReadAllBytes(Server.MapPath("~" + file.dataUrl));

                        MessageAttachList attach = new MessageAttachList(sid, business_type, key_object_link, doc_number);
                        attach.item_no = item_no;
                        attach.doc_year = doc_year;
                        attach.doc_type = doc_type;
                        attach.description = description;
                        attach.doc_number = doc_number;
                        attach.key_search1 = description;
                        attach.file_extension = file.name.LastIndexOf('.') > 0 ? file.name.Substring(file.name.LastIndexOf('.') + 1) : "";
                        attach.logical_name = file.name;
                        attach.file_category = uploadType;
                        attach.file_name = attach.logical_name;
                        attach.created_by = ERPWAuthentication.EmployeeCode;
                        attach.created_on = Validation.getCurrentServerStringDateTime();
                        attach.updated_by = ERPWAuthentication.EmployeeCode;
                        attach.updated_on = Validation.getCurrentServerStringDateTime();
                        attach.key_group = ERPWAuthentication.FullNameEN;

                        AttachFileUtils.readFile(attach, new System.IO.MemoryStream(bytes));

                        attach.file_size = attach.data_stream.Length;

                        ListOfAttatchment.Add(attach);
                        AttachListBuffer buffer = new AttachListBuffer(
                            sid,
                            business_type,
                            key_object_link,
                            doc_year,
                            doc_type,
                            description,
                            doc_number,
                            item_no,
                            null,
                            attach);

                        EntityUtils.doTransection(EntityUtils.ACTION_CREATE, ListOfBufferSave, buffer);
                        String srcThumbnail = getSrcThumbnailPathAndFile(Server.MapPath("~" + file.dataUrl));
                        moveThumbnailFileToKM(srcThumbnail, key_object_link);
                    }

                    AttachListBuffer buff = null;

                    for (int i = 0; i < ListOfBufferSave.Count; i++)
                    {
                        buff = (AttachListBuffer)ListOfBufferSave[i];

                        if (EntityUtils.isCreate(buff))
                        {
                            buff.business_type = !string.IsNullOrEmpty(businessType) ? businessType : "KM";
                            buff.doc_year = !string.IsNullOrEmpty(fiscalYear) ? fiscalYear : "";
                            buff.doc_type = !string.IsNullOrEmpty(documentType) ? documentType : "KM";
                            buff.attach.business_type = !string.IsNullOrEmpty(businessType) ? businessType : "KM";
                            buff.attach.doc_type = !string.IsNullOrEmpty(documentType) ? documentType : "KM";
                            buff.attach.doc_year = !string.IsNullOrEmpty(fiscalYear) ? fiscalYear : "";
                            buff.attach.doc_number = doc_number;
                            buff.attach.key_search1 = "";

                            AttachFileUtils.saveAttachList(
                                buff.sid,
                                buff.business_type,
                                buff.key_object_link,
                                buff.doc_year,
                                buff.doc_type,
                                buff.description,
                                doc_number,
                                buff.attach.key_search1,
                                buff.listUploadFile,
                                EntityUtils.ACTION_CREATE,
                                buff.attach);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ClientService.AGError("Cannot upload file [" + ex.Message + "]");

                return "";
            }
        }

        #endregion
        public static readonly String KM_PATH_FILE = @"\kmfile\{0}\{1}\{2}";
        public static readonly String KM_DIR = @"\kmfile";
        private static void moveThumbnailFileToKM(String srcThumbnailPath, String keyObjectLink)
        {
            if (!File.Exists(srcThumbnailPath))
            {
                return;
            }

            String destFilePath = String.Format(KM_PATH_FILE, new Object[]
                    {
                        DateTime.Now.ToString("yyyy",culture_EN_US), 
                        DateTime.Now.ToString("MM",culture_EN_US),
                        DateTime.Now.ToString("dd",culture_EN_US)
                    }
                );
            // String destFileName = getThumbnailFileName(srcThumbnailPath) + ".jpg";
            String destFileName = keyObjectLink + ".jpg";
            String destthumbnailKMPath = HttpContext.Current.Server.MapPath("~") + "managefile\\" + ERPWAuthentication.SID + destFilePath + "\\";
            if (!Directory.Exists(destthumbnailKMPath))
            {
                Directory.CreateDirectory(destthumbnailKMPath);
            }


            File.Move(srcThumbnailPath, destthumbnailKMPath + destFileName);
        }
        private static String getThumbnailFileName(String srcThumbnailPath)
        {
            if ((srcThumbnailPath.LastIndexOf('\\') < 0) || (srcThumbnailPath.LastIndexOf('.') < 0))
            {
                return srcThumbnailPath;
            }
            String destFileName = srcThumbnailPath.Substring(srcThumbnailPath.LastIndexOf('\\') + 1, srcThumbnailPath.LastIndexOf('.') - srcThumbnailPath.LastIndexOf('\\') - 1);

            return destFileName;
        }
        private static String getSrcThumbnailPathAndFile(String srcThumbnailPath)
        {
            if ((srcThumbnailPath.LastIndexOf('\\') < 0) || (srcThumbnailPath.LastIndexOf('.') < 0))
            {
                return srcThumbnailPath;
            }
            String srcPathAndFileName = srcThumbnailPath.Substring(0, srcThumbnailPath.LastIndexOf('.')) + ".jpg";

            return srcPathAndFileName;
        }

        public void InitialUploadedPicture(string DataPathField, string DataNameField, string DataIDField)
        {
            JObject obj = new JObject();
            obj.Add("name", DataNameField);
            obj.Add("path", DataPathField);
            obj.Add("guid", DataIDField);

            JArray arr = new JArray();
            arr.Add(obj);

            ClientService.DoJavascript("appendUploadedFiles" + ClientID + "(" + arr.ToString() + ",'" + AcceptType + "');");
        }
    }
}