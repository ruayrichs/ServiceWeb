using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class MasterDocType : AbstractsSANWebpage
    {

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        MasterConfigLibrary libMasConf = new MasterConfigLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
            //  libMasConf.getMasterDocType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            if (!IsPostBack)
            {
                dataBinding();

            }

        }

        private void dataBinding()
        {
            DataTable datatable = libMasConf.GetMasterDocType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            tableData.DataSource = datatable;
            tableData.DataBind();
            udpMasterConfig.Update();

            // libMasConf.UpdateERPWBusinessObjectMappingTicketType(ERPWAuthentication.SID, datatable);
            // System.Diagnostics.Debug.WriteLine(datatable);
            /*foreach (DataRow dataRow in datatable.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    //Console.WriteLine(item);
                   System.Diagnostics.Debug.WriteLine(item);
                }
            }*/

        }

        protected void ddlYourDDL_DataBinding(object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(((DropDownList)(sender)).SelectedValue);
        }
        protected void ddlSALGp_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(((DropDownList)(sender)).SelectedValue);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("TicketTypeCode");
                dt.Columns.Add("BusinessOject");
                dt.Columns.Add("Default_SLAGroup");

                foreach (RepeaterItem item in tableData.Items)
                {
                    string docCode = (item.FindControl("txtDocumentTypeCode") as TextBox).Text;
                    string busObj = (item.FindControl("ddlBusinessOject") as DropDownList).SelectedValue;
                    string ddlObj = (item.FindControl("ddlSALGp") as DropDownList).SelectedValue;
                    //  string busObj = (item.FindControl("ddlBusinessOject") as TextBox).Text;
                    DataRow dr = dt.NewRow();
                    dr[0] = docCode;
                    dr[1] = busObj;
                    dr[2] = ddlObj;
                    dt.Rows.Add(dr);
                    System.Diagnostics.Debug.WriteLine(dr[2]);
                }
                libMasConf.UpdateERPWBusinessObjectMappingTicketType(ERPWAuthentication.SID,
                    ERPWAuthentication.EmployeeCode,
                    dt
                    );
                ClientService.AGSuccess("Success");
                dataBinding();
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

        protected void tableData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable dt = libMasConf.getSLAGp(ERPWAuthentication.SID);
            DropDownList ddlSLA = e.Item.FindControl("ddlSALGp") as DropDownList;
            TextBox settxt = e.Item.FindControl("txtSLAGroup") as TextBox;          

            ddlSLA.DataValueField = "TierGroupCode";
            ddlSLA.DataTextField = "TierGroupDescription";
            ddlSLA.DataSource = dt;
            ddlSLA.DataBind();
            ddlSLA.Items.Insert(0, new ListItem("None", ""));
            ddlSLA.SelectedValue = settxt.Text;
            System.Diagnostics.Debug.WriteLine(settxt.ToString());
        }
    }
}