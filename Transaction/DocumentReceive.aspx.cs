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
using ERPLink.Lib.DocumentReceive.Entity;
using System.Globalization;
using agape.lib.constant;
using agape.proxy.data.dataset;
using ServiceWeb.Service;
using ServiceWeb.UserControl.AutoComplete.General;
using ERPLink.Lib.DocumentReceive.ienum;
using SNA.Lib.POS.utils;
using ERPW.Lib.F1WebService.ICMUtils;

namespace ServiceWeb.Transaction
{
    public partial class DocumentReceive : AbstractsSANWebpage
    {
        #region Session
        public string _ReceiveDocNumber
        {
            get { return (string)Session["DocumentReceive_ReceiveDocNumber"]; }
            set { Session["DocumentReceive_ReceiveDocNumber"] = value; }
        }
        public string _Mode
        {
            get { return (string)Session["DocumentReceive_Mode"]; }
            set { Session["DocumentReceive_Mode"] = value; }
        }
        public string _ReceiveDoctype
        {
            get { return (string)Session["DocumentReceive_ReceiveDoctype"]; }
            set { Session["DocumentReceive_ReceiveDoctype"] = value; }
        }
        public string _ReceiveFiscalYear
        {
            get { return (string)Session["DocumentReceive_ReceiveFiscalYear"]; }
            set { Session["DocumentReceive_ReceiveFiscalYear"] = value; }
        }

        private DocReceiveHeader _mEnDocReceive;
        public DocReceiveHeader mEnDocReceive
        {
            get
            {
                if (_mEnDocReceive == null)
                {
                    _mEnDocReceive = new DocReceiveHeader();
                    if (ApplicationSession.CREATE_MODE_STRING.Equals(_Mode))
                    {
                        string fisicalyear = !String.IsNullOrEmpty(_ReceiveFiscalYear) ? _ReceiveFiscalYear : Validation.getFiscalYear(Validation.getCurrentServerDate());
                        _mEnDocReceive = DocReSer.prepareEntityForCreate(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.UserName, fisicalyear, _ReceiveDoctype, _ReceiveDocNumber, _ReceiveDoctype, _ReceiveFiscalYear);
                    }
                    else 
                    {
                        _mEnDocReceive = DocReSer.getDocReceive(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, _ReceiveFiscalYear, _ReceiveDoctype, _ReceiveDocNumber);
                    }
                }
                return _mEnDocReceive;
            }
            set { _mEnDocReceive = value; }
        }

        private DataTable dtEquipmentStatus_;
        private DataTable dtEquipmentStatus
        {
            get
            {
                if (dtEquipmentStatus_ == null)
                {
                    Object[] objParam = new Object[] { "0800066", POSDocumentHelper.getSessionId(ERPWAuthentication.SID, ERPWAuthentication.UserName) };
                    DataSet[] ds = new DataSet[] { };
                    DataSet objReturn = icmUtil.ICMDataSetInvoke(objParam, ds);
                    if (objReturn.Tables.Count > 0)
                    {
                        dtEquipmentStatus_ = objReturn.Tables[0].DefaultView.ToTable(true, "StatusCode", "StatusName");
                    }
                    else
                    {
                        dtEquipmentStatus_ = new DataTable();
                    }
                }

                return dtEquipmentStatus_;
            }
        }
      
        #endregion

        #region properties status

        public bool isCreate { get { return ApplicationSession.CREATE_MODE_STRING.Equals(_Mode); } }

        public bool isDocNotRelease { get { return DocRecvStatus.NOT_RELEASE.code.Equals(mEnDocReceive.DocStatus); } }
        public bool isDocRelease { get { return DocRecvStatus.RELEASE.code.Equals(mEnDocReceive.DocStatus); } }
        public bool isDocCancel { get { return DocRecvStatus.CANCEL.code.Equals(mEnDocReceive.DocStatus); } }

        public bool isHasAsset { get { return mEnDocReceive.DocMaterial.FindAll(x => ("A").Equals(x.AccAss)).Count > 0;}}
        
        

        #endregion 

