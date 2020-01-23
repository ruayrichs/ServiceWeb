
using Newtonsoft.Json;
using ServiceWeb.Asset.API.Class;
using ServiceWeb.LinkFlowChart.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Asset
{
    public partial class AssetStructureDiagram : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init();
            }
        }
        private void Init()
        {
            LinkFlowChartService.LinkFlowChartGetDatas FlowChartDatas = LinkFlowChartService.GetLinkFlowChart(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                LinkFlowChartService.ItemGroup_ASSET
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

            LinkFlowChartService.SaveLinkFlowChart(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                LinkFlowChartService.ItemGroup_ASSET,
                Items,
                Connectors
            );

            ClientService.AGLoading(false);
        }
        
        
    }
}