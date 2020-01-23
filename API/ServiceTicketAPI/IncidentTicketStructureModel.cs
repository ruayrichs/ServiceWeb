using Agape.Lib.DBService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.API.ServiceTicketAPI
{
    public class IncidentTicketStructureModel
    {
        public static List<IncidentTicketStructureModel> GetProductCategory(string SID, string AreaCode)
        {
            string sql = @"select * from ERPW_INCIDENT_TICKET_STRUCTURE_ITEM where sid = '" + SID +
                "' and AreaCode = '" + AreaCode +
                "' order by StructureName";

            DataTable dt = new DBService().selectDataFocusone(sql);
            return JsonConvert.DeserializeObject<List<IncidentTicketStructureModel>>(JsonConvert.SerializeObject(dt));
        }
        public string SID { get; set; }
        public string AreaCode { get; set; }
        public string StructureCode { get; set; }
        public string StructureName { get; set; }
        public string NodeHierarchyCode { get; set; }
        public string NodeParentCode { get; set; }
        public int NodeLevel { get; set; }
        public string Description { get; set; }
        public string created_on { get; set; }
        public string created_by { get; set; }
        public string updated_on { get; set; }
        public string updated_by { get; set; }
        public int CountItem { get; set; }
    }
}