using System;
using Agape.Lib.DBService;
using System.Data;
using ERPW.Lib.Service;

namespace ServiceWeb.Service
{
    
    public class CalendarService
    {
        private DBService dbService = new DBService();
        
        public  DataTable getNextMaintenanceTime()
        {
            string sql = "select * from ERPW_master_equipment_warranty WHERE NextMaintenanceDate != ''";
            DataTable dtWarranty = dbService.selectDataFocusone(sql);
            return dtWarranty;
        }
      
        public DataTable getAllChangeOrder()
        {
            string sql = @"select distinct a.Doctype, a.PlanStartDate, a.PlanStartTime, a.PlanEndDate, a.PlanEndTime, b.CallerID, b.HeaderText, a.ObjectID, b.ObjectID, a.Fiscalyear, c.PrefixCode, e.FirstName , e.LastName
                        FROM cs_servicecall_item a 
                        JOIN cs_servicecall_header b 
                        ON a.ObjectID = b.ObjectID 
                        LEFT JOIN master_config_cs_nr c
                        ON a.Doctype = c.NumberRangeCode
                        INNER JOIN ERPW_BUSINESSOBJECT_MAPPING_TICKET_TYPE f WITH (NOLOCK)  
                        ON b.SID = f.SID AND b.Doctype = f.TicketType
                        INNER JOIN CRM_SERVICECALL_MAPPING_ACTIVITY k 
						ON k.ServiceDocNo = b.CallerID
						INNER JOIN ticket_service_header t
						ON t.TicketCode = k.AOBJECTLINK
						LEFT JOIN master_employee e 
						ON t.MainDelegate = e.EmployeeCode
                        WHERE f.BusinessObject NOT IN ('" + ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT + @"'
                            ,'" + ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST + @"'
                            ,'" + ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM + @"') 
                        AND a.PlanStartDate != '' AND a.PlanEndDate != ''";

            DataTable dtChangeOrder = dbService.selectDataFocusone(sql);
            return dtChangeOrder;
        }

        public DataTable getAllTicketCallbackDateTime()
        {
            string sql = @" SELECT DISTINCT h.CallerID, h.Doctype, h.HeaderText, c.PrefixCode, h.Fiscalyear, h.CallbackDate, h.CallbackTime, e.FirstName, e.LastName
                            FROM cs_servicecall_header h
                            LEFT JOIN master_config_cs_nr c
                            ON h.Doctype = c.NumberRangeCode
                            INNER JOIN CRM_SERVICECALL_MAPPING_ACTIVITY k 
							ON k.ServiceDocNo = h.CallerID
							INNER JOIN ticket_service_header t
							ON t.TicketCode = k.AOBJECTLINK
							LEFT JOIN master_employee e 
							ON t.MainDelegate = e.EmployeeCode
                            WHERE h.CallbackDate != '' AND h.CallbackTime != ''";
            DataTable dtHaveCallback = dbService.selectDataFocusone(sql);
            return dtHaveCallback;
        }

    }
}