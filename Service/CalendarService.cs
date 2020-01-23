using System;
using Agape.Lib.DBService;
using System.Data;
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
            string sql = "select distinct a.Doctype, a.PlanStartDate, a.PlanStartTime, a.PlanEndDate, a.PlanEndTime, b.CallerID, b.HeaderText, a.ObjectID, b.ObjectID, a.Fiscalyear, c.PrefixCode " +
                        "FROM cs_servicecall_item a " +
                        "JOIN cs_servicecall_header b " +
                        "ON a.ObjectID = b.ObjectID " +
                        "LEFT JOIN master_config_cs_nr c " +
                        "ON a.Doctype = c.NumberRangeCode " +
                        "WHERE a.Doctype NOT IN ('I','R','P') AND a.PlanStartDate != '' AND a.PlanEndDate != ''";

            DataTable dtChangeOrder = dbService.selectDataFocusone(sql);
            return dtChangeOrder;
        }

    }
}