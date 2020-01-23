using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class ProjectService
    {
        DBService db = new DBService();

        private static ProjectService _instance = null;

        public static ProjectService getInstance()
        {
            if (_instance == null)
                _instance = new ProjectService();
            return _instance;
        }

        public DataTable searchWorkGroupHeader(string sid, string company, string projectCode, string projectCategory)
        {
            string sql = @"
                        SELECT p.[SID]
                          ,p.[CompanyCode]
                          ,p.[ProjectCode]
                          ,p.[ProjectName]
                          ,p.[ProjectType]
                          ,p.[ProjectTarget]
                          ,p.[ProjectStatus]
                          ,p.[ProjectOverview]
                          ,p.[ProjectOwner]
                          ,p.[StartDate]
                          ,p.[EndDate]
                          ,p.[AccessType]   
                          ,p.[created_on]
                          ,p.[created_by]
                          ,p.[updated_on]
                          ,p.[updated_by]
                          ,p.[ProjectCategory]
	                     ,ISNULL((em.FirstName_TH +' '+em.LastName_TH),'') as ProjectOwnerFullName
                          FROM [dbo].[LINK_PROJECT_MASTER] p
    
                         left join [dbo].[master_employee] em
                          on p.SID= em.SID and p.CompanyCode= em.CompanyCode and p.ProjectOwner = em.EmployeeCode
                          where p.SID='" + sid + @"' and p.CompanyCode='" + company + @"' 
                        ";
            if (!String.IsNullOrEmpty(projectCategory))
            {
                sql += " and p.ProjectCategory = '" + projectCategory + @"' ";
            }
            sql += "  order by p.ProjectCode,p.ProjectName,p.created_on desc  ";
            return db.selectDataFocusone(sql);
        }

        public DataTable SearchMappingForWorkGroupAndProjectCode(string sid, string workgroupCode, string structureCode, string ProjectCode)
        {
            string sql = @"

                SELECT d.[SID]
                      ,d.[WorkGroupCode]
                      ,d.[StructureCode]
                      ,d.[ProjectCode]
                      ,d.[created_on]
                      ,d.[created_by]
                      ,d.[updated_on]
                      ,d.[updated_by]
                      ,ISNULL(ms.ProjectName,'') as ProjectName
                  FROM [dbo].[LINK_PROJECT_COMPANY_STRUCTURE_MAPPING_PROJECT_DETAIL] d
                    left join LINK_PROJECT_MASTER ms
                    on ms.SID = d.[SID] and d.[ProjectCode] = ms.ProjectCode
                  where d.SID='" + sid + @"' 
            ";

            if (!String.IsNullOrEmpty(workgroupCode))
            {
                sql += " and d.[WorkGroupCode]='" + workgroupCode + "'  ";

            }

            if (!String.IsNullOrEmpty(structureCode))
            {
                sql += " and d.[StructureCode]='" + structureCode + "' ";
            }

            if (!String.IsNullOrEmpty(ProjectCode))
            {
                sql += " and d.[ProjectCode]='" + ProjectCode + "'  ";
            }
            return db.selectDataFocusone(sql);
        }

        public DataTable searchWorkGroupCustomizeTemplate(string sid, string company, string projectCode)
        {
            string sql = @"
                            SELECT [CompanyCode]
                                  ,[SID]
                                  ,[ProjectCode]
                                  ,[Latitude]
                                  ,[Longitude]
                                  ,[ProjectType]
                                  ,[BudgetType]
                                  ,[ProjectValue]
                                  ,[BuildingNumber]
                                  ,[MaxNumberClass]
                                  ,[AreaBuildingMeter]
                                  ,[Insurance]
                                  ,[InsuranceStartDate]
                                  ,[Remark]
                                  ,[created_on]
                                  ,[created_by]
                                  ,[updated_on]
                                  ,[updated_by]
                              FROM [LINK_PROJECT_MASTER_CUSTOMIZE_TEMPLATE]
                            where CompanyCode = '" + company + "' and SID ='" + sid + @"'
                ";

            if (!String.IsNullOrEmpty(projectCode))
            {
                sql += " and projectCode = '" + projectCode + @"' ";
            }
            return db.selectDataFocusone(sql);
        }

        public void UpdateMappingForWorkGroupAndProjectCode(string sid, string workgroupCode, string structureCode, string ProjectCode, string employeeCode)
        {
            string sql = @"Delete LINK_PROJECT_COMPANY_STRUCTURE_MAPPING_PROJECT_DETAIL where SID='" + sid + @"' 
                            and [WorkGroupCode]='" + workgroupCode + @"' 
                            and [ProjectCode]='" + ProjectCode + @"' ;
                            ";
            sql += @"
                    INSERT INTO [dbo].[LINK_PROJECT_COMPANY_STRUCTURE_MAPPING_PROJECT_DETAIL]
                               ([SID]
                               ,[WorkGroupCode]
                               ,[StructureCode]
                               ,[ProjectCode]
                               ,[created_on]
                               ,[created_by]
                               ,[updated_on]
                               ,[updated_by])
                         VALUES
                               ('" + sid + @"'
                               ,'" + workgroupCode + @"'
                               ,'" + structureCode + @"'
                               ,'" + ProjectCode + @"'
                               ,'" + Validation.getCurrentServerStringDateTime() + @"'
                               ,'" + employeeCode + @"'
                               ,''
                               ,'');
                        ";

            db.executeSQLForFocusone(sql);
        }
    }
}