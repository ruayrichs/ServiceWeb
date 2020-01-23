using ServiceWeb.API.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ServiceWeb.API.Model.Respond
{
    public class TicketDetailResponseModel : CommonResponseModel
    {
        public TicketDetailModel data { get; set; }
    }
    public class TicketDetailModel
    {
        public string TicketType { get; set; }
        public string TicketNumber { get; set; }
        public string TicketNumberDisplay { get; set; }
        public string TicketStatusCode { get; set; }
        public string TicketStatusDesc { get; set; }
        public string TicketDate { get; set; }
        public string TicketCallBackDate { get; set; }
        public string TicketCallBackTime { get; set; }
        public string ReferenceExternalTicket { get; set; }
        public string TicketCreatedBy { get; set; }
        public string TicketCreatedDateTime { get; set; }

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ContactCode { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactRemark { get; set; }

        public string TicketImpact { get; set; }
        public string TicketUrgency { get; set; }
        public string TicketPriority { get; set; }
        public string TicketSubject { get; set; }
        public string TicketDescription { get; set; }

        public string CICode { get; set; }
        public string CIName { get; set; }
        public string CIFamily { get; set; }
        public string CIClass { get; set; }
        public string CICategory { get; set; }
        public string CISerialNo { get; set; }

        public string OwnerService { get; set; }
        public string IncidentGroup { get; set; }
        public string IncidentType { get; set; }
        public string IncidentSource { get; set; }
        public string ContactSource { get; set; }

        public string AffectSLA { get; set; }
        public string SummaryProblem { get; set; }
        public string SummaryCause { get; set; }
        public string SummaryResolution { get; set; }

        public List<AssignToModel> AssignTo { get; set; }
        //public string Ticket { get; set; }
        //public string Ticket { get; set; }
        //public string Ticket { get; set; }
        //public string Ticket { get; set; }
        //public string Ticket { get; set; }
    }

    public class AssignToModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IsMainDelegate { get; set; }
    }
}