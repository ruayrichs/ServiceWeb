using Agape.FocusOne.Utilities;
using ServiceWeb.Accountability.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service.Workflow;
using Agape.Lib.Web.Bean.CS;
using ServiceWeb.Service;

namespace ServiceWeb.Accountability.UserControl
{
    public partial class ApprovalProcedureControl : System.Web.UI.UserControl
    {
        #region Private variable
        private WorkflowService serWorkflow = WorkflowService.getInstance();
        //private InitiativeManagementCenter initService = InitiativeManagementCenter.getInstance();
        private AccountabilityService accountService = new AccountabilityService();
        #endregion

        #region Session object
        public string WorkGroupCode
        {
            get
            {
                return hddWorkGroupCode.Value;
            }
        }
        public string InitiativeCode
        {
            get
            {
                return hddInitiativeCode.Value;
            }
        }

        private DataTable DTHeader
        {
            get
            {
                if (Session["ApprovalProcedure.DTHeader" + WorkGroupCode + InitiativeCode] == null)
                {
                    Session["ApprovalProcedure.DTHeader" + WorkGroupCode + InitiativeCode] = structureApprovalHeader();
                }
                return (DataTable)Session["ApprovalProcedure.DTHeader" + WorkGroupCode + InitiativeCode];
            }
            set { Session["ApprovalProcedure.DTHeader" + WorkGroupCode + InitiativeCode] = value; }
        }

        private DataTable DTParticipants
        {
            get
            {
                if (Session["ApprovalProcedure.DTParticipants" + WorkGroupCode + InitiativeCode] == null)
                {
                    Session["ApprovalProcedure.DTParticipants" + WorkGroupCode + InitiativeCode] = structureApprovalParticipants();
                }
                return (DataTable)Session["ApprovalProcedure.DTParticipants" + WorkGroupCode + InitiativeCode];
            }
            set
            {
                Session["ApprovalProcedure.DTParticipants" + WorkGroupCode + InitiativeCode] = value;
            }
        }
        #endregion

        #region String constant
        private const string L0 = "L0";
        private const string L1 = "L1";
        private const string L2 = "L2";
        private const string L3 = "L3";
        private const string L4 = "L4";
        private const string L5 = "L5";
        private const string GET_STRUCTURE = "GET_STRUCTURE";
        #endregion

