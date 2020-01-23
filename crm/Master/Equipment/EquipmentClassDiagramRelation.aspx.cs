using ERPW.Lib.Authentication;
using ServiceWeb.auth;
using ServiceWeb.LinkFlowChart.Service;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Equipment
{
    public partial class EquipmentClassDiagramRelation : AbstractsSANWebpage
    {
        EquipmentService serEquipment = new EquipmentService();
        private string ClassCode
        {
            get
            {
                if (string.IsNullOrEmpty(Request["id"]))
                {
                    //return "Root";
                }
                return Request["id"];
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
            LinkFlowChartService.DiagramRelation dataEn = new LinkFlowChartService.DiagramRelation();
            if (string.IsNullOrEmpty(ClassCode))
            {
                dataEn.parentNode = new List<LinkFlowChartService.FlowChartRelation>();
                dataEn.chindNode = new List<LinkFlowChartService.FlowChartRelation>();
                DataTable dt = serEquipment.getEMClass(ERPWAuthentication.SID);
                List<LinkFlowChartService.FlowChartRelation> listRelation = LinkFlowChartService.getDiagramRelationList(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    LinkFlowChartService.ItemGroup_CLASS
                );

                foreach (DataRow dr in dt.Rows)
                {
                    int xCountIsParent = listRelation.Where(w =>
                        w.ParentItemCode.Equals(dr["ClassCode"].ToString())
                    ).Count();
                    int xCountIsChild = listRelation.Where(w =>
                        w.ItemCode.Equals(dr["ClassCode"].ToString())
                    ).Count();

                    bool IsDiagram = xCountIsParent + xCountIsChild > 0;

                    if (IsDiagram)
                    {
                        if (xCountIsParent > 0 && xCountIsChild == 0)
                        {
                            dataEn.chindNode.AddRange(
                                    LinkFlowChartService.getDiagramRelation(
                                     ERPWAuthentication.SID,
                                     ERPWAuthentication.CompanyCode,
                                     dr["ClassCode"].ToString(),
                                     LinkFlowChartService.ItemGroup_CLASS
                                 ).chindNode
                             );
                        }
                    }
                    else
                    {
                        dataEn.chindNode.Add(new LinkFlowChartService.FlowChartRelation
                        {
                            ItemCode = dr["ClassCode"].ToString(),
                            ItemDescription = dr["ClassName"].ToString(),
                            ItemGroup = LinkFlowChartService.ItemGroup_CLASS.Value,
                            Level = 0,
                            ParentItemCode = "",
                            RelationCode = "",
                            RelationDesc = ""
                        });
                    }
                    dr["ClassCode"].ToString();
                }
            }
            else
            {
                dataEn = LinkFlowChartService.getDiagramRelation(
                     ERPWAuthentication.SID,
                     ERPWAuthentication.CompanyCode,
                     ClassCode,
                     LinkFlowChartService.ItemGroup_CLASS
                 );
            }


            if (dataEn.parentNode.Count + dataEn.chindNode.Count > 0)
            {
                FlowChartDiagramRelationControl.AlowEditMode = !string.IsNullOrEmpty(ClassCode); //EquipmentCode;
                FlowChartDiagramRelationControl.nodeActive = ClassCode; //EquipmentCode;
                FlowChartDiagramRelationControl.listParentDiagram = dataEn.parentNode;
                FlowChartDiagramRelationControl.listChildDiagram = dataEn.chindNode;
                FlowChartDiagramRelationControl.initFlowChartDiagram();
            }
        }

        protected void saveRelation_Click(object sender, EventArgs e)
        {
            try
            {
                List<LinkFlowChartService.FlowChartRelation> listDiagramDataSave = FlowChartDiagramRelationControl.listDiagramDataSave;
                listDiagramDataSave.ForEach(r =>
                {

                });


                LinkFlowChartService.updateDataDiagram(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    LinkFlowChartService.ItemGroup_CLASS,
                    ClassCode, //EquipmentCode,
                    listDiagramDataSave,
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