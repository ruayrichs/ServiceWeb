using agape.lib.constant;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using Newtonsoft.Json;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Tier.UserControl
{
    public partial class PopupCreateTierZeroControl : System.Web.UI.UserControl
    {
        private TierZeroLibrary libTierZero = new TierZeroLibrary();
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private UniversalService universalService = new UniversalService();

        private string _SID;
        protected string SID
        {
            get
            {
                if (_SID == null)
                {
                    _SID = ERPWAuthentication.SID;
                }
                return _SID;
            }
        }

        private string _CompanyCode;
        protected string CompanyCode
        {
            get
            {
                if (_CompanyCode == null)
                {
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                }
                return _CompanyCode;
            }
        }

        DataTable dtPriority
        {
            get { return Session["ServiceCallCriteria.SCT_dtPriority"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCT_dtPriority"]; }
            set { Session["ServiceCallCriteria.SCT_dtPriority"] = value; }

        }

        DataTable dtTempDoc
        {
            get
            {
                if (Session["SC_dtTempDoc"] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("doctype");
                    dt.Columns.Add("docnumber");
                    dt.Columns.Add("docfiscalyear");
                    dt.Columns.Add("indexnumber");
                    Session["SC_dtTempDoc"] = dt;
                }
                return (DataTable)Session["SC_dtTempDoc"];
            }
            set { Session["SC_dtTempDoc"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initDataCloaseCase();
            }
        }

        private void initData()
        {
            //initDataListType();
            initDataCloaseCase();
            //bindListTierZero();
            //GetEquipmentMappingOwner();
            GetddlSctype();
            GetImpact();
            GetUrgency();
            GetPriority();
        }

        private void clarInputData()
        {

        }

        private void initDataCloaseCase()
        {
            ddlCloaseCase.Items.Insert(0, new ListItem("Select", ""));
            ddlCloaseCase.Items.Insert(1, new ListItem(TierZeroLibrary.TIER_ZERO_STATUS_COMPLETED_DESC, TierZeroLibrary.TIER_ZERO_STATUS_COMPLETED));
            ddlCloaseCase.Items.Insert(2, new ListItem(TierZeroLibrary.TIER_ZERO_STATUS_CANCEL_DESC, TierZeroLibrary.TIER_ZERO_STATUS_CANCEL));
        }

        private void GetddlSctype()
        {
            _ddl_sctype.Items.Clear();
            DataTable dtSCType = AfterSaleService.getInstance().getSearchDoctype("", ERPWAuthentication.CompanyCode, true, false);
            _ddl_sctype.Items.Add(new ListItem("", ""));
            _ddl_sctype.AppendDataBoundItems = true;
            _ddl_sctype.DataTextField = "Description";
            _ddl_sctype.DataValueField = "DocumentTypeCode";
            _ddl_sctype.DataSource = dtSCType;
            _ddl_sctype.DataBind();
        }

        private void GetImpact()
        {
            DataTable dt = lib.GetImpactMaster(ERPWAuthentication.SID);
            ddlImpact.DataTextField = "ImpactName";
            ddlImpact.DataValueField = "ImpactCode";
            ddlImpact.DataSource = dt;
            ddlImpact.DataBind();
            ddlImpact.Items.Insert(0, new ListItem("", ""));
        }

        private void GetUrgency()
        {
            DataTable dt = lib.GetUrgencyMaster(ERPWAuthentication.SID);
            ddlUrgency.DataTextField = "UrgencyName";
            ddlUrgency.DataValueField = "UrgencyCode";
            ddlUrgency.DataSource = dt;
            ddlUrgency.DataBind();
            ddlUrgency.Items.Insert(0, new ListItem("", ""));
        }

        private void GetPriority()
        {
            dtPriority = lib.GetSeverity(ERPWAuthentication.SID, "", "", "");
            DataTable dt = dtPriority.Clone();
            if (dtPriority.Rows.Count > 0)
            {
                dt = dtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
            }
            ddlPriority.DataSource = dt;
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("", ""));

            if (dtPriority.Rows.Count > 0)
            {
                ddlPriority.SelectedIndex = 0;
            }

        }

        private void GetSeverity()
        {
            string impactCode = ddlImpact.SelectedValue;
            string urgencyCode = ddlUrgency.SelectedValue;


            DataTable dt = dtPriority.Clone();
            DataRow[] drr = dtPriority.Select("ImpactCode='" + impactCode + "' and UrgencyCode='" + urgencyCode + "' ");
            if (drr.Length > 0)
            {
                dt = drr.CopyToDataTable();
            }
            else
            {
                dt = dtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
            }
            ddlPriority.Items.Clear();
            ddlPriority.DataSource = dt;
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("", ""));
            if (drr.Length == 1)
            {
                ddlPriority.SelectedValue = drr[0]["PriorityCode"].ToString();
            }
            else
            {
                ddlPriority.SelectedValue = "";
            }
        }

        protected void ddlSelectBindPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSeverity();
                udpnProblem.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        //private void GetEquipmentMappingOwner()
        //{
        //    string equipmentCode = equipmentSelect.SelectedValue.Trim();
        //    string customerCode = AutoCompleteCustomer.SelectedValue.Trim();

        //    if (customerCode == "")
        //    {
        //        ClientService.DoJavascript("loadEquipmentWithoutCondition" + equipmentSelect.ClientID + "(); loadCustomerWithoutCondition" + AutoCompleteCustomer.ClientID + "();");
        //    }
        //    else
        //    {
        //        DataTable dt = universalService.GetEquipmentCustomerAssignment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", customerCode);

        //        List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource> result = new List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource>();

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            string equipmentName = lib.PrepareCodeAndDescription(dr["EquipmentCode"].ToString(), dr["EquipmentName"].ToString());

        //            result.Add(new ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource
        //            {
        //                code = dr["EquipmentCode"].ToString(),
        //                desc = dr["EquipmentName"].ToString(),
        //                display = equipmentName
        //            });
        //        }

        //        GC.Collect();

        //        string defaultValue = "";

        //        if (result.Count == 1)
        //        {
        //            defaultValue = result[0].display;
        //        }
        //        else
        //        {
        //            if (equipmentCode != "")
        //            {
        //                var en = result.Find(x => x.code == equipmentCode);
        //                if (en != null)
        //                {
        //                    defaultValue = en.display;
        //                }
        //            }
        //        }

        //        string responseJson = JsonConvert.SerializeObject(result);
        //        equipmentSelect.SelectedDisplay = defaultValue;
        //        ClientService.DoJavascript("bindAutoCompleteEquipment" + equipmentSelect.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
        //    }
        //}

        private void GetEquipmentMappingOwner()
        {
            string equipmentCode = equipmentSelect.SelectedValue.Trim();
            string customerCode = AutoCompleteCustomer.SelectedValue.Trim();

            if (customerCode == "")
            {
                ClientService.DoJavascript("loadEquipmentWithoutCondition" + equipmentSelect.ClientID + "(); loadCustomerWithoutCondition" + AutoCompleteCustomer.ClientID + "();");
            }
            else
            {
                DataTable dt = universalService.GetEquipmentCustomerAssignment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", customerCode);

                List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource> result = new List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource>();

                foreach (DataRow dr in dt.Rows)
                {
                    string equipmentName = lib.PrepareCodeAndDescription(dr["EquipmentCode"].ToString(), dr["EquipmentName"].ToString());

                    result.Add(new ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource
                    {
                        code = dr["EquipmentCode"].ToString(),
                        desc = dr["EquipmentName"].ToString(),
                        display = equipmentName
                    });
                }

                GC.Collect();

                string defaultValue = "";

                if (result.Count == 1)
                {
                    defaultValue = result[0].display;
                }
                else
                {
                    if (equipmentCode != "")
                    {
                        var en = result.Find(x => x.code == equipmentCode);
                        if (en != null)
                        {
                            defaultValue = en.display;
                        }
                    }
                }

                string responseJson = JsonConvert.SerializeObject(result);
                equipmentSelect.SelectedDisplay = defaultValue;
                ClientService.DoJavascript("bindAutoCompleteEquipment" + equipmentSelect.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
            }
        }

        public void btnCreateClose_Click(object sender, EventArgs e)
        {
            try
            {
                string SEQ = hdfSEQ_Target.Value;
                string closecase = ddlCloaseCase.SelectedValue;
                libTierZero.UpdateTierZeroStatus(SID, CompanyCode, SEQ, closecase);
                ClientService.DoJavascript("$('#btnRebindTierZeroList').click();");
                //string selectType = ddlListType.SelectedValue;
                //rptListDataUpload.DataSource = libTierZero.getTierZeroList(SID, CompanyCode, selectType);
                //rptListDataUpload.DataBind();
                //updFastService.Update();
                ClientService.AGSuccess("อัพเดตข้อมูลแล้ว");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
            ClientService.DoJavascript("location.reload();");
        }

        protected void btnCreateTicket_Click(object sender, EventArgs e)
        {

            try
            {
                initData();

                string SEQ = hdfSEQ_Target.Value;
                TierZeroEn Detail_SEQ = libTierZero.getTierZeroDetail(SID, CompanyCode, SEQ);
                //System.Diagnostics.Debug.WriteLine("SEQ is "+SEQ);
                DataTable dt = lib.findTicketNumberBySEQ(SID, CompanyCode, SEQ);
                //System.Diagnostics.Debug.WriteLine(dt.Rows.Count);
                if (!string.IsNullOrEmpty(dt.Rows[0]["TicketNumber"].ToString()))
                {
                    ClientService.DoJavascript("document.getElementById('btnRebindTierZeroList').click();");
                    throw new Exception("ไม่สามมารถสร้างเป็น Ticket ได้ เนื่องจากถูกนำไปสร้าง Ticket แล้ว ในเลข Ticket : " + dt.Rows[0]["TicketNumber"].ToString());
                    //return;
                }
                txtSubject.Text = Detail_SEQ.Subject;
                txtDetail.Text = Detail_SEQ.Detail;
                udpSubject.Update();

                //txtCustomer.Text = Detail_SEQ.CustomerCode + " : " + Detail_SEQ.CustomerName;
                //hddCustomerCode.Value = Detail_SEQ.CustomerCode;
                //hddCustomerName.Value = Detail_SEQ.CustomerName;
                

                if (!string.IsNullOrEmpty(Detail_SEQ.CustomerCode))
                {
                    AutoCompleteCustomer.SelectedValue = Detail_SEQ.CustomerCode;
                }
                else
                {
                    AutoCompleteCustomer.SelectedValue = "";
                }

                GetEquipmentMappingOwner();
                //udpCI.Update();

                _ddl_sctype.SelectedIndex = 0;
                ddlImpact.SelectedIndex = 0;
                ddlUrgency.SelectedIndex = 0;
                ddlPriority.SelectedIndex = 0;
                udpnProblem.Update();

                ClientService.DoJavascript("$('#modal_Create_Ticket').modal('show');");
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

        protected void btnCreatedTicket_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                if (string.IsNullOrEmpty(_ddl_sctype.SelectedValue))
                    message += "กรุณาระบุ ประเภทใบแจ้งบริการ <br/>";
                if (!string.IsNullOrEmpty(message))
                    throw new Exception(message);

                UniversalService universalService = new UniversalService();

                string equipmentCode = equipmentSelect.SelectedValue;
                string customerCode = AutoCompleteCustomer.SelectedValue;
                DataTable dt = universalService.GetEquipmentCustomerAssignment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, equipmentCode, customerCode);
                if (dt.Rows.Count <= 0)
                {
                    throw new Exception("The Configuration Item= " + equipmentCode + " and customer= " + customerCode + " not accord.");
                }

                bool hasTicket = universalService.CheckOpenTicket(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                    _ddl_sctype.SelectedValue, customerCode, equipmentSelect.SelectedValue);

                if (hasTicket)
                {
                    ClientService.DoJavascript("warningClick('" + _ddl_sctype.SelectedValue + "', '" + customerCode + "', '" + equipmentSelect.SelectedValue + "');");
                }
                else
                {
                    CreateTicket();
                }
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

        protected void btnCreateWithWarning_Click(object sender, EventArgs e)
        {
            try
            {
                CreateTicket();
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

        private void CreateTicket()
        {
            string idGen = Guid.NewGuid().ToString().Substring(0, 8);

            Session["SCT_created_doctype_code" + idGen] = _ddl_sctype.SelectedValue;
            Session["SCT_created_doctype_desc" + idGen] = _ddl_sctype.SelectedItem.Text;
            Session["SCT_created_cust_code" + idGen] = AutoCompleteCustomer.SelectedValue;
            Session["SCT_created_cust_desc" + idGen] = AutoCompleteCustomer.SelectedText;
            Session["SCT_created_contact_code" + idGen] = "";
            Session["SCT_created_contact_desc" + idGen] = "";
            Session["SCT_created_fiscalyear" + idGen] = DateTime.Now.Year.ToString();
            Session["SCT_created_remark" + idGen] = txtSubject.Text;
            Session["SCT_created_description" + idGen] = txtDetail.Text;
            Session["SCT_created_equipment" + idGen] = equipmentSelect.SelectedValue;
            Session["SCT_created_impact" + idGen] = ddlImpact.SelectedValue;
            Session["SCT_created_urgency" + idGen] = ddlUrgency.SelectedValue;
            Session["SCT_created_priority" + idGen] = ddlPriority.SelectedValue;
            Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
            Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;

            Session["SC_MODE_REF" + idGen] = ServiceTicketLibrary.TICKET_CREATE_MODE_TIERZERO;
            Session["SC_MODE_REF_KEY" + idGen] = hdfSEQ_Target.Value;
            //System.Diagnostics.Debug.WriteLine(Session["SCT_created_remark" + idGen].ToString());
            dtTempDoc.Clear();
            //int oldCount = lib.countTicket(SID, CompanyCode);
            //int newCount = 0;
            ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen) + "');");
            //Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen));
            //do{
            //    System.Diagnostics.Debug.WriteLine("Old is " + oldCount + " : New is " + newCount);
            //    newCount = lib.countTicket(SID, CompanyCode);
            //} while (newCount < oldCount);
            //for (newCount = 0; newCount < oldCount; newCount = lib.countTicket(SID, CompanyCode))
            //{
            //    System.Diagnostics.Debug.WriteLine("Old is " + oldCount + " : New is " + newCount);
            //}
            //Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void btnCancelTierZero_Click(object sender, EventArgs e)
        {
            try
            {

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

        protected void btnBindContactCus_Click(object sender, EventArgs e)
        {
            try
            {
                GetEquipmentMappingOwner();
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


        protected void btnLoadCustomerEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                string equipmentCode = equipmentSelect.SelectedValue.Trim();
                string customerCode = AutoCompleteCustomer.SelectedValue.Trim();

                if (equipmentCode == "")
                {
                    ClientService.DoJavascript("loadEquipmentWithoutCondition" + equipmentSelect.ClientID + "();");
                    //ClientService.DoJavascript("loadEquipmentWithoutCondition" + equipmentSelect.ClientID + "(); loadCustomerWithoutCondition" + CustomerSelect.ClientID + "();");
                }
                else
                {
                    DataTable dt = universalService.GetEquipmentCustomerAssignment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, equipmentCode, "");

                    List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource> result = new List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        string customerName = lib.PrepareNameAndForiegnName(dr["CustomerName"].ToString(), dr["ForeignName"].ToString());

                        result.Add(new ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource
                        {
                            code = dr["OwnerCode"].ToString(),
                            desc = customerName,
                            display = customerName == "" ? dr["OwnerCode"].ToString() : dr["OwnerCode"] + " : " + customerName
                        });
                    }

                    GC.Collect();

                    string defaultValue = "";

                    if (result.Count == 1)
                    {
                        defaultValue = result[0].display;
                    }
                    else
                    {
                        if (customerCode != "")
                        {
                            var en = result.Find(x => x.code == customerCode);
                            if (en != null)
                            {
                                defaultValue = en.display;
                            }
                        }
                    }

                    string responseJson = JsonConvert.SerializeObject(result);
                    AutoCompleteCustomer.SelectedDisplay = defaultValue;
                    ClientService.DoJavascript("bindAutoCompleteCustomer" + AutoCompleteCustomer.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
                    //getcontact_person();
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
    }
}