using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Service;
using SNA.Lib.POS.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.widget.usercontrol
{
    public partial class SearchHelpCIControl : System.Web.UI.UserControl
    {
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private UniversalService universalService = new UniversalService();
        private MasterConfigLibrary config = MasterConfigLibrary.GetInstance();
        private EquipmentService ServiceEquipment = new EquipmentService();
        private EquipmentService libCI = new EquipmentService();
        private string _idGen;
        private string idGen
        {
            get
            {
                if (string.IsNullOrEmpty(_idGen))
                {
                    _idGen = Request["id"];
                }
                return _idGen;
            }
        }

        private tmpServiceCallDataSet _serviceCallEntity;
        private tmpServiceCallDataSet serviceCallEntity
        {
            get
            {
                if (_serviceCallEntity == null)
                {
                    if (Session["ServicecallEntity" + idGen] == null)
                    {
                        Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
                    }

                    _serviceCallEntity = (tmpServiceCallDataSet)Session["ServicecallEntity" + idGen];
                }

                if (_serviceCallEntity.cs_servicecall_header.PrimaryKey.Length == 6)
                    _serviceCallEntity.cs_servicecall_header.PrimaryKey = new DataColumn[] {
                        _serviceCallEntity.cs_servicecall_header.Columns["SID"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CompanyCode"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CallerID"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CustomerCode"],
                        _serviceCallEntity.cs_servicecall_header.Columns["Doctype"],
                        _serviceCallEntity.cs_servicecall_header.Columns["Fiscalyear"]
                    };

                if (_serviceCallEntity.cs_servicecall_item.PrimaryKey.Length == 4)
                    _serviceCallEntity.cs_servicecall_item.PrimaryKey = new DataColumn[] {
                        _serviceCallEntity.cs_servicecall_item.Columns["SID"],
                        _serviceCallEntity.cs_servicecall_item.Columns["CompanyCode"],
                        _serviceCallEntity.cs_servicecall_item.Columns["xLineNo"],
                        _serviceCallEntity.cs_servicecall_item.Columns["ObjectID"]
                    };

                return _serviceCallEntity;
            }
            set
            {
                Session["ServicecallEntity" + idGen] = value;
                if (_serviceCallEntity != null)
                {
                    _serviceCallEntity = value;
                }
            }
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
                    DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, ds);
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
        public List<string> listCICode
        {
            get
            {
                List<string> listcode = new List<string>();
                if (!string.IsNullOrEmpty(txtSearchHelp_DataCISelect.Text))
                {
                    listcode = txtSearchHelp_DataCISelect.Text.Split(',').ToList();
                }
                return listcode;
            }
        }

        public void openAddCI()
        {
            hddClearCheck.Value = "false";
            txtSearchHelp_DataCISelect.Text = "";
            udpHddCISelect.Update();
            udpSearchConfigurationItem.Update();
        }
        public void removeCIformSearchwithButton(string CIcode)
        {
            string CI = txtSearchHelp_DataCISelect.Text.ToString();
            List<string> CIList = CI.Split(',').ToList();

            CIList.Remove(CIcode);

            txtSearchHelp_DataCISelect.Text = String.Join(",", CIList.ToArray());

            udpHddCISelect.Update();
            udpSearchConfigurationItem.Update();
            ClientService.AGLoading(false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setDefaultSearchCI();
            }
            
        }

        private void setDefaultSearchCI()
        {
            txtEquipmentCode.Text = "";
            txtEquipmentName.Text = "";
            //ddlEquipmentStatus.SelectedIndex = 0;

            if (ddlEquipmentType.Items.Count > 1)
            {
                ddlEquipmentType.SelectedIndex = 0;
                ddlSearch_EMClass.SelectedIndex = 0;
                ddlSearch_Category.SelectedIndex = 0;

            }
            else
            {
                ddlEquipmentType.DataTextField = "Description";
                ddlEquipmentType.DataValueField = "MaterialGroupCode";
                ddlEquipmentType.DataSource = universalService.getEquipmentType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
                ddlEquipmentType.DataBind();
                ddlEquipmentType.Items.Insert(0, new ListItem("All", ""));

                ddlSearch_EMClass.DataTextField = "ClassName";
                ddlSearch_EMClass.DataValueField = "ClassCode";
                ddlSearch_EMClass.DataSource = ServiceEquipment.getEMClass(ERPWAuthentication.SID);
                ddlSearch_EMClass.DataBind();
                ddlSearch_EMClass.Items.Insert(0, new ListItem("All", ""));

                DataTable dt = dtEquipmentStatus;
                ddlEquipmentStatus.DataTextField = "StatusName";
                ddlEquipmentStatus.DataValueField = "StatusCode";
                ddlEquipmentStatus.DataSource = dt;
                ddlEquipmentStatus.DataBind();
                ddlEquipmentStatus.Items.Insert(0, new ListItem("All", ""));

                ddlOwnerService.DataTextField = "OwnerGroupName";
                ddlOwnerService.DataValueField = "OwnerGroupCode";
                ddlOwnerService.DataSource = config.GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
                ddlOwnerService.DataBind();
                ddlOwnerService.Items.Insert(0, new ListItem("All", ""));
            }
        }
        protected void search_ci_btn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
               
                List<string> listEquipment = new List<string>();
                List<string> listEquipmentNew = new List<string>();
                foreach (DataRow drCI in serviceCallEntity.cs_servicecall_item.Rows)
                {
                    if (drCI.RowState == DataRowState.Unchanged)
                    {
                        listEquipment.Add(Convert.ToString(drCI["EquipmentNo"]));
                    } else
                    {
                        listEquipmentNew.Add(Convert.ToString(drCI["EquipmentNo"])); // list for set checkbox
                    }
                }
                DataTable dtCI = libCI.getEquipmentDetail(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, listEquipment);

                List<EquipmentService.EquipmentItemData> listEquipmentItem = ServiceEquipment.getListEquipment(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    txtEquipmentCode.Text,
                    txtEquipmentName.Text,
                    ddlEquipmentType.SelectedValue,
                    ddlEquipmentStatus.SelectedValue,
                    ddlSearch_EMClass.SelectedValue,
                    ddlSearch_Category.SelectedValue,
                    ddlOwnerService.SelectedValue,
                    dtCI
                    );
                //dt = listEquipmentItem.toDataTable();
                //rptCI.DataSource = dt;
                //rptCI.DataBind();
                //ClientService.DoJavascript("$('#tableItems').DataTable();");
                //udpSearchConfigurationItem.Update();

                var dataSource = listEquipmentItem.Select(s => new
                {
                    s.EquipmentCode,
                    s.Description,
                    s.EquipmentTypeName,
                    s.Status,
                    s.EquipmentClassName,
                    s.CategoryCode,
                    s.OwnerGroupName,
                    Selected = listEquipmentNew.Contains(s.EquipmentCode)
                });
                JArray data = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(dataSource));
                JArray datastatus = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(dtEquipmentStatus));
                udpSearchConfigurationItem.Update();
                ClientService.DoJavascript("afterSearchBindEquipmentCriteria(" + data + "," + datastatus + ");");
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

        /// <summary>
        /// return selected EquimentCode
        /// </summary>
        private void setCIDefult()
        {
            listCICode.Clear();
            string CI = txtSearchHelp_DataCISelect.Text.ToString();
            string[] CIList = CI.Split(',');
            for (int index = 0; index < CIList.Length; index++)
            {
                System.Diagnostics.Debug.WriteLine(CIList[index]);
                listCICode.Add(CIList[index]);
            }
        }

        protected void clear_ci_btn_Click(object sender, EventArgs e)
        {
            try
            {
                listCICode.Clear();
                hddClearCheck.Value = "true";
                udpSearchConfigurationItem.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }
    }
}