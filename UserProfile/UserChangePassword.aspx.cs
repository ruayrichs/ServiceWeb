using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Service;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserProfile
{    
    public partial class UserChangePassword : System.Web.UI.Page
    {
        private ServiceContractLibrary lib = new ServiceContractLibrary();
        private CustomerService libCustomer = new CustomerService();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

       

      
        
    }
}