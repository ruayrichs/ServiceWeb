using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using System;
using System.Data;

namespace ServiceWeb.MasterConfig
{
    public partial class RequestPermissionKeyList : System.Web.UI.Page
    {
        private ERPW_API_Permission_Token_Key pkey_model = new ERPW_API_Permission_Token_Key();
        private ERPW_API_Permission_Token_Key_DAO pkey_dao = new ERPW_API_Permission_Token_Key_DAO();
        RequestPermissionLibrary reqpermission = new RequestPermissionLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region bind TB
        private void binddata()
        {
            string gettxtStartDate = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtStartDate.Text);
            string gettxtEndDate = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtEndDate.Text);
            getddlChanel = ddlChanel.SelectedValue;
            getddlStatus = ddlStatus.SelectedValue;
            DataTable dt = reqpermission.getPermission(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, getddlChanel, getddlStatus, gettxtStartDate, gettxtEndDate);
            rptSearch.DataSource = dt;
            rptSearch.DataBind();
            updateList.Update();
            ClientService.DoJavascript("$('#tableItems').DataTable({});");
        }
        #endregion
        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Page.ResolveUrl("~/MasterConfig/RequestPermissionKeyDetail.aspx"));
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
        protected void btnSetEdit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(PermissionKey.Value);
            try
            {
                Session["ServiceWeb.Page.MasterConfig.PermissionKey"] = PermissionKey.Value;

                DataTable dt = reqpermission.getKey(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PermissionKey"].ToString() == PermissionKey.Value)
                    {
                        Response.Redirect(Page.ResolveUrl("~/MasterConfig/RequestPermissionKeyDetail.aspx?key=" + PermissionKey.Value + "&edit=true"));
                    }
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                binddata();
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
        public string getddlChanel { get; set; }
        public string getddlStatus { get; set; }
    }
}