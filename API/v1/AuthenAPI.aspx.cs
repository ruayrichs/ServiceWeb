using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Constant;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using ServiceWeb.API.Model.Respond;
using ServiceWeb.crm.AfterSale;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class AuthenAPI : System.Web.UI.Page
    {
        private string username { get { return Request["Username"]; } }
        private string password { get { return Request["Password"]; } }

        private DBService dbService = new DBService();

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticationMobileModel response = new AuthenticationMobileModel();
            try
            {
                _Login();

                response.resultCode = ConfigurationConstant.API_RESULT_CODE_SUCCESS;
                response.message = "Success";
                response.resultTime = DateTime.Now.ToString();

                response.username = username;
                response.employeeCode = ERPWAuthentication.EmployeeCode;
                response.employeeName = ERPWAuthentication.FullNameEN;
                response.OwnerCode = ERPWAuthentication.Permission.OwnerGroupCode;
                response.OwnerName = ERPWAuthentication.Permission.OwnerGroupName;

                string curData = Validation.Convert2DateDB(Validation.getCurrentServerDate());
                response.permissionKey = ServiceLibrary.LookUpTable(
                    "PermissionKey",
                    "ERPW_API_Permission_Token_Key",
                    "where EmployeeCode = '" + ERPWAuthentication.EmployeeCode + "' AND Active = 'true' and StartDate <= '" + curData + "' AND EndDate >= '" + curData + "'"
                );

                if (string.IsNullOrEmpty(response.permissionKey))
                {
                    string dateStart = DateTime.Now.Year.ToString() +
                        DateTime.Now.Month.ToString().PadLeft(2, '0') +
                        DateTime.Now.Day.ToString().PadLeft(2, '0');

                    string dateEnd = (DateTime.Now.Year + 20).ToString() +
                        DateTime.Now.Month.ToString().PadLeft(2, '0') +
                        DateTime.Now.Day.ToString().PadLeft(2, '0');

                    ERPW_API_Permission_Token_Key_DAO pkey_dao = new ERPW_API_Permission_Token_Key_DAO();
                    ERPW_API_Permission_Token_Key pkey_model = new ERPW_API_Permission_Token_Key();
                    pkey_model.Active = "true";
                    pkey_model.ChannelRequest = ConfigurationConstant.TIER_ZERO_CHANNEL_MOBILE;
                    pkey_model.CompanyCode = ERPWAuthentication.CompanyCode;
                    pkey_model.Created_By = "";
                    pkey_model.Created_On = "";
                    pkey_model.EmployeeCode = ERPWAuthentication.EmployeeCode;
                    pkey_model.IPAddress = "0.0.0.0";
                    pkey_model.PermissionKey = generatePermissionKey(30);
                    pkey_model.ProgramName = "API Login";
                    pkey_model.Remark = "Register";
                    pkey_model.SID = ERPWAuthentication.SID;
                    pkey_model.StartDate = dateStart;
                    pkey_model.EndDate = dateEnd;
                    pkey_model.Updated_By = "";
                    pkey_model.Updated_On = "";
                    pkey_dao.addRow(pkey_model);

                    response.permissionKey = pkey_model.PermissionKey;
                }
            }
            catch (Exception ex)
            {
                response.resultCode = ConfigurationConstant.API_RESULT_CODE_ERROR;
                response.message = ex.Message;
                response.resultTime = DateTime.Now.ToString();
            }
            string srResponse = JsonConvert.SerializeObject(response);
            Response.Write(srResponse);
        }

        private string generatePermissionKey(int length)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }

        protected void _Login()
        {
            string sid = ERPWebConfig.GetSID();
            string company = ERPWebConfig.GetCompany();
            string username = this.username.Trim();
            string password = this.password.Trim();

            bool isLogin = false;
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception("กรุณาระบุ Username");
                }

                if (string.IsNullOrEmpty(password))
                {
                    throw new Exception("กรุณาระบุ Password");
                }

                //sid = ERPWebConfig.GetSID();
                //company = ERPWebConfig.GetCompany();

                //1. Validate User Login                

                String callResult = "";

                try
                {
                    callResult = F1WebService.getUserLoginService().Login(sid, username, password);

                }
                catch (Exception ex)
                {
                    //VisibleCaptcha(true);
                    throw ex;
                }
                if (!string.IsNullOrEmpty(callResult) && callResult.Split('#')[0].Equals("S"))
                {
                    //2. Validate Password Expired
                    bool expired = false; //F1WebService.getUserLoginService().CheckPasswordExpired(sid, username, password);

                    if (!expired)
                    {
                        ProcessLogin(sid, username, ref isLogin);
                    }

                }
                else if (!string.IsNullOrEmpty(callResult) && callResult.Split('#')[0].Equals("E"))
                {
                    throw new Exception(callResult);
                }

                else
                {
                    throw new Exception("Invalid Username / Password");
                }


            }
            catch (System.Threading.ThreadAbortException tae)
            {
                //throw new Exception(getErrorMessage(tae));
                // do nothing
            }
            catch (SoapException soe)
            {
                throw new Exception(getErrorMessage(soe));
            }
            catch (Exception ex)
            {
                throw new Exception(getErrorMessage(ex));
            }
            finally
            {
                if (!isLogin)
                {
                    SaveLogLogin(sid, company, username, "LOGIN", isLogin);
                }
            }

        }


        private void ProcessLogin(String sid, String userName, ref bool isLogin)
        {
            string mode = ConfigurationHelper.getValue("system.mode");

            SystemModeControlService.SystemModeEntities modeEn = SystemModeControlService.getInstanceMode(mode);

            new ERPWAutoLoginService(sid, userName, modeEn);

            SystemModeControlService.LoginSystemModeWithRedirect(mode, "", sid, userName, false);
            isLogin = true;
            SaveLogLogin(sid, ERPWAuthentication.CompanyCode, userName, "LOGIN", isLogin);

            //Response.Redirect(Page.ResolveUrl("~/Default.aspx"), true);
        }

        private string GetUserName(string sid, string companyCode)
        {
            string user = username;

            string sql = "SELECT UserName FROM master_employee WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + "' AND UserName = '" + user + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return user;
            }
            else
            {
                sql = @"SELECT a.UserName, b.Email
                        FROM master_employee a
                        LEFT OUTER JOIN master_employee_address b ON a.SID = b.SID
	                        AND a.CompanyCode = b.CompanyCode
	                        AND a.EmployeeCode = b.EmployeeCode
                        WHERE a.SID = '" + sid + @"'
    	                    AND a.CompanyCode = '" + companyCode + @"'
	                        AND b.Email = '" + user + "'";

                dt = dbService.selectDataFocusone(sql);

                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["UserName"].ToString()))
                    {
                        throw new Exception("Username " + user + " not found in system.");
                    }

                    return dt.Rows[0]["UserName"].ToString().Trim();
                }
                else
                {
                    throw new Exception("Username " + user + " not found in system.");
                }
            }
        }

        private String getErrorMessage(Exception ex)
        {
            String errMessage = ex.Message;
            if (errMessage.IndexOf("--->") > -1)
            {

                if (errMessage.IndexOf("at Link") > -1)
                {
                    int messageLength = errMessage.IndexOf("at Link") - 7 - errMessage.IndexOf("--->") + 4;
                    errMessage = errMessage.Substring(errMessage.IndexOf("--->") + 4, messageLength);
                }
                else
                {
                    errMessage = errMessage.Substring(errMessage.IndexOf("--->") + 4);
                }


            }

            return errMessage;
        }

        private void SaveLogLogin(string sid, string companyCode, string username, string ACCESS_MODE, bool ACCESS_STATUS)
        {
            string employeecode = ERPWAuthentication.EmployeeCode;
            new LogServiceLibrary().saveLogUserAccess(sid, companyCode, employeecode, username, ACCESS_STATUS, ACCESS_MODE);

        }
    }
}