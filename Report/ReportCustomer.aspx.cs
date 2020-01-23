using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.Service.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Report
{
    public partial class ReportCustomer : System.Web.UI.Page
    {
        private string reportOn = "";
        private string[] monthTh = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม"
            , "เมษายน", "พฤษภาคม", "มิถุนายน"
            , "กรกฎาคม", "สิงหาคม", "กันยายน"
            , "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
        private ReportDAO reportdao = new ReportDAO();
        private ReportUnity reportunity = new ReportUnity();

        private ReportCustomerDetail reportCustomerDetail = new ReportCustomerDetail();

        public String CustomerCode
        {
            get
            {
                return AutoCompleteCustomer.SelectedValue.ToString();
            }
        }
        private CustomerService serviceCustomer = CustomerService.getInstance();
        private CustomerProfile _CustomerProfile = null;
        public CustomerProfile CustomerProfile
        {
            get
            {
                if (_CustomerProfile == null)
                {
                    _CustomerProfile = serviceCustomer.getUserProfile(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        CustomerCode
                        );

                    if (_CustomerProfile == null)
                    {
                        _CustomerProfile = new CustomerProfile();
                    }
                }
                return _CustomerProfile;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private DataTable formatData(DataTable dt)
        {
            dt = reportunity.incidentNoFormater(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, dt, "IncidentNO");

            for (int index = 0; index < dt.Rows.Count; index++)
            {
                dt.Rows[index]["Duration_Time"] = reportunity.ConvertToDurationTimeString(dt.Rows[index]["Open_Date"].ToString(), dt.Rows[index]["Resolved_Date"].ToString());
                dt.Rows[index]["Open_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(dt.Rows[index]["Open_Date"].ToString());
                dt.Rows[index]["Resolved_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(dt.Rows[index]["Resolved_Date"].ToString());
            }
            return dt;
        }
        private void bindDataTable(string customerCode,string month,string year)
        {
            DataTable dt = reportdao.getReportCustomer(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode,
                customerCode,
                month,
                year
                );
            dt = formatData(dt);

            //rtpReportCustomer.DataSource = dt;
            //rtpReportCustomer.DataBind();
            divDataJson.InnerHtml = JsonConvert.SerializeObject(dt);

            udpCustomer.Update();
            //ClientService.DoJavascript("$('#tableItems').DataTable();");
            ClientService.DoJavascript("afterSearch();");
        }

        private void bindCustomerDetail(string customerCode)
        {
            //DataTable dt = reportdao.getCCID(
            //    ERPWAuthentication.SID,
            //    ERPWAuthentication.CompanyCode,
            //    customerCode,
            //    "01"
            //    );
            //string eqp = "";
            //for(int index = 0; index < dt.Rows.Count; index++)
            //{
            //    if (index > 0)
            //    {
            //        eqp += " ,";
            //    }
            //    eqp += dt.Rows[index][0].ToString();
            //}

            //pack object
            reportCustomerDetail.customerName = CustomerProfile.CustomerName;
            reportCustomerDetail.customerAddress = CustomerProfile.Address;
            //reportCustomerDetail.customerEquipment = eqp;//result look code above
            reportCustomerDetail.customerCCID = "-";
            reportCustomerDetail.customerReportMonth = reportOn;

            //string message = "";
            //string dot_message = "";
            //if (eqp.Length > 20)
            //{
            //    for(int index = 0; index < eqp.Length; index++)
            //    {
            //        if (index == 20)
            //        {
            //            message += "<span id='dots'>...</span><span id='more'>";
            //        }
            //        message += eqp[index];
            //        if (index == eqp.Length - 1)
            //        {
            //            message += "</span><a onclick='myFunction()' id='myBtn'>Read more</a>";
            //        }
            //    }
            //}

            customerName_label.Text = reportCustomerDetail.customerName;
            address_label.Text = reportCustomerDetail.customerAddress;
            //service_type_label.Text = reportCustomerDetail.customerEquipment;
            service_type_label.Text = "-";
            cc_label.Text = reportCustomerDetail.customerCCID;
            month_label.Text = reportCustomerDetail.customerReportMonth;

            udpCustomerDetail.Update();
        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                string customerCode;// = AutoCompleteCustomer.SelectedValue;
                string month;// = inp_month.SelectedValue.ToString();
                string year;// = inp_year.Text.ToString();
                
                if(string.IsNullOrEmpty(AutoCompleteCustomer.SelectedValue))
                {
                    throw new Exception("กรุุณาระบุ Customer Name");
                }
                customerCode = AutoCompleteCustomer.SelectedValue;

                if(string.IsNullOrEmpty(inp_month.SelectedValue))
                {
                    throw new Exception("กรุุณาระบุ Month");
                }
                month = inp_month.SelectedValue.ToString();

                if(string.IsNullOrEmpty(inp_year.Text))
                {
                    throw new Exception("กรุุณาระบุ Year");
                }
                year = inp_year.Text.ToString();

                reportOn = monthTh[Int16.Parse(inp_month.SelectedValue.ToString()) - 1] + " " + (Int16.Parse(year) + 543).ToString();

                bindCustomerDetail(customerCode);
                bindDataTable(customerCode, month, year);
            }
            catch (Exception e1)
            {
                ClientService.AGError(e1.Message.ToString());
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        private void exportTable(string customerCode,string month,string year)
        {
            DataTable dt = reportdao.getCCID(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                customerCode,
                "01"
                );
            string eqp = "";
            for (int index = 0; index < dt.Rows.Count; index++)
            {
                if (index > 0)
                {
                    eqp += " ,";
                }
                eqp += dt.Rows[index][0].ToString();
            }
            //pack object
            reportCustomerDetail.customerName = CustomerProfile.CustomerName;
            reportCustomerDetail.customerAddress = CustomerProfile.Address;
            //reportCustomerDetail.customerEquipment = eqp;//result look code above
            reportCustomerDetail.customerCCID = "-";
            reportCustomerDetail.customerReportMonth = reportOn;
            //System.Diagnostics.Debug.WriteLine("report on: "+reportOn);
            dt = reportdao.getReportCustomerV2(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                customerCode,
                month,
                year
                );
            dt = formatData(dt);

            Session["report.excel.Report_Excel_Export_Datatable"] = dt;
            Session["report.excel.Report_Excel_Export_CustomerProfile"] = reportCustomerDetail;
            Session["report.excel.Report_Excel_Export_Name"] = "ReportCustomer";
            ClientService.DoJavascript("exportExcelAPI();");
        }

        protected void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                string customerCode;// = AutoCompleteCustomer.SelectedValue;
                string month;// = inp_month.SelectedValue.ToString();
                string year;// = inp_year.Text.ToString();

                if (string.IsNullOrEmpty(AutoCompleteCustomer.SelectedValue))
                {
                    throw new Exception("กรุุณาระบุ Customer Name");
                }
                customerCode = AutoCompleteCustomer.SelectedValue;

                if (string.IsNullOrEmpty(inp_month.SelectedValue))
                {
                    throw new Exception("กรุุณาระบุ Month");
                }
                month = inp_month.SelectedValue.ToString();

                if (string.IsNullOrEmpty(inp_year.Text))
                {
                    throw new Exception("กรุุณาระบุ Year");
                }
                year = inp_year.Text.ToString();

                //reportOn = monthTh[Int16.Parse(inp_month.SelectedValue.ToString()) - 1] + " " + (Int16.Parse(year) + 543).ToString();
                exportTable(customerCode,month,year);
            }
            catch (Exception e1)
            {
                ClientService.AGError(e1.Message.ToString());
                //throw;
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
    }
}