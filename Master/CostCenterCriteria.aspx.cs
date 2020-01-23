using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Master
{
    public partial class CostCenterCriteria : AbstractsSANWebpage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindDataCostCenter();
            }
        }

        private void bindDataCostCenter()
        {
            List<CostCenterEn> enList = serCostCenter.getListCostCenter(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                "",
                false
            );

            rptListData.DataSource = enList;
            rptListData.DataBind();
            ClientService.DoJavascript("initDataTable();");
        }
    }
}