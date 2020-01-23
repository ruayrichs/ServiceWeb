using Newtonsoft.Json;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API
{
    public partial class FavoriteMenuAPI : System.Web.UI.Page
    {
        private MenuService menuService = new MenuService();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            string ActionCase = Request["actionCase"];

            if (ActionCase == "addFavoriteMenu")
            {
                AddFavoriteMenu();
            }
            if (ActionCase == "loadFavoriteMenu")
            {
                getListFavoriteMenu();
            }
        }

        private void getListFavoriteMenu()
        {
            //DataTable dt = menuService.getSelectedEmployeeMenuMapping(
            //    ERPWAuthentication.SID,
            //    ERPWAuthentication.CompanyCode,
            //    ERPWAuthentication.EmployeeCode
            //);

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dr["NAVIGATEURL"] = Page.ResolveUrl(dr["NAVIGATEURL"].ToString());
            //    try
            //    {
            //        string checkPath = Convert.ToString(dr["icon_small"]).Replace("~", "");
            //        string serverPath = Server.MapPath("~" + checkPath);
            //        if (File.Exists(serverPath))
            //        {
            //            dr["icon_small"] = checkPath;
            //        }
            //        else
            //        {
            //            throw new Exception("no img");
            //        }
            //    }
            //    catch
            //    {
            //        dr["icon_small"] = "/images/menupic/TEMP_ICON_NO_IMAGE.png";
            //    }
            //}
            string responseJson = MenuService.getInStance().getMenuFavoriteString(ERPWAuthentication.SID, ERPWAuthentication.EmployeeCode);
            Response.Write(responseJson);
        }

        protected void AddFavoriteMenu()
        {
            bool Status = false;
            try
            {
                string mode = Request["mode"];
                string id = Request["id"];

                if (mode.Equals("insert"))
                {
                    menuService.insertSelectedMenu(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        ERPWAuthentication.EmployeeCode,
                        id
                    );
                }
                else
                {
                    menuService.deleteSelectedMenu(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        ERPWAuthentication.EmployeeCode,
                        id
                    );
                }
                Status = true;
                string responseJson = JsonConvert.SerializeObject(Status);
                Response.Write(responseJson);
            }
            catch (Exception)
            {
                Status = true;
                string responseJson = JsonConvert.SerializeObject(Status);
                Response.Write(responseJson);
            }
        }
    }
}