using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Constant;
using ERPW.Lib.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class TicketStatusAuto : System.Web.UI.Page
    {
        private MasterConfigLibrary lib = new MasterConfigLibrary();
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
            DataTable dt = lib.GetMasterConfigTicketStatusAuto(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, null);

            dt.Columns.Add("TicketDocStatusDescBegin", typeof(string));
            dt.Columns.Add("TicketDocStatusDescTarget", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                string TicketStatusCodeBegin = dr["TicketStatusCodeBegin"].ToString();
                string TicketStatusCodeTarget = dr["TicketStatusCodeTarget"].ToString();

                dr["TicketDocStatusDescBegin"] = TicketStatusCodeBegin + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    TicketStatusCodeBegin);

                dr["TicketDocStatusDescTarget"] = TicketStatusCodeTarget + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(
                  ERPWAuthentication.SID,
                  ERPWAuthentication.CompanyCode,
                  TicketStatusCodeTarget);
            }
            rptItems.DataSource = dt;
            rptItems.DataBind();

            udpnItems.Update();


        }

        private Dictionary<string, string> _mDicEvenType;
        private Dictionary<string, string> mDicEvenType
        {
            get
            {
                if (_mDicEvenType == null)
                {
                    _mDicEvenType = ConfigurationConstant.GetMasterDataEventDocStatusDesc();
                    _mDicEvenType = _mDicEvenType.ToDictionary(obj => obj.Key, obj => getTicketStatusDescbyEventType(obj.Key));
                }
                return _mDicEvenType;
            }
        }
        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {

                ddlstatusbegin.DataSource = mDicEvenType;
                ddlstatusbegin.DataTextField = "Value";
                ddlstatusbegin.DataValueField = "Key";
                ddlstatusbegin.DataBind();
                ddlstatusbegin.Enabled = true;

                ddlstatustarget.DataSource = mDicEvenType;
                ddlstatustarget.DataTextField = "Value";
                ddlstatustarget.DataValueField = "Key";
                ddlstatustarget.DataBind();

                hdfMode.Value = ApplicationSession.CREATE_MODE_STRING;

                tbdelaytime.Text = "";

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
                string codebegin = hdfEditCode.Value;

                hdfEditCode.Value = "";

                DataTable dt = lib.GetMasterConfigTicketStatusAuto(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, codebegin);
                
                if (dt.Rows.Count > 0)
                {
                    double DelaySecond = Convert.ToDouble(dt.Rows[0]["DelayTime"].ToString());
                    bool isWorking = Convert.ToBoolean(dt.Rows[0]["WorkingStatus"].ToString());
                    string TicketStatusCodeBegin = dt.Rows[0]["TicketStatusCodeBegin"].ToString();
                    string TicketStatusCodeTarget = dt.Rows[0]["TicketStatusCodeTarget"].ToString();

                    string TicketDocStatusDescBegin = ServiceTicketLibrary.GetTicketDocStatusDesc(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    TicketStatusCodeBegin
                   );

                    string TicketDocStatusDescTarget = ServiceTicketLibrary.GetTicketDocStatusDesc(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    TicketStatusCodeTarget);

                    ddlstatusbegin.DataSource = mDicEvenType;
                    ddlstatusbegin.DataTextField = "Value";
                    ddlstatusbegin.DataValueField = "Key";
                    ddlstatusbegin.DataBind();

                    var beginKey = mDicEvenType.FirstOrDefault(x => x.Value == TicketStatusCodeBegin + " : " + TicketDocStatusDescBegin).Key;
                    ddlstatusbegin.SelectedValue = beginKey;
                    ddlstatusbegin.Enabled = false;

                    ddlstatustarget.DataSource = mDicEvenType;
                    ddlstatustarget.DataTextField = "Value";
                    ddlstatustarget.DataValueField = "Key";
                    ddlstatustarget.DataBind();

                    var targetKey = mDicEvenType.FirstOrDefault(x => x.Value == TicketStatusCodeTarget + " : " + TicketDocStatusDescTarget).Key;
                    ddlstatustarget.SelectedValue = targetKey;

                    tbdelaytime.Text = (DelaySecond / 60).ToString();

                    chkIsWorking.Checked = isWorking;

                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;

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
                string statusbegin = ddlstatusbegin.Text.Trim();
                string statustarget = ddlstatustarget.Text.Trim();
                double delaytime = Convert.ToDouble(tbdelaytime.Text.Trim());
                bool isWorking = chkIsWorking.Checked;

                if (statusbegin.Equals(statustarget))
                {
                    throw new Exception("Must not be the same ticket status.");
                }

                string TICKET_STATUS_EVENT_BEGIN_CODE = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, statusbegin);
                string TICKET_STATUS_EVENT_TARGET_CODE = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, statustarget);

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigTicketStatusAuto(
                        ERPWAuthentication.SID, 
                        ERPWAuthentication.CompanyCode,
                        TICKET_STATUS_EVENT_BEGIN_CODE,
                        TICKET_STATUS_EVENT_TARGET_CODE, 
                        isWorking, 
                        delaytime, 
                        ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigTicketStatusAuto(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        TICKET_STATUS_EVENT_BEGIN_CODE,
                        TICKET_STATUS_EVENT_TARGET_CODE,
                        isWorking,
                        delaytime,
                        ERPWAuthentication.UserName
                        );
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

                lib.DeleteMasterConfigTicketStatusAuto(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code);

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

        private string getTicketStatusDescbyEventType(string EventType)
        {
            string TicketStatusCode = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, EventType);
            
            return TicketStatusCode + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, TicketStatusCode);
        }

        public string ConvertToTime(string time, bool returnEmpty)
        {
            if (!string.IsNullOrEmpty(time))
            {
                double xTime = 0;
                double.TryParse(time, out xTime);

                if (xTime > 0)
                {
                    TimeSpan t = TimeSpan.FromSeconds(xTime);

                    string answer = "";
                    if (t.Days > 0)
                    {
                        answer += t.Days + " วัน ";
                    }
                    if (t.Days > 0 || t.Hours > 0)
                    {
                        answer += t.Hours + " ชม ";
                    }
                    if (t.Days > 0 || t.Hours > 0 || t.Minutes > 0)
                    {
                        answer += t.Minutes + " นาที ";
                    }
                    if (t.Days > 0 || t.Hours > 0 || t.Minutes > 0 || t.Seconds > 0)
                    {
                        answer += t.Seconds + " วินาที ";
                    }

                    return answer;
                }
            }

            if (returnEmpty)
            {
                return "";
            }

            return "ไม่กำหนด";
        }

        public string formatWorkingStatus(string workingStatus)
        {
            string resStatus = "Inactive";
            bool workingStatusBool = Convert.ToBoolean(workingStatus);

            if (workingStatusBool)
            {
                resStatus = "Active";
            }

            return resStatus;
        }

    }
}