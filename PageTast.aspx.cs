using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Service;
//using JobSchedulerManager.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class PageTast : System.Web.UI.Page
    {
        Customer_Training lib = new Customer_Training();
        DashboardLib libDashboard = new DashboardLib();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               //List<string> listData =  SearchHelpCIControl.listCICode;

                //ActivitySendMailModal.refAobjectlink = "fe904dba4f2146aca760c9171bb07fbb";
                //libDashboard.PreparFinanDataDashboard(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            }
        }

        protected void btnTestrecurring_Click(object sender, EventArgs e)
        {
            AppClientLibrary.GetInstance().GenerateCorperatePermissionKey(
                "555", "INET", "01", Validation.getCurrentServerStringDateTime(), ERPWAuthentication.EmployeeCode
            );
            //JobServiceTierZero.retrieveEmailAndSendTierZero("555", "INET", "RE00015"); 
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            AppClientLibrary.GetInstance().AcceptionRequestAppClientPermission(
                "555", "INET", txtSEQ.Text,
                ERPWAuthentication.EmployeeCode, Validation.getCurrentServerStringDateTime()
            );
            //JobServiceTierZero.retrieveEmailAndSendTierZero("555", "INET", "RE00015"); 
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AppClientLibrary.GetInstance().RejectRequestAppClientPermission(
                "555", "INET", txtSEQ.Text,
                ERPWAuthentication.EmployeeCode, Validation.getCurrentServerStringDateTime()
            );
            //JobServiceTierZero.retrieveEmailAndSendTierZero("555", "INET", "RE00015"); 
        }
        
        //private void iniData()
        //{
        //}

        //private void saveData () {
        //    Customer_Training.Customer_Model en = new Customer_Training.Customer_Model();
        //    en.CustomerName = txtText1.Text;
        //    en.ContactType = ddlType.SelectedItem.Text;
            
        //    lib.addDataCustomer_Training(
        //        ERPWAuthentication.SID,
        //        ERPWAuthentication.CompanyCode,
        //        en
        //    );
        //}

        //private void bindData()
        //{
        //    List<Customer_Training.Customer_Model> en = lib.getDataCustomer_Training(
        //        ERPWAuthentication.SID,
        //        ERPWAuthentication.CompanyCode,
        //        ""
        //    );

        //    rptCusDetail.DataSource = en;
        //    rptCusDetail.DataBind();
        //}

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        saveData();
        //        bindData();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientService.AGError(ex.Message);
        //    }
        //}

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bindData();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientService.AGError(ex.Message);
        //    }
        //}
    }
}