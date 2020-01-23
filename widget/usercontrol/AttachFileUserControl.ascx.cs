using agape.entity;
using agape.entity.km;
using agape.lib.KMV2.entity;
using agape.lib.KMV2.service;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Agape.Lib.KMV2.Core.service;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Focusone.ICMWcfService;
using ERPW.Lib.Authentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.F1WebService.ICMUtils;

namespace ServiceWeb.widget.usercontrol
{
    public partial class AttachFileUserControl : System.Web.UI.UserControl
    {
        #region Properties
        SortedList<String, IList> CatchAttatchment
        {
            get { return Session["catchAttacth"] == null ? null : (SortedList<String, IList>)Session["catchAttacth"]; }
            set { Session["catchAttacth"] = value; }
        }
        public String SID
        {
            get { return Session["sid"] == null ? null : (String)Session["sid"]; }
            set { Session["sid"] = value; }
        }
        public String EmployeeCode
        {
            get { return Session["employeeCode"] == null ? null : (String)Session["employeeCode"]; }
            set { Session["employeeCode"] = value; }
        }
        public String BusinessType
        {
            get { return Session["business_type"] == null ? null : (String)Session["business_type"]; }
            set { Session["business_type"] = value; }
        }
        public String DocType
        {
            get { return Session["doc_type"] == null ? null : (String)Session["doc_type"]; }
            set { Session["doc_type"] = value; }
        }
        public String DocNumber
        {
            get { return Session["doc_number"] == null ? null : (String)Session["doc_number"]; }
            set { Session["doc_number"] = value; _hd_gobalVardoc_number.Value = value; }
        }
        public String DocYear
        {
            get { return Session["doc_year"] == null ? null : (String)Session["doc_year"]; }
            set { Session["doc_year"] = value; }
        }
        public String KeyOption
        {
            get { return Session["key_option1"] == null ? null : (String)Session["key_option1"]; }
            set { Session["key_option1"] = value; }
        }

        public Boolean UpdateHeaderOnCommit
        {
            get { return Session["AttachFileUserContro_DragNDropl.UpdateHeaderOnCommit"] == null ? false : (Boolean)Session["AttachFileUserContro_DragNDropl.UpdateHeaderOnCommit"]; }
            set { Session["AttachFileUserContro_DragNDropl.UpdateHeaderOnCommit"] = value; }
        }
        public IList ListOfAttatchment
        {
            get
            {
                return Session["listAttachmentUserControl"] == null ? null : (IList)Session["listAttachmentUserControl"];
            }
            set
            {
                Session["listAttachmentUserControl"] = value;
            }
        }
        private IList ListOfBufferSave
        {
            get { return Session["listBufferSave"] == null ? null : (IList)Session["listBufferSave"]; }
            set { Session["listBufferSave"] = value; }
        }
        //public Boolean SetVisibleAddButton
        //{
        //    get { return btAdd.Visible; }
        //    set { btAdd.Visible = value; }
        //}
        //public Boolean SetVisibleUploadControl
        //{
        //    get { return ASPxUploadControl2.Visible; }
        //    set { ASPxUploadControl2.Visible = value; }
        //}
        //public Boolean SetVisibleText
        //{
        //    get { return _LB_Description.Visible; }
        //    set { _LB_Description.Visible = value; }
        //}
        //public Boolean SetVisibleDescription
        //{
        //    get { return txtDescription.Visible; }
        //    set { txtDescription.Visible = value; }
        //}
        public String FilePath
        {
            get { return Session["attach_file_usercontrol_filepath"] == null ? null : (String)Session["attach_file_usercontrol_filepath"]; }
            set { Session["attach_file_usercontrol_filepath"] = value; }
        }

        private UpdatePanel upanel
        {
            get { return (UpdatePanel)this.Parent.FindControl(hddUPanel.Value); }
        }
        public string UpdatePanelID
        {
            get { return hddUPanel.Value; }
            set { hddUPanel.Value = value; }
        }
        public bool display
        {
            set
            {
                if (value)
                {
                    gv_attachfile.Columns[0].Visible = !value;
                    pndd.Visible = !value;
                }
                else
                {
                    gv_attachfile.Columns[0].Visible = value;
                    pndd.Visible = value;
                }
            }
        }
        public string FromPage
        {
            get { return Session["attach_file_FromPage"] == null ? null : (String)Session["attach_file_FromPage"]; }
            set { Session["attach_file_FromPage"] = value; }
        }

