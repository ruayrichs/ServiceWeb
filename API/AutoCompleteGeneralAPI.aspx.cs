using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using Newtonsoft.Json;
using SNA.Lib.POS.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API
{
    public partial class AutoCompleteGeneralAPI : System.Web.UI.Page
    {
        private DBService dbService = new DBService();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";

            string ActionCase = Request["actionCase"];
            switch (Request["actionCase"])
            {
                case "costcenter": GetCostCenterMaster(Request["keySearch"]);
                    break;
                case "location" : GetAssetLocation1(Request["keySearch"]);
                    break;
                case "ci_class": GetMasterCI_Class(Request["keySearch"]);
                    break;
                case "ci_family": GetMasterCI_Family(Request["keySearch"]);
                    break;
                case "ci_ownersevice": GetMasterCI_Ownersevice(Request["keySearch"]);
                    break;
                case "employee": GetEmployeeData("");
                    break;
                case "employee_not_user": GetEmployeeData("USERNAME_EMPTY");
                    break;
                default :
                    break;
            }
        }

        #region Document Recieve

        private void GetCostCenterMaster(string keySearch)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"select top 100 COSTCENTERCODE as code
                            , COSTCENTERNAME as name
                            , COSTCENTERCODE + ' : ' + COSTCENTERNAME as display
                            from co_costcenter_master a WITH (NOLOCK)
                            where  a.SID = '" + ERPWAuthentication.SID + "' ");
            if (!string.IsNullOrEmpty(keySearch))
            {
                 sql.AppendLine(" AND (a.COSTCENTERCODE LIKE '%" + keySearch + "%' OR a.COSTCENTERNAME LIKE '%" + keySearch + @"%'
                                       OR a.COSTCENTERCODE+' : '+a.COSTCENTERNAME LIKE '%" + keySearch + @"%')");
            }
            sql.AppendLine(" ORDER BY a.COSTCENTERCODE");
            GC.Collect();
            Response.Write(JsonConvert.SerializeObject(dbService.selectDataFocusone(sql.ToString())));
            sql.Clear();
        }

        private void GetAssetLocation1(string keySearch)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"SELECT top 100 AssetLocation as code
                            ,Description as name
                            ,AssetLocation+' : '+Description as display
                            FROM am_define_assetlocation  a WITH (NOLOCK)
                    WHERE SID='" + ERPWAuthentication.SID + @"' 
                          AND CompanyCode='" + ERPWAuthentication.CompanyCode + @"' ");
            if (!string.IsNullOrEmpty(keySearch))
            {
                sql.AppendLine(" AND (a.AssetLocation LIKE '%" + keySearch + "%' OR a.Description LIKE '%" + keySearch + @"%' 
                                        OR a.AssetLocation+' : '+a.Description LIKE '%" + keySearch + @"%')");
            }
            sql.AppendLine(" ORDER BY a.AssetLocation ");
            GC.Collect();
            Response.Write(JsonConvert.SerializeObject(dbService.selectDataFocusone(sql.ToString())));
            sql.Clear();
        }

        #endregion

        #region CI

        private void GetMasterCI_Class(string keySearch)////
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"select top 100 ClassCode as code
                            , ClassName as name
                            , ClassCode + ' : ' + ClassName as display
                            from master_equipment_class a WITH (NOLOCK)
                            where  a.SID = '" + ERPWAuthentication.SID + "' ");
            if (!string.IsNullOrEmpty(keySearch))
            {
                sql.AppendLine(" AND (a.ClassCode LIKE '%" + keySearch + "%' OR a.ClassName LIKE '%" + keySearch + @"%'
                                       OR a.ClassCode+' : '+a.ClassName LIKE '%" + keySearch + @"%')");
            }
            sql.AppendLine(" ORDER BY a.ClassCode");
            GC.Collect();
            Response.Write(JsonConvert.SerializeObject(dbService.selectDataFocusone(sql.ToString())));
            sql.Clear();
        }

        private void GetMasterCI_Family(string keySearch)////
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"select top 100 MaterialGroupCode as code
                            , Description as name
                            , MaterialGroupCode + ' : ' + Description as display
                            from master_config_material_doctype a WITH (NOLOCK)
                            where  a.SID = '" + ERPWAuthentication.SID + @"' 
                            AND (Companycode = '" + ERPWAuthentication.CompanyCode + @"' or Companycode = '*') ");
            if (!string.IsNullOrEmpty(keySearch))
            {
                sql.AppendLine(" AND (a.MaterialGroupCode LIKE '%" + keySearch + "%' OR a.Description LIKE '%" + keySearch + @"%'
                                       OR a.MaterialGroupCode+' : '+a.Description LIKE '%" + keySearch + @"%')");
            }
            sql.AppendLine(" ORDER BY a.MaterialGroupCode");
            GC.Collect();
            Response.Write(JsonConvert.SerializeObject(dbService.selectDataFocusone(sql.ToString())));
            sql.Clear();
        }

        private void GetMasterCI_Ownersevice(string keySearch)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"select top 100 OwnerGroupCode as code
                            , OwnerGroupName as xdesc
                            , OwnerGroupCode + ' : ' + OwnerGroupName as display
                            from ERPW_OWNER_GROUP a WITH (NOLOCK)
                            where  a.SID = '" + ERPWAuthentication.SID + "' and a.CompanyCode ='" + ERPWAuthentication.CompanyCode + "' ");
            if (!string.IsNullOrEmpty(keySearch))
            {
                sql.AppendLine(" AND (a.OwnerGroupCode LIKE '%" + keySearch + "%' OR a.OwnerGroupName LIKE '%" + keySearch + @"%'
                                       OR a.OwnerGroupCode+' : '+a.OwnerGroupName LIKE '%" + keySearch + @"%')");
            }
            sql.AppendLine(" ORDER BY a.OwnerGroupCode");
            GC.Collect();
            Response.Write(JsonConvert.SerializeObject(dbService.selectDataFocusone(sql.ToString())));
            sql.Clear();
        }

        #endregion

        #region User & Employee

        private void GetEmployeeData(string filterCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"SELECT TOP 300 EmployeeCode as code
                                ,case isnull(FirstName + ' ' + LastName,'')
                                    when '' then FirstName_TH + ' ' +  FirstName_TH
                                    else FirstName + ' ' + FirstName
                                 END name
                               ,case isnull(FirstName + ' ' + LastName,'')
                                    when '' then EmployeeCode +' : '+ FirstName_TH + ' ' +  FirstName_TH
                                    else EmployeeCode +' : '+ FirstName + ' ' + LastName
                                 END display
                            FROM master_employee WITH (NOLOCK)
                            WHERE SID = '" +  ERPWAuthentication.SID + "' AND CompanyCode = '" + ERPWAuthentication.CompanyCode + "'");
            if (filterCode == "USERNAME_EMPTY")
            {
                sql.AppendLine(" AND ISNULL(UserName,'') = '' ");
            }

            if (!string.IsNullOrEmpty(Request["keySearch"]))
            {
                string keySearch = Request["keySearch"];
                sql.AppendLine(" AND (EmployeeCode LIKE '%" + keySearch + "%' OR FirstName_TH + ' ' + LastName_TH LIKE '%" + keySearch + @"%'
                                       OR FirstName + ' ' +  LastName LIKE '%" + keySearch + @"%'
                                       OR EmployeeCode+' : '+FirstName_TH + ' ' + LastName_TH LIKE '%" + keySearch + @"%')");
            }

            sql.AppendLine(" ORDER BY EmployeeCode");
            GC.Collect();
            Response.Write(JsonConvert.SerializeObject(dbService.selectDataFocusone(sql.ToString())));
            sql.Clear();
        }

        #endregion


        public class AutoCompleteSource
        {
            public string code { get; set; }
            public string name { get; set; }
            public string display { get; set; }
            public string detail { get; set; }//ข้อมูล + รายละเอียดอื่นๆ
        }
    }
}