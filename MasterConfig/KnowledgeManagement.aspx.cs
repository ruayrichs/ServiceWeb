using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class KnowledgeManagement : System.Web.UI.Page
    {
        private KMGroupLib kMGroupLib = new KMGroupLib();
        private OwnerGroupLib ownerGroupLib = new OwnerGroupLib();
        private string _SID;
        private string _CompanyCode;
        private string _EmployeeCode;
        private string _FullNameEn;

        private DateTime cdt = DateTime.Now;
        private string _year;
        private string _month;
        private string _day;
        private string _hour;
        private string _minute;
        private string _seconds;

        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                {
                    _SID = ERPWAuthentication.SID;
                }
                return _SID;
            }
        }
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                {
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                }
                return _CompanyCode;
            }
        }
        private string EmployeeCode
        {
            get
            {

                if (string.IsNullOrEmpty(_EmployeeCode))
                {
                    _EmployeeCode = ERPWAuthentication.EmployeeCode;
                }
                return _EmployeeCode;
            }
        }
        private string FullNameEn
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameEn))
                {
                    _FullNameEn = ERPWAuthentication.FullNameEN;
                }
                return _FullNameEn;
            }
        }

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
                init();
            }
        }
        private void init()
        {
            List<KMGroupEntity> rawData = new List<KMGroupEntity>();
            rawData = kMGroupLib.readByMessageGroupCode("");
            rptItem.DataSource = rawData;
            rptItem.DataBind();
            udpnItems.Update();

            getOwnerGroup();
            ClientService.DoJavascript("afterLoad();");            
        }

        private bool getOwnerGroup()
        {
            List<OwnerGroupEN> ownerGroupENs = new List<OwnerGroupEN>();
            ownerGroupENs = ownerGroupLib.readByCode(SID, CompanyCode, "");

            DataTable dt = ownerGroupENs.toDataTable();
            ownerGroupCode_input.DataTextField = "OwnerGroupName";
            ownerGroupCode_input.DataValueField = "OwnerGroupCode";
            ownerGroupCode_input.DataSource = dt;
            ownerGroupCode_input.DataBind();
            ownerGroupCode_input.Items.Insert(0, new ListItem("", ""));

            return true;
        }
        private bool getSelecedOwnerGroup(KMGroupEntity rawDate)
        {
            List<OwnerGroupEN> ownerGroupENs = new List<OwnerGroupEN>();
            //ownerGroupENs = ownerGroupLib.readByCode(SID, CompanyCode, "");

            DataTable dt = ownerGroupENs.toDataTable();
            bool allpermission = ERPWAuthentication.Permission.AllPermission;
            //System.Diagnostics.Debug.WriteLine("AllPermission: " + allpermission.ToString());
            //System.Diagnostics.Debug.WriteLine("OwnerGroupCode: " + ERPWAuthentication.Permission.OwnerGroupCode);
            if (allpermission == true)
            {
                ownerGroupENs = ownerGroupLib.readByCode(SID, CompanyCode, "");
                dt = ownerGroupENs.toDataTable();
                //System.Diagnostics.Debug.WriteLine("count: " + ownerGroupENs.Count);
                //ownerGroupENs.Count
                update_ownerGroup.DataTextField = "OwnerGroupName";
                update_ownerGroup.DataValueField = "OwnerGroupCode";
                update_ownerGroup.DataSource = dt;
                update_ownerGroup.DataBind();
                update_ownerGroup.Items.Insert(0, new ListItem("", ""));
            }
            else
            {
                ownerGroupENs = ownerGroupLib.readByCode(SID, CompanyCode, ERPWAuthentication.Permission.OwnerGroupCode);
                update_ownerGroup.DataTextField = "OwnerGroupName";
                update_ownerGroup.DataValueField = "OwnerGroupCode";
                update_ownerGroup.DataSource = dt;
                update_ownerGroup.DataBind();
            }
            update_ownerGroup.SelectedValue = rawDate.OwnerGroupCode;
            return true;
        }

        protected void save_btn_click(object sender, EventArgs e)
        {
            string message_group;// = message_group_input.Text;
            string group_name;
            string ownerGroupCode;
            try
            {
                if(string.IsNullOrEmpty(message_group_input.Text))
                {
                    throw new Exception("Enter code!!");
                }
                
                if (!kMGroupLib.isinDB(message_group_input.Text))
                {
                    throw new Exception("That code is taken. Try another.");
                }

                if (string.IsNullOrEmpty(group_name_input.Text))
                {
                    throw new Exception("Enter name!!");
                }

                message_group = message_group_input.Text;
                group_name = group_name_input.Text;

                if (string.IsNullOrEmpty(ownerGroupCode_input.SelectedValue))
                {
                    throw new Exception("Select OwnerGroup!!");
                }
                ownerGroupCode = ownerGroupCode_input.SelectedValue;
                if (!addrow(message_group, group_name, ownerGroupCode))
                {
                    throw new Exception("Cannot create new knowledge!!");
                }

                init();
                ClientService.AGSuccess("Create Success");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            //finally
            //{
            //    ClientService.AGLoading(false);
            //}
        }

        private bool addrow(string message_group, string group_name, string ownerGroupCode)
        {
            KMGroupEntity rawData = new KMGroupEntity();
            rawData.sid = SID;
            rawData.message_group = message_group;
            rawData.group_name = group_name;
            rawData.created_by = ERPWAuthentication.EmployeeCode;
            rawData.created_on = year + month + day + hour + minute + seconds;
            rawData.updated_by = ERPWAuthentication.EmployeeCode;
            rawData.updated_on = year + month + day + hour + minute + seconds;
            rawData.OwnerGroupCode = ownerGroupCode;
            try
            {
                return kMGroupLib.createRow(SID, CompanyCode, rawData);
            }
            catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return false;
            }
            
        }

        protected void readone_Click(object sender, EventArgs e)
        {
            bool isexit;
            string message_group;
            try
            {
                //check code is not null
                if (string.IsNullOrEmpty(code_input.Text))
                {
                    throw new Exception("Enter Code!!");
                }
                message_group = code_input.Text;
                //check code in db
                isexit = kMGroupLib.isinDB(message_group);
                if (isexit)
                {
                    throw new Exception("This Code is in database!!");
                }
                //
                KMGroupEntity rawData = new KMGroupEntity();
                rawData = kMGroupLib.readOneByMessageGroupCode(message_group);
                update_massage_group_input.Text = rawData.message_group;
                update_group_name_input.Text = rawData.group_name;
                getSelecedOwnerGroup(rawData);
                udpUpdate.Update();
                //rptItem.DataSource = rawData;
                //rptItem.DataBind();
                //update_udp.Update();
                ClientService.DoJavascript("afterLoad();");
                ClientService.DoJavascript("openupdatemodal();");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        protected void updateDesc_button_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty())
            bool isexit;
            bool result;
            string message_group;
            string group_name;
            string ownerGroupCode;
            try
            {
                if (string.IsNullOrEmpty(update_massage_group_input.Text))
                {
                    throw new Exception("Enter Code !!");
                }
                message_group = update_massage_group_input.Text;
                isexit = kMGroupLib.isinDB(message_group);
                System.Diagnostics.Debug.WriteLine("code: " + message_group);
                if (!isexit)
                {
                    if (!code_input.Text.Equals(message_group))
                    {
                        throw new Exception("Enter new Code!!");
                    }
                }
                if (string.IsNullOrEmpty(update_massage_group_input.Text))
                {
                    throw new Exception("Enter Name!!");
                }
                group_name = update_group_name_input.Text;
                if (string.IsNullOrEmpty(update_ownerGroup.SelectedValue))
                {
                    throw new Exception("Select Name!!");
                }
                ownerGroupCode = update_ownerGroup.SelectedValue;

                result = updaterow(message_group, group_name, ownerGroupCode);
                if (result)
                {
                    init();
                    ClientService.AGSuccess("Update Code Success");
                }
                else
                {
                    throw new Exception("Update Code Unsuccess!!");
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        private bool updaterow(string message_group, string group_name, string groupCode)
        {
            KMGroupEntity rawData = new KMGroupEntity();
            rawData = kMGroupLib.readOneByMessageGroupCode(message_group);
            //rawData.sid = SID;
            rawData.message_group = message_group;
            rawData.group_name = group_name;
            //rawData.created_by = "";
            //rawData.created_on = year + month + day + hour + minute + seconds;
            rawData.updated_by = ERPWAuthentication.EmployeeCode;
            rawData.updated_on = year + month + day + hour + minute + seconds;
            rawData.OwnerGroupCode = groupCode;
            try
            {
                return kMGroupLib.updateRow(SID, CompanyCode, rawData);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return false;
            }

        }

        private bool deleteRow(string message_group)
        {
            bool result;
            try
            {
                result = kMGroupLib.deleteRow(SID, CompanyCode, message_group);
                if (!result)
                {
                    throw new Exception("Cannot delete this row!!");
                }
                return true;
            }catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return false;
            }
        }

        protected void delete_button_Click(object sender, EventArgs e)
        {
            string message_group;
            bool result;
            try
            {
                if (string.IsNullOrEmpty(code_input.Text))
                {
                    throw new Exception("Cannot delete this row!!");
                }
                message_group = code_input.Text;
                result = deleteRow(message_group);
                if (!result)
                {
                    throw new Exception("Cannot delete this row!!");
                }
                init();
            }
            catch (Exception err)
            {
                ClientService.AGError(ObjectUtil.Err(err.Message));
            }
        }
    }
}