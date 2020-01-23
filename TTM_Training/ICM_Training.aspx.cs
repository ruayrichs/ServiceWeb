using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Service;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.TTM_Training
{
    public partial class ICM_Training : System.Web.UI.Page
    {
        private ServiceTicketLibrary libService = new ServiceTicketLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initData();
            }
        }

        #region Call Function ERP
        // การ Call Function จะมี 4 ขั้นตอน
        // 1. Using Lib 'ICMUtils' // using ERPW.Lib.F1WebService.ICMUtils;
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

        public void CallFunction_ERP(string doctype, string docnumber, string fiscalyear)
        {

            // 2. เตรียม Paramiter ที่จำเป็นต้องใช้ในการ Call Function
            Object[] objParam = new Object[] { 
                "1500117", // Number FOBGP เป็นรหัสที่ใช้ในการเรียก Function ในระบบ ERP (รหัส FOBGP จะเปลียนไปตามแต่ละ Function)
                (string)Session[ApplicationSession.USER_SESSION_ID], // Paramiter ที่ต้องส่งไปตามแต่ Function กำหนด (ขึ้นอยู่กับเลข FOBGP กำหนด)
                ERPWAuthentication.CompanyCode, // Paramiter ที่ต้องส่งไปตามแต่ Function กำหนด (ขึ้นอยู่กับรหัส FOBGP กำหนด)
                doctype,    // Paramiter ที่ต้องส่งไปตามแต่ Function กำหนด (ขึ้นอยู่กับเลข FOBGP กำหนด)
                docnumber,  // Paramiter ที่ต้องส่งไปตามแต่ Function กำหนด (ขึ้นอยู่กับเลข FOBGP กำหนด)
                fiscalyear  // Paramiter ที่ต้องส่งไปตามแต่ Function กำหนด (ขึ้นอยู่กับเลข FOBGP กำหนด)
            };
            DataSet[] objDataSet = new DataSet[] { new tmpServiceCallDataSet() }; // ประเภท Data Type ที่ใช้รับค่าที่ Return กลับมา (จะเป็น Data Model, Data set, Data Table หรืออื่นๆ)


            // 3. เรียกใช้ Function ICMService.ICMDataSetInvoke จะรับตัวแปร 2 ตัว
            DataSet objReturn = ICMService.ICMDataSetInvoke(
                objParam, // 1. Paramiter ที่ต้องการทั้งหมดของ Function ฝั่งระบบ ERP
                objDataSet // 2. Data Type ที่ใช้ Return ค่ากลับมา
            ); // ICMService.ICMDataSetInvoke เป็น Framework ที่ใช้ Call Function ผ่าน Socket
               
                
            // 4. เช็คค่าที่ได้กลับมาก แล้วนำไปใช้ได้เลย
            if (objReturn != null)
            {
                tmpServiceCallDataSet serviceTempCallEntity = new tmpServiceCallDataSet(); // สร้าง Paramiter ที่ใช้รับ
                serviceTempCallEntity.Merge(objReturn.Copy()); // แล้วเอาค่าไปเก็บพร้อมใช้งาน

                // ตัวอย่าง tmpServiceCallDataSet Model จะเป็น Data Set 
                // ภายใน tmpServiceCallDataSet จะมี Data Table ตัวอย่างเช่น....
                // 1. serviceTempCallEntity.cs_servicecall_header; 
                // 2. serviceTempCallEntity.cs_servicecall_item;
                // 3. serviceTempCallEntity.cs_service_call_activity;

                if (serviceTempCallEntity.cs_servicecall_header.Rows.Count > 0)
                {
                    DataRow drHeader = serviceTempCallEntity.cs_servicecall_header.Rows[0];
                    txtSubject.Text = drHeader["HeaderText"].ToString();
                    txtTicketDate.Text = Validation.Convert2DateDisplay(drHeader["DOCDATE"].ToString());
                    txtCreatedBy.Text = drHeader["CREATED_BY"].ToString();
                    txtCreatedOn.Text = Validation.Convert2DateTimeDisplay(drHeader["CREATED_ON"].ToString());
                }
                rptEquipment.DataSource = serviceTempCallEntity.cs_servicecall_item;
                rptEquipment.DataBind();

                udpResultDetail.Update();
            }
        }
        #endregion
        private void initData()
        {
            txtFiscalYear.Text = DateTime.Now.Year.ToString();

            DataTable dtSCType = AfterSaleService.getInstance().getSearchDoctype("", ERPWAuthentication.CompanyCode);
            ddlDocType.DataTextField = "Description";
            ddlDocType.DataValueField = "DocumentTypeCode";
            ddlDocType.DataSource = dtSCType;
            ddlDocType.DataBind();
            ddlDocType.Items.Insert(0, new ListItem("เลือก", ""));
            udpPanelSearch.Update();
        }

        private void initDataDocNumber()
        {
            ddlListDocNumber.DataTextField = "CallerID";
            ddlListDocNumber.DataValueField = "CallerID";

            if (string.IsNullOrEmpty(ddlDocType.SelectedValue))
            {
                ddlListDocNumber.DataSource = new DataTable();
                ddlListDocNumber.DataBind();
            }
            else
            {
                //DataTable dtDataSearch = libService.GetTicketList(
                //    ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ddlDocType.SelectedValue,
                //    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", false, false,"",""
                //);
                //ddlListDocNumber.DataSource = dtDataSearch;
                //ddlListDocNumber.DataBind();
                //ddlListDocNumber.Items.Insert(0, new ListItem("เลือก", ""));
            }

            udpPanelSearch.Update();
        }

        protected void ddlDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                initDataDocNumber();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlDocType.SelectedValue))
                {
                    throw new Exception("ระบุ Doc Type");
                }
                if (string.IsNullOrEmpty(ddlListDocNumber.SelectedValue))
                {
                    throw new Exception("ระบุ Doc Number");
                }
                if (string.IsNullOrEmpty(txtFiscalYear.Text))
                {
                    throw new Exception("ระบุ Fiscal Year");
                }
                CallFunction_ERP(ddlDocType.SelectedValue, ddlListDocNumber.SelectedValue, txtFiscalYear.Text);
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
    }
}