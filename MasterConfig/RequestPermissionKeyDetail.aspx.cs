using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class PageDetail : AbstractsSANWebpage
    {
        private ERPW_API_Permission_Token_Key pkey_model = new ERPW_API_Permission_Token_Key();
        private ERPW_API_Permission_Token_Key_DAO pkey_dao = new ERPW_API_Permission_Token_Key_DAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["key"]))
                    {
                        //key not null
                        if (Request.QueryString["edit"].Equals("true"))
                        {
                            string pkey = Request.QueryString["key"];
                            string edit_mode = Request.QueryString["edit"];
                            //System.Diagnostics.Debug.WriteLine("Enable edit mode on key: "+ Request.QueryString["key"]+" !!!");
                            setDefault(pkey, edit_mode);
                        }
                    }
                    else
                    {
                        //key null
                        //System.Diagnostics.Debug.WriteLine("you runing create mode !!!");
                        setDefault();
                    }
                }
                catch (Exception e1)
                {
                    System.Diagnostics.Debug.WriteLine("Error(INVALID KEY OR INVALID MODE)" + e1);
                    lockInput();
                }

            }
        }

        private string generatePermissionKey(int length)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }
        private void checkNullSpaceIPAddress()
        {
            if (string.IsNullOrEmpty(ip_addr_inp1.Text))
            {
                ip_addr_inp1.Text = "0";
            }
            if (string.IsNullOrEmpty(ip_addr_inp2.Text))
            {
                ip_addr_inp2.Text = "0";
            }
            if (string.IsNullOrEmpty(ip_addr_inp3.Text))
            {
                ip_addr_inp3.Text = "0";
            }
            if (string.IsNullOrEmpty(ip_addr_inp4.Text))
            {
                ip_addr_inp4.Text = "0";
            }
        }
        protected void IPAddress_OnChange(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(status_inp.Enabled.ToString());
            string status;
            //string select_value;
            checkNullSpaceIPAddress();
            string[] newIPAddr = new string[4];
            newIPAddr[0] = ip_addr_inp1.Text;
            newIPAddr[1] = ip_addr_inp2.Text;
            newIPAddr[2] = ip_addr_inp3.Text;
            newIPAddr[3] = ip_addr_inp4.Text;
            int ip_status = pkey_dao.checkActiveByIP(ipFormater2DB(newIPAddr));
            // ค่าเป็น 0 เมื่อไม่มีIPตัวเดียวกัน Active
            // ค่าเป็น 1 เมื่อมีตัวIPเดียวกัน Active
            System.Diagnostics.Debug.WriteLine("IP STATUS: " + ip_status);
            // ค่าเป็น 0 เมื่อไม่มีIPตัวเดียวกัน Active
            if (ip_status == 0)
            {
                try
                {
                    status = status_inp.Enabled.ToString();
                    // ถ้า DLL Active ปิดอยู่
                    if (status.Equals("False"))
                    {
                        //ให้เปิด
                        status_inp.Enabled = true;
                        //select_value = status_inp.SelectedValue;
                        //System.Diagnostics.Debug.WriteLine("DDL SELECTED VALUE: " + select_value);

                        //if (select_value.Equals("false"))
                        //{
                        //    status_inp.ClearSelection();
                        //    status_inp.Items.FindByValue("true").Selected = true;
                        //}
                    }
                }
                catch (Exception e1) { }

            }
            // ค่าเป็น 1 เมื่อมีตัวIPเดียวกัน Active
            if (ip_status == 1)
            {
                try
                {
                    // รับสถานะของ DLL ว่าเปิดการใช้งานอยู่หรือไม่
                    status = status_inp.Enabled.ToString();
                    System.Diagnostics.Debug.WriteLine("DDL is SELECT: " + status);
                    // ถ้า DLL Active เปิดอยู่
                    if (status_inp.Enabled == true)
                    {
                        // ให้ปิด
                        status_inp.Enabled = false;
                        // Clear DDL
                        status_inp.ClearSelection();
                        // SET ค่า DDL เป็น false
                        status_inp.Items.FindByValue("false").Selected = true;
                        //select_value = status_inp.SelectedValue;
                        //System.Diagnostics.Debug.WriteLine("DDL SELECTED VALUE: " + select_value);
                        //if (select_value.Equals("true"))
                        //{
                        //    status_inp.Items.FindByValue("false").Selected = true;  
                        //}
                    }

                }
                catch (Exception e1) { }
            }
        }
        /// <summary>
        /// ถ้า StartDate มากกว่า EndDate ให้บันทึกไม่ได้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void checkEndDate(object sender, EventArgs e)
        {
            try
            {
                DateTime start = DateTime.ParseExact(start_date_inp.Text, "dd/M/yyyy", CultureInfo.InvariantCulture);
                DateTime end = DateTime.ParseExact(end_date_inp.Text, "dd/M/yyyy", CultureInfo.InvariantCulture);
                //System.Diagnostics.Debug.WriteLine("start date: "+start.ToString());
                //System.Diagnostics.Debug.WriteLine("end date: " + end.ToString());
                //ถ้าปุ่มเปิดให้ ปิด
                if (start > end)
                {
                    //ถ้าปุ่มเปิดให้ ปิด
                    if (btn_save.Enabled == true)
                    {
                        btn_save.Enabled = false;
                    }
                }
                //ถ้าปุ่มปิดให้ เปิด
                if (start < end)
                {
                    //ถ้าปุ่มปิดให้ เปิด
                    if (btn_save.Enabled == false)
                    {
                        btn_save.Enabled = true;
                    }
                }
            }
            catch (Exception e1) { }
        }
        private string[] ipFormater2UI(string ip_addr)
        {
            string[] ip = ip_addr.Split('.');
            return ip;
        }
        private string ipFormater2DB(string[] ip_addr)
        {
            try
            {
                return ip_addr[0] + "." + ip_addr[1] + "." + ip_addr[2] + "." + ip_addr[3];
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error(INVALID IP)");
                return "";
            }
        }
        private void setDefault()
        {
            mode_inp.Value = "create";
            pkey_inp.Text = generatePermissionKey(30);
            channel_request_inp.ClearSelection();
            channel_request_inp.Items.FindByValue("2").Selected = true;
            status_inp.ClearSelection();
            status_inp.Items.FindByValue("true").Selected = true;
        }
        private string dateFormater2UI(string date)
        {
            date = Validation.Convert2DateDisplay(date).ToString();
            return date;
        }
        private void setDefault(string key, string edit)
        {
            mode_inp.Value = "update";
            //ip_addr_inp.Enabled = false;
            ip_addr_inp1.Enabled = false;
            ip_addr_inp2.Enabled = false;
            ip_addr_inp3.Enabled = false;
            ip_addr_inp4.Enabled = false;

            program_name_inp.Enabled = false;

            start_date_inp.Enabled = false;
            end_date_inp.Enabled = false;

            channel_request_inp.Enabled = false;
            DataTable dt = pkey_dao.getMasterPermissionKey(SID, CompanyCode, key);
            try
            {
                int active_status = pkey_dao.checkActiveByIP((string)dt.Rows[0]["IPAddress"]);

                //ถ้า IP Active เปลี่ยน STATUS ไม่ได้
                if (active_status == 1)
                {
                    status_inp.Enabled = false;

                }
                System.Diagnostics.Debug.WriteLine("Active: " + dt.Rows[0]["Active"].ToString());
                if (dt.Rows[0]["Active"].Equals("true"))
                {
                    status_inp.Enabled = true;
                }
                string[] ip_addr = new string[4];
                ip_addr = ipFormater2UI((string)dt.Rows[0]["IPAddress"]);
                //ip_addr_inp.Text = (string)dt.Rows[0]["IPAddress"];
                ip_addr_inp1.Text = ip_addr[0];
                ip_addr_inp2.Text = ip_addr[1];
                ip_addr_inp3.Text = ip_addr[2];
                ip_addr_inp4.Text = ip_addr[3];
                program_name_inp.Text = (string)dt.Rows[0]["ProgramName"];
                pkey_inp.Text = (string)dt.Rows[0]["PermissionKey"];
                start_date_inp.Text = dateFormater2UI((string)dt.Rows[0]["StartDate"]);
                end_date_inp.Text = dateFormater2UI((string)dt.Rows[0]["EndDate"]);
                channel_request_inp.ClearSelection();
                channel_request_inp.Items.FindByValue((string)dt.Rows[0]["ChanelRequest"]).Selected = true;
                status_inp.ClearSelection();
                status_inp.Items.FindByValue((string)dt.Rows[0]["Active"]).Selected = true;
                AutoCompleteEmployee.SelectedValue = (string)dt.Rows[0]["EmployeeCode"];
                remark_inp.Text = (string)dt.Rows[0]["Remark"];
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("Error(Active Status is NULL)" + e1);
                lockInput();
            }
        }
        /// <summary>
        /// เตรียมข้อมูลก่อน Create/Update
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private ERPW_API_Permission_Token_Key setModel(ERPW_API_Permission_Token_Key model, string mode)
        {
            string[] newIPAddr = new string[4];
            newIPAddr[0] = ip_addr_inp1.Text;
            newIPAddr[1] = ip_addr_inp2.Text;
            newIPAddr[2] = ip_addr_inp3.Text;
            newIPAddr[3] = ip_addr_inp4.Text;
            pkey_model.SID = ERPWAuthentication.SID;
            pkey_model.CompanyCode = ERPWAuthentication.CompanyCode;
            pkey_model.IPAddress = ipFormater2DB(newIPAddr);
            pkey_model.PermissionKey = pkey_inp.Text;
            pkey_model.StartDate = Validation.Convert2DateDB(start_date_inp.Text);
            pkey_model.EndDate = Validation.Convert2DateDB(end_date_inp.Text);
            pkey_model.ProgramName = program_name_inp.Text;
            pkey_model.ChannelRequest = channel_request_inp.SelectedValue;
            pkey_model.Active = status_inp.SelectedValue;
            pkey_model.EmployeeCode = AutoCompleteEmployee.SelectedValue;
            pkey_model.Remark = remark_inp.Text;

            string by = ERPWAuthentication.EmployeeCode;
            string on = Validation.Convert2DateDB(DateTime.Today.ToString("dd/M/yyyy")) + Validation.Convert2TimeDB(DateTime.Now.ToString("HH:mm:ss"));
            //System.Diagnostics.Debug.WriteLine("on: " + on);

            if (mode.Equals("create"))
            {
                pkey_model.Created_By = by;
                pkey_model.Created_On = on;
                //System.Diagnostics.Debug.WriteLine("created by: "+pkey_model.Created_By);
                //System.Diagnostics.Debug.WriteLine("created on: "+pkey_model.Created_On);
            }
            if (mode.Equals("update"))
            {
                pkey_model.Updated_By = by;
                pkey_model.Updated_On = on;
                //System.Diagnostics.Debug.WriteLine("updated by: " + pkey_model.Updated_By);
                //System.Diagnostics.Debug.WriteLine("updated on: " + pkey_model.Updated_On);
            }
            return model;
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(AutoCompleteEmployee.SelectedValue))
                {
                    throw new Exception("กรุุณาระบุ Employee Name");
                }
                if (string.IsNullOrEmpty(ip_addr_inp1.Text)
                    || string.IsNullOrEmpty(ip_addr_inp2.Text)
                    || string.IsNullOrEmpty(ip_addr_inp3.Text)
                    || string.IsNullOrEmpty(ip_addr_inp4.Text))
                {
                    throw new Exception("กรุุณาระบุ IP Address ให้ถูกต้อง");
                }
                if (string.IsNullOrEmpty(program_name_inp.Text))
                {
                    throw new Exception("กรุุณาระบุ Program Name");
                }
                if (string.IsNullOrEmpty(start_date_inp.Text))
                {
                    throw new Exception("กรุณาระบุ Start Date");
                }
                if (string.IsNullOrEmpty(end_date_inp.Text))
                {
                    throw new Exception("กรุณาระบุ End Date");
                }
                if (
                    Convert.ToInt32(Validation.Convert2DateDB(start_date_inp.Text)) >
                    Convert.ToInt32(Validation.Convert2DateDB(end_date_inp.Text))
                    )
                {
                    throw new Exception("กรุณาระบุ End Date มากกว่า Start Date");
                }

                string mode = mode_inp.Value;
                System.Diagnostics.Debug.WriteLine("show mode: " + mode);
                pkey_model = setModel(pkey_model, mode);
                //System.Diagnostics.Debug.WriteLine("sid: "+pkey_model.SID);
                //System.Diagnostics.Debug.WriteLine("code: "+pkey_model.CompanyCode);
                //System.Diagnostics.Debug.WriteLine("ip: " + pkey_model.IPAddress);
                //System.Diagnostics.Debug.WriteLine("name: " + pkey_model.ProgramName);
                //System.Diagnostics.Debug.WriteLine("pkey: " + pkey_model.PermissionKey);
                //System.Diagnostics.Debug.WriteLine("start: " + pkey_model.StartDate);
                //System.Diagnostics.Debug.WriteLine("end: " + pkey_model.EndDate);
                //System.Diagnostics.Debug.WriteLine("channel: " + pkey_model.ChannelRequest);
                //System.Diagnostics.Debug.WriteLine("active: " + pkey_model.Active);
                //System.Diagnostics.Debug.WriteLine("con: " + pkey_model.Created_On);
                //System.Diagnostics.Debug.WriteLine("cby: " + pkey_model.Created_By);
                //System.Diagnostics.Debug.WriteLine("remark: " + pkey_model.Remark);
                //System.Diagnostics.Debug.WriteLine("uon: " + pkey_model.Updated_On);
                //System.Diagnostics.Debug.WriteLine("uby: " + pkey_model.Updated_By);
                int exc_status;

                if (mode.Equals("create"))
                {
                    exc_status = pkey_dao.addRow(pkey_model);
                    ClientService.AGSuccessRedirect("Success", "/MasterConfig/RequestPermissionKeyDetail.aspx?key=" + pkey_inp.Text + "&edit=true");
                }
                if (mode.Equals("update"))
                {
                    exc_status = pkey_dao.updateRows(pkey_model);
                    ClientService.AGSuccess("Success");
                    // Response.-Redirect("~/MasterConfig/RequestPermissionKeyDetail.aspx?key=" + pkey_inp.Text + "&edit=true");

                }

            }
            catch (Exception e1)
            {
                //System.Diagnostics.Debug.WriteLine("Error(ERPW_API_Permission_Token_Key_DAO): "+e1);
                ClientService.AGError(e1.Message.ToString());
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
        private void lockInput()
        {
            ip_addr_inp1.Enabled = false;
            ip_addr_inp2.Enabled = false;
            ip_addr_inp3.Enabled = false;
            ip_addr_inp4.Enabled = false;
            //ip_addr_inp.Enabled = false;
            program_name_inp.Enabled = false;
            pkey_inp.Enabled = false;
            start_date_inp.Enabled = false;
            end_date_inp.Enabled = false;
            channel_request_inp.Enabled = false;
            status_inp.Enabled = false;
            remark_inp.Enabled = false;
            btn_save.Enabled = false;
        }
    }
}