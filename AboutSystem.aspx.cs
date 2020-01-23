using ERPW.Lib.Master.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class AboutSystem : System.Web.UI.Page
    {
        private MasterConfigLibrary libMasterConfig = new MasterConfigLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
            readApplicationVersion();
        }
        private void readApplicationVersion()
        {
            DataTable dt = libMasterConfig.GetApplicationVersion();

            foreach (DataRow row in dt.Rows)
            {
                txReleres.Text = row["Releres"].ToString();
                txVersion.Text = row["Version"].ToString();
                txLicensed.Text = row["Licensed"].ToString();
                txSiteID.Text = row["SiteID"].ToString();
                txBoxProductSpecificationRemark.Text = row["ProductSpecificationRemark"].ToString();
                txBoxContactTechinicalSupport.Text = row["ContactTechinicalSupport"].ToString();
                txBoxTelephoneTechinicalSupport.Text = row["TelephoneTechinicalSupport"].ToString();
                txBoxThirdPratyNoticesRemark.Text = row["ThirdPratyNoticesRemark"].ToString();
            }
        }
    }
}