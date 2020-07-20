using Agape.Lib.DBService;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service.API.v1;
using ERPW.Lib.Service.Entity.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v2
{
    public partial class CustomerDetailAPI : AbstractWebAPI
    {
        DBService db = new DBService();
        private CustomerService serviceCustomer = CustomerService.getInstance();
        private MasterConfigLibrary config = MasterConfigLibrary.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                initData();
            }
            catch(Exception ex)
            {
                BadRequest(ex.Message);
            }
        }

        //method here
        private void initData()
        {
            string responseJson = "";
            string CustomerCode = !string.IsNullOrEmpty(Request["CustomerCode"]) ? Request["CustomerCode"] : Request.Headers["CustomerCode"];
            List<String> listResponse = new List<string>();
            Boolean LoginByPermission = false;
            if (string.IsNullOrEmpty(_EmployeeCode))
            {
                LoginByPermission = true;
            }

            if (!checkPermission())
            {
                responseJson = "{\"message\":\"invalid permission key\"}";
                Response.Write(responseJson);
                return;
            }

            Response.ContentType = "application/json";
            #region to do
            //CustomerDashboardFinalDataModel customerDashboardFinalDataModel = new CustomerDashboardFinalDataModel();
            //customerDashboardFinalDataModel = customerDashboardLib.PreparFinanDataDashboard(SID, CompanyCode, CustomerCode);
            //responseJson = JsonConvert.SerializeObject(customerDashboardFinalDataModel);
            CustomerProfileLib customerProfileLib = new CustomerProfileLib(_SID, _CompanyCode, CustomerCode);
            ClientDetail dataClientDetail = customerProfileLib.finalDateModelClientDetail(_SID, _CompanyCode, CustomerCode);
            CustomerDetailV2 dataClientDetailV2 = JsonConvert.DeserializeObject<CustomerDetailV2>(JsonConvert.SerializeObject(dataClientDetail));

            CustomerProfile CustomerProfile = serviceCustomer.SearchCustomerDataByCustomerCode(
                        _SID,
                        _CompanyCode,
                        CustomerCode
                    );

            dataClientDetailV2.responsible_organization = CustomerProfile.ResponsibleOrganization;
            dataClientDetailV2.critical = CustomerProfile.CustomerCritical;
            dataClientDetailV2.status = CustomerProfile.Active;
            dataClientDetailV2.foreign_name = CustomerProfile.ForeignName;
            dataClientDetailV2.OwnerService = new List<OwnerService>();

            DataTable dt = CustomerOwner(_SID, _CompanyCode, CustomerCode);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataClientDetailV2.OwnerService.Add(new OwnerService()
                {
                    Code = Convert.ToString(dt.Rows[i]["code"]),
                    Description = Convert.ToString(dt.Rows[i]["description"])
                });
            }

            var json = new JavaScriptSerializer().Serialize(dataClientDetailV2);
            Response.Write(json);
            //Response.Write(responseJson);
            #endregion
            if (LoginByPermission)
            {
                Session.Abandon();
            }
        }

        public DataTable CustomerOwner(string sid, string CompanyCode, string CustomerCode)
        {
            string Owner = "";
            if (!string.IsNullOrEmpty(CustomerCode))
            {
                Owner = " AND owner_a.OwnerCode = '" + db.escapeSingleQuote(CustomerCode.Trim()) +"'";
            }
            string sql = @"
                            select ISNULL(ge.EquipmentObjectType,'') as code
                            ,ISNULL(Owner.OwnerGroupName,'') as description
                            from master_equipment_owner_assignment owner_a with(nolock)

                            left join master_equipment Equipment  with(nolock)
                                on owner_a.SID = Equipment.SID
                                and owner_a.CompanyCode = Equipment.CompanyCode
                                and owner_a.EquipmentCode = Equipment.EquipmentCode

                            left join master_equipment_general ge  with(nolock)
                                on ge.sid = Equipment.sid 
                                and ge.companyCode = Equipment.companycode
                                and Equipment.EquipmentCode = ge.EquipmentCode

                            LEFT JOIN ERPW_OWNER_GROUP AS [Owner]  with(nolock)
                            ON ge.SID = Owner.SID and ge.CompanyCode = Owner.CompanyCode
	                            AND ge.EquipmentObjectType = Owner.OwnerGroupCode

                            where owner_a.SID = '"+ sid + @"' 
                                and owner_a.CompanyCode = '"+ CompanyCode + @"'
                                and owner_a.OwnerType = '01'
                                "+ Owner + @"
                            group by ISNULL(ge.EquipmentObjectType,'') ,ISNULL(Owner.OwnerGroupName,'') 
                            order by ISNULL(ge.EquipmentObjectType,'') asc
                            ";
            DataTable dt = db.selectDataFocusone(sql);
            return dt;
        }

        public class CustomerDetailV2 : ClientDetail
        {
            public string responsible_organization { get; set; }
            public string critical { get; set; }
            public string status { get; set; }
            public string foreign_name { get; set; }
            public List<OwnerService> OwnerService { get; set; }
        }
        public class OwnerService
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }
    }
}