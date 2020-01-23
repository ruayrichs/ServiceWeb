using Agape.Lib.DBService;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using ServiceWeb.LinkFlowChart.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.API.ServiceTicketAPI
{
    public class ServiceTicketStructureModel
    {
        public static List<ServiceTicketStructureModel> GetTicketStructure(string SID, string CompanyCode,
            string TicketNo, string CustomerCode)
        {
            List<ServiceTicketStructureModel> en = new List<ServiceTicketStructureModel>();

            LinkFlowChartService.DiagramRelation dataEn = LinkFlowChartService.getDiagramRelation(
                 SID,
                 CompanyCode,
                 TicketNo,
                 LinkFlowChartService.ItemGroup_TICKET,
                 CustomerCode
             );
            var lastParent = dataEn.parentNode.Where(w => w.Level == dataEn.parentNode.Max(m => m.Level)).First();
            if (lastParent.Level > 0)
            {
                dataEn = LinkFlowChartService.getDiagramRelation(
                     SID,
                     CompanyCode,
                     lastParent.ItemCode,
                     LinkFlowChartService.ItemGroup_TICKET
                 );
            }

            List<string> listTicket = dataEn.chindNode.Select(s => s.ItemCode).ToList();
            DataTable dtDocType = new DBService().selectDataFocusone(
                @"select Doctype from cs_servicecall_header where CallerID in ('" + string.Join("', '", listTicket) + "')"
            );
            List<string> listDocType = dtDocType.AsEnumerable().Select(s => Convert.ToString(s["Doctype"])).ToList();

            DataTable dtPrefix = new ServiceTicketLibrary().getDataPrefixDocType(SID, CompanyCode, listDocType);

            List<String> listTicketCode = dataEn.chindNode.Select(s => s.ItemCode).ToList();
            listTicketCode.AddRange(dataEn.parentNode.Select(s => s.ItemCode).ToList());

            string sql_getTicket = @"select a.CallerID, a.Docstatus, a.CallStatus, a.CustomerCode, Fiscalyear, Doctype
                                    from cs_servicecall_header a
                                    where a.sid = '" + SID + @"'
	                                    and a.CompanyCode = '" + CompanyCode + @"'
	                                    and a.CallerID in ('" + string.Join("', '", listTicketCode) + @"')";
            DataTable dtTicket = new DBService().selectDataFocusone(sql_getTicket); 

            dataEn.chindNode.ForEach(r =>
            {
                DataRow[] drrTicket = dtTicket.Select("CallerID = '" + r.ItemCode + "'");

                string ticketNoDisplay = "";
                DataRow[] drr = dtPrefix.Select("'" + r.ItemCode + "' like PrefixCode + '%'");
                if (drr.Length > 0)
                {
                    string prefix = drr[0]["PrefixCode"].ToString();

                    ticketNoDisplay = r.ItemCode;
                    //for (int i = 0; i < prefix.Length; i++)
                    //{
                    //    ticketNoDisplay = ticketNoDisplay.Replace(prefix[i].ToString(), "");
                    //}

                    ticketNoDisplay = ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay);//  prefix + Convert.ToInt32(ticketNoDisplay);
                }
                else
                {
                    ticketNoDisplay = r.ItemCode;
                }

                en.Add(new ServiceTicketStructureModel
                {
                    SID = SID,
                    CustomerCode = drrTicket[0]["CustomerCode"].ToString(),
                    StructureCode = r.ItemCode,
                    StructureName = ticketNoDisplay,
                    TicketCode = ticketNoDisplay,
                    NodeHierarchyCode = r.ItemCode,
                    NodeParentCode = r.ParentItemCode,
                    NodeLevel = r.Level,
                    Description = ticketNoDisplay,
                    created_on = "",
                    created_by = "",
                    updated_on = "",
                    updated_by = "",
                    CountItem = 0,
                    CallStatus = drrTicket[0]["CallStatus"].ToString(),
                    Doctype = drrTicket[0]["Doctype"].ToString(),
                    Fiscalyear = drrTicket[0]["Fiscalyear"].ToString(),
                    IsCloseTicket = drrTicket[0]["CallStatus"].ToString() != ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN
                });
            });

            return en;
        }
        public static List<ServiceTicketStructureModel> GetProductCategory(string SID, string CustomerCode)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(CustomerCode))
            {
                condition = " and a.CustomerCode = '" + CustomerCode + @"' ";
            }

            string sql = @"select a.[SID] ,a.[CustomerCode] ,a.[StructureCode],a.[NodeHierarchyCode], a.StructureName as TicketCode
	                            , '(' + b.Doctype + ' : ' + c.Description + ') ' + a.StructureName + ' : ' +  b.HeaderText as StructureName
	                            , a.[NodeParentCode] ,a.[NodeLevel] ,a.[Description] 
	                            , a.[created_on] ,a.[created_by] ,a.[updated_on] ,a.[updated_by]
                            from ERPW_REFERENT_FROM_TICKET_STRUCTURE_ITEM a
                            inner join cs_servicecall_header b
	                            on a.SID = b.SID
	                            and a.StructureName = b.CallerID
                            inner join master_config_cs_doctype c
	                            on a.SID = c.SID
	                            and b.Doctype = c.DocumentTypeCode

                            where a.sid = '" + SID + @"' 
                                " + condition + @"
                            order by StructureName
";
            //and CustomerCode = '" + CustomerCode + @"'

            DataTable dt = new DBService().selectDataFocusone(sql);
            return JsonConvert.DeserializeObject<List<ServiceTicketStructureModel>>(JsonConvert.SerializeObject(dt));
        }
        public string SID { get; set; }
        public string CustomerCode { get; set; }
        public string StructureCode { get; set; }
        public string StructureName { get; set; }
        public string TicketCode { get; set; }
        public string NodeHierarchyCode { get; set; }
        public string NodeParentCode { get; set; }
        public int NodeLevel { get; set; }
        public string Description { get; set; }
        public string created_on { get; set; }
        public string created_by { get; set; }
        public string updated_on { get; set; }
        public string updated_by { get; set; }
        public int CountItem { get; set; }
        public string CallStatus { get; set; }
        public bool IsCloseTicket { get; set; }
        public string Fiscalyear { get; set; }
        public string Doctype { get; set; }
        
    }
}