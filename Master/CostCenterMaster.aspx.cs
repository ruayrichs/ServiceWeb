using agape.lib.agcommonutils;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.Service;
using ServiceWeb.widget.usercontrol;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Master
{
    public partial class CostCenterMaster : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        private CostCenterService serCostCenter = new CostCenterService();

        private CostCenterEn enCostCenter { get { return getCostCenterDisplay(); } }

        private DataTable _dtMaterial;
        private DataTable dtMaterial
        {
            get
            {
                if (_dtMaterial == null)
                {
                    _dtMaterial = materialService.getInSatnce().getMaterialGeneral_V2(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "", "", "", "True", ""
                    );
                }
                return _dtMaterial;
            }
        }

        public string CostCenterID
        {
            get { return Request["id"]; }
        }

        public string GetSaveBOMDescription()
        {
            //if (serCostCenter.HasCreateBOM(ERPWAuthentication.SID, CostCenterID, ""))
            //{
            //    return "Update BOM";
            //}

            return "Create BOM";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindCurrency();
                bindDataMat();
                bindPlant();

                if (!string.IsNullOrEmpty(CostCenterID))
                {
                    bindDataCostCenter();
                }
            }
        }

        private void bindCurrency()
        {
            lbCurrency.Text = serCostCenter.GetCompanyCurrency(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
        }

        private void bindPlant()
        {
            //DataTable dt = serCostCenter.GetPlant(ERPWAuthentication.SID);

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dr["PLANTNAME1"] = ObjectUtil.PrepareCodeAndDescription(dr["PLANTCODE"].ToString(), dr["PLANTNAME1"].ToString());
            //}

            //ddlPlant.DataValueField = "PLANTCODE";
            //ddlPlant.DataTextField = "PLANTNAME1";
            //ddlPlant.DataSource = dt;
            //ddlPlant.DataBind();
            //ddlPlant.Items.Insert(0, new ListItem("", ""));
        }

        private void bindDataCostCenter()
        {
            CostCenterEn enCostCenter = new CostCenterEn();

            List<CostCenterEn> enList = serCostCenter.getListCostCenter(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                CostCenterID,
                true
            );

            if (enList.Count > 0)
            {
                enCostCenter = enList.First();
                txtID.Text = enCostCenter.CostCenterID;
                txtContractMonth.Text = enCostCenter.ContractMonth.ToString();
                txtSubject.Text = enCostCenter.Subject;
                txtRemark.Text = enCostCenter.Remark;
                txtValidFrom.Text = Validation.Convert2DateDisplay(enCostCenter.ValidFrom);
                txtValidTo.Text = Validation.Convert2DateDisplay(enCostCenter.ValidTo);
                rdoOneTime.Checked = Convert.ToBoolean(enCostCenter.Onetime);
                rdoRecurring.Checked = !Convert.ToBoolean(enCostCenter.Onetime);
                lbTotalAmount.Text = enCostCenter.TotalAmount.ToString("N2");
                hddJsonCostCenter.Value = JsonConvert.SerializeObject(enCostCenter.ListStructure.OrderBy(o => o.NodeLevel));

                ClientService.DoJavascript("bindCostCenterStructure();");
            }
        }

        private void bindDataMat()
        {
            DataTable DTSourceMaterial = new DataTable();
            DTSourceMaterial.Columns.Add("code");
            DTSourceMaterial.Columns.Add("desc");
            DTSourceMaterial.Columns.Add("display");
            DTSourceMaterial.Columns.Add("uom");

            foreach (DataRow dr in dtMaterial.Rows)
            {
                DataRow drNew = DTSourceMaterial.NewRow();
                drNew["code"] = Convert.ToString(dr["ItmNumber"]);
                drNew["desc"] = Convert.ToString(dr["ItmDescription"]);
                drNew["display"] = Convert.ToString(dr["ItmNumber"]) + " : " + Convert.ToString(dr["ItmDescription"]);
                drNew["uom"] = Convert.ToString(dr["BaseUOM"]);
                DTSourceMaterial.Rows.Add(drNew);
            }
            hddJsonMat.Value = JsonConvert.SerializeObject(DTSourceMaterial);

            //rptListMaterial.DataSource = dtMaterial;
            //rptListMaterial.DataBind();
        }

        private CostCenterEn getCostCenterDisplay()
        {
            decimal contractMonth = 1, totalAmount = 0;
            decimal.TryParse(txtContractMonth.Text, out contractMonth);
            decimal.TryParse(lbTotalAmount.Text, out totalAmount);

            CostCenterEn en = new CostCenterEn();
            en.Subject = txtSubject.Text;
            en.Remark = txtRemark.Text;
            en.ContractMonth = contractMonth;
            en.Onetime = rdoOneTime.Checked ? "True" : "False";
            en.ValidFrom = Validation.Convert2DateDB(txtValidFrom.Text);
            en.ValidTo = Validation.Convert2DateDB(txtValidTo.Text);
            en.TotalAmount = totalAmount;
            en.ListStructure = JsonConvert.DeserializeObject<List<CostCenterStructureEn>>(hddJsonCostCenter.Value);
            return en;
        }

        protected void btnSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CostCenterID))
                {
                    string costsheetID = serCostCenter.createCostCenter(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        enCostCenter,
                        ERPWAuthentication.EmployeeCode
                    );                     

                    ClientService.AGSuccessRedirect("Created Cost Sheet ID : " + costsheetID + " Success.", "CostCenterMaster.aspx?id=" + costsheetID);
                }
                else
                {
                    serCostCenter.updateCostCenter(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        CostCenterID,
                        enCostCenter,
                        ERPWAuthentication.EmployeeCode
                    );

                    ClientService.AGSuccessRedirectCurrentPage("Updated Cost Sheet ID : " + CostCenterID + " Success.");
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

        private List<ERPW.Lib.Master.CostCenterService.BOMHeader> listBOM
        {
            get { return (List<ERPW.Lib.Master.CostCenterService.BOMHeader>)Session["listBOM_" + CostCenterID]; }
            set { Session["listBOM_" + CostCenterID] = value; }
        }

        private void DefaultBOM()
        {
            if (listBOM != null && listBOM.Count > 0)
            {
                ddlPrice.SelectedValue = listBOM[0].Price;
                ddlStock.SelectedValue = listBOM[0].Stock;
                ddlCost.SelectedValue = listBOM[0].Cost;
            }
            else
            {
                ddlPrice.SelectedValue = CostCenterService.BOM_CALULATE_MATERIAL;
                ddlStock.SelectedValue = CostCenterService.BOM_CALULATE_MATERIAL;
                ddlCost.SelectedValue = CostCenterService.BOM_CALULATE_MATERIAL;
            }
        }

        private void PrepareBOM(CostCenterEn en)
        {
            listBOM = serCostCenter.PrepareBOM(ERPWAuthentication.SID, ERPWAuthentication.UserName, en);
        }

        protected void btnBindingBOM_Click(object sender, EventArgs e)
        {
            try
            {                
                List<CostCenterEn> enList = serCostCenter.getListCostCenter(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    CostCenterID,
                    true
                );

                if (enList.Count > 0)
                {                                                            
                    rptBom.DataSource = enList[0].ListStructure.OrderBy(o => o.NodeCode);
                    rptBom.DataBind();

                    PrepareBOM(enList[0]);
                    DefaultBOM();

                    ClientService.DoJavascript("$('#modal-create-bom').modal('show');");
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

        protected void btnCreateBOM_Click(object sender, EventArgs e)
        {
            try
            {                
                if (listBOM != null && listBOM.Count > 0)
                {
                    int index = 0;

                    string createdOn = Validation.getCurrentServerStringDateTime();

                    listBOM.ForEach(r =>
                    {                       
                        RepeaterItem rptItem = rptBom.Items[index];

                        string plantCode = "";

                        if (rptItem != null)
                        {
                            plantCode = (rptItem.FindControl("ddlPlant") as DropDownList).SelectedValue;
                        }

                        r.Plant = plantCode;
                        r.Stock = ddlStock.SelectedValue;
                        r.Price = ddlPrice.SelectedValue;
                        r.Cost = ddlCost.SelectedValue;
                        r.CREATED_ON = createdOn;

                        index++;

                        r.listBOMItem.ForEach(i =>
                        {
                            RepeaterItem rptItemChild = rptBom.Items[index];

                            if (rptItemChild != null)
                            {
                                plantCode = (rptItemChild.FindControl("ddlPlant") as DropDownList).SelectedValue;                                
                            }

                            i.PlantCode = plantCode;
                            i.CREATED_ON = createdOn;

                            index++;
                        });                   
                    });

                    serCostCenter.SaveBOM(ERPWAuthentication.CompanyCode, CostCenterID, listBOM);

                    ClientService.DoJavascript("$('#modal-create-bom').modal('hide');");                    
                    ClientService.AGSuccessRedirectCurrentPage("Save BOM success.");
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

        protected void rptBom_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                DropDownList ddl = e.Item.FindControl("ddlPlant") as DropDownList;

                if (ddl != null)
                {                    
                    HiddenField hdfMaterial = e.Item.FindControl("hdfMaterial") as HiddenField;
                    HiddenField hdfPlant = e.Item.FindControl("hdfPlant") as HiddenField;

                    DataTable dt = materialService.getInSatnce().getMaterialInventory(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, hdfMaterial.Value, "");

                    DataTable dtPlant = dt.DefaultView.ToTable(true, "Plant", "PlantDesc");

                    ddl.DataSource = dtPlant;
                    ddl.DataBind();

                    if (hdfPlant.Value != "")
                    {
                        if (ddl.Items.FindByValue(hdfPlant.Value) != null)
                        {
                            ddl.SelectedValue = hdfPlant.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }
    }
}