using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.usercontrol
{
    public partial class WorkerAssignedControl : System.Web.UI.UserControl
    {
        public string WorkGroupCode
        {
            get
            {
                return hddWorkGroupCode.Value;
            }
            set
            {
                hddWorkGroupCode.Value = value;
                udpCodectr.Update();
            }
        }
        
        public string TierCode
        {
            get 
            { 
                return hddTierCode.Value;
            }
            set 
            {
                hddTierCode.Value = value;
                udpCodectr.Update();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        public void GetData(string TierCode, string WorkGroupCode)
        {
            this.WorkGroupCode = WorkGroupCode;
            this.TierCode = TierCode;
            DataTable dtTier = AfterSaleService.getInstance().getTierItem(ERPWAuthentication.SID, WorkGroupCode, TierCode);
            rptOperation.DataSource = dtTier;
            rptOperation.DataBind();
            updWorkAssigned.Update();
        }

        protected void rptOperation_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptMainDelegate = e.Item.FindControl("rptMainDelegate") as Repeater;
            Repeater rptOtherDelegate = e.Item.FindControl("rptOtherDelegate") as Repeater;
            HiddenField hddTier = e.Item.FindControl("hddTier") as HiddenField;
            Panel PanelShowOther = e.Item.FindControl("PanelShowOther") as Panel;
            Panel PanelHideOther = e.Item.FindControl("PanelHideOther") as Panel;

            DataTable dtMain = AfterSaleService.getInstance().getTierDefaultMain(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, TierCode, hddTier.Value);
            DataTable dtParticipant = AfterSaleService.getInstance().getTierDefaultParticipant(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, TierCode, hddTier.Value);

            rptMainDelegate.DataSource = dtMain;
            rptMainDelegate.DataBind();

            rptOtherDelegate.DataSource = dtParticipant;
            rptOtherDelegate.DataBind();

            if (dtParticipant.Rows.Count > 0)
            {
                PanelShowOther.Style["display"] = "";
                PanelHideOther.Style["display"] = "none";
            }
            else
            {
                PanelHideOther.Style["display"] = "";
                PanelShowOther.Style["display"] = "none";
            }
        }
    }
}