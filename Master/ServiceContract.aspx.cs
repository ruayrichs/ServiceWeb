using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Master;
using ERPW.Lib.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.auth;

namespace ServiceWeb.Master
{
    public partial class ServiceContract : AbstractsSANWebpage
    {
        private ServiceContractLibrary lib = new ServiceContractLibrary();
        private CustomerService libCustomer = new CustomerService();

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

        public string CustomerCode
        {
            get { return mainBean.master_customer_service_contract.Rows[0]["CustomerCode"].ToString(); }
        }

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
                }
            }
            catch(Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindingScreen()
        {
            tbCompany.Text = CompanyCode + " : " + ERPWAuthentication.CompanyName;

            #region Header
            foreach (DataRow dr in mainBean.master_customer_service_contract.Rows)
            {
                tbCustomer.Text = dr["CustomerCode"] + " : " + libCustomer.GetCustomerNameAndForeignName(SID, CompanyCode, dr["CustomerCode"].ToString());
                tbDocumentType.Text = dr["DocumentType"] + " : " + lib.GetDocumentTypeDesc(SID, CompanyCode, dr["DocumentType"].ToString());
                tbFiscalYear.Text = dr["FiscalYear"].ToString();
                tbDocumentNo.Text = dr["ContractNo"].ToString();
                tbStatus.Text = dr["DocStatus"] + " : " + lib.GetDocumentStatusDesc(SID, dr["DocStatus"].ToString());

                hdfContactCode.Value = dr["ContractPerson"].ToString();
                tbContactPerson.Text = libCustomer.GetContactName(SID, dr["ContractPerson"].ToString());                

                tbDocumentDate.Text = Validation.Convert2DateDisplay(dr["DocDate"].ToString());
                tbStartDate.Text = Validation.Convert2DateDisplay(dr["StartDate"].ToString());
                tbEndDate.Text = Validation.Convert2DateDisplay(dr["EndDate"].ToString());
                tbDescription.Text = dr["Description"].ToString();
            }
            #endregion

            #region General
            foreach (DataRow dr in mainBean.master_customer_service_contract_general.Rows)
            {
                string ownerName = "", templateName = "";

                if (!string.IsNullOrEmpty(dr["Owner"].ToString()))
                {
                    ownerName = EmployeeService.GetInstance().GetEmployeeName(SID, CompanyCode, dr["Owner"].ToString());
                    ownerName = ownerName == "" ? dr["Owner"].ToString() : (dr["Owner"] + " : " + ownerName);
                }

                if (!string.IsNullOrEmpty(dr["Template"].ToString()))
                {
                    templateName = lib.GetContractTemplateName(SID, CompanyCode, dr["Template"].ToString());                    
                }

                hdfOwnerCode.Value = dr["Owner"].ToString();
                tbOwner.Text = ownerName;

                hdfTemplateCode.Value = dr["Template"].ToString();
                tbTemplate.Text = templateName;

                tbResponseTime.Text = dr["ResponseTime"].ToString();
                tbResolutionTime.Text = dr["ResolutionTime"].ToString();
                tbRemark.Text = dr["Remark"].ToString();
            }
            #endregion

            #region Coverage
            foreach (DataRow dr in mainBean.master_customer_service_contract_converange.Rows)
            {
                chkMon.Checked = Convert.ToBoolean(dr["Monday"]);
                chkTue.Checked = Convert.ToBoolean(dr["Tuesday"]);
                chkWed.Checked = Convert.ToBoolean(dr["Wenesday"]);
                chkThu.Checked = Convert.ToBoolean(dr["Thursday"]);
                chkFri.Checked = Convert.ToBoolean(dr["Friday"]);
                chkSat.Checked = Convert.ToBoolean(dr["Saturday"]);
                chkSun.Checked = Convert.ToBoolean(dr["Sunday"]);

                tbMonStartTime.Text = Validation.Convert2TimeDisplay(dr["MondayStartTime"].ToString()).Substring(0, 5);
                tbMonEndTime.Text = Validation.Convert2TimeDisplay(dr["MondayEndTime"].ToString()).Substring(0, 5);
                tbTueStartTime.Text = Validation.Convert2TimeDisplay(dr["TuesdayStartTime"].ToString()).Substring(0, 5);
                tbTueEndTime.Text = Validation.Convert2TimeDisplay(dr["TuesdayEndTime"].ToString()).Substring(0, 5);
                tbWedStartTime.Text = Validation.Convert2TimeDisplay(dr["WenesdayStartTime"].ToString()).Substring(0, 5);
                tbWedEndTime.Text = Validation.Convert2TimeDisplay(dr["WenesdayEndTime"].ToString()).Substring(0, 5);
                tbThuStartTime.Text = Validation.Convert2TimeDisplay(dr["ThursdayStartTime"].ToString()).Substring(0, 5);
                tbThuEndTime.Text = Validation.Convert2TimeDisplay(dr["ThursdayEndTime"].ToString()).Substring(0, 5);
                tbFriStartTime.Text = Validation.Convert2TimeDisplay(dr["FridayStartTime"].ToString()).Substring(0, 5);
                tbFriEndTime.Text = Validation.Convert2TimeDisplay(dr["FridayEndTime"].ToString()).Substring(0, 5);
                tbSatStartTime.Text = Validation.Convert2TimeDisplay(dr["SaturdayStartTime"].ToString()).Substring(0, 5);
                tbSatEndTime.Text = Validation.Convert2TimeDisplay(dr["SaturdayEndTime"].ToString()).Substring(0, 5);
                tbSunStartTime.Text = Validation.Convert2TimeDisplay(dr["SundayStartTime"].ToString()).Substring(0, 5);
                tbSunEndTime.Text = Validation.Convert2TimeDisplay(dr["SundayEndTime"].ToString()).Substring(0, 5);

                chkParts.Checked = Convert.ToBoolean(dr["Parts"]);
                chkLabor.Checked = Convert.ToBoolean(dr["Labour"]);
                chkTravel.Checked = Convert.ToBoolean(dr["Travel"]);
                chkHolidays.Checked = Convert.ToBoolean(dr["IncludingHolidays"]);
            }
            #endregion            

            BindingItems();
        }

        private void BindingItems()
        {
            rptItems.DataSource = mainBean.master_customer_service_contract_item;
            rptItems.DataBind();

            udpnItems.Update();            
        }

        private void ControlScreen()
        {
            tbDocumentDate.Enabled = false;
            tbStartDate.Enabled = false;
            tbEndDate.Enabled = false;
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {                
                string startDate = "";
                string endDate = "";

                if (tbStartDate.Text.Trim() != "" && tbStartDate.Text.Length == 10)
                {
                    startDate = Validation.Convert2DateDB(tbStartDate.Text);
                }
                else
                {
                    startDate = Validation.Convert2DateDB(Validation.getCurrentServerDate());
                }

                if (tbEndDate.Text.Trim() != "" && tbEndDate.Text.Length == 10)
                {
                    endDate = Validation.Convert2DateDB(tbEndDate.Text);
                }
                else
                {
                    endDate = Validation.Convert2DateDB(Validation.getCurrentServerDate());
                }

                DataRow dr = mainBean.master_customer_service_contract_item.NewRow();
                dr["EquipmentNo"] = hdfEquipmentNo.Value.Trim();
                dr["EquipmentDesc"] = hdfEquipmentDesc.Value.Trim();
                dr["StartDate"] = startDate;
                dr["EndDate"] = endDate;
                mainBean.master_customer_service_contract_item.Rows.Add(dr);

                dr = mainBean.MASTER_CS_CONTRACT_DATE.NewRow();
                dr["EquipmentNo"] = hdfEquipmentNo.Value.Trim();
                dr["xLineno"] = "01";
                dr["Clinestatus"] = "0001";
                dr["Xdate"] = startDate;
                dr["Ydate"] = endDate;
                mainBean.MASTER_CS_CONTRACT_DATE.Rows.Add(dr);

                hdfEquipmentNo.Value = "";
                hdfEquipmentDesc.Value = "";

                BindingItems();
            }
            catch(Exception ex)
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
            string startDate = Validation.Convert2DateDB(tbStartDate.Text);
            string endDate = Validation.Convert2DateDB(tbEndDate.Text);

            #region Header
            foreach (DataRow dr in mainBean.master_customer_service_contract.Rows)
            {
                dr["ContractPerson"] = hdfContactCode.Value;
                dr["PhoneNo"] = "";
                dr["DocDate"] = Validation.Convert2DateDB(tbDocumentDate.Text);
                dr["StartDate"] = startDate;
                dr["EndDate"] = endDate;
                dr["Description"] = tbDescription.Text.Trim();
            }
            #endregion

            #region Amortize
            foreach (DataRow dr in mainBean.master_customer_service_contract_Amortize_Header.Rows)
            {
                dr["StartDate"] = startDate;
                dr["EndDate"] = endDate;
            }
            #endregion

            #region General
            foreach (DataRow dr in mainBean.master_customer_service_contract_general.Rows)
            {
                dr["Owner"] = hdfOwnerCode.Value;
                dr["Template"] = hdfTemplateCode.Value;
                dr["ResponseTime"] = tbResponseTime.Text.Trim() == "" ? 0 : Convert.ToInt32(tbResponseTime.Text.Trim());
                dr["ResponseTimeUnit"] = ServiceContractLibrary.TIME_UNIT_DAYS;
                dr["ResolutionTime"] = tbResolutionTime.Text.Trim() == "" ? 0 : Convert.ToInt32(tbResolutionTime.Text.Trim());
                dr["ResolutionTimeUnit"] = ServiceContractLibrary.TIME_UNIT_DAYS;
                dr["Remark"] = tbRemark.Text.Trim();
            }
            #endregion

            #region Coverage
            foreach (DataRow dr in mainBean.master_customer_service_contract_converange.Rows)
            {
                dr["Monday"] = chkMon.Checked.ToString();
                dr["Tuesday"] = chkTue.Checked.ToString();
                dr["Wenesday"] = chkWed.Checked.ToString();
                dr["Thursday"] = chkThu.Checked.ToString();
                dr["Friday"] = chkFri.Checked.ToString();
                dr["Saturday"] = chkSat.Checked.ToString();
                dr["Sunday"] = chkSun.Checked.ToString();
                dr["MondayStartTime"] = GetTimePickerValue(tbMonStartTime);
                dr["MondayEndTime"] = GetTimePickerValue(tbMonEndTime);
                dr["TuesdayStartTime"] = GetTimePickerValue(tbTueStartTime);
                dr["TuesdayEndTime"] = GetTimePickerValue(tbTueEndTime);
                dr["WenesdayStartTime"] = GetTimePickerValue(tbWedStartTime);
                dr["WenesdayEndTime"] = GetTimePickerValue(tbWedEndTime);
                dr["ThursdayStartTime"] = GetTimePickerValue(tbThuStartTime);
                dr["ThursdayEndTime"] = GetTimePickerValue(tbThuEndTime);
                dr["FridayStartTime"] = GetTimePickerValue(tbFriStartTime);
                dr["FridayEndTime"] = GetTimePickerValue(tbFriEndTime);
                dr["SaturdayStartTime"] = GetTimePickerValue(tbSatStartTime);
                dr["SaturdayEndTime"] = GetTimePickerValue(tbSatEndTime);
                dr["SundayStartTime"] = GetTimePickerValue(tbSunStartTime);
                dr["SundayEndTime"] = GetTimePickerValue(tbSunEndTime);
                dr["Parts"] = chkParts.Checked.ToString();
                dr["Labour"] = chkLabor.Checked.ToString();
                dr["Travel"] = chkTravel.Checked.ToString();
                dr["IncludingHolidays"] = chkHolidays.Checked.ToString();
            }
            #endregion
        }

        private string GetTimePickerValue(TextBox tb)
        {
            if (tb.Text.Trim() != "" && tb.Text.Trim().Length == 5)
            {
                return Validation.Convert2TimeDB(tb.Text.Trim() + ":00");
            }

            return "000000";
        }

        protected void btnValidateSave_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.ID)
            {
                case "btnValidateSave":                    
                    ClientService.DoJavascript("$('#btnSaveClient').click();");
                    break;
                case "btnValidateAddItem":
                    ClientService.DoJavascript("addNewItem();");
                    break;
            }            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ServiceContractDataSet bakupBean = ObjectUtil.Copy<ServiceContractDataSet>(mainBean);
            try
            {
                PrepareSaveBean();

                if (mode.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    mainBean = lib.CreateServiceContract(SessionID, mainBean);
                    mode = ApplicationSession.CHANGE_MODE_STRING;

                    ClientService.AGSuccessRedirectCurrentPage("Created Service Contract " + mainBean.master_customer_service_contract.Rows[0]["ContractNo"] + " Success.");
                }
                else if (mode.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    mainBean = lib.ChangeServiceContract(SessionID, mainBean, ERPWAuthentication.UserName);

                    ClientService.AGSuccessRedirectCurrentPage("Updated Service Contract " + mainBean.master_customer_service_contract.Rows[0]["ContractNo"] + " Success.");
                }
            }
            catch (Exception ex)
            {
                mainBean = bakupBean;
                ClientService.AGError(ex.Message);
            }            
            finally
            {
                ClientService.AGLoading(false);
            }
        }       
    }
}