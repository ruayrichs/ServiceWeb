using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class OwnerService
    {
        private DBService dbService = new DBService();
        public void addMapingOwner(String SID, String CompanyCode, String EmployeeCode, List<String> ownerService)
        {

            String sql = @"INSERT INTO ERPW_OwnerService_Maping_Employee (SID, CompanyCode, EmployeeCode, OwnerService) VALUES ";

            foreach (String item in ownerService)
            {
                    sql += "('"+SID+"','"+CompanyCode+"','"+EmployeeCode+"', '"+item+"'),";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            sql+=";";

            dbService.selectDataFocusone(sql);
        }

        public DataTable getMappingOwner(String EmployeeCode)
        {
            String sql = @"SELECT 
                                a.EmployeeCode, a.OwnerService, b.OwnerGroupName 
                           FROM 
                                ERPW_OwnerService_Maping_Employee a 
                           LEFT JOIN 
                                ERPW_OWNER_GROUP b  ON  a.OwnerService = b.OwnerGroupCode
                           WHERE 
                                EmployeeCode = '" + EmployeeCode + "'";

          return  dbService.selectDataFocusone(sql);
            
        }
        public void clearDataForUpdate(String EmployeeCode)
        {
            String sql = @"DELETE FROM ERPW_OwnerService_Maping_Employee WHERE EmployeeCode = '" + EmployeeCode + "'";
            dbService.selectDataFocusone(sql);
        }
    }
}