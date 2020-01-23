using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Service;
using ServiceWeb.auth;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserProfile
{    
    public partial class UserProfile : AbstractsSANWebpage
    {
        private ServiceContractLibrary lib = new ServiceContractLibrary();
        private ERPW.Lib.Master.CustomerService libCustomer = new ERPW.Lib.Master.CustomerService();
        private UserProfileService userProfileService = UserProfileService.getInstance();

        private string PageID
        {
            get { return Request["pid"]; }
        }

        private string SessionID
        {
            get { return (string)Session[ApplicationSession.USER_SESSION_ID]; }
        }

        private string SID
        {
            get { return ERPWAuthentication.SID; }
        }

        private string CompanyCode
        {
            get { return ERPWAuthentication.CompanyCode; }
        }

        public string CustomerCode = "000";
        public int currentYear;
        public int birthYear;
        protected string mode
        {
            get
            {
                if (Session[ServiceContractLibrary.SESSION_MODE + PageID] == null)
                {
                    Session[ServiceContractLibrary.SESSION_MODE + PageID] = ApplicationSession.CREATE_MODE_STRING;
                }
                return (string)Session[ServiceContractLibrary.SESSION_MODE + PageID];
            }
            set { Session[ServiceContractLibrary.SESSION_MODE + PageID] = value; }
        }

        protected ServiceContractDataSet mainBean
        {
            get { return (ServiceContractDataSet)Session[ServiceContractLibrary.SESSION_BEAN + PageID]; }
            set { Session[ServiceContractLibrary.SESSION_BEAN + PageID] = value; }
        }

        private string _EmployeeCode = "";
        //public string EmployeeCode
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_EmployeeCode))
        //            _EmployeeCode = ActivityService.getInstance().getEmployeeCodeByLinkID(PublicAuthentication.CompanyCode, PublicAuthentication.SID, LinkID);
        //        return _EmployeeCode;
        //    }
        //}

        public string EmployeeCode
        {
            get
            {
                return string.IsNullOrEmpty(Convert.ToString(Request["linkid"])) ? ERPWAuthentication.EmployeeCode : Convert.ToString(Request["linkid"]);
            }
        }
        public bool ISMyProFile
        {
            get
            {
                if (!ERPWAuthentication.EmployeeCode.Equals(EmployeeCode))
                    return false;
                return EmployeeCode == ERPWAuthentication.EmployeeCode;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindingScreen();

                    if (mode.Equals(ApplicationSession.CHANGE_MODE_STRING))
                    {
                        ControlScreen();
                    }

                    if (!Page.IsPostBack)
                    {
                        if (!string.IsNullOrEmpty(EmployeeCode))
                        {
                            hfdStudentLinkId.Value = EmployeeCode;

                            bindingProfile(EmployeeCode);
                            //if (EmployeeCode == ERPWAuthentication.EmployeeCode)
                            //{
                            //    btnFollow.Style["display"] = "none";
                            //    btnCancelFollow.Style["display"] = "none";
                            //}
                            //else
                            //{
                            //    //Checkfollow();
                            //}
                        }

                        hdfMyLinkId.Value = ERPWAuthentication.EmployeeCode;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindingScreen()
        {
           
        }

        private void BindingItems()
        {
           
        }

        private void ControlScreen()
        {
           
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                BindingItems();
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

        protected void tbDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                string equipmentNo = btn.CommandArgument.Trim();

                DataRow[] drr = mainBean.master_customer_service_contract_item.Select("EquipmentNo='" + equipmentNo + "'");

                if (drr.Length > 0)
                {
                    drr[0].Delete();

                    BindingItems();
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

        private void PrepareSaveBean()
        {
            
            
        }

        private string GetTimePickerValue(TextBox tb)
        {
            if (tb.Text.Trim() != "" && tb.Text.Trim().Length == 5)
            {
                return Validation.Convert2TimeDB(tb.Text.Trim() + ":00");
            }

            return "000000";
        }

        private void bindingProfile(string linkid)
        {
            divImageProfile.Style["background-image"] = "url(" + PublicAuthentication.FocusOneLinkProfileImageByEmployeeCode(EmployeeCode) + "),url('/images/user.png')";
            divImageProfile_Redesign.Style["background-image"] = "url(" + PublicAuthentication.FocusOneLinkProfileImageByEmployeeCode(EmployeeCode) + "),url('/images/user.png')";

            DataTable dt = userProfileService.getStudentProfile(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, EmployeeCode);

            string birthDate = "";
            String positionCode = "";
            String positionOther = "";

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                lbHeaderName.Text = dr["FirstName_TH"].ToString() + "  " + dr["LastName_TH"].ToString();
                lbName.Text = dr["FirstName_TH"].ToString() + "  " + dr["LastName_TH"].ToString();
                lbResume.Text = dr["ProfileDetail"].ToString();
                txtResumeEdit.Text = dr["ProfileDetail"].ToString();
                txtFirstNameEdit.Text = dr["FirstName_TH"].ToString();
                txtLastNameEdit.Text = dr["LastName_TH"].ToString();
                lbEmail.Text = dr["Email"].ToString();

                lbfacebook.Text = dr["FaceBookID"].ToString();
                lbinstagram.Text = dr["InstagramID"].ToString();
                lbtwitter.Text = dr["TwitterID"].ToString();
                txtFaceBookEdit.Text = dr["FaceBookID"].ToString();
                txtInstagramEdit.Text = dr["InstagramID"].ToString();
                txtTwitterEdit.Text = dr["TwitterID"].ToString();

                positionCode = dr["Position"].ToString();
                if (String.IsNullOrEmpty(positionCode))
                {
                    positionOther = dr["PositionOther"].ToString();
                    lbPosition.Text = positionOther;
                }
                else
                {
                    lbPosition.Text = dr["PositionName"].ToString();
                }

                birthDate = dr["BirthDate"].ToString();
            }

            bindDropDownList(birthDate, positionCode, positionOther);
        }


        private void bindDropDownList(string birthDate, String positionCode, String positionOther)
        {
            currentYear = Convert.ToInt32(Validation.getCurrentServerStringDateTime().Substring(0, 4));
            birthYear = Convert.ToInt32(Validation.getCurrentServerStringDateTime().Substring(0, 4)) - 50;

            if (!string.IsNullOrEmpty(birthDate) && birthDate.Length == 8)
            {
                birthYear = Convert.ToInt32(birthDate.Substring(0, 4));
            }

            #region position
            DataTable dtPosition = userProfileService.getListPosition(ERPWAuthentication.SID);

            ddlPostionEdit.Attributes["onchange"] = "changePositionEdit(this);";
            ddlPostionEdit.DataTextField = "PositionName";
            ddlPostionEdit.DataValueField = "PositionCode";
            ddlPostionEdit.DataSource = dtPosition;
            ddlPostionEdit.DataBind();

            ddlPostionEdit.Items.Add(new ListItem("อื่น ๆ", ""));
            ddlPostionEdit.SelectedValue = positionCode;
            txtPositionOtherEdit.Text = positionOther;
            txtPositionOtherEdit.Enabled = String.IsNullOrEmpty(positionCode);


            #endregion
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                String otherPosition = (String.IsNullOrEmpty(ddlPostionEdit.SelectedValue)) ? txtPositionOtherEdit.Text : "";
                userProfileService.EditProfile(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, EmployeeCode,
                    ddlPostionEdit.SelectedValue,
                    otherPosition,
                    txtResumeEdit.Text, txtFirstNameEdit.Text,
                    txtLastNameEdit.Text, txtFaceBookEdit.Text, txtInstagramEdit.Text, txtTwitterEdit.Text);

                SaveFiles();
                ClientService.DoJavascript("hideAddProfile();");

                bindingProfile(EmployeeCode);
                udpProfile.Update();
                udpName.Update();
                udpTitle.Update();

                //ERPWAuthentication.ChangeSNALinkAuthentication(Validation.getCurrentServerStringDateTime());
                Response.Redirect(Page.ResolveUrl("~/UserProfile/UserProfile.aspx"));
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

        private void SaveFiles()
        {
            StreamWriter file = null;
            try
            {
                string ImageAndFilepath = "\\images\\profile\\128\\" + ERPWAuthentication.SID + "_" + ERPWAuthentication.CompanyCode + "_" + EmployeeCode + ".png";
                UploadPictureTitle.SavePath = ImageAndFilepath;


                DataTable dtTitleImage = UploadPictureTitle.SaveFilesNoRenameFixedFileName();

                try
                {
                    string part_Profile = System.Configuration.ConfigurationManager.AppSettings["path_link_profile_image"];
                    if (!string.IsNullOrEmpty(part_Profile))
                    {
                        string FileName = ERPWAuthentication.SID + "_" + ERPWAuthentication.CompanyCode + "_" + EmployeeCode + ".png";
                        //File.Copy(
                        //    Server.MapPath("/") + "images\\profile\\128\\" + FileName,
                        //    part_Profile + "\\" + FileName,
                        //    true
                        //);

                        ImgProfileService.UploadProfileProcess(
                            part_Profile,
                            ERPWAuthentication.SID,
                            ERPWAuthentication.CompanyCode,
                            EmployeeCode,
                            Server.MapPath("~") + "images\\profile\\128\\" + FileName
                        );

                        //ClientService.DoJavascript("clearjQueryCache();");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                    ClientService.AGError(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw;
                ClientService.AGError(ex.Message);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }

        //private void Checkfollow()
        //{
        //    bool IsFollow = AddFriendService.getInstance().IsFollow(ERPWAuthentication.SID, ERPWAuthentication.EmployeeCode, EmployeeCode);

        //    if (IsFollow)
        //    {
        //        btnFollow.Style["display"] = "none";
        //        btnCancelFollow.Style["display"] = "";
        //    }
        //    else
        //    {
        //        btnFollow.Style["display"] = "";
        //        btnCancelFollow.Style["display"] = "none";
        //    }
        //}



    }
}