using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service.Workflow;
using ServiceWeb.Service;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using Agape.FocusOne.Utilities;

namespace ServiceWeb.MasterConfig
{
    public partial class OwnerService : AbstractsSANWebpage
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
        private AppClientLibrary ClientLib = new AppClientLibrary();

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

        public class EmployeeList
        {
            public string EmployeeListID { get; set; }
            public string EmployeeListDesc { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindingData();
                    getDropdownParticipant();
                   
                }
                if (string.IsNullOrEmpty(tbCorporatePermissionKey.Text))
                {
                    GenKey.Text = "Re-Generate Permission Key";
                }
                else
                {
                    GenKey.Text = "Generate Permission Key";
                }

            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        #region binding table

        private void BindingData()
        {
            DataTable dt = lib.GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
            dt.Columns.Add("desc");
            DataTable dtchastruc = dtCharacterStructure;
            foreach (DataRow row in dt.Rows)
            {
                DataRow[] dtr = dtchastruc.Select("code = '" + row["RoleConfig"].ToString() + "'");
                if (dtr.Length > 0)
                {
                    row["desc"] = dtr[0]["desc"].ToString();
                }
                else
                {
                    row["desc"] = "";
                }
            }
            rptItems.DataSource = dt;
            rptItems.DataBind();
            udpnItems.Update();

            ClientService.DoJavascript("bindingDataTableJS();");
        }
        #endregion

        #region CRUD

