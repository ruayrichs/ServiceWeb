using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ServiceWeb.API.Model.Common
{
    public class CommonResponseModel
    {
        public string resultCode { get; set; }
        public string _resultTime { get; set; }
        public string resultTime { get { return string.IsNullOrEmpty(_resultTime) ? DateTime.Now.ToString() : _resultTime; } set { _resultTime = value; } }
        public string message { get; set; }
    }
}