        private DocReceiveService DocReSer = DocReceiveService.getInstance();
        private AssetService assetService = new AssetService();
        private SearchHelpDocReceive searchHelp = SearchHelpDocReceive.getInstance();
        private ICMUtils icmUtil = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mEnDocReceive = null;
                DropdownSet();
                tbDocumentDate.Text = Validation.getCurrentServerDate();
                BindDataToScreen();
                disablePageRefStatus();
            }
        }

        

        #region Bind Criteria & Get Data
        private void DropdownSet()
        {
            ddlDocumentType.DataValueField = "DocumentTypeCode";
            ddlDocumentType.DataTextField = "Description2";
            ddlDocumentType.DataSource = DocReSer.GetListDocumentTypeCode(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, DocReceiveService.POSTING_TYPE_RECEIVE);
            ddlDocumentType.DataBind();
            ddlDocumentType.Items.Insert(0, new ListItem("", ""));

            ddlSupplier.DataValueField = "VendorCode";
            ddlSupplier.DataTextField = "txttitle";
            ddlSupplier.DataSource = DocReSer.getVendorList(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddlSupplier.DataBind();

            ddlDocumentStatus.DataValueField = "code";
            ddlDocumentStatus.DataTextField = "name";
            ddlDocumentStatus.DataSource = DocRecvStatus.GetListAll();
            ddlDocumentStatus.DataBind();

        }

        private void disablePageRefStatus()
        {
            ddlDocumentType.Enabled = false;
            ddlReleaseStatus.Enabled = false;
            ddlSupplier.Enabled = false;
            tbReceiveDate.Enabled = false;
            tbDocumentDate.Enabled = false;
            tbReferenceDocument.Enabled = false;
            tbAmount.Enabled = false;
            tbRemark.Enabled = false;
            if (isCreate)
            {
                ddlDocumentType.Enabled = true;
                ddlReleaseStatus.Enabled = true;
                ddlSupplier.Enabled = true;
                tbReceiveDate.Enabled = true;
                tbDocumentDate.Enabled = true;
                //tbReferenceDocument.Enabled = true;
                //tbAmount.Enabled = true;
                tbRemark.Enabled = true;
            }
            else if (isDocNotRelease)
            {
                ddlSupplier.Enabled = true;
                tbReceiveDate.Enabled = true;
                //tbDocumentDate.Enabled = true;
                //tbReferenceDocument.Enabled = true;
                //tbAmount.Enabled = true;
                tbRemark.Enabled = true;
            }
            updnTabContent.Update();
        }

        #endregion

        #region Bind Data

        private void BindDataToScreen()
        {
            if (ApplicationSession.CREATE_MODE_STRING.Equals(_Mode))
            {
                string fisicalyear = !String.IsNullOrEmpty(_ReceiveFiscalYear) ? _ReceiveFiscalYear :  Validation.getFiscalYear(Validation.getCurrentServerDate());
                DocReceiveHeader EnDocReceive = DocReSer.prepareEntityForCreate(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.UserName, fisicalyear, _ReceiveDoctype, _ReceiveDocNumber, _ReceiveDoctype, _ReceiveFiscalYear);
                rebindDocumentCreate(EnDocReceive);
                hdfrefPODocType.Value = _ReceiveDoctype;
                tbReferenceDocument.Text = _ReceiveDocNumber;
                updnTabContent.Update();
            }
            else if (ApplicationSession.CHANGE_MODE_STRING.Equals(_Mode))
            {
                rebindDocument(_ReceiveFiscalYear, _ReceiveDoctype, _ReceiveDocNumber);
            }
        }

        private void rebindDocumentCreate(DocReceiveHeader ENForCreate)
        {
            rebindDocumentTotalMode("", "", "", ENForCreate);
        }
        private void rebindDocument(string reffiyear, string refDoctype, string docNumber)
        {
            rebindDocumentTotalMode(reffiyear, refDoctype, docNumber, null);
        }
        private void rebindDocumentTotalMode(string reffiyear, string refDoctype, string docNumber, DocReceiveHeader EnDocReceive)
        {
            if (EnDocReceive == null)
            {
                mEnDocReceive = DocReSer.getDocReceive(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, reffiyear, refDoctype, docNumber);
            }
            else
            {
                mEnDocReceive = EnDocReceive;
            }
            DocReceiveHeader _data = mEnDocReceive;

            string dateCurr = Validation.getCurrentServerStringDateTime();
            _data.DelivDate = !string.IsNullOrEmpty(_data.DelivDate) ? _data.DelivDate : dateCurr.Substring(0, 8);
            _data.DocDate = !string.IsNullOrEmpty(_data.DocDate) ? _data.DocDate : dateCurr.Substring(0, 8);

            _ReceiveFiscalYear = mEnDocReceive.FiscalYear;
            _ReceiveDoctype = mEnDocReceive.DocType;
            ddlDocumentType.SelectedValue = mEnDocReceive.DocType;
            tbDocumentNo.Text = _data.DocNumber;
            tbRationale.Text = _data.ReasonText;


            //General data
            try
            {
                ddlSupplier.SelectedValue = _data.VendorCode;
            }
            catch (Exception)
            {
            }
            tbReceiveDate.Text = Validation.Convert2DateDisplay(_data.DelivDate);
            tbDocumentDate.Text = Validation.Convert2DateDisplay(_data.DocDate);
            tbReferenceDocument.Text = _data.Ref1;
            tbRemark.Text = _data.Remark;
            tbCreatedBy.Text = _data.Created_by;
            tbCreatedOnDate.Text = !string.IsNullOrEmpty(_data.Created_on) ? Validation.Convert2DateDisplay(_data.Created_on.Substring(0, 8)) : "";
            tbCreatedOnTime.Text = !string.IsNullOrEmpty(_data.Created_on) ? Validation.Convert2TimeDisplay(_data.Created_on.Substring(8, 6)) : "";
            tbUpdatedBy.Text = _data.Updated_by;
            tbUpdatedOnDate.Text = !string.IsNullOrEmpty(_data.Updated_on) ? Validation.Convert2DateDisplay(_data.Updated_on.Substring(0, 8)) : "";
            tbUpdatedOnTime.Text = !string.IsNullOrEmpty(_data.Updated_on) ? Validation.Convert2TimeDisplay(_data.Updated_on.Substring(8, 6)) : "";

            try
            {
                ddlDocumentStatus.SelectedValue = _data.DocStatus;
            }
            catch (Exception)
            {
            }

            rptCrad.DataSource = _data.DocItem;
            rptCrad.DataBind();

            rptReceiveMaterial.DataSource = _data.DocMaterial;
            rptReceiveMaterial.DataBind();
            decimal totalNetValue = 0;
            foreach (DocReceiveMaterial mat in _data.DocMaterial)
            {
                totalNetValue += mat.NetValue;
            }
            decimal totalItem = 0;
            tbAmount.Text = totalNetValue.ToString("#,##0.00");
            lbPI_BalanceAmount.Text = totalNetValue.ToString("#,##0.00");
            foreach (DocReceiveItem mat in _data.DocItem)
            {
                totalItem += mat.ReqApproveAmount;
            }
            lbPT_ApprovePay.Text = totalItem.ToString("#,##0.00");
            udpReceiveMaterial.Update();
            udpContrac.Update();
            disablePageRefStatus();
        }

        protected void rptReceiveMaterial_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hddObjectID = e.Item.FindControl("hddObjectID") as HiddenField;
                CheckBox chkSelectAsset = e.Item.FindControl("chkSelectAsset") as CheckBox;
                if (isDocRelease || isDocCancel)
                {
                    (e.Item.FindControl("txtQty") as TextBox).Enabled = false;
                }
                DocReceiveMaterial dataItem = (DocReceiveMaterial)e.Item.DataItem;
                bool isASSETCOMPLETE = false;
                bool isEQUIPMENTCOMPLETE = false;
                bool.TryParse(dataItem.ASSETCOMPLETE, out isASSETCOMPLETE);
                bool.TryParse(dataItem.EQUIPMENTCOMPLETE, out isEQUIPMENTCOMPLETE);
                bool isAssetNotBom = IsMatAssetNotBom(dataItem.AccAss, dataItem);
                if (!isAssetNotBom || (isASSETCOMPLETE && isEQUIPMENTCOMPLETE))
                {
                    chkSelectAsset.Visible = false;
                }
            }
        }
       
        protected void rptItemDetailRefBom_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DocReceiveAssetItem item = (DocReceiveAssetItem)e.Item.DataItem;
            AutoCompleteGeneral Location1 = e.Item.FindControl("AutoCompleteLocation1") as AutoCompleteGeneral;
            AutoCompleteGeneral Costcenter = e.Item.FindControl("AutoCompleteCostcenter") as AutoCompleteGeneral;
            Location1.SelectedDisplay = item.Location1;
            Costcenter.SelectedDisplay = item.COSTCENTER;

            bool isAsset = !string.IsNullOrEmpty((e.Item.FindControl("lblASSETCODE") as Label).Text);
            if (isAsset)
            {
                (e.Item.FindControl("txtASSETDESCRIPTION") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtACQUSITIONVALUE") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtACQUISITIONDATE") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtUSAGEYEAR") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtUSAGEMONTH") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtBrand") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtSerialno") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtModel") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtRemark1") as TextBox).Enabled = !isAsset;
                (e.Item.FindControl("txtRemark2") as TextBox).Enabled = !isAsset;

                (e.Item.FindControl("txtPROJECTCODE") as TextBox).Enabled = !isAsset;
                Location1.Enabled = !isAsset;
                Costcenter.Enabled = !isAsset;
            }
        }

        protected void rptCrad_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (isCreate)
            {
                decimal NetValue = 0, PayAmount = 0;
                decimal.TryParse((e.Item.FindControl("txtAmount") as TextBox).Text, out NetValue);
                decimal.TryParse((e.Item.FindControl("txtAmountPay") as TextBox).Text, out PayAmount);
                bool isBalance = NetValue == PayAmount;
                (e.Item.FindControl("_chk_select") as CheckBox).Enabled = !isBalance;
                (e.Item.FindControl("txtApprovePay") as TextBox).Enabled = !isBalance;
            }
            else
            {
                (e.Item.FindControl("_chk_select") as CheckBox).Enabled = false;
                (e.Item.FindControl("txtApprovePay") as TextBox).Enabled = false;
            }
        }

       
        #endregion

        #region Save Transection Event

        #region New Create & Update
        protected void btnSaveDoc_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlDocumentType.SelectedValue))
                {
                    throw new Exception("กรุณาเลือกประเภทเอกสาร!");
                }

                string SID = ERPWAuthentication.SID;
                string Companycode = ERPWAuthentication.CompanyCode;
                string EmployeeCode = ERPWAuthentication.EmployeeCode;
                DocReceiveHeader _dataHeader = prepareDataFromScreen();
                decimal totalNetValue = _dataHeader.DocMaterial.Sum(x => x.NetValue);
             
                decimal totalItem = _dataHeader.DocItem.Sum(x => x.ReqApproveAmount);
                //rptCrad.Items.Count > 0 = มีงวดการชำระ
                if (rptCrad.Items.Count > 0 && totalNetValue != totalItem)
                {
                    throw new Exception("ยอดรวมสินค้าและยอดรวมรายจ่ายไม่เท่ากัน <br/> ไม่สามารถบันทึกรายการได้!");
                }

                DocReceiveHeader Result = new DocReceiveHeader();
                if (ApplicationSession.CREATE_MODE_STRING.Equals(_Mode))
                {
                    foreach (DocReceiveItem en in _dataHeader.DocItem)
                    {
                        en.BalanceAmount = en.PendingAmount - en.ReqApproveAmount;
                    }
                    Result = DocReSer.saveCreateDocumentReceive(SID, Companycode, EmployeeCode, _dataHeader);
                    _Mode = ApplicationSession.CHANGE_MODE_STRING;
                    _ReceiveDocNumber = Result.DocNumber;
                    _ReceiveDoctype = Result.DocType;
                    _ReceiveFiscalYear = Result.FiscalYear;
                }
                else if(ApplicationSession.CHANGE_MODE_STRING.Equals(_Mode))
                {
                    //null = not update status
                    _dataHeader.DocNumber = _ReceiveDocNumber;
                    _dataHeader.FiscalYear = _ReceiveFiscalYear;
                    _dataHeader.DocType = _ReceiveDoctype;
                    Result = DocReSer.updateDocumentReceive(SID, Companycode, EmployeeCode,_dataHeader);
                }
                ClientService.AGSuccess("บันทึกใบตรวจรับ เลขที่ " + Result.DocNumber + " สำเร็จ");
                rebindDocument(Result.FiscalYear, Result.DocType, Result.DocNumber);

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


        private DocReceiveHeader prepareDataFromScreen()
        {
            string SID = ERPWAuthentication.SID;
            string Companycode = ERPWAuthentication.CompanyCode;
            string datetimenow = Validation.getCurrentServerDate();
            string fiscalyear = Validation.getFiscalYear(datetimenow);
            string Doctype = ddlDocumentType.SelectedValue;
            string dateCurr = Validation.getCurrentServerStringDateTime();

            DocReceiveHeader _dataHeader = new DocReceiveHeader();
            _dataHeader.SID = SID;
            _dataHeader.CompanyCode = Companycode;
            _dataHeader.FiscalYear = fiscalyear;
            _dataHeader.DocType = Doctype;
            _dataHeader.DocNumber = tbDocumentNo.Text;
            _dataHeader.DocDate = !string.IsNullOrEmpty(tbDocumentDate.Text) ? Validation.Convert2DateDB(tbDocumentDate.Text): dateCurr.Substring(0,8);
            _dataHeader.DocStatus = ddlDocumentStatus.SelectedValue;
            _dataHeader.DelivDate = !string.IsNullOrEmpty(tbReceiveDate.Text) ? Validation.Convert2DateDB(tbReceiveDate.Text) : "";
            _dataHeader.VendorCode = ddlSupplier.SelectedValue;
            _dataHeader.Ref1 = tbReferenceDocument.Text;
            _dataHeader.Remark = tbRemark.Text;
            decimal ganaralNetValue = 0;
            decimal.TryParse(tbAmount.Text, out ganaralNetValue);
            _dataHeader.NetValue = ganaralNetValue;
            _dataHeader.ReasonText = tbRationale.Text;
            _dataHeader.Created_on = dateCurr;
            _dataHeader.Created_by = ERPWAuthentication.UserName;
            _dataHeader.Updated_on = dateCurr;
            _dataHeader.Updated_by = ERPWAuthentication.UserName;
          
            #region ReceiveMaterial
            List<DocReceiveMaterial> _dataMaterial = new List<DocReceiveMaterial>();
            foreach (RepeaterItem item in rptReceiveMaterial.Items)
            {
                HiddenField hddPONumber = (HiddenField)item.FindControl("hddPONumber");
                HiddenField hddAccAssCatCode = (HiddenField)item.FindControl("hddAccAssCatCode");
                HiddenField hddItem = (HiddenField)item.FindControl("hddItem");
                HiddenField hddFiscalYear = (HiddenField)item.FindControl("hddFiscalYear");
                HiddenField hddMaterialCode = (HiddenField)item.FindControl("hddMaterialCode");
                HiddenField hddObjectID = (HiddenField)item.FindControl("hddObjectID");
                HiddenField hddUOM = (HiddenField)item.FindControl("hddUOM");
                HiddenField hddPlantCode = (HiddenField)item.FindControl("hddPlantCode");
                HiddenField hddStorageCode = (HiddenField)item.FindControl("hddStorageCode");
                HiddenField hddPOItem = (HiddenField)item.FindControl("hddPOItem");
                TextBox txtQty = (TextBox)item.FindControl("txtQty");
                TextBox txtUnitPrice = (TextBox)item.FindControl("txtPriceUnit");
                HiddenField hddRefPOCompanycode = (HiddenField)item.FindControl("hddRefPOCompanycode");
                HiddenField hddRefPODocNumber = (HiddenField)item.FindControl("hddRefPODocNumber");
                HiddenField hddRefPODocType = (HiddenField)item.FindControl("hddRefPODocType");
                HiddenField hddRefPOItem = (HiddenField)item.FindControl("hddRefPOItem");
                HiddenField hddRefPOYear = (HiddenField)item.FindControl("hddRefPOYear");
                HiddenField hddpendingPOQTY = (HiddenField)item.FindControl("hddpendingPOQTY");
                

                List<DocReceiveMaterialBom> Matbom = new List<DocReceiveMaterialBom>();
                decimal ReceiveQty = 0;
                decimal.TryParse(txtQty.Text, out ReceiveQty);
                decimal UnitPrice = 0;
                decimal.TryParse(txtUnitPrice.Text, out UnitPrice);
                decimal pendingPOQTY = 0;
                decimal.TryParse(hddpendingPOQTY.Value, out pendingPOQTY);

                _dataMaterial.Add(new DocReceiveMaterial()
                {
                    SID = ERPWAuthentication.SID,
                    CompanyCode = ERPWAuthentication.CompanyCode,
                    FiscalYear = hddFiscalYear.Value,
                    DocType = ddlDocumentType.SelectedValue,
                    DocNumber = "",
                    ItemNo = hddItem.Value,
                    MaterialCode = hddMaterialCode.Value,
                    ReceiveQty = ReceiveQty,
                    AccAss = hddAccAssCatCode.Value,
                    UnitPrice = UnitPrice,
                    NetValue = ReceiveQty * UnitPrice,
                    PlantCode = hddPlantCode.Value,
                    StorageCode = hddStorageCode.Value,
                    ReceiveUom = hddUOM.Value,
                    RefPOCompanyCode = hddRefPOCompanycode.Value,
                    RefPODocNumber = hddRefPODocNumber.Value,
                    RefPODocType = hddRefPODocType.Value,
                    RefPOItem = hddPOItem.Value,
                    RefPOYear = hddRefPOYear.Value,
                    PendingPOQty = pendingPOQTY,
                    ObjectID = hddObjectID.Value,
                    listMatBom = Matbom
                });
            }
            _dataHeader.DocMaterial = _dataMaterial;
            #endregion

            #region DocReceiveItem => งวดการชำระ

            List<DocReceiveItem> _dataitem = new List<DocReceiveItem>();
            foreach (RepeaterItem item in rptCrad.Items)
            {
                CheckBox chk = item.FindControl("_chk_select") as CheckBox;
                if (chk.Checked)
                {
                    DocReceiveItem enitem = JsonConvert.DeserializeObject<DocReceiveItem>(((Label)item.FindControl("lblPaymentPeriodData")).Text);

                    TextBox txtApprovePay = (TextBox)item.FindControl("txtApprovePay");
                    TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                    TextBox txtAmountPay = (TextBox)item.FindControl("txtAmountPay");
                    HiddenField hddPendingAmount = (HiddenField)item.FindControl("hddPendingAmount");
                    decimal NetValue = 0;
                    decimal PayAmount = 0;
                    decimal PendingAmount = 0;
                    decimal ReqApproveAmount = 0;
                    decimal.TryParse(txtAmount.Text, out NetValue);
                    decimal.TryParse(txtAmountPay.Text, out PayAmount);
                    decimal.TryParse(hddPendingAmount.Value, out PendingAmount);
                    decimal.TryParse(txtApprovePay.Text, out ReqApproveAmount);
                    //decimal dBalanceAmount = PendingAmount - ReqApproveAmount;
                    enitem.NetValue = NetValue;
                    enitem.PayAmount = PayAmount;
                    enitem.ReqApproveAmount = ReqApproveAmount;
                    //enitem.BalanceAmount = dBalanceAmount;
                    enitem.PendingAmount = PendingAmount;
                    _dataitem.Add(enitem);
                }
            }
            _dataHeader.DocItem = _dataitem;
            #endregion 

            return _dataHeader;
        }

        #endregion

        protected void btnAddTax_Click(object sender, EventArgs e)
        {
            List<TaxItem> temptax = new List<TaxItem>();
            Button btn = (sender as Button);
            Repeater rptTax = btn.FindControl("rptTax") as Repeater;
            int seq_tax = 1;
            foreach (RepeaterItem item in rptTax.Items)
            {
                //bool i.ch
                CheckBox chkWaitDocument = (CheckBox)item.FindControl("chkWaitDocument");
                TextBox tbTaxNumber = (TextBox)item.FindControl("tbTaxNumber");
                DropDownList ddlTaxCode = (DropDownList)item.FindControl("ddlTaxCode");
                TextBox txtTaxBase = (TextBox)item.FindControl("txtTaxBase");
                TextBox txtTaxValue = (TextBox)item.FindControl("txtTaxValue");
                TextBox txtTaxTotal = (TextBox)item.FindControl("txtTaxTotal");
                TextBox txtTaxCurrency = (TextBox)item.FindControl("txtTaxCurrency");

                temptax.Add(new TaxItem()
                {
                    //SID = SID,
                    //COMPANYCODE = Companycode,
                    DocumentNumber = tbDocumentNo.Text,
                    SEQ = seq_tax.ToString(),
                    Status = chkWaitDocument.Checked.ToString(),
                    TaxNo = tbTaxNumber.Text,
                    TaxCode = ddlTaxCode.SelectedValue,
                    TaxBase = txtTaxBase.Text,
                    TaxValue = txtTaxValue.Text,
                    TaxTotal = txtTaxTotal.Text,
                    Currency = txtTaxCurrency.Text
                });
                seq_tax++;
            }

            temptax.Add(new TaxItem()
            {
                Status = "false"
            });
            rptTax.DataSource = temptax;
            rptTax.DataBind();
            udpTax.Update();
            ClientService.DoJavascript("$('.tab-pane').removeClass('active');");
            ClientService.DoJavascript("$('.tab-pane').removeClass('show');");
            ClientService.DoJavascript("$('#nav-tax').addClass('active');");
            ClientService.DoJavascript("$('#nav-tax').addClass('show');");
        }

        #region Release Document
        protected void btnReleaseDocument_Click(object sender, EventArgs e)
        {
            try
            {
                //if (string.IsNullOrEmpty(tbRationale.Text.Trim()))
                //{
                //    throw new Exception("กรุณาระบุเหตุผลการตรวจรับ!");
                //}
                if (!isDocNotRelease)
                {
                    throw new Exception("ยืนยันรายการล้มเหลว เอกสารไม่ได้อยู่รอยืนยัน!");
                }

                if (ddlDocTypeReceive.Items.Count > 0)
                {
                    try
                    {
                        ddlDocTypeReceive.SelectedIndex = 0;
                        ddlOrderReasonReceive.SelectedIndex = 0;
                    }
                    catch (Exception)
                    {

                    }
                }
                else 
                {
                    string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
                    DataTable dtData = searchHelp.getListDocTypeRefPO(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
                    ddlDocTypeReceive.DataValueField = "DocumentTypeCode";
                    ddlDocTypeReceive.DataTextField = "Description";
                    ddlDocTypeReceive.DataSource = dtData;
                    ddlDocTypeReceive.DataBind();
                    ddlDocTypeReceive.Items.Insert(0, new ListItem("", ""));

                    string orderReasonCode = "";
                    if (mEnDocReceive.DocMaterial.Count > 0)
                    {
                        DocReceiveMaterial enMat = mEnDocReceive.DocMaterial[0];
                        DataTable dtPO = searchHelp.GetPODataHeader(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                         enMat.RefPOYear, enMat.RefPODocType, enMat.RefPODocNumber);
                        if (dtPO.Rows.Count > 0)
                        {
                            orderReasonCode = dtPO.Rows[0]["OrderReasonCode"].ToString();
                        }
                    }
                    dtData = searchHelp.getReasonGRRefPO(sessionid,ERPWAuthentication.SID,orderReasonCode);
                    ddlOrderReasonReceive.DataValueField = "OrderReasonCode";
                    ddlOrderReasonReceive.DataTextField = "Description";
                    ddlOrderReasonReceive.DataSource = dtData;
                    ddlOrderReasonReceive.DataBind();
                }
                AutoComEmployeeReceive.SelectedValue = "";
                ClientService.DoJavascript("loadEmployeeWithoutCondition" + AutoComEmployeeReceive.ClientID + "();");
                udpDocReceiveRelease.Update();
                ClientService.DoJavascript("showInitiativeModal('modalDocuemntReceiveRelease');");
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

        protected void btnConfirmDocuemntReceive_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> msg = new List<string>();
                //if (string.IsNullOrEmpty(tbRationale.Text.Trim()))
                //{
                //    msg.Add("กรุณาระบุเหตุผลการตรวจรับ!");
                //}
                if (string.IsNullOrEmpty(ddlDocTypeReceive.SelectedValue))
                {
                    msg.Add("กรุณาระบุประเถทการรับ!");
                }
                if (string.IsNullOrEmpty(ddlOrderReasonReceive.SelectedValue))
                {
                    msg.Add("กรุณาระบุเหตผล!");
                }
                if (string.IsNullOrEmpty(AutoComEmployeeReceive.SelectedValue))
                {
                    msg.Add("กรุณาระบุผู้รับ!");
                }

                if (msg.Count > 0)
                {
                    throw new Exception(string.Join("<br/>", msg));
                }

                if (!isDocNotRelease)
                {
                    throw new Exception("ยืนยันรายการล้มเหลว เอกสารไม่ได้อยู่รอยืนยัน!");
                }

                
                mEnDocReceive.RefRecipient = AutoComEmployeeReceive.SelectedValue;
                mEnDocReceive.RefDocType = ddlDocTypeReceive.SelectedValue;
                mEnDocReceive.RefFiscalYear = Validation.getCurrentServerStringDateTime().Substring(0,4);
                mEnDocReceive.RefCompanyCode = ERPWAuthentication.CompanyCode;
                mEnDocReceive.RefDocnumber = "";
                mEnDocReceive.ReasonText = ddlOrderReasonReceive.SelectedValue;
                mEnDocReceive.Remark = txtRemarkReceive.Text;

                string DocRGNumebr = DocReSer.confirmDocumentReceive(
                    (string)Session[ApplicationSession.USER_SESSION_ID],
                     ERPWAuthentication.SID,
                     ERPWAuthentication.CompanyCode,
                     ERPWAuthentication.EmployeeCode,
                     mEnDocReceive
                     );
                rebindDocument(mEnDocReceive.FiscalYear, mEnDocReceive.DocType, mEnDocReceive.DocNumber);
                ClientService.AGSuccess("ยืนยันรายการสำเร็จ");

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

        #region Cancel
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbRationale.Text.Trim()))
                {
                    throw new Exception("กรุณาระบุเหตุ!");
                }

                if ((isDocNotRelease || isDocRelease) && !isCreate)
                {
                    mEnDocReceive.ReasonText = tbRationale.Text;
                    DocReceiveHeader en = DocReSer.updateDocumentStatus(
                                         ERPWAuthentication.SID,
                                         ERPWAuthentication.CompanyCode,
                                         ERPWAuthentication.EmployeeCode,
                                         DocRecvStatus.CANCEL,
                                         mEnDocReceive
                                         );
                    rebindDocument(en.FiscalYear, en.DocType, en.DocNumber);
                    ClientService.AGSuccess("ยกเลิกเอกสารสำเร็จ");
                }
                else 
                {
                    throw new Exception("ยกเลิกเอกสารล้มเหลว เอกสารไม่ได้อยู่สถานะที่ยกเลิกได้!");
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

        #region Create Asset
        protected void btnOpenModalCreateAsset_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validate
                int nSelect = 0;
                foreach (RepeaterItem mat in rptReceiveMaterial.Items)
                {
                    CheckBox chkSelectAsset = (mat.FindControl("chkSelectAsset") as CheckBox);
                    if (chkSelectAsset.Checked)
                    {
                        HiddenField hddASSETCOMPLETE = mat.FindControl("hddASSETCOMPLETE") as HiddenField;
                        HiddenField hddisItemDetail =  mat.FindControl("hddisItemDetail") as HiddenField;
                        if (hddisItemDetail.Value == "False" || hddASSETCOMPLETE.Value == "True")
                        {
                            hddItemDetailOBjectID.Value = (mat.FindControl("hddObjectID") as HiddenField).Value;
                            hddItemDetailItemNO.Value = (mat.FindControl("hddItem") as HiddenField).Value;
                            hddItemDetailMaterial.Value = (mat.FindControl("hddMaterialCode") as HiddenField).Value;
                            hddItemDetailUOM.Value = (mat.FindControl("hddUOM") as HiddenField).Value;
                            hddItemDetailBomMaterialCode.Value = (mat.FindControl("hddMaterialCode") as HiddenField).Value;
                            hddItemDetailBomMaterialName.Value = (mat.FindControl("hddMaterialName") as HiddenField).Value;
                            rebindBOMItemDetail();
                            udpOpenItemDetailRefMatBom.Update();
                            ClientService.DoJavascript("setRowCurrentItemDetail('" + hddItemDetailOBjectID.Value + "','" + hddItemDetailItemNO.Value + "','" + hddItemDetailBomMaterialCode.Value + "');");
                            string smg = "";
                            if (hddisItemDetail.Value != "True")
                            {
                                smg = "กรุณาส้รางรายละเอียดรายการ " + hddItemDetailBomMaterialCode.Value + " ก่อนการสร้างสินทรัพย์!";
                            }
                            else if (hddASSETCOMPLETE.Value == "True")
                            {
                                smg = "สินค้า " + hddItemDetailBomMaterialCode.Value + " สร้างสินทรัพย์แล้ว!";
                            }
                            throw new Exception(smg);
                        }
                        nSelect++;
                    }
                    //else 
                    //{

                    //    foreach (RepeaterItem bom in (mat.FindControl("rptMatBom") as Repeater).Items)
                    //    {
                    //        CheckBox chkSelectCreateAsset = (bom.FindControl("chkSelectAsset") as CheckBox);
                    //        bool isCreateItemDetail = false;
                    //        bool.TryParse((bom.FindControl("hhdisItemDetail") as HiddenField).Value, out isCreateItemDetail);
                            
                    //        if (chkSelectCreateAsset.Checked && !isCreateItemDetail)
                    //        {
                    //            DocReceiveMaterialBom enBom = JsonConvert.DeserializeObject<DocReceiveMaterialBom>((bom.FindControl("lblMatBomData") as Label).Text);
                    //            hddItemDetailOBjectID.Value = enBom.ObjectID;
                    //            hddItemDetailItemNO.Value = enBom.ItemNo;
                    //            hddItemDetailMaterial.Value = enBom.refMaterialCode;
                    //            hddItemDetailUOM.Value = enBom.refUOM;
                    //            hddItemDetailBomMaterialCode.Value = enBom.BomMaterialCode;
                    //            hddItemDetailBomMaterialName.Value = enBom.BomMaterialName;
                    //            rebindBOMItemDetail();
                    //            udpOpenItemDetailRefMatBom.Update();
                    //            ClientService.DoJavascript("setRowCurrentItemDetail('" + enBom.ObjectID + "','" + enBom.ItemNo + "','" + enBom.BomMaterialCode + "');");
                    //            throw new Exception("กรุณาส้รางรายละเอียดรายการ " + enBom.BomMaterialCode + " ก่อนการสร้างสินทรัพย์!");
                    //        }
                    //        else if (chkSelectCreateAsset.Checked && isCreateItemDetail)
                    //        {
                    //            DocReceiveMaterialBom enBom = JsonConvert.DeserializeObject<DocReceiveMaterialBom>((bom.FindControl("lblMatBomData") as Label).Text);
                    //            if (enBom.ASSETCOMPLETE == "True")
                    //            {
                    //                hddItemDetailOBjectID.Value = enBom.ObjectID;
                    //                hddItemDetailItemNO.Value = enBom.ItemNo;
                    //                hddItemDetailMaterial.Value = enBom.refMaterialCode;
                    //                hddItemDetailUOM.Value = enBom.refUOM;
                    //                hddItemDetailBomMaterialCode.Value = enBom.BomMaterialCode;
                    //                hddItemDetailBomMaterialName.Value = enBom.BomMaterialName;
                    //                rebindBOMItemDetail();
                    //                udpOpenItemDetailRefMatBom.Update();
                    //                throw new Exception("สินค้า " + hddItemDetailBomMaterialCode.Value + " สร้างสินทรัพย์แล้ว!");
                    //            }
                    //            nSelect++;
                    //        }
                    //    }
                    //}
                }

                if (nSelect <= 0)
                {
                    throw new Exception("กรุณาเลือกรายการเพื่อสร้าง");
                }

                #endregion

                if (ddlAssetCompayCode.Items.Count <= 0)
                {
                    #region Bind Dropdown
                    try
                    {
                        ddlAssetCompayCode.DataSource = DocReSer.getMasterCompany(ERPWAuthentication.SID, "");
                        ddlAssetCompayCode.DataBind();
                        if (ddlAssetCompayCode.Items.Count != 1)
                        {
                            ddlAssetCompayCode.Items.Insert(0, new ListItem("", ""));
                        }

                        ddlAssetCompayCode.SelectedValue = ERPWAuthentication.CompanyCode;
                    }
                    catch (Exception)
                    {

                    }

                    ddlAssetBuArea.DataSource = assetService.getBUArea(ERPWAuthentication.SID);
                    ddlAssetBuArea.DataBind();
                    if (ddlAssetBuArea.Items.Count != 1)
                    {
                        ddlAssetBuArea.Items.Insert(0, new ListItem("", ""));
                    }
                    DataTable dtData = assetService.getAssetGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
                    foreach (DataRow dr in dtData.Rows)
                    {
                        dr["Description"] = dr["AssetGroup"] + " : " + dr["Description"];
                    }
                    ddlAssetGroup.DataSource = dtData;
                    ddlAssetGroup.DataBind();
                    if (ddlAssetGroup.Items.Count != 1)
                    {
                        ddlAssetGroup.Items.Insert(0, new ListItem("", ""));
                    }
                    dtData = assetService.getAssetType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "");
                    foreach (DataRow dr in dtData.Rows)
                    {
                        dr["GroupName"] = dr["GroupCode"] + " : " + dr["GroupName"];
                    }
                    ddlAssetType.DataSource = dtData;
                    ddlAssetType.DataBind();
                    if (ddlAssetGroup.Items.Count != 1)
                    {
                        ddlAssetType.Items.Insert(0, new ListItem("", ""));
                    }

                    dtData = assetService.getAssetCategory1(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "", "");
                    foreach (DataRow dr in dtData.Rows)
                    {
                        dr["Description"] = dr["AssetCategory"] + " : " + dr["Description"];
                    }
                    ddlAssetCategory1.DataSource = dtData;
                    ddlAssetCategory1.DataBind();
                    ddlAssetCategory1.Items.Insert(0, new ListItem("", ""));

                    dtData = assetService.getAssetCategory2(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "", "", "");
                    foreach (DataRow dr in dtData.Rows)
                    {
                        dr["Description"] = dr["AssetCategory"] + " : " + dr["Description"];
                    }
                    ddlAssetCategory2.DataSource = dtData;
                    ddlAssetCategory2.DataBind();
                    ddlAssetCategory2.Items.Insert(0, new ListItem("", ""));

                    dtData = assetService.getDepreciationMethod(ERPWAuthentication.SID);
                    foreach (DataRow dr in dtData.Rows)
                    {
                        dr["calculation_method_description"] = dr["calculation_code"] + " : " + dr["calculation_method_description"];
                    }
                    ddlDepreciationMethod.DataSource = dtData;
                    ddlDepreciationMethod.DataBind();
                    if (ddlDepreciationMethod.Items.Count != 1)
                    {
                        ddlDepreciationMethod.Items.Insert(0, new ListItem("", ""));
                    }
                    #endregion
                }

                txtAssetDoctype.Text = "ASREFPO";
                ddlAssetBuArea.SelectedIndex = 0;
                ddlAssetGroup.SelectedIndex = 0;
                ddlAssetType.SelectedIndex = 0;
                ddlAssetCategory1.SelectedIndex = 0;
                ddlAssetCategory2.SelectedIndex = 0;
                ddlDepreciationMethod.SelectedIndex = 0;

                udpModalCreateAsset.Update();
                ClientService.DoJavascript("showInitiativeModal('modalCreateAssetMaster');");
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

        protected void btnCreateAsset_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listAOJBLINK = new List<string>();
                foreach (RepeaterItem mat in rptReceiveMaterial.Items)
                {

                     CheckBox chkSelectAsset = (mat.FindControl("chkSelectAsset") as CheckBox);
                     if (chkSelectAsset.Checked)
                     {
                         string ObjectID = (mat.FindControl("hddObjectID") as HiddenField).Value;
                         string ItemNO = (mat.FindControl("hddItem") as HiddenField).Value;
                         listAOJBLINK.Add(ObjectID + ItemNO);
                     }
                     //else 
                     //{
                     //    foreach (RepeaterItem bom in (mat.FindControl("rptMatBom") as Repeater).Items)
                     //    {
                     //        if ((bom.FindControl("chkSelectAsset") as CheckBox).Checked)
                     //        {
                     //            listAOJBLINK.Add((bom.FindControl("hhdAOBJECTLINK") as HiddenField).Value);
                     //        }
                     //    }
                     //}
                }
                List<DocReceiveAssetItem> listAsset = DocReSer.getAssetItemByAOBJECTLINK(ERPWAuthentication.SID, listAOJBLINK);
                listAsset.ForEach(r =>
                {
                    r.BUAREA = ddlAssetBuArea.SelectedValue;
                    r.DocType = txtAssetDoctype.Text;
                    r.DocCompany = ddlAssetCompayCode.SelectedValue;
                    r.ASSETGROUP = ddlAssetGroup.SelectedValue;
                    r.ASSETTYPE =  ddlAssetType.SelectedValue;
                    r.ASSETCATEGORY1 = ddlAssetCategory1.SelectedValue;
                    r.ASSETCATEGORY2 = ddlAssetCategory2.SelectedValue;
                    r.DEPREMETHOD = ddlDepreciationMethod.SelectedValue;
                });

                DocReSer.createAssetMasterRefPOItemDetail(
                    (string)Session[ApplicationSession.USER_SESSION_ID],
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.UserName
                    , listAsset);
                rebindDocument(_ReceiveFiscalYear, _ReceiveDoctype, _ReceiveDocNumber);
                rebindBOMItemDetail();
                ClientService.AGSuccess("Create Asset success.");
                ClientService.DoJavascript("closeInitiativeModal('modalCreateAssetMaster');");
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

        #endregion

        #region Create CI

        protected void btnOpenModalCreateCI_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validate
                List<DocReceiveMaterialBom> listBom = new List<DocReceiveMaterialBom>();
                foreach (RepeaterItem mat in rptReceiveMaterial.Items)
                {
                    CheckBox chkSelectAsset = (mat.FindControl("chkSelectAsset") as CheckBox);
                    if (chkSelectAsset.Checked)
                    {
                        #region Mat
                        HiddenField hddASSETCOMPLETE = mat.FindControl("hddASSETCOMPLETE") as HiddenField;
                        HiddenField hddEQUIPMENTCOMPLETE = mat.FindControl("hddEQUIPMENTCOMPLETE") as HiddenField;
                        HiddenField hddisItemDetail =  mat.FindControl("hddisItemDetail") as HiddenField;

                        DocReceiveMaterialBom en = new DocReceiveMaterialBom();
                        en.ObjectID = (mat.FindControl("hddObjectID") as HiddenField).Value;
                        en.ItemNo = (mat.FindControl("hddItem") as HiddenField).Value;
                        en.refMaterialCode = (mat.FindControl("hddMaterialCode") as HiddenField).Value;
                        en.BomMaterialCode = (mat.FindControl("hddMaterialCode") as HiddenField).Value;
                        en.BomMaterialName = (mat.FindControl("hddMaterialName") as HiddenField).Value;
                        DocReceiveMaterial enmat = mEnDocReceive.DocMaterial.Find(x => x.ObjectID == en.ObjectID);
                        en.BomQty = enmat.ReceiveQty;
                        en.NETVALUE = enmat.NetValue;
                        listBom.Add(en);

                        if (hddisItemDetail.Value == "False" 
                            || hddASSETCOMPLETE.Value != "True" 
                            || hddEQUIPMENTCOMPLETE.Value == "True")
                        {

                            hddItemDetailOBjectID.Value = (mat.FindControl("hddObjectID") as HiddenField).Value;
                            hddItemDetailItemNO.Value = (mat.FindControl("hddItem") as HiddenField).Value;
                            hddItemDetailMaterial.Value = (mat.FindControl("hddMaterialCode") as HiddenField).Value;
                            hddItemDetailUOM.Value = (mat.FindControl("hddUOM") as HiddenField).Value;
                            hddItemDetailBomMaterialCode.Value = (mat.FindControl("hddMaterialCode") as HiddenField).Value;
                            hddItemDetailBomMaterialName.Value = (mat.FindControl("hddMaterialName") as HiddenField).Value;
                            rebindBOMItemDetail();
                            udpOpenItemDetailRefMatBom.Update();
                            ClientService.DoJavascript("setRowCurrentItemDetail('" + hddItemDetailOBjectID.Value + "','" + hddItemDetailItemNO.Value + "','" + hddItemDetailBomMaterialCode.Value + "');");
                            string smg = "";
                            if (hddisItemDetail.Value == "False")
                            {
                                smg = "กรุณาสร้างรายละเอียดรายการและสินทรัพย์ " + hddItemDetailBomMaterialCode.Value + " ก่อนการสร้าง CI !";
                            }
                            else if (hddASSETCOMPLETE.Value != "True")
                            {
                                smg = "สินค้า " + hddItemDetailBomMaterialCode.Value + " ยังไม่ได้สร้างสินทรัพย์!";
                            }
                            else if (hddEQUIPMENTCOMPLETE.Value == "True")
                            {
                                smg = "สินค้า " + hddItemDetailBomMaterialCode.Value + " สร้าง CI แล้ว!";
                            }
                            throw new Exception(smg);
                        }
                        #endregion
                    }
                    //else
                    //{
                    //    #region BOM
                    //    foreach (RepeaterItem bomitm in (mat.FindControl("rptMatBom") as Repeater).Items)
                    //    {
                    //        if ((bomitm.FindControl("chkSelectAsset") as CheckBox).Checked)
                    //        {
                    //            DocReceiveMaterialBom en = JsonConvert.DeserializeObject<DocReceiveMaterialBom>(((Label)bomitm.FindControl("lblMatBomData")).Text);
                    //            listBom.Add(en);

                    //            bool isCreateItemDetail = false;
                    //            bool.TryParse((bomitm.FindControl("hhdisItemDetail") as HiddenField).Value, out isCreateItemDetail);
                               
                               
                    //            string msg = "";
                    //            if (!isCreateItemDetail)
                    //            {
                    //                msg = "กรุณาส้รางรายละเอียดรายการ " + hddItemDetailBomMaterialCode.Value + " ก่อนการสร้างสินทรัพย์!";
                    //            }
                    //            else if (en.ASSETCOMPLETE != "True")
                    //            {
                    //                msg = "สินค้า " + hddItemDetailBomMaterialCode.Value + " ยังไม่ได้สร้างสินทรัพย์!";
                    //            }
                    //            else if (en.EQUIPMENTCOMPLETE == "True")
                    //            {
                    //                msg = "สินค้า " + hddItemDetailBomMaterialCode.Value + " สร้าง CI แล้ว!";
                    //            }

                    //            if (msg != "")
                    //            {
                    //                hddItemDetailOBjectID.Value = en.ObjectID;
                    //                hddItemDetailItemNO.Value = en.ItemNo;
                    //                hddItemDetailMaterial.Value = en.refMaterialCode;
                    //                hddItemDetailUOM.Value = en.refUOM;
                    //                hddItemDetailBomMaterialCode.Value = en.BomMaterialCode;
                    //                hddItemDetailBomMaterialName.Value = en.BomMaterialName;
                    //                rebindBOMItemDetail();
                    //                udpOpenItemDetailRefMatBom.Update();
                    //                ClientService.DoJavascript("setRowCurrentItemDetail('" + hddItemDetailOBjectID.Value + "','" + hddItemDetailItemNO.Value + "','" + hddItemDetailBomMaterialCode.Value + "');");
                    //                throw new Exception(msg);
                    //            }
                    //        }
                    //    }
                    //    #endregion
                    //}
                }
                if (listBom.Count <= 0)
                {
                    throw new Exception("กรุณาเลือกรายการเพื่อสร้าง CI!");
                }
                #endregion

                rptMateriaBOMCreateCI.DataSource = listBom;
                rptMateriaBOMCreateCI.DataBind();
                udpCreateEquipment.Update();
                ClientService.DoJavascript("showInitiativeModal('modalAddEquipment');");
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

        protected void btnCreateCI_Click(object sender, EventArgs e)
        {
            try
            {
                List<DocReceiveMaterialBomCI> listForCreateCI = new List<DocReceiveMaterialBomCI>();

                #region Validate Data
                List<string> listError = new List<string>();
                int itemNO = 0;
                foreach (RepeaterItem item in rptMateriaBOMCreateCI.Items)
                {
                    itemNO++;
                    DocReceiveMaterialBomCI en = JsonConvert.DeserializeObject<DocReceiveMaterialBomCI>((item.FindControl("lblBOMCreateCI_Data") as Label).Text);
                    en.EquipmentCode = (item.FindControl("txtCICode") as TextBox).Text;
                    en.Family = (item.FindControl("AutoCompleteFamilyCI") as AutoCompleteGeneral).SelectedValue;
                    en.Class = (item.FindControl("AutoCompleteClassCI") as AutoCompleteGeneral).SelectedValue;
                    en.OwnerService = (item.FindControl("AutoCompleteOwnerServiceCI") as AutoCompleteGeneral).SelectedValue;
                    en.Category = (item.FindControl("ddlCategory") as DropDownList).SelectedValue;
                    en.Status = (item.FindControl("ddlEquipmentStatusCI") as DropDownList).SelectedValue;

                    List<string> temp = new List<string>();
                    if (string.IsNullOrEmpty(en.Family))
                    {
                        temp.Add("Family");
                    }
                    if (string.IsNullOrEmpty(en.Class))
                    {
                        temp.Add("Class");
                    }
                    if (string.IsNullOrEmpty(en.OwnerService))
                    {
                        temp.Add("OwnerService");
                    }
                    if (string.IsNullOrEmpty(en.Category))
                    {
                        temp.Add("Category");
                    }
                    if (string.IsNullOrEmpty(en.Status))
                    {
                        temp.Add("Status");
                    }

                    if (temp.Count > 0)
                    {
                        listError.Add("[" + itemNO + "]กรุณาระบุ " + string.Join(",",temp));
                    }
                    listForCreateCI.Add(en);
                }

                if (listError.Count > 0)
                {
                    throw new Exception(string.Join("<br/>", listError));
                }

                #endregion

                DocReSer.createEquipmentRefAseetPOItemDetail(
                    (string)Session[ApplicationSession.USER_SESSION_ID],
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.UserName,
                    listForCreateCI);
                rebindDocument(_ReceiveFiscalYear, _ReceiveDoctype, _ReceiveDocNumber);
                rebindBOMItemDetail();
                ClientService.AGSuccess("Create CI success.");
                ClientService.DoJavascript("closeInitiativeModal('modalAddEquipment');");
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


        protected void rptMateriaBOMCreateCI_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            (e.Item.FindControl("AutoCompleteFamilyCI") as AutoCompleteGeneral).SelectedDisplay = "";
            (e.Item.FindControl("AutoCompleteClassCI") as AutoCompleteGeneral).SelectedDisplay = "";
            (e.Item.FindControl("AutoCompleteOwnerServiceCI") as AutoCompleteGeneral).SelectedDisplay = "";
            DropDownList ddlCategory = e.Item.FindControl("ddlCategory") as DropDownList;
            DropDownList ddlEquipmentStatusCI = e.Item.FindControl("ddlEquipmentStatusCI") as DropDownList;
            ddlCategory.SelectedValue = "";
            ddlEquipmentStatusCI.DataSource = dtEquipmentStatus;
            ddlEquipmentStatusCI.DataBind();
            try
            {
                ddlEquipmentStatusCI.SelectedValue = "N";
            }
            catch (Exception)
            {
            }
        }

       
        #endregion

        #region calculate Qty Payment
        protected void btnrecalFifoPaymentPlan_Click(object sender, EventArgs e)
        {

            try
            {

                DocReceiveHeader _dataHeader = prepareDataFromScreen();
                decimal totalNetValue = 0;
                decimal totalItem = 0;
                foreach (DocReceiveMaterial mat in _dataHeader.DocMaterial)
                {
                    totalNetValue += mat.NetValue;
                }
                //ถ้า status create ให้ recalculate
                if (isCreate)
                {
                    _dataHeader = DocReSer.recalFifoPaymentPlan(_dataHeader);
                }
                tbAmount.Text = totalNetValue.ToString("#,##0.00");
                lbPI_BalanceAmount.Text = totalNetValue.ToString("#,##0.00");
                foreach (DocReceiveItem itm in _dataHeader.DocItem)
                {
                    totalItem += itm.ReqApproveAmount;
                }
                lbPT_ApprovePay.Text = totalItem.ToString("#,##0.00");
                rptCrad.DataSource = _dataHeader.DocItem;
                rptCrad.DataBind();
                udpContrac.Update();
                rptReceiveMaterial.DataSource = _dataHeader.DocMaterial;
                rptReceiveMaterial.DataBind();
                udpReceiveMaterial.Update();
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

        #region view & Save Split item detail Ref MatBom
        protected void btnOpenItemDetailRefMatBom_Click(object sender, EventArgs e)
        {
            try
            {
                rebindBOMItemDetail();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.DoJavascript("closeWhiteLoadingItemDetail()");
            }
        }
        protected void btnSaveSplitItemDetail_Click(object sender, EventArgs e)
        {
            try
            {
                List<DocReceiveAssetItem> listForSave = getListBomItemDeatil();
                DocReceiveHeader enHeader = prepareDataFromScreen();
                string OBjectID = hddItemDetailOBjectID.Value;
                string Material = hddItemDetailMaterial.Value;
                decimal nValueItem = listForSave.Sum(x => x.ACQUSITIONVALUE);
                decimal nNetValue = enHeader.DocMaterial.Find(x => x.ObjectID == OBjectID).NetValue;
                if (nValueItem > nNetValue)
                {
                    throw new Exception("รายละเอียดรายการมากกว่ารายการสินค้า " + Material);
                }
                else if(nValueItem < nNetValue)
                {
                    throw new Exception("รายละเอียดรายการน้อยกว่ารายการสินค้า " + Material);
                }
                DocReSer.saveAssetItemRefReceiveItem(ERPWAuthentication.SID, ERPWAuthentication.EmployeeCode,
                    listForSave);
                rebindDocument(_ReceiveFiscalYear, _ReceiveDoctype, _ReceiveDocNumber);
                ClientService.DoJavascript("closeWhiteLoadingItemDetail();");
                ClientService.AGSuccess("บันทึกสำเร็จ");
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

        private void rebindBOMItemDetail()
        {
            string ObjevtID = hddItemDetailOBjectID.Value.Trim();
            string ItemNo = hddItemDetailItemNO.Value.Trim();
            string MaterialCode = hddItemDetailMaterial.Value.Trim();
            string UOM = hddItemDetailUOM.Value.Trim();
            string BomMaterialCode = hddItemDetailBomMaterialCode.Value.Trim();
            DocReceiveMaterial enMat = mEnDocReceive.DocMaterial.Find(x => x.ObjectID == ObjevtID);
            decimal NatValue = enMat.NetValue;
            string MaterialName = enMat.MaterialName;
            List<DocReceiveAssetItem> listBomItemDeatil = DocReSer.getAssetItemRefReceiveItem(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                 ObjevtID, ItemNo,
                 MaterialCode, MaterialName
                 , UOM,
                 NatValue
                );
            txtSplitItemQty.Text = "";
            lblItemDetailTitle.Text = BomMaterialCode;
            txtSplitItemQty.Visible = !(enMat.ASSETCOMPLETE == "True");
            DEV_BTNSAVEDETAIL_SPLIT.Visible = !(enMat.ASSETCOMPLETE == "True");
            rptItemDetailRefBom.DataSource = listBomItemDeatil;
            rptItemDetailRefBom.DataBind();
            udpItemDetailData.Update();
        }

        protected void btnSplitItemQty_Click(object sender, EventArgs e)
        {
            try
            {
                int nSplit = 0;
                int.TryParse(txtSplitItemQty.Text, out nSplit);
                if (nSplit <= 0)
                {
                    throw new Exception("Please input split more than 0");
                }
                List<DocReceiveAssetItem> listBomItemDeatil = getListBomItemDeatil();
                string ObjevtID = hddItemDetailOBjectID.Value.Trim();
                string MaterialName = hddItemDetailBomMaterialName.Value.Trim();
                listBomItemDeatil = DocReSer.calAssetItemForSplit(MaterialName, nSplit, listBomItemDeatil);
                rptItemDetailRefBom.DataSource = listBomItemDeatil;
                rptItemDetailRefBom.DataBind();
                udpItemDetailData.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.DoJavascript("$('.panel-item-detail-refmatbom').AGWhiteLoading(false);");
            }
        }

        private List<DocReceiveAssetItem> getListBomItemDeatil()
        { 
            List<DocReceiveAssetItem> listItem = new List<DocReceiveAssetItem>();
            string ObjevtID = hddItemDetailOBjectID.Value.Trim();
            string ItemNo = hddItemDetailItemNO.Value.Trim();
            string MaterialCode = hddItemDetailMaterial.Value.Trim();
            string UOM = hddItemDetailUOM.Value.Trim();
            string AOBJECTLINK = ObjevtID + ItemNo;
            int itemNo = 0;
            foreach (RepeaterItem item in rptItemDetailRefBom.Items)
            {
                itemNo++;
                decimal dVALUE = 0;
                int dYear = 0, dMonth = 0;
                decimal.TryParse((item.FindControl("txtACQUSITIONVALUE") as TextBox).Text, out dVALUE);
                int.TryParse((item.FindControl("txtUSAGEYEAR") as TextBox).Text, out dYear);
                int.TryParse((item.FindControl("txtUSAGEMONTH") as TextBox).Text, out dMonth);
                string ACQUISITIONDATE = (item.FindControl("txtACQUISITIONDATE") as TextBox).Text;

                DocReceiveAssetItem en = JsonConvert.DeserializeObject<DocReceiveAssetItem>((item.FindControl("lblDataDetail") as Label).Text);
                en.AOBJECTLINK = AOBJECTLINK;
                en.ITEMNUMBER = itemNo.ToString("D4");
                en.ASSETGROUP = (item.FindControl("lblASSETGROUP") as Label).Text;
                en.ASSETTYPE = (item.FindControl("lblASSETTYPE") as Label).Text;
                en.ASSETCODE = (item.FindControl("lblASSETCODE") as Label).Text;
                en.ASSETDESCRIPTION = (item.FindControl("txtASSETDESCRIPTION") as TextBox).Text;
                en.ACQUSITIONVALUE = dVALUE;
                en.ACQUISITIONDATE = string.IsNullOrEmpty(ACQUISITIONDATE) ? "" : Validation.Convert2DateDB(ACQUISITIONDATE);
                en.COSTCENTER = (item.FindControl("AutoCompleteCostcenter") as AutoCompleteGeneral).SelectedValue;
                en.PROJECTCODE = (item.FindControl("txtPROJECTCODE") as TextBox).Text;
                en.USAGEYEAR = dYear.ToString();
                en.USAGEMONTH = dMonth.ToString();
                en.Brand = (item.FindControl("txtBrand") as TextBox).Text;
                en.Serialno = (item.FindControl("txtSerialno") as TextBox).Text;
                en.Model = (item.FindControl("txtModel") as TextBox).Text;
                en.Location1 = (item.FindControl("AutoCompleteLocation1") as AutoCompleteGeneral).SelectedValue;
                en.Remark1 = (item.FindControl("txtRemark1") as TextBox).Text;
                en.Remark2 = (item.FindControl("txtRemark2") as TextBox).Text;
               
                listItem.Add(en);
            }
            return listItem;
        }

        #endregion

        #endregion

        #region Properties & Convert Data
        public string ConvertTypeDecimal(Object val)
        {
            decimal number = 0;
            decimal.TryParse(Convert.ToString(val), out number);
            return number.ToString("#,##0.00");
        }

        public string FormatACCASSIGNMENT(string str)
        {
            string result = "";
            if (!string.IsNullOrEmpty(str))
            {
                switch (str)
                {
                    case "": result = "Stock"; break;
                    case "A": result = "Asset"; break;
                    case "K": result = "General Expense"; break;
                    default: result = ""; break;
                }
            }
            else
            {
                result = "";
            }

            return result;
        }

        public string FormatDocStat(string strDocStat)
        {
            if (!string.IsNullOrEmpty(strDocStat))
            {
                switch (strDocStat)
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

        public string FormatAapproval(string strAapStat)
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

        public bool IsMatAssetNotBom(Object MatAssetType,Object enMaterial)
        {
            if (isDocRelease && Convert.ToString(MatAssetType).Equals("A") && enMaterial != null)
            {
                return true;
            }
            return false;
        }

        public string GetBackgroundColor(object AccAss, object ASSETCOMPLETE, object EQUIPMENTCOMPLETE,object enMaterial)
        {
            if (Convert.ToString(AccAss).Equals("A") && IsMatAssetNotBom(AccAss,enMaterial))
            {
                bool isASSETCOMPLETE = Convert.ToString(ASSETCOMPLETE) == "True";
                bool isEQUIPMENTCOMPLETE = Convert.ToString(EQUIPMENTCOMPLETE) == "True";
                if (isASSETCOMPLETE && isEQUIPMENTCOMPLETE)
                {
                    return "background-color:#fb8d3e47;";
                }
                else if (isASSETCOMPLETE)
                {
                    return "background-color:#00ff9054;";
                }
                else 
                {
                    return "background-color:#c5c1c1a1;";
                }
            }
            return "";
        }

        #endregion

        #region Class Entity
        public class TaxItem
        {
            public string SID { get; set; }
            public string COMPANYCODE { get; set; }
            public string DocumentNumber { get; set; }
            public string SEQ { get; set; }
            public string Status { get; set; }
            public string TaxNo { get; set; }
            public string TaxCode { get; set; }
            public string TaxBase { get; set; }
            public string TaxValue { get; set; }
            public string TaxTotal { get; set; }
            public string Currency { get; set; }
        }

        #endregion

        

       
    }
}