
using Newtonsoft.Json;
using ServiceWeb.LinkFlowChart.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.LinkFlowChart
{
    public partial class LinkFlowChartControl : System.Web.UI.UserControl
    {
        public string CallbackSaveDatas { get; set; }
        public string CallbackOnLinkDrawn { get; set; }
        public string CallbackOnDropBox { get; set; }

        public bool IsVisibleLinkDesc { get; set; }
        public bool IsCanUndo { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void InitLinkFlowChart(
            List<LinkFlowChartService.LinkFlowChartPalette> PaletteList, 
            List<LinkFlowChartService.LinkFlowChartItem> DataItemList, 
            List<LinkFlowChartService.LinkFlowChartConnector> ConnectorList)
        {
            txtPaletteDataArray.Text = JsonConvert.SerializeObject(PaletteList);
            txtNodeDataArray.Text = JsonConvert.SerializeObject(DataItemList);
            txtLinkDataArray.Text = JsonConvert.SerializeObject(ConnectorList);

            udpBackupJSON.Update();

            ClientService.DoJavascript("_LinkFlowChartInit();");
        }
    }
}