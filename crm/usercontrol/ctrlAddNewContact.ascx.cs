using ERPW.Lib.Authentication;
using SNA.Lib.crm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.usercontrol
{
    public partial class ctrlAddNewContact : System.Web.UI.UserControl
    {
        public String firstname
        {
            get
            {
                return txtFirstName.Text;
            }
        }
        public String lastname
        {
            get
            {
                return txtLastName.Text;
            }
        }

        public String nickname
        {
            get
            {
                return txtNickName.Text;
            }
        }

        public Boolean isMale
        {
            get
            {
                if (ddlGender.SelectedValue == "M")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public String tel
        {
            get
            {
                return txtTel.Text;
            }
        }
        public String email
        {
            get
            {
                return txtEmail.Text;
            }
        }

        public String remark
        {
            get
            {
                return txtRemark.Text;
            }
        }

        public String facebook
        {
            get
            {
                return txtFaceBook.Text;
            }
        }

        public String WorkPisition
        {
            get
            {
                return txtWorkPosition.Text;
            }
        }
        public String Instagram
        {
            get
            {
                return txtInstagram.Text;
            }
        }
        public String twitter
        {
            get
            {
                return txtTwitter.Text;
            }
        }

        public String ContactType
        {
            get
            {
                return ddlContactType.SelectedValue;
            }
        }
        public String LineID
        {
            get
            {
                return txtLineID.Text;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDDLContactType();
            }
        }

        private void BindDDLContactType()
        {
            DataTable dt = CRMService.getInstance().getListDataCRMType(ERPWAuthentication.SID);
            ddlContactType.DataValueField = "ContactTypeCode";
            ddlContactType.DataTextField = "ContactTypeName";

            ddlContactType.DataSource = dt;
            ddlContactType.DataBind();
            ddlContactType.Items.Insert(0, new ListItem("กรุณาเลือก", ""));
        }
    }
}