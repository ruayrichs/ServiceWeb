using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.Service;

namespace ServiceWeb.MasterConfig
{
    public partial class MasterAdConfig : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }
        MasterActiveDirectoryConfigLib Obj = new MasterActiveDirectoryConfigLib();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    dataBinding();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void dataCheckInput()
        {
            if (string.IsNullOrEmpty(AdIP.Text))
            {
                throw new Exception("กรุุณาระบุ IP Address");
            }
            if (string.IsNullOrEmpty(ADDomain.Text))
            {
                throw new Exception("กรุณาระบุ AD Domain");
            }
            if (string.IsNullOrEmpty(ADPort.Text))
            {
                throw new Exception("กรุณาระบุ AD Port");
            }
            if (string.IsNullOrEmpty(ADBaseDN.Text))
            {
                throw new Exception("กรุณาระบุ AD Base DN");
            }

        }
        private void dataCheckname() {

            if (string.IsNullOrEmpty(ADUsername.Text))
            {
                throw new Exception("กรุณาระบุ Username");
            }
            if (string.IsNullOrEmpty(ADPassword.Text))
            {
                throw new Exception("กรุณาระบุ Password");
            }
        }
        private void dataBinding()
        {
            List<ActiveDirectoryConfig> ListADConfigEn = Obj.GetAD_Host_Config(
               ERPWAuthentication.SID,
               ERPWAuthentication.CompanyCode,
               MasterActiveDirectoryConfigLib.HostEvent_AUTH
            );

            if(ListADConfigEn.Count > 0)
            {
                ActiveDirectoryConfig AD_En = ListADConfigEn[0];
                AdIP.Text = AD_En.ADIPAddress;
                ADDomain.Text = AD_En.ADDomain;
                ADPort.Text = AD_En.ADPort;
                ADBaseDN.Text = AD_En.ADBaseDn;
            }
        }

        protected void Btn_AD_Click(object sender, EventArgs e)
        {
            try
            {
                dataCheckInput();
                ActiveDirectoryConfig AD_Entity = new ActiveDirectoryConfig();
                AD_Entity.ADIPAddress = AdIP.Text;
                AD_Entity.ADDomain = ADDomain.Text;
                AD_Entity.ADPort = ADPort.Text;
                AD_Entity.ADBaseDn = ADBaseDN.Text;

                Obj.addAD_Host_Config(
                      ERPWAuthentication.SID,
                      ERPWAuthentication.CompanyCode,
                      AD_Entity,
                      MasterActiveDirectoryConfigLib.HostEvent_AUTH,
                      ERPWAuthentication.EmployeeCode
                  );

                ClientService.AGSuccess("บันทึกสำเร็จ");
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
        protected void Check_Connect_AD(object sender, EventArgs e)
        {

            try
            {
                dataCheckname();
                dataCheckInput();
                if (ActiveDirectoryService.connectStatus(AdIP.Text, ADPort.Text, ADBaseDN.Text, ADUsername.Text, ADPassword.Text))
                {
                    ClientService.AGSuccess("Connect Server Sucessfull.");
                }
                
            }catch(Exception ex)
            {
          
                    ClientService.AGError(ex.Message);
            }
            finally
            {
                    ClientService.AGLoading(false);
            }

                
        }
    }
   
}