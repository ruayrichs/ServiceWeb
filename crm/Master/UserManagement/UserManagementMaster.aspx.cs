using agape.lib.constant;
using agape.proxy.data.dataset;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService;
using ERPW.Lib.F1WebService.ChangePwdService;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Config;
using Focusone.ICMWcfService;
using Newtonsoft.Json;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.Service;

namespace ServiceWeb.crm.Master.UserManagement
{
    public partial class UserManagementMaster : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.UserManagementView;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.UserManagementModify;
        }

        private UserManagementService userservice = UserManagementService.getInstance();
        private MasterConfigLibrary masterservice = MasterConfigLibrary.GetInstance();
        private ILookupICMService _icmLookupService = new LookupICMService();
        private String SYSTEM_ERP = "ERP";
        private String mDefuault_Profileid = "000001";//สิทธิบันทึกเอกสาร
        private string id { get { return Request["id"]; } }
        private String userid
        {
            get { return Session["userid_" + id] == null ? "" : (String)Session["userid_" + id]; }
            set { Session["userid_" + id] = value; }
        }

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
                    ControlUI();
                    txtissueDate.Text = Validation.Convert2DateDisplay(Validation.getCurrentServerStringDateTime().Substring(0, 8));
                    txtexpDate.Text = txtissueDate.Text;
                    binCriteria();
                    if (!(String.IsNullOrEmpty(userid)))
                    {
                        initData(userid);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }

            if (String.IsNullOrEmpty(userid) && !IsAllFeature)
            {
                Response.Redirect(Page.ResolveUrl("~/crm/Master/UserManagement/UserManagementCriteria.aspx"), true);
            }
        }

        private void binCriteria()
        {
           
            DataTable dt = masterservice.GetRoleCode(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

            
            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                dt.DefaultView.RowFilter = "AllPermission <> 'True'";
                dt = dt.DefaultView.ToTable();
                dt.DefaultView.RowFilter = string.Empty;
            }

            ddl_role_code.DataValueField = "RoleCode";
            ddl_role_code.DataTextField = "RoleName";
            ddl_role_code.DataSource = dt;
            ddl_role_code.DataBind();
            ddl_role_code.Items.Insert(0, new ListItem("เลือก", ""));

            //customer group
            dt = userservice.GetEmployeeGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
            ddlCustomerGroup.DataValueField = "DocumentTypeCode";
            ddlCustomerGroup.DataTextField = "Description";
            ddlCustomerGroup.DataSource = dt;
            ddlCustomerGroup.DataBind();
            if (ddlCustomerGroup.Items.Count != 1)
            {
                ddlCustomerGroup.Items.Insert(0, new ListItem("เลือก", ""));
            }
           
            //Title Name
            dt = _icmLookupService.GetTitleName(ERPWAuthentication.SID);
            ddlTitleName.DataValueField = "TitleCode";
            ddlTitleName.DataTextField = "TitleName";
            ddlTitleName.DataSource = dt;
            ddlTitleName.DataBind();
            ddlTitleName.Items.Insert(0, new ListItem("เลือก", ""));
            
            //ตำแหน่ง
            dt = _icmLookupService.getPosition(ERPWAuthentication.SID);
            ddlPosition.DataTextField = "DESCRIPTION";
            ddlPosition.DataValueField = "CODE";
            ddlPosition.DataSource = dt;
            ddlPosition.DataBind();
            ddlPosition.Items.Insert(0, new ListItem("เลือก", ""));


            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                ddl_Owner_Ser.Items.Clear();
                ddl_Owner_Ser.Items.Insert(0,
                    new ListItem(
                        ERPWAuthentication.Permission.OwnerGroupName,
                        ERPWAuthentication.Permission.OwnerGroupCode
                    )
                );
                ddl_Owner_Ser.Enabled = false;
                ddl_Owner_Ser.CssClass = "form-control form-control-sm";

                lstOwnerService.Attributes.Add("disabled", "");
            }
            else
            {
                //Owner Service
                dt = masterservice.GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
                ddl_Owner_Ser.DataTextField = "OwnerGroupName";
                ddl_Owner_Ser.DataValueField = "OwnerGroupCode";
                ddl_Owner_Ser.DataSource = dt;
                ddl_Owner_Ser.DataBind();
                ddl_Owner_Ser.Items.Insert(0, new ListItem("เลือก", ""));

                //================== เซ็ต list box สำหรับเลือก ใน owner service tab =====================================
                lstOwnerService.DataTextField = "OwnerGroupName";
                lstOwnerService.DataValueField = "OwnerGroupCode";
                lstOwnerService.DataSource = dt;
                lstOwnerService.DataBind();
                
            }


            dt = _icmLookupService.GetNationality(ERPWAuthentication.SID);
            ddlNational.DataTextField = "DESCRIPTION";
            ddlNational.DataValueField = "CODE";
            ddlNational.DataSource = dt;
            ddlNational2.DataTextField = "DESCRIPTION";
            ddlNational2.DataValueField = "CODE";
            ddlNational2.DataSource = dt;
            ddlNational.DataBind();
            ddlNational2.DataBind();
            ddlNational.Items.Insert(0, new ListItem("เลือก", ""));
            ddlNational2.Items.Insert(0, new ListItem("เลือก", ""));
            if (dt.Select("CODE='THA'").Length > 0)
            {
                ddlNational.SelectedValue = "THA";
                ddlNational2.SelectedValue = "THA";
            }

            dt = _icmLookupService.GetCountryBirth(ERPWAuthentication.SID);
            ddlCountryBirthPlace.DataTextField = "DESCRIPTION";
            ddlCountryBirthPlace.DataValueField = "CODE";
            ddlCountryBirthPlace.DataSource = dt;
            ddlCountryBirthPlace.DataBind();
            ddlCountryBirthPlace.Items.Insert(0, new ListItem("เลือก", ""));
            if (dt.Select("CODE='THA'").Length > 0)
            {
                ddlCountryBirthPlace.SelectedValue = "THA";
            }


            dt = _icmLookupService.GetLanguange(ERPWAuthentication.SID);
            ddlLanguane.DataTextField = "DESCRIPTION";
            ddlLanguane.DataValueField = "CODE";
            ddlLanguane.DataSource = dt;
            ddlLanguane.DataBind();
            ddlLanguane.Items.Insert(0, new ListItem("เลือก", ""));
            if (dt.Select("CODE='TH'").Length > 0)
            {
                ddlLanguane.SelectedValue = "TH";
            }

            dt = _icmLookupService.GetRegion(ERPWAuthentication.SID);
            ddlRegion.DataTextField = "DESCRIPTION";
            ddlRegion.DataValueField = "CODE";
            ddlRegion.DataSource = dt;
            ddlRegion.DataBind();
            ddlRegion.Items.Insert(0, new ListItem("เลือก", ""));
            if (dt.Select("CODE='AA'").Length > 0)
            {
                ddlRegion.SelectedValue = "AA";
            }

            dt = _icmLookupService.GetMarriageStatus(ERPWAuthentication.SID);
            ddlMarriageStatus.DataTextField = "DESCRIPTION";
            ddlMarriageStatus.DataValueField = "CODE";
            ddlMarriageStatus.DataSource = dt;
            ddlMarriageStatus.DataBind();
            ddlMarriageStatus.Items.Insert(0, new ListItem("เลือก", ""));
            if (dt.Select("CODE='S'").Length > 0)
            {
                ddlMarriageStatus.SelectedValue = "S";
            }
            
            dt = _icmLookupService.GetCountryBirth(ERPWAuthentication.SID);
            ddlCountryBirthPlace.DataTextField = "DESCRIPTION";
            ddlCountryBirthPlace.DataValueField = "CODE";
            ddlCountryBirthPlace.DataSource = dt;
            ddlCountryBirthPlace.DataBind();
            ddlCountryBirthPlace.Items.Insert(0, new ListItem("เลือก", ""));
            if (dt.Select("CODE='THA'").Length > 0)
            {
                ddlCountryBirthPlace.SelectedValue = "THA";
            }

            dt = _icmLookupService.GetCountryBirth(ERPWAuthentication.SID);
            ddlCountry.DataTextField = "DESCRIPTION";
            ddlCountry.DataValueField = "CODE";
            ddlCountry.DataSource = dt;
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("", ""));
            if (dt.Select("CODE='THA'").Length > 0)
            {
                ddlCountry.SelectedValue = "THA";
            }

            ////Role F1
            //DataTable dtRole = userservice.getRoleUser(ERPWAuthentication.SID, UserManagementService.SYSTEM_ERP);
            //ddlRole.DataValueField = "RoleID";
            //ddlRole.DataTextField = "RoleDisplay";
            //ddlRole.DataSource = dtRole;
            //ddlRole.DataBind();
            //ddlRole.Items.Insert(0, new ListItem("เลือกบทบาท", ""));
            //udpnBtnRole.Update();

            //Owner Service bind Data listbox
            dt = masterservice.GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            lstOwnerService.DataTextField = "OwnerGroupName";
            lstOwnerService.DataValueField = "OwnerGroupCode";
            lstOwnerService.DataSource = dt;
            lstOwnerService.DataBind();


        }

        private void initData(String usercode)
        {
            UserManagementService.DataModel en = userservice.getDataUserName(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, UserManagementService.SYSTEM_ERP, usercode);
            txtUsernameCreate.Text = en.userid;
            txtUsernameCreate.Enabled = string.IsNullOrEmpty(en.userid);
            txtUsernameOld.Text = en.username;
            txtPassword.Text = "";
            txtPassword.CssClass = "form-control form-control-sm";
            txtissueDate.Text = string.IsNullOrEmpty(en.startdate) ? "" : Validation.Convert2DateDisplay(en.startdate);
            txtexpDate.Text = string.IsNullOrEmpty(en.enddate) ? "" : Validation.Convert2DateDisplay(en.enddate);
            txtEmployeeCode.Text = en.EmployeeCode;
            txtEmail.Text = en.Email;
            txtMobilePhone.Text = en.MobilePhone;
            txtTelphone.Text = en.TelephoneNumber;
            chkStatus.Checked = en.USERSTATUS == "A" ? true : false;
            txtDepartment.Text = en.Department;
            ddlPosition.SelectedValue = en.PositionCode;

            try { ddlCustomerGroup.SelectedValue = en.EmployeeGroup; }
            catch (Exception ex) { }
            try { ddlTitleName.SelectedValue = en.NamePreFix; }
            catch (Exception ex) { }
            txtFirstname.Text = en.FirstName;
            txtLastName.Text = en.LastName;
            txtFirstname_TH.Text = en.FirstName_TH;
            txtLastName_TH.Text = en.LastName_TH;


            //tab ข้อมูลส่วนตัว
            try { ddlGender.SelectedValue = en.Gender; }
            catch (Exception ex) { }
            txtNatinalityCode.Text = en.SocialID;
            try { ddlNational.SelectedValue = en.NationalityCode; }
            catch (Exception ex) { }
            try { ddlNational2.SelectedValue = en.OtherNationalityCode; }
            catch (Exception ex) { }
            txtBirthDay.Text = string.IsNullOrEmpty(en.BirthDate) ? "" : Validation.Convert2DateDisplay(en.BirthDate);
            txtBirthPlace.Text = en.BirthPlace;
            txtBirthProvinvce.Text = en.BirthCity;
            try { ddlCountryBirthPlace.SelectedValue = en.BirthCountry; }
            catch (Exception ex) { }
            try { ddlLanguane.SelectedValue = en.LanguageCode; }
            catch (Exception ex) { }
            try { ddlRegion.SelectedValue = en.ReligionCode; }
            catch (Exception ex) { }
            try { ddlMarriageStatus.SelectedValue = en.MarryStatus; }
            catch (Exception ex) { }
            txtCountOfChild.Text = en.NoChild;

            //tab ข้อมูลที่อยู่
            txtHouseNo.Text = en.HouseNumber;
            txtRoad.Text = en.Street;
            txtSubDistinct.Text = en.Locality;
            txtDistinct.Text = en.Amphur;
            txtProvice.Text = en.CityCode;
            try { ddlCountry.SelectedValue = en.CountryCode; }
            catch (Exception ex) { }
            txtPostCode.Text = en.ZipCode ;

            //Role service web
            try { ddl_role_code.SelectedValue = en.RoleCode; }
            catch (Exception ex) { }
            //try { ddl_Owner_Ser.SelectedValue = en.OwnerService; }
            //catch (Exception ex) { }
            
            setMultiselect(en.EmployeeCode);

            //initOwnerServiceChip();
            udpEmployeeData.Update();
            udpEmployeeGeneral.Update();
            udpEmployeeAddress.Update();
            udpEmployeeOwnerService.Update();

        }

        //====================== owner service tab =================================================
        protected void setMultiselect (string empCode)
        {

            OwnerService ownerService = new OwnerService();
            DataTable owner_dt = ownerService.getMappingOwner(empCode);//get data ownerService
            List<DataRow> owner_list = owner_dt.AsEnumerable().ToList();
            


            for (int i = 0; i < owner_list.Count; i++)
            {
                for (int j = 0; j < lstOwnerService.Items.Count; j++)
                {
                    if (lstOwnerService.Items[j].Value == owner_list[i][1].ToString())
                    {
                        lstOwnerService.Items[j].Selected = true;
                    }
                }
                
            }
            initOwnerServiceChip();

        }
        protected void save_multiSelect(string empCode)
        {

            OwnerService ownerService = new OwnerService();
            List<string> owner = new List<string>();

            //qiwzee get data from listitem OwnerService
            foreach (ListItem item in lstOwnerService.Items)
            {
                if (item.Selected)
                {
                    owner.Add(item.Value);
                }
            }
            if (owner.Count > 0)
            {
                ownerService.clearDataForUpdate(empCode);
                ownerService.addMapingOwner(SID, CompanyCode, empCode, owner);
                setMultiselect(empCode);
            }
            else
            {
                throw new Exception(String.Join("<br>", "กรุณาเลือก owner service อย่างน้อย 1 รายการ"));
            }
         
        }
        protected void initOwnerServiceChip()
        {
            List<string> owner = new List<string>();

            //qiwzee get data from listitem OwnerService
            foreach (ListItem item in lstOwnerService.Items)
            {
                if (item.Selected)
                {
                    owner.Add(item.Text);
                }
            }
            rptOwnerService.DataSource = owner;
            rptOwnerService.DataBind();
            udpShowSelected.Update();
        }
        protected void btninitChip_Click(object sender, EventArgs e)
        {
            initOwnerServiceChip();
        }
        //==========================================================================================
        #region Button Event
        private bool isPasswordhavespecialChar(string password)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            int count = 0;
            foreach (var item in specialChar)
            {
                if (password.Contains(item))
                {
                    //MessageBox.Show("Contains special char");
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                

                UserManagementService.DataModel en = prepareEntityFromUI();
                #region Valid data
                List<string> listMes = new List<string>();
                if (string.IsNullOrEmpty(en.userid))
                {
                    listMes.Add("กรุณาระบุชื่อผู้ใช้งาน!!");
                }

                if (string.IsNullOrEmpty(userid) || !string.IsNullOrEmpty(txtPassword.Text))
                {
                    if (txtPassword.Text.Trim() == "")
                    {
                        listMes.Add("กรุณาระบุรหัสผ่าน!!");
                    }

                    if (txtPassword.Text.Trim().Length < 8)
                    {
                        listMes.Add("กรุณาระบุรหัสผ่านไม่น้อยกว่า 8 ตัวอักษร!!");
                    }

                    string LowerCase = "abcdefghijklmnopqrstuvwxyz";
                    string UperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string NumCase = "1234567890";
                    string SpecialCase = "!@#$%^&*()_+|~-=\\`{}[]:\";'<>?,./";

                    bool IsLowerCase = false;
                    bool IsUperCase = false;
                    bool IsNumCase = false;
                    bool IsSpecialCase = false;
                    for (int i = 0; i < txtPassword.Text.Trim().Length; i++)
                    {
                        string pass = txtPassword.Text.Trim()[i].ToString();

                        if (LowerCase.IndexOf(pass) >= 0)
                        {
                            IsLowerCase = true;
                        }
                        if (UperCase.IndexOf(pass) >= 0)
                        {
                            IsUperCase = true;
                        }
                        if (NumCase.IndexOf(pass) >= 0)
                        {
                            IsNumCase = true;
                        }
                        if (SpecialCase.IndexOf(pass) >= 0)
                        {
                            IsSpecialCase = true;
                        }
                    }

                    if (!IsLowerCase)
                    {
                        listMes.Add("รหัสผ่านต้องเป็นตัวอักษรพิมเล็กอย่างน้อย 1 ตัว"); 
                    }
                    if (!IsUperCase)
                    {
                        listMes.Add("รหัสผ่านต้องเป็นตัวอักษรพิมใหญ่อย่างน้อย 1 ตัว");
                    }
                    if (!IsNumCase)
                    {
                        listMes.Add("รหัสผ่านต้องเป็นตัวเลขอย่างน้อย 1 ตัว");
                    }
                    //if (isPasswordhavespecialChar(txtPassword.Text.Trim()))
                    if (!IsSpecialCase)
                    {
                        listMes.Add("รหัสผ่านต้องอักขระพิเศษอย่างน้อย 1 ตัว");
                    }
                }


                if (IsAllFeature)
                {
                    if (string.IsNullOrEmpty(en.startdate))
                    {
                        listMes.Add("กรุณาวันที่เริ่มต้น!");
                    }
                    if (string.IsNullOrEmpty(en.enddate))
                    {
                        listMes.Add("กรุณาระบุวันที่สิ้นสุด!");
                    }
                    if (listMes.Count > 0)
                    {
                        throw new Exception(String.Join("<br>", listMes));
                    }

                    int dates = 0;
                    int datee = 0;
                    Int32.TryParse(en.startdate, out dates);
                    Int32.TryParse(en.enddate, out datee);
                    if (dates > datee)
                    {
                        throw new Exception("วันที่สิ้นสุดไม่ถูกต้อง!");
                    }

                    if (string.IsNullOrEmpty(userid))
                    {
                        List<string> listError = new List<string>();
                        DataTable dtCheck = userservice.getUserByEmailORUserID(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, en.Email, userid);
                        foreach (DataRow dr in dtCheck.Rows)
                        {
                            if (en.userid == Convert.ToString(dr["UserName"]))
                            {
                                listError.Add("มีข้อมูลชื่อผู้ใช้งาน " + en.userid + " อยู่ในระบบแล้ว !! <br> กรุณาตั้งชื่อผู้ใช้ใหม่ !!");
                            }
                            if (en.Email == Convert.ToString(dr["Email"]))
                            {
                                listError.Add("มีข้อมูล " + Convert.ToString(dr["UserName"]) + " ใช้อีเมล์ " + en.Email + " อยู่ในระบบแล้ว !! <br> กรุณาตั้งใช้อีเมล์ใหม่ !!");
                            }
                        }

                        if (listError.Count > 0)
                        {
                            throw new Exception(String.Join("<br>", listError));
                        }
                    }

                    //Default Role F1
                    if (en.listRole.Find(x => x.profileid == mDefuault_Profileid) == null)
                    {
                        en.listRole.Add(new UserManagementService.AuthRole()
                        {
                            profileid = mDefuault_Profileid,
                            RoleID = "",
                            RoleName = "",
                            userid = en.userid
                        });
                    }

                    string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
                    en = userservice.saveUserSystem(UserManagementService.SYSTEM_ERP, sessionid, ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.UserName, en);



                }
                else
                {
                    if (listMes.Count > 0)
                    {
                        throw new Exception(String.Join("<br>", listMes));
                    }
                }
                #endregion

                userservice.saveUserAuthServiceWeb(
                    ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.UserName,
                    txtEmployeeCode.Text,
                    ddl_role_code.SelectedValue,
                    ""
                    //ddl_Owner_Ser.SelectedValue

                );



                if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(en.password))
                {
                    ChangePwdService PwdService = F1WebService.getChangePwdService();
                    string _result = PwdService.ChangePassword(ERPWAuthentication.SID, userid, en.password);
                }
                userid = en.userid;
                initData(userid);
                
                save_multiSelect(en.EmployeeCode);
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

        protected void btnValidateSave_Click(object sender, EventArgs e)
        {
            ClientService.DoJavascript("saveEmailMessage();");
        }

        protected void btnAddRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlRole.SelectedValue == "")
                {
                    throw new Exception("กรุณาเลือกข้อมูลก่อน กดเพิ่ม!");
                }
                string roleID = ddlRole.SelectedValue;
                List<UserManagementService.AuthRole> listRole = prepareRoleFromUI();
                if (listRole.Find(x => x.RoleID == roleID) != null)
                {
                    throw new Exception("Role : " + ddlRole.SelectedItem.Text + " ข้อมูลซ้ำไม่สามารถเพิ่มได้!");
                }
                string profileid = userservice.getProfileidFromRoleID(ERPWAuthentication.SID, roleID);
                listRole.Add(new UserManagementService.AuthRole()
                {
                    profileid = profileid,
                    RoleID = roleID,
                    RoleName = ddlRole.SelectedItem.Text.Split(':')[1].Trim()
                });
                rptRoleList.DataSource = listRole;
                rptRoleList.DataBind();
                udpnRoleList.Update();
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

        protected void btnDeleteRole_Click(object sender, EventArgs e)
        {
            try
            {
                string profileid = (sender as Button).CommandArgument;
                List<UserManagementService.AuthRole> listRole = prepareRoleFromUI();
                rptRoleList.DataSource = listRole.FindAll(x => x.profileid != profileid);
                rptRoleList.DataBind();
                udpnRoleList.Update();
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

        #region Perpare data UI

        private UserManagementService.DataModel prepareEntityFromUI()
        {
            UserManagementService.DataModel en = new UserManagementService.DataModel();
            en.userid = txtUsernameCreate.Text;
            en.username = !string.IsNullOrEmpty(txtUsernameOld.Text) ? txtUsernameOld.Text : en.userid;
            en.description = txtFirstname.Text +" "+ txtLastName.Text;
            en.startdate = string.IsNullOrEmpty(txtissueDate.Text) ? "" : Validation.Convert2DateDB(txtissueDate.Text);
            en.enddate = string.IsNullOrEmpty(txtexpDate.Text) ? "" : Validation.Convert2DateDB(txtexpDate.Text);
            en.USERSTATUS = chkStatus.Checked ? "A" : "I";
            en.password = txtPassword.Text.Trim();
            en.RoleCode = ddl_role_code.SelectedValue;//role service web
            en.OwnerService = ddl_Owner_Ser.SelectedValue;//Drop down Owner Service
            en.listRole = prepareRoleFromUI();//role f1
            System.Diagnostics.Debug.WriteLine(ddl_Owner_Ser.SelectedValue.ToString());
            //Employee
            en.EmployeeGroup = ddlCustomerGroup.SelectedValue; //"EMP01";
            en.EmployeeCode = txtEmployeeCode.Text;
            en.NamePreFix = ddlTitleName.SelectedValue;
            en.FirstName = txtFirstname.Text;
            en.LastName = txtLastName.Text;
            en.FirstName_TH = txtFirstname_TH.Text;
            en.LastName_TH = txtLastName_TH.Text;
            en.Email = txtEmail.Text;
            en.MobilePhone = txtMobilePhone.Text;
            en.TelephoneNumber = txtTelphone.Text;
            en.Department = txtDepartment.Text;
            en.PositionCode = ddlPosition.SelectedValue;

            //tab ข้อมูลส่วนตัว
            en.Gender = ddlGender.SelectedValue;
            en.SocialID = txtNatinalityCode.Text;
            en.NationalityCode = ddlNational.SelectedValue;
            en.OtherNationalityCode = ddlNational2.SelectedValue;
            en.BirthDate = string.IsNullOrEmpty(txtBirthDay.Text) ? "" : Validation.Convert2DateDB(txtBirthDay.Text);
            en.BirthPlace = txtBirthPlace.Text;
            en.BirthCity = txtBirthProvinvce.Text;
            en.BirthCountry = ddlCountryBirthPlace.SelectedValue;
            en.LanguageCode = ddlLanguane.SelectedValue;
            en.ReligionCode = ddlRegion.SelectedValue;
            en.MarryStatus = ddlMarriageStatus.SelectedValue;
            int nCountOfChild = 0;
            int.TryParse(txtCountOfChild.Text, out nCountOfChild);
            en.NoChild = nCountOfChild.ToString();

            //tab ข้อมูลที่อยู่
            en.HouseNumber = txtHouseNo.Text;
            en.Street = txtRoad.Text;
            en.Locality = txtSubDistinct.Text;
            en.Amphur = txtDistinct.Text;
            en.CityCode = txtProvice.Text;
            en.CountryCode = ddlCountry.SelectedValue;
            en.ZipCode = txtPostCode.Text;
            
            return en;
        }

        private List<UserManagementService.AuthRole> prepareRoleFromUI()
        {
            List<UserManagementService.AuthRole> lisiRole = new List<UserManagementService.AuthRole>();
            foreach (RepeaterItem item in rptRoleList.Items)
            {
                string JsonRoledata = (item.FindControl("lblRoleDataJson") as Label).Text;
                UserManagementService.AuthRole en = JsonConvert.DeserializeObject<UserManagementService.AuthRole>(JsonRoledata);
                lisiRole.Add(en);
            }
            return lisiRole;
        }
        #endregion

        #region Control UI

        private void ControlUI()
        {
            if (IsAllFeature)
            {
                ddlCustomerGroup.Enabled = true;
                txtFirstname.Enabled = true;
                txtLastName.Enabled = true;
                ddlTitleName.Enabled = true;
                txtFirstname_TH.Enabled = true;
                txtLastName_TH.Enabled = true;
                txtEmail.Enabled = true;
                txtMobilePhone.Enabled = true;
                txtTelphone.Enabled = true;
                ddlPosition.Enabled = true;
                txtDepartment.Enabled = true;
                //ddl_Owner_Ser.Enabled = true;
                chkStatus.Enabled = true;
                txtissueDate.Enabled = true;
                txtexpDate.Enabled = true;
                ddlRole.Enabled = true;
                txtNatinalityCode.Enabled = true;
                ddlNational.Enabled = true;
                ddlNational2.Enabled = true;
                ddlGender.Enabled = true;
                txtBirthDay.Enabled = true;
                txtBirthPlace.Enabled = true;
                txtBirthProvinvce.Enabled = true;
                ddlCountryBirthPlace.Enabled = true;
                ddlLanguane.Enabled = true;
                ddlRegion.Enabled = true;
                ddlMarriageStatus.Enabled = true;
                txtCountOfChild.Enabled = true;
                txtHouseNo.Enabled = true;
                txtRoad.Enabled = true;
                txtSubDistinct.Enabled = true;
                txtDistinct.Enabled = true;
                txtProvice.Enabled = true;
                ddlCountry.Enabled = true;
                txtPostCode.Enabled = true;
            }
            else
            {
                ddlCustomerGroup.Enabled = false;
                txtFirstname.Enabled = false;
                txtLastName.Enabled = false;
                ddlTitleName.Enabled = false;
                txtFirstname_TH.Enabled = false;
                txtLastName_TH.Enabled = false;
                txtEmail.Enabled = false;
                txtMobilePhone.Enabled = false;
                txtTelphone.Enabled = false;
                ddlPosition.Enabled = false;
                txtDepartment.Enabled = false;
                //ddl_Owner_Ser.Enabled = false;
                chkStatus.Enabled = false;
                txtissueDate.Enabled = false;
                txtexpDate.Enabled = false;
                ddlRole.Enabled = false;
                txtNatinalityCode.Enabled = false;
                ddlNational.Enabled = false;
                ddlNational2.Enabled = false;
                ddlGender.Enabled = false;
                txtBirthDay.Enabled = false;
                txtBirthPlace.Enabled = false;
                txtBirthProvinvce.Enabled = false;
                ddlCountryBirthPlace.Enabled = false;
                ddlLanguane.Enabled = false;
                ddlRegion.Enabled = false;
                ddlMarriageStatus.Enabled = false;
                txtCountOfChild.Enabled = false;
                txtHouseNo.Enabled = false;
                txtRoad.Enabled = false;
                txtSubDistinct.Enabled = false;
                txtDistinct.Enabled = false;
                txtProvice.Enabled = false;
                ddlCountry.Enabled = false;
                txtPostCode.Enabled = false; 
                lstOwnerService.Attributes.Add("disabled", "");
            }
        }

        #endregion

    }
}