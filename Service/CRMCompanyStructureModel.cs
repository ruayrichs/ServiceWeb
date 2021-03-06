﻿using Agape.Lib.DBService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    //[Serializable] // zaan
    public class CRMCompanyStructureModel
    {
        public static List<CRMCompanyStructureModel> GetProductCategory(string SID, string WorkGroupCode)
        {
            string sql = @"select * from LINK_PROJECT_COMPANY_STRUCTURE_ITEM where sid = '" + SID +
                "' and WorkGroupCode = '" + WorkGroupCode +
                "' order by StructureName";

            DataTable dt = new DBService().selectDataFocusone(sql);
            return JsonConvert.DeserializeObject<List<CRMCompanyStructureModel>>(JsonConvert.SerializeObject(dt));
        }
        public string SID { get; set; }
        public string WorkGroupCode { get; set; }
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