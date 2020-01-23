using agape.lib.constant;
using Agape.Lib.Web.Bean.CS;
using ServiceWeb.Accountability.Service;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.crm.AfterSale;
using ERPW.Lib.Service.Workflow;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Workflow.Entity;

namespace ServiceWeb.Accountability.UserControl
{
    public partial class ApprovalListControl : System.Web.UI.UserControl
    {
        WorkflowService ServiceWorkflow = new WorkflowService();

        public string WorkGroupCode
        {
            get
            {
                return hddWorkGroupCode.Value;
            }
        }
        public bool showAllList
        {
            get;
            set;
        }
        public bool showListUpgrade
        {
            get;
            set;
        }
        public bool showListDowngrade
        {
            get;
            set;
        }
        public bool showListCancel
        {
            get;
            set;
        }
        public bool showListNextApproval
        {
            get;
            set;
        }
        public bool showListApproved
        {
            get;
            set;
        }
        private DataTable _dtEventObject;
        private DataTable dtEventObject
        {
            get
            {
                if (_dtEventObject == null)
                {
                    _dtEventObject = ServiceWorkflow.getAllEventObject();
                }
                return _dtEventObject;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void bindData(string WorkgroupCode)
        {
            hddWorkGroupCode.Value = WorkgroupCode;
            udpCodeidentityInitiative.Update();

            if (showListUpgrade || showAllList)
            {
                bindlistUpgradeInitiative();
                panelListUpgrade.Visible = true;
            }
            else
            {
                panelListUpgrade.Visible = false;
            }

            if (showListDowngrade || showAllList)
            {
                bindlistDowngradeInitiative();
                panelDowngrade.Visible = true;
            }
            else
            {
                panelDowngrade.Visible = false;
            }

            if (showListCancel || showAllList)
            {
                bindlistCancelInitiative();
                panelCancel.Visible = true;
            }
            else
            {
                panelCancel.Visible = false;
            }

            if (showListNextApproval || showAllList)
            {
                bindlistNextApprovalInitiative();
                panelListMyNextApproval.Visible = true;
            }
            else
            {
                panelListMyNextApproval.Visible = false;
            }

            if (showListApproved || showAllList)
            {
                bindlistMyApprovedInitiative();
                panelListMyApproved.Visible = true;
            }
            else
            {
                panelListMyApproved.Visible = false;
            }

            udpTableUpgrade.Update();
            udpTableDowngrade.Update();
            udpTableCancel.Update();            
            //ClientService.DoJavascript("bindingDataTableCancelApproval();");
            //ClientService.DoJavascript("bindingDataTableNextApproval();");
            //ClientService.DoJavascript("bindingDataTableStepDown();");
            //ClientService.DoJavascript("bindingDataTableStepUp();");
            //ClientService.DoJavascript("hideIsNoApproval();");
        }

        private void bindlistUpgradeInitiative()
        {
            DataTable dt = ServiceWorkflow.getListInitiativeRequestUpgradeManagement(
                "",
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ERPWAuthentication.EmployeeCode,
                WorkGroupCode
            );

            dt.Columns.Add("WorkFlowStatus", typeof(string));
            List<string> listAobjectLink = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                listAobjectLink.Add(Convert.ToString(dr["AOBJECTLINK"]));
            }
            List<ApprovalHeader> listEn = ServiceWorkflow.GetApprovalHeaderPresentObject(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode, 
                WorkGroupCode, 
                listAobjectLink
            );
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    ApprovalHeader StateGate = listEn.Where(w => 
                        w.InitiativeCode == Convert.ToString(dr["AOBJECTLINK"])
                    ).First();

                    dr["WorkFlowStatus"] = getWorkFlowStatus(StateGate);
                }
                catch (Exception)
                {
                    dr["WorkFlowStatus"] = "";
                }
            }

            //if (dt.Rows.Count > 0)
            //{
            //    panelNoListUpgrade.Visible = false;
            //}
            //else
            //{
            //    //panelNoListUpgrade.Visible = true;

