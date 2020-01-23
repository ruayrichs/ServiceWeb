using agape.lib.constant;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.Authentication.Entity;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Service;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterPage
{
    public partial class HeaderMaster : System.Web.UI.MasterPage
    {
        #region ERPW Authentication
        public string _SID;
        public string SID
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

        public string _CompanyCode;
        public string CompanyCode
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

        public string _UserName;
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_UserName))
                {
                    _UserName = ERPWAuthentication.UserName;
                }
                return _UserName;
            }
        }

        public string _EmployeeCode;
        public string EmployeeCode
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

        public string _FullNameEN;
        public string FullNameEN
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameEN))
                {
                    _FullNameEN = ERPWAuthentication.FullNameEN;
                }
                return _FullNameEN;
            }
        }

        public string _FullNameTH;
        public string FullNameTH
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameTH))
                {
                    _FullNameTH = ERPWAuthentication.FullNameTH;
                }
                return _FullNameTH;
            }
        }

        public string _CompanyName;
        public string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyName))
                {
                    _CompanyName = ERPWAuthentication.CompanyName;
                }
                return _CompanyName;
            }
        }

        public AuthenticationPermission _Permission;
        public AuthenticationPermission Permission
        {
            get
            {
                if (_Permission == null)
                {
                    _Permission = ERPWAuthentication.Permission;
                }
                return _Permission;
            }
        }
        #endregion

        //public string SID
        //{
        //    get { return hddSID.Value; }
        //    set { hddSID.Value = value; }
        //}

        //public string CompanyCode
        //{
        //    get { return hddCompanyCode.Value; }
        //    set { hddCompanyCode.Value = value; }
        //}

        public string PortalURL
        {
            get { return ""; }
        }

        public string HomePage
        {
            get
            {
                return Page.ResolveUrl("~/Default.aspx");
            }
        }

        public string DefaultPageURL
        {
            get
            {
                return "";
            }
        }
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        public string UserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(UserImg.Image_128))
                {
                    string filePath = Page.Request.PhysicalApplicationPath + UserImg.Image_128_WithOutCheckFile;

                    if (System.IO.File.Exists(filePath))
                    {
                        return UserImg.Image_128;
                    }
                }

                return Page.ResolveUrl("~/images/user_avatar.png");
            }
        }

        private UserImageService.UserImage _userImg;

        public UserImageService.UserImage UserImg
        {
            get
            {
                if (_userImg == null)
                {
                    _userImg = new UserImageService.UserImage(
                            CompanyCode,
                            SID,
                            EmployeeCode,
                            ERPWAuthentication.LatestUpdatedOn
                        );
                }

                return _userImg;
            }
        }

        public bool IsFilterOwner
        {
            get
            {
                bool _IsFilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out _IsFilterOwner);
                if (ERPWAuthentication.Permission.AllPermission)
                {
                    _IsFilterOwner = false;
                }
                return _IsFilterOwner;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //SID = SID;
                //CompanyCode = CompanyCode;
                showCiTimeNearlyUpExpire();
            }
        }

        protected void btnMasterPage_Logout_Click(object sender, EventArgs e)
        {
            Response.Cookies["SystemControlService_SID"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["SystemControlService_Email"].Expires = DateTime.Now.AddDays(-1);
            //ClientService.DoJavascript("windows.close();window.open('" + Page.ResolveUrl("~/Logout.aspx") + "');");
            Page.Response.Redirect(Page.ResolveUrl("~/Logout.aspx"));
        }

        protected void btnToTicket_Click(object sender, EventArgs e) {
            getdataToedit(txtReply_DocType.Text,txtReply_TicketNo.Text, txtReply_Filcalyear.Text, txtReply_RefCode.Text);
           
        }

        private void getdataToedit(string doctype, string docnumber, string fiscalyear,string refCode)
        {
            ERPW.Lib.Service.ServiceLibrary lib = new ERPW.Lib.Service.ServiceLibrary();
            try
            {
                lib.UpdateReplyCommentReaded(refCode);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }

            
            string customer = (string)Session["SCT_created_cust_code"];//CustomerSelect.SelectedValue;
            string idGen = redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                string PageRedirect = ServiceTicketLibrary.GetInstance().getPageTicketRedirect(
                    SID,
                    doctype
                );
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                //Response.-Redirect("/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen + "&ref=" + refCode, false);
            }
        }
        
        public string redirectViewToTicketDetail(string customerCode, string doctype, string docnumber, string fiscalyear)
        {
            string idGen = "";
            Object[] objParam = new Object[] { "1500117",
                    (string)Session[ApplicationSession.USER_SESSION_ID],
                    CompanyCode,doctype,docnumber,fiscalyear};

            DataSet[] objDataSet = new DataSet[] { new tmpServiceCallDataSet() };
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            if (objReturn != null)
            {
                idGen = Guid.NewGuid().ToString().Substring(0, 8);
                tmpServiceCallDataSet serviceTempCallEntity = new tmpServiceCallDataSet();
                serviceTempCallEntity.Merge(objReturn.Copy());
                Session["ServicecallEntity" + idGen] = serviceTempCallEntity;
                Session["SCT_created_cust_code" + idGen] = customerCode;//Customer
                Session["SC_MODE" + idGen] = ApplicationSession.DISPLAY_MODE_STRING;
            }
            return idGen;
        }

        private void showCiTimeNearlyUpExpire()
        {
            if (Permission == null || 
                !Permission.AllPermission)
            {
                return;
            }
            AfterSaleService after = new AfterSaleService();
            
            List<AfterSaleService.Time_NearlyUp> list_time_nearlyup = after.GetCITimesNearlyUp(SID, CompanyCode);
           int count =  list_time_nearlyup.Count;
            time_nearly_up_lists.DataSource = list_time_nearlyup;
            time_nearly_up_lists.DataBind();
            res_time_nearly_up_lists.DataSource = list_time_nearlyup;
            res_time_nearly_up_lists.DataBind();
            num_time_nearly_up.InnerText = count.ToString();
            res_num_time_neary_up.InnerText = count.ToString();
            update_time_nearly_up_list.Update();
            res_update_time_nearly_up_list.Update();
        }
        // 09/11/2556 add go to detail CI (by born kk)
        protected void btnOpenDetailEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ServiceWeb.Page.Equipment.EquipmentCode"] = hddEquipmentCode.Value;
                Session["ServiceWeb.Page.Equipment.Page_Mode"] = hddPage_Mode.Value;
                Response.Redirect(Page.ResolveUrl("~/crm/Master/Equipment/EquipmentDetail.aspx"), false);
            }
            catch (Exception ex)
            {

            }
        }

    }
}