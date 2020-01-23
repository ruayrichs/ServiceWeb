using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.AGapeGalleryFinal.API
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

        public class FilesModel
        {
            public string guid { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
            public string FileExtension { get; set; }
        }
    }
}