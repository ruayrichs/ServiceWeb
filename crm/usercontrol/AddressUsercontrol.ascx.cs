using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using SNA.Lib.crm;
using SNA.Lib.crm.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSWeb.crm.usercontrol
{
    public partial class AddressUsercontrol : System.Web.UI.UserControl
    {
        private CRMService serviceCRM = CRMService.getInstance();

        
        private List<LabelValueBean> listAddressType
        {
            get
            {
                return serviceCRM.searchCusAddressType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            }
        }
        public String bpcode
        {
            get
            {
                return Request.QueryString["id"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                bindDataAddress();
                bindAddressList();
            }
            //ClientService.AGLoading(false);
        }

        private void bindAddressList()
        {
            ContactCustomer.AddressInfo AddressDetailUscl = serviceCRM.getCustomerAddress(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                bpcode,
                ""
                );

            rptAddAddressModal.DataSource = AddressDetailUscl.listAddressProperty;
            rptAddAddressModal.DataBind();

            ddlTypeAddressModalAddNew.DataTextField = "Text";
            ddlTypeAddressModalAddNew.DataValueField = "Value";
            ddlTypeAddressModalAddNew.DataSource = listAddressType;
            ddlTypeAddressModalAddNew.DataBind();
        }

        private void bindDataAddress()
        {
            List<ContactCustomer.AddressInfo> listAdress = serviceCRM.searchCustomerAddress(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                bpcode);

            rptAddressModal.DataSource = listAdress;
            rptAddressModal.DataBind();
            udpAddressModal.Update();
        }

        protected void rptAddressModal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hddAddressCode = (HiddenField)e.Item.FindControl("hddAddressModalCode");
            Repeater rptDetail = (Repeater)e.Item.FindControl("rptDetail");
            Repeater rptAddressEdit = (Repeater)e.Item.FindControl("rptAddressModalEdit");

            ContactCustomer.AddressInfo AddressDetail = serviceCRM.getCustomerAddress(
                               ERPWAuthentication.SID,
                               ERPWAuthentication.CompanyCode,
                               bpcode,
                               hddAddressCode.Value
                               );

           //var ListAddr = AddressDetail.listAddressProperty.Where(a => a.AddressCode.Equals(hddAddressCode.Value)).ToList();

           rptDetail.DataSource = AddressDetail.listAddressProperty;
            rptDetail.DataBind();

            rptAddressEdit.DataSource = AddressDetail.listAddressProperty;
            rptAddressEdit.DataBind();
        }



        public class Address
        {
            public string PropertyCode { get; set; }
            public string Description { get; set; }
            public string PropertyValue { get; set; }
        }
        public class AddressListDetail
        {
            public List<Address> address { get; set; }
            
        }

        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {
                List<AddressListDetail> addressDetail = new JavaScriptSerializer().Deserialize<List<AddressListDetail>>(hddJsonAddressModal.Value);
                ContactCustomer.AddressInfo listAddress = serviceCRM.getCustomerAddress(
                               ERPWAuthentication.SID,
                               ERPWAuthentication.CompanyCode,
                               bpcode,
                               hddAddressModalCodeEdit.Value);

                foreach (var item in addressDetail)
                {
                    for (int i = 0; i < listAddress.listAddressProperty.Count; i++)
                    {
                        listAddress.listAddressProperty[i].PropertyValue = item.address[i].PropertyValue;
                    }

                    serviceCRM.createNewCustomerAddress(ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        bpcode,
                        ERPWAuthentication.EmployeeCode,
                        hddAddressModalCodeEdit.Value,
                        listAddress.listAddressProperty
                        );

                }

                hddJsonAddressModal.Value = "";
                bindDataAddress();

                hddJsonAddressModal.Value = "";
                bindDataAddress();
                ClientService.AGSuccess("บันทึกสำเร็จ");
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

        protected void btnEditMOdal_Click(object sender, EventArgs e)
        {
            try
            {
                List<AddressListDetail> addressDetail = new JavaScriptSerializer().Deserialize<List<AddressListDetail>>(hddJsonAddressModal.Value);

                ContactCustomer.AddressInfo listAddress = serviceCRM.getCustomerAddress(
                   ERPWAuthentication.SID,
                   ERPWAuthentication.CompanyCode,
                   bpcode,
                   hddAddressModalCodeEdit.Value);

                foreach (var item in addressDetail)
                {
                    for (int i = 0; i < listAddress.listAddressProperty.Count; i++)
                    {
                        listAddress.listAddressProperty[i].PropertyValue = item.address[i].PropertyValue;
                    }

                    serviceCRM.updateCustomerAddress(ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        bpcode,
                        ERPWAuthentication.EmployeeCode,
                        hddAddressModalCodeEdit.Value,
                        listAddress.listAddressProperty
                        );

                }

                hddJsonAddressModal.Value = "";
                bindDataAddress();
                ClientService.AGSuccess("แก้ไขสำเร็จ");
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

    }
}