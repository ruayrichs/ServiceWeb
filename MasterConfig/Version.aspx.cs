using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class Version : System.Web.UI.Page
    {
        private DBService dbService = new DBService();
        private MasterConfigLibrary libMasterConfig = new MasterConfigLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetApplicationVersion();
            
            if (!IsPostBack)
            {
                readApplicationVersion();
            }

        }
        protected void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                string Releres = txtReleres.Text;
                string Version = txtVersion.Text;
                string Licensed = txtLicensed.Text;
                string SiteID = txtSiteID.Text;
                string ProductSpecificationRemark = txtBoxProductSpecificationRemark.Text;
                string ContactTechinicalSupport = txtBoxContactTechinicalSupport.Text;
                string TelephoneTechinicalSupport = txtBoxTelephoneTechinicalSupport.Text;
                string ThirdPratyNoticesRemark = txtBoxThirdPratyNoticesRemark.Text;
                string Created_By = ERPWAuthentication.EmployeeCode;
                //string Created_On = "20191010";

                libMasterConfig.updateApplicationVersion(Releres, Version, Licensed, SiteID, ProductSpecificationRemark, ContactTechinicalSupport,
                TelephoneTechinicalSupport, ThirdPratyNoticesRemark, Created_By);

                ClientService.AGSuccess("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
        

        private void readApplicationVersion()
        {
            DataTable dt = libMasterConfig.GetApplicationVersion();

            foreach (DataRow row in dt.Rows)
            {
                txtReleres.Text = row["Releres"].ToString();
                txtVersion.Text = row["Version"].ToString();
                txtLicensed.Text = row["Licensed"].ToString();
                txtSiteID.Text = row["SiteID"].ToString();
                txtBoxProductSpecificationRemark.Text = row["ProductSpecificationRemark"].ToString();
                txtBoxContactTechinicalSupport.Text = row["ContactTechinicalSupport"].ToString();
                txtBoxTelephoneTechinicalSupport.Text = row["TelephoneTechinicalSupport"].ToString();
                txtBoxThirdPratyNoticesRemark.Text = row["ThirdPratyNoticesRemark"].ToString();
            }
        }
    }
}