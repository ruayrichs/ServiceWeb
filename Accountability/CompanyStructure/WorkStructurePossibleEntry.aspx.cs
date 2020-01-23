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
using ServiceWeb.MasterConfig.MasterPage;

namespace ServiceWeb.Accountability.CompanyStructure
{
    public partial class WorkStructurePossibleEntry : AbstractsSANWebpage
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getData();
            }
            ClientService.DoJavascript("bindBluePrintSortable();");
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

        public void getData()
        {
            String sql = @"SELECT [SID],[CompanyCode],[StructureCode],[Name],[Description],[NodeLevel],[created_by],[created_on] 
                            FROM [ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE] 
                            where SID = '" + ERPWAuthentication.SID + @"'
                                and CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'
                                and WorkGroupCode = '" + WorkGroupCode + @"' 
                            ORDER BY [NodeLevel] ASC";

            dataTableLinkProjectCompanyStructure = dbservice.selectDataFocusone(sql);
            rptLinkProjectCompanyStructure.DataSource = dataTableLinkProjectCompanyStructure;
            rptLinkProjectCompanyStructure.DataBind();
            udpLinkProjectCompanyStructure.Update();
            txtName.Text = "";
            txtDescription.Text = "";
            //udpLinkProjectCompanyStructure.Update();
        }

        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                ClientService.AGError("Input your name !!");
                ClientService.AGLoading(false);
            }
            else
            {
                for (int i = 0; i <= dataTableLinkProjectCompanyStructure.Rows.Count; i++)
                {
                    int dtSelect = dataTableLinkProjectCompanyStructure.Select("StructureCode = '" + i + "'").Length;
                    if (dtSelect == 0)
                    {
                        String sql = "insert into [ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE] ([SID],[CompanyCode],[WorkGroupCode],[StructureCode],[Name],[Description],[NodeLevel],[created_by],[created_on]) ";
                        sql += "VALUES ('" + ERPWAuthentication.SID + "','" + ERPWAuthentication.CompanyCode + "','" + "" + WorkGroupCode + "" + "','" + i + "',";
                        sql += "'" + txtName.Text + "','" + txtDescription.Text + "','" + dataTableLinkProjectCompanyStructure.Rows.Count + "','Admin','" + DateTime.Today.ToString("yyyyMMdd") + "');";
                        dbservice.executeSQLForFocusone(sql);
                        break;
                    }
                }
                getData();
                ClientService.AGLoading(false);
            }
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            string StructureCode = (sender as LinkButton).CommandName;
            DataRow[] StructureSelect = dataTableLinkProjectCompanyStructure.Select("StructureCode = '" + StructureCode + "'");
            int StructureSelectNodeLevel = Convert.ToInt32(StructureSelect[0]["NodeLevel"].ToString());
            for (int i = dataTableLinkProjectCompanyStructure.Rows.Count - 1; i >= StructureSelectNodeLevel; i--)
            {
                String sql = @"DELETE FROM [ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE] 
                            WHERE [NodeLevel] = '" + i + @"' 
                            and WorkGroupCode = '" + WorkGroupCode + @"' 
                            and SID = '" + ERPWAuthentication.SID + @"'
                            and CompanyCode = '" + ERPWAuthentication.CompanyCode + @"' ;";

                dbservice.executeSQLForFocusone(sql);
            }
            getData();
            ClientService.AGLoading(false);
        }

        protected void btnHideUpdate_Click(object sender, EventArgs e)
        {
            string valueHideUpdate = "{\"results\":" + txtHideUpdate.Text + "}";
            JObject results = JObject.Parse(valueHideUpdate);
            foreach (var rs in results["results"])
            {
                string id = (string)rs["id"];
                string index = (string)rs["index"];
                String sql = @"UPDATE [ERPW_ACCOUNTABILITY_PROJECT_COMPANY_STRUCTURE] 
                                SET [NodeLevel] = '" + index + @"' 
                                WHERE [StructureCode] ='" + id + @"'
                                and WorkGroupCode = '" + WorkGroupCode + @"' 
                                and SID = '" + ERPWAuthentication.SID + @"'
                                and CompanyCode = '" + ERPWAuthentication.CompanyCode + @"' ";
                dbservice.executeSQLForFocusone(sql);
            }
            getData();
        }
    }
}