using ERPW.Lib.Authentication;
using ServiceWeb.auth;
using ServiceWeb.LinkFlowChart.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Equipment
{
    public partial class EquipmentDiagramRelation : AbstractsSANWebpage
    {
        private string EquipmentCode
        {
            get
            {
                return Request["id"];
            }
        }

        private bool IsNotRedirect
        {
            get 
            {
                bool isRe = false;
                bool.TryParse(Request["noRedirect"], out isRe);
                return isRe;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getDataRelation();
            }
        }

        private void getDataRelation()
        {
            LinkFlowChartService.DiagramRelation dataEn = LinkFlowChartService.getDiagramRelation(
                 ERPWAuthentication.SID,
                 ERPWAuthentication.CompanyCode,
                 EquipmentCode, //"203.151.94.199",
                 LinkFlowChartService.ItemGroup_EQUIPMENT
             );

            if (dataEn.parentNode.Count + dataEn.chindNode.Count > 0)
            {
                if (IsNotRedirect)
                {
                    FlowChartDiagramRelationControl.URLNodeRedirect = "#";
                }
                FlowChartDiagramRelationControl.nodeActive = EquipmentCode;
                FlowChartDiagramRelationControl.listParentDiagram = dataEn.parentNode;
                FlowChartDiagramRelationControl.listChildDiagram = dataEn.chindNode;
                FlowChartDiagramRelationControl.initFlowChartDiagram();
            }
        }

        protected void saveRelation_Click(object sender, EventArgs e)
        {
            try
            {
                LinkFlowChartService.updateDataDiagram(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    LinkFlowChartService.ItemGroup_EQUIPMENT,
                    EquipmentCode,
                    FlowChartDiagramRelationControl.listDiagramDataSave,
                    ERPWAuthentication.EmployeeCode
                );

                getDataRelation();
                ClientService.AGSuccess("บันทึกสำเร็จ");
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
    }
}