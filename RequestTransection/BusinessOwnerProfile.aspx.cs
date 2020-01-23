using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;

namespace ServiceWeb.RequestTransection
{
    public partial class BusinessOwnerProfile : AbstractsSANWebpage
    {
        public bool IsFilterOwner
        {
            get { return (Master as ServiceWeb.MasterPage.MasterPage).IsFilterOwner; }
        }

        private AppClientLibrary appClientLib = new AppClientLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                initData();
            }
        }

        private void initData()
        {
            string owner = ERPWAuthentication.Permission.OwnerGroupCode + " : " + ERPWAuthentication.Permission.OwnerGroupName;

            LabelBusinessOwner.Text = owner;
            setInitDataCorporatePermissionKey();
            setInitDataApplicationPermissionKey();
            setInitDataRequestActivationApplication();
        }


        private void setInitDataApplicationPermissionKey()
        {
            string OwnerGroupCode = "";
            if (IsFilterOwner) OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;

            List<AppClientModel.ApplicationPermissionKeyModel> listAppPerKey = appClientLib.getDataApplicationPermissionKeyModel(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                OwnerGroupCode
            );
            rptListAppPerKeyItem.DataSource = listAppPerKey;
            rptListAppPerKeyItem.DataBind();
            udpnListAppPerKeyItems.Update();
            ClientService.DoJavascript("bindingDataTableJSAppPerKey();");
        }

        private void setInitDataCorporatePermissionKey()
        {
            string OwnerGroupCode = "";
            if (IsFilterOwner) OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;

            List<AppClientModel.CorporatePermissionKeyModel> listCopPerKey = appClientLib.getDataCorporatePermissionKeyModel(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                OwnerGroupCode
            );
            rptListCopPerKeyItems.DataSource = listCopPerKey;
            rptListCopPerKeyItems.DataBind();
            udpnListCopPerKeyItems.Update();
            ClientService.DoJavascript("bindingDataTableJSCopPerKey();");
            if (listCopPerKey.Count > 0)
            {
                AppClientModel.CorporatePermissionKeyModel lastcop = listCopPerKey.Last();
                LabelCopPerKey.Text = lastcop.CorporatePermissionKey;
            }
        }

        private void setInitDataRequestActivationApplication()
        {
            List<AppClientModel.RequestActivationModel> listReqAppPerKey = appClientLib.getDataRequestApplicationPermissionKeyModel(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode
            );
            if (IsFilterOwner)
            {
                listReqAppPerKey = listReqAppPerKey.Where(w =>
                    w.OwnerService == ERPWAuthentication.Permission.OwnerGroupCode
                ).ToList();
            }

            rptListReqActiveAppItems.DataSource = listReqAppPerKey;
            rptListReqActiveAppItems.DataBind();
            udpnListReqActiveAppItems.Update();
            ClientService.DoJavascript("bindingDataTableJSReqApp();");
        }

    }
}