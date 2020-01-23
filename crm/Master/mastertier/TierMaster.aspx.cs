using Agape.FocusOne.Utilities;
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.Accountability.Service;
using ERPW.Lib.Service.Workflow;
using System.Web.Configuration;

namespace ServiceWeb.crm.Master.mastertier
{
    public partial class TierMaster : AbstractsSANWebpage //System.Web.UI.Page
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.SLAView;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.SLAModify;
        }

        #region Function
        private TierService tierService = TierService.getInStance();
        private AccountabilityService accountService = new AccountabilityService();
        #endregion

        #region Session object
        public String WorkGroupCode
        {
            get
            {
                return "20170121162748444411";
            }
        }

        public DataTable dtTierMaster
        {
            get;
            set;
        }

        public DataTable dtTierMasterItem
        {
            get;
            set;
        }

        private DataTable _dtCharacterStructure;
        private DataTable dtCharacterStructure
        {
            get
            {
                if (_dtCharacterStructure == null)
                {
                    _dtCharacterStructure = CharacterService.getInstance().getCharacterStructure(ERPWAuthentication.SID, "");
                }
                return _dtCharacterStructure;
            }
        }

        private List<TierService.entityParticipant> _listParticipant;
        private List<TierService.entityParticipant> listParticipant
        {
            get
            {
                if (_listParticipant == null)
                {
                    _listParticipant = tierService.GetAllTierItemEmployeeDetail(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode
                    );
                }
                return _listParticipant;
            }
        }
        #endregion
        public bool FilterOwner
        {
            get
            {
                bool _FilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out _FilterOwner);
                return _FilterOwner;
            }
        }

        public string OwnerGroupCode
        {
            get
            {
                string _OwnerGroupCode = "";
                if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
                {
                    _OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
                }
                return _OwnerGroupCode;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindGroupDataForTierItem();
                    //bindDropdownRole();
                    getDropdownParticipant();
                    getDynamicParticipant();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        //private void bindDropdownRole()
        //{
        //    DataTable dtRole = tierService.searchRoleCriteriaData(ERPWAuthentication.SID, WorkGroupCode);
        //    ddlModalTierMasterRoleSelect.DataValueField = "HierarchyCode";
        //    ddlModalTierMasterRoleSelect.DataTextField = "HierarchyDesc";
        //    ddlModalTierMasterRoleSelect.DataSource = dtRole;
        //    ddlModalTierMasterRoleSelect.DataBind();
        //    ddlModalTierMasterRoleSelect.Items.Insert(0, new ListItem("Select Role", ""));
        //}

        private void BindGroupDataForTierItem()
        {
            dtTierMaster = tierService.searchTierMaster(ERPWAuthentication.SID, WorkGroupCode, "", "", "");
            dtTierMasterItem = tierService.searchTierMasterItem(ERPWAuthentication.SID, WorkGroupCode, "", "", "", "");

            DataTable dtGroupTier = tierService.searchTierGorupMaster(ERPWAuthentication.SID, WorkGroupCode, "", txtSearchTierGroupMaster.Text);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                dtGroupTier.DefaultView.RowFilter = "OwnerService = '" + OwnerGroupCode + "'";
                dtGroupTier = dtGroupTier.DefaultView.ToTable();
            }


            rptDataTierGroup.DataSource = dtGroupTier;
            rptDataTierGroup.DataBind();
            udpDataTier.Update();

            //bind data modal tier group
            rptTierData.DataSource = dtGroupTier;
            rptTierData.DataBind();

            ClearItem();
            ClientService.DoJavascript("bindRequestTooltip();UpdateDescriptionTierCodeMaster();minimalListEmployee();");

        }

        #region Bind Data ItemDataBound
        protected void rptDataTierGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptDataTierMaster = e.Item.FindControl("rptDataTierMaster") as Repeater;
            HiddenField hddTierGroupCode = e.Item.FindControl("hddTierGroupCode") as HiddenField;
            DataView dv = dtTierMaster.DefaultView;
            dv.RowFilter = "TierGroupCode = '" + hddTierGroupCode.Value + "'";
            rptDataTierMaster.DataSource = dv;
            rptDataTierMaster.DataBind();
        }

        protected void rptDataTierMaster_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptDataTierMasterItem = e.Item.FindControl("rptDataTierMasterItem") as Repeater;
            HiddenField hddTierGroupCode = e.Item.Parent.Parent.FindControl("hddTierGroupCode") as HiddenField;
            HiddenField hddTierCode = e.Item.FindControl("hddTierCode") as HiddenField;
            DataView dv = dtTierMasterItem.DefaultView;
            dv.RowFilter = "TierGroupCode = '" + hddTierGroupCode.Value + "' and TierCode = '" + hddTierCode.Value + "' ";
            rptDataTierMasterItem.DataSource = dv;
            rptDataTierMasterItem.DataBind();
        }

        #endregion

        protected void btnRebindTierGroupMasterPage_Click(object sender, EventArgs e)
        {
            try
            {
                BindGroupDataForTierItem();
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

        #region Tier Group

         private void bindTierDataToScreen(string TierGroupDescription)
        {
            DataTable dtTier = tierService.searchTierGorupMaster(ERPWAuthentication.SID, WorkGroupCode, "", TierGroupDescription);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                dtTier.DefaultView.RowFilter = "OwnerService = '" + OwnerGroupCode + "'";
                dtTier = dtTier.DefaultView.ToTable();
            }


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
                string TierCode = Validation.getCurrentServerStringDateTimeMillisecond() 
                    + new Random().Next(9).ToString() 
                    + new Random().Next(2).ToString() 
                    + new Random().Next(8).ToString();

                tierService.InsertTierMaster(
                    ERPWAuthentication.SID, 
                    WorkGroupCode,
                    TierCode, 
                    txtTierGroupDescription.Text,
                    ERPWAuthentication.EmployeeCode,
                    OwnerGroupCode
                );
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

        #endregion

        //protected void btnModalBindPepleForRoleSelect_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable dtPeople = tierService.searchPeopleForRoleSelect(
        //            ERPWAuthentication.SID,
        //            ""//ddlModalTierMasterRoleSelect.SelectedValue
        //        );
        //        rptTableRoleSelectPeople.DataSource = dtPeople;
        //        rptTableRoleSelectPeople.DataBind();
        //        udpTableRoleSelectPeople.Update();
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

        protected void btnModalCreateTierMasterForSaveTierMaster_Click(object sender, EventArgs e)
        {
            try
            {
                string SID = ERPWAuthentication.SID;
                string TierCode = tierService.getAutoGenerateTierMasterCode(SID, WorkGroupCode, 2);
                int Resolution = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtResolutionTime.Text)*60), out Resolution);
                int Requester = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtRequester.Text)*60), out Requester);
                int HeadShift = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtHeadShift.Text)*60), out HeadShift);
                int AVPSale = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtAVPSale.Text)*60), out AVPSale);
                int SVPSale = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtSVPSale.Text)*60), out SVPSale);

                //List<TierService.entityParticipant> en = JsonConvert.DeserializeObject<List<TierService.entityParticipant>>(txtTierResourceJsonDataForSave.Text);
                //List<TierService.entityParticipant> en = getEntityParticipant();

                tierService.SaveTierMaster(SID, WorkGroupCode, txtModalTierMasterTierGroup.Text,
                    TierCode, txtModalTierMasterTierCodeDescript.Text,
                    txtModalTierMasterTierItemDescript.Text,
                    ddlParticipantsDescription.SelectedValue, //txtChangeFolderSubProjectObject.Text, //ddlModalTierMasterRoleSelect.SelectedValue, 
                    new List<TierService.entityParticipant>(), ERPWAuthentication.EmployeeCode,
                    false, Resolution, Requester, HeadShift, AVPSale, SVPSale,false
                );

                BindGroupDataForTierItem();
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

        protected void btnModalCreateTierMasterForSaveTierItem_Click(object sender, EventArgs e)
        {
            try
            {
                string SID = ERPWAuthentication.SID;
                string TierCode = txtModalTierMasterTierCode.Text;
                int Resolution = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtResolutionTime.Text)*60), out Resolution);
                int Requester = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtRequester.Text)*60), out Requester);
                int HeadShift = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtHeadShift.Text)*60), out HeadShift);
                int AVPSale = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtAVPSale.Text)*60), out AVPSale);
                int SVPSale = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtSVPSale.Text)*60), out SVPSale);

                //List<TierService.entityParticipant> en = JsonConvert.DeserializeObject<List<TierService.entityParticipant>>(txtTierResourceJsonDataForSave.Text);
                //List<TierService.entityParticipant> en = getEntityParticipant();

                if (chkDynamicRole.Checked)
                {
                    List<TierService.entityParticipantDynamic> enParti = new List<TierService.entityParticipantDynamic>();
                    foreach (RepeaterItem item in rptRoleDynamic.Items)
                    {
                        CheckBox chkSelectOwner = item.FindControl("chkSelectOwner") as CheckBox;
                        DropDownList ddlRoleParticipants = item.FindControl("ddlRoleParticipants") as DropDownList;
                        if (chkSelectOwner.Checked && !string.IsNullOrEmpty(ddlRoleParticipants.SelectedValue))
                        {
                            HiddenField hddOwnerGroupCode = item.FindControl("hddOwnerGroupCode") as HiddenField;
                            enParti.Add(new TierService.entityParticipantDynamic
                            {
                                OwnerGroupCode = hddOwnerGroupCode.Value,
                                RoleCode = ddlRoleParticipants.SelectedValue
                            });
                        }
                    }

                    tierService.SaveTierMaster(
                        SID,
                        WorkGroupCode,
                        txtModalTierMasterTierGroup.Text,
                        TierCode,
                        txtModalTierMasterTierCodeDescript.Text,
                        txtModalTierMasterTierItemDescript.Text,
                        "",
                        new List<TierService.entityParticipant>(),
                        ERPWAuthentication.EmployeeCode,
                        true, Resolution,
                        Requester, HeadShift,
                        AVPSale, SVPSale, true
                    );

                    tierService.saveTierOwnerMappingRole(
                        SID, TierCode, txtModalTierMasterTierGroup.Text, enParti, 
                        ERPWAuthentication.EmployeeCode
                    );

                }
                else
                {
                    tierService.SaveTierMaster(
                        SID,
                        WorkGroupCode,
                        txtModalTierMasterTierGroup.Text,
                        TierCode,
                        txtModalTierMasterTierCodeDescript.Text,
                        txtModalTierMasterTierItemDescript.Text,
                        ddlParticipantsDescription.SelectedValue,
                        new List<TierService.entityParticipant>(),
                        ERPWAuthentication.EmployeeCode,
                        true, Resolution,
                        Requester, HeadShift,
                        AVPSale, SVPSale, false
                    );
                }
                BindGroupDataForTierItem();
                ClientService.AGSuccess("บันทึกสำเร็จ");
                ClientService.DoJavascript("closeInitiativeModal('ModalCreateTierMaster');");
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

        protected void btnDeleteTierMasterInTierGroup_Click(object sender, EventArgs e)
        {
            try
            {
                string sid = ERPWAuthentication.SID;
                string TierCode = txtModalTierMasterTierCode.Text;
                bool checkDelete = tierService.IsCheckTierMasterUserWorkingForDelete(
                    sid,
                    WorkGroupCode,
                     TierCode
                    );
                if (!checkDelete)
                {
                    throw new Exception("ไม่สามารถลบ Tier " + txtModalTierMasterTierCodeDescript.Text + " เนื่องจากมีการใช้งาน!");
                }
                tierService.DeleteTierMaster(sid, WorkGroupCode, txtModalTierMasterTierGroup.Text, txtModalTierMasterTierCode.Text);
                BindGroupDataForTierItem();
                ClientService.AGSuccess("Delete Tier " + txtModalTierMasterTierCodeDescript.Text + " สำเร็จ ");

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

        protected void btnbtnUpdateTierMasterInTierGroup_Click(object sender, EventArgs e)
        {
            try
            {
                tierService.UpdateDescriptionTierMaster(ERPWAuthentication.SID, WorkGroupCode, txtModalTierMasterTierCode.Text,
                                                        txtModalTierMasterTierCodeDescript.Text, ERPWAuthentication.EmployeeCode);
                BindGroupDataForTierItem();
                ClientService.AGSuccess("Update Tier " + txtModalTierMasterTierCodeDescript.Text+" สำเร็จ");
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

        public string getImagehtmlFormEmployee_V2(string TierCode, string Tier, string EmpType)
        {
            List<string> listEmployeeCode = listParticipant.Where(w =>
                w.TierCode.Equals(TierCode) &&
                w.Tier.Equals(Tier) &&
                w.EmpType.Equals(EmpType)
            ).ToList().Select(s => 
                s.EmpCode + "," + s.UpdateOnEmployee + "," + s.FullName_TH
            ).ToList();

            return getImagehtmlFormEmployee(string.Join("|", listEmployeeCode));
        }

        public String getImagehtmlFormEmployee(string employeeCode)
        {
            if (String.IsNullOrEmpty(employeeCode.Trim(',')))
            {
                return "";
            }

            string imgHtml = "";
            string imgEmployeeCode = "";
            string [] strimg = employeeCode.Split('|');
            for (int i = 0; i < strimg.Length; i++)
			{
                string [] ArrEmpData = strimg[i].Split(',');
                imgEmployeeCode = getPathImgEmployeeForEmployeeCode(ArrEmpData[0], ArrEmpData[1]);
                if (String.IsNullOrEmpty(imgEmployeeCode))
                {
                    imgEmployeeCode = "/images/user.png";
                }
                imgHtml += "<div><span>" + "<img class='image-box-card z-depth-1' src='" + imgEmployeeCode + "' "
                       + " data-toggle='tooltip' data-placement='top' title='" + ArrEmpData[2] + "' /></span>&nbsp;" + ArrEmpData[2] + "</div>";
            }
            return imgHtml;
        }

        public string getPathImgEmployeeForEmployeeCode(string EmployeeCode, string UpdateOnEmployee)
        {
            UserImageService.UserImage UserImg = new UserImageService.UserImage(
                              ERPWAuthentication.CompanyCode,
                              ERPWAuthentication.SID,
                              EmployeeCode,
                              UpdateOnEmployee
                          );
            return UserImg.Image_128;
        }

        private void ClearItem()
        {
            txtModalTierMasterTierCodeDescript.Enabled = true;
            txtModalTierMasterTierItemDescript.Enabled = true;
            udpEventModalCreateTierMaster.Update();

        }

        private void EnabledItem()
        {
            txtModalTierMasterTierCodeDescript.Enabled = false;
            //txtModalTierMasterTierItemDescript.Enabled = false;
            udpEventModalCreateTierMaster.Update();
        }

        #region Update Tier Item
        protected void btnBindDataTierMasterForUpdateTierItem_Click(object sender, EventArgs e)
        {
            try
            {
                /// Todo
                //DataTable dtRoleOld = tierService.SearchTierItemRoleSelected(
                //    ERPWAuthentication.SID,
                //    WorkGroupCode,
                //    txtModalTierMasterTierCode.Text,
                //    txtModalTierMasterTierItemCode.Text,
                //    txtTierResourceJsonDataForSave.Text
                //);

                //string MainDelegate = "";
                //List<string> OtherDelegate = new List<string>();
                //foreach (DataRow dr in dtRoleOld.Rows)
                //{
                //    if (string.IsNullOrEmpty(Convert.ToString(dr["DefaultMain"])))
                //    {
                //        OtherDelegate.Add(Convert.ToString(dr["EmployeeCode"]));
                //    }
                //    else
                //    {
                //        MainDelegate = Convert.ToString(dr["EmployeeCode"]);
                //    }
                //}
                //SmartSearchMainDelegate.SelectedCode = MainDelegate;
                //SmartSearchMainDelegate.rebindSmartSearch();
                //SmartSearchOtherDelegate.SelectedCode = string.Join(",", OtherDelegate);
                //SmartSearchOtherDelegate.rebindSmartSearch();

                List<TierService.entityParticipantDynamic> enParti = new List<TierService.entityParticipantDynamic>();
                if (chkDynamicRole.Checked)
                {
                    ddlParticipantsDescription.SelectedValue = "";
                    enParti = tierService.getTierOwnerMappingRole(
                        ERPWAuthentication.SID,
                        txtModalTierMasterTierCode.Text,
                        txtModalTierMasterTierItemCode.Text
                    );
                }

                btnChangeSubProjectObject_Click(null, null);
                foreach (RepeaterItem item in rptRoleDynamic.Items)
                {
                    CheckBox chkSelectOwner = item.FindControl("chkSelectOwner") as CheckBox;
                    HiddenField hddOwnerGroupCode = item.FindControl("hddOwnerGroupCode") as HiddenField;
                    DropDownList ddlRoleParticipants = item.FindControl("ddlRoleParticipants") as DropDownList;

                    var en = enParti.Where(w => w.OwnerGroupCode.Equals(hddOwnerGroupCode.Value)).ToList();
                    if (en.Count > 0)
                    {
                        chkSelectOwner.Checked = true;
                        try
                        {
                            ddlRoleParticipants.SelectedValue = en.First().RoleCode;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        chkSelectOwner.Checked = false;
                        ddlRoleParticipants.SelectedValue = "";
                    }

                    ddlRoleParticipants_SelectedIndexChanged(ddlRoleParticipants as object, null);
                }
                udpRoleDynamic.Update();

                EnabledItem();
                ClientService.DoJavascript("showInitiativeModal('ModalCreateTierMaster');swithModeButtonForUpdateTierItem();");
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
        protected void btnModalCreateTierMasterForUpdateTierItem_Click(object sender, EventArgs e)
        {
            try
            {
                string SID = ERPWAuthentication.SID;
                string TierCode = txtModalTierMasterTierCode.Text;
                int Resolution = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtResolutionTime.Text)*60), out Resolution);
                int Requester = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtRequester.Text)*60), out Requester);
                int HeadShift = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtHeadShift.Text)*60), out HeadShift);
                int AVPSale = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtAVPSale.Text)*60), out AVPSale);
                int SVPSale = 0;
                int.TryParse(Convert.ToString(Convert.ToInt32(txtSVPSale.Text)*60), out SVPSale);

                //List<TierService.entityParticipant> en = JsonConvert.DeserializeObject<List<TierService.entityParticipant>>(txtTierResourceJsonDataForSave.Text);
                //List<TierService.entityParticipant> en = getEntityParticipant();

                if (chkDynamicRole.Checked)
                {
                    List<TierService.entityParticipantDynamic> enParti = new List<TierService.entityParticipantDynamic>();
                    foreach (RepeaterItem item in rptRoleDynamic.Items)
                    {
                        CheckBox chkSelectOwner = item.FindControl("chkSelectOwner") as CheckBox;
                        DropDownList ddlRoleParticipants = item.FindControl("ddlRoleParticipants") as DropDownList;
                        if (chkSelectOwner.Checked && !string.IsNullOrEmpty(ddlRoleParticipants.SelectedValue))
                        {
                            HiddenField hddOwnerGroupCode = item.FindControl("hddOwnerGroupCode") as HiddenField;
                            enParti.Add(new TierService.entityParticipantDynamic
                            {
                                OwnerGroupCode = hddOwnerGroupCode.Value,
                                RoleCode = ddlRoleParticipants.SelectedValue
                            });
                        }
                    }

                    tierService.UpdateTierItemForSelectNewRole(
                        SID, WorkGroupCode, txtModalTierMasterTierGroup.Text, TierCode,
                        txtModalTierMasterTierItemCode.Text, txtModalTierMasterTierItemDescript.Text,
                        "",
                        txtHideTierItemSequence.Text, new List<TierService.entityParticipant>(),
                        ERPWAuthentication.EmployeeCode,
                        Resolution, Requester, HeadShift, AVPSale, SVPSale, true
                    );

                    tierService.saveTierOwnerMappingRole(
                        SID, TierCode, txtModalTierMasterTierItemCode.Text, enParti,
                        ERPWAuthentication.EmployeeCode
                    );

                }
                else
                {
                    tierService.UpdateTierItemForSelectNewRole(
                        SID, WorkGroupCode, txtModalTierMasterTierGroup.Text, TierCode,
                        txtModalTierMasterTierItemCode.Text, txtModalTierMasterTierItemDescript.Text,
                        ddlParticipantsDescription.SelectedValue,
                        txtHideTierItemSequence.Text, new List<TierService.entityParticipant>(),
                        ERPWAuthentication.EmployeeCode,
                        Resolution, Requester, HeadShift, AVPSale, SVPSale, false
                    );
                }

                BindGroupDataForTierItem();
                ClientService.AGSuccess("บันทึก " + txtModalTierMasterTierItemDescript.Text + " สำเร็จ");
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
        protected void btnDeleteTierItemInTierMaster_Click(object sender, EventArgs e)
        {
            try
            {
                tierService.getQueryeDeleteRoleOldForAddNewRole(
                    ERPWAuthentication.SID,
                    WorkGroupCode,
                    txtModalTierMasterTierCode.Text,
                    txtModalTierMasterTierItemCode.Text
                    );
                tierService.DeleteTierItemInTierMaster();
                BindGroupDataForTierItem();
                ClientService.AGSuccess("ลบรายการ " + txtModalTierMasterTierItemDescript.Text + " สำเร็จ");
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
        #endregion

        #region Public Function
        public string ConvertToTime(string time)
        {
            double xTime= 0;
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

            return "";
        }

        public string ConvertToTotalTime(string TierGroupCode, string TierCode)
        {
            double xTime = 0;
            DataRow[] drs = dtTierMasterItem.Select("TierGroupCode='" + TierGroupCode + "' and TierCode='" + TierCode + "'");
            if (drs.Length > 0)
            {
                foreach (DataRow item in drs)
                {
                    double resolution = 0;
                    double.TryParse(item["Resolution"].ToString(), out resolution);
                   
                    xTime += resolution;
                }
            }

            return ConvertToTime(xTime.ToString());
        }
        #endregion

        #region new entityParticipant
        //private List<TierService.entityParticipant> getEntityParticipant()
        //{
        //    List<TierService.entityParticipant> en = new List<TierService.entityParticipant>();
        //    if (string.IsNullOrEmpty(SmartSearchMainDelegate.SelectedCode.Trim()))
        //    {
        //        throw new Exception("กรุณาเลือก Default Main!");
        //    }

        //    en.Add(new TierService.entityParticipant
        //    {
        //        emplayeeCode = SmartSearchMainDelegate.SelectedCode.Trim(),
        //        type = "main"
        //    });
        //    foreach (string empCode in SmartSearchOtherDelegate.SelectedCode.Split(','))
        //    {
        //        if (empCode.Equals(SmartSearchMainDelegate.SelectedCode.Trim()))
        //        {
        //            continue;
        //        }
        //        if (!string.IsNullOrEmpty(empCode.Trim()))
        //        {
        //            en.Add(new TierService.entityParticipant
        //            {
        //                emplayeeCode = empCode.Trim(),
        //                type = "Participant"
        //            });
        //        }
        //    }
        //    return en;
        //}

        protected void getDropdownParticipant()
        {
            try
            {
                ddlParticipantsDescription.DataSource = dtCharacterStructure;
                ddlParticipantsDescription.DataValueField = "code";
                ddlParticipantsDescription.DataTextField = "desc";
                ddlParticipantsDescription.DataBind();
                ddlParticipantsDescription.Items.Insert(0, new ListItem("Select Role", ""));

                updEventObject.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        private void getDynamicParticipant()
        {
            DataTable dt = tierService.getOwnerServiceMaster(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode
            );

            rptRoleDynamic.DataSource = dt;
            rptRoleDynamic.DataBind();
            udpRoleDynamic.Update();
        }


        protected void btnChangeSubProjectObject_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtMain = CharacterService.getInstance().getRoleMappingEmployee(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ddlParticipantsDescription.SelectedValue,
                    true
                );
                rptMainDelegate.DataSource = dtMain;
                rptMainDelegate.DataBind();
                udpMainDelegate.Update();

                DataTable dtParticipants = CharacterService.getInstance().getRoleMappingEmployee(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ddlParticipantsDescription.SelectedValue,
                    false
                );
                rptParticipants.DataSource = dtParticipants;
                rptParticipants.DataBind();
                udpParticipants.Update();
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

        protected void rptRoleDynamic_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList ddlRole = e.Item.FindControl("ddlRoleParticipants") as DropDownList;
            ddlRole.DataSource = dtCharacterStructure;
            ddlRole.DataValueField = "code";
            ddlRole.DataTextField = "desc";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("Select Role", ""));

        }
        protected void ddlRoleParticipants_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlRole = sender as DropDownList;

                Repeater rptMainDelegate_Dynamic = ddlRole.Parent.FindControl("rptMainDelegate_Dynamic") as Repeater;
                UpdatePanel udpMainDelegate_Dynamic = ddlRole.Parent.FindControl("udpMainDelegate_Dynamic") as UpdatePanel;
                System.Web.UI.HtmlControls.HtmlTableRow tr_MainEmpty = ddlRole.Parent.FindControl("tr_MainEmpty") as System.Web.UI.HtmlControls.HtmlTableRow;

                Repeater rptParticipants_Dynamic = ddlRole.Parent.FindControl("rptParticipants_Dynamic") as Repeater;
                UpdatePanel udpParticipants_Dynamic = ddlRole.Parent.FindControl("udpParticipants_Dynamic") as UpdatePanel;
                System.Web.UI.HtmlControls.HtmlTableRow tr_ParticipantsEmpty = ddlRole.Parent.FindControl("tr_ParticipantsEmpty") as System.Web.UI.HtmlControls.HtmlTableRow;

                DataTable dtMain = CharacterService.getInstance().getRoleMappingEmployee(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ddlRole.SelectedValue,
                    true
                );
                tr_MainEmpty.Visible = dtMain.Rows.Count == 0;
                rptMainDelegate_Dynamic.DataSource = dtMain;
                rptMainDelegate_Dynamic.DataBind();
                udpMainDelegate_Dynamic.Update();

                DataTable dtParticipants = CharacterService.getInstance().getRoleMappingEmployee(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ddlRole.SelectedValue,
                    false
                );
                tr_ParticipantsEmpty.Visible = dtParticipants.Rows.Count == 0;
                rptParticipants_Dynamic.DataSource = dtParticipants;
                rptParticipants_Dynamic.DataBind();
                udpParticipants_Dynamic.Update();
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
        #endregion


    }
 }
