using ServiceWeb.Accountability.MasterPage;
using ServiceWeb.Accountability.Service;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
//using SNA.Lib.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service.Workflow;
using ERPW.Lib.Service.Workflow.Entity;
using ServiceWeb.MasterConfig.MasterPage;
using ERPW.Lib.Master.Constant;
using ERPW.Lib.Master.Config;

namespace ServiceWeb.Accountability.Master
{
    public partial class AccountabilityManagement : AbstractsSANWebpage
    {
        AccountabilityService accountService = new AccountabilityService();

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        public string WorkGroupCode
        {
            get
            {
                return (Master as AccountabilityMaster).WorkGroupCode;
            }
        }
        #region TicketStatus Config
        private MasterConfigLibrary lib = new MasterConfigLibrary();
        private string[] arrUnusedEventType = new string[] { 
            ConfigurationConstant.TICKET_STATUS_EVENT_START,
            ConfigurationConstant.TICKET_STATUS_EVENT_CANCEL,
            ConfigurationConstant.TICKET_STATUS_EVENT_RESOLVE,
            ConfigurationConstant.TICKET_STATUS_EVENT_CLOSED,
            ConfigurationConstant.TICKET_STATUS_EVENT_START_BUSINESS_CHANGE,
            ConfigurationConstant.TICKET_STATUS_EVENT_INPROGRESS_BUSINESS_CHANGE,
            ConfigurationConstant.TICKET_STATUS_EVENT_RESOLVE_BUSINESS_CHANGE,
            ConfigurationConstant.TICKET_STATUS_EVENT_IMPLEMENT_BUSINESS_CHANGE,
            ConfigurationConstant.TICKET_STATUS_EVENT_CLOSED_BUSINESS_CHANGE,
            ConfigurationConstant.TICKET_STATUS_EVENT_CANCEL_BUSINESS_CHANGE,
            ConfigurationConstant.TICKET_STATUS_EVENT_ROLL_BACK_BUSINESS_CHANGE
        };
        private Dictionary<string, string> _mDicTicketStatus;
        private Dictionary<string, string> mDicTicketStatus
        {
            get
            {
                if (_mDicTicketStatus == null)
                {
                    _mDicTicketStatus = new Dictionary<string, string>();
                    DataTable dt = lib.GetMasterConfigTicketStatus(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "");
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!arrUnusedEventType.Contains(dr["EventType"].ToString()))
                        {
                            _mDicTicketStatus.Add(dr["TicketStatusCode"].ToString(), dr["TicketStatusDesc"].ToString());
                        }
                    }
                }
                return _mDicTicketStatus;
            }
        }
        private void getDropdownTicketStatus()
        {
            ddlTicketStatus.DataTextField = "Value";
            ddlTicketStatus.DataValueField = "Key";
            ddlTicketStatus.DataSource = mDicTicketStatus;
            ddlTicketStatus.DataBind();
            ddlTicketStatus.Items.Insert(0, new ListItem("-เลือกข้อมูล-", ""));
        }

        public string getDescTicketStatus(string ticketStatusCode)
        {

            if (!String.IsNullOrEmpty(ticketStatusCode))
            {
                return !String.IsNullOrEmpty(mDicTicketStatus[ticketStatusCode]) ? mDicTicketStatus[ticketStatusCode] : "";
            } else
            {
                return "";
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindDataDropdownEventObject();
            }
        }

        private void bindDataDropdownEventObject()
        {
            List<WorkflowEventObject> enEvent = WorkflowService.getInstance().geteventObjectWorkflow(
                new List<string> { "EVENT" , "LEVEL" }
            );

            ddlEventObjec.DataValueField = "objectCode";
            ddlEventObjec.DataTextField = "EventDesc";
            ddlEventObjec.DataSource = enEvent;
            ddlEventObjec.DataBind();

            ddlEventObjec.Items.Insert(0, new ListItem("-เลือกข้อมูล-", ""));
        }

        protected void btnBindData_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hddStructureCode.Value))
                {
                    //bindDataInitiativeOwner();
                    //bindDataParticipant();
                    bindDataEventObject();
                    getDropdownParticipant();
                    getDropdownTicketStatus();

                    lblTitle.Text = accountService.getStructure(
                        ERPWAuthentication.SID,
                        hddStructureCode.Value,
                        WorkGroupCode);
                    udpTitle.Update();
                    ClientService.DoJavascript("document.getElementById('panelFirstLoad').style.display = 'none';");
                    ClientService.DoJavascript("document.getElementById('panelContent').style.display = 'block';");
                }
                else
                {

                    lblTitle.Text = "";
                    udpTitle.Update();

                    ClientService.AGMessage("กรุณาเลือก Structure ภายใต้ Root");
                    ClientService.DoJavascript("document.getElementById('panelFirstLoad').style.display = 'block';");
                    ClientService.DoJavascript("document.getElementById('panelContent').style.display = 'none';");
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

        #region Initiative Owner
        //private void bindDataInitiativeOwner()
        //{
        //    DataTable dt = accountService.getDataInitiativeOwner(ERPWAuthentication.SID,
        //        ERPWAuthentication.CompanyCode,
        //        hddStructureCode.Value,
        //        WorkGroupCode
        //    );

        //    rptInitiativeOwner.DataSource = dt;
        //    rptInitiativeOwner.DataBind();

        //    udpTableOwner.Update();
        //}
        //protected void btnRemove_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string[] code = (sender as Button).CommandArgument.Split(',');

        //        accountService.DeleteInitiativeOwner(ERPWAuthentication.SID,
        //            ERPWAuthentication.CompanyCode,
        //            code[0],
        //            code[1],
        //            WorkGroupCode);
        //        ClientService.AGSuccess("ลบสำเร็จ");
        //        bindDataInitiativeOwner();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientService.AGError(ObjectUtil.Err(ex.Message));
        //    }
        //    finally
        //    {
        //        ClientService.AGLoading(false);
        //    }
        //}
        //protected void btnSaveEmpOwner_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool Islive = accountService.chackEmp(
        //            "LINK_ACCOUNTABILITY_INITIATIVE_OWNER_MASTER",
        //            "CompanyStructureCode",
        //            hddStructureCode.Value,
        //            ERPWAuthentication.SID,
        //            ERPWAuthentication.CompanyCode,
        //            ActivityService.getInstance().getEmployeeCodeByLinkID(SmartSearchMainDelegate.SelectedCode),
        //            WorkGroupCode);
        //        if (Islive)
        //        {
        //            accountService.InsertInitiativeOwner(ERPWAuthentication.SID,
        //                ERPWAuthentication.CompanyCode,
        //                hddStructureCode.Value,
        //                ActivityService.getInstance().getEmployeeCodeByLinkID(SmartSearchMainDelegate.SelectedCode),
        //                ERPWAuthentication.EmployeeCode,
        //                WorkGroupCode);

        //            bindDataInitiativeOwner();
        //            ClientService.AGSuccess("บันทึกสำเร็จ");
        //        }
        //        else
        //        {
        //            ClientService.AGMessage(SmartSearchMainDelegate.SelectedValue + "เข้าร่วมแล้ว");
        //        }
        //        SmartSearchMainDelegate.SelectedCode = "";
        //        SmartSearchMainDelegate.rebindSmartSearch();
        //        udpSmartSearch.Update();

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientService.AGError(ObjectUtil.Err(ex.Message));
        //    }
        //    finally
        //    {
        //        ClientService.DoJavascript("$('#modalAddEmp').modal('hide');");
        //        ClientService.AGLoading(false);
        //    }
        //}
        #endregion

        #region Participant
        protected void getDropdownParticipant()
        {
            try
            {
                DataTable dt = CharacterService.getInstance().getCharacter(ERPWAuthentication.SID, WorkGroupCode);
                //ddlParticipantsDesc.DataSource = dt;
                //ddlParticipantsDesc.DataValueField = "HierarchyType";
                //ddlParticipantsDesc.DataTextField = "HierarchyTypeName";
                //ddlParticipantsDesc.DataBind();
                dt.DefaultView.Sort = "HierarchyTypeName asc";
                ddlParticipantsDescription.DataSource = dt;
                ddlParticipantsDescription.DataValueField = "HierarchyType";
                ddlParticipantsDescription.DataTextField = "HierarchyTypeName";
                ddlParticipantsDescription.DataBind();
                updEventObject.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        //protected void btnRemoveParticipants_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string[] code = (sender as Button).CommandArgument.Split(',');

        //        accountService.DeleteParticipants(
        //            ERPWAuthentication.SID,
        //            ERPWAuthentication.CompanyCode,
        //            code[0],
        //            code[1],
        //            WorkGroupCode);

        //        ClientService.AGSuccess("ลบสำเร็จ");
        //        bindDataParticipant();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientService.AGError(ObjectUtil.Err(ex.Message));
        //    }
        //    finally
        //    {
        //        ClientService.AGLoading(false);
        //    }
        //}
        #endregion

        #region Event Object

        protected void btnEventObject_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlParticipantsDescription.SelectedValue) && !string.IsNullOrEmpty(ddlEventObjec.SelectedValue))
                {
                    string ParticipantsCode = accountService.GetGenerateNextIDExecute(
                        ERPWAuthentication.SID,
                        "ERPW_ACCOUNTABILITY_EVENT_OBJECT_MASTER",
                        "ParticipantsCode",
                        "P",
                        9,
                        WorkGroupCode);

                    accountService.InsertEventObject(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        hddStructureCode.Value,
                        ddlEventObjec.SelectedValue,
                        ParticipantsCode,
                        "",
                        LastHierarchyCode(txtChangeFolderSubProjectObject.Text),
                        CharacterService.getInstance().getCharacterCodeByhierarchyType(ERPWAuthentication.SID, WorkGroupCode, ddlParticipantsDescription.SelectedValue),
                        ERPWAuthentication.EmployeeCode,
                        WorkGroupCode,
                        ddlTicketStatus.SelectedValue
                        );

                    ClientService.AGSuccess("บันทึกสำเร็จ");
                    bindDataEventObject();
                }
                else
                {
                    ClientService.AGMessage("กรุณากรอกข้อมูลให้ครบถ้วน");
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

        private void bindDataEventObject()
        {
            bindConfigEventObjectDetail();
        }
        #endregion

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

        protected void btnChangeSubProjectObject_Click(object sender, EventArgs e)
        {
            DataTable dt = accountService.getCharacterForControl(ERPWAuthentication.SID, txtChangeFolderSubProjectObject.Text);
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
                ClientService.DoJavascript("$('#txtSubProjectDescriptionObject').val('" + NameCharacter + "');");
            }
        }

        private DataSet dsDataEventObject;
        private void bindConfigEventObjectDetail()
        {
            Boolean IsAutoWorkflow = accountService.getConfigStructureIsAutoWorkflow(
                ERPWAuthentication.SID, 
                WorkGroupCode,
                hddStructureCode.Value
            );

            chk_IsAutoWorkflow.Checked = IsAutoWorkflow;

            List<WorkflowEventObject> enEventObject = WorkflowService.getInstance().geteventObjectWorkflow(
                new List<string>()
            );

            dsDataEventObject = accountService.getDataEventObject_V2(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                hddStructureCode.Value,
                WorkGroupCode,
                enEventObject
            );


            rptEventLevel.DataSource = enEventObject;
            rptEventLevel.DataBind();
            updEventObject.Update();
            udpCheckAutoWorkflow.Update();
        }

        protected void rptEventLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Panel panelEventObject = e.Item.FindControl("panelEventObject") as Panel;
            Repeater rptEventObject = e.Item.FindControl("rptEventObject") as Repeater;

            string EventCode = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "EventCode"));
            string objectCode = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "objectCode"));

            if (dsDataEventObject.Tables["DTEventObject" + EventCode + objectCode].Rows.Count > 0)
            {
                panelEventObject.Style["display"] = "";
            }
            else
            {
                panelEventObject.Style["display"] = "none";
            }

            rptEventObject.DataSource = dsDataEventObject.Tables["DTEventObject" + EventCode + objectCode];
            rptEventObject.DataBind();
        }

        protected void rptEventObject_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptEventObjectDetail = (Repeater)e.Item.FindControl("rptEventObjectDetail");
            HiddenField hddParticipantsCodes = (HiddenField)e.Item.FindControl("hddParticipantsCodes");
            HiddenField hddHierarchyCode = (HiddenField)e.Item.FindControl("hddHierarchyCode");
            HiddenField hddCharacterCode = (HiddenField)e.Item.FindControl("hddCharacterCode");

            DataTable dt = accountService.getDataParticipantDetailV2(ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                hddHierarchyCode.Value,
                hddCharacterCode.Value
            );

            rptEventObjectDetail.DataSource = dt;
            rptEventObjectDetail.DataBind();
        }

        protected void btnRemoveEventObject_Click(object sender, EventArgs e)
        {
            try
            {
                string[] code = (sender as Button).CommandArgument.Split(',');

                accountService.DeleteEventObject(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    code[0],
                    code[1],
                    WorkGroupCode
                );

                bindDataEventObject();
                ClientService.AGSuccess("ลบสำเร็จ");
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

        protected void btnUpdateIsAutoWorkflow_Click(object sender, EventArgs e)
        {
            try
            {
                accountService.updateConfigStructureIsAutoWorkflow(
                    ERPWAuthentication.SID,
                    WorkGroupCode,
                    hddStructureCode.Value,
                    chk_IsAutoWorkflow.Checked
                );
                udpCheckAutoWorkflow.Update();

                ClientService.AGSuccess("Update Success");
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
    }
}