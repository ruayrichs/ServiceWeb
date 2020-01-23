using ERPW.Lib.Authentication;
using SNA.Lib.crm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSWeb.crm.usercontrol
{
    public partial class WorkStreamConfigControl : System.Web.UI.UserControl
    {
        private CRMService crmService = CRMService.getInstance();
        public string Mode
        {
            // mode string = 'contact'
            // mode string = 'customer'
            get;
            set;
        }
        public string ContactCode
        {
            get
            {
                return hddCodeContact.Value;
            }
            set
            {
                hddCodeContact.Value = value;
                udpDataCode.Update();
            }
        }
        public string WorkGroupCode
        {
            get
            {
                return hddWorkGroupCode.Value;
            }
            set
            {
                hddWorkGroupCode.Value = value;
                udpDataCode.Update();
            }
        }

        private const string modeContact = "contact";
        private const string modeCustomer = "customer";

        private DataTable dtListHeirarchy_;
        private DataTable dtListHeirarchy
        {
            get
            {
                if (dtListHeirarchy_ == null)
                {
                    dtListHeirarchy_ = crmService.getHierarchyStructure(
                        ERPWAuthentication.SID,
                        WorkGroupCode,
                        null
                    );
                }
                return dtListHeirarchy_;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindDDLListHeirarchy();
                bindListStructureRef();
            }
        }

        private void bindDDLListHeirarchy()
        {
            ddlListHierarchy.DataValueField = "DataValue";
            ddlListHierarchy.DataTextField = "DataText";

            ddlListHierarchy.DataSource = dtListHeirarchy;
            ddlListHierarchy.DataBind();
            ddlListHierarchy.Items.Insert(0, new ListItem("กรุณาเลือก", ""));
        }

        private void bindListStructureRef()
        {
            DataTable dt = new DataTable();
            if (Mode.Equals(modeContact))
            {
                dt = crmService.getStructureRefContact(
                    ERPWAuthentication.SID, 
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    ContactCode
                );
            }
            else if (Mode.Equals(modeCustomer))
            {
                dt = crmService.getStructureRefCustomer(
                   ERPWAuthentication.SID,
                   ERPWAuthentication.CompanyCode,
                   WorkGroupCode,
                   ContactCode
               );
            }

            rptListRefStructure.DataSource = dt;
            rptListRefStructure.DataBind();
            udpListHierarchyMapping.Update();
        }

        public string bindHeirarchy(string StructureCode)
        {
            string Heirarchy = "";
            string ICon = "<i class='fa fa-level-up fa-fw' style='transform: rotate(-270deg);'></i>";

            foreach (DataRow dr in dtListHeirarchy.Select("DataValue = '" + StructureCode + "'"))
            {
                string[] listHeirarchy = dr["DataText"].ToString().Replace("-->", ">").Split('>');
                int i = 0;
                foreach (string str in listHeirarchy)
                {
                    Heirarchy += "<div style='margin-left:" + (i * 8) + "px;'>" + ICon + str + "</div>";
                    i++;
                }
            }

            return Heirarchy;
        }

        protected void btnSaveStructure_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlListHierarchy.SelectedValue))
                {
                    throw new Exception("กรุณาเลือกโครงสร้างการติดต่อ");
                }
                if (Mode.Equals(modeContact))
                {
                    crmService.SaveStructureRefContact(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        ddlListHierarchy.SelectedValue,
                        ContactCode,
                        ERPWAuthentication.EmployeeCode
                    );
                }
                else if (Mode.Equals(modeCustomer))
                {
                    crmService.SaveStructureRefCustomer(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        ddlListHierarchy.SelectedValue,
                        ContactCode,
                        ERPWAuthentication.EmployeeCode
                   );
                }

                bindListStructureRef();
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

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string CompanyStructureCode = (sender as Button).CommandArgument;

                if (Mode.Equals(modeContact))
                {
                    crmService.DeleteStructureRefContact(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        CompanyStructureCode,
                        ContactCode
                    );
                }
                else if (Mode.Equals(modeCustomer))
                {
                    crmService.DeleteStructureRefCustomer(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        CompanyStructureCode,
                        ContactCode
                   );
                }

                bindListStructureRef();
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
    }
}