using ERPW.Lib.Service.Entity;
using ServiceWeb.API.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ServiceWeb.API.Model.Respond
{
    public class TicketCommentResponseModel : CommonResponseModel
    {
        public TicketCommentModel data { get; set; }
    }
    public class TicketCommentModel
    {
        public int TotalRemark { get; set; }
        public List<ActivityRemark> Remarks { get; set; }
        public List<ActivityRemarkTicketReply> RemarkTicketReplys { get; set; }
    }
}