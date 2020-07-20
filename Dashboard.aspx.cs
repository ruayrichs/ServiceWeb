using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.crm.AfterSale;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.Service;

namespace ServiceWeb
{
    public partial class Dashboard : AbstractsSANWebpage//System.Web.UI.Page
    {

        public bool IsFilterOwner
        {
            get { return (Master as ServiceWeb.MasterPage.MasterPage).IsFilterOwner; }
        }

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.DashboardView || ERPWAuthentication.Permission.DashboardViewAll || ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.DashboardView || ERPWAuthentication.Permission.DashboardViewAll || ERPWAuthentication.Permission.AllPermission;
        }
        private DashboardLib dashboardLib = new DashboardLib();
        private string _SID;
        private string _CompanyCode;
        private string _CustomerCode;
        private DashboardFinalDataModel _DashboardFinalData;

        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = ERPWAuthentication.SID;
                return _SID;
            }
        }
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                return _CompanyCode;
            }
        }
        private string CustomerCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CustomerCode))
                    _CustomerCode = ERPWAuthentication.EmployeeCode;
                return _CustomerCode;
            }
        }

        private DashboardFinalDataModel DashboardFinalData
        {
            get
            {
                bool _IsFilterOwner = IsFilterOwner;
                if (ERPWAuthentication.Permission.DashboardViewAll)
                {
                    _IsFilterOwner = false;
                }
                if (_DashboardFinalData == null)
                    _DashboardFinalData = dashboardLib.PreparFinanDataDashboard(
                        SID, CompanyCode, "today", _IsFilterOwner, ERPWAuthentication.Permission.OwnerGroupCode, CustomerCode
                    );
                return _DashboardFinalData;
            }
        }

        #region Pie
        protected int incident_count
        {
            get
            {
                //DashboardFinalDataModel dfdm = new DashboardFinalDataModel();
                //dfdm = DashboardFinalData;
                //System.Diagnostics.Debug.WriteLine("SID: " + SID + " ||| CompanyCode: "+CompanyCode);
                //System.Diagnostics.Debug.WriteLine("incidentCount: "+dfdm.PieChartDataReport.CountTicketIncident);
                return DashboardFinalData.PieChartDataReport.CountTicketIncident;
            }
        }
        protected int request_count
        {
            get
            {
                return DashboardFinalData.PieChartDataReport.CountTicketRequest;
            }
        }
        protected int problem_count
        {
            get
            {
                return DashboardFinalData.PieChartDataReport.CountTicketProblem;
            }
        }
        protected int change_count
        {
            get
            {
                return DashboardFinalData.PieChartDataReport.CountTicketChangeOrder;
            }
        }
        #endregion

        #region BarChart

        protected int i_open_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketIncident_InProgress;
            }
        }
        protected int i_close_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketIncident_Close;
            }
        }
        protected int i_cancel_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketIncident_Cancel;
            }
        }

        protected int p_open_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketProblem_InProgress;
            }
        }
        protected int p_close_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketProblem_Close;
            }
        }
        protected int p_cancel_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketProblem_Cancel;
            }
        }

        protected int r_open_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketRequest_InProgress;
            }
        }
        protected int r_close_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketRequest_Close;
            }
        }
        protected int r_cancel_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketRequest_Cancel;
            }
        }

        protected int c_open_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketChangeOrder_InProgress;
            }
        }
        protected int c_close_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketChangeOrder_Close;
            }
        }
        protected int c_cancel_count
        {
            get
            {
                return DashboardFinalData.BarChartDataReport.CountTicketChangeOrder_Cancel;
            }
        }
        #endregion
       

        #region line
        protected string datasProblemGroup
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.LineChartDataReport.datasProblemGroup);
            }
        }

        protected string dataCountTicketIncident
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.LineChartDataReport.dataCountTicketIncident);
            }
        }
        protected string dataCountTicketProblem
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.LineChartDataReport.dataCountTicketProblem);
            }
        }
        protected string dataCountTicketRequest
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.LineChartDataReport.dataCountTicketRequest);
            }
        }
        protected string dataCountTicketChangeOrder
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.LineChartDataReport.dataCountTicketChangeOrder);
            }
        }
        #endregion
        private ServiceTicketLibrary libServiceTicket = new ServiceTicketLibrary();

        private listTicketServiceEn _ListTicketEn;
        private listTicketServiceEn ListTicketEn
        {
            get
            {
                if (_ListTicketEn == null)
                {
                    _ListTicketEn = libServiceTicket.GetAllTaskDash(
                       ERPWAuthentication.SID,
                       ERPWAuthentication.CompanyCode
                   );
                }
                return _ListTicketEn;
            }
        }
       
        #region Ticket per month
        public string LabelofLineAll
        {
            get
            {
                DataTable dt = dashboardLib.getCountTicketIRPperMonth(SID, CompanyCode, CustomerCode);
                List<string> list = dt.AsEnumerable()
                             .Select(r => r.Field<string>("CreatedonDate"))
                             .ToList();
                return JsonConvert.SerializeObject(list);
            }
        }
        public string DatasofLineOpen
        {
            get
            {
                DataTable dt = dashboardLib.getCountTicketIRPperMonth(SID, CompanyCode, CustomerCode);
                List<string> list = dt.AsEnumerable()
                             .Select(r => r.Field<string>("Open_Count"))
                             .ToList();
                return JsonConvert.SerializeObject(list);
            }
        }
        public string DatasofLineCloseandResolve
        {
            get
            {
                DataTable dt = dashboardLib.getCountTicketIRPperMonth(SID, CompanyCode, CustomerCode);
                List<string> list = dt.AsEnumerable()
                             .Select(r => r.Field<string>("CloseResolve_Count"))
                             .ToList();
                return JsonConvert.SerializeObject(list);
            }
        }
        #endregion

        #region DoughnutChart I R P
        public string dataChartInc
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketCountofDoctype.DataofInc);
            }
        }
        public string dataChartReq 
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketCountofDoctype.DataofReq);
            }
        }
        public string dataIncChartProb 
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketCountofDoctype.DataofPromb);
            }
        }
        #endregion

        #region Rating Chart
        public string RatingData
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketRatingCount.RatingDatas);
            }
        }
        #endregion

        #region Ticket Status
        public string TicketStatusData
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketOnTimeOverdueCount.Datas);
            }
        }
        #endregion

        #region BarUserChart
        public string LabelsBarUserChart
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketOnHandCount.Labels);
            }
        }
        public string Data1BarUserChart
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketOnHandCount.DatasetOpen);
            }
        }
        public string Data2BarUserChart
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketOnHandCount.DatasetResolve);
            }
        }

        public string Color1BarUserChart
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketOnHandCount.ColorsetOpen);
            }
        }

        public string Color2BarUserChart
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataTicketOnHandCount.ColorsetResolve);
            }
        }
        #endregion

        #region BarEquipmentChart
        public string EquipmentChartLabel
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataEquipmentCount.Labels);
            }
        }
        public string EquipmentChartData
        {
            get
            {
                return JsonConvert.SerializeObject(DashboardFinalData.DataforNewChartReport.DataEquipmentCount.Datas);
            }
        }
        #endregion
        private void pageinit()
        {
            lblCountOpen.Text = DashboardFinalData.OverviewDataReport.CountTicketOpen.ToString();
            //lblCountUnassigned.Text = DashboardFinalData.OverviewDataReport.CountTicketUnassigned.ToString();
            lblCountDelay.Text = DashboardFinalData.OverviewDataReport.CountTicketDelay.ToString();
            lblCountSuccess.Text = DashboardFinalData.OverviewDataReport.CountTicketFinish.ToString();
            lblCountAll.Text = DashboardFinalData.OverviewDataReport.CountTicketAll.ToString();

            //DataTable ISL_dt = dashboardLib.getFinalISL();
            //foreach (DataRow dataRow in ISL_dt.Rows)
            //{
            //    System.Diagnostics.Debug.WriteLine("");
            //    foreach (var item in dataRow.ItemArray)
            //    {
            //        //Console.WriteLine(item);
            //        System.Diagnostics.Debug.WriteLine(item);
            //    }
            //}
            //

            rtpPriority.DataSource = DashboardFinalData.PiorityDataReport;
            rtpPriority.DataBind();
            udpPriority.Update();

            //DataTable TicketStatus_dt;
            ////TicketStatus_dt = dashboardLib.getFinalTicketStatus();
            //TicketStatus_dt = dashboardLibV2.fetchAll_ETS_ForDashboard().toDataTable();
            rptTS.DataSource = DashboardFinalData.StatusDataReport;
            rptTS.DataBind();
            udpTS.Update();

            rptCICountFamily.DataSource = dashboardLib.getCountCIwithFamily(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.EmployeeCode);
            rptCICountFamily.DataBind();
            udpnCICountFamily.Update();

            rptCICountClass.DataSource = dashboardLib.getCountCIwithClass(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.EmployeeCode);
            rptCICountClass.DataBind();
            udpnCICountClass.Update();
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

        protected void btnLinkTransactionSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string CallerID = hddCallerID_Criteria.Value;
                List<TicketServiceEn> enList = ListTicketEn.ListTicket_All.Where(w =>
                    w.CallerID.Equals(CallerID)
                ).ToList();

                dtTempDoc.Clear();
                int i = 1;
                enList.ForEach(r =>
                {
                    DataRow drt = dtTempDoc.NewRow();
                    drt["doctype"] = r.Doctype;
                    drt["docnumber"] = r.CallerID;
                    drt["docfiscalyear"] = r.Fiscalyear;
                    drt["indexnumber"] = i++;
                    dtTempDoc.Rows.Add(drt);
                });

                if (enList.Count > 0)
                {
                    getdataToedit(enList[0].Doctype, enList[0].CallerID, enList[0].Fiscalyear);
                }
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

        protected void getdataToedit(string doctype, string docnumber, string fiscalyear)
        {

            ServiceCallFastEntryCriteria link = new ServiceCallFastEntryCriteria();
            string idGen = link.redirectViewToTicketDetail(CustomerCode, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                ServiceTicketLibrary lib_TK = new ServiceTicketLibrary();
                string PageRedirect = lib_TK.getPageTicketRedirect(
                    SID,
                    (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                );
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                //Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                //Response.-Redirect("/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen, false);
            }

            //Object[] objParam = new Object[] { "1500117",
            //        (string)Session[ApplicationSession.USER_SESSION_ID],
            //        CompanyCode,doctype,docnumber,fiscalyear};

            //DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
            //DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            //if (objReturn != null)
            //{
            //    serviceCallEntity = new tmpServiceCallDataSet();
            //    serviceCallEntity.Merge(objReturn.Copy());
            //    mode_stage = ApplicationSession.CHANGE_MODE_STRING;
            //    Response.-Redirect("~/crm/AfterSale/ServiceCallTransaction.aspx", false);
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pageinit();
               

            }
        }

        protected void btnLinkTransactionSearchCI_Click(object sender, EventArgs e)
        {
            string ciSearchKey = hddSearchCIKey.Value;
            string ciSearchValue = hddSearchCIValue.Value;
            Response.Redirect(Page.ResolveUrl("~/crm/Master/Equipment/EquipmentCriteria.aspx?ci_search_key=" + ciSearchKey + "&ci_search_value=" + ciSearchValue));
        }
    }
}