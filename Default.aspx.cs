using agape.lib.constant;
using Agape.Lib.DBService;
using Agape.Lib.Web.Bean.CS;
using Newtonsoft.Json;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Service;
using ServiceWeb.crm.AfterSale;
using ServiceWeb.auth;
using ERPW.Lib.Master.Config;
using System.Web.Configuration;
using ERPW.Lib.Service.Entity;
using System.ComponentModel;

namespace ServiceWeb
{
    public partial class Default : AbstractsSANWebpage
    {
        public bool IsFilterOwner
        {
            get { return (Master as ServiceWeb.MasterPage.MasterPage).IsFilterOwner; }
        }
        
        //private static DBService dbService = new DBService();
        private ERPW.Lib.Service.Report.TicketAnalysis ta = new ERPW.Lib.Service.Report.TicketAnalysis();
        private DataTable datatable { get; set; }
        private TierZeroLibrary libTierZero = new TierZeroLibrary();
        private ServiceTicketLibrary libServiceTicket = new ServiceTicketLibrary();
        private MasterConfigLibrary libconfig = MasterConfigLibrary.GetInstance();
        private tmpServiceCallDataSet serviceCallEntity
        {
            get { return Session["ServicecallEntity"] == null ? new tmpServiceCallDataSet() : (tmpServiceCallDataSet)Session["ServicecallEntity"]; }
            set { Session["ServicecallEntity"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string thisDefaultPage = "/Default.aspx";
                if (thisDefaultPage != ERPWAuthentication.Permission.DefaultPage)
                {
                    Response.Redirect(Page.ResolveUrl("~" + ERPWAuthentication.Permission.DefaultPage), true);
                }

                //setDefualTicketAnalysis();
                setTierZeroList();
                BindMyTaskAndDelayRisk();
                ApprovalListControl.bindData("");
                //GetOwnerGroupService();

                //ClientService.DoJavascript("bindingDataTableJSOverdue();");
            }
            System.Diagnostics.Debug.WriteLine("employee code: " + ERPWAuthentication.EmployeeCode);
        }

        #region Tier 0
        /// <summary>
        /// divJsonTier0List control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divJsonTier0List;
        protected void setTierZeroList()
        {
            string OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
            //bool IsFilterOwner = false;
            //bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out IsFilterOwner);

            string SID = ERPWAuthentication.SID;
            string Companycode = ERPWAuthentication.CompanyCode;
            //int DisplayRow = 3;
            //List<TierZero> DisplayData = getTierZeroList(SID, Companycode, "", DisplayRow);
            List<TierZeroEn> lists = new List<TierZeroEn>();
            lists = libTierZero.getTierZeroList(
                SID, Companycode, TierZeroLibrary.TIER_ZERO_STATUS_OPEN,
                OwnerGroupCode, IsFilterOwner
            );

            lists.ForEach(r =>
            {
                //
                if (!string.IsNullOrEmpty(r.CREATED_ON) && r.CREATED_ON.Length >= 14)
                {
                    r.CREATED_ON = Validation.Convert2DateTimeDisplay(r.CREATED_ON);
                }
                else if (!string.IsNullOrEmpty(r.CREATED_ON) && r.CREATED_ON.Length >= 8)
                {
                    r.CREATED_ON = Validation.Convert2DateDisplay(r.CREATED_ON);
                }

                r.Channel = FormatChannel(r.Channel);
                r.ControlDisplayStatus = FormatStatus(r.Status);
                r.ControlDisplay = (!IsCanCreatedTicket(r.Status)).ToString();
            });

            //for(int index = 0; index < lists.Count; index++)
            //{
            //    lists[index].CREATED_ON = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(lists[index].CREATED_ON);
            //    lists[index].Channel = FormatChannel(lists[index].Channel);
            //    lists[index].ControlDisplayStatus = FormatStatus(lists[index].Status);
            //    lists[index].ControlDisplay = (!IsCanCreatedTicket(lists[index].Status)).ToString();
            //}
            //new code
            var Tier0List = lists;
            var dataSource = Tier0List.Select(s => new
            {
                s.CREATED_ON,
                s.Channel,
                s.Subject,
                s.CustomerName,
                s.SEQ,
                s.Status,
                s.ControlDisplayStatus,
                s.ControlDisplay
            });
            divJsonTier0List.InnerHtml = JsonConvert.SerializeObject(dataSource);
            ////old code
            ////rptListTierZero.DataSource = lists;
            ////rptListTierZero.DataBind();
            udpJsonTierList.Update();
            updTierZero.Update();
            ClientService.DoJavascript("setTier0List();");
            //ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected string FormatChannel(string strChannel)
        {
            return TierZeroLibrary.getChannelDescription(strChannel);
        }
        protected string FormatStatus(string strStatus)
        {
            return TierZeroLibrary.getStatusDescription(strStatus);
        }
        #endregion
        //private Report.ReportUnity report_unity = new Report.ReportUnity();
        private void BindMyTaskAndDelayRisk()
        {
            //DataSet ds = libServiceTicket.GetMyTask(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.EmployeeCode);

            //DataTable dt = ds.Tables["my_task"];
            //dt.DefaultView.ToTable(true, "TicketNoDisplay");
            //dt = report_unity.incidentNoFormater(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, dt, "TicketNoDisplay");
            //rptMyTask.DataSource = dt;
            //rptMyTask.DataBind();
            //ClientService.DoJavascript("bindingDataTableMyTask();");

            //dt = ds.Tables["dtUnAssignCI"];
            //dt.DefaultView.ToTable(true, "TicketNoDisplay");
            //dt = report_unity.incidentNoFormater(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, dt, "TicketNoDisplay");
            //rptUACI.DataSource = dt;
            //rptUACI.DataBind();

            //dt = ds.Tables["delay_risk"];
            //dt.DefaultView.ToTable(true, "TicketNoDisplay");
            //dt = report_unity.incidentNoFormater(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, dt, "TicketNoDisplay");
            //rptDelayRisk.DataSource = dt;
            //rptDelayRisk.DataBind();

            //ClientService.DoJavascript("bindingDataTableJSACI();");
        }

        ////28/08/2562 Un MyTask(by Zan)
        //protected void btnChange_Click(object sender, EventArgs e)
        //{            
        //    string[] keys = hdfChange.Value.Split('|');

        //    string docType = keys[0];
        //    string docNumber = keys[1];
        //    string fiscalYear = keys[2];
        //    string customer = keys[3];

        //    GetDataToedit(docType, docNumber, fiscalYear, customer);
        //}

        protected void GetDataToedit(string doctype, string docnumber, string fiscalyear, string customer)
        {
            ServiceCallFastEntryCriteria link = new ServiceCallFastEntryCriteria();
            string idGen = link.redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                //ServiceTicketLibrary libServiceTicket = new ServiceTicketLibrary();
                string PageRedirect = ServiceTicketLibrary.GetInstance().getPageTicketRedirect(
                    ERPWAuthentication.SID,
                    (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                );
                if (PageRedirect.Equals("ServiceCallTransaction.aspx")) {
                    ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                } else if (PageRedirect.Equals("ServiceCallTransactionChange.aspx")) {
                    ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                }
                else
                {
                    Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                }
                //Response-.Redirect("/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen, false);
            }
            #region Redirect Old
            //ICMUtils ICMService = WSHelper.getICMUtils();

            //Object[] objParam = new Object[] { "1500117",
            //        (string)Session[ApplicationSession.USER_SESSION_ID],
            //        ERPWAuthentication.CompanyCode,
            //        doctype,
            //        docnumber,
            //        fiscalyear };

            //DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
            //DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            //if (objReturn != null)
            //{
            //    serviceCallEntity = new tmpServiceCallDataSet();
            //    serviceCallEntity.Merge(objReturn.Copy());

            //    Session["SC_MODE"] = ApplicationSession.CHANGE_MODE_STRING;

            //    Response-.Redirect("~/crm/AfterSale/ServiceCallTransaction.aspx", false);
            //}
            #endregion
        }
        //13/11/2561 add filter by owner group(by born kk)
        //28/08/2562 Un filter by owner group(by Zan)
        //private void GetOwnerGroupService()
        //{

        //    DataTable dtOwner = libconfig.GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
        //    ddlOwnerGroupService.DataValueField = "OwnerGroupCode";
        //    ddlOwnerGroupService.DataTextField = "OwnerGroupName";
        //    ddlOwnerGroupService.DataSource = dtOwner;
        //    ddlOwnerGroupService.DataBind();
        //    ddlOwnerGroupService.Items.Insert(0, new ListItem("", ""));

        //}
        //13/11/2561 add filter by owner group(by born kk)
        protected void OnSelectedIndexChanged(object sender,EventArgs e) 
        {
            //string OwnerGroup = ddlOwnerGroupService.SelectedItem.Value;
            //DataSet ds = libServiceTicket.GetMyTask(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.EmployeeCode);
            //DataView dataView = ds.Tables["delay_risk"].DefaultView;
            //if (!string.IsNullOrEmpty(OwnerGroup))
            //{
            //    dataView.RowFilter = "OwnerGroupService = '" + OwnerGroup + "'";
            //}
            
            //rptDelayRisk.DataSource = dataView;
            //rptDelayRisk.DataBind();
            //updateTableOverdue.Update();
            //ClientService.DoJavascript("bindingDataTableJSOverdue();");
        }
        public bool IsCanCreatedTicket(string strStatus)
        {
            return TierZeroLibrary.TIER_ZERO_STATUS_OPEN == strStatus;
        }
        #region funtionDB
        //        private static List<TierZero> getTierZeroList(string sid, string CompanyCode, string Status, int row)
        //        {
        //            string sql = @"SELECT ";
        //            if (row > 0)
        //            {
        //                sql += "TOP(" + row.ToString() + ") ";
        //            }
        //            sql += @"       [SID]
        //                          ,[CompanyCode]
        //                          ,[SEQ]
        //                          ,[Channel]
        //                          ,[EMail]
        //                          ,[CustomerCode]
        //                          ,[CustomerName]
        //                          ,[TelNo]
        //                          ,[Subject]
        //                          ,[Detail]
        //                          ,[Status]
        //                          ,[TicketNumber]
        //                          ,[TicketType]
        //                          ,[CREATED_BY]
        //                          ,[CREATED_ON]
        //                          ,[UPDATED_BY]
        //                          ,[UPDATED_ON]
        //                      FROM [ERPW_Service_tier0]
        //                    where sid='" + sid + "' AND CompanyCode = '" + CompanyCode + "'";
        //            if (!string.IsNullOrEmpty(Status))
        //            {
        //                sql += " AND Status = '" + Status + "'";
        //            }
        //            DataTable dt = dbService.selectDataFocusone(sql);
        //            List<TierZero> momaster = JsonConvert.DeserializeObject<List<TierZero>>(JsonConvert.SerializeObject(dt));
        //            return momaster;
        //        }

        //        private static TierZero getTierZeroDetail(string sid, string CompanyCode, string SEQ)
        //        {
        //            TierZero Result = new TierZero();
        //            string sql = @"SELECT [SID]
        //                          ,[CompanyCode]
        //                          ,[SEQ]
        //                          ,[Channel]
        //                          ,[EMail]
        //                          ,[CustomerCode]
        //                          ,[CustomerName]
        //                          ,[TelNo]
        //                          ,[Subject]
        //                          ,[Detail]
        //                          ,[Status]
        //                          ,[TicketNumber]
        //                          ,[TicketType]
        //                          ,[CREATED_BY]
        //                          ,[CREATED_ON]
        //                          ,[UPDATED_BY]
        //                          ,[UPDATED_ON]
        //                      FROM [ERPW_Service_tier0]
        //                    where sid='" + sid + "' AND CompanyCode = '" + CompanyCode + "' AND SEQ =" + SEQ;
        //            DataTable dt = dbService.selectDataFocusone(sql);
        //            List<TierZero> momaster = JsonConvert.DeserializeObject<List<TierZero>>(JsonConvert.SerializeObject(dt));
        //            if (momaster.Count > 0)
        //            {
        //                Result = momaster.First();
        //            }
        //            return Result;
        //        }

        //        private void UpdateTierZeroStatus(string SID, string Companycode, string SEQ, string Value)
        //        {
        //            string DBDate = Validation.getCurrentServerDate();
        //            //string UserName = ERPWAuthentication.UserName;
        //            string UserName = "Focusone";
        //            string sql = @"UPDATE ERPW_Service_tier0
        //                           SET Status = '" + Value + @"'
        //                              ,UPDATED_BY = '" + UserName + @"'
        //                              ,UPDATED_ON = '" + DBDate + @"'
        //                         WHERE SID = '" + SID + @"'
        //                        AND CompanyCode = '" + Companycode + @"'
        //                        AND SEQ = " + SEQ;
        //            dbService.executeSQLForFocusone(sql);
        //        } 
        #endregion

        #region Class
        //public class TierZero
        //{
        //    public string SID { get; set; }
        //    public string CompanyCode { get; set; }
        //    public string SEQ { get; set; }
        //    public string Channel { get; set; }
        //    public string EMail { get; set; }
        //    public string CustomerCode { get; set; }
        //    public string CustomerName { get; set; }
        //    public string TelNo { get; set; }
        //    public string Subject { get; set; }
        //    public string Detail { get; set; }
        //    public string Status { get; set; }
        //    public string TicketNumber { get; set; }
        //    public string TicketType { get; set; }
        //    public string CREATED_BY { get; set; }
        //    public string CREATED_ON { get; set; }
        //    public string UPDATED_BY { get; set; }
        //    public string UPDATED_ON { get; set; }
        //}        
        #endregion

        private void setDefualTicketAnalysis()
        {
            datatable = ta.ticketAnalysisDefault(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode,ERPWAuthentication.EmployeeCode);
            rptMyChart.DataSource = datatable;
            rptMyChart.DataBind();
            udpMyChart.Update();

            //List<string>  list = SearchCustomerControl.listCustomerCode;
        }

        protected void btnRebindTierZeroList_Click(object sender, EventArgs e)
        {
            try
            {
                setTierZeroList();
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

        protected void btnLoadMyChart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkIsLoadMyChart.Checked)
                {
                    setDefualTicketAnalysis();
                    ClientService.DoJavascript("BindMyChart();");
                    chkIsLoadMyChart.Checked = true;
                }
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