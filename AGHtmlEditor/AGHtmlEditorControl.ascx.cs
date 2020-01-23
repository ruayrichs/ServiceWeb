using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.AGHtmlEditor
{
    public partial class AGHtmlEditorControl : System.Web.UI.UserControl
    {
        public Boolean _hideParamButton;
        public Boolean hideParamButton
        {
            get
            {
                return _hideParamButton;
            }
            set
            {
                _hideParamButton = value;
            }
        }
        protected string _Height = "300px";
        public string Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }
        public string HTML
        {
            get
            {
                return txtSummerNoteTextTemp.Text.Replace("SIGNLESSTHAN", "<");
            }
            set
            {
                txtSummerNoteTextTemp.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init();
            }
        }

        public void Init()
        {
            ClientService.DoJavascript("summerNoteTextInit" + ClientID + "();");
        }


        #region Read & Write File txt

        public String ReadFileTxtHtml(string filepath)
        {
            string path = Server.MapPath(filepath);
            if (File.Exists(path))
                return File.ReadAllText(path);
            return "";
        }

        public void WriteFileConfig(string SID,string GroupCode, string AOBJECTLINK)
        {
            string path = GetConfigFilePath(SID, GroupCode);
            string Path = Server.MapPath(path);
            string FileName = AOBJECTLINK + ".txt";
            string FullPath = Path + FileName;
            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
            }
            StreamWriter file = new StreamWriter(FullPath, true);
            file.WriteLine(HTML);
            file.Close();
        }

        public string GetConfigFilePathTempFile(string SID, string GroupCode, string AOBJECTLINK,bool isPhysicalPath = false)
        {
            if (!isPhysicalPath)
            {
                return GetConfigFilePath(SID, GroupCode) + AOBJECTLINK + ".txt";
            }
            else
            {
                return Server.MapPath(GetConfigFilePath(SID, GroupCode)) + AOBJECTLINK + ".txt";
            }
        }

        private string GetConfigFilePath(string SID, string GroupCode)
        {
            string path = "~/managefile/" + SID + "/HTMLEditor/" + (String.IsNullOrEmpty(GroupCode) ? "" : GroupCode + "/");
            if (!Directory.Exists(Server.MapPath(path)))
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }
            return path;
        }
        #endregion
    }
}