using Agape.Lib.DBService;
using ERPW.Lib.Master;
using Newtonsoft.Json;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using ERPW.Lib.Service;
using ServiceWeb.LinkFlowChart.Service;
using System.Web.Configuration;

namespace ServiceWeb.API
{
    public partial class AutoCompleteAPI : System.Web.UI.Page
    {
        private DBService dbService = new DBService();
        private ServiceWeb.Service.EquipmentService serEquipment = new ServiceWeb.Service.EquipmentService();
        private ServiceWeb.Service.OwnerService ownerService = new ServiceWeb.Service.OwnerService();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";

            string ActionCase = Request["actionCase"];

            if (ActionCase == "customer")
            {
                string keySearch = "";

                if (!string.IsNullOrEmpty(Request["keySearch"]))
                {
                    keySearch = Request["keySearch"];
                }

                GetCustomerData(keySearch);
            }
            else if (ActionCase == "equipment")
            {
                string keySearch = "";

                if (!string.IsNullOrEmpty(Request["keySearch"]))
                {
                    keySearch = Request["keySearch"];
                }

                GetEquipmentData(keySearch);
            }
            else if (ActionCase == "equipment_fordiagram")
            {
                string keySearch = "";

                if (!string.IsNullOrEmpty(Request["keySearch"]))
                {
                    keySearch = Request["keySearch"];
                }

                GetEquipmentForDiagramData(keySearch);
            }
            else if (ActionCase == "contact")
            {
                GetContactData();
            }
            else if (ActionCase == "employee")
            {
                GetEmployeeData();
            }
            else if (ActionCase == "contractTemplate")
            {
                GetContractTemplateData();
            }
            else if (ActionCase == "class_fordiagram")
            {
                string keySearch = "";

                if (!string.IsNullOrEmpty(Request["keySearch"]))
                {
                    keySearch = Request["keySearch"];
                }
                GetEquipmentClassTemplateData(keySearch);
            }
            else if (ActionCase == "ticket_fordiagram")
            {
                string keySearch = "";

                if (!string.IsNullOrEmpty(Request["keySearch"]))
                {
                    keySearch = Request["keySearch"];
                }
                GetTicketForDiagramData(keySearch);
            }
        }

        private void GetCustomerData(string keySearch)
        {
            string sql = @"SELECT TOP(1000) a.CustomerCode, a.CustomerName, a.ForeignName
                            FROM master_customer a
                            INNER JOIN master_customer_general b
                                ON a.SID = b.SID 
                                AND a.CompanyCode = b.CompanyCode
                                AND a.CustomerCode = b.CustomerCode
                            WHERE a.SID = '" + ERPWAuthentication.SID + "' AND a.CompanyCode = '" + ERPWAuthentication.CompanyCode + @"' AND b.Active='True'";

            
            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);


