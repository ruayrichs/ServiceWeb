using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Constant;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class TicketStatus : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        private MasterConfigLibrary lib = new MasterConfigLibrary();
        private Dictionary<string, string> _mDicEvenType;
        private Dictionary<string, string> mDicEvenType
        {
            get 
            {
                if (_mDicEvenType == null)
                {
                    _mDicEvenType = ConfigurationConstant.GetMasterDataEventDocStatusDesc();
                }
                return _mDicEvenType;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindingData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindingData()
        {
            DataTable dt = lib.GetMasterConfigTicketStatus(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode,"","");

            rptItems.DataSource = dt;
            rptItems.DataBind();
            udpnItems.Update();

            ddlEventType.DataTextField = "Value";
            ddlEventType.DataValueField = "Key";
            ddlEventType.DataSource = mDicEvenType;
            ddlEventType.DataBind();
            ddlEventType.Items.Insert(0, new ListItem("", ""));
            ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {
                hdfMode.Value = ApplicationSession.CREATE_MODE_STRING;

                tbCode.Text = "";
                tbName.Text = "";
                ddlEventType.SelectedValue = "";
                chkStopTimer.Checked = true;
                tbCode.Enabled = true;

                udpn.Update();

                ClientService.DoJavascript("openModal('New');");
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

        protected void btnSetEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string code = hdfEditCode.Value;

                hdfEditCode.Value = "";

                DataTable dt = lib.GetMasterConfigTicketStatus(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode, code,"");

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;

                    tbCode.Text = dt.Rows[0]["TicketStatusCode"].ToString();
                    tbName.Text = dt.Rows[0]["TicketStatusDesc"].ToString();
                    tbCode.Enabled = false;


                    bool isStopTimer = false;
                    bool.TryParse(dt.Rows[0]["StopTimer"].ToString(),out isStopTimer);
                    chkStopTimer.Checked = isStopTimer;

                    try
                    {
                        ddlEventType.SelectedValue = dt.Rows[0]["EventType"].ToString();
                    }
                    catch (Exception)
                    {
                    }


                    udpn.Update();

                    ClientService.DoJavascript("openModal('Edit');");
                }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string code = tbCode.Text.Trim();
                string name = tbName.Text.Trim();
                string EventCode = ddlEventType.SelectedValue;
                bool isStopTimer = chkStopTimer.Checked;

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigTicketStatus(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code, name,isStopTimer,EventCode, ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigTicketStatus(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code, name,isStopTimer,EventCode, ERPWAuthentication.UserName);
                }

                BindingData();

                ClientService.DoJavascript("closeModal('Save');");
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                string code = btn.CommandArgument;

                lib.DeleteMasterConfigTicketStatus(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode, code);

                BindingData();

                ClientService.DoJavascript("closeModal('Delete');");
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

        public string convertTypeToDesc(string EventTypeCode)
        {
            if (mDicEvenType.ContainsKey(EventTypeCode))
            {
                return mDicEvenType[EventTypeCode];
            }
            return EventTypeCode;
        }
    }
}