        public void setpermission(string code)
        {
            List<AppClientModel.CorporatePermissionKeyModel> dtclientPermission = ClientLib.getDataCorporatePermissionKeyModel(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code);

            int max = 0;
            List<int> listLine = new List<int>();
            foreach (var item in dtclientPermission)
            {
                int x = Int32.Parse(item.LineNumber.ToString());
                listLine.Add(x);
                max = listLine.Max();

            }

            if (dtclientPermission.Count > 0)
            {
                AppClientModel.CorporatePermissionKeyModel PermissionEntity = dtclientPermission[max - 1];
                foreach (var getmaxline in dtclientPermission)
                {
                    if (Int32.Parse(getmaxline.LineNumber.ToString()) == max)
                    {
                        tbCorporatePermissionKey.Text = PermissionEntity.CorporatePermissionKey;
                    }
                }

            }
            else
            {
                tbCorporatePermissionKey.Text = "";
            }

        }

        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {
                hdfMode.Value = ApplicationSession.CREATE_MODE_STRING;
                navpermissiontab.Visible = false;
                tbCode.Text = "";
                tbName.Text = "";
                tbMail.Text = "";
                tbCode.Enabled = true;
                ddlParticipantsDescription.SelectedIndex = 0;
                rptMainDelegate.DataSource = new DataTable();
                rptMainDelegate.DataBind();
                rptParticipants.DataSource = new DataTable();
                rptParticipants.DataBind();
                udpn.Update();
                udpn2.Update();
                udpnnav.Update();
                udpParticipants.Update();
                udpMainDelegate.Update();

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
                navpermissiontab.Visible = true;
                string code = hdfEditCode.Value;
                List<EmployeeList> GetEmpList = new List<EmployeeList>();
                hdfEditCode.Value = "";
                DataTable dt = lib.GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code);
                DataTable dtCBA = lib.GetMasterConfigCBAGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code);

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;
                    List<EmployeeList> listEmail = new List<EmployeeList>();
                    foreach (DataRow dr in dtCBA.Rows)
                    {
                        string ListCBACode = Convert.ToString(dr["EmployeeCodeCBA"]);
                        string ListCBAtxt = Convert.ToString(dr["FirstName"] + " " + dr["LastName"]);
                        if (!string.IsNullOrEmpty(ListCBACode) || !string.IsNullOrEmpty(ListCBAtxt))
                        {
                            string[] arrListCBA = ListCBACode.Split(',');
                            string[] arrListCBAtxt = ListCBAtxt.Split(',');
                            for (int i = 0; i < arrListCBA.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(arrListCBA[i].Trim()))
                                {
                                    if (!string.IsNullOrEmpty(arrListCBAtxt[i].Trim()))
                                    {
                                        GetEmpList.Add(new EmployeeList { EmployeeListID = ListCBACode, EmployeeListDesc = ListCBAtxt });
                                    }
                                }
                            }
                        }
                    }
                    tbCode.Text = dt.Rows[0]["OwnerGroupCode"].ToString();
                    tbName.Text = dt.Rows[0]["OwnerGroupName"].ToString();
                    tbMail.Text = dt.Rows[0]["Email"].ToString();
                    setpermission(code);
                    try
                    {
                        ddlParticipantsDescription.SelectedValue = dt.Rows[0]["RoleConfig"].ToString();
                        btnChangeSubProjectObject_Click(null, null);
                    }
                    catch (Exception)
                    {
                        ddlParticipantsDescription.SelectedIndex = 0;
                    }

                    tbCode.Enabled = false;
                    rptEmp.DataSource = GetEmpList;
                    rptEmp.DataBind();
                    udpnPermission.Update();
                    udpn.Update();
                    udpnEmp.Update();
                    udpParticipants.Update();
                    udpn2.Update();
                    udpnnav.Update();
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
                string mail = tbMail.Text.Trim();
                List<string> listHD = new List<string>();
                Button btn = (sender as Button);
                Repeater rptApprovalProcedureStateGateInnerStructure = btn.Parent.FindControl("rptEmp") as Repeater;
                foreach (RepeaterItem item in rptApprovalProcedureStateGateInnerStructure.Items)
                {
                    HiddenField hddApprovalProcedure_EmployeeCode = item.FindControl("hddEmployeeListID") as HiddenField;
                    listHD.Add(hddApprovalProcedure_EmployeeCode.Value);
                }
                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode
                        , code, name, ERPWAuthentication.UserName, ddlParticipantsDescription.SelectedValue, mail);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode
                        , code, name, mail, ERPWAuthentication.UserName, ddlParticipantsDescription.SelectedValue);
                }
                lib.SaveOwnerEmpCAB(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode
                        , code, listHD, ERPWAuthentication.UserName);

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

                lib.DeleteMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code);

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

        #endregion

        #region CABTAB


        protected void AddEmpList(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);
                Repeater rptEmp = btn.Parent.FindControl("rptEmp") as Repeater;

                string FullName = AutoCompleteEmployee.SelectedText;
                string ID = AutoCompleteEmployee.SelectedValue;

                List<EmployeeList> GetEmpList = new List<EmployeeList>();

                foreach (RepeaterItem item in rptEmp.Items)
                {

                    HiddenField getHDDEmployeeListDesc = item.FindControl("hddEmployeeListDesc") as HiddenField;
                    HiddenField getHDDEmployeeListID = item.FindControl("hddEmployeeListID") as HiddenField;
                    if (ID == getHDDEmployeeListID.Value && FullName == getHDDEmployeeListDesc.Value)
                    {
                        AutoCompleteEmployee.SelectedValueRefresh = "";
                    }
                    else
                    {
                        GetEmpList.Add(
                   new EmployeeList { EmployeeListID = getHDDEmployeeListID.Value, EmployeeListDesc = getHDDEmployeeListDesc.Value });
                        AutoCompleteEmployee.SelectedValueRefresh = "";
                    }
                }

                if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(FullName))
                {
                    GetEmpList.Add(
                   new EmployeeList { EmployeeListID = ID, EmployeeListDesc = FullName });
                    AutoCompleteEmployee.SelectedValueRefresh = "";
                }
                else
                {
                    AutoCompleteEmployee.SelectedValueRefresh = "";
                }



                rptEmp.DataSource = GetEmpList;
                rptEmp.DataBind();
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

        protected void DelEmpList(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                string code = btn.CommandArgument;
                List<EmployeeList> GetEmpList = new List<EmployeeList>();
                foreach (RepeaterItem item in rptEmp.Items)
                {
                    HiddenField getHDDEmployeeListDesc = item.FindControl("hddEmployeeListDesc") as HiddenField;
                    HiddenField getHDDEmployeeListID = item.FindControl("hddEmployeeListID") as HiddenField;
                    GetEmpList.Add(
                    new EmployeeList { EmployeeListID = getHDDEmployeeListID.Value, EmployeeListDesc = getHDDEmployeeListDesc.Value });
                }
                GetEmpList.RemoveAt(Int32.Parse(code));
                rptEmp.DataSource = GetEmpList;
                rptEmp.DataBind();
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

        protected void GenKey_Click(object sender, EventArgs e)
        {           
            
            string GenerateDateTime = Validation.getCurrentServerStringDateTime();
           
            try
            {

                AppClientModel.CorporatePermissionKeyModel permissionEntity = new AppClientModel.CorporatePermissionKeyModel();
                tbCorporatePermissionKey.Text = permissionEntity.CorporatePermissionKey;

                ClientLib.GenerateCorperatePermissionKey(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    tbCode.Text.ToString(),
                    GenerateDateTime,
                    ERPWAuthentication.EmployeeCode
                );
                setpermission(tbCode.Text.ToString());
                ClientService.AGSuccess("บันทึกสำเร็จ");
               

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


    }
}