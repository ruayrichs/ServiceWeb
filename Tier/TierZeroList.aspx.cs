using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.auth;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ServiceWeb.Service;
using Agape.Lib.Web.Bean.CS;
using ServiceWeb.Report;
using System.Web.Configuration;

namespace ServiceWeb.Tier
{
    public partial class TierZeroList : AbstractsSANWebpage
    {
        //private static DBService dbService = new DBService();

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.TierZeroView;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.TierZeroModify;
        }
        private TierZeroLibrary libTierZero = new TierZeroLibrary();
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private UniversalService universalService = new UniversalService();
        private ServiceWeb.Report.ReportUnity report_unity = new ReportUnity();
        private string _SID;
        protected string SID
        {
            get
            {
                if (_SID == null)
                {
                    _SID = ERPWAuthentication.SID;
                }
                return _SID;
            }
        }

        private string _CompanyCode;
        protected string CompanyCode
        {
            get
            {
                if (_CompanyCode == null)
                {
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                }
                return _CompanyCode;
            }
        }

        DataTable dtPriority
        {
            get { return Session["ServiceCallCriteria.SCT_dtPriority"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCT_dtPriority"]; }
            set { Session["ServiceCallCriteria.SCT_dtPriority"] = value; }

        }

        DataTable dtTempDoc
        {
            get
            {
                if (Session["SC_dtTempDoc"] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("doctype");
                    dt.Columns.Add("docnumber");
                    dt.Columns.Add("docfiscalyear");
                    dt.Columns.Add("indexnumber");
                    Session["SC_dtTempDoc"] = dt;
                }
                return (DataTable)Session["SC_dtTempDoc"];
            }
            set { Session["SC_dtTempDoc"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindListTierZero();
                initDataListType();
                //initData();
            }
        }

        private void bindListTierZero()
        {
            string OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
            bool IsFilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out IsFilterOwner);

            DataTable dt = new DataTable();
            List<TierZeroEn> lists = new List<TierZeroEn>();
            lists = libTierZero.getTierZeroList(SID, CompanyCode, "", OwnerGroupCode, IsFilterOwner);
            for (int index = 0; index < lists.Count; index++)
            {
                lists[index].CREATED_ON = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(lists[index].CREATED_ON);
                lists[index].Channel = FormatChannel(lists[index].Channel);
                lists[index].ControlDisplayStatus = FormatStatus(lists[index].Status);
                lists[index].ControlDisplay = (!IsCanCreatedTicket(lists[index].Status)).ToString();
            }
            dt = lists.toDataTable();
            dt = report_unity.incidentNoFormater(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, dt, "TicketNumber");
            lists = JsonConvert.DeserializeObject<List<TierZeroEn>>(JsonConvert.SerializeObject(dt)); // dt.toList<TierZeroEn>();
            var dataSource = lists.Select(s=>new
            {
                s.ControlDisplayStatus,
                s.ControlDisplay,
                s.CREATED_ON,
                s.Channel,
                s.SEQ,
                s.EMail,
                s.CustomerCode,
                s.CustomerName,
                s.TelNo,
                s.Subject,
                //s.Detail,
                s.Status,
                s.TicketNumber,
                s.TicketType,
            });
            divJsonTier0List.InnerHtml = JsonConvert.SerializeObject(dataSource).Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br />");
            //rptListDataUpload.DataSource = dt;
            //rptListDataUpload.DataBind();
            updFastService.Update();
            udpJsonTierList.Update();
            ClientService.DoJavascript("setTier0List();");            
        }

        private void initDataListType()
        {
            ddlListType.DataTextField = "Description";
            ddlListType.DataValueField = "Code";

            ddlListType.DataSource = TierZeroLibrary.getDataStatus();
            ddlListType.DataBind();
            ddlListType.Items.Insert(0, new ListItem("Default", ""));
        }

        protected void ddlListType_Change(object sender, EventArgs e)
        {
            try
            {
                string selectType = ddlListType.SelectedValue;
                //System.Diagnostics.Debug.WriteLine(selectType);
                rptListDataUpload.DataSource = libTierZero.getTierZeroList(SID, CompanyCode, selectType);
                rptListDataUpload.DataBind();
                updFastService.Update();
                //ClientService.DoJavascript("$('#tableItems').DataTable({});");

            }
            catch (Exception ex)
            {
                ClientService.DoJavascript(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
            ClientService.DoJavascript("$('#tableItems').DataTable({});");
        }

        protected string FormatChannel(string strChannel)
        {
            return TierZeroLibrary.getChannelDescription(strChannel);
        }
        protected string FormatStatus(string strStatus)
        {
            return TierZeroLibrary.getStatusDescription(strStatus);
        }

        public bool IsCanCreatedTicket(string strStatus)
        {
            return TierZeroLibrary.TIER_ZERO_STATUS_OPEN == strStatus;
        }

        protected void btnRebindList_Click(object sender, EventArgs e)
        {
            try
            {
                bindListTierZero();
                //ClientService.DoJavascript("setTier0List();");
                udpJsonTierList.Update();
            }
            catch (Exception ex)
            {
                ClientService.DoJavascript(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

    }
}