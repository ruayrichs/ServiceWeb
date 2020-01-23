using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Master.Entity;

namespace ServiceWeb.MasterConfig
{
    public partial class EquipmentLocationMaster : System.Web.UI.Page
    {
        MasterConfigLibrary libMasConf = new MasterConfigLibrary();
        LocationDataEn locationDataCopy = new LocationDataEn();
        LocationDataEn locationData = new LocationDataEn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dataBinding();
            }

        }

      
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            var rowIndex = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            // ClientService.AGError(tableDataLocation.Rows[rowIndex].Cells[0].Text); 
            DataTable data = libMasConf.GetERPWLocationEquipmentMasterByLocationCode(tableDataLocation.Rows[rowIndex].Cells[0].Text);
            foreach (DataRow dataRow in data.Rows)
            {
                /* System.Diagnostics.Debug.WriteLine(txtbox.Text);
                 txtBoxPlant.Text = dataRow.ItemArray[4].ToString();*/
                //txtBoxPlant.Text ="sdadsdasd";
                txtBoxLocationName.Text = dataRow["LocationName"].ToString();
                txtBoxAddressName1.Text = dataRow["AddressName1"].ToString();
                txtBoxAddressName2.Text = dataRow["AddressName2"].ToString();
                txtBoxCity.Text = dataRow["AddressCity"].ToString();
                txtBoxFax.Text = dataRow["AddressFax"].ToString();
                txtBoxLocation.Text = dataRow["Location"].ToString();
                txtBoxLocationCategory.Text = dataRow["LocationCategory"].ToString();
                txtBoxPlant.Text = dataRow["Plant"].ToString();
                txtBoxRoom.Text = dataRow["Room"].ToString();
                txtBoxShelf.Text = dataRow["Shelf"].ToString();
                txtBoxSlot.Text = dataRow["Slot"].ToString();
                txtBoxStreet.Text = dataRow["AddressStreet"].ToString();
                txtBoxTelephone.Text = dataRow["AddressTelephone"].ToString();
                txtBoxWorkCenter.Text = dataRow["WorkCenter"].ToString();
                txtBoxZipcode.Text = dataRow["AddressZipCode"].ToString();

            }
            btnNew.Visible = true;
            btnSave.Visible = false;
            udpMasterConfig.Update();
            tableDataLocation.HeaderRow.TableSection = TableRowSection.TableHeader;

            btnDataLocationPopup.Update();
            btnButtonLocationPopup.Update();

            ClientService.DoJavascript("openModal('myModal');");
            ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                locationData = new LocationDataEn();
                locationData.addressName1 = txtBoxAddressName1.Text.Trim();
                locationData.addressName2 = txtBoxAddressName2.Text.Trim();
                locationData.city = txtBoxCity.Text.Trim();
                locationData.fax = txtBoxFax.Text.Trim();
                locationData.location = txtBoxLocation.Text.Trim();
                locationData.locationCategory = txtBoxLocationCategory.Text.Trim();
                locationData.plant = txtBoxPlant.Text.Trim();
                locationData.room = txtBoxRoom.Text.Trim();
                locationData.shelf = txtBoxShelf.Text.Trim();
                locationData.slot = txtBoxSlot.Text.Trim();
                locationData.street = txtBoxStreet.Text.Trim();
                locationData.telePhone = txtBoxTelephone.Text.Trim();
                locationData.workCenter = txtBoxWorkCenter.Text.Trim();
                locationData.zipCode = txtBoxZipcode.Text.Trim();

                libMasConf.AddERPWLocationEquipmentMaster(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, locationData);
                dataBinding();
                ClientService.AGSuccess("สำเร็จ");
                ClientService.DoJavascript("closeModal('myModal');");

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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                locationData = new LocationDataEn();
                locationData.locationName = txtBoxLocationName.Text.Trim();
                locationData.addressName1 = txtBoxAddressName1.Text.Trim();
                locationData.addressName2 = txtBoxAddressName2.Text.Trim();
                locationData.city = txtBoxCity.Text.Trim();
                locationData.fax = txtBoxFax.Text.Trim();
                locationData.location = txtBoxLocation.Text.Trim();
                locationData.locationCategory = txtBoxLocationCategory.Text.Trim();
                locationData.plant = txtBoxPlant.Text.Trim();
                locationData.room = txtBoxRoom.Text.Trim();
                locationData.shelf = txtBoxShelf.Text.Trim();
                locationData.slot = txtBoxSlot.Text.Trim();
                locationData.street = txtBoxStreet.Text.Trim();
                locationData.telePhone = txtBoxTelephone.Text.Trim();
                locationData.workCenter = txtBoxWorkCenter.Text.Trim();
                locationData.zipCode = txtBoxZipcode.Text.Trim();

                libMasConf.UpdateERPWLocationEquipment(ERPWAuthentication.CompanyCode,txtBoxLocationId.Text ,locationData);
                dataBinding();
                ClientService.AGSuccess("สำเร็จ");
                ClientService.DoJavascript("closeModal('myModal');");
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var rowIndex = ((GridViewRow)((Control)sender).NamingContainer).RowIndex; 
                string locationCode = tableDataLocation.Rows[rowIndex].Cells[0].Text;
                libMasConf.DeleteERPWLocationEquipmentMaster(locationCode);
                dataBinding();
                ClientService.AGSuccess("สำเร็จ");
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

        protected void btnNew_Click(object sender, EventArgs e)
        {
           
            txtBoxAddressName1.Text = "";
            txtBoxAddressName2.Text = "";
            txtBoxCity.Text = "";
            txtBoxFax.Text = "";
            txtBoxLocation.Text = "";
            txtBoxLocationCategory.Text = "";
            txtBoxPlant.Text = "";
            txtBoxRoom.Text = "";
            txtBoxShelf.Text = "";
            txtBoxSlot.Text = "";
            txtBoxStreet.Text = "";
            txtBoxTelephone.Text = "";
            txtBoxWorkCenter.Text = "";
            txtBoxZipcode.Text = "";
            
            btnNew.Visible = true;
            btnSave.Visible = false;
            udpMasterConfig.Update();
            if (tableDataLocation.Rows.Count > 0)
            {
                tableDataLocation.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            btnDataLocationPopup.Update();
            btnButtonLocationPopup.Update();

            ClientService.DoJavascript("openModal('myModal');");
            ClientService.DoJavascript("bindingDataTableJS();");
        }

       
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            var rowIndex = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            // ClientService.AGError(tableDataLocation.Rows[rowIndex].Cells[0].Text); 
            string locationCode = tableDataLocation.Rows[rowIndex].Cells[0].Text;
            DataTable data = libMasConf.GetERPWLocationEquipmentMasterByLocationCode(locationCode);
            foreach (DataRow dataRow in data.Rows)
            {
                /* System.Diagnostics.Debug.WriteLine(dataRow.ItemArray[4]);
                 txtBoxPlant.Text = dataRow.ItemArray[4].ToString();*/
                //txtBoxPlant.Text ="sdadsdasd";
                txtBoxLocationId.Text = dataRow["LocationCode"].ToString();
                txtBoxLocationName.Text = dataRow["LocationName"].ToString();
                txtBoxAddressName1.Text = dataRow["AddressName1"].ToString();
                txtBoxAddressName2.Text = dataRow["AddressName2"].ToString();
                txtBoxCity.Text = dataRow["AddressCity"].ToString();
                txtBoxFax.Text = dataRow["AddressFax"].ToString();
                txtBoxLocation.Text = dataRow["Location"].ToString();
                txtBoxLocationCategory.Text = dataRow["LocationCategory"].ToString();
                txtBoxPlant.Text = dataRow["Plant"].ToString();
                txtBoxRoom.Text = dataRow["Room"].ToString();
                txtBoxShelf.Text = dataRow["Shelf"].ToString();
                txtBoxSlot.Text = dataRow["Slot"].ToString();
                txtBoxStreet.Text = dataRow["AddressStreet"].ToString();
                txtBoxTelephone.Text = dataRow["AddressTelephone"].ToString();
                txtBoxWorkCenter.Text = dataRow["WorkCenter"].ToString();
                txtBoxZipcode.Text = dataRow["AddressZipCode"].ToString();

            }
            btnNew.Visible = false;
            btnSave.Visible = true;
            udpMasterConfig.Update();
            if (tableDataLocation.Rows.Count > 0)
            {
                tableDataLocation.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            btnDataLocationPopup.Update();
            btnButtonLocationPopup.Update();

            ClientService.DoJavascript("openModal('myModal');");
            ClientService.DoJavascript("bindingDataTableJS();");
        }
       
       
       
        private void dataBinding()
        {
            DataTable datatable;
            datatable = libMasConf.GetERPWLocationEquipmentMaster(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            tableDataLocation.DataSource = datatable;
            tableDataLocation.DataBind();
            udpMasterConfig.Update();

            if (tableDataLocation.Rows.Count > 0)
            {
                tableDataLocation.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            ClientService.DoJavascript("bindingDataTableJS();");
            /* tableData.DataSource = datatable;
            tableData.DataBind();
            
            udpMasterConfig.Update();*/
        }
    }
}