using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSWeb.crm.usercontrol
{
    public partial class ctrlRegisMemberForCompany : System.Web.UI.UserControl
    {
        public string CompanyThai
        {
            get 
            {
                return txtCompanyNameThai.Text;
            }
        }

        public string CompanyEnglish
        {
            get
            {
                return txtCompanyNameEnglish.Text;
            }
        }

        public string TaxNumber
        {
            get 
            {
                return txtTaxNumber.Text.Replace("-","");
            }
        }

        public string PhonNamber1
        {
            get
            {
                return txtPhon1.Text.Replace("-","");
            }
        }

        public string PhonNamber2
        {
            get
            {
                return txtPhon2.Text.Replace("-","");
            }
        }

        public string FaxNumber
        {
            get 
            {
                return txtFax.Text.Replace("-","");
            }
        }

        public string websiteName
        {
            get 
            {
                return txtWebsite.Text;
            }
        }

        public string email
        {
            get 
            {
                return txtEmail.Text;
            }
        
        }

        public string remark1
        {
            get 
            {
                return txtRemark1.Text;
            }
        }

        public string remark2
        {
            get
            {
                return txtRemark2.Text;
            }
        }

        public string remark3
        {
            get
            {
                return txtRemark3.Text;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}