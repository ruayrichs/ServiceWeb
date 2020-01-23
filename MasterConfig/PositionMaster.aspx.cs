using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Service;
using System.Data;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ServiceWeb.auth;

namespace ServiceWeb.MasterConfig
{
    public partial class PositionMaster : AbstractsSANWebpage
    {
        #region Service
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        private UserManagementService userservice = UserManagementService.getInstance();
        DBService db = new DBService();
        PositionService positionService = new PositionService();

        #endregion

        #region Session


        DataTable DTGridView
        {
            get
            {
                if (Session["DTGridView.PositionMaster"] == null)
                { Session["DTGridView.PositionMaster"] = new DataTable(); }
                return (DataTable)Session["DTGridView.PositionMaster"];
            }
            set { Session["DTGridView.PositionMaster"] = value; }
        }

        DataTable SESSION_DT_MAIN_DELEGATE
        {
            get
            {
                return (DataTable)Session["SESSION_DT_MAIN_DELEGATE"];
            }
            set
            {
                Session["SESSION_DT_MAIN_DELEGATE"] = value;
            }
        }

        DataTable SESSION_DT_OTHER_DELEGATE
        {
            get
            {
                return (DataTable)Session["SESSION_DT_OTHER_DELEGATE"];
            }
            set
            {
                Session["SESSION_DT_OTHER_DELEGATE"] = value;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                getgvPositionMaster();

            }

        }

        protected void getgvPositionMaster()
        {
            DTGridView = positionService.GetPositionMaster(ERPWAuthentication.SID, "", "");
            rptSearch.DataSource = DTGridView;
            rptSearch.DataBind();
            updateList.Update();
        }

        protected void Show_Create_Edit()
        {
            DTGridView = positionService.GetPositionMaster(ERPWAuthentication.SID, txtPositionCodeModol.Text, "");
            rptSearch.DataSource = DTGridView;
            rptSearch.DataBind();
            updateList.Update();
        }

        protected void btnEditPosition_Click(object sender, EventArgs e)
        {
            positionService.UpdatePositionMaster(ERPWAuthentication.SID, txtPositionCodeModol.Text, txtPositionName.Text, txtDescription.Text, ERPWAuthentication.EmployeeCode);

            ClientService.DoJavascript("$('.modal').modal('hide');");
            ClientService.DoJavascript("AGSuccess('Change success.')");
            getgvPositionMaster();
            updatePanelPositionModol.Update();
            //Show_Create_Edit();
            //ClearItem();
            btnCreatePosition.Visible = true;
            btnEditPosition.Visible = false;
            ClientService.DoJavascript("setDataTable()");
        }

        protected void btnCreatePosition_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtPositionCodeModol.Text))
                {
                    throw new Exception("กรุณากรอก Position Code");
                }

                positionService.CreatePositionMaster(ERPWAuthentication.SID, txtPositionCodeModol.Text, txtPositionName.Text, txtDescription.Text, ERPWAuthentication.EmployeeCode);

                ClientService.DoJavascript("$('.modal').modal('hide');");
                ClientService.DoJavascript("AGSuccess('Create success.')");
                getgvPositionMaster();
                //Show_Create_Edit();
                ClearItem();
                ClientService.DoJavascript("setDataTable()");
            }
            catch (Exception ex)
            {

                ClientService.DoJavascript("AGError('" + ObjectUtil.Err(ex.Message) + "')");
            }


        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    DTGridView = positionService.GetPositionMaster(ERPWAuthentication.SID, txtcode.Text, txtname.Text);
        //    rptSearch.DataSource = DTGridView;
        //    rptSearch.DataBind();
        //    updateList.Update();
        //    //updatePanelPositionModol.Update();
        //}

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string code = Convert.ToString(((Button)sender).CommandArgument);
            DTGridView = positionService.GetPositionMaster(ERPWAuthentication.SID, "", "");
            DataRow[] drrP = DTGridView.Select("PositionCode ='" + code + "' ");
            if (drrP.Length > 0)
            {
                DataRow drP = drrP[0];
                txtPositionCodeModol.Text = Convert.ToString(drP["PositionCode"]);
                txtPositionCodeModol.Enabled = false;
                txtPositionName.Text = Convert.ToString(drP["PositionName"]);
                txtDescription.Text = Convert.ToString(drP["Description"]);
            }
            btnCreatePosition.Visible = false;
            btnEditPosition.Visible = true;
            updatePanelPositionModol.Update();
            ClientService.DoJavascript("$('#master-data').modal('show');");

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string code = Convert.ToString(((Button)sender).CommandArgument);
            positionService.DeletePositionMaster(ERPWAuthentication.SID, code);

            ClientService.DoJavascript("AGSuccess('Delete success.')");
            getgvPositionMaster();
            ClientService.DoJavascript("setDataTable()");

        }

        protected void ClearItem()
        {
            txtPositionCodeModol.Text = "";
            txtPositionCodeModol.Enabled = true;
            txtPositionName.Text = "";
            txtPositionName.Enabled = true;
            txtDescription.Text = "";
            txtDescription.Enabled = true;
        }

        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {
                //Response-.Redirect("~/MasterConfig/RequestPermissionKeyDetail.aspx");
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

        }

        

        //protected void gvPosition_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        //rptSearch.PageIndex = e.NewPageIndex;
        //        rptSearch.DataSource = DTGridView;
        //        rptSearch.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(updatePanelPosition, updatePanelPosition.GetType(), "msgbox", "alertMessage('" + ObjectUtil.Err(ex.Message) + "');", true);
        //    }

        //}

    }
}