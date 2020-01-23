using agape.lib.constant;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.Service.Workflow;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.RequestTransection
{
    public partial class BusinessOwnerRequest : System.Web.UI.Page
    {
        private BusinessOwnerLib businessOwnerLib = new BusinessOwnerLib();
        private OwnerGroupLib ownerGroupLib = new OwnerGroupLib();

        private MasterConfigLibrary masterservice = MasterConfigLibrary.GetInstance();

        public String WorkGroupCode
        {
            get
            {
                return "20170121162748444411";
            }
        }

        private string _SID;
        private string _CompanyCode;
        private string _EmployeeCode;
        private string _FullNameEn;

        private DateTime cdt = DateTime.Now;
        private DateTime _nxdt;
        private DateTime nxdt { get
            {
                _nxdt = DateTime.Now.AddYears(20);
                return _nxdt;
            } }
        private string _year;
        private string _nxyear;
        private string _month;
        private string _nxmonth;
        private string _day;
        private string _nxday;
        private string _hour;
        private string _minute;
        private string _seconds;

        private string SID { get
            {
                if (string.IsNullOrEmpty(_SID))
                {
                    _SID = ERPWAuthentication.SID;
                }
                return _SID;
            } }
        private string CompanyCode { get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                {
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                }
                return _CompanyCode;
            } }
        private string EmployeeCode { get
            {
                
                if (string.IsNullOrEmpty(_EmployeeCode))
                {
                    _EmployeeCode = ERPWAuthentication.EmployeeCode;
                }
                return _EmployeeCode;
            } }
        private string FullNameEn { get
            {
                if (string.IsNullOrEmpty(_FullNameEn))
                {
                    _FullNameEn = ERPWAuthentication.FullNameEN;
                }
                return _FullNameEn;
            } }

        private string employee_group { get; set; }
        private string startdate { get; set; }
        private string enddate { get; set; }
        private string role { get; set; }
        private string OwnerGroupCode { get; set; }

        private string year
        {
            get
            {
                if (string.IsNullOrEmpty(_year))
                {
                    _year = cdt.Year.ToString();
                }
                return _year;
            }
        }
        private string nxyear
        {
            get
            {
                if (string.IsNullOrEmpty(_nxyear))
                {
                    _nxyear = nxdt.Year.ToString();
                }
                return _nxyear;
            }
        }
        private string month
        {
            get
            {
                if (string.IsNullOrEmpty(_month))
                {
                    if (cdt.Month < 10)
                    {
                        _month = "0" + cdt.Month.ToString();
                    }
                    else
                    {
                        _month = cdt.Month.ToString();
                    }

                }
                return _month;
            }
        }
        private string nxmonth { get
            {
                if (string.IsNullOrEmpty(_nxmonth))
                {
                    _nxmonth = nxdt.Month.ToString();
                }
                return _nxmonth;
            } }
        private string day
        {
            get
            {
                if (string.IsNullOrEmpty(_day))
                {
                    if (cdt.Day < 10)
                    {
                        _day = "0" + cdt.Day.ToString();
                    }
                    else
                    {
                        _day = cdt.Day.ToString();
                    }

                }
                return _day;
            }
        }
        private string nxday
        {
            get
            {
                //int nxday = cdt.AddDays(1);
                if (string.IsNullOrEmpty(_nxday))
                {
                    if (nxdt.Day < 10)
                    {
                        _nxday = "0" + nxdt.Day.ToString();
                    }
                    else
                    {
                        _nxday = nxdt.Day.ToString();
                    }

                }
                return _nxday;
            }
        }
        private string hour
        {
            get
            {
                if (string.IsNullOrEmpty(_hour))
                {
                    if (cdt.Hour < 10)
                    {
                        _hour = "0" + cdt.Hour.ToString();
                    }
                    else
                    {
                        _hour = cdt.Hour.ToString();
                    }

                }
                return _hour;
            }
        }
        private string minute
        {
            get
            {
                if (string.IsNullOrEmpty(_minute))
                {
                    if (cdt.Minute < 10)
                    {
                        _minute = "0" + cdt.Minute.ToString();
                    }
                    else
                    {
                        _minute = cdt.Minute.ToString();
                    }

                }
                return _minute;
            }
        }
        private string seconds
        {
            get
            {
                if (string.IsNullOrEmpty(_seconds))
                {
                    if (cdt.Second < 10)
                    {
                        _seconds = "0" + cdt.Second.ToString();
                    }
                    else
                    {
                        _seconds = cdt.Second.ToString();
                    }

                }
                return _seconds;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindDataTable();
                initPage();
            }
        }
        private void initPage()
        {
            //customer group
            DataTable dt = new DataTable();
            dt = userservice.GetEmployeeGroup(SID, CompanyCode, "");
            employee_group_inp.DataValueField = "DocumentTypeCode";
            employee_group_inp.DataTextField = "Description";
            employee_group_inp.DataSource = dt;
            employee_group_inp.DataBind();
            if (employee_group_inp.Items.Count != 1)
            {
                employee_group_inp.Items.Insert(0, new ListItem("Select", ""));
            }

            //ddl employee_position
            dt = masterservice.GetRoleCode(SID, CompanyCode);
            position_inp.DataValueField = "RoleCode";
            position_inp.DataTextField = "RoleName";
            position_inp.DataSource = dt;
            position_inp.DataBind();
            position_inp.Items.Insert(0, new ListItem("Select", ""));

            //string startdate = year + "/" + month + "/" + day;
            //string enddate = year + "/" + month + "/" + nxday;

            startDate_inp.Text = day + "/" + month + "/" + year;
            endDate_inp.Text = nxday + "/" + nxmonth + "/" + nxyear;
        }

        private void bindDataTable()
        {
            List<BusinessOwnerEN> lists = businessOwnerLib.readBySEQ(SID, CompanyCode);
            for(int index = 0; index < lists.Count; index++)
            {
                lists[index].SignUp_DateTime = Validation.Convert2DateTimeDisplay(lists[index].SignUp_DateTime);
                lists[index].UpdateStatus_On = Validation.Convert2DateTimeDisplay(lists[index].UpdateStatus_On);
                lists[index].Activation_DateTime = Validation.Convert2DateTimeDisplay(lists[index].Activation_DateTime);
                //if (lists[index].DisplayControlApproveButton.Equals("APPROVE"))
                //{
                //    lists[index].DisplayControlApproveButton = "d-none";
                //}
                //if (lists[index].DisplayControlRejectButton.Equals("REJECT"))
                //{
                //    lists[index].DisplayControlRejectButton = "d-none";
                //}
                if (!lists[index].Status.Equals("WAITING"))
                {
                    lists[index].DisplayControlApproveButton = "d-none";
                    lists[index].DisplayControlRejectButton = "d-none";
                }
            }
            rptSEQItem.DataSource = lists.OrderByDescending(o => Convert.ToInt32(o.SEQ)).ToList();
            rptSEQItem.DataBind();
            udpnItems.Update();

            ClientService.DoJavascript("afterLoad();");
        }

        private void updateSEQ(string SEQ, string Status)
        {
            //string SEQ;
            //string Status;
            //SEQ = seq_inp.Text;
            //Status = status_inp.Text;

            BusinessOwnerEN rawData = businessOwnerLib.readOneBySEQ(SID, CompanyCode, SEQ);
            rawData.Status = Status;
            rawData.UpdateStatus_On = year + month + day + hour + minute + seconds;
            businessOwnerLib.updateOneBySEQ(SID, CompanyCode, SEQ, rawData);

            ClientService.AGSuccess("Update data success");
        }

        #region usermanagement
        private UserManagementService userservice = UserManagementService.getInstance();
        private String mDefuault_Profileid = "000001";//สิทธิบันทึกเอกสาร

        private UserManagementService.DataModel prepareEntityFromUI(BusinessOwnerEN businessOwnerEN, string Password, string OwnerServiceCode)
        {
            UserManagementService.DataModel en = new UserManagementService.DataModel();
            //en.userid = businessOwnerEN.FirstName + "." + businessOwnerEN.LastName.Substring(0,2);
            en.userid = businessOwnerEN.Username;
            //en.username = !string.IsNullOrEmpty(txtUsernameOld.Text) ? txtUsernameOld.Text : en.userid;
            en.username = businessOwnerEN.Username;
            //en.description = txtFirstname.Text + " " + txtLastName.Text;
            en.description = businessOwnerEN.FirstName + " " + businessOwnerEN.LastName;
            en.startdate = year+month+day;
            en.enddate = ((int.Parse(year)+1).ToString())+month+day;
            //en.USERSTATUS = chkStatus.Checked ? "A" : "I";
            en.USERSTATUS = "A";
            //en.password = txtPassword.Text.Trim();
            en.password = Password;
            en.RoleCode = this.role;//role service web
            //en.RoleCode = "ROLE001";
            //en.OwnerService = ddl_Owner_Ser.SelectedValue;//Drop down Owner Service
            en.OwnerService = OwnerServiceCode;

            en.listRole = new List<UserManagementService.AuthRole>();
            en.listRole.Add(new UserManagementService.AuthRole
            {
                profileid = mDefuault_Profileid,
                RoleID = "",
                RoleName = "",
                userid = en.userid
            });

            //en.listRole = prepareRoleFromUI();//role f1
            //en.listRole = "";
            //System.Diagnostics.Debug.WriteLine(ddl_Owner_Ser.SelectedValue.ToString());

            //Employee
            en.EmployeeGroup = this.employee_group;
            en.EmployeeCode = "";
            en.NamePreFix = "";
            //en.FirstName = txtFirstname.Text;
            en.FirstName = businessOwnerEN.FirstName;
            //en.LastName = txtLastName.Text;
            en.LastName = businessOwnerEN.LastName;
            //en.FirstName_TH = txtFirstname_TH.Text;
            en.FirstName_TH = businessOwnerEN.FirstName;
            //en.LastName_TH = txtLastName_TH.Text;
            en.LastName_TH = businessOwnerEN.LastName;
            en.Email = businessOwnerEN.Email;
            en.MobilePhone = businessOwnerEN.Phone;
            en.TelephoneNumber = "";
            en.Department = "";
            en.PositionCode = "";

            //tab ข้อมูลส่วนตัว
            en.Gender = "";
            en.SocialID = "";
            en.NationalityCode = "";
            en.OtherNationalityCode = "";
            en.BirthDate = "";
            en.BirthPlace = "";
            en.BirthCity = "";
            en.BirthCountry = "";
            en.LanguageCode = "";
            en.ReligionCode = "";
            en.MarryStatus = "";
            int nCountOfChild = 0;
            int.TryParse("0", out nCountOfChild);
            en.NoChild = nCountOfChild.ToString();

            //tab ข้อมูลที่อยู่
            en.HouseNumber = "";
            en.Street = "";
            en.Locality = "";
            en.Amphur = "";
            en.CityCode = "";
            en.CountryCode = "";
            en.ZipCode = "";

            return en;
        }

        public void prepareApproveUser(string SID, string CompanyCode, BusinessOwnerEN businessOwnerEN, string Password, string OwnerServiceCode)
        {
            //string OwnerServiceCode = "";
            UserManagementService.DataModel en = prepareEntityFromUI(businessOwnerEN, Password, OwnerServiceCode);
            en.startdate = Validation.Convert2DateDB(this.startdate);
            en.enddate = Validation.Convert2DateDB(this.enddate);
            //System.Diagnostics.Debug.WriteLine(en.userid);
            #region Valid data
            List<string> listMes = new List<string>();
            if (string.IsNullOrEmpty(en.userid))
            {
                listMes.Add("กรุณาระบุชื่อผู้ใช้งาน!!");
            }

            if (listMes.Count > 0)
            {
                throw new Exception(String.Join("<br>", listMes));
            }

            if (string.IsNullOrEmpty(en.userid))
            {
                List<string> listError = new List<string>();
                DataTable dtCheck = userservice.getUserByEmailORUserID(SID, CompanyCode, en.Email, en.userid);
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

            #endregion

            string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
            en = userservice.saveUserSystem(UserManagementService.SYSTEM_ERP, sessionid, SID, CompanyCode, businessOwnerEN.Username, en);
        }

        #endregion
        private void prepareUpdateSEQ()
        {
            string SEQ;
            string Status;
            string Password;
            SEQ = seq_inp.Text;
            Status = status_inp.Text;
            //Password = businessOwnerLib.CreatePassword(8);
            Password = businessOwnerLib.CreatePassword(true, true, true, true, false, 8);

            BusinessOwnerEN rawData = businessOwnerLib.readOneBySEQ(SID, CompanyCode, SEQ);
            rawData.UpdateStatus_By = EmployeeCode;
            rawData.DisplayUpdated_BY_FullNameEn = FullNameEn;
            rawData.Status = Status;
            rawData.UpdateStatus_On = year + month + day + hour + minute + seconds;
            rawData.BusinessOwnerCode = OwnerGroupCode;
            
            OwnerGroupEN OGrawData = new OwnerGroupEN();
            OGrawData.SID = SID;
            OGrawData.CompanyCode = CompanyCode;
            OGrawData.OwnerGroupCode = rawData.BusinessOwnerCode;
            OGrawData.OwnerGroupName = rawData.BusinessOwner;
            OGrawData.CREATED_BY = EmployeeCode;
            OGrawData.CREATED_ON = year + month + day + hour + minute + seconds;
            OGrawData.UPDATED_BY = EmployeeCode;
            OGrawData.UPDATED_ON = year + month + day + hour + minute + seconds;
            OGrawData.RoleConfig = "";
            OGrawData.Email = rawData.Email;

            #region switch
            switch (Status)
            {
                case "APPROVE":

                    string TierGroupCode = Validation.getCurrentServerStringDateTimeMillisecond()
                        + new Random().Next(9).ToString()
                        + new Random().Next(2).ToString()
                        + new Random().Next(8).ToString();

                    #region create new business owner
                    ownerGroupLib.createRow(SID, CompanyCode, OGrawData);
                    #endregion
                    
                    #region create new user
                    prepareApproveUser(SID, CompanyCode, rawData, Password, rawData.BusinessOwnerCode);
                    #endregion

                    businessOwnerLib.approveUser(SID, CompanyCode, rawData, Password);//send email to user

                    #region Create Doctype For Ticket
                    businessOwnerLib.GenDoctypeTicket(SID, CompanyCode, OGrawData.OwnerGroupCode, TierGroupCode, EmployeeCode);
                    #endregion
                    
                    #region Create SLA Default
                    HierarchyService.getInstance().InsertHierarchyType(
                        OGrawData.OwnerGroupCode,
                        OGrawData.OwnerGroupName,
                        "Tier"
                    );
                    businessOwnerLib.GenDefaultPossibleEntry(
                        SID, CompanyCode, OGrawData.OwnerGroupCode, EmployeeCode
                    );
                    String EmployeeOwner = ServiceTicketLibrary.LookUpTable(
                        "EmployeeCode",
                        "ERPW_Role_Maping_Employee",
                        "where SID = '" + SID + @"' AND CompanyCode = '" + CompanyCode + @"' AND OwnerService = '" + OGrawData.OwnerGroupCode + @"' "
                    );

                    string HierachyCatalog = HierarchyService.getInstance().insertMatHierachyCatalog(
                        ERPWAuthentication.SID,
                        "Tier",
                        OGrawData.OwnerGroupCode,
                        OGrawData.OwnerGroupName + " Tier 1",
                        "",
                        ERPWAuthentication.UserName
                    );
                    string CharacterCode = Guid.NewGuid().ToString();
                    CharacterService.getInstance().AddCharacter(
                        SID,
                        CharacterCode,
                        "",
                        "Tier",
                        OGrawData.OwnerGroupCode
                    );

                    CharacterService.getInstance().AddCharacterWithPerson(
                        ERPWAuthentication.SID,
                        "",
                        EmployeeOwner,
                        HierachyCatalog,
                        CharacterCode,
                        true
                    );

                    List<string> listTierCode = TierService.getInStance().InsertTierMaster(
                        SID,
                        WorkGroupCode,
                        TierGroupCode,
                        "Default SLA (" + OGrawData.OwnerGroupName + ")",
                        EmployeeCode, 
                        OGrawData.OwnerGroupCode
                    );

                    int Resolution = 300;
                    int Requester = 0;
                    int HeadShift = 0;
                    int AVPSale = 0;
                    int SVPSale = 0;

                    DataTable dtPriority = ServiceTicketLibrary.GetInstance().GetPriorityMaster(SID);
                    int TierCodeID = 0;
                    foreach (DataRow drPriority in dtPriority.Rows)
                    {
                        //string TierCode = TierService.getInStance().getAutoGenerateTierMasterCode(SID, WorkGroupCode, 2, TierCodeID);
                        TierService.getInStance().SaveTierMaster(
                            SID,
                            WorkGroupCode,
                            TierGroupCode,
                            listTierCode[TierCodeID],
                            drPriority["PriorityCode"].ToString() + " : " + drPriority["Description"].ToString(),
                            "Tier 1",
                            HierachyCatalog,
                            new List<TierService.entityParticipant>(),
                            ERPWAuthentication.EmployeeCode,
                            true,
                            Resolution,
                            Requester,
                            HeadShift,
                            AVPSale, 
                            SVPSale,
                            false
                        );
                        TierCodeID++;
                    }
                    #endregion

                    #region Create Client Default

                    #endregion
                    
                    #region Create CI Default

                    #endregion

                    updateSEQ(SEQ, Status); //update data
                    break;
                case "REJECT":
                    try
                    {
                        businessOwnerLib.rejectUser(SID, CompanyCode, rawData); //send email to user
                    }
                    catch (Exception) { }
                    updateSEQ(SEQ, Status); //update data
                    break;
                default:
                    ClientService.AGError("Status invalid");
                    break;
            }
            #endregion
        }
        
        protected void changeStatus_Click(object sender, EventArgs e)
        {
            prepareUpdateSEQ();
            //updateSEQ();
            bindDataTable();
            ClientService.AGSuccess("Update data success");
            ClientService.AGLoading(false);
            ClientService.DoJavascript("$('#userdesc_modal').modal('hide')");
        }
        private void onChangeStatus()
        {
            DropDownList ddl = new DropDownList();
            //ddl.Items.Insert(0, new ListItem)
            prepareUpdateSEQ();
            //updateSEQ();
            bindDataTable();
            ClientService.AGSuccess("Update data success");
            ClientService.DoJavascript("$('#userdesc_modal').modal('hide')");
        }

        protected void confirm_btn_Click(object sender, EventArgs e)
        {
            //ClientService.AGMessage("Hello World");
            System.Diagnostics.Debug.WriteLine("Hello World");
        }

        protected void updateSEQ_Click(object sender, EventArgs e)
        {
            int _startdate;
            int _enddate;
            int _count;
            try
            {
                if (string.IsNullOrEmpty(OwnerGroupCode_inp.Text))
                {
                    throw new Exception("Enter Ownergroup Code !!");
                }
                if (OwnerGroupCode_inp.Text.Length > 3)
                {
                    throw new Exception("Ownergroup Code limit 3 character !!");
                }
                if (!checkDataText(OwnerGroupCode_inp.Text))
                {
                    throw new Exception("Ownergroup Code must be englist or number !!");
                }
                this.OwnerGroupCode = OwnerGroupCode_inp.Text;
                _count = ownerGroupLib.isThisCodeInDatabase(SID,CompanyCode,this.OwnerGroupCode);
                if (_count != 0)
                {
                    throw new Exception("Ownergroup Code was used please enter new Ownergroup Code !!");
                }

                if (string.IsNullOrEmpty(employee_group_inp.SelectedValue))
                {
                    throw new Exception("Enter Employee Group !!");
                }
                this.employee_group = employee_group_inp.SelectedValue;

                if (string.IsNullOrEmpty(position_inp.SelectedValue))
                {
                    throw new Exception("Enter Role !!");
                }
                this.role = (string)position_inp.SelectedValue.ToString();

                if (string.IsNullOrEmpty(startDate_inp.Text))
                {
                    throw new Exception("Please select startdate !!");
                }
                this.startdate = startDate_inp.Text;

                if (string.IsNullOrEmpty(endDate_inp.Text))
                {
                    throw new Exception("Please select enddate !!");
                }
                this.enddate = endDate_inp.Text;

                _startdate = int.Parse(Validation.Convert2DateDB(this.startdate));
                _enddate = int.Parse(Validation.Convert2DateDB(this.enddate));
                if (_startdate > _enddate)
                {
                    throw new Exception("startdate and enddate invalidate !!");
                }
                //System.Diagnostics.Debug.WriteLine("ownergrooupcode: " + this.OwnerGroupCode);
                //System.Diagnostics.Debug.WriteLine("employee_group: " + employee_group);
                //System.Diagnostics.Debug.WriteLine("position: " + position_inp.SelectedValue);
                //System.Diagnostics.Debug.WriteLine("start: " + _startdate + " enddate: " + _enddate);
                onChangeStatus();
            }
            catch(Exception err)
            {
                ClientService.AGError(ObjectUtil.Err(err.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        private bool checkDataText(string input)
        {
            bool IsValit = true;
            string InputCheck = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+|~-=\\`{}[]:\";'<>?,./";
            for (int i = 0; i < input.Trim().Length && IsValit; i++)
            {
                if (InputCheck.IndexOf(input[i].ToString()) >= 0)
                {
                    IsValit = true;
                }
                else
                {
                    IsValit = false;
                }
            }
            return IsValit;
        }
    }
}