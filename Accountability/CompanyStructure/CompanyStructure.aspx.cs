using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json.Linq;
using ServiceWeb.Accountability.MasterPage;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.Service;
using ServiceWeb.MasterConfig.MasterPage;

namespace ServiceWeb.Accountability.CompanyStructure
{
    public partial class CompanyStructure : AbstractsSANWebpage
    {
        DBService dbservice = new DBService();

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        public string WorkGroupCode
        {
            get
            {
                return (Master as AccountabilityMaster).WorkGroupCode;
            }
        }


        DataTable dataTableLinkProjectCompanyStructure
        {
            get
            {
                if (Session["LinkProjectCompanyStructure"] == null)
                { Session["LinkProjectCompanyStructure"] = new DataTable(); }
                return (DataTable)Session["LinkProjectCompanyStructure"];
            }
            set { Session["LinkProjectCompanyStructure"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Remove("LinkProjectCompanyStructure");
                getData();
                getPossibleEntry();
                //setNodelevel();
                getDataWorkGroupForSelect();
            }
        }
        protected void btnRebindHierarchyBlackboard_Click(object sender, EventArgs e)
        {
            //CompanyStructureBlackboard.RebindConfig(true);
        }

        public void getData()
        {
            String sql = @"SELECT [SID],[CompanyCode],[StructureCode],[Name],[Description],[NodeLevel],[created_by],[created_on] 
                            FROM [ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE] 
                            where SID = '" + ERPWAuthentication.SID + @"'
                                and CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'
                                and WorkGroupCode = '" + WorkGroupCode + @"' 
                            ORDER BY [NodeLevel] ASC";

            dataTableLinkProjectCompanyStructure = dbservice.selectDataFocusone(sql);
            rptConfigStructure.DataSource = dataTableLinkProjectCompanyStructure;
            rptConfigStructure.DataBind();
        }

        //private void setNodelevel()
        //{
        //    CompanyStructureBlackboard.maxNodeLevel = dataTableLinkProjectCompanyStructure.Rows.Count - 1;
        //}

        private void getPossibleEntry()
        {
            JArray dataPossibleEntry = new JArray();

            foreach (DataRow dr in dataTableLinkProjectCompanyStructure.Rows)
            {
                JObject objoption = new JObject();

                objoption.Add("StructureCode", dr["StructureCode"].ToString());
                objoption.Add("Name", dr["Name"].ToString());
                objoption.Add("NodeLevel", dr["NodeLevel"].ToString());
                dataPossibleEntry.Add(objoption);
            }

            //JObject response = new JObject();
            //response.Add("data_possible_entry", dataPossibleEntry);

            txtPossibleEntry.Text = dataPossibleEntry.ToString();
        }


        private void getDataWorkGroupForSelect()
        {
            DataTable dt = ProjectService.getInstance().searchWorkGroupHeader(
                                       ERPWAuthentication.SID,
                                       ERPWAuthentication.CompanyCode,
                                       "",
                                       "GENERAL"
                                       );
            ddlWorkGroup.DataTextField = "ProjectName";
            ddlWorkGroup.DataValueField = "ProjectCode";
            ddlWorkGroup.DataSource = dt;
            ddlWorkGroup.DataBind();
            ddlWorkGroup.Items.Insert(0, new ListItem("Select Project", ""));

        }



        protected void btnBindDataCustomizeTemplateForWorkGroup_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtWorkGroup = ProjectService.getInstance().SearchMappingForWorkGroupAndProjectCode
                    (
                    ERPWAuthentication.SID,
                    WorkGroupCode,
                    txtProjextCodeForCustomizeTemplate.Text,
                    ""
                    );
                string ProjectCode = "";
                string Projectname = "Select Project";
                if (dtWorkGroup.Rows.Count > 0)
                {
                    ProjectCode = dtWorkGroup.Rows[0]["ProjectCode"].ToString();
                    Projectname = dtWorkGroup.Rows[0]["ProjectName"].ToString();
                }
                getDataCustomizeTemplateFromProjectCode(ProjectCode);
                ClientService.DoJavascript("setDropdowModeStatusPrijectSelect('" + ProjectCode + "','" + Projectname + "')");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        private void getDataCustomizeTemplateFromProjectCode(string projectCode)
        {
            if (String.IsNullOrEmpty(projectCode))
            {
                panelProjectDataDetial.Style["display"] = "none";
                udpUpdateDataForProjectSelect.Update();
                return;
            }
            DataTable dt = ProjectService.getInstance().searchWorkGroupCustomizeTemplate(
                                            ERPWAuthentication.SID,
                                            ERPWAuthentication.CompanyCode,
                                            projectCode
                                            );
            decimal latitude = (decimal)13.778200;
            decimal logitude = (decimal)100.476958;
            decimal latitudeDraf = 0;
            decimal logitudeDraf = 0;

            if (dt.Rows.Count > 0)
            {
                txtLatitudeLongitudeCustomize.Text = String.IsNullOrEmpty(dt.Rows[0]["Latitude"].ToString()) ? "" : dt.Rows[0]["Latitude"].ToString() + "," + dt.Rows[0]["Longitude"].ToString();
                txtProjectTypeCustomize.Text = dt.Rows[0]["ProjectType"].ToString();
                txtBudgetsTypeCustomize.Text = dt.Rows[0]["BudgetType"].ToString();
                txtProjectValueCustomize.Text = Convert.ToDecimal(dt.Rows[0]["ProjectValue"].ToString()).ToString("#,###.00");
                txtBuildingNumberCutomize.Text = Convert.ToDecimal(dt.Rows[0]["BuildingNumber"]).ToString("#,###.00");
                txtMaximumNumberClassCutomize.Text = Convert.ToDecimal(dt.Rows[0]["MaxNumberClass"].ToString()).ToString("#,###.00");
                txtAreaBuildingCustomize.Text = Convert.ToDecimal(dt.Rows[0]["AreaBuildingMeter"].ToString()).ToString("#,###.00");
                txtInsuranceCustomize.Text = dt.Rows[0]["Insurance"].ToString();
                txtStartDateInsuranceCustomize.Text = Validation.Convert2DateDisplay(dt.Rows[0]["InsuranceStartDate"].ToString());
                txtRemarkCustomize.Text = dt.Rows[0]["Remark"].ToString();

                decimal.TryParse(dt.Rows[0]["Latitude"].ToString(), out latitudeDraf);
                decimal.TryParse(dt.Rows[0]["Longitude"].ToString(), out logitudeDraf);
            }
            else
            {
                txtLatitudeLongitudeCustomize.Text = "-";
                txtProjectTypeCustomize.Text = "-";
                txtBudgetsTypeCustomize.Text = "-";
                txtProjectValueCustomize.Text = "-";
                txtBuildingNumberCutomize.Text = "-";
                txtMaximumNumberClassCutomize.Text = "-";
                txtAreaBuildingCustomize.Text = "-";
                txtInsuranceCustomize.Text = "-";
                txtStartDateInsuranceCustomize.Text = "-";
                txtRemarkCustomize.Text = "";
            }
            panelProjectDataDetial.Style["display"] = "";
            udpUpdateDataForProjectSelect.Update();
            latitude = latitudeDraf == 0 ? latitude : latitudeDraf;
            logitude = logitudeDraf == 0 ? logitude : logitudeDraf;
            ClientService.DoJavascript("setlatlongFortextboxDisplayMap('" + latitude + "','" + logitude + "');");
        }

        protected void btnBindDataForSelectProjectCodeCustomizeTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                getDataCustomizeTemplateFromProjectCode(ddlWorkGroup.SelectedValue);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void btnSaveMappingProject_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectService.getInstance().UpdateMappingForWorkGroupAndProjectCode
                    (
                    ERPWAuthentication.SID,
                    WorkGroupCode,
                    txtProjextCodeForCustomizeTemplate.Text,
                    ddlWorkGroup.SelectedValue,
                    ERPWAuthentication.EmployeeCode
                    );
                ClientService.DoJavascript("Save Success.");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }
    }
}