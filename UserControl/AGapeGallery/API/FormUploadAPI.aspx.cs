
using ERPW.Lib.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserControl.AGapeGallery.API
{
    public partial class FormUploadAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PostUploadFile();
            }
        }

        private void PostUploadFile()
        {
            JObject obj = new JObject();
            try
            {
                HttpFileCollection Files = Request.Files;
                if (Files.Count == 0)
                    throw new Exception("no files");

                List<FilesModel> returnFile = SaveFiles(Files);
                obj.Add("result", "S");
                obj.Add("datas", JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(returnFile)));
                obj.Add("message", "");

            }
            catch (Exception ex)
            {
                obj.Add("result", "F");
                obj.Add("datas", new JArray());
                obj.Add("message", ObjectUtil.Err(ex.Message));
            }

            Response.Write(obj.ToString());
        }

        private List<FilesModel> SaveFiles(HttpFileCollection Files)
        {
            string absolutePath = "/managefile/" + ERPWAuthentication.SID + "/UploadFileTemp/";
            string serverPath = Server.MapPath("~" + absolutePath);

            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            List<FilesModel> fileModel = new List<FilesModel>();
            for (int i = 0; i < Files.Count; i++)
            {
                HttpPostedFile file = Files[i];

                string extension = System.IO.Path.GetExtension(file.FileName);
                string realName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                string guid = Guid.NewGuid().ToString("D");
                string fileName = guid + extension;


                string SavePath = serverPath + fileName;
                file.SaveAs(SavePath);
                if (file.ContentType.IndexOf("video") > -1)
                {
                    createThumbnail(SavePath, serverPath + guid + ".jpg");
                }


                #region abd: convert to video use too much time!! and cannot convert from mov to mp4
                //string SavePath = serverPath + "-tmp-" + fileName;
                //file.SaveAs(SavePath);

                //createThumbnail(SavePath, serverPath + guid + ".jpg");
                //fileName = guid + ".mp4";

                //var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                //ffMpeg.ConvertMedia(SavePath, serverPath + fileName, Format.mp4);

                //if (File.Exists(SavePath))
                //{
                //    File.Delete(SavePath);
                //}
                #endregion

                fileModel.Add(new FilesModel
                {
                    FileName = realName + extension,
                    FilePath = absolutePath + fileName,
                    FileExtension = extension.Replace(".", "").ToLower(),
                    guid = guid
                });
            }
            return fileModel;
        }

        private void createThumbnail(String pathToVideoFile, String pathForThumbnail)
        {

            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.GetVideoThumbnail(pathToVideoFile, pathForThumbnail, 5);
        }

        public class FilesModel
        {
            public string guid { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
            public string FileExtension { get; set; }
        }
    }
}