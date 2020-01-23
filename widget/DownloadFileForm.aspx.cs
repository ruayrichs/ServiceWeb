using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.widget
{
    public partial class DownloadFileForm : System.Web.UI.Page
    {
        private string FileName
        {
            get
            {
                return txtFileName.Text;
            }
            set
            {
                txtFileName.Text = value;
            }
        }
        private string FilePath
        {
            get
            {
                return txtFilePath.Text;
            }
            set
            {
                txtFilePath.Text = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool hasFile = false;
                try
                {
                    string fName = Request.Form["FILENAME"];
                    string fPath = Request.Form["FILEPATH"];
                    FileName = fName;
                    try
                    {
                        FilePath = Server.MapPath("~" + fPath);
                    }
                    catch
                    {
                        FilePath = fPath;
                    }

                    lblFilename.Text = FileName;

                    if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
                    {
                        hasFile = true;
                    }
                    if (Request["del"] != null && Request["del"].Equals("true"))
                    {
                        btnDownload.Style["display"] = "none";
                        divDel.Style["display"] = "";
                        divDownload.Style["display"] = "none";
                    }
                }
                finally
                {
                    divDownload.Visible = hasFile;
                    divEmpty.Visible = !hasFile;
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
            response.TransmitFile(FilePath);
            response.Flush();
            if (Request["del"] != null && Request["del"].Equals("true"))
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
            response.End();

        }
    }
}