        public List<KMAttachFileEntity> km_entity
        {
            get
            {
                if (Session["km.list.bean" + ClientID] == null)
                {
                    Session["km.list.bean" + ClientID] = new List<KMAttachFileEntity>();
                }
                return (List<KMAttachFileEntity>)Session["km.list.bean" + ClientID];
            }
            set { Session["km.list.bean" + ClientID] = value; }
        }

        public String doc_number
        {
            get { return Session["attach_v2_doc_number" + ClientID] == null ? null : (String)Session["attach_v2_doc_number" + ClientID]; }
            set { Session["attach_v2_doc_number" + ClientID] = value; }
        }
        #endregion

        LookupICMService lookupICMService = new LookupICMService();        
        private ICMUtils icmService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        KMService KMLib = new KMService();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsCallback)
            {
                if (ListOfAttatchment != null && ListOfAttatchment.Count > 0)
                {
                    gv_attachfile.DataSource = ListOfAttatchment;
                    gv_attachfile.DataBind();
                }
            }

        }

        public void init(
            String sid, bool req_sid,
            String employeeCode, bool req_emp,
            String bussiness, bool req_business,
            String doc_type, bool req_doctype,
            String doc_number, bool req_doc_number,
            String doc_year, bool req_doc_year,
            String key_option1, bool req_key_option1)
        {
            init(sid, req_sid,
            employeeCode, req_emp,
            bussiness, req_business,
            doc_type, req_doctype,
            doc_number, req_doc_number,
            doc_year, req_doc_year,
            key_option1, req_key_option1, false);
        }


        public void init(
            String sid, bool req_sid,
            String employeeCode, bool req_emp,
            String bussiness, bool req_business,
            String doc_type, bool req_doctype,
            String doc_number, bool req_doc_number,
            String doc_year, bool req_doc_year,
            String key_option1, bool req_key_option1
            , bool initWithOutDocNumber)
        {
            this.SID = sid;
            this.EmployeeCode = employeeCode;
            this.BusinessType = bussiness;
            this.DocType = doc_type;
            this.DocNumber = doc_number;
            this.DocYear = doc_year;
            this.KeyOption = key_option1;

            _hd_gobalVarsid.Value = sid;
            _hd_gobalVarbusiness_type.Value = bussiness;
            _hd_gobalVardoc_year.Value = doc_year;
            _hd_gobalVardoc_type.Value = doc_type;
            _hd_gobalVardescription.Value = "";
            _hd_gobalVardoc_number.Value = doc_number;
            _hd_gobalVaritem_no.Value = key_option1;
            _hd_gobalVaraction.Value = EntityUtils.ACTION_CREATE.ToString();

            this.CatchAttatchment = new SortedList<String, IList>();
            this.ListOfBufferSave = new ArrayList();
            this.ListOfAttatchment = new ArrayList();
            if (!initWithOutDocNumber)
            {
                this.refresh();
            }
            ControlScreen(doc_number, initWithOutDocNumber);
        }

        public void preinit(
            String sid, bool req_sid,
            String employeeCode, bool req_emp,
            String bussiness, bool req_business,
            String doc_type, bool req_doctype,
            String doc_number, bool req_doc_number,
            String doc_year, bool req_doc_year,
            String key_option1, bool req_key_option1)
        {
            this.SID = sid;
            this.EmployeeCode = employeeCode;
            this.BusinessType = bussiness;
            this.DocType = doc_type;
            this.DocNumber = doc_number;
            this.DocYear = doc_year;
            this.KeyOption = key_option1;

            _hd_gobalVarsid.Value = sid;
            _hd_gobalVarbusiness_type.Value = bussiness;
            _hd_gobalVardoc_year.Value = doc_year;
            _hd_gobalVardoc_type.Value = doc_type;
            _hd_gobalVardescription.Value = "";
            _hd_gobalVardoc_number.Value = doc_number;
            _hd_gobalVaritem_no.Value = key_option1;
            _hd_gobalVaraction.Value = EntityUtils.ACTION_CREATE.ToString();
        }
        public void refresh()
        {
            ListOfAttatchment = AttachFileUtils.loadAttachList(
                this.SID,
                this.EmployeeCode,
                this.DocType,
                this.DocNumber,
                this.DocYear, this.KeyOption, "");
            DataTable dtKMAttachment = ERPW.Lib.Master.KMServiceLibrary.getInstance().searchKMAttachment(this.SID, ERPWAuthentication.CompanyCode, this.BusinessType, this.DocType, this.DocNumber, this.DocYear);
            IList listAttachmentExt = new ArrayList();
            for (int i = 0; i < ListOfAttatchment.Count; i++)
            {
                MessageAttachList curEntity = (MessageAttachList)ListOfAttatchment[i];

                DataRow[] arrFoundRow = dtKMAttachment.Select("item_no='" + curEntity.item_no + "' ");
                if (arrFoundRow.Length <= 0)
                {
                    continue;
                }
                String createdOn = Convert.ToString(arrFoundRow[0]["created_on"]);
                curEntity.created_by = Convert.ToString(arrFoundRow[0]["created_by"]);
                curEntity.created_on = createdOn;
                curEntity.key_group = Convert.ToString(arrFoundRow[0]["firstname"]) + " " + Convert.ToString(arrFoundRow[0]["lastname"]);
            }

        }
        private void ControlScreen(string docnumber, bool initWithOutDocNumber)
        {
            if (initWithOutDocNumber)
            {
                isCreate.Visible = false;
                isChange.Visible = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(docnumber))
                {
                    isCreate.Visible = false;
                    isChange.Visible = true;
                }
                else
                {
                    isCreate.Visible = true;
                    isChange.Visible = false;
                }
            }
        }
        public void loadCatching()
        {
            if (CatchAttatchment == null)
                return;
            String dummykey = this.DocNumber + "|" + this.KeyOption;
            if (String.IsNullOrEmpty(this.DocNumber))
            {
                dummykey = "|" + this.KeyOption;
            }
            if (this.CatchAttatchment.ContainsKey(dummykey))
            {
                this.ListOfAttatchment = this.CatchAttatchment[dummykey];
            }
        }

        public void addBuffer(String sid,
                    String business_type,
                    String key_object_link,
                    String doc_year,
                    String doc_type,
                    String description,
                    String doc_number,//null dai
                    String item_no,//not null key search
                    IList listUploadFile,
                    int action,
                    MessageAttachList attach)
        {
            AttachListBuffer buffer = new AttachListBuffer(
                sid,
                business_type,
                key_object_link,
                doc_year,
                doc_type,
                description,
                doc_number,
                item_no,
                listUploadFile == null ? null : new ArrayList(listUploadFile),
                attach);

            if (listUploadFile != null && listUploadFile.Count > 0)
            {
                //UploadedFile upload = (UploadedFile)listUploadFile[0];
                FileUpload upload = (FileUpload)listUploadFile[0];

                byte[] imageData = new byte[upload.FileBytes.Length];
                attach.data_stream = imageData;
                attach.file_name = upload.FileName;
                attach.logical_name = upload.FileName;
                attach.description = description;
                attach.file_extension = upload.FileName.Substring(upload.FileName.LastIndexOf('.') + 1);
                attach.file_size = upload.PostedFile.ContentLength;
                attach.file_category = MessageAttachList.KM_CATAGORY_GENERAL;
                Decimal byteTomb = (attach.file_size / 1024) / 1024;
            }

            EntityUtils.doTransection(action, ListOfBufferSave, buffer);
        }

        public void Commit(String doc_number)
        {
            if (string.IsNullOrEmpty(doc_number))
                return;
            if (ListOfBufferSave == null)
                return;
            AttachListBuffer buff = null;
            DataSet dsResult = new DataSet();
            for (int i = 0; i < ListOfBufferSave.Count; i++)
            {
                buff = (AttachListBuffer)ListOfBufferSave[i];

                if (EntityUtils.isCreate(buff))
                {
                    buff.business_type = _hd_gobalVarbusiness_type.Value.ToString();
                    buff.doc_year = _hd_gobalVardoc_year.Value.ToString();
                    buff.doc_type = _hd_gobalVardoc_type.Value.ToString();
                    buff.attach.business_type = _hd_gobalVarbusiness_type.Value.ToString();
                    buff.attach.doc_type = _hd_gobalVardoc_type.Value.ToString();
                    buff.attach.doc_year = _hd_gobalVardoc_year.Value.ToString();
                    buff.attach.doc_number = _hd_gobalVardoc_number.Value.ToString();
                    buff.attach.key_search1 = _hd_gobalVaritem_no.Value.ToString();
                    if (buff.attach.doc_number == null || string.IsNullOrEmpty(buff.attach.doc_number))
                    {
                        buff.attach.doc_number = doc_number;
                    }

                    dsResult = AttachFileUtils.saveAttachList(
                        buff.sid,
                        buff.business_type,
                        buff.key_object_link,
                        buff.doc_year,
                        buff.doc_type,
                        buff.description,
                        doc_number,
                        buff.attach.key_search1,
                        buff.listUploadFile,
                        EntityUtils.ACTION_CREATE,
                        buff.attach);
                }
                if (EntityUtils.isUpdate(buff))
                {
                    AttachFileUtils.saveAttachList(
                        buff.sid,
                        buff.business_type,
                        buff.key_object_link,
                        buff.doc_year,
                        buff.doc_type,
                        buff.description,
                        buff.doc_number,
                        buff.attach.key_search1,
                        buff.listUploadFile,
                        EntityUtils.ACTION_UPDATE,
                        buff.attach);
                }
                if (EntityUtils.isDelete(buff))
                {
                    AttachFileUtils.saveAttachList(
                        buff.sid,
                        buff.business_type,
                        buff.key_object_link,
                        buff.doc_year,
                        buff.doc_type,
                        buff.description,
                        buff.doc_number,
                        buff.attach.key_search1,
                        buff.listUploadFile,
                        EntityUtils.ACTION_DELETE,
                        buff.attach);
                }
            }
            this.refresh();
            this.rebind();

            this.ListOfBufferSave = new ArrayList();
            this.ListOfAttatchment = new ArrayList();
        }

        //POM For Genereate Attached file [Release]
        //ReportDocument rpt = new ReportDocument();
        //public void GenerateReportForRelease(String _business, String _doc_type, String _doc_number, String _doc_year
        //    , string _reportgroup, ExportFormatType _format)
        //{
        //    try
        //    {
        //        DataTable dtReport = ReportUtils.getObjectiveList(ERPWAuthentication.SID
        //            , _reportgroup
        //            , _doc_type
        //            , ERPWAuthentication.CompanyCode);
        //        foreach (DataRow drReport in dtReport.Rows)
        //        {
        //            string _reportDll = "", _reportName = "", _datasetDll = "", _dataSetName = "";
        //            DataSet _dsDataSourceReport = new DataSet();
        //            #region Get Report Information
        //            Object[] objParam = new Object[] { icmconstants.ICM_CONST_MR_GETREPORTFILEPATH
        //                    , Session[ApplicationSession.USER_SESSION_ID]
        //                    , _reportgroup
        //                    , drReport["OBJECTIVECODE"].ToString() };
        //            DataSet[] objDataSet = new DataSet[] { };
        //            Object objReturn = icmService.ICMPrimitiveInvoke(objParam, objDataSet);
        //            _reportDll = objReturn.ToString();

        //            objParam = new Object[] { icmconstants.ICM_CONST_MR_GETREPORTFILENAME
        //                    , Session[ApplicationSession.USER_SESSION_ID]
        //                    , _reportgroup
        //                    , drReport["OBJECTIVECODE"].ToString() };
        //            objDataSet = new DataSet[] { };
        //            objReturn = icmService.ICMPrimitiveInvoke(objParam, objDataSet);
        //            _reportName = objReturn.ToString();

        //            objParam = new Object[] { icmconstants.ICM_CONST_MR_GETREPORTDATAPATH
        //                    , Session[ApplicationSession.USER_SESSION_ID]
        //                    , _reportgroup
        //                    , drReport["OBJECTIVECODE"].ToString() };
        //            objDataSet = new DataSet[] { };
        //            objReturn = icmService.ICMPrimitiveInvoke(objParam, objDataSet);
        //            _datasetDll = objReturn.ToString();

        //            objParam = new Object[] { icmconstants.ICM_CONST_MR_GETREPORTDATANAME
        //                    , Session[ApplicationSession.USER_SESSION_ID]
        //                    , _reportgroup
        //                    , drReport["OBJECTIVECODE"].ToString() };
        //            objDataSet = new DataSet[] { };
        //            objReturn = icmService.ICMPrimitiveInvoke(objParam, objDataSet);
        //            _dataSetName = objReturn.ToString();

        //            DataSet ds = new DataSet();
        //            ds = ReportUtils.getReportSchema2(_datasetDll, _dataSetName);

        //            DataTable _dtCondition = new DataTable("condition");
        //            _dtCondition.Columns.Add("p_sessionid", typeof(string));
        //            _dtCondition.Columns.Add("p_companycode", typeof(string));
        //            _dtCondition.Columns.Add("p_documenttype", typeof(string));
        //            _dtCondition.Columns.Add("p_documentnumber", typeof(string));
        //            _dtCondition.Columns.Add("p_fiscalyear", typeof(string));

        //            DataRow _dr = _dtCondition.NewRow();
        //            _dr["p_sessionid"] = Session[ApplicationSession.USER_SESSION_ID];
        //            _dr["p_companycode"] = ERPWAuthentication.CompanyCode;
        //            _dr["p_documenttype"] = _doc_type;
        //            _dr["p_documentnumber"] = _doc_number;
        //            _dr["p_fiscalyear"] = _doc_year;
        //            _dtCondition.Rows.Add(_dr);

        //            DataSet _ds = new DataSet();
        //            _ds.Tables.Add(_dtCondition);

        //            objParam = new Object[] { icmconstants.ICM_CONST_MR_GETREPORTSOURCEANDDATA
        //                    , Session[ApplicationSession.USER_SESSION_ID]
        //                    , _reportgroup
        //                    , drReport["OBJECTIVECODE"].ToString() };
        //            objDataSet = new DataSet[] { (DataSet)ds.Copy(), _ds };
        //            _dsDataSourceReport = icmService.ICMDataSetInvoke(objParam, objDataSet);
        //            #endregion
        //            InsertGenerateFile(_reportDll, _reportName, _dsDataSourceReport, _format, _business, _doc_type, _doc_number, _doc_year
        //                , drReport["OBJECTIVECODEDESC"].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void InsertGenerateFile(string _dllname, string _reportname, DataSet _dataset, ExportFormatType _format
        //    , string _business, string _doc_type, string _doc_number, string _doc_year, string _reportdesc)
        //{
        //    try
        //    {
        //        byte[] _data_report = ConvertCrReportByType(_dllname, _reportname, _dataset, _format);
        //        string _line = Convert.ToString(km_entity.Count + 1).PadLeft(3, '0');
        //        String object_id = Guid.NewGuid().ToString();

        //        MessageAttachList entity = new MessageAttachList(ERPWAuthentication.SID, _business, object_id);
        //        entity.sid = ERPWAuthentication.SID;
        //        entity.business_type = _business;
        //        entity.key_object_link = object_id;
        //        entity.key_search1 = _reportname;
        //        entity.key_search2 = "";
        //        entity.key_search3 = "";
        //        entity.key_search4 = "";
        //        entity.description = "";
        //        entity.item_no = _line;
        //        entity.doc_type = _doc_type;
        //        entity.doc_number = _doc_number;
        //        entity.doc_year = _doc_year;
        //        entity.file_path = "";
        //        entity.file_size = _data_report.Length;
        //        entity.data_stream = _data_report;
        //        entity.file_name = string.Join("-", new string[] { !string.IsNullOrEmpty(_reportdesc) ? _reportdesc : doc_number, Validation.getCurrentServerStringDateTime() });
        //        entity.logical_name = entity.file_name;
        //        entity.file_extension = getExtension(_format);
        //        entity.file_category = MessageAttachList.KM_CATAGORY_GENERAL;
        //        entity.created_by = ERPWAuthentication.EmployeeCode;
        //        entity.created_on = Validation.getCurrentServerStringDateTime();
        //        entity.updated_by = ERPWAuthentication.EmployeeCode;
        //        entity.updated_on = Validation.getCurrentServerStringDateTime();

        //        AttachListBuffer buffer = new AttachListBuffer(entity.sid
        //            , entity.business_type
        //            , entity.key_object_link
        //            , entity.doc_year
        //            , entity.doc_type
        //            , entity.description
        //            , entity.doc_number
        //            , entity.item_no
        //            , null
        //            , entity);

        //        EntityUtils.doTransection(EntityUtils.ACTION_CREATE, ListOfBufferSave, buffer);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private string getExtension(ExportFormatType _format)
        {
            string _strformat = "txt";
            if (_format.Equals((Enum)ExportFormatType.Excel)) _strformat = "xls";
            else if (_format.Equals((Enum)ExportFormatType.PortableDocFormat)) _strformat = "pdf";
            else if (_format.Equals((Enum)ExportFormatType.RichText)) _strformat = "rtf";
            else if (_format.Equals((Enum)ExportFormatType.WordForWindows)) _strformat = "doc";
            else if (_format.Equals((Enum)ExportFormatType.CrystalReport)) _strformat = "rpt";
            return _strformat;
        }
        //private byte[] ConvertCrReportByType(string _dllname, string _reportname, DataSet _dataset, ExportFormatType _format)
        //{
        //    try
        //    {
        //        rpt = ReportUtils.getReportDocument2(_dllname, _reportname);
        //        rpt.SetDataSource(_dataset);
        //        return getReportStream(rpt, _format);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private byte[] getReportStream(ReportDocument _rpt, ExportFormatType _format)
        {
            try
            {
                Stream stream = _rpt.ExportToStream(_format);
                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, (int)stream.Length);
                return imageData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lnkFile_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gv_attachfile.DataKeys[gvrow.RowIndex].Values["file_path"].ToString();
                string fileName = gv_attachfile.DataKeys[gvrow.RowIndex].Values["file_name"].ToString();
                //string filePath = lnkbtn.Text;
                //Response.ContentType = "image/jpg";


                string pathFile = Server.MapPath("~") + "\\managefile\\" + ERPWAuthentication.SID + filePath;
                if (File.Exists(pathFile))
                {
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                    Response.TransmitFile(pathFile);
                    Response.End();
                }
                else
                {
                    DataTable dtF1ServerPath = getKMServerMapPath();
                    foreach (DataRow dr in dtF1ServerPath.Rows)
                    {
                        if (dr["F1_PATH"].ToString() != "")
                        {
                            string f1_pathshareFolder = dtF1ServerPath.Rows[0]["F1_PATH"].ToString();
                            string f1_domain = dtF1ServerPath.Rows[0]["F1_Domain"].ToString();
                            string f1_username = dtF1ServerPath.Rows[0]["F1_Username"].ToString();
                            string f11_password = dtF1ServerPath.Rows[0]["F1_Password"].ToString();
                            Stream stream = null;
                            //This controls how many bytes to read at a time and send to the client
                            int bytesToRead = 10000;

                            // Buffer to read bytes in chunk size specified above
                            byte[] buffer = new Byte[bytesToRead];
                            try
                            {

                                //Create a WebRequest to get the file
                                WebRequest fileReq = (WebRequest)FileWebRequest.Create(new Uri(f1_pathshareFolder + filePath));
                                fileReq.Credentials = new NetworkCredential(f1_domain == "" ? f1_username : f1_domain + @"\" + f1_username, f11_password);
                                fileReq.PreAuthenticate = true;
                                //Create a response for this request
                                WebResponse fileResp = (WebResponse)fileReq.GetResponse();

                                if (fileResp.ContentLength > 0)
                                {
                                    //fileResp.ContentLength = fileReq.ContentLength;
                                    //Get the Stream returned from the response
                                    stream = fileResp.GetResponseStream();

                                    // prepare the response to the client. resp is the client Response
                                    var resp = HttpContext.Current.Response;

                                    //Indicate the type of data being sent
                                    resp.ContentType = "application/octet-stream";

                                    //Name the file 
                                    resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                                    resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

                                    int length;
                                    do
                                    {
                                        // Verify that the client is connected.
                                        if (resp.IsClientConnected)
                                        {
                                            // Read data into the buffer.
                                            length = stream.Read(buffer, 0, bytesToRead);

                                            // and write it out to the response's output stream
                                            resp.OutputStream.Write(buffer, 0, length);

                                            // Flush the data
                                            resp.Flush();

                                            //Clear the buffer
                                            buffer = new Byte[bytesToRead];
                                        }
                                        else
                                        {
                                            // cancel the download if client has disconnected
                                            length = -1;
                                        }
                                    } while (length > 0); //Repeat until no data is read
                                    resp.End();
                                    Dispose();
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                if (stream != null)
                                {
                                    //Close the input stream
                                    stream.Close();
                                }
                            }
                            //using (new NetworkConnection(f1_pathshareFolder, new NetworkCredential(f1_username, f11_password)))
                            //{
                            //    if (!File.Exists(f1_pathshareFolder + filePath))
                            //    {
                            //        continue;
                            //    }

                            //    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                            //    Response.TransmitFile(f1_pathshareFolder + filePath);
                            //    Response.End();
                            //    Dispose();
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }
        private DataTable getKMServerMapPath()
        {
            DBService dbService = new DBService();
            string sql = "select * from km_server_map_path where sid='" + this.SID + "'";
            DataTable dt = dbService.selectDataFocusone(sql);
            dt.TableName = "km_server_map_path";
            return dt;
        }

        protected void _rp_maintain_items_Init(object sender, EventArgs e)
        {
            if (ListOfAttatchment != null)
            {
                Repeater rp = sender as Repeater;
                rp.DataSource = ListOfAttatchment;
                rp.DataBind();
            }
        }


        protected void gv_attachfile_RowDeleted(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string sid = e.Keys[0].ToString();
                string business_type = e.Keys[1].ToString();
                string key_object_link = e.Keys[2].ToString();
                string item_no = e.Keys[3].ToString();

                int index = ListOfAttatchment.IndexOf(new MessageAttachList(sid, business_type, key_object_link, item_no));
                if (index >= 0)
                {
                    //KMLib.DeleteAttachFile(sid, business_type, key_object_link, item_no);

                    ListOfAttatchment.RemoveAt(index);
                }

                int indexbuffer = ListOfBufferSave.IndexOf(new AttachListBuffer(sid, business_type, key_object_link, "", "", "", ""
                    , item_no, null, new MessageAttachList(sid, business_type, key_object_link, item_no)));

                if (indexbuffer >= 0)
                {
                    ListOfBufferSave.RemoveAt(indexbuffer);
                }

                AttachListBuffer buffer = new AttachListBuffer(sid, business_type, key_object_link, "", "", "", ""
                    , item_no, null, new MessageAttachList(sid, business_type, key_object_link, item_no));

                EntityUtils.doTransection(EntityUtils.ACTION_DELETE, ListOfBufferSave, buffer);

                //this.refresh();
                this.rebind();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alertMessage('" + ObjectUtil.Err(ex.Message) + "');", true);
            }
        }

        protected void gv_attachfileRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                if (e.Row.RowType == DataControlRowType.DataRow && scriptManager != null)
                {
                    scriptManager.RegisterPostBackControl(e.Row.FindControl("lnkFile"));
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alertMessage('" + ObjectUtil.Err(ex.Message) + "');", true);
            }
        }

        public void rebind()
        {
            if (ListOfAttatchment != null)
            {
                gv_attachfile.DataSource = ListOfAttatchment;
                gv_attachfile.DataBind();
            }
        }

        public int countFile
        {
            get { return ListOfAttatchment == null ? 0 : ListOfAttatchment.Count; }
        }


        protected void _btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                if (_file_attach.HasFile)
                {
                    byte[] bytes = _file_attach.FileBytes;

                    string sid = ERPWAuthentication.SID;
                    string business_type = _hd_gobalVarbusiness_type.Value;
                    string key_object_link = Guid.NewGuid().ToString();
                    string doc_year = _hd_gobalVardoc_year.Value;
                    string doc_type = _hd_gobalVardoc_type.Value;
                    string description = _hd_gobalVardescription.Value;
                    string doc_number = _hd_gobalVardoc_number.Value;
                    //string key_no = _hd_gobalVaritem_no.Value;
                    string item_no = "1";

                    if (ListOfAttatchment != null)
                    {
                        item_no = (ListOfAttatchment.Count + 1).ToString();
                    }

                    MessageAttachList attach = new MessageAttachList(sid, business_type, key_object_link, doc_number);
                    attach.item_no = item_no;
                    attach.doc_year = doc_year;
                    attach.doc_type = doc_type;
                    attach.description = description;
                    attach.doc_number = doc_number;
                    attach.key_search1 = description;
                    attach.file_extension = _file_attach.FileName.LastIndexOf('.') > 0 ? _file_attach.FileName.Substring(_file_attach.FileName.LastIndexOf('.') + 1) : "";
                    attach.logical_name = _file_attach.FileName;
                    attach.file_category = MessageAttachList.KM_CATAGORY_GENERAL;
                    attach.file_name = attach.logical_name;
                    attach.created_by = ERPWAuthentication.EmployeeCode;
                    attach.created_on = Validation.getCurrentServerStringDateTime();
                    attach.updated_by = ERPWAuthentication.EmployeeCode;
                    attach.updated_on = Validation.getCurrentServerStringDateTime();
                    attach.key_group = ERPWAuthentication.FullNameEN;

                    AttachFileUtils.readFile(attach, new System.IO.MemoryStream(bytes));
                    attach.file_size = attach.data_stream.Length;

                    ListOfAttatchment.Add(attach);
                    AttachListBuffer buffer = new AttachListBuffer(
                        sid,
                        business_type,
                        key_object_link,
                        doc_year,
                        doc_type,
                        description,
                        doc_number,
                        item_no,
                        null,
                        attach);

                    EntityUtils.doTransection(EntityUtils.ACTION_CREATE, ListOfBufferSave, buffer);

                    gv_attachfile.DataSource = ListOfAttatchment;
                    gv_attachfile.DataBind();
                    updatePanelAttachfile.Update();
                }
                else
                {
                    throw new Exception("กรุณาเลือกเอกสารแนบ ก่อนกดปุ่มเพิ่มรายการ !!");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msgbox", "alert('" + ObjectUtil.Err(ex.Message) + "');", true);
            }
        }

        public static string GetCurrentDirectory = HostingEnvironment.ApplicationPhysicalPath.TrimEnd('\\');
        public void copyFileFromSrc(DataTable p_dtSrcFile)
        {
            try
            {
                string fullPath = "";
                FileStream file = null;
                foreach (DataRow dr in p_dtSrcFile.Rows)
                {
                    string filePath = dr["file_path"].ToString();


                    fullPath = GetCurrentDirectory + filePath;

                    if (File.Exists(fullPath))
                    {

                        using (file = new FileStream(fullPath, FileMode.Open))
                        {
                            byte[] bytes = new byte[file.Length];
                            file.Read(bytes, 0, bytes.Length);

                            string sid = ERPWAuthentication.SID;
                            string business_type = _hd_gobalVarbusiness_type.Value;
                            string key_object_link = Guid.NewGuid().ToString();
                            string doc_year = _hd_gobalVardoc_year.Value;
                            string doc_type = _hd_gobalVardoc_type.Value;
                            string description = _hd_gobalVardescription.Value;
                            string doc_number = _hd_gobalVardoc_number.Value;
                            string item_no = "1";

                            if (ListOfAttatchment != null)
                            {
                                item_no = (ListOfAttatchment.Count + 1).ToString();
                            }

                            MessageAttachList attach = new MessageAttachList(sid, business_type, key_object_link, doc_number);
                            attach.item_no = item_no;
                            attach.doc_year = doc_year;
                            attach.doc_type = doc_type;
                            attach.description = description;
                            attach.doc_number = doc_number;
                            attach.key_search1 = description;
                            attach.file_extension = dr["file_extension"].ToString();
                            attach.logical_name = dr["logical_name"].ToString();
                            attach.file_category = MessageAttachList.KM_CATAGORY_GENERAL;
                            attach.file_name = attach.logical_name;
                            attach.created_by = ERPWAuthentication.EmployeeCode;
                            attach.created_on = Validation.getCurrentServerStringDateTime();
                            attach.updated_by = ERPWAuthentication.EmployeeCode;
                            attach.updated_on = Validation.getCurrentServerStringDateTime();

                            AttachFileUtils.readFile(attach, new System.IO.MemoryStream(bytes));
                            attach.file_size = attach.data_stream.Length;

                            ListOfAttatchment.Add(attach);
                            AttachListBuffer buffer = new AttachListBuffer(
                                sid,
                                business_type,
                                key_object_link,
                                doc_year,
                                doc_type,
                                description,
                                doc_number,
                                item_no,
                                null,
                                attach);

                            EntityUtils.doTransection(EntityUtils.ACTION_CREATE, ListOfBufferSave, buffer);

                            gv_attachfile.DataSource = ListOfAttatchment;
                            gv_attachfile.DataBind();
                        }
                    }
                }
              
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msgbox", "alert('" + ObjectUtil.Err(ex.Message) + "');", true);
            }
        }

        protected void btn_refresh_attach_Click(object sender, EventArgs e)
        {
            rebind();
        }
    }
}