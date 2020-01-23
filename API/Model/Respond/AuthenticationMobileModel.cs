using ServiceWeb.API.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ServiceWeb.API.Model.Respond
{
    public class AuthenticationMobileModel : CommonResponseModel
    {
        public string permissionKey { get; set; }
        public string username { get; set; }
        public string employeeCode { get; set; }
        public string employeeName { get; set; }
        public string OwnerCode { get; set; }
        public string OwnerName { get; set; }
    }
}