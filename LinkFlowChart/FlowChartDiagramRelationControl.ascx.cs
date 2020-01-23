using ERPW.Lib.Authentication;
using Newtonsoft.Json;
using ServiceWeb.LinkFlowChart.Service;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.LinkFlowChart
{
    public partial class FlowChartDiagramRelationControl : System.Web.UI.UserControl
    {
        private EquipmentService serEquipment = new EquipmentService();

        private List<LinkFlowChartService.FlowChartRelation> _listParentDiagram;
        public List<LinkFlowChartService.FlowChartRelation> listParentDiagram
        {
            get
            {
                _listParentDiagram = JsonConvert.DeserializeObject<List<LinkFlowChartService.FlowChartRelation>>(txtParendDataSource.Text);
                if (_listParentDiagram.Count == 0)
                {
                    return new List<LinkFlowChartService.FlowChartRelation>();
                }

                return _listParentDiagram.OrderBy(o => o.Level).ToList();
            }
            set { txtParendDataSource.Text = JsonConvert.SerializeObject(value.OrderBy(o => o.Level)); udpDataSource.Update(); }
        }

        private List<LinkFlowChartService.FlowChartRelation> _listChildDiagram;
        public List<LinkFlowChartService.FlowChartRelation> listChildDiagram
        {
            get 
            {
                _listChildDiagram = JsonConvert.DeserializeObject<List<LinkFlowChartService.FlowChartRelation>>(txtChildDataSource.Text);
                if (_listChildDiagram.Count == 0)
                {
                    return new List<LinkFlowChartService.FlowChartRelation>();
                }
                return _listChildDiagram.OrderBy(o => o.Level).ToList();
            }
            set { txtChildDataSource.Text = JsonConvert.SerializeObject(value); udpDataSource.Update(); }
        }

        private List<LinkFlowChartService.FlowChartRelation> _listDiagramDataSave;
        public List<LinkFlowChartService.FlowChartRelation> listDiagramDataSave
        {
            get 
            {

                _listDiagramDataSave = JsonConvert.DeserializeObject<List<LinkFlowChartService.FlowChartRelation>>(txtDataSave.Text);
                if (_listDiagramDataSave.Count == 0)
                {
                    return new List<LinkFlowChartService.FlowChartRelation>();
                }
                return _listDiagramDataSave.OrderBy(o => o.Level).ToList();
            }
            set { txtDataSave.Text = JsonConvert.SerializeObject(value); udpDataSource.Update(); }
        }


        public int MaximumItemInLevel
        {
            get
            {
                int countItems = 0;
                listChildDiagram.Select(s => s.Level).GroupBy(g => g).ToList().ForEach(r =>
                {
                    var xCountItems = listChildDiagram.Where(w => w.Level == r.Key).Count();
                    if (xCountItems > countItems)
                    {
                        countItems = xCountItems;
                    }
                });

                listParentDiagram.Select(s => s.Level).GroupBy(g => g).ToList().ForEach(r =>
                {
                    var xCountItems = listParentDiagram.Where(w => w.Level == r.Key).Count();
                    if (xCountItems > countItems)
                    {
                        countItems = xCountItems;
                    }
                });

                return countItems;
            }
        }

        public int MaximumLevel
        {
            get
            {
                if (listParentDiagram.Count == 0)
                {
                    if (listChildDiagram.Count == 0)
                    {
                        return 0;
                    }
                    return listChildDiagram.Max(m => m.Level);
                }

                if (listChildDiagram.Max(m => m.Level) > listParentDiagram.Max(m => m.Level))
                {
                    return listChildDiagram.Max(m => m.Level);
                }
                else
                {
                    return listParentDiagram.Max(m => m.Level);
                }

            }
        }

        public string nodeActive
        {
            get
            {
                return txtNodeActive.Text;
            }
            set
            {
                txtNodeActive.Text = value;
                udpDataSource.Update();
            }
        }

        public string URLNodeRedirect
        {
            get
            {
                if (String.IsNullOrEmpty(txtURLNodeRedirect.Text))
                {
                    return HttpContext.Current.Request.Url.AbsolutePath + "?id={#ID}";
                }
                return txtURLNodeRedirect.Text;
            }
            set
            {
                txtURLNodeRedirect.Text = value;
                udpDataSource.Update();
            }
        }

        public string RelationType
        {
            get
            {
                return txtRelationType.Text;
            }
            set
            {
                txtRelationType.Text = value;
                udpDataSource.Update();
            }
        }

        public string OtherKey
        {
            get
            {
                return txtOtherKey.Text;
            }
            set
            {
                txtOtherKey.Text = value;
                udpDataSource.Update();
            }
        }

        public bool AlowEditMode
        {

            get
            {
                return Convert.ToBoolean(txtAlowEditMode.Text);
            }
            set
            {
                txtAlowEditMode.Text = value.ToString();
                udpDataSource.Update();
            }
        }

        public string CssClass_Add
        {
            get
            {
                return txtCssClass_Add.Text;
            }
            set
            {
                txtCssClass_Add.Text = value;
                udpDataSource.Update();
            }
        }
        public bool RequiredRelation
        {

            get
            {
                return Convert.ToBoolean(txtRequiredRelationType.Text);
            }
            set
            {
                txtRequiredRelationType.Text = value.ToString();
                udpDataSource.Update();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //initFlowChartDiagram();
                bindDataDropdownEquipmentClass();
            }
        }

        public void initFlowChartDiagram()
        {
            ClientService.DoJavascript("initFlowChartDiagram();");
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