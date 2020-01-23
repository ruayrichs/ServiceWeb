using Agape.Lib.DBService;
using ERPW.Lib.Master;
using Newtonsoft.Json;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceWeb.API
{
    public partial class CostSheetAPI : System.Web.UI.Page
    {
        private CostCenterService lib = new CostCenterService();        

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";

            string ActionCase = Request["actionCase"];

            if (ActionCase == "get_price_uom")
            {
                GetBasePriceAndUnit(Request["material_code"]);
            }
            else if (ActionCase == "get_price")
            {
                GetBasePrice(Request["material_code"], Request["uom"]);
            }
        }

        private void GetBasePriceAndUnit(string materialCode)
        {           
            ERPW.Lib.Master.CostCenterService.MaterialModel en = lib.GetDefaultPriceAndUnit(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, materialCode);
           
            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(en);

            Response.Write(responseJson);
        }     

        private void GetBasePrice(string materialCode, string uom)
        {
            ERPW.Lib.Master.CostCenterService.MaterialModel en = new ERPW.Lib.Master.CostCenterService.MaterialModel();
            en.base_price = lib.GetBasePrice(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, materialCode, uom);

            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(en);

            Response.Write(responseJson);
        }

        public class AutoCompleteSource
        {
            public string code { get; set; }
            public string desc { get; set; }
            public string display { get; set; }
        }
    }
}