            AccountabilityService accountabilityService = new AccountabilityService();
            DataTable dtAcc = accountabilityService.getAccountabilityStructureV2(ERPWAuthentication.SID, "");

            DataTable ddt = dt.DefaultView.ToTable(
                true, "DOCNUMBER", "JOBDESCRIPTION", "AOBJECTLINK", "DateTime",
                "PROJECTCODE", "DocNumberDisplay", "SUBPROJECT", "WorkFlowStatus"
            );

            foreach (DataRow item in ddt.Rows)
            {
                foreach (DataRow iitem in dtAcc.Rows)
                {
                    if (item["SUBPROJECT"].ToString() == iitem["DataValue"].ToString())
                    {
                        item["SUBPROJECT"] = iitem["DataText"].ToString();
                    }
                }
            }

            rptListUpgrade.DataSource = ddt;
            rptListUpgrade.DataBind();
            ClientService.DoJavascript("bindingDataTableStepUp();");
        }

        private void bindlistNextApprovalInitiative()
        {
            DataTable dt = ServiceWorkflow.getListInitiativeMyNextApproval(
                "",
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ERPWAuthentication.EmployeeCode,
                WorkGroupCode
            );

            dt.Columns.Add("WorkFlowStatus", typeof(string));
            List<string> listAobjectLink = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                listAobjectLink.Add(Convert.ToString(dr["AOBJECTLINK"]));
            }
            List<ApprovalHeader> listEn = ServiceWorkflow.GetApprovalHeaderPresentObject(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                WorkGroupCode,
                listAobjectLink
            );
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    ApprovalHeader StateGate = listEn.Where(w =>
                        w.InitiativeCode == Convert.ToString(dr["AOBJECTLINK"])
                    ).First();

