using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class CustomerComplaintsService
    {
        DBService db = new DBService();


        public DataTable GetDocTyp()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DocumentTypeCode", typeof(System.String));
            dt.Columns.Add("Description", typeof(System.String));
            dt.Rows.Add("NCR", "Non Conforming Received");
            dt.Rows.Add("NCS", "Non Conforming Storage");
            dt.Rows.Add("NCC", "Non Conforming Customer Return");
            dt.Rows.Add("NCA", "Non Conforming Assurance");
            return dt;
        }

        public string saveDataCustomerComplaints(string SID, string CompanyCode,
            string FiscalYear, string DocType, string DocDate, string Status,
            string EmployeeReport, string EmployeeRespon, string MaterialCode,
            string BatchNumber, int TotalQTY, int QTY, string CustomerCode, string DateCheck, 
            string PO_SO, string AreaCheck, string AreaReferance, string Remark, string CreatedBy)
        {
            string DocNumber = genDocNumberCustomerComplaints(SID, CompanyCode, DocType);
            string sql = @"insert into LINK_CRM_CUSTOMER_COMPLAINTS
                            (
                               SID
                              ,CompanyCode
                              ,FiscalYear
                              ,DocType
                              ,DocNumber
                              ,DocDate
                              ,Status
                              ,EmployeeReport
                              ,EmployeeRespon
                              ,MaterialCode
                              ,BatchNumber
                              ,TotalQTY
                              ,QTY
                              ,CustomerCode
                              ,DateCheck
                              ,PO_SO
                              ,AreaCheck
                              ,AreaReferance
                              ,Remark
                              ,CreatedBy
                              ,CreatedOn
                            ) VALUES (
                               '" + SID + @"'
                              ,'" + CompanyCode + @"'
                              ,'" + FiscalYear + @"'
                              ,'" + DocType + @"'
                              ,'" + DocNumber + @"'
                              ,'" + DocDate + @"'
                              ,'" + Status + @"'
                              ,'" + EmployeeReport + @"'
                              ,'" + EmployeeRespon + @"'
                              ,'" + MaterialCode + @"'
                              ,'" + BatchNumber + @"'
                              ," + TotalQTY + @"
                              ," + QTY + @"
                              ,'" + CustomerCode + @"'
                              ,'" + DateCheck + @"'
                              ,'" + PO_SO + @"'
                              ,'" + AreaCheck + @"'
                              ,'" + AreaReferance + @"'
                              ,'" + Remark + @"'
                              ,'" + CreatedBy + @"'
                              ,'" + Validation.getCurrentServerStringDateTime() + @"'
                            )";
            
            db.executeSQLForFocusone(sql);
            return DocNumber;
        }

        public string genDocNumberCustomerComplaints(string SID, string CompanyCode, string DocType)
        {
            string sql = @"select * from LINK_CRM_CUSTOMER_COMPLAINTS
                            where SID = '" + SID + @"' 
                              AND CompanyCode = '" + CompanyCode + @"' 
                              AND DocType = '" + DocType + @"'
                            order by DocNumber desc";
            DataTable dt = db.selectDataFocusone(sql);

            string DocNumber = CompanyCode + "-" + DocType + "-";
            if (dt.Rows.Count == 0)
            {
                DocNumber += "C000000001";
            }
            else
            {
                int count = Convert.ToInt32(dt.Rows[0]["DocNumber"].ToString().Split('-')[2].Substring(1, 9)) + 1;
                DocNumber += "C" + (count.ToString().PadLeft(9, '0'));
            }
            return DocNumber;
        }

        public DataTable getMaterialCustomerComplaints(string SID)
        {
            string sql = @"select item.ItmNumber, item.ItmDescription, item.ItmGroup
                              , bgroup.batchgroup, bnumber.batchnumber
                              , item.ItemCat, general.BaseUOM as SalesUoM
                              , item.CurrCode , general.Style
                              , item.ItmDescription + ', Batch Number : ' + bnumber.batchnumber as ItemDescription
                              , item.ItmNumber + '|' + bnumber.batchnumber as itemCode
                            from master_mm_items item
                            inner join master_mm_item_general general
                              on general.SID = item.SID
                              and general.ItmNumber = item.ItmNumber
                            inner join mm_master_batchgroup bgroup
                              on bgroup.SID = item.SID 
                              AND bgroup.ItmNumber = item.ItmNumber
                            inner join mm_master_batchnumber bnumber
                              on bnumber.SID = item.SID
                              AND bnumber.batchgroup = bgroup.batchgroup
                            where item.SID = '" + SID + @"'";

            return db.selectDataFocusone(sql);
        }

        public DataTable getDataCustomerComplaints(string SID, string CompanyCode, string FiscalYear, 
            string DocType, string DocNumber, string DocDate)
        {
            string sql = @"select * from LINK_CRM_CUSTOMER_COMPLAINTS
                            where SID = '" + SID + @"' 
                              AND CompanyCode = '" + CompanyCode + @"' ";

            if (!String.IsNullOrEmpty(FiscalYear))
            {
                sql += " AND FiscalYear = '" + FiscalYear + @"' ";
            }
            if (!String.IsNullOrEmpty(DocType))
            {
                sql += " AND DocType = '" + DocType + @"' ";
            }
            if (!String.IsNullOrEmpty(DocNumber))
            {
                sql += " AND DocNumber = '" + DocNumber + @"' ";
            }
            if (!String.IsNullOrEmpty(DocDate))
            {
                sql += " AND DocDate = '" + DocDate + @"' ";
            }

            return db.selectDataFocusone(sql);
        }
    }
}