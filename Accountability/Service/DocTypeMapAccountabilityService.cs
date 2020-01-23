using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Agape.Lib.DBService;

namespace ServiceWeb.Accountability.Service
{
    public class DocTypeMapAccountabilityService
    {

        DBService dbService = new DBService();
        private static DocTypeMapAccountabilityService _instance;
        public static DocTypeMapAccountabilityService getInstance()
        {
            if (_instance == null)
            {
                _instance = new DocTypeMapAccountabilityService();
            }
            return _instance;
        }

        public  DataTable GetDoccumentTypeMappingAccountabilityData(string SID ,string companyCode) {
            string sql = "select * from ERPW_ACCOUNTABILITY_MAPPING_CHANGEORDER where SID = '"+SID+@"'  and  CompanyCode = '"+companyCode+@"' ";
            return dbService.selectDataFocusone(sql);
        }

        public void AddDoccumentTypeMappingAccountability(string SID ,string companyCode,string docTypeCode ,string accountabilityCode ,string createby) {
            string createon = Agape.FocusOne.Utilities.Validation.getCurrentServerStringDateTime();
            string sql = @"insert  into  ERPW_ACCOUNTABILITY_MAPPING_CHANGEORDER(SID, CompanyCode, DocTypeCode, AccountabilityCode, Created_By, Created_On) values (
                            '"+SID+@"',
                            '"+companyCode+@"', 
                            '"+docTypeCode+@"',
                            '"+accountabilityCode+@"',
                            '"+createby+@"',
                            '"+createon+@"');";
            dbService.executeSQLForFocusone(sql);
        }

        public void DeleteDocumentTypeMappingAccountability(string SID ,string companyCode,string docTypeCode , string accountabilityCode) {
            string sql = @"delete  from ERPW_ACCOUNTABILITY_MAPPING_CHANGEORDER where  
                            SID = '"+SID+@"' and 
                            CompanyCode = '"+companyCode+@"' and 
                            DocTypeCode = '"+docTypeCode+@"' and 
                            AccountabilityCode = '"+accountabilityCode+@"'";

            dbService.executeSQLForFocusone(sql);
        }

        public void EditDocumentTypeMappingAccountability(string SID, string companyCode, string OldDocTypeCode, string OldaccountabilityCode,string newDocTypeCode,string newAccountabilityCode) {
            string sql = @"update ERPW_ACCOUNTABILITY_MAPPING_CHANGEORDER 
                            set  DocTypeCode = '"+newDocTypeCode+@"' , 
                                 AccountabilityCode = '"+newAccountabilityCode+@"' 
                            where SID = '"+SID+@"' and  
                                  CompanyCode = '"+companyCode+@"' and  
                                  DocTypeCode = '"+OldDocTypeCode+@"' and  
                                  AccountabilityCode = '"+OldaccountabilityCode+@"'";
            dbService.executeSQLForFocusone(sql);
        }

        public DataTable GetDocumentTypeMappingAccountabilityDataByDocTypeCode(string SID , string CompanyCode,string DocTypeCode) {

            string sql = "select * from ERPW_ACCOUNTABILITY_MAPPING_CHANGEORDER where SID = '" + SID + @"'  and  CompanyCode = '" + CompanyCode + @"' and  DocTypeCode = '"+DocTypeCode+@"' ";
            return dbService.selectDataFocusone(sql);

        }
    }
}