                    dr["WorkFlowStatus"] = getWorkFlowStatus(StateGate);
                }
                catch (Exception)
                {
                    dr["WorkFlowStatus"] = "";
                }
            }

            //if (dt.Rows.Count > 0)
            //{
            //    panelNoListNextApproval.Visible = false;
            //}
            //else
            //{
            //    //panelNoListNextApproval.Visible = true;
            //}

            AccountabilityService accountabilityService = new AccountabilityService();
            DataTable dtAcc = accountabilityService.getAccountabilityStructureV2(ERPWAuthentication.SID, "");

            DataTable ddt = dt.DefaultView.ToTable(
                true, "DOCNUMBER", "JOBDESCRIPTION", "AOBJECTLINK", "DateTime",
                "PROJECTCODE", "DocNumberDisplay", "SUBPROJECT", "WorkFlowStatus"
            );

            foreach (DataRow item in ddt.Rows)
            {
                foreach (DataRow iitem in dtAcc.Rows)
                {
                    if (item["SUBPROJECT"].ToString() == iitem["DataValue"].ToString())
                    {
                        item["SUBPROJECT"] = iitem["DataText"].ToString();
                    }
                }
            }

            rptListMyNextApproval.DataSource = ddt;

            


            rptListMyNextApproval.DataBind();
            ClientService.DoJavascript("bindingDataTableNextApproval();");
        }
        private void bindlistMyApprovedInitiative()
        {
            DataTable dt = ServiceWorkflow.getListInitiativeMyApproved(
                "",
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ERPWAuthentication.EmployeeCode,
                WorkGroupCode
            );

            dt.Columns.Add("WorkFlowStatus", typeof(string));
            List<string> listAobjectLink = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                listAobjectLink.Add(Convert.ToString(dr["AOBJECTLINK"]));
            }
            List<ApprovalHeader> listEn = ServiceWorkflow.GetApprovalHeaderPresentObject(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                WorkGroupCode,
                listAobjectLink
            );
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    ApprovalHeader StateGate = listEn.Where(w =>
                        w.InitiativeCode == Convert.ToString(dr["AOBJECTLINK"])
                    ).First();

                    dr["WorkFlowStatus"] = getWorkFlowStatus(StateGate);
                }
                catch (Exception)
                {
                    dr["WorkFlowStatus"] = "";
                }
            }

            //if (dt.Rows.Count > 0)
            //{
            //    panelNoListMyApproved.Visible = false;
            //    //ClientService.DoJavascript("bindingDataTableMyDocument();");
            //}
            //else
            //{
            //   // panelNoListMyApproved.Visible = true;
            //}

            AccountabilityService accountabilityService = new AccountabilityService();
            DataTable dtAcc = accountabilityService.getAccountabilityStructureV2(ERPWAuthentication.SID, "");

            DataTable ddt = dt.DefaultView.ToTable(
                true, "DOCNUMBER", "JOBDESCRIPTION", "AOBJECTLINK", "DateTime",
                "PROJECTCODE", "DocNumberDisplay", "SUBPROJECT", "WorkFlowStatus"
            );

            foreach (DataRow item in ddt.Rows)
            {
                foreach (DataRow iitem in dtAcc.Rows)
                {
                    if (item["SUBPROJECT"].ToString() == iitem["DataValue"].ToString())
                    {
                        item["SUBPROJECT"] = iitem["DataText"].ToString();
                    }
                }
            }

            rptListMyApproved.DataSource = ddt;
            rptListMyApproved.DataBind();
            ClientService.DoJavascript("bindingDataTableMyDocument();");

        }

        private void bindlistDowngradeInitiative()
        {
            DataTable dt = ServiceWorkflow.getListInitiativeRequestDowngradeManagement(
                "",
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ERPWAuthentication.EmployeeCode,
                WorkGroupCode
            );

            dt.Columns.Add("WorkFlowStatus", typeof(string));
            List<string> listAobjectLink = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                listAobjectLink.Add(Convert.ToString(dr["AOBJECTLINK"]));
            }
            List<ApprovalHeader> listEn = ServiceWorkflow.GetApprovalHeaderPresentObject(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                WorkGroupCode,
                listAobjectLink
            );
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    ApprovalHeader StateGate = listEn.Where(w =>
                        w.InitiativeCode == Convert.ToString(dr["AOBJECTLINK"])
                    ).First();

                    dr["WorkFlowStatus"] = getWorkFlowStatus(StateGate);
                }
                catch (Exception)
                {
                    dr["WorkFlowStatus"] = "";
                }
            }

            //if (dt.Rows.Count > 0)
            //{
            //    panelNolistDowngrade.Visible = false;

            //}
            //else
            //{
            //    //panelNolistDowngrade.Visible = true;
            //}

            rptlistDowngrade.DataSource = dt.DefaultView.ToTable(
                true, "DOCNUMBER", "JOBDESCRIPTION", "AOBJECTLINK", "DateTime",
                "PROJECTCODE", "DocNumberDisplay", "WorkFlowStatus"
            );
            rptlistDowngrade.DataBind();
            ClientService.DoJavascript("bindingDataTableStepDown();");
        }
        private void bindlistCancelInitiative()
        {
            DataTable dt = ServiceWorkflow.getListInitiativeRequestCancelManagement(
                "",
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ERPWAuthentication.EmployeeCode,
                WorkGroupCode
            );

            dt.Columns.Add("WorkFlowStatus", typeof(string));
            List<string> listAobjectLink = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                listAobjectLink.Add(Convert.ToString(dr["AOBJECTLINK"]));
            }
            List<ApprovalHeader> listEn = ServiceWorkflow.GetApprovalHeaderPresentObject(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                WorkGroupCode,
                listAobjectLink
            );
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    ApprovalHeader StateGate = listEn.Where(w =>
                        w.InitiativeCode == Convert.ToString(dr["AOBJECTLINK"])
                    ).First();

                    dr["WorkFlowStatus"] = getWorkFlowStatus(StateGate);
                }
                catch (Exception)
                {
                    dr["WorkFlowStatus"] = "";
                }
            }

            //if (dt.Rows.Count > 0)
            //{
            //    panelNoListCancel.Visible = false;
            //}
            //else
            //{
            //    //panelNoListCancel.Visible = true;
            //}

            rptListCancel.DataSource = dt.DefaultView.ToTable(
                true, "DOCNUMBER", "JOBDESCRIPTION", "AOBJECTLINK", "DateTime",
                "PROJECTCODE", "DocNumberDisplay", "WorkFlowStatus"
            );
            rptListCancel.DataBind();
            ClientService.DoJavascript("bindingDataTableCancelApproval();");
        }

        protected void btnRefreshDataApprove_Click(object sender, EventArgs e)
        {
            try
            {
                showAllList = true;
                bindData(WorkGroupCode);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void btnRefreshDataApproveNoload_Click(object sender, EventArgs e)
        {
            try
            {
                showAllList = true;
                bindData(WorkGroupCode);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        public string GetCurrentStateGate(string initiativeCode)
        {
            DataTable dtActiveStategate = ServiceWorkflow.GetApprovalHeaderPresentDatatable(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, initiativeCode);
            string StateGateDesc = "";
            if (dtActiveStategate.Rows.Count > 0)
            {
                DataRow drSG = dtActiveStategate.Rows[0];
                string SGFrom = drSG["STATEGATEFROM"].ToString();
                StateGateDesc = SGFrom;

                //if (SGFrom.Equals("L0"))
                //    StateGateDesc = "IL0 Idea Gethering";
                //if (SGFrom.Equals("L1"))
                //    StateGateDesc = "IL1 Validation Plan";
                //if (SGFrom.Equals("L2"))
                //    StateGateDesc = "IL2 Validation";
                //if (SGFrom.Equals("L3"))
                //    StateGateDesc = "IL3 Milestone & KPI Tracking";
                //if (SGFrom.Equals("L4"))
                //    StateGateDesc = "IL4 Tracking";
            }

            return StateGateDesc;
        }

        public string GetStatusStateGate(string initiativeCode)
        {
            DataTable dtActiveStategate = ServiceWorkflow.GetApprovalHeaderPresentDatatable(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, initiativeCode);
            string StateGateStatus = "";
            if (dtActiveStategate.Rows.Count > 0)
            {
                DataRow drSG = dtActiveStategate.Rows[0];
                string SGFrom = drSG["STATEGATEFROM"].ToString();
                string SGTo = drSG["STATEGATETO"].ToString();
                bool isRequestedUpgrade = Convert.ToBoolean(drSG["APPROVESTARTED"]);
                bool isRequestedDownGrade = Convert.ToBoolean(drSG["REQUESTAPPROVADOWNGRADE"]);

                bool isWaitingApprove = false;
                if (isRequestedUpgrade)
                {
                    isWaitingApprove = true;
                    StateGateStatus = "Submited for I" + SGTo + " approval";
                }
                else
                {
                    string[] DownGradedStateGete = ServiceWorkflow.GetdowngradedHeaderPresent(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        initiativeCode
                        ).Split(',');

                    if (DownGradedStateGete.Length == 3 && Convert.ToBoolean(DownGradedStateGete[2]))
                    {
                        isWaitingApprove = true;
                        StateGateStatus = "Submited for downgrade to IL" + (Convert.ToInt32(SGFrom.Substring(1, 1)) - 1) + " approval";
                    }
                    else
                        StateGateStatus = "Working on stategate I" + SGFrom;
                }
            }
            else
            {
                StateGateStatus = "Working on stategate IL5";
            }

            return StateGateStatus;
        }

        private string getWorkFlowStatus(ApprovalHeader StateGate)
        {
            string WorkFlowStatus = "";
            if (!string.IsNullOrEmpty(StateGate.StategateFrom))
            {
                if (StateGate.ApproveStarted)
                {
                    WorkFlowStatus = "Wait for " + getDescriptionEventObject(
                        StateGate.StategateTo
                    ) + " approval (Step Up)";
                }
                else if (StateGate.RequestApprovaDowngrade)
                {
                    WorkFlowStatus = "Wait for " + getDescriptionEventObject(
                        StateGate.StategateTo
                    ) + " approval (Step Down)";
                }
                else
                {
                    if (StateGate.StategateFrom == "L0" && !StateGate.ApproveStatus)
                    {
                        WorkFlowStatus = "Workflow Not Start.";
                    }
                    else if (StateGate.WorkflowSuccess)
                    {
                        WorkFlowStatus = "Workflow Success";
                    }
                    else
                    {
                        WorkFlowStatus = getDescriptionEventObject(
                            StateGate.StategateFrom
                        ) + " approved";
                    }
                }
            }
            else
            {
                WorkFlowStatus = "Workflow Success";
            }
            return WorkFlowStatus;
        }
        private string getDescriptionEventObject(string EventCode)
        {
            DataRow[] drr = dtEventObject.Select("EventCode = '" + EventCode + "'");
            if (drr.Count() > 0)
                return Convert.ToString(drr[0]["EventDesc"]);

            return EventCode;
        }
        
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private tmpServiceCallDataSet serviceCallEntity
        {
            get { return Session["ServicecallEntity"] == null ? new tmpServiceCallDataSet() : (tmpServiceCallDataSet)Session["ServicecallEntity"]; }
            set { Session["ServicecallEntity"] = value; }
        }
        public string mode_stage
        {
            get
            {
                if (Session["SC_MODE"] == null)
                { Session["SC_MODE"] = ApplicationSession.CREATE_MODE_STRING; }
                return (string)Session["SC_MODE"];
            }
            set { Session["SC_MODE"] = value; }
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

        protected void btnOpenDocument_Click(object sender, EventArgs e)
        {
            try
            {
                string docnumber = (sender as Button).CommandArgument;

                DataTable dtDataSearch = AfterSaleService.getInstance().getServiceCall(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "", "", "", "", "", "", "", "", 
                    docnumber,
                    "", ""
                );

                DataRow[] drr = dtDataSearch.Select("CallerID='" + docnumber + "'");
                dtTempDoc.Clear();
                int i = 1;
                foreach (DataRow dr in dtDataSearch.Rows)
                {
                    DataRow drt = dtTempDoc.NewRow();
                    drt["doctype"] = dr["Doctype"].ToString();
                    drt["docnumber"] = dr["CallerID"].ToString();
                    drt["docfiscalyear"] = dr["Fiscalyear"].ToString();
                    drt["indexnumber"] = i++;
                    dtTempDoc.Rows.Add(drt);
                }

                if (drr.Length > 0)
                {
                    getdataToedit(drr[0]["Doctype"].ToString(), drr[0]["CallerID"].ToString(), drr[0]["Fiscalyear"].ToString(), drr[0]["CustomerCode"].ToString());
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

        protected void getdataToedit(string doctype, string docnumber, string fiscalyear,string customer)
        {
            ServiceCallFastEntryCriteria link = new ServiceCallFastEntryCriteria();
            string idGen = link.redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                ServiceTicketLibrary lib_TK = new ServiceTicketLibrary();
                string PageRedirect = lib_TK.getPageTicketRedirect(
                    ERPWAuthentication.SID,
                    (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                );
                if (PageRedirect.Equals("ServiceCallTransaction.aspx"))
                {
                    ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                }
                else if (PageRedirect.Equals("ServiceCallTransactionChange.aspx"))
                {
                    ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                }
                else
                {
                    Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                }
                
                //Response.-Redirect("/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen, false);
            }

            //Object[] objParam = new Object[] { "1500117",
            //        (string)Session[ApplicationSession.USER_SESSION_ID],
            //        ERPWAuthentication.CompanyCode,doctype,docnumber,fiscalyear};

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
    }
}