using agape.lib.constant;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;
using ERPW.Lib.Service;
using ServiceWeb.auth;
using ServiceWeb.crm.AfterSale;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.KM
{
    public partial class KnowledgeManagementDetail : AbstractsSANWebpage
    {
        private KMServiceLibrary kmservice = KMServiceLibrary.getInstance();
        private LogServiceLibrary logservice = new LogServiceLibrary();        
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

        #region Properties private

        public string GUID_RERUEST
        {
            get
            {
                return Guid.NewGuid().ToString().Substring(0, 8);
            }

        }
        public string SessionNameDefault = "KnowledgeManagementDetail_";

        private string pid 
        {
            get {
                return Request["id"];
            }
        }
        private string mObjectID
        {
            get
            {
                if (Session[SessionNameDefault + pid] == null)
                {
                    Session[SessionNameDefault + pid] = "";
                }
                return (string)Session[SessionNameDefault + pid];
            }
        }

        #endregion
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadCustomerDocType();
                bindData();
            }
        }

        private void loadCustomerDocType()
        {
            List<KMCriteriaEntity> listData = kmservice.GetKMGroup(ERPWAuthentication.SID);
            ddlGroup.DataValueField = "xKey";
            ddlGroup.DataTextField = "xValue";
            ddlGroup.DataSource = listData;
            ddlGroup.DataBind();
        }

        private void bindData()
        {
            string _where = kmservice.getWhereCondition("", mObjectID, "", "", "","");
            KMHeaderData en = kmservice.getKMDatarRefObjectID(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, _where, mObjectID);
            hhdKeyAobjectlink.Value = en.AttachmentID;
            hhdObjectItem.Value = en.ObjectItem;
            txtObjectID.Text = en.ObjectID;
            ddlGroup.SelectedValue = en.KMGroup;
            txtKeyword.Text = en.PrimaryKeyWord;
            txtSubject.Text = en.Description;
            txtDetail.Text = en.Details;
            txtSymtom.Text = en.Symptom;
            txtCause.Text = en.Cause;
            txtSolution.Text = en.Solution;
            loadTimeLineFileAttachment();

            #region Load Ticket

            bindTickNoRefKnowledgeID(en.ObjectID);
            #endregion

            #region Bind Log
            DataTable dt = logservice.GetKMManagementLog(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, txtObjectID.Text, hhdKeyAobjectlink.Value);
            KMManageChangeLog.BindingLog(dt);
            #endregion
        }

        protected void btnSaveKM_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSubject.Text.Trim()))
                {
                    throw new Exception("Subject is empty!<br/>");
                }
                KMHeaderData en = new KMHeaderData();
                en.KMGroup = ddlGroup.SelectedValue;
                en.KMGroupName = "";
                en.ObjectID = txtObjectID.Text;
                en.PrimaryKeyWord = txtKeyword.Text;
                en.Description = txtSubject.Text;
                en.AttachmentID = hhdKeyAobjectlink.Value;

                en.ObjectItem = hhdObjectItem.Value;
                en.Details = txtDetail.Text;
                en.Solution = txtSolution.Text;
                en.Symptom = txtSymtom.Text;
                en.Cause = txtCause.Text;

                kmservice.updateKMManagement(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.UserName,
                    en);
                bindData();
                ClientService.AGSuccess("Update knowledge  " + en.ObjectID + "  success.");
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

        private void loadTimeLineFileAttachment()
        {
            TimeLineControl.ProgramID = "";// LogServiceLibrary.PROGRAM_ID_KNOWLEDGE_MANAGEMENT;
            TimeLineControl.KeyAobjectlink = hhdKeyAobjectlink.Value;
            TimeLineControl.bindDataTimeLineHasFile();
        }

        protected void btnCheckRequired_Click(object sender, EventArgs e)
        {
            ClientService.DoJavascript("$('#btnSaveKM').click();");
        }

        #region tab Ticket

        private void bindTickNoRefKnowledgeID(string KnowledgeID)
        {
           List<KMTicketGroupEntity> listGroup = kmservice.getTiketKnowledgeMaping(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, KnowledgeID);
           rptSearchSaleGroup.DataSource = listGroup;
           rptSearchSaleGroup.DataBind();
           lblTicketRefKMAll.Text = listGroup.Sum(x => x.Qty).ToString("#,###");
           udpnItems.Update();
           ClientService.DoJavascript("afterSearch();");
        }

        protected void rptSearchSaleGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptSearchSale = e.Item.FindControl("rptSearchSale") as Repeater;
            KMTicketGroupEntity en = (KMTicketGroupEntity)e.Item.DataItem;
            rptSearchSale.DataSource = en.listData;
            rptSearchSale.DataBind();
        }

        public string GetRowColorAssign(string status)
        {
            string result = "";

            switch (status)
            {
                case ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL:
                    result = "text-danger";
                    break;
                case ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE:
                    result = "text-success";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        protected void btnLinkTransactionRedirect_Click(object sender, EventArgs e)
        {
            try
            {
                string doctype = hddDoctype.Value;
                string fiscalyear = hddFiscalyear.Value;
                string docnumber = hddDocNumner.Value;
                string customer = hddCustomer.Value;
                ServiceCallFastEntryCriteria ServiceCall = new ServiceCallFastEntryCriteria();
                string idGen = ServiceCall.redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
                if (!String.IsNullOrEmpty(idGen))
                {
                    ServiceTicketLibrary lib_TK = new ServiceTicketLibrary();
                    string PageRedirect = lib_TK.getPageTicketRedirect(
                        ERPWAuthentication.SID,
                        (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                    );
                    ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                    //Response.-Redirect("/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen, false);
                    //ClientService.DoJavascript("window.open('/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen + "','_blank');");
                }
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

        #endregion


    }
}