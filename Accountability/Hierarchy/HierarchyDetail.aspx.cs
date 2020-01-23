using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json.Linq;
using ServiceWeb.Accountability.MasterPage;
using ServiceWeb.Accountability.Service;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service.Workflow;

namespace ServiceWeb.Accountability.Hierarchy
{
    public partial class HierarchyDetail : AbstractsSANWebpage
    {
        HierarchyService hierarchyservice = HierarchyService.getInstance();
        DBService dbservice = new DBService();

        private string _groupcode;
        public string groupcode
        {
            get
            {
                _groupcode = Request["groupcode"].ToString();
                return _groupcode;
            }
        }

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

        DataTable dtHierarchyType
        {
            get
            {
                if (Session["HierarchyType"] == null)
                { Session["HierarchyType"] = new DataTable(); }
                return (DataTable)Session["HierarchyType"];
            }
            set { Session["HierarchyType"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    bindDierarchyDetail();
                    getPossibleEntry();
                }
                ClientService.DoJavascript("bindBluePrintSortable();");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }
        private void bindDierarchyDetail()
        {
            DataTable dt = hierarchyservice.getHierarchyType(groupcode);

            ddlHierarchyType.DataValueField = "HIERARCHYTYPECODE";
            ddlHierarchyType.DataTextField = "HIERARCHYTYPENAME";

            ddlHierarchyType.DataSource = dt;
            ddlHierarchyType.DataBind();
            UpdatePanel1.Update();
            bindPossibleEntry(ddlHierarchyType.SelectedValue);
        }

        protected void showPopupClick(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = hierarchyservice.getHierarchyType(groupcode);

                rptHierarchy.DataSource = dt;
                rptHierarchy.DataBind();
                udpList.Update();
                ClientService.DoJavascript("showModal();");

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

        protected void showStructurClick(object sender, EventArgs e)
        {
            try
            {
                getDataPossibleEntry();
                ClientService.DoJavascript("showModalDetail();");
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

        protected void btnAddType_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCode.Text) && !string.IsNullOrEmpty(txtName.Text))
                {
                    bool chackCode = hierarchyservice.chackCodeHierarchyType(
                    groupcode,
                    txtCode.Text);

                    if (chackCode)
                    {
                        hierarchyservice.InsertHierarchyType(
                            txtCode.Text,
                            txtName.Text,
                            groupcode);

                        txtCode.Text = "";
                        txtName.Text = "";
                        bindDierarchyDetail();
                        ClientService.DoJavascript("hideModal();");
                        ClientService.AGSuccess("บันทึกสำเร็จ");
                    }
                    else
                    {
                        ClientService.AGMessage("Hierarchy type code<br /> \"" + txtCode.Text + "\" <br />มีอยู่แล้ว กรุณากรอกใหม่");
                    }
                }
                else
                {
                    ClientService.AGMessage("กรุณากรอกข้อมูลให้ครบถ้วน");
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

        protected void btnDeleteType_Click(object sender, EventArgs e)
        {
            try
            {
                string[] code = (sender as Button).CommandArgument.Split(',');
                hierarchyservice.DeleteHierarchyType(code[0], code[1]);
                bindDierarchyDetail();
                ClientService.DoJavascript("hideModal();");
                ClientService.AGSuccess("ลบสำเร็จ");
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

        protected void ddlHierarchyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bindPossibleEntry(ddlHierarchyType.SelectedValue);
                getPossibleEntry();
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

        public void getDataPossibleEntry()
        {
            dtHierarchyType = HierarchyService.getInstance().getHierarchy(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ddlHierarchyType.SelectedValue);
            rptHierarchyPossibleEntry.DataSource = dtHierarchyType;
            rptHierarchyPossibleEntry.DataBind();
            udpLinkProjectCompanyStructure.Update();
        }

        private void bindPossibleEntry(string HierarchyTypeCode)
        {
            if (string.IsNullOrEmpty(HierarchyTypeCode))
                return;

            dtHierarchyType = hierarchyservice.getPossibleEntry(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    HierarchyTypeCode);


            if (dtHierarchyType.Rows.Count > 0)
            {
                rptPossibleEntry.DataSource = dtHierarchyType;
                rptPossibleEntry.DataBind();
                panelStructure.Style.Add("display", "block");
                panelNoData.Style.Add("display", "none");
            }
            else
            {
                panelNoData.Style.Add("display", "block");
                panelStructure.Style.Add("display", "none");
            }

            ClientService.DoJavascript("bindHierarchy();");
            udpPossibleEntry.Update();
        }

        private void getPossibleEntry()
        {
            JArray dataPossibleEntry = new JArray();

            foreach (DataRow dr in dtHierarchyType.Rows)
            {
                JObject objoption = new JObject();

                objoption.Add("StructureCode", dr["PossibleEntryCode"].ToString());
                objoption.Add("Name", dr["Name"].ToString());
                objoption.Add("NodeLevel", dr["NodeLevel"].ToString());
                dataPossibleEntry.Add(objoption);
            }

            //JObject response = new JObject();
            //response.Add("data_possible_entry", dataPossibleEntry);

            txtPossibleEntry.Text = dataPossibleEntry.ToString();
            UpdatePanel2.Update();
        }

        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNameStructure.Text))
            {
                ClientService.AGError("กรุณากรอกชื่อ !!");
            }
            else
            {
                for (int i = 0; i <= dtHierarchyType.Rows.Count; i++)
                {
                    int dtSelect = dtHierarchyType.Select("PossibleEntryCode = '" + i + "'").Length;
                    if (dtSelect == 0)
                    {
                        string sql = @"insert into ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY_POSSIBLE_ENTRY (
                                SID
                                ,CompanyCode
                                ,PossibleEntryCode
                                ,HierarchyTypeCode
                                ,[Name]
                                ,[Description]
                                ,NodeLevel
                                ,created_by
                                ,created_on
                            ) VALUES (
                                '" + ERPWAuthentication.SID + @"'
                                ,'" + ERPWAuthentication.CompanyCode + @"'
                                ,'" + i + @"'
                                ,'" + ddlHierarchyType.SelectedValue + @"'
                                ,'" + txtNameStructure.Text + @"'
                                ,'" + txtDescription.Text + @"'
                                ," + dtHierarchyType.Rows.Count + @"
                                ,'" + ERPWAuthentication.EmployeeCode + @"'
                                ,'" + Validation.getCurrentServerStringDateTime() + @"'
                            )";

                        dbservice.executeSQLForFocusone(sql);
                        break;
                    }
                }
                getDataPossibleEntry();
                bindPossibleEntry(ddlHierarchyType.SelectedValue);
                getPossibleEntry();
                txtNameStructure.Text = "";
                ClientService.AGSuccess("เพิ่มสำเร็จ");
                ClientService.AgroLoading(false);
            }
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            string PossibleEntryCode = (sender as Button).CommandName;
            DataRow[] StructureSelect = dtHierarchyType.Select("PossibleEntryCode = '" + PossibleEntryCode + "'");
            int StructureSelectNodeLevel = Convert.ToInt32(StructureSelect[0]["NodeLevel"].ToString());
            for (int i = dtHierarchyType.Rows.Count - 1; i >= StructureSelectNodeLevel; i--)
            {
                String sql = @"delete from ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY_POSSIBLE_ENTRY 
                    WHERE SID = '" + ERPWAuthentication.SID + @"'
                    AND CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'
                    AND PossibleEntryCode = '" + i + @"'
                    AND HierarchyTypeCode = '" + ddlHierarchyType.SelectedValue + "'";

                dbservice.executeSQLForFocusone(sql);
            }
            getDataPossibleEntry();
            bindPossibleEntry(ddlHierarchyType.SelectedValue);
            getPossibleEntry();
            ClientService.AGSuccess("ลบสำเร็จ");
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

                string sql = @"update ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY_POSSIBLE_ENTRY 
                    SET NodeLevel = '" + index + @"'
                    WHERE SID = '" + ERPWAuthentication.SID + @"'
                    AND CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'
                    AND PossibleEntryCode = '" + id + @"'
                    AND HierarchyTypeCode = '" + ddlHierarchyType.SelectedValue + @"'";

                dbservice.executeSQLForFocusone(sql);
            }
            getDataPossibleEntry();
            bindPossibleEntry(ddlHierarchyType.SelectedValue);
            getPossibleEntry();
        }
    }
}