using Agape.FocusOne.Utilities;
using Newtonsoft.Json;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.mastertier
{
    public partial class TierGroupMasterControl : System.Web.UI.UserControl
    {
        TierService tierService = TierService.getInStance();

        public String WorkGroupCode
        {
            get
            {
                return "20170121162748444411";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindTierDataToScreen("");
            }
        }

        private void bindTierDataToScreen(string TierGroupDescription)
        {
            DataTable dtTier = tierService.searchTierGorupMaster(ERPWAuthentication.SID, WorkGroupCode, "", TierGroupDescription);
            rptTierData.DataSource = dtTier;
            rptTierData.DataBind();
            udpTierData.Update();

        }

        protected void btnSearchForTierGroup_Click(object sender, EventArgs e)
        {
            try
            {
                bindTierDataToScreen(txtTierGroupDescription.Text);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally 
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnSaveForTierGroupMater_Click(object sender, EventArgs e)
        {
            try
            {
                string TierCode = Validation.getCurrentServerStringDateTimeMillisecond() + new Random().Next(9).ToString() + new Random().Next(2).ToString() + new Random().Next(8).ToString();
                tierService.InsertTierMaster(
                    ERPWAuthentication.SID,
                    WorkGroupCode,
                    TierCode, 
                    txtTierGroupDescription.Text,
                    ERPWAuthentication.EmployeeCode);
                bindTierDataToScreen("");
                ClientService.AGSuccess("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnUpdateForTierGroupMaterServer_Click(object sender, EventArgs e)
        {
            try
            {
                string Resource = txtTierGroupMasterControlResource.Text;
                List<TierService.entityTierMaster> listTier
                    = JsonConvert.DeserializeObject<List<TierService.entityTierMaster>>(Resource);
                tierService.UpdateTierMaster(ERPWAuthentication.SID, WorkGroupCode, ERPWAuthentication.EmployeeCode, listTier);
                ClientService.AGSuccess("แก้ไขสำเร็จ");
                bindTierDataToScreen("");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
    }
}