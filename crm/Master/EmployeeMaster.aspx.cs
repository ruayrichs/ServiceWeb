using agape.entity.UserProfile;
using agape.lib.constant;
using agape.proxy.data.dataset;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;
using Focusone.ICMWcfService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master
{
    public partial class EmployeeMaster : System.Web.UI.Page
    {
        LookupICMService lookupICMService = new LookupICMService();
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        ILookupICMService _icmLookupService = new LookupICMService();
        string _session_pic_path = "SESSION_PIC_PATH";
        EmployeeService libEmployee = EmployeeService.GetInstance();

        public string SessionEmpCode
        {
            get { return Session["EmployeeMasterList.empCode"] != null ? Session["EmployeeMasterList.empCode"].ToString() : ""; }
            set { Session["EmployeeMasterList.empCode"] = value; }
        }

        public UserProfileBean UserProfileBean
        {
            get
            {
                return (UserProfileBean)Session[ApplicationSession.EMPLOYEE_SESSION_BEAN];
            }
        }

        public AuthUserDataset UMEDataset
        {
            get
            {
                if (Session["AuthUserDataset"] == null)
                { Session["AuthUserDataset"] = new AuthUserDataset(); }
                return (AuthUserDataset)Session["AuthUserDataset"];
            }
            set { Session["AuthUserDataset"] = value; }
        }

        private v_EmployeeDataset empDataSet
        {
            get
            {
                if (Session["DS_employee"] == null)
                    Session["DS_employee"] = new v_EmployeeDataset();
                return (v_EmployeeDataset)Session["DS_employee"];
            }
            set { Session["DS_employee"] = value; }
        }

        private DataTable SESSION_DT_LINK
        {
            get { return (DataTable)Session["EmployeeMaster.SESSION_DT_LINK"]; }
            set { Session["EmployeeMaster.SESSION_DT_LINK"] = value; }
        }

        public SNA.Lib.Master.EmployeeMaster EmpMaster
        {
            get { return Session["EmployeeMasterList.EmployeeMaster"] != null ? (SNA.Lib.Master.EmployeeMaster)Session["EmployeeMasterList.EmployeeMaster"] : null; }
            set { Session["EmployeeMasterList.EmployeeMaster"] = value; }
        }

        //private void ValidateField(int step)
        //{
        //    switch (step)
        //    {
        //        case 1:
        //            {
        //                if (string.IsNullOrWhiteSpace(ddlEmployeeGroup.SelectedValue))
        //                {
        //                    throw new Exception("กรุณาเลือกกลุ่มพนักงาน");
        //                }
        //                else if (string.IsNullOrWhiteSpace(ddlTitleName.SelectedValue))
        //                {
        //                    throw new Exception("กรุณาเลือกคำนำหน้าชื่อ");
        //                }
        //                else if (string.IsNullOrWhiteSpace(txtNameTH.Value.Trim()))
        //                {
        //                    throw new Exception("กรุณาระบุชื่อ");
        //                }
        //                else if (string.IsNullOrWhiteSpace(txtSurnameTH.Value.Trim()))
        //                {
        //                    throw new Exception("กรุณาระบุนามสกุล");
        //                }

        //                DataTable group = libEmployee.getEmployeeGroup(ERPWAuthentication.SID
        //                    , ERPWAuthentication.CompanyCode, "", ddlEmployeeGroup.SelectedValue);

        //                if (group.Rows.Count > 0)
        //                {
        //                    DataRow dr = group.Rows[0];
        //                    bool external = Convert.ToBoolean(dr["ExternalOrNot"]);

        //                    if (external)
        //                    {
        //                        if (String.IsNullOrEmpty(txtEmployeeCode.Text))
        //                        {
        //                            throw new Exception("กรุณาระบุ Employee Code");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception("Employee group not found.");
        //                }

        //                break;
        //            }
        //        case 2:
        //            {
        //                break;
        //            }

        //        case 3:
        //            {
        //                if (string.IsNullOrWhiteSpace(txtEmail.Text.Trim()))
        //                {
        //                    throw new Exception("กรุณาระบุอีเมล์");
        //                }
        //                //else if (HasEmailEmployee(txtEmail.Text.Trim()))
        //                //{
        //                //    throw new Exception("อีเมล์ : " + txtEmail.Text.Trim() + " นี้มีอยู่ในระบบแล้ว");
        //                //}

        //                break;
        //            }
        //    }
        //}

        ////private bool HasEmailEmployee(string email)
        ////{
        ////    bool hasValue = false;
        ////    if (ERPWAuthentication.Email != email)
        ////    {
        ////        hasValue = LookUpHelper.verifyEmail(SessionEmpCode, email, ERPWAuthentication.SID);
        ////    }
        ////    return hasValue;

        ////}

        //private void ControlPanel(int step)
        //{
        //}

        //private void BindDropDownList()
        //{
        //    BindDDLEmployeeGroup("");
        //    BindTitleName();
        //    BindNationality();
        //    BindLanguange();
        //    BindMarriageStatus();
        //    BindRegion();
        //    BindCountryBirth();
        //    BindCountry();
        //    BindBranch();
        //    BindPosition();
        //    BindCostCenter();
        //    BindDepartment();
        //}

        //private void BindDDLEmployeeGroup(string empType)
        //{
        //    DataTable dt = libEmployee.GetEmployeeGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, empType);
        //    ddlEmployeeGroup.DataTextField = "Description";
        //    ddlEmployeeGroup.DataValueField = "DocumentTypeCode";
        //    ddlEmployeeGroup.DataSource = dt;
        //    ddlEmployeeGroup.DataBind();
        //    ddlEmployeeGroup.SelectedIndex = 0;
        //}

        //private void BindTitleName()
        //{
        //    DataTable dt = _icmLookupService.GetTitleName(ERPWAuthentication.SID);
        //    ddlTitleName.DataTextField = "TitleName";
        //    ddlTitleName.DataValueField = "TitleCode";
        //    ddlTitleName.DataSource = dt;
        //    ddlTitleName.DataBind();
        //    ddlTitleName.SelectedValue = "002";
        //}

        //private void BindNationality()
        //{
        //    DataTable dt = _icmLookupService.GetNationality(ERPWAuthentication.SID);
        //    dt.DefaultView.Sort = "DESCRIPTION asc";
        //    ddlNational.DataTextField = "DESCRIPTION";
        //    ddlNational.DataValueField = "CODE";
        //    ddlNational.DataSource = dt;
        //    ddlNational.DataBind();
        //    if (dt.Select("CODE='THA'").Length > 0)
        //    {
        //        ddlNational.SelectedValue = "THA";
        //    }


        //    ddlNational2.DataTextField = "DESCRIPTION";
        //    ddlNational2.DataValueField = "CODE";
        //    ddlNational2.DataSource = dt;
        //    ddlNational2.DataBind();
        //    if (dt.Select("CODE='THA'").Length > 0)
        //    {
        //        ddlNational2.SelectedValue = "THA";
        //    }
        //}

        //private void BindLanguange()
        //{
        //    DataTable dt = _icmLookupService.GetLanguange(ERPWAuthentication.SID);
        //    ddlLanguane.DataTextField = "DESCRIPTION";
        //    ddlLanguane.DataValueField = "CODE";
        //    ddlLanguane.DataSource = dt;
        //    ddlLanguane.DataBind();
        //    if (dt.Select("CODE='TH'").Length > 0)
        //    {
        //        ddlLanguane.SelectedValue = "TH";
        //    }
        //}

        //private void BindMarriageStatus()
        //{
        //    DataTable dt = _icmLookupService.GetMarriageStatus(ERPWAuthentication.SID);
        //    ddlMarriageStatus.DataTextField = "DESCRIPTION";
        //    ddlMarriageStatus.DataValueField = "CODE";
        //    ddlMarriageStatus.DataSource = dt;
        //    ddlMarriageStatus.DataBind();
        //    if (dt.Select("CODE='S'").Length > 0)
        //    {
        //        ddlMarriageStatus.SelectedValue = "S";
        //    }
        //}

        //private void BindRegion()
        //{
        //    DataTable dt = _icmLookupService.GetRegion(ERPWAuthentication.SID);
        //    ddlRegion.DataTextField = "DESCRIPTION";
        //    ddlRegion.DataValueField = "CODE";
        //    ddlRegion.DataSource = dt;
        //    ddlRegion.DataBind();
        //    if (dt.Select("CODE='AA'").Length > 0)
        //    {
        //        ddlRegion.SelectedValue = "AA";
        //    }
        //}

        //private void BindCountryBirth()
        //{
        //    DataTable dt = _icmLookupService.GetCountryBirth(ERPWAuthentication.SID);
        //    dt.DefaultView.Sort = "DESCRIPTION asc";
        //    ddlCountryBirthPlace.DataTextField = "DESCRIPTION";
        //    ddlCountryBirthPlace.DataValueField = "CODE";
        //    ddlCountryBirthPlace.DataSource = dt;
        //    ddlCountryBirthPlace.DataBind();
        //    if (dt.Select("CODE='THA'").Length > 0)
        //    {
        //        ddlCountryBirthPlace.SelectedValue = "THA";
        //    }
        //}

        //private void BindCountry()
        //{
        //    DataTable dt = _icmLookupService.GetCountryBirth(ERPWAuthentication.SID);
        //    dt.DefaultView.Sort = "DESCRIPTION asc";
        //    ddlCountry.DataTextField = "DESCRIPTION";
        //    ddlCountry.DataValueField = "CODE";
        //    ddlCountry.DataSource = dt;
        //    ddlCountry.DataBind();
        //    if (dt.Select("CODE='THA'").Length > 0)
        //    {
        //        ddlCountry.SelectedValue = "THA";
        //    }
        //}

        //private void BindBranch()
        //{
        //    DataTable dt = _icmLookupService.getBranch(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
        //    ddlBranch.DataTextField = "DESCRIPTION";
        //    ddlBranch.DataValueField = "CODE";
        //    ddlBranch.DataSource = dt;
        //    ddlBranch.DataBind();
        //}

        //private void BindPosition()
        //{
        //    DataTable dt = _icmLookupService.getPosition(ERPWAuthentication.SID);
        //    ddlPosition.DataTextField = "DESCRIPTION";
        //    ddlPosition.DataValueField = "CODE";
        //    ddlPosition.DataSource = dt;
        //    ddlPosition.DataBind();
        //}

        //private void BindCostCenter()
        //{
        //    DataTable dt = _icmLookupService.getCostCenter(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
        //    ddlCostCenter.DataTextField = "DESCRIPTION";
        //    ddlCostCenter.DataValueField = "CODE";
        //    ddlCostCenter.DataSource = dt;
        //    ddlCostCenter.DataBind();
        //}

        //private void BindDepartment()
        //{
        //    DataTable dt = libEmployee.getDepartment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
        //    ddlDepartment.DataTextField = "DESCRIPTION";
        //    ddlDepartment.DataValueField = "CODE";
        //    ddlDepartment.DataSource = dt;
        //    ddlDepartment.DataBind();
        //}
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}