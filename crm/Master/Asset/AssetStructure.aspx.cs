
using Newtonsoft.Json;
using ServiceWeb.Asset.API.Class;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using SNA.Lib.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Asset
{
    public partial class AssetStructure : AbstractsSANWebpage //System.Web.UI.Page
    {
        AssetStructureModel ServiceAssetStructure = new AssetStructureModel();
        private AssetService assetService = new AssetService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindDataListMat();
            }
        }

        private void bindDataListMat()
        {
            DataTable dtDataSearch = ServiceAssetStructure.getListDataAsset(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode
            );


            string Json = JsonConvert.SerializeObject(dtDataSearch);
            List<DataAssetDetail> listAssetDetail = JsonConvert.DeserializeObject<List<DataAssetDetail>>(Json);

            List<AssetStructureModel.EnAssetStructureModel> listAssetStructure = AssetStructureModel.GetProductCategory(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode
            );

            var data = listAssetDetail.Where(w =>
                !listAssetStructure.Select(s =>
                    s.StructureCode
                ).ToList().Contains(w.AssetCode + w.AssetSubCode)
            );

            rptListMat.DataSource = data;
            rptListMat.DataBind();
            udpListMat.Update();
        }

        protected void btnAddMatStructure_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceAssetStructure.AddAssetStructure(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    hddAssetCode.Value,
                    hddAssetSubCode.Value,
                    "",
                    hddAssetParentCode.Value,
                    Convert.ToInt32(hddAssetParentNoteLevel.Value) + 1,
                    "",
                    ERPWAuthentication.EmployeeCode
                );
                bindDataListMat();
                ClientService.DoJavascript("bindHierarchy(true);");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnRebindListAsset_Click(object sender, EventArgs e)
        {
            try
            {
                bindDataListMat();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        public class DataAssetDetail
        {
            public string AssetGroup { get; set; }
            public string AssetGroupName { get; set; }
            public string AssetType { get; set; }
            public string AssetTypeName { get; set; }
            public string AssetCode { get; set; }
            public string AssetSubCode { get; set; }
            public string AssetSubCodeDescription { get; set; }
            public double NetValue { get; set; }
            public string CURRENCYCODE { get; set; }
            public string AssetOwner { get; set; }
            public string FirstName_TH { get; set; }
            public string LastName_TH { get; set; }
            public string AssetStatus { get; set; }
        }

    }
}