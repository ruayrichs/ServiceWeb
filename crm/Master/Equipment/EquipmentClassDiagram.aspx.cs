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
    public partial class EquipmentClassDiagram : AbstractsSANWebpage
    {
        EquipmentService serEquipment = new EquipmentService();
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
                LinkFlowChartService.ItemGroup_CLASS
            );

            LinkFlowChartControl.InitLinkFlowChart(
                FlowChartDatas.Palette,
                FlowChartDatas.Item,
                FlowChartDatas.Connector
            );
        }

        protected void btnSaveDataFlowChart_Click(object sender, EventArgs e)
        {
            List<LinkFlowChartService.LinkFlowChartItem> Items = JsonConvert.DeserializeObject<List<LinkFlowChartService.LinkFlowChartItem>>(txtSaveNodeDataArray.Text);
            List<LinkFlowChartService.LinkFlowChartConnector> Connectors = JsonConvert.DeserializeObject<List<LinkFlowChartService.LinkFlowChartConnector>>(txtSaveLinkDataArray.Text);

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
                LinkFlowChartService.ItemGroup_CLASS,
                Items,
                Connectors
            );

            ClientService.AGLoading(false);
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
    }
}