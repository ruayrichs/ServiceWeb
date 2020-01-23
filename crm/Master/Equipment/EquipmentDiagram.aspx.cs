
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.LinkFlowChart.Service;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Equipment
{
    public partial class EquipmentDiagram : AbstractsSANWebpage //System.Web.UI.Page
    {
        EquipmentService serEquipment = new EquipmentService();
        //LinkFlowChartService serLinkFlowChart = new LinkFlowChartService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init();
                bindDataDropdownEquipmentClass();
            }
        }

        private void Init()
        {
            LinkFlowChartService.LinkFlowChartGetDatas FlowChartDatas = LinkFlowChartService.GetLinkFlowChart(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                LinkFlowChartService.ItemGroup_EQUIPMENT
            );

            LinkFlowChartControl.InitLinkFlowChart(
                FlowChartDatas.Palette,
                FlowChartDatas.Item,
                FlowChartDatas.Connector
            );

            getDataValidate();
        }

        private void bindDataDropdownEquipmentClass()
        {
            DataTable dt = serEquipment.getConfigClassRelationMater(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode
            );

            ddlRelationType.DataSource = dt;
            ddlRelationType.DataBind();
            ddlRelationType.Items.Insert(0, new ListItem("เลือก", ""));
        }

        protected void btnSaveDataFlowChart_Click(object sender, EventArgs e)
        {

            try
            {
                List<LinkFlowChartService.LinkFlowChartItem> Items = JsonConvert.DeserializeObject<List<LinkFlowChartService.LinkFlowChartItem>>(txtSaveNodeDataArray.Text);
                List<LinkFlowChartService.LinkFlowChartConnector> Connectors = JsonConvert.DeserializeObject<List<LinkFlowChartService.LinkFlowChartConnector>>(txtSaveLinkDataArray.Text);

                //validateData(Items, Connectors);

                Connectors.ForEach(r =>
                {
                    if (!string.IsNullOrEmpty(r.TextDescription))
                    {
                        r.TextDescription = r.TextDescription.Split(':')[0].Trim();
                    }
                });

                LinkFlowChartService.SaveLinkFlowChart(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    LinkFlowChartService.ItemGroup_EQUIPMENT,
                    Items,
                    Connectors
                );
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

        private void getDataValidate()
        {
            List<EquipmentService.EquipmentItemData> listEquipmentItem = serEquipment.getListEquipment(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                "",
                "",
                "",
                ""
            );

            List<LinkFlowChartService.LinkFlowChartRelationDetail> listRelation_Class = LinkFlowChartService.getFlowChartRelation(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                LinkFlowChartService.ItemGroup_CLASS
            );

            txtLinkRelationArray.Text = JsonConvert.SerializeObject(listRelation_Class);
            txtEquipmentItemArray.Text = JsonConvert.SerializeObject(listEquipmentItem);
        }

        private void validateData(
            List<LinkFlowChartService.LinkFlowChartItem> Items, 
            List<LinkFlowChartService.LinkFlowChartConnector> Connectors)
        {
            List<string> listErr = new List<string>();

            List<EquipmentService.EquipmentItemData> listEquipmentItem = serEquipment.getListEquipment(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                "",
                "",
                "",
                ""
            );
            List<string> listEquipmentCode = Items.Select(s => s.ItemCode).ToList();
            listEquipmentItem = listEquipmentItem.Where(w => listEquipmentCode.Contains(w.EquipmentCode)).ToList();

            #region Check equipment not mapping class
            List<EquipmentService.EquipmentItemData> listEquipmentItemSelect = listEquipmentItem.Where(w => 
                string.IsNullOrEmpty(w.EquipmentClass)
            ).ToList();
            if (listEquipmentItemSelect.Count > 0)
            {

                listErr.Add("รายการ Configuration Item " + string.Join(", ", listEquipmentItemSelect.Select(s => s.EquipmentCode)) + " ไม่มีการกำหนด Class");
            }
            #endregion
            
            #region Check equipment relation Class
            List<LinkFlowChartService.LinkFlowChartRelationDetail> listRelation_Class = LinkFlowChartService.getFlowChartRelation(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode, 
                LinkFlowChartService.ItemGroup_CLASS
            );

            Items.ForEach(r =>
            {
                string EquipmentClass = listEquipmentItem.Where(w => 
                    w.EquipmentCode.Equals(r.ItemCode)
                ).First().EquipmentClass;

                List<string> listParentKey = Connectors.Where(w =>
                    w.ToKey.Equals(r.ItemKey)
                ).Select(s =>
                    s.FromKey
                ).ToList();

                List<string> listItemParentCode = Items.Where(w => 
                    listParentKey.Contains(w.ItemKey)
                ).Select(s => 
                    s.ItemCode
                ).ToList();

                List<string> listClassParrentCode = listEquipmentItem.Where(w =>
                    listItemParentCode.Contains(w.EquipmentCode)
                ).Select(s => 
                    s.EquipmentClass
                ).ToList();

                List<string> listClassParentCodeConfig = listRelation_Class.Where(w => 
                    w.ItemCode.Equals(EquipmentClass)
                ).Select(s => 
                    s.parentCode
                ).ToList();

                List<string> listParentCodeNoConfig = listClassParrentCode.Where(w => 
                    !listClassParentCodeConfig.Contains(w)
                ).ToList();

                if (listParentCodeNoConfig.Count > 0)
                {
                    listErr.Add("ไม่สามารถเชื่อม " + r.ItemText + " ภายใต้ " + string.Join(", ", listParentCodeNoConfig) + " ได้");
                }
            });
            #endregion

            if (listErr.Count > 0)
            {
                throw new Exception("<div>" + string.Join("<br />", listErr) + "</div>");
            }
        }
    }
}