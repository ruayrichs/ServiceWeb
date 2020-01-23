using agape.entity;
using agape.lib.constant;
using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ServiceWeb.auth;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using SNA.Lib.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.F1WebService.ICMUtils;

namespace ServiceWeb.Tier
{
    public partial class TierZero : AbstractsSANWebpage //System.Web.UI.Page
    {
        UniversalService universalService = new UniversalService();        
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        public string WorkGroupCode = "20170121162748444411";
        public string AobjectlinkServiceFromActivity
        {
            get { return (string)Session["AobjectlinkServiceFromActivity_saleservice"]; }
            set { Session["AobjectlinkServiceFromActivity_saleservice"] = value; }
        }

        private tmpServiceCallDataSet serviceCallEntity
        {
            get { return Session["ServicecallEntity"] == null ? new tmpServiceCallDataSet() : (tmpServiceCallDataSet)Session["ServicecallEntity"]; }
            set { Session["ServicecallEntity"] = value; }
        }

        public TierZeroService tierzeroser = new TierZeroService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ERPWAuthentication.SID == null || ERPWAuthentication.SID == "")
                {
                    AutoLoginSpider();
                }
                AobjectlinkServiceFromActivity = Request["aobj"] as string;
                CustomerSelect.DataBind();
                //txtYear.Text = Validation.getCurrentServerDateTime().Year.ToString();

            }
        }

        private void AutoLoginSpider()
        {
            try
            {
                if (Request.QueryString.Count == 0)
                {
                    throw new Exception("no logged in");
                }

                string mode = Request.QueryString["mode"];
                string id = Request.QueryString["id"];
                string sc = Request.QueryString["sc"];
                string sid = Request.QueryString["sid"];
                string lang = Request.QueryString["lang"];

                if (lang == null || lang == "")
                {
                    Session[ApplicationSession.USER_SESSION_LANG] = "en-US";
                }
                else
                {
                    Session[ApplicationSession.USER_SESSION_LANG] = lang;
                }             

                string email = AGStringCipher.Decrypt(id, sc);
                
                if (mode.Equals("cus"))
                {
                    mode = SystemModeControlService.ConstantCustomerSystemMode.Mode;
                }
                else if (mode.Equals("sup"))
                {
                    mode = SystemModeControlService.ConstantSupplierSystemMode.Mode;
                }
                else
                {
                    mode = ConfigurationHelper.getValue("system.mode");
                }

                string motherSid = SystemModeControlService.GetMotherSID(sid);
                SystemModeControlService.SystemModeEntities modeEn = SystemModeControlService.getInstanceMode(mode);

                ERPWAutoLoginService AutoLogin = new ERPWAutoLoginService(sid, email, modeEn);                 

                Response.Redirect(Page.ResolveUrl("~/"), true);
            }
            catch (System.Threading.ThreadAbortException tae)
            {
                // do nothing
            }
            catch (SoapException soe)
            {
                Response.Redirect(Page.ResolveUrl("~/login.aspx"));

            }
            catch (Exception ex)
            {
                Response.Redirect(Page.ResolveUrl("~/login.aspx"));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnSaveFastService_Click(object sender, EventArgs e)
        {
            try
            {
                
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

        protected void btnNewCall_Click(object sender, EventArgs e)
        {
            try
            {
                ResetBox();
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

        private void clearForm()
        {
            //string docType = ddlServiceCallType.SelectedValue == null ? "" : ddlServiceCallType.SelectedValue.ToString();
            //string obj = ERPWAuthentication.SID + docType + txtYear.Text + getCallerID();

            //setAttachMent("", false
            //    , docType, true
            //    , "", true
            //    , txtYear.Text, true);
            //clear screen
            //ddlServiceCallType.SelectedIndex = -1;
            //ddl_call_type.SelectedIndex = -1;
            //ddl_project.SelectedValue = "";
            ////CustomerSelect.SelectedValue = "";

            //ddl_project_element.DataSource = DTProjectElement;
            //ddl_project_element.DataBind();

            //_tb_contactperson.contactPerson_SelectedIndex = -1;
            //_tb_contactperson.contactPhone_SelectedIndex = -1;
            //_tb_contactperson.contactEmail_SelectedIndex = -1;
            //_tb_contactperson.contactAddress_SelectedIndex = -1;

            //ddl_priority.SelectedValue = "";
            //ddl_servierityTran.SelectedIndex = -1;
            txtProblemDetail.Text = "";

            //ddl_EquipmentNo.SelectedIndex = -1;
            //txtSerialNo.Text = "";
            //ddl_source_of_problem.SelectedIndex = -1;
            //ddl_problem_type.SelectedIndex = -1;
            //ddl_call_type.SelectedIndex = -1;
            //txtRemark.Text = "";

            //_cbb_priority.SelectedValue = "";
            //ddl_problem_group.SelectedIndex = -1;

            //string StructureCode = universalService.getStrucyureCodeByProjectCode(ERPWAuthentication.SID, WorkGroupCode, ddl_project.SelectedValue);

        }


        //public void getDataSave()
        //{
        //    //serviceCallEntity = new tmpServiceCallDataSet();
        //    if (serviceCallEntity.Tables["cs_servicecall_header"].Rows.Count <= 0)
        //    {
        //        throw new Exception("No Data found in Header!!");
        //    }

        //    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< GetData Header >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //    string doctype = ddlServiceCallType.SelectedValue == null ? "" : ddlServiceCallType.SelectedValue.ToString();
        //    string fiscalyear = txtYear.Text;
        //    DataRow drHeader = serviceCallEntity.Tables["ERPW_Service_tier0"].Rows[0];

        //    drHeader["sid"] = ERPWAuthentication.SID;
        //    drHeader["CompanyCode"] = ERPWAuthentication.CompanyCode;
        //    drHeader["Channel"] = ERPWAuthentication.CompanyCode;
        //    drHeader["DocType"] = doctype;
        //    drHeader["Fiscalyear"] = fiscalyear;
        //    drHeader["DOCDATE"] = Validation.Convert2DateDB(Validation.getCurrentServerDate());
        //    drHeader["CustomerCode"] = CustomerSelect.SelectedValue;
        //    drHeader["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
        //    drHeader["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
        //    drHeader["ContractPersonName"] = "";
        //    drHeader["ContractPersonTel"] = "";
        //    drHeader["Email"] = "";
        //    drHeader["Address"] = "";
        //    drHeader["Priority"] = ddl_priority.SelectedValue;
        //    drHeader["CallStatus"] = "01";
        //    drHeader["HeaderText"] = txtProblemDetail.Text;
        //    drHeader["ProjectCode"] = ddl_project.SelectedValue;
        //    drHeader["ProjectElement"] = ddl_project_element.SelectedValue == null ? "" : ddl_project_element.SelectedValue.ToString();

        //    int round = 1;
        //    foreach (RepeaterItem rptItem in rptEquipmentMappingContact.Items)
        //    {
        //        DataRow drContact = serviceCallEntity.Tables["cs_servicecall_contract"].NewRow();
        //        drContact["SID"] = ERPWAuthentication.SID;
        //        drContact["ObjectID"] = Session["ObjectID"].ToString();
        //        drContact["ItemNo"] = (round++).ToString().PadLeft(4, '0');
        //        drContact["PersonName"] = (rptItem.FindControl("lbContactCode") as Label).Text;
        //        drContact["EMail"] = (rptItem.FindControl("lbContactEmail") as Label).Text;
        //        drContact["Telephone"] = (rptItem.FindControl("lbContactPhone") as Label).Text;
        //        drContact["PersonAddress"] = (rptItem.FindControl("lbContactAddress") as Label).Text;
        //        drContact["Remark"] = (rptItem.FindControl("lbContactRemark") as Label).Text;
        //        drContact["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
        //        drContact["CREATED_ON"] = Validation.getCurrentServerStringDateTime();

        //        serviceCallEntity.Tables["cs_servicecall_contract"].Rows.Add(drContact);
        //    }
        //}

        private String getNewItemId()
        {
            int numrows = serviceCallEntity.Tables["cs_servicecall_item"].Rows.Count;
            numrows++;

            return Convert.ToString(numrows).PadLeft(3, '0');
        }

        private String getCallerID()
        {
            int numrows = serviceCallEntity.Tables["cs_servicecall_item"].Rows.Count;
            numrows++;
            //int numberrow = 

            return Convert.ToString(numrows).PadLeft(4, '0');
        }

        private void addInitData()
        {
            if (serviceCallEntity.Tables["cs_servicecall_Contactdetail_Header"].Rows.Count <= 0)
            {
                DataTable dtContactHeader = serviceCallEntity.Tables["cs_servicecall_Contactdetail_Header"];
                DataRow drContactHeader = dtContactHeader.NewRow();
                drContactHeader["sid"] = ERPWAuthentication.SID;
                drContactHeader["ObjectID"] = Session["ObjectID"];
                dtContactHeader.Rows.Add(drContactHeader);
            }

            if (serviceCallEntity.Tables["cs_servicecall_Properties_Header"].Rows.Count <= 0)
            {
                DataTable dtPropertiesHeader = serviceCallEntity.Tables["cs_servicecall_Properties_Header"];
                DataRow drPropertiesHeader = dtPropertiesHeader.NewRow();
                drPropertiesHeader["sid"] = ERPWAuthentication.SID;
                drPropertiesHeader["ObjectID"] = Session["ObjectID"];
                dtPropertiesHeader.Rows.Add(drPropertiesHeader);
            }

            if (serviceCallEntity.Tables["cs_servicecall_Properties_Item"].Rows.Count <= 0)
            {
                DataTable dtPropertiesItem = serviceCallEntity.Tables["cs_servicecall_Properties_Item"];
                DataRow drPropertiesItem = dtPropertiesItem.NewRow();
                drPropertiesItem["sid"] = ERPWAuthentication.SID;
                drPropertiesItem["ObjectID"] = Session["BObjectID"];
                drPropertiesItem["PropertiesCode"] = "01";
                dtPropertiesItem.Rows.Add(drPropertiesItem);
            }

            if (serviceCallEntity.Tables["cs_servicecall_subject"].Rows.Count <= 0)
            {
                DataTable dtsubject = serviceCallEntity.Tables["cs_servicecall_subject"];
                DataRow drsubject = dtsubject.NewRow();
                drsubject["sid"] = ERPWAuthentication.SID;
                drsubject["ObjectID"] = Session["BObjectID"];
                drsubject["xLineNo"] = "001";
                dtsubject.Rows.Add(drsubject);
            }
        }

        private List<string> validateForm()
        {
            List<string> _rs = new List<string>();
            if (string.IsNullOrEmpty(CustomerSelect.SelectedValue))
            {
                _rs.Add("กรุณาระบุ ลูกค้า");
            }
            //if (string.IsNullOrEmpty(ddlServiceCallType.SelectedValue))
            //{
            //    _rs.Add("กรุณาระบุ ประเภทเอกสาร");
            //}
            //if (string.IsNullOrEmpty(ddl_priority.SelectedValue))
            //{
            //    _rs.Add("กรุณาระบุ ลำดับความสำคัญ");
            //}
            //if (string.IsNullOrEmpty(ddl_servierityTran.SelectedValue))
            //{
            //    _rs.Add("กรุณาระบุ ความรุนแรง");
            //}
            //if (string.IsNullOrEmpty(ddl_problem_group.SelectedValue))
            //{
            //    _rs.Add("กรุณาระบุ กลุ่มของปัญหา");
            //}
            //if (string.IsNullOrEmpty(ddl_source_of_problem.SelectedValue))
            //{
            //    _rs.Add("กรุณาระบุ ที่มาของปัญหา");
            //}
            //if (string.IsNullOrEmpty(ddl_problem_type.SelectedValue))
            //{
            //    _rs.Add("กรุณาระบุ ประเภทของปัญหา");
            //}
            //if (string.IsNullOrEmpty(ddl_call_type.SelectedValue))
            //{
            //    _rs.Add("กรุณาระบุ ที่มาของการแจ้งบริการ");
            //}

            return _rs;
        }

        public string displayContantCustomer(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = "<span style='color:#aaa;'>ไม่ได้ระบุ</span>";
            }

            return text;
        }

        protected void btnTireZeroNewSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Channel = "2"; //set  "1 E-Mail 2 Web   3 System";
                string EMail = txtEmail.Text;
                string CustomerCode = CustomerSelect.SelectedValue;
                string CustomerName = CustomerSelect.SelectedText;
                string TelNo = txtTelNo.Text;
                string Subject = txtSubject.Text;
                string Detail = txtProblemDetail.Text;
                //string Status = ddlDocStatus.SelectedValue;
                string TicketNumber = "";
                string TicketType = "";

                ClientService.DoJavascript("saveTierZerItem('" + Channel + "', '" + EMail + "', '" + CustomerCode + "', '" + CustomerName + "', '" + TelNo + "', '" + Subject + "', '" + Detail + "', '0', '" + TicketNumber + "', '" + TicketType + "');");
                ClientService.AGSuccess("Input " + Subject + "Success");
                ResetBox();
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

        public void ResetBox()
        {
            txtEmail.Text = "";
            //CustomerSelect.SelectedValue = "";
            
            txtTelNo.Text = "";
            txtSubject.Text = "";
            txtProblemDetail.Text = "";
            //CustomerSelect.RebindData();
            CustomerSelect.ResetValue();
            updAddTierZero.Update();

        }

        private void getcontact_person()
        {
            string custcode = CustomerSelect.SelectedValue.Trim();

            DataTable dt = new DataTable();

            if (custcode == "")
            {
                dt.Columns.Add("BOBJECTLINK");
                dt.Columns.Add("NAME1");
            }
            else
            {
                dt = AfterSaleService.getInstance().getContactPerson(ERPWAuthentication.CompanyCode, custcode, "TRUE");
            }


            //_ddl_contact_person.Items.Clear();
            //_ddl_contact_person.Items.Add(new ListItem("", ""));
            //_ddl_contact_person.AppendDataBoundItems = true;
            //_ddl_contact_person.DataTextField = "NAME1";
            //_ddl_contact_person.DataValueField = "BOBJECTLINK";
            //_ddl_contact_person.DataSource = dt;
            //_ddl_contact_person.DataBind();

            updAddTierZero.Update();
        }
    }

}