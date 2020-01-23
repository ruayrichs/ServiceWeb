using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using SNA.Lib.crm.entity;
using SNA.Lib.crm.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSWeb.crm.usercontrol
{
    public partial class ctrlRegisMember : System.Web.UI.UserControl
    {
        DBService dbservice = new DBService();
        public String IdMember
        {
            get
            {
                return txtIdMember.Text;
            }
        }

        public String FirstName
        {
            get
            {
                return txtFirstName.Text;
            }
        }
        public String  LastName
        {
            get
            {
                return txtLastName.Text;
            }
        }
        public String  Phone
        {
            get
            {
                return txtPhone.Text.Replace("-","");
            }
        }
        public String  CellPhone
        {
            get
            {
                return txtCellPhone.Text.Replace("-","");
            }
        }
         public String  Email
        {
            get
            {
                return txtEmail.Text;
            }
        }
 
        public String Title
        {
            get
            {
                return ddlTitle.SelectedItem.Value;
            }
        }
        public String Branch
        {
            get
            {
                return   ddlBranch.SelectedItem.Value;
            }
        }
        public int NumMemberShip
        {
            get
            {
                return Convert.ToInt32( txtMembership.Text);
            }
        }

        public String memberType
        {
            get
            {
                return  ddlMemberType.SelectedItem.Value ;
            }
        }
        public String Birthdate
        {
            get
            {
                
                return  txtBirthDate.Text;
            }
        }
        public String Startdate
        {
            get
            {
                return txtStartDate.Text;
            }
        }

        
        public String comment
        {
            get
            {
                return txtcomment.Text;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if ( !IsPostBack )
            {

                txtStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                initDropDown();
            }
            
        }

        private void initDropDown()
        {
            List<LabelValueBean> listCusTitle =  SearchHelpHelper.searchCusTitle(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddlTitle.DataSource = listCusTitle;
            ddlTitle.DataTextField = "Text";
            ddlTitle.DataValueField = "Value";
            ddlTitle.DataBind();

            List<LabelValueBean> listBranch = SearchHelpHelper.searchListBranch(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddlBranch.DataSource = listBranch;
            ddlBranch.DataTextField = "Text";
            ddlBranch.DataValueField = "Value";
            ddlBranch.DataBind();
        }

        public void initData(ContactCustomer contact)
        {
            if ( contact == null )
            {
                return;
            }
            if (contact.Email.Split(',').Length > 1)
            {
                txtEmail.Text = contact.Email.Split(',')[0];
            }
            else
            {
                txtEmail.Text = contact.Email;
            }

            if (contact.Phone.Split(',').Length > 1)
            {
                txtPhone.Text = contact.Phone.Split(',')[0].Trim();
                try
                {
                    string sub = txtPhone.Text;
                    txtPhone.Text = sub.Substring(0, 3) + "-" + sub.Substring(3, 3) + "-" + sub.Substring(6, (sub.Length - 6));
                }
                catch { }
            }
            else
            {
                if (!String.IsNullOrEmpty(contact.Phone))
                {
                    txtPhone.Text = contact.Phone.Trim();
                    try
                    {
                        string sub = txtPhone.Text;
                        txtPhone.Text = sub.Substring(0, 3) + "-" + sub.Substring(3, 3) + "-" + sub.Substring(6, (sub.Length - 6));
                    }
                    catch { }
                }
                
            }

            txtFirstName.Text = String.IsNullOrEmpty(contact.FirstName) ? contact.NickName : contact.FirstName;
            txtLastName.Text = contact.LastName;
            txtMembership.Text = "1"; 
        }

        public DataTable getCheckMember(string name,string email,string Tel1,string mobile)
        {
            string sql = @"select cm.CustomerCode, cm.CustomerName  as customer
                          ,cm_gen.TelNo1,cm_gen.Mobile,cm_gen.EMail 
                             from master_customer cm 
                             left join master_customer_general cm_gen 
                             on cm.SID = cm_gen.SID and cm.CustomerCode = cm_gen.CustomerCode ";
                            sql += " where cm.SID = '"+ERPWAuthentication.SID+"' and cm.CompanyCode = '"+ERPWAuthentication.CompanyCode+"' ";


                            string sqlWhere = "";
                            if (!String.IsNullOrEmpty(name))
                            {
                                sqlWhere = " and  ";
                                sqlWhere += " cm.CustomerName LIKE '%" + name + "%' ";
                            }
                           
                            if (!String.IsNullOrEmpty(email))
                            {
                                sqlWhere = " and  ";
                                sqlWhere += "  cm_gen.EMail LIKE '%" + email + "%' ";
                            }
                            if (!String.IsNullOrEmpty(Tel1)) 
                            {
                                sqlWhere = " and  ";
                                sqlWhere += " cm_gen.TelNo1 LIKE '%" + Tel1 + "%' ";
                            }
                            if (!String.IsNullOrEmpty(mobile))
                           {
                               sqlWhere = " and  ";
                               sqlWhere += " cm_gen.Mobile LIKE '%" + mobile + "%' ";
                           }

                            if (String.IsNullOrEmpty(sqlWhere))
                            {
                                sql = sqlWhere;
                            }
                            sql += " " + sqlWhere;

                            return dbservice.selectDataFocusone(sql);

        }

        protected void btnclickSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = getCheckMember(txtFirstName.Text,"","","");
                
                if (dt.Rows.Count == 0)
                {
                    checkDataName.InnerHtml = "<div class='text-center'>ไม่พบรายการ</div>";
                }
                else
                {
                    checkDataName.InnerHtml = "";
                    rptlistMember.DataSource = dt;
                    rptlistMember.DataBind();
                }
                udpListMember.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        protected void btnClickSearchSername_Click(object sender, EventArgs e)
        {
            try
            {
                 DataTable dt = new DataTable();
                dt = getCheckMember(txtLastName.Text,"","","");
                
                if (dt.Rows.Count == 0)
                {
                    divLisSerName.InnerHtml = "<div class='text-center'>ไม่พบรายการ</div>";
                }
                else
                {
                    divLisSerName.InnerHtml = "";
                    RptListSerName.DataSource = dt;
                    RptListSerName.DataBind();
                }
                UdpListSerName.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        protected void btnSearchNumberMobile_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = getCheckMember("","","",txtPhone.Text.Replace("-",""));
                
                if (dt.Rows.Count == 0)
                {
                    divListMobile.InnerHtml = "<div class='text-center'>ไม่พบรายการ</div>";
                }
                else
                {
                    divListMobile.InnerHtml = "";
                    rptListMobile.DataSource = dt;
                    rptListMobile.DataBind();
                }
                udpListMobile.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        protected void btnSearchNumberTell_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = getCheckMember("","",txtCellPhone.Text,"");
                
                if (dt.Rows.Count == 0)
                {
                    divListTell.InnerHtml = "<div class='text-center'>ไม่พบรายการ</div>";
                }
                else
                {
                    divListTell.InnerHtml = "";
                    rptListTell.DataSource = dt;
                    rptListTell.DataBind();
                }
                udpListTell.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        protected void btnSearchEmail_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = getCheckMember("", txtEmail.Text, "", "");
                
                if (dt.Rows.Count == 0)
                {
                    divListEmail.InnerHtml = "<div class='text-center'>ไม่พบรายการ</div>";
                }
                else
                {
                    divListEmail.InnerHtml = "";
                    rptListEmail.DataSource = dt;
                    rptListEmail.DataBind();
                }
                udpListEmail.Update();
            }
            catch (Exception ex)
            {
                
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        public string  convertCommaToSpace(Object email)
        {
            string strEmailData = email.ToString();
            if (!String.IsNullOrEmpty(strEmailData))
            {
                strEmailData = strEmailData.Replace(",", " ");
            }
            return strEmailData;
        }
    }
}