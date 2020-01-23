using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.auth;
using ERPLink.Lib.DocumentReceive;
using agape.lib.constant;
using ERPLink.Lib.DocumentReceive.ienum;

namespace ServiceWeb.Transaction
{
    public partial class DocumentReceiveCriteria : AbstractsSANWebpage
    {
        #region Doc Session
        public string _ReceiveDocNumber
        {
            get { return Session["DocumentReceive_ReceiveDocNumber"].ToString(); }
            set { Session["DocumentReceive_ReceiveDocNumber"] = value; }
        }
        public string _Mode
        {
            get { return Session["DocumentReceive_Mode"].ToString(); }
            set { Session["DocumentReceive_Mode"] = value; }
        }
        public string _ReceiveDoctype
        {
            get { return Session["DocumentReceive_ReceiveDoctype"].ToString(); }
            set { Session["DocumentReceive_ReceiveDoctype"] = value; }
        }
        public string _ReceiveFiscalYear
        {
            get { return Session["DocumentReceive_ReceiveFiscalYear"].ToString(); }
            set { Session["DocumentReceive_ReceiveFiscalYear"] = value; }
        }

        #endregion

        private string SID
        {
            get { return ERPWAuthentication.SID; }
        }

        private string CompanyCode
        {
            get { return ERPWAuthentication.CompanyCode; }

        }
       
      
        private static DBService dbService = new DBService();
        public DocReceiveService DocReSer = DocReceiveService.getInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _ReceiveDocNumber = "";
                try
                {
                    string datastart = "";
                    string dateend = "";
                    if (!string.IsNullOrEmpty(tbStartDate.Text))
                    {
                        datastart = Validation.Convert2DateDB(tbStartDate.Text);
                    }
                    if (!string.IsNullOrEmpty(tbStartDate.Text))
                    {
                        dateend = Validation.Convert2DateDB(tbEndDate.Text);

                        if (string.IsNullOrEmpty(tbStartDate.Text))
                        {
                            datastart = Validation.Convert2DateDB(tbStartDate.Text);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(tbStartDate.Text))
                        {
                            dateend = datastart;
                            tbEndDate.Text = tbStartDate.Text;
                        }
                    }
                    DataTable DocfirstList = DocReSer.getListDocument(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ddlDocumentType.SelectedValue, datastart, dateend, ddlStatus.SelectedValue);
                    rptFisrtList.DataSource = DocfirstList;
                    rptFisrtList.DataBind();
                    udpFirstList.Update();

                    DataTable dtPOList = DocReceiveService.getInstance().getPOItemForDocumentReceive(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "", "", "", "");
                    rptDocList.DataSource = dtPOList;
                    rptDocList.DataBind();
                    udpDocList.Update();
                    SetDropDownList();
                    DefaultCriteria();
                    ClientService.AGLoading(false);
                }
                catch (Exception ex)
                {
                    ClientService.AGLoading(false);
                    ClientService.AGError(ObjectUtil.Err(ex.Message));
                }
            }
        }
        private void DefaultCriteria()
        {
            tbCompany.Text = CompanyCode + " : " + ERPWAuthentication.CompanyName;
            //tbFiscalYear.Text = Validation.getCurrentServerStringDateTime().Substring(0, 4);
        }
        private void SetDropDownList()
        {
            DataTable DocumentType = GetListDocumentTypeCode();
            DocumentType.Columns.Add("itemdes");
            foreach (DataRow i in DocumentType.Rows)
            {
                i["itemdes"] = i["DocumentTypeCode"] + " : " + i["Description"];
            }
            ddlDocumentType.DataValueField = "DocumentTypeCode";
            ddlDocumentType.DataTextField = "itemdes";
            ddlDocumentType.DataSource = DocumentType;
            ddlDocumentType.DataBind();
            ddlDocumentType.Items.Insert(0, new ListItem("", ""));

            ddlStatus.DataValueField = "code";
            ddlStatus.DataTextField = "name";
            ddlStatus.DataSource = DocRecvStatus.GetListAll();
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("", ""));

        }

       

        public string ConvertTypeDecimal(string val, string _index)
        {
            val = TypeDecimal(val, _index);
            return val;
        }

        public string TypeDecimal(string price, string typeDec)
        {
            if (!string.IsNullOrEmpty(price))
            {
                if (typeDec == "0") { price = string.Format("{0:#,##0}", Convert.ToDecimal(price)); }
                else if (typeDec == "2") { price = string.Format("{0:#,##0.00}", Convert.ToDecimal(price)); }
                else { price = string.Format("{0:#,##0.00}", Convert.ToDecimal(price)); }
            }
            else
            {
                price = "0.00";
            }
            return price;
        }

        

        protected string FormatDocStat(string strDocStat)
        {
                switch (strDocStat)
                {
                    case "01" : return "01 : Not Release";
                    case "02": return "02 : Release";
                    case "03": return "03 : Cancel";
                    default: return "01 : Not Releases";
                }
        }



        protected string FormatAapproval(string strAapStat)
        {
            if (!string.IsNullOrEmpty(strAapStat))
            {
                switch (strAapStat)
                {
                    case "N": return "N : Not Start";
                    case "S": return "S : Start";
                    default: return "N : Not Starts";
                }
            }
            else
            {
                return "N : Not Starts";
            }
        }

        protected string FormatBankCode(string strBankCode)
        {
            if (!string.IsNullOrEmpty(strBankCode))
            {
                switch (strBankCode)
                {
                    case "00": return "00 : Not Release";
                    case "01": return "01 : Release";
                    default: return "00 : Not Releases";
                }
            }
            else
            {
                return "00 : Not Releases";
            }
        }

        protected DataTable GetListDocumentTypeCode()
        {
           
            string sql = @"SELECT A.DocumentTypeCode,A.PostingType,B.Description
		                    FROM master_config_lo_doctype_docdetail AS A
		                    INNER JOIN master_config_lo_doctype AS B ON A.SID = B.SID
		                    	AND A.DocumentTypeCode = B.DocumentTypeCode
		                    	AND A.companyCode = B.CompanyCode
		                    LEFT OUTER JOIN master_config_business AS C ON A.PostingType = C.BusinessCode
		                    WHERE A.SID = '" + SID +@"'
		                    	AND B.CompanyCode = '"+ CompanyCode + @"'
		                    	AND A.PostingType IN ('DOCRECEIVE')";
            DataTable dt = dbService.selectDataFocusone(sql);
            //List<DocumentReceiveItem> momaster = JsonConvert.DeserializeObject<List<DocumentReceiveItem>>(JsonConvert.SerializeObject(dt));
            //if (momaster.Count > 0)
            //{
            //    Result = momaster;
            //}

            return dt;
        }

       

        protected void btnSelectDoc_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string code = btn.CommandArgument;
            string SID = ERPWAuthentication.SID;
            string CompanyCode = ERPWAuthentication.CompanyCode;
            HiddenField hdfDocType = btn.FindControl("hdfDocType") as HiddenField; 
            HiddenField hdfFiscalYear = btn.FindControl("hdfFiscalYear") as HiddenField;
            try
            {
                string tmpdoc = (String)(sender as Button).CommandArgument;
                _ReceiveDocNumber = tmpdoc;
                _ReceiveDoctype = hdfDocType.Value;
                _ReceiveFiscalYear = hdfFiscalYear.Value;
                _Mode = ApplicationSession.CHANGE_MODE_STRING;
                Response.Redirect("/Transaction/DocumentReceive.aspx");
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string datestart = "";
            string dateend = "";
            if (!string.IsNullOrEmpty(tbStartDate.Text))
            {
                datestart = Validation.Convert2DateDB(tbStartDate.Text);
            }
            if (!string.IsNullOrEmpty(tbEndDate.Text))
            {
                dateend = Validation.Convert2DateDB(tbEndDate.Text);
            }

            DataTable DocfirstList = DocReSer.getListDocument(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode
                , ddlDocumentType.SelectedValue
                , datestart, dateend, ddlStatus.SelectedValue);
            rptFisrtList.DataSource = DocfirstList;
            rptFisrtList.DataBind();
            udpFirstList.Update();
            ClientService.DoJavascript("rebindTableFirstlist();");
            ClientService.AGLoading(false);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                bool stat = false;
                foreach(RepeaterItem i in rptDocList.Items)
                {
                    CheckBox chk = i.FindControl("_chk_select") as CheckBox;
                    if (chk.Checked) {
                        stat = true;
                        string PoNo = (i.FindControl("hddPONumber") as HiddenField).Value.ToString();
                        string PoType = (i.FindControl("hddPODoctype") as HiddenField).Value.ToString();
                        string POFiscalYear = (i.FindControl("hddFiscalYear") as HiddenField).Value.ToString();
                        _ReceiveDocNumber = PoNo;
                        _ReceiveDoctype = PoType;
                        _ReceiveFiscalYear = POFiscalYear;
                        _Mode = ApplicationSession.CREATE_MODE_STRING;
                        Response.Redirect("/Transaction/DocumentReceive.aspx");
                    }
                }
                if (!stat)
                {
                    ClientService.AGError("กรุณาระบุ PO ที่ต้องการสร้าง");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        

    }


}