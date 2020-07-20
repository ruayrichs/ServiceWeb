using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Constant;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using ServiceWeb.API.Model.Common;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class ConfigurationItemAPI : System.Web.UI.Page
    {
        private string Channel { get { return !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"]; } }
        private string PermissionKey { get { return !string.IsNullOrEmpty(Request["PermissionKey"]) ? Request["PermissionKey"] : Request.Headers["PermissionKey"]; } }
        
        #region Primary Key
        private string _SID;
        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = !string.IsNullOrEmpty(ERPWAuthentication.SID) ? ERPWAuthentication.SID : ERPWebConfig.GetSID(); // "555";
                return _SID;
            }
        }

        private string _CompanyCode;
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = !string.IsNullOrEmpty(ERPWAuthentication.CompanyCode) ? ERPWAuthentication.CompanyCode : ERPWebConfig.GetCompany(); // "INET";
                return _CompanyCode;
            }
        }

        private string _UserName;
        private string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_UserName))
                    _UserName = !string.IsNullOrEmpty(ERPWAuthentication.UserName) ? ERPWAuthentication.UserName : ""; // "focusone";
                return _UserName;
            }
        }

        private string _EmployeeCode;
        private string EmployeeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_EmployeeCode))
                    _EmployeeCode = !string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode) ? ERPWAuthentication.EmployeeCode : ""; // "focusone";
                return _EmployeeCode;
            }
        }
        #endregion
        private string CategoryCode { get { return Request["CategoryCode"]; } }

        #region data for master_equipment
        //private string EquipmentCode { get { return ""; } }
        //private string Reference { get { return Request["Reference"]; } }
        //private string Material { get { return Request["Material"]; } }
        private string Description { get { return Request["Description"]; } }
        private string Status { get { return Request["Status"]; } }
        private string Valid_From { get { return Request["Valid_From"]; } }
        private string Valid_To { get { return Request["Valid_To"]; } }
        private string EquipmentType { get { return Request["EquipmentType"]; } }
        //private string Province { get { return Request["Province"]; } }
        //private string Country { get { return Request["Country"]; } }
        //private string PicturePart { get { return Request["PicturePart"]; ; } }
        //private string ObjectID { get { return Request["ObjectID"]; ; } }
        //private string ActiveBy { get { return Request["ActiveBy"]; ; } }
        //private string ActiveDate { get { return Request["ActiveDate"]; ; } }
        //private string ActiveTime { get { return Request["ActiveTime"]; ; } }

        #endregion

        #region data for master_equipment_detail
        private string EquipmentClass { get { return Request["EquipmentClass"]; } }
        private string EquipmentObjectType { get { return Request["EquipmentObjectType"]; } }
        //private string AuthorizeGroup { get { return Request["AuthorizeGroup"]; } }
        private string Weight { get { return Request["Weight"]; } }
        //private string WeightUnit { get { return Request["WeightUnit"]; } }
        //private string SizeDimension { get { return Request["SizeDimension"]; } }
        //private string InventoryNO { get { return Request["InventoryNO"]; } }
        //private string StartupDate { get { return Request["StartupDate"]; } }
        //private string AcquisitionValue { get { return Request["AcquisitionValue"]; } }
        //private string AcquisitionDate { get { return Request["AcquisitionDate"]; } }
        //private string ManufacturerNO { get { return Request["ManufacturerNO"]; } }
        //private string ModelNumber { get { return Request["ModelNumber"]; } }
        //private string ManufacturerPartNO { get { return Request["ManufacturerPartNO"]; } }
        //private string ManufacturerSerialNO { get { return Request["ManufacturerSerialNO"]; } }
        //private string ManufacturerCountry { get { return Request["ManufacturerCountry"]; } }
        //private string ConstructYear { get { return Request["ConstructYear"]; } }
        //private string ConstructMonth { get { return Request["ConstructMonth"]; } }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonResponseModel response = new CommonResponseModel();
            try
            {
                checkPermission(PermissionKey);
                validateData();

                if (Channel == TierZeroLibrary.TIER_ZERO_CHANNEL_WEB)
                {
                    ConfigurationItemModel ciModel = new ConfigurationItemModel();
                    ciModel.sid = SID;
                    ciModel.companyCode = CompanyCode;
                    ciModel.categoryCode = CategoryCode;
                    ciModel.reference = "";
                    ciModel.material = "";
                    ciModel.created_by = EmployeeCode;
                    ciModel.description = Description;
                    ciModel.status = Status;
                    ciModel.valid_From = Valid_From;
                    ciModel.valid_To = Valid_To;
                    ciModel.equipmentType = EquipmentType;
                    ciModel.province = "";
                    ciModel.country = "";
                    ciModel.picturePart = "";
                    ciModel.objectID = "";
                    ciModel.activeBy = "";
                    ciModel.activeDate = "";
                    ciModel.activeTime = "";

                    ConfigurationItemGeneralModel ciGeModel = new ConfigurationItemGeneralModel();
                    ciGeModel.sid = SID;
                    ciGeModel.companyCode = CompanyCode;
                    ciGeModel.categoryCode = CategoryCode;
                    ciGeModel.equipmentClass = EquipmentClass;
                    ciGeModel.equipmentObjectType = EquipmentObjectType;
                    ciGeModel.authorizeGroup = "";
                    ciGeModel.weight = Weight;
                    ciGeModel.weightUnit = "";
                    ciGeModel.sizeDimension = "";
                    ciGeModel.inventoryNO = "";
                    ciGeModel.startupDate = "";
                    ciGeModel.acquisitionValue = "";
                    ciGeModel.acquisitionDate = "";
                    ciGeModel.manufacturerNO = "";
                    ciGeModel.modelNumber = "";
                    ciGeModel.manufacturerPartNO = "";
                    ciGeModel.manufacturerSerialNO = "";
                    ciGeModel.manufacturerCountry = "";
                    ciGeModel.constructYear = "";
                    ciGeModel.constructMonth = "";
                    ciGeModel.created_by = EmployeeCode;

                    string equipmentCpde = ConfigurationItemLib.GetInstance().insertConfigurationItem(ciModel, ciGeModel);

                    response.resultCode = ConfigurationConstant.API_RESULT_CODE_SUCCESS;
                    response.message = "Insert Success, Configuration Item Code: " + equipmentCpde;
                    response.resultTime = DateTime.Now.ToString();
                }

            } catch (Exception ex)
            {
                response.resultCode = ConfigurationConstant.API_RESULT_CODE_ERROR;
                response.message = ex.Message;
                response.resultTime = DateTime.Now.ToString();
            }
            string srResponse = JsonConvert.SerializeObject(response);
            Response.Write(srResponse);

        }

        private void validateData()
        {
            if (String.IsNullOrEmpty(CategoryCode)) throw new Exception("Please input CategoryCode.");

            #region validate master_equipment
            if (String.IsNullOrEmpty(Description)) throw new Exception("Please input Description.");
            if (String.IsNullOrEmpty(Status)) throw new Exception("Please input Status.");
            if (String.IsNullOrEmpty(Valid_From)) throw new Exception("Please input Valid_From.");
            if (String.IsNullOrEmpty(Valid_To)) throw new Exception("Please input Valid_To.");
            if (String.IsNullOrEmpty(EquipmentType)) throw new Exception("Please input EquipmentType.");
            #endregion

            #region validate master_equipment_general
            if (String.IsNullOrEmpty(EquipmentClass)) throw new Exception("Please input EquipmentObjectType.");
            if (String.IsNullOrEmpty(EquipmentObjectType)) throw new Exception("Please input EquipmentObjectType.");
            if (String.IsNullOrEmpty(Weight)) throw new Exception("Please input Weight.");
            #endregion
        }

        #region Properties Data 
        private void checkPermission(string PermissionKey)
        {
            bool HasPermission = false;
            ERPW_API_Permission_Token_Key_DAO libPermission = new ERPW_API_Permission_Token_Key_DAO();
            DataTable dtPermission = libPermission.getOneByKey(PermissionKey);
            if (dtPermission.Rows.Count > 0)
            {
                _SID = dtPermission.Rows[0]["SID"].ToString();
                _CompanyCode = dtPermission.Rows[0]["CompanyCode"].ToString();
                _EmployeeCode = dtPermission.Rows[0]["EmployeeCode"].ToString();
                _UserName = dtPermission.Rows[0]["UserName"].ToString();

            }

            if (string.IsNullOrEmpty(ERPWAuthentication.UserName))
            {
                SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");
                ERPWAutoLoginService loginService = new ERPWAutoLoginService(_SID, _UserName, mode);
                HasPermission = true;
            }
            else if (!string.IsNullOrEmpty(_UserName) && !string.IsNullOrEmpty(ERPWAuthentication.UserName))
            {
                HasPermission = true;
            }

            if (!HasPermission)
            {
                throw new Exception("No Permission.");
            }
        }
        #endregion
    }
}