        private string CurrentStageGate
        {
            get
            {
                string[] StateGate = serWorkflow.GetApprovalHeaderPresent(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode
                ).Split(',');
                return StateGate[0];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindData(string InitiativeCode, string WorkGroupCode)
        {
            try
            {
                DataTable dtWF = serWorkflow.getWorkflow("", WorkGroupCode, InitiativeCode);

                if (dtWF.Rows.Count > 0)
                {
                    if (!chkLoadData.Checked)
                    {
                        DataTable DTRequestApproveObject = serWorkflow.GetEventObjectStatus("", InitiativeCode);
                        DataTable DTApproveEventObjectStatus = serWorkflow.GetApproveEventObjectStatus("", InitiativeCode);

                        if (DTRequestApproveObject.Rows.Count > 3)
                        {
                            bool isRequestApproveCancel = Convert.ToBoolean(DTRequestApproveObject.Select("Object = 'Cancel'")[0]["IsRequest"]);
                            bool isRequestApproveRelease = Convert.ToBoolean(DTRequestApproveObject.Select("Object = 'Release'")[0]["IsRequest"]);
                            bool isRequestApproveRevise = Convert.ToBoolean(DTRequestApproveObject.Select("Object = 'Revise'")[0]["IsRequest"]);
                        }

                        if (DTRequestApproveObject.Rows.Count > 2)
                        {
                            bool isApprovedRelease = Convert.ToBoolean(DTApproveEventObjectStatus.Select("Object = 'Release'")[0]["IsApprovel"]);
                            bool isApproveCancel = Convert.ToBoolean(DTApproveEventObjectStatus.Select("Object = 'Cancel'")[0]["IsApprovel"]);
                        }

                        clareAllValueText();

                        hddInitiativeCode.Value = InitiativeCode;
                        hddWorkGroupCode.Value = WorkGroupCode;
                        udpCodeidentityInitiative.Update();


                        getDropdownParticipant();
                        udpApprovalProcedureModelControl.Update();

                        ClientService.DoJavascript("checkPanelLoadModel('panelApprovalProcedureModelControl');");
                    }
                    LoadApprovalProdure();
                    ClientService.DoJavascript("setBackgroundSuccessEventObjectApproval();");
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }
        protected void getDropdownParticipant()
        {
            try
            {
                DataTable dt = CharacterService.getInstance().getCharacter(ERPWAuthentication.SID, WorkGroupCode);
                ddlParticipantsDesc.DataSource = dt;
                ddlParticipantsDesc.DataValueField = "HierarchyType";
                ddlParticipantsDesc.DataTextField = "HierarchyTypeName";
                ddlParticipantsDesc.DataBind();

            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        protected void btnChangeSubProject_Click(object sender, EventArgs e)
        {
            DataTable dt = accountService.getCharacterForControl(ERPWAuthentication.SID, txtChangeFolderSubProject.Text);
            if (dt.Rows.Count > 0)
            {
                string code = dt.Rows[0]["ObjectID"].ToString();
                string[] codeaa = code.Split('|');
                string NameCharacter = "";
                bool first = true;
                for (int i = 0; codeaa.Length > i; i++)
                {
                    if (first)
                    {
                        NameCharacter = accountService.getCharacterNameForControl(ERPWAuthentication.SID,
                            codeaa[i]);
                        first = false;
                    }
                    else
                    {
                        NameCharacter += "/" + accountService.getCharacterNameForControl(ERPWAuthentication.SID,
                            codeaa[i]);
                    }
                }
                ClientService.DoJavascript("$('#txtSubProjectDescription').val('" + NameCharacter + "');");
            }
        }

        protected void btnAddActor_Click(object sender, EventArgs e)
        {
            try
            {
                string ID = hddID.Value;
                string lastHierarchyCode = LastHierarchyCode(txtChangeFolderSubProject.Text);
                Repeater rpt = udpStateGate.FindControl("rpt" + ID) as Repeater;
                DataTable dt = accountService.getNameByHierarchy(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, lastHierarchyCode);
                if (dt.Rows.Count > 0)
                {
                    rpt.DataSource = dt;
                    rpt.DataBind();
                    udpStateGate.Update();
                    ClientService.DoJavascript("closePopUp();");
                }
                else
                {
                    ClientService.AGError("ยังไม่มีผู้รับบทบาท");
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

        private string LastHierarchyCode(string StructureCode)
        {
            string[] hierarchy = StructureCode.Split('|');
            int lastHierarchy = hierarchy.Length - 1;
            string HierarchyCode = hierarchy[lastHierarchy].ToString();

            return HierarchyCode;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveApprovalProdure();
                LoadApprovalProdure();
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

        #region Load approval produre
        public class ApprovalProcedureStateGateRole
        {
            public string RoleCode { get; set; }
            public string RoleName { get; set; }
            public string StatusAP { get; set; }
        }

        private DataTable dtApprovalProcedureLog;

        private DataTable dtApprovalProcedureStateGate;
        private DataTable dtApprovalProcedureStateGateRole;
        private void LoadApprovalProdure()
        {
            try
            {
                dtApprovalProcedureStateGate = serWorkflow.getInitiativeWorkStructure(WorkGroupCode, InitiativeCode, "", "");
                dtApprovalProcedureLog = serWorkflow.getInitiativeApproveLog(InitiativeCode);

                DataTable dtStategate = serWorkflow.getInitiativeApproveHeader(ERPWAuthentication.SID, InitiativeCode);
                DataTable dtCompaare = dtStategate.Clone();
                foreach (DataRow row in dtStategate.Rows)
                {
                    foreach (DataRow row1 in dtApprovalProcedureStateGate.Rows)
                    {
                        if ((row["STATEGATEFROM"]).ToString() == (row1["STATEGATEFROM"]).ToString() && (row["STATEGATETO"]).ToString() == (row1["STATEGATETO"]).ToString())
                        {
                            //case Level person approve > 1
                            //dtCompaare.ImportRow(row);
                            dtCompaare.DefaultView.RowFilter = "STATEGATEFROM = '" + row1["STATEGATEFROM"] + "' AND STATEGATETO = '" + row1["STATEGATETO"] + "'";
                            if (dtCompaare.DefaultView.ToTable().Rows.Count == 0)
                            {
                                dtCompaare.ImportRow(row);
                            }
                        }
                    }
                }

                //for (int i = dtstategate.rows.count - 1; i >= 0; i--)
                //{
                //    datarow row = dtstategate.rows[i];
                //dtapprovalprocedurestategate.defaultview.rowfilter = "stategatefrom = '" + row["stategatefrom"] + "' and stategateto = '" + row["stategateto"] + "'";

                //    if (dtapprovalprocedurestategate.defaultview.totable().rows.count == 0)
                //    {
                //        dtstategate.rows[i].delete();
                //    }
                //}
                //dtstategate.acceptchanges();
                dtCompaare.DefaultView.RowFilter = "";
                rptApprovalProcedureStateGate.DataSource = dtCompaare;
                rptApprovalProcedureStateGate.DataBind();


                LoadApprovalProdureEventObject();

                udpStateGate.Update();

                ClientService.DoJavascript("approvalhistoryBoxHeight();");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error : " + ex);
                throw ex;
            }
        }
        protected void rptApprovalProcedureStateGate_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = e.Item.FindControl("rptApprovalProcedureStateGateInner") as Repeater;
            string sgFrom = (e.Item.FindControl("hddApprovalProcedureStateGateFrom") as HiddenField).Value;
            string sgTo = (e.Item.FindControl("hddApprovalProcedureStateGateTo") as HiddenField).Value;
            dtApprovalProcedureStateGate.DefaultView.RowFilter = "STATEGATEFROM = '" + sgFrom + "' and STATEGATETO = '" + sgTo + "'";
            //System.Diagnostics.Debug.WriteLine(dtApprovalProcedureStateGate.Rows[e.Item.ItemIndex]["STATEGATEFROM"]);
            //System.Diagnostics.Debug.WriteLine(dtApprovalProcedureStateGate.Rows[e.Item.ItemIndex]["STATEGATEFROM"] + " == " + sgFrom);
            System.Diagnostics.Debug.WriteLine("STATEGATEFROM : " + sgFrom + " STATEGATETO : " + sgTo);
            if (dtApprovalProcedureStateGate.Rows.Count > 0)
            {
                // ทำให้แสดง
            }
            else
            {
                // ทำให้ซ่อน
            }
            rpt.DataSource = dtApprovalProcedureStateGate.DefaultView; // ซ่อน / แสดง  rptApprovalProcedureStateGate Item ที่ไม่มีคน <<<<<<
            rpt.DataBind();

            Repeater rptLog = e.Item.FindControl("rptApprovalProcedureLog") as Repeater;
            dtApprovalProcedureLog.DefaultView.RowFilter = "StateGateFrom = '" + sgFrom + "' and (Event = 'APPROVAL_UPGARDE_STATEGATE' or Event = 'APPROVAL_DOWNGARDE_STATEGATE' or Event = 'REQUEST_DOC_APPROVAL')";
            rptLog.DataSource = dtApprovalProcedureLog.DefaultView;
            rptLog.DataBind();

            (e.Item.FindControl("lblApprovalProcedureLogEmpty") as Label).Visible = rptLog.Items.Count == 0;
        }
        protected void rptApprovalProcedureStateGateInner_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                string sgFrom = (e.Item.Parent.Parent.FindControl("hddApprovalProcedureStateGateFrom") as HiddenField).Value;
                string sgTo = (e.Item.Parent.Parent.FindControl("hddApprovalProcedureStateGateTo") as HiddenField).Value;

                Repeater rptApprovalProcedureStateGateInnerRole = (Repeater)e.Item.FindControl("rptApprovalProcedureStateGateInnerRole");
                HiddenField hddApprovalProcedureStateGateInnerStructure = (HiddenField)e.Item.FindControl("hddApprovalProcedureStateGateInnerStructure");

                dtApprovalProcedureStateGateRole = getApprovalProcedure(sgFrom, sgTo, hddApprovalProcedureStateGateInnerStructure.Value);
                DataTable dtStategate = serWorkflow.getInitiativeApproveHeader(ERPWAuthentication.SID, InitiativeCode);
                string ModeApproval = "";
                if (dtStategate.Rows.Count > 0)
                {
                    if (dtStategate.Select("STATEGATEFROM = '" + sgFrom + "' and (APPROVESTARTED = 'TRUE' or (APPROVESTATUS = 'TRUE' and APPROVESTARTED = 'FALSE' and REQUESTAPPROVADOWNGRADE = 'FALSE'))").Count() > 0)
                    {
                        ModeApproval = "Upgrade";
                    }
                    else if (dtStategate.Select("STATEGATEFROM = '" + sgFrom + "' and REQUESTAPPROVADOWNGRADE = 'TRUE'").Count() > 0)
                    {
                        ModeApproval = "Downgrade";
                    }
                    else
                    {
                        ModeApproval = "Wait";
                    }
                }

                List<ApprovalProcedureStateGateRole> ListRole = new List<ApprovalProcedureStateGateRole>();

                if (ModeApproval == "Upgrade")
                {
                    foreach (DataRow dr in dtApprovalProcedureStateGateRole.Rows)
                    {
                        string RoleCode = Convert.ToString(dr["HierarchyCode"]);
                        string RoleName = Convert.ToString(dr["HierarchyDesc"]);
                        string StatusAp = Convert.ToString(dr["Status"]);

                        ApprovalProcedureStateGateRole foundRole = ListRole.Find(a => a.RoleName.Equals(RoleName));
                        if (foundRole == null)
                        {
                            ListRole.Add(new ApprovalProcedureStateGateRole
                            {
                                RoleCode = RoleCode,
                                RoleName = RoleName,
                                StatusAP = StatusAp
                            });
                        }
                        else
                        {
                            if (StatusAp.Equals("APPROVED"))
                            {
                                foundRole.StatusAP = StatusAp;
                            }

                        }
                    }
                }
                else if (ModeApproval == "Downgrade")
                {
                    foreach (DataRow dr in dtApprovalProcedureStateGateRole.Rows)
                    {
                        string RoleCode = Convert.ToString(dr["HierarchyCode"]);
                        string RoleName = Convert.ToString(dr["HierarchyDesc"]);
                        string DowngradeAp = Convert.ToString(dr["STATUSDOWNGRADE"]);

                        ApprovalProcedureStateGateRole foundRole = ListRole.Find(a => a.RoleName.Equals(RoleName));
                        if (foundRole == null)
                        {
                            ListRole.Add(new ApprovalProcedureStateGateRole
                            {
                                RoleCode = RoleCode,
                                RoleName = RoleName,
                                StatusAP = DowngradeAp
                            });
                        }
                        else
                        {
                            if (DowngradeAp.Equals("APPROVED"))
                            {
                                foundRole.StatusAP = DowngradeAp;
                            }

                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dtApprovalProcedureStateGateRole.Rows)
                    {
                        string RoleCode = Convert.ToString(dr["HierarchyCode"]);
                        string RoleName = Convert.ToString(dr["HierarchyDesc"]);
                        string DowngradeAp = Convert.ToString(dr["STATUSDOWNGRADE"]);

                        ApprovalProcedureStateGateRole foundRole = ListRole.Find(a => a.RoleName.Equals(RoleName));
                        if (foundRole == null)
                        {
                            ListRole.Add(new ApprovalProcedureStateGateRole
                            {
                                RoleCode = RoleCode,
                                RoleName = RoleName,
                                StatusAP = DowngradeAp
                            });
                        }
                    }
                }

                rptApprovalProcedureStateGateInnerRole.DataSource = ListRole;
                rptApprovalProcedureStateGateInnerRole.DataBind();               
            }
            catch { }
        }
        protected void rptApprovalProcedureStateGateInnerRole_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lbEnvelopebt = (Label)e.Item.FindControl("lbEnvelopebt") as Label;
            HiddenField hddStatusAPforhide = (HiddenField)e.Item.FindControl("hddStatusAPforhide") as HiddenField;
            HiddenField hddApproveStartedforhide = (HiddenField)e.Item.Parent.Parent.Parent.Parent.FindControl("hddApproveStartedforhide") as HiddenField;
            HiddenField hddRequestApproveDowngradeforhide = (HiddenField)e.Item.Parent.Parent.Parent.Parent.FindControl("hddRequestApproveDowngradeforhide") as HiddenField;

            if ((hddApproveStartedforhide.Value == "TRUE" || hddRequestApproveDowngradeforhide.Value == "TRUE") && hddStatusAPforhide.Value != "APPROVED")
            {
                lbEnvelopebt.CssClass = "fa fa-envelope";
            }
            else
            {
                lbEnvelopebt.CssClass = "fa fa-envelope hide";
            }

            Repeater rptApprovalProcedureStateGateInnerStructure = (Repeater)e.Item.FindControl("rptApprovalProcedureStateGateInnerStructure");
            HiddenField hddApprovalProcedureStateGateInnerRoleName = (HiddenField)e.Item.FindControl("hddApprovalProcedureStateGateInnerRoleName");

            dtApprovalProcedureStateGateRole.DefaultView.RowFilter = "HierarchyDesc = '" + hddApprovalProcedureStateGateInnerRoleName.Value + "'";
            rptApprovalProcedureStateGateInnerStructure.DataSource = dtApprovalProcedureStateGateRole.DefaultView;
            rptApprovalProcedureStateGateInnerStructure.DataBind();
        }


        private DataTable dtApprovalProcedureEventObject;
        private DataTable dtApprovalProcedureEventObjectRole;
        private void LoadApprovalProdureEventObject()
        {
            try
            {
                DataTable dtWF = serWorkflow.getWorkflow("", WorkGroupCode, InitiativeCode);

                if (dtWF.Rows.Count > 0)
                {
                    dtApprovalProcedureEventObject = serWorkflow.getInitiativeWorkStructureEventObject(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        InitiativeCode,
                        WorkGroupCode
                        );
                    dtApprovalProcedureEventObjectRole = serWorkflow.getApprovalEvenObjectParticipants(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode
                    );


                    DataTable dtEventObject = new DataTable();
                    dtEventObject.Columns.Add("EventCode");
                    dtEventObject.Columns.Add("EventDesc");
                    dtEventObject.Columns.Add("EventApproving");
                    dtEventObject.Columns.Add("EventApproved");

                    DataTable DTRequestApproveObject = serWorkflow.GetEventObjectStatus("", InitiativeCode);
                    DataTable DTApproveEventObjectStatus = serWorkflow.GetApproveEventObjectStatus("", InitiativeCode);
                    bool isRequestApproveCancel = Convert.ToBoolean(DTRequestApproveObject.Select("Object = 'Cancel'")[0]["IsRequest"]);
                    bool isRequestApproveRelease = Convert.ToBoolean(DTRequestApproveObject.Select("Object = 'Release'")[0]["IsRequest"]);
                    bool isRequestApproveRevise = Convert.ToBoolean(DTRequestApproveObject.Select("Object = 'Revise'")[0]["IsRequest"]);

                    bool isApprovedRelease = Convert.ToBoolean(DTApproveEventObjectStatus.Select("Object = 'Release'")[0]["IsApprovel"]);
                    bool isApproveCancel = Convert.ToBoolean(DTApproveEventObjectStatus.Select("Object = 'Cancel'")[0]["IsApprovel"]);


                    //dtEventObject.Rows.Add("Cancel", "Cancel Document", isRequestApproveCancel, isApproveCancel);

                    rptApprovalProcedureEventObject.DataSource = dtEventObject;
                    rptApprovalProcedureEventObject.DataBind();

                    udpStateGate.Update();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void rptApprovalProcedureEventObject_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = e.Item.FindControl("rptApprovalProcedureEventObjectInner") as Repeater;
            string EventObjectCode = (e.Item.FindControl("hddApprovalProcedureEventObject") as HiddenField).Value;

            dtApprovalProcedureEventObject.DefaultView.RowFilter = "EventApproval = '" + EventObjectCode + "'";
            rpt.DataSource = dtApprovalProcedureEventObject.DefaultView;
            rpt.DataBind();

            Repeater rptLog = e.Item.FindControl("rptApprovalProcedureEventObjectLog") as Repeater;

            string ApproveCondition = "";
            if (EventObjectCode.Equals("Revise"))
            {
                ApproveCondition = " Event = 'APPROVAL_REVISE_FINANCAIL'";
            }
            else if (EventObjectCode.Equals("Release"))
            {
                ApproveCondition = "  Event = 'APPROVAL_RELEASE_FINANCAIL'";
            }
            else if (EventObjectCode.Equals("Cancel"))
            {
                ApproveCondition = "  Event = 'APPROVAL_CANCEL_INITIATIVE' ";
            }
            dtApprovalProcedureLog.DefaultView.RowFilter = ApproveCondition;
            rptLog.DataSource = dtApprovalProcedureLog.DefaultView;
            rptLog.DataBind();

            (e.Item.FindControl("lblApprovalProcedureEventObjectLogEmpty") as Label).Visible = rptLog.Items.Count == 0;
        }
        protected void rptApprovalProcedureEventObjectInner_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptApprovalProcedureEventObjectInnerRole = (Repeater)e.Item.FindControl("rptApprovalProcedureEventObjectInnerRole");
            HiddenField hddApprovalProcedureEventObjectInnerStructure = (HiddenField)e.Item.FindControl("hddApprovalProcedureEventObjectInnerStructure");
            string EventObjectCode = (e.Item.Parent.Parent.FindControl("hddApprovalProcedureEventObject") as HiddenField).Value;

            DataRow[] drRoles = dtApprovalProcedureEventObjectRole.Select("EventApproval = '" + EventObjectCode + "' and WorkStructureHierarchyCode = '" + hddApprovalProcedureEventObjectInnerStructure.Value + "'");

            List<ApprovalProcedureStateGateRole> ListRole = new List<ApprovalProcedureStateGateRole>();
            //foreach (DataRow dr in drRoles)
            //{
            //    if (ListRole.Where(a => a.RoleName.Equals(Convert.ToString(dr["RoleName"]))).Count() == 0)
            //    {
            //        ListRole.Add(new ApprovalProcedureStateGateRole
            //        {
            //            RoleCode = dr["RoleCode"].ToString(),
            //            RoleName = dr["RoleName"].ToString(),
            //            StatusAP = dr["STATUS"].ToString()
            //        });
            //    }
            //}

            foreach (DataRow dr in drRoles)
            {
                string RoleCode = Convert.ToString(dr["RoleCode"]);
                string RoleName = Convert.ToString(dr["RoleName"]);
                string StatusAP = Convert.ToString(dr["STATUS"]);

                ApprovalProcedureStateGateRole foundRole = ListRole.Find(a => a.RoleName.Equals(RoleName));
                if (foundRole == null)
                {
                    ListRole.Add(new ApprovalProcedureStateGateRole
                    {
                        RoleCode = RoleCode,
                        RoleName = RoleName,
                        StatusAP = StatusAP
                    });
                }
                else
                {
                    if (StatusAP.Equals("APPROVE"))
                    {
                        foundRole.StatusAP = StatusAP;
                    }

                }
            }

            rptApprovalProcedureEventObjectInnerRole.DataSource = ListRole;
            rptApprovalProcedureEventObjectInnerRole.DataBind();
        }
        protected void rptApprovalProcedureEventObjectInnerRole_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lbEnvelopebtFinancial = (Label)e.Item.FindControl("lbEnvelopebtFinancial") as Label;
            HiddenField hddStatusAPFinancial = (HiddenField)e.Item.FindControl("hddStatusAPFinancial") as HiddenField;
            HiddenField hddEventApproving = (HiddenField)e.Item.Parent.Parent.Parent.Parent.FindControl("hddEventApproving") as HiddenField;

            if (Convert.ToBoolean(hddEventApproving.Value) && hddStatusAPFinancial.Value != "APPROVE")
            {
                lbEnvelopebtFinancial.CssClass = "fa fa-envelope";
            }
            else
            {
                lbEnvelopebtFinancial.CssClass = "fa fa-envelope hide";
            }

            Repeater rptApprovalProcedureEventObjectInnerStructure = (Repeater)e.Item.FindControl("rptApprovalProcedureEventObjectInnerStructure");
            HiddenField hddApprovalProcedureEventObjectInnerRoleName = (HiddenField)e.Item.FindControl("hddApprovalProcedureEventObjectInnerRoleName");
            HiddenField hddApprovalProcedureEventObjectInnerStructure = (HiddenField)e.Item.Parent.Parent.FindControl("hddApprovalProcedureEventObjectInnerStructure");
            string EventObjectCode = (e.Item.Parent.Parent.Parent.Parent.FindControl("hddApprovalProcedureEventObject") as HiddenField).Value;

            dtApprovalProcedureEventObjectRole.DefaultView.RowFilter = "EventApproval = '" + EventObjectCode + "' and WorkStructureHierarchyCode = '" + hddApprovalProcedureEventObjectInnerStructure.Value + "' and RoleName = '" + hddApprovalProcedureEventObjectInnerRoleName.Value + "'";
            rptApprovalProcedureEventObjectInnerStructure.DataSource = dtApprovalProcedureEventObjectRole.DefaultView;
            rptApprovalProcedureEventObjectInnerStructure.DataBind();
        }

        private DataTable getApprovalProcedure(string STATEGATEFROM, string STATEGATETO, string WorkStructureCode)
        {
            DataTable dt = serWorkflow.getApprovalParticipantsForApprovalProcedure(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                WorkGroupCode,
                InitiativeCode,
                STATEGATEFROM,
                STATEGATETO,
                "",
                "",
                "",
                WorkStructureCode);

            return dt;
        }
        public string TranslateApprovalLog(Object Event)
        {
            string Desc = "";

            return Desc;
        }
        public string TranslateParticipantsStatus(Object Status, Object ApproveOn)
        {
            string Desc = "";
            if (Status.ToString().Trim().ToUpper().Equals("WAITTING"))
            {
                Desc = "<span class='text-muted apprval-state-waiting'>Waiting</span>";
            }
            else if (Status.ToString().Trim().ToUpper().Equals("APPROVED") || Status.ToString().Trim().ToUpper().Equals("APPROVE"))
            {
                Desc = "<span class='text-success apprval-state-approve'>Approve on " + Validation.Convert2DateTimeDisplay(Convert.ToString(ApproveOn)) + "</span>";
            }
            else if (Status.ToString().Trim().ToUpper().Equals("REJECTED") || Status.ToString().Trim().ToUpper().Equals("REJECTED"))
            {
                Desc = "<span class='text-danger apprval-state-approve'>Reject on " + Validation.Convert2DateTimeDisplay(Convert.ToString(ApproveOn)) + "</span>";
            }
            return Desc;
        }

        #endregion

        #region Add approval produre


        private void AddApprovalProdureAll(DataTable dt, string STATEGATEFROM, string STATEGATETO, string APPROVEDETIAL, string LASTHIERARCHYCODE)
        {
            try
            {
                DataRow[] dr = DTHeader.Select("STATEGATEFROM='" + STATEGATEFROM + "' AND STATEGATETO='" + STATEGATETO + "'");
                if (dr.Length == 0)
                {
                    DataRow drAdd = DTHeader.NewRow();
                    drAdd["SID"] = ERPWAuthentication.SID;
                    drAdd["COMPANYCODE"] = ERPWAuthentication.CompanyCode;
                    drAdd["WORKGROUPCODE"] = WorkGroupCode;
                    drAdd["INITIATIVECODE"] = InitiativeCode;
                    drAdd["STATEGATEFROM"] = STATEGATEFROM;
                    drAdd["STATEGATETO"] = STATEGATETO;
                    drAdd["HIERARCHYCODE"] = LASTHIERARCHYCODE;
                    drAdd["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
                    drAdd["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
                    DTHeader.Rows.Add(drAdd);
                }
                else
                {
                    dr[0]["HIERARCHYCODE"] = LASTHIERARCHYCODE;
                }

                #region Remove for change employee
                DataRow[] drrx = DTParticipants.Select("STATEGATEFROM='" + STATEGATEFROM + "' AND STATEGATETO='" + STATEGATETO + "'");
                if (drrx.Length > 0)
                {
                    foreach (DataRow drx in drrx)
                    {
                        DTParticipants.Rows.Remove(drx);
                    }
                    DTParticipants.AcceptChanges();
                }
                #endregion

                foreach (DataRow item in dt.Rows)
                {
                    DataRow[] drr = DTParticipants.Select("STATEGATEFROM='" + STATEGATEFROM + "' AND STATEGATETO='" + STATEGATETO + "' AND EMPLOYEECODE='" + item["EmployeeCode"].ToString() + "'");
                    if (drr.Length == 0)
                    {
                        DataRow drParticipants = DTParticipants.NewRow();
                        drParticipants["SID"] = ERPWAuthentication.SID;
                        drParticipants["COMPANYCODE"] = ERPWAuthentication.CompanyCode;
                        drParticipants["WORKGROUPCODE"] = WorkGroupCode;
                        drParticipants["INITIATIVECODE"] = InitiativeCode;
                        drParticipants["STATEGATEFROM"] = STATEGATEFROM;
                        drParticipants["STATEGATETO"] = STATEGATETO;
                        drParticipants["EMPLOYEECODE"] = item["EmployeeCode"] == null ? "" : item["EmployeeCode"].ToString();
                        drParticipants["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
                        drParticipants["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
                        DTParticipants.Rows.Add(drParticipants);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Save approval produre
        private void SaveApprovalProdure()
        {
            try
            {
                serWorkflow.saveApprovalHeader(DTHeader);
                serWorkflow.saveApprovalParticipants(DTParticipants);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Create structure
        private DataTable structureApprovalHeader()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SID", typeof(string));
            dt.Columns.Add("COMPANYCODE", typeof(string));
            dt.Columns.Add("WORKGROUPCODE", typeof(string));
            dt.Columns.Add("INITIATIVECODE", typeof(string));
            dt.Columns.Add("STATEGATEFROM", typeof(string));
            dt.Columns.Add("STATEGATETO", typeof(string));
            dt.Columns.Add("HIERARCHYCODE", typeof(string));
            dt.Columns.Add("APPROVESTATUS", typeof(string));
            dt.Columns.Add("APPROVESTARTED", typeof(string));
            dt.Columns.Add("APPROVEDETIAL", typeof(string));
            dt.Columns.Add("CREATED_BY", typeof(string));
            dt.Columns.Add("CREATED_ON", typeof(string));
            dt.Columns.Add("UPDATED_BY", typeof(string));
            dt.Columns.Add("UPDATED_ON", typeof(string));

            return dt;
        }

        private DataTable structureApprovalParticipants()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SID", typeof(string));
            dt.Columns.Add("COMPANYCODE", typeof(string));
            dt.Columns.Add("WORKGROUPCODE", typeof(string));
            dt.Columns.Add("INITIATIVECODE", typeof(string));
            dt.Columns.Add("STATEGATEFROM", typeof(string));
            dt.Columns.Add("STATEGATETO", typeof(string));
            dt.Columns.Add("EMPLOYEECODE", typeof(string));
            dt.Columns.Add("CONDITION", typeof(string));
            dt.Columns.Add("STATUS", typeof(string));
            dt.Columns.Add("REMARKS", typeof(string));
            dt.Columns.Add("CREATED_BY", typeof(string));
            dt.Columns.Add("CREATED_ON", typeof(string));
            dt.Columns.Add("UPDATED_BY", typeof(string));
            dt.Columns.Add("UPDATED_ON", typeof(string));

            return dt;
        }
        #endregion

        public void KeepDataOverViewChanger()
        {
            try
            {
                if (chkKeepDataSession.Checked)
                {
                    string currentDateTime = Validation.getCurrentServerStringDateTime();

                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        public void SaveDataSessionKeep()
        {
            try
            {
                if (chkSaveDataSession.Checked)
                {
                    SaveApprovalProdure();
                    LoadApprovalProdure();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void clareAllValueText()
        {
            txtChangeFolderSubProject.Text = "";

            txtSubProjectDescription.Value = "";
            hddID.Value = "";
            hddInitiativeCode.Value = "";
            hddName.Value = "";
            hddWorkGroupCode.Value = "";
        }


        private string idGen
        {
            get
            {
                return Request["id"];
            }
        }
        private tmpServiceCallDataSet serviceCallEntity
        {
            get { return Session["ServicecallEntity" + idGen] == null ? new tmpServiceCallDataSet() : (tmpServiceCallDataSet)Session["ServicecallEntity" + idGen]; }
            set { Session["ServicecallEntity" + idGen] = value; }
        }

        public void defaultMailTo(List<string> mailTo)
        {
            mailTo = mailTo.Distinct().ToList();

            if (mailTo.Count > 0)
                ClientService.DoJavascript("setDefaultMailTo(['" + string.Join("', '", mailTo) + "']);");
        }

        protected void btnSendGroupNotApproved_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);
                Repeater rptApprovalProcedureStateGateInnerStructure = btn.Parent.FindControl("rptApprovalProcedureStateGateInner") as Repeater;
                List<string> listCTCode = new List<string>();
                List<string> listHDFrom = new List<string>();
                List<string> listHDTo = new List<string>();
                foreach (RepeaterItem item in rptApprovalProcedureStateGateInnerStructure.Items)
                {
                    HiddenField hddApprovalProcedure_EmployeeCode = item.FindControl("hddApprovalProcedureStateGateInnerStructure") as HiddenField;
                    HiddenField hddApprovalProcedure_hddSTATEGATEFROM = item.FindControl("hddSTATEGATEFROM") as HiddenField;
                    HiddenField hddApprovalProcedure_hddSTATEGATETO = item.FindControl("hddSTATEGATETO") as HiddenField;
                    listCTCode.Add(hddApprovalProcedure_EmployeeCode.Value);
                    listHDFrom.Add(hddApprovalProcedure_hddSTATEGATEFROM.Value);
                    listHDTo.Add(hddApprovalProcedure_hddSTATEGATETO.Value);

                }
                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    _docnumber = dr["CallerID"].ToString();
                }
                string OnTopCTCode = listCTCode.First();
                string OnToplistHDFrom = listHDFrom.First();
                string OnToplistHDTo = listHDTo.First();
                List<string> listEmpCode = new List<string>();
                listEmpCode = serWorkflow.getListEmployee(ERPWAuthentication.CompanyCode,
              OnTopCTCode, OnToplistHDFrom, OnToplistHDTo
              );
                List<string> listEmpMailTo = new List<string>();
                listEmpMailTo = serWorkflow.getListEmailEmployee(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
              listEmpCode
              );
                ClientService.DoJavascript("sendCustomEmail('Please approve Change Order : " + _docnumber + "', '" + ERPWAuthentication.FullNameEN + "')");
                defaultMailTo(listEmpMailTo);

                //ActivitySendMailModal.defaultMailTo(listEmpMailTo);

                #region
                //string[] CommandArgument = (sender as Button).CommandArgument.ToString().Split(',');
                //string InitiativeCode = CommandArgument[0];
                //string StateGateFrom = CommandArgument[1];
                //string StateGateTo = CommandArgument[2];
                //DataTable dt = serWorkflow.getGroupNotApproved(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, StateGateFrom, StateGateTo);

                //foreach (DataRow dr in dt.Rows)
                //{

                //    //InitiativeEmailService.SendMailEntity MailEn = new InitiativeEmailService.SendMailEntity();
                //    //MailEn.Email = dr["Email"].ToString().ToString();
                //    //MailEn.FullName = dr["FullName"].ToString().ToString();

                //    //InitiativeEmailService.getInstance().SendInitiativeEMail(
                //    //    ERPWAuthentication.SID,
                //    //    "",
                //    //    ERPWAuthentication.CompanyCode,
                //    //    WorkGroupCode,
                //    //    InitiativeCode,
                //    //    "ALERT_EMAIL_APPROVAL",
                //    //    ERPWAuthentication.EmployeeCode,
                //    //    MailEn
                //    //);
                //}

                //string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
                //WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                //    ERPWAuthentication.SID,
                //    ERPWAuthentication.CompanyCode,
                //    "",
                //    "",
                //    ERPWAuthentication.EmployeeCode,
                //    ERPWAuthentication.FullNameTH,
                //    InitiativeCode,
                //    StateGateFrom,
                //    "ALERT_EMAIL_APPROVAL",
                //    "ALERT",
                //    "Please approve",
                //    curDateTime
                //);

                //ClientService.AGSuccess("Alert send email success");
                #endregion
            }
            catch (Exception ex)
            {
                ClientService.Alert(ex.Message);
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }
        
        protected String _docnumber { get; set; }
        protected void btnSendNotApproved_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);
                Repeater rptApprovalProcedureStateGateInnerStructure = btn.Parent.FindControl("rptApprovalProcedureStateGateInnerStructure") as Repeater;

                List<string> listEmpCode = new List<string>();
                foreach (RepeaterItem item in rptApprovalProcedureStateGateInnerStructure.Items)
                {
                    HiddenField hddApprovalProcedure_EmployeeCode = item.FindControl("hddApprovalProcedure_EmployeeCode") as HiddenField;
                    listEmpCode.Add(hddApprovalProcedure_EmployeeCode.Value);
                }
                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    _docnumber = dr["CallerID"].ToString();
                }
                string OnTopEmployeeCode = listEmpCode.First();
                List<string> listEmpMailTo = new List<string>();
                listEmpMailTo = serWorkflow.getListEmailEmployee(
              ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
              listEmpCode
          );
                ClientService.DoJavascript("sendCustomEmail('Please approve Change Order : " + _docnumber + "', '" + ERPWAuthentication.FullNameEN + "')");
                defaultMailTo(listEmpMailTo);
                //ActivitySendMailModal.defaultMailTo(listEmpMailTo);

                // To Do Function Get Email Form "listEmpCode"

            }
            catch (Exception ex)
            {
                ClientService.Alert(ex.Message);
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void btnSendAllGroupNotApprovedFinancial_Click(object sender, EventArgs e)
        {
            try
            {
                string EventCode = (sender as Button).CommandArgument.ToString();
                DataTable dt = serWorkflow.getGroupNotApprovedFinancial(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, EventCode);

                foreach (DataRow dr in dt.Rows)
                {
                    //InitiativeEmailService.SendMailEntity MailEn = new InitiativeEmailService.SendMailEntity();
                    //MailEn.Email = dr["Email"].ToString().ToString();
                    //MailEn.FullName = dr["FullName"].ToString().ToString();

                    //InitiativeEmailService.getInstance().SendInitiativeEMail(
                    //    ERPWAuthentication.SID,
                    //    "",
                    //    ERPWAuthentication.CompanyCode,
                    //    WorkGroupCode,
                    //    InitiativeCode,
                    //    "ALERT_EMAIL_APPROVAL_FINANCIAL",
                    //    ERPWAuthentication.EmployeeCode,
                    //    MailEn
                    //);
                }

                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    CurrentStageGate,
                    "ALERT_EMAIL_APPROVAL_FINANCIAL",
                    "ALERT",
                    "Please approve",
                    curDateTime
                );

                ClientService.AGSuccess("Alert send email success");
                ClientService.DoJavascript("setBackgroundSuccessEventObjectApproval();");
                ClientService.DoJavascript("$('#objectEvent').click();");
            }
            catch (Exception ex)
            {
                ClientService.Alert(ex.Message);
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void btnSendNotApprovedFinancial_Click(object sender, EventArgs e)
        {
            try
            {
                HiddenField hddApprovalProcedureEventObject = (sender as Button).Parent.Parent.Parent.Parent.Parent.FindControl("hddApprovalProcedureEventObject") as HiddenField;

                string HierarchyCode = (sender as Button).CommandArgument.ToString();
                DataTable dt = serWorkflow.getNotApprovedFinancial(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, hddApprovalProcedureEventObject.Value, HierarchyCode);

                foreach (DataRow dr in dt.Rows)
                {
                    //InitiativeEmailService.SendMailEntity MailEn = new InitiativeEmailService.SendMailEntity();
                    //MailEn.Email = dr["Email"].ToString().ToString();
                    //MailEn.FullName = dr["FullName"].ToString().ToString();

                    //InitiativeEmailService.getInstance().SendInitiativeEMail(
                    //    ERPWAuthentication.SID,
                    //    "",
                    //    ERPWAuthentication.CompanyCode,
                    //    WorkGroupCode,
                    //    InitiativeCode,
                    //    "ALERT_EMAIL_APPROVAL_FINANCIAL",
                    //    ERPWAuthentication.EmployeeCode,
                    //    MailEn
                    //);
                }

                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    CurrentStageGate,
                    "ALERT_EMAIL_APPROVAL_FINANCIAL",
                    "ALERT",
                    "Please approve",
                    curDateTime
                );

                ClientService.AGSuccess("Alert send email success");
                ClientService.DoJavascript("setBackgroundSuccessEventObjectApproval();");
                ClientService.DoJavascript("$('#objectEvent').click();");
            }
            catch (Exception ex)
            {
                ClientService.Alert(ex.Message);
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        public string controlColorMassage(string Event, string ApproveType)
        {
            string cssColor = "";

            if (ApproveType.Equals("CANCEL"))
            {
                cssColor = "text-init-cancel";
            }
            else
            {
                if (Event.Equals("APPROVAL_UPGARDE_STATEGATE"))
                {
                    cssColor = "text-success";
                }
                else
                {
                    cssColor = "text-danger";
                }
            }
            return cssColor;
        }
        public string controlMassage(string Event, string ApproveType)
        {
            string msg = "";

            if (ApproveType.Equals("CANCEL"))
            {
                if (Event.Equals("APPROVAL_UPGARDE_STATEGATE"))
                {
                    msg = "Cancel requst upgrade";
                }
                else
                {
                    msg = "Cancel requst downgrade";
                }
            }
            else if (ApproveType.Equals("APPROVE"))
            {
                if (Event.Equals("APPROVAL_UPGARDE_STATEGATE"))
                {
                    msg = "Approve upgrade workflow";
                }
                else
                {
                    msg = "Approve downgrade workflow";
                }
            }
            else if (ApproveType.Equals("REJECT"))
            {
                if (Event.Equals("APPROVAL_UPGARDE_STATEGATE"))
                {
                    msg = "Reject upgrade workflow";
                }
                else
                {
                    msg = "Reject downgrade workflow";
                }
            }
            else if (ApproveType.Equals("REQUEST"))
            {
                if (Event.Equals("APPROVAL_UPGARDE_STATEGATE"))
                {
                    msg = "Request upgrade workflow";
                }
                else
                {
                    msg = "Request downgrade workflow";
                }
            }
            else if (ApproveType.Equals("REQUEST_DOC"))
            {
                msg = "Request more documents";
            }
            else
            {
                if (Event.Equals("APPROVAL_UPGARDE_STATEGATE"))
                {
                    msg = "Upgrade workflow";
                }
                else
                {
                    msg = "Downgrade workflow";
                }
            }
            return msg;
        }

        public string controlApproveTypeMassage(string ApproveType, string Message)
        {
            string msg = "";

            if (ApproveType.Equals("CANCEL"))
            {
                msg = "<span class='text-init-cancel'> [Cancel Requst] </span>" + Message;
            }
            else if (ApproveType.Equals("REQUEST") || ApproveType.Equals("REQUEST_DOC"))
            {
                msg = "<span class='text-primary'> [Requst] </span>" + Message;
            }
            else if (ApproveType.Equals("APPROVE"))
            {
                msg = "<span class='text-success'> [Approve] </span>" + Message;
            }
            else if (ApproveType.Equals("REJECT"))
            {
                msg = "<span class='text-danger'> [Reject] </span>" + Message;
            }
            else
            {
                msg = Message;
            }
            return msg;
        }

    }
}