            // #Edit Multi Owner Customer
            DataTable dt_owner = ownerService.getMappingOwner(ERPWAuthentication.EmployeeCode);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                if (dt_owner.Rows.Count > 0)
                {
                    sql += @" and";
                }
                foreach (DataRow dr in dt_owner.Rows)
                {
                    if (dr == dt_owner.Rows[dt_owner.Rows.Count - 1])
                    {
                        sql += @" b.BPChannalCode = '" + dr["OwnerService"].ToString() + "'";
                    }
                    else
                    {
                        sql += @" b.BPChannalCode = '" + dr["OwnerService"].ToString() + "' or";
                    }
                }
               
            }

            if (keySearch != "")
            {
                sql += " AND (a.CustomerCode LIKE '%" + keySearch + "%' OR a.CustomerName LIKE '%" + keySearch + "%' OR a.ForeignName LIKE '%" + keySearch + "%')";
            }

            sql += " ORDER BY a.CustomerCode";

            DataTable dt = dbService.selectDataFocusone(sql);

            List<AutoCompleteSource> result = new List<AutoCompleteSource>();

            CustomerService libCustomer = new CustomerService();

            foreach (DataRow dr in dt.Rows)
            {
                string customerName = libCustomer.PrepareCustomerNameAndForeignName(dr["CustomerName"].ToString(), dr["ForeignName"].ToString());

                result.Add(new AutoCompleteSource
                {
                    code = dr["CustomerCode"].ToString(),
                    desc = customerName,
                    display = customerName == "" ? dr["CustomerCode"].ToString() : dr["CustomerCode"] + " : " + customerName
                });
            }
        
            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(result);

            Response.Write(responseJson);
        }

        private void GetEquipmentData(string keySearch)
        {
            string sql = @"SELECT TOP(1000) a.EquipmentCode, a.[Description] 
                            FROM master_equipment a
                            left join master_equipment_general b
                                ON b.sid = a.sid
	                            AND b.companyCode = a.companycode
	                            AND b.EquipmentCode = a.EquipmentCode
                            WHERE a.SID = '" + ERPWAuthentication.SID + @"' 
                                AND a.CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'";

            if (keySearch != "")
            {
                sql += " AND (a.EquipmentCode LIKE '%" + keySearch + "%' OR a.[Description] LIKE '%" + keySearch + "%')";
            }

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            // #Edit Multi Owner CI
            DataTable dt_owner = ownerService.getMappingOwner(ERPWAuthentication.EmployeeCode);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                if(dt_owner.Rows.Count > 0)
                {
                    sql += @" and";
                }
                foreach (DataRow dr in dt_owner.Rows)
                {
                    if (dr == dt_owner.Rows[dt_owner.Rows.Count - 1])
                    {
                        sql += @" b.EquipmentObjectType = '" + dr["OwnerService"].ToString() + "'";
                    }
                    else
                    {
                        sql += @" b.EquipmentObjectType = '" + dr["OwnerService"].ToString() + "' or";
                    }
                }

            }

            sql += " ORDER BY a.[Description]";

            DataTable dt = dbService.selectDataFocusone(sql);

            List<AutoCompleteSource> result = new List<AutoCompleteSource>();

            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new AutoCompleteSource
                {
                    code = dr["EquipmentCode"].ToString(),
                    desc = dr["Description"].ToString(),
                    display = string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["EquipmentCode"].ToString() : (dr["EquipmentCode"] + " : " + dr["Description"])
                });
            }

            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(result);

            Response.Write(responseJson);
        }

        private void GetContactData()
        {
            string customerCode = Request["customer"];

            string sql = @"SELECT a.BPCODE, b.BOBJECTLINK, b.NAME1, b.NAME2 FROM CONTACT_MASTER a
                            INNER JOIN CONTACT_DETAILS b
                            ON a.SID = b.SID AND a.AOBJECTLINK = b.AOBJECTLINK
                            WHERE a.SID = '" + ERPWAuthentication.SID + "' AND a.COMPANYCODE = '" + ERPWAuthentication.CompanyCode + @"' AND a.BPTYPE = 'C'
                            AND a.BPCODE = '" + customerCode + @"' AND b.ACTIVESTATUS = 'True'
                            ORDER BY b.AOBJECTLINK";

            DataTable dt = dbService.selectDataFocusone(sql);

            List<AutoCompleteSource> result = new List<AutoCompleteSource>();

            CustomerService libCustomer = new CustomerService();

            foreach (DataRow dr in dt.Rows)
            {
                string customerName = libCustomer.PrepareCustomerNameAndForeignName(dr["NAME1"].ToString(), dr["NAME2"].ToString());

                result.Add(new AutoCompleteSource
                {
                    code = dr["BOBJECTLINK"].ToString(),
                    desc = customerName,
                    display = customerName
                });
            }

            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(result);

            Response.Write(responseJson);
        }

        private void GetEmployeeData()
        {
            string sql = @"SELECT a.EmployeeCode, a.FirstName_TH + ' ' + a.LastName_TH AS NameTH, a.FirstName + ' ' +  a.LastName AS xName
                            FROM master_employee a
                            left join ERPW_Role_Maping_Employee b
                                ON a.SID = b.SID
                                AND a.CompanyCode = b.CompanyCode 
                                AND a.EmployeeCode = b.EmployeeCode
                            WHERE a.SID = '" + ERPWAuthentication.SID + @"'
                            AND a.CompanyCode = '" + ERPWAuthentication.CompanyCode + "'";

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            // #Edit Multi Owner Employee
            DataTable dt_owner = ownerService.getMappingOwner(ERPWAuthentication.EmployeeCode);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                if (dt_owner.Rows.Count > 0)
                {
                    sql += @" and";
                }
                foreach (DataRow dr in dt_owner.Rows)
                {
                    if (dr == dt_owner.Rows[dt_owner.Rows.Count - 1])
                    {
                        sql += @" b.OwnerService = '" + dr["OwnerService"].ToString() + "'";
                    }
                    else
                    {
                        sql += @" b.OwnerService = '" + dr["OwnerService"].ToString() + "' or";
                    }
                }

            }

            DataTable dt = dbService.selectDataFocusone(sql);

            List<AutoCompleteSource> result = new List<AutoCompleteSource>();

            CustomerService libCustomer = new CustomerService();

            foreach (DataRow dr in dt.Rows)
            {
                string name = libCustomer.PrepareCustomerNameAndForeignName(dr["NameTH"].ToString(), dr["xName"].ToString());

                result.Add(new AutoCompleteSource
                {
                    code = dr["EmployeeCode"].ToString(),
                    desc = Convert.ToString(dr["xName"]),
                    display = name == "" ? dr["EmployeeCode"].ToString() : dr["EmployeeCode"] + " : " + name
                });
            }

            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(result);

            Response.Write(responseJson);
        }

        private void GetContractTemplateData()
        {
            string sql = @"SELECT ContractName, [Description] 
                            FROM master_service
                            WHERE SID = '" + ERPWAuthentication.SID + "' AND CompanyCode = '" + ERPWAuthentication.CompanyCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            List<AutoCompleteSource> result = new List<AutoCompleteSource>();

            CustomerService libCustomer = new CustomerService();            

            foreach (DataRow dr in dt.Rows)
            {
                string name = libCustomer.PrepareCustomerNameAndForeignName(dr["ContractName"].ToString(), dr["Description"].ToString());

                result.Add(new AutoCompleteSource
                {
                    code = dr["ContractName"].ToString(),
                    desc = name,
                    display = name
                });
            }

            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(result);

            Response.Write(responseJson);
        }

        private void GetEquipmentClassTemplateData(string keySearch)
        {
            string sql = @"select ClassCode, ClassName from dbo.master_equipment_class
                            WHERE SID = '" + ERPWAuthentication.SID + "'";
            if (!string.IsNullOrEmpty(keySearch))
            {
                sql += " AND (ClassName like '%" + keySearch + "%' OR ClassCode like '%" + keySearch + "%')";
            }
            DataTable dt = dbService.selectDataFocusone(sql);

            List<AutoCompleteSource> result = new List<AutoCompleteSource>();

            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new AutoCompleteSource
                {
                    code = dr["ClassCode"].ToString(),
                    desc = dr["ClassName"].ToString(),
                    display = dr["ClassCode"].ToString() + " : " + dr["ClassName"].ToString()
                });
            }

            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(result);

            Response.Write(responseJson);
        }

        private void GetEquipmentForDiagramData(string keySearch)
        {
            string conditionClass = "";
            if (!string.IsNullOrEmpty(Request["IsRelation"]) && Convert.ToBoolean(Request["IsRelation"]))
            {
                string nodeCode = Request["nodeCode"];
                List<string> listClass = serEquipment.getEquipmentClassRelationConfig(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, nodeCode);

                if (listClass.Count == 0)
                {
                    Response.Write("[]");
                    return;
                }
                conditionClass = " and b.EquipmentClass in ('" + string.Join("', '", listClass) + "')";
            }
            string sql = @"select TOP 1000 a.*
                            from master_equipment a
                            inner join dbo.master_equipment_general b
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.EquipmentCode = b.EquipmentCode
                            WHERE a.SID = '" + ERPWAuthentication.SID + @"' 
                                AND a.CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'
                                " + conditionClass;

            if (keySearch != "")
            {
                sql += " AND (a.EquipmentCode LIKE '%" + keySearch + "%' OR a.[Description] LIKE '%" + keySearch + "%')";
            }

            sql += " ORDER BY [Description]";

            DataTable dt = dbService.selectDataFocusone(sql);

            List<AutoCompleteSource> result = new List<AutoCompleteSource>();

            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new AutoCompleteSource
                {
                    code = dr["EquipmentCode"].ToString(),
                    desc = dr["Description"].ToString(),
                    display = string.IsNullOrEmpty(dr["Description"].ToString()) ? dr["EquipmentCode"].ToString() : (dr["EquipmentCode"] + " : " + dr["Description"])
                });
            }

            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(result);

            Response.Write(responseJson);
        }

        private void GetTicketForDiagramData(string keySearch)
        {
            //string SERVICE_DOC_STATUS_OPEN = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(
            //    ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_START
            //);
            string CustomerCode = Request["OtherKey"];
            string conditionClass = "";
            if (!string.IsNullOrEmpty(Request["IsRelation"]) && Convert.ToBoolean(Request["IsRelation"]))
            {
                string nodeCode = Request["nodeCode"];

                if (string.IsNullOrEmpty(nodeCode))
                {
                    Response.Write("[]");
                    return;
                }

                LinkFlowChartService.DiagramRelation dataEn = LinkFlowChartService.getDiagramRelation(
                     ERPWAuthentication.SID,
                     ERPWAuthentication.CompanyCode,
                     nodeCode,
                     LinkFlowChartService.ItemGroup_TICKET,
                     CustomerCode
                 );
                if (!string.IsNullOrEmpty(CustomerCode))
                {
                    conditionClass += @" and CustomerCode = '" + CustomerCode + @"' ";
                }

                conditionClass += @" and CallerID <> '" + nodeCode + @"' 
                                    and CallerID <> '" + dataEn.parentNode[dataEn.parentNode.Count - 1].ItemCode + @"'
	                                and CallerID not in (
		                                select distinct ItemCode
		                                from ERPW_DIAGRAM_RELATION
		                                where sid = '" + ERPWAuthentication.SID + @"' 
			                                and CompanyCode = '" + ERPWAuthentication.CompanyCode + @"' 
			                                and ItemGroup = 'TICKET'
			                                and ParentItemCode <> '" + nodeCode + @"'
                                        union
                                        select distinct a.ParentItemCode
                                        from ERPW_DIAGRAM_RELATION a
                                        left join ERPW_DIAGRAM_RELATION b
	                                        on a.ParentItemCode = b.ItemCode
                                        where a.sid = '" + ERPWAuthentication.SID + @"' 
	                                        and a.CompanyCode = '" + ERPWAuthentication.CompanyCode + @"' 
	                                        and a.ItemGroup = 'TICKET'
	                                        and (b.ParentItemCode <> '" + nodeCode + @"' or b.ParentItemCode is null)
	                                ) ";
            }
            string sql = @"select * from cs_servicecall_header
                            where sid = '" + ERPWAuthentication.SID + @"'
	                            and CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'
	                            and CallStatus = '" + ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN + @"'
                                " + conditionClass;
                                //and Docstatus = '" + SERVICE_DOC_STATUS_OPEN + @"'
            
            if (!string.IsNullOrEmpty(keySearch))
            {
                keySearch = string.Join("%", keySearch.Split('-'));
                sql += " AND (CallerID LIKE '%" + keySearch + "%' or HeaderText LIKE '%" + keySearch + "%') ";
            }

            sql += " ORDER BY [CallerID]";

            DataTable dt = dbService.selectDataFocusone(sql);

            List<AutoCompleteSource> result = new List<AutoCompleteSource>();


            string sql_Prefix = @"SELECT a.DocumentTypeCode, b.PrefixCode
                            FROM master_config_cs_nr_mapping a
                            INNER JOIN master_config_cs_nr b 
	                            ON a.SID = b.SID
	                            AND a.CompanyCode = b.CompanyCode
	                            AND a.NumberRangeCode = b.NumberRangeCode
                            WHERE a.SID = '" + ERPWAuthentication.SID + @"'
	                            AND a.CompanyCode = '" + ERPWAuthentication.CompanyCode + @"'";
            DataTable dtPrefix = dbService.selectDataFocusone(sql_Prefix);

            foreach (DataRow dr in dt.Rows)
            {
                string ticketNoDisplay = "";
                DataRow[] drr = dtPrefix.Select("DocumentTypeCode = '" + dr["Doctype"] + "'");
                if (drr.Length > 0)
                {
                    string prefix = drr[0]["PrefixCode"].ToString();

                    ticketNoDisplay = dr["CallerID"].ToString();
                    for (int i = 0; i < prefix.Length; i++)
                    {
                        ticketNoDisplay = ticketNoDisplay.Replace(prefix[i].ToString(), "");
                    }

                    ticketNoDisplay = prefix + Convert.ToInt32(ticketNoDisplay);
                }
                else
                {
                    ticketNoDisplay = dr["CallerID"].ToString();
                }

                result.Add(new AutoCompleteSource
                {
                    code = dr["CallerID"].ToString(),
                    desc = string.IsNullOrEmpty(dr["HeaderText"].ToString()) ? ticketNoDisplay : (ticketNoDisplay + " : " + dr["HeaderText"].ToString()),
                    display = string.IsNullOrEmpty(dr["HeaderText"].ToString()) ? ticketNoDisplay : (ticketNoDisplay + " : " + dr["HeaderText"].ToString())
                });
            }

            GC.Collect();

            string responseJson = JsonConvert.SerializeObject(result);

            Response.Write(responseJson);
        }

        public class AutoCompleteSource
        {
            public string code { get; set; }
            public string desc { get; set; }
            public string display { get; set; }
        }
    }
}