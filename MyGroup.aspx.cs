using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Service;
using ServiceWeb.auth;
using ServiceWeb.crm.AfterSale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class MyGroup : AbstractsSANWebpage//: System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            string[] keys = hdfChange.Value.Split('|');

            string docType = keys[0];
            string docNumber = keys[1];
            string fiscalYear = keys[2];
            string customer = keys[3];

            GetDataToedit(docType, docNumber, fiscalYear, customer);
        }

        protected void GetDataToedit(string doctype, string docnumber, string fiscalyear, string customer)
        {
            ServiceCallFastEntryCriteria link = new ServiceCallFastEntryCriteria();
            string idGen = link.redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                //ServiceTicketLibrary libServiceTicket = new ServiceTicketLibrary();
                string PageRedirect = ServiceTicketLibrary.GetInstance().getPageTicketRedirect(
                    SID,
                    (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                );
                if (PageRedirect.Equals("ServiceCallTransaction.aspx"))
                {
                    ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                }
                else if (PageRedirect.Equals("ServiceCallTransactionChange.aspx"))
                {
                    ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                }
                else
                {
                    Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                }
                //Response-.Redirect("/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen, false);
            }
            #region Redirect Old
            //ICMUtils ICMService = WSHelper.getICMUtils();

            //Object[] objParam = new Object[] { "1500117",
            //        (string)Session[ApplicationSession.USER_SESSION_ID],
            //        ERPWAuthentication.CompanyCode,
            //        doctype,
            //        docnumber,
            //        fiscalyear };

            //DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
            //DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            //if (objReturn != null)
            //{
            //    serviceCallEntity = new tmpServiceCallDataSet();
            //    serviceCallEntity.Merge(objReturn.Copy());

            //    Session["SC_MODE"] = ApplicationSession.CHANGE_MODE_STRING;

            //    Response-.Redirect("~/crm/AfterSale/ServiceCallTransaction.aspx", false);
            //}
            #endregion
        }
    }
}