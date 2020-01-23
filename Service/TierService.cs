using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class TierService
    {
        DBService dbservice = new DBService();
        private ServiceTicketLibrary libServiceTicket = new ServiceTicketLibrary();

        public const string Main = "MAIN";
        public const string Participant = "PARTICIPANT";

        private static TierService _instance;
        public static TierService getInStance()
        {
            if (_instance == null)
            {
                _instance = new TierService();
            }
            return _instance;
        }

        private TierService()
        { 
        }

        [Serializable]
        public class entityParticipant
        {
            public string TierCode { get; set; }
            public string Tier { get; set; }
            public string Role { get; set; }
            public string RoleDescript { get; set; }
            public bool DynamicOwner { get; set; }
            public string EmpType { get; set; }
            public string EmpCode { get; set; }
            public string FullName_EN { get; set; }
            public string FullName_TH { get; set; }
            public string UpdateOnEmployee { get; set; }
        }

        [Serializable]
        public class entityParticipantDynamic
        {
            public string RoleCode;
            public string OwnerGroupCode;
        }

        public class entityTierItemForSequence
        {
            public String SID;
            public String WorkGroupCode;
            public String TierCode;
            public String Tier;
            public String sequence;
            public String Created_By;
            public String Created_On;
        }

        #region Tier Group Master
        public DataTable searchTierGorupMaster(string sid, string WorkGroupCode, string TierGroupCode,string Description)
        {
            string sql = @"SELECT  Gp.[SID]
                          ,Gp.[WorkGroupCode]
                          ,Gp.[TierGroupCode]
                          ,Gp.[TierGroupDescription]
                          ,Gp.[Created_By]
                          ,Gp.[Created_On]
	                      ,Isnull(em.FirstName_TH,Gp.[Created_By]) as name
	                      ,em.LastName_TH
	                      ,case ISNULL(Gp.[Created_On],'')
	                      when '' then ''
	                      else CONVERT(VARCHAR(10), CAST(SUBSTRING(Gp.[Created_On],1,8) AS DATETIME), 103)
			                    +' '+CONVERT(VARCHAR(8), Stuff(Stuff(SUBSTRING(Gp.[Created_On],9,6), 3, 0, ':'), 6, 0, ':'), 8)
	                      end Create_ON_Des 
                          , Gp.OwnerService
                      FROM [Link_TierGroup_Master] Gp WITH (NOLOCK) 

                      left join master_employee em WITH (NOLOCK) 
                      on em.SID= gp.SID and gp.Created_By=em.EmployeeCode
                        where Gp.SID='" + sid + @"' 
                    ";
            if (!String.IsNullOrEmpty(WorkGroupCode))
            {
                sql += " and Gp.WorkGroupCode='" + WorkGroupCode + "' ";
            }
            if (!string.IsNullOrEmpty(TierGroupCode))
            {
                sql += "  and Gp.TierGroupCode LIKE '%" + TierGroupCode + "%' ";
            }
            if (!string.IsNullOrEmpty(Description))
            {
                sql += "  and Gp.TierGroupDescription LIKE '%" + Description + "%' ";
            }

            sql += " order by Gp.[Created_On] desc ";

            return dbservice.selectDataFocusone(sql);
        }

        public List<string> InsertTierMaster(string sid, string WorkGroupCode, string TierGroupCode, string Description, string Employee,
            string OwnerService = "")
        {
            List<string> listTierCode = new List<string>();
            List<string> listSQL = new List<string>();

            string CreatedOn = Validation.getCurrentServerStringDateTime();
            string sql = @" INSERT INTO [dbo].[Link_TierGroup_Master]
                           ([SID]
                           ,[WorkGroupCode]
                           ,[TierGroupCode]
                           ,[TierGroupDescription]
                           ,[Created_By]
                           ,[Created_On]
                           ,[OwnerService]
                            )
                     VALUES
                           ('" + sid + @"'
                           ,'" + WorkGroupCode + @"'
                           ,'" + TierGroupCode + @"'
                           ,'" + Description + @"'
                           ,'" + Employee + @"'
                           ,'" + CreatedOn + @"'
                           ,'" + OwnerService + @"'
                            )";
            listSQL.Add(sql);

            DataTable dt = libServiceTicket.GetPriorityMaster(sid);
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                string TierCode = getAutoGenerateTierMasterCode(sid, WorkGroupCode, 2, i);
                string sqlTierMaster = getTierQueryForSaveTierMaster(
                    sid,
                    WorkGroupCode,
                    TierCode,
                    dr["Description"].ToString(),
                    TierGroupCode,
                    Employee,
                    CreatedOn,
                    dr["PriorityCode"].ToString()
                );
                listSQL.Add(sqlTierMaster);
                listTierCode.Add(TierCode);
            }

            dbservice.executeSQLForFocusone(listSQL);
            return listTierCode;
        }

        public void UpdateTierMaster(string sid, string WorkGroupCode,string employeeCode, List<TierService.entityTierMaster> ListTier)
        {
            string sql = "";
            string CreateOn = Validation.getCurrentServerStringDateTime();

            foreach (TierService.entityTierMaster en in ListTier)
            {
                sql += @"UPDATE [dbo].[Link_TierGroup_Master]
                            SET [TierGroupDescription] = '" + en.TierGroupDescription + @"' 
                               ,[Created_By] = '" + employeeCode + @"'
                               ,[Created_On] = '" + CreateOn + @"'
                          WHERE [SID] = '" + sid + @"'
                            AND WorkGroupCode = '" + WorkGroupCode + @"'
                            AND TierGroupCode = '" + en.TierGroupCode + @"';";
            }

            if (!string.IsNullOrEmpty(sql))
            {
                dbservice.executeSQLForFocusone(sql);
            }
        }

        [Serializable]
        public class entityTierMaster
        {
            public string SID;
            public string WorkGroupCode;
            public string TierGroupCode;
            public string TierGroupDescription;
            public string Created_By;
            public string Created_On;
            public string name;
            public string LastName_TH;
            public string Create_ON_Des;
        }

        #endregion

        #region Tier Master
        
        public DataTable searchTierMaster(string sid, string WorkGroupCode, string TierCode, string TierName, string TierGroupCode)
        {
            string sql = @" 
                    SELECT tr.[SID]
                    ,tr.[WorkGroupCode]
                    ,tr.[TierCode]
                    ,tr.[TierName]
                    ,tr.[PriorityCode]
                    ,tr.[TierGroupCode]
                    ,gp.TierGroupDescription
                    ,tr.[Created_By]
                    ,tr.[Created_On]
                    ,Isnull(em.FirstName_TH,tr.[Created_By]) as name
                    ,em.LastName_TH
                    ,case ISNULL(tr.[Created_On],'')
                    when '' then ''
                    else CONVERT(VARCHAR(10), CAST(SUBSTRING(tr.[Created_On],1,8) AS DATETIME), 103)
	                    +' '+CONVERT(VARCHAR(8), Stuff(Stuff(SUBSTRING(tr.[Created_On],9,6), 3, 0, ':'), 6, 0, ':'), 8)
                    end Create_ON_Des 
                    FROM [Link_Tier_Master] tr WITH (NOLOCK) 
                    left join master_employee em WITH (NOLOCK) 
                    on em.SID= tr.SID and tr.Created_By=em.EmployeeCode

                left join [Link_TierGroup_Master] gp WITH (NOLOCK) 
                on tr.SID = gp.SID  and tr.TierGroupCode = gp.TierGroupCode

                      where tr.SID='" + sid + "'  ";
            if (!String.IsNullOrEmpty(WorkGroupCode))
            {
                sql += " and tr.WorkGroupCode='" + WorkGroupCode + "' ";
            }
            if (!String.IsNullOrEmpty(TierCode))
            {
                sql += "  and tr.TierCode = '" + TierCode + "' ";
            }
            if (!String.IsNullOrEmpty(TierName))
            {
                sql += "  and tr.TierName LIKE '%" + TierName + "%' ";
            }
            if (!String.IsNullOrEmpty(TierGroupCode))
            {
                sql += " and tr.TierGroupCode ='" + TierGroupCode + "' ";
            }
            sql += @" order by tr.WorkGroupCode asc
                        ,tr.[TierCode] asc
                        ,tr.[TierName] asc
                        ,tr.Created_On asc  ";
           return dbservice.selectDataFocusone(sql);
        }


        public void SaveTierMaster(string sid, string WorkGroupCode ,string tierGroupCode , string tierCode, string tierCodeDes, 
                                   string tierItemCodeDes ,string Role , List<TierService.entityParticipant> enParticipant ,string CreatedBy ,
                                   bool addTierItemOnly, int Resolution, int Requester, int HeadShift, int AVPSale, int SVPSale, bool DynamicOwner)
        {
            string CreatedOn = Validation.getCurrentServerStringDateTime();

            List<string> listSql = new List<string>();
            String sql = "";
            if (!addTierItemOnly)
            {
                 sql = getTierQueryForSaveTierMaster(sid, WorkGroupCode, tierCode, tierCodeDes, tierGroupCode, CreatedBy, CreatedOn);
                 listSql.Add(sql);
            }
            else
            {
                int sequence = getMaxSequenceMasterItem(sid, WorkGroupCode, tierCode);
                string strSequence = (sequence + 1).ToString();
                string TierItemCode = "TC" + strSequence;

                sql = getTierQueryForSaveTierItem(
                    sid, WorkGroupCode, tierCode, TierItemCode, tierItemCodeDes,
                    Role, "", strSequence, CreatedBy, CreatedOn,
                    Resolution, Requester, HeadShift, AVPSale, SVPSale, DynamicOwner
                );
                listSql.Add(sql);

                //foreach (entityParticipant en in enParticipant)
                //{
                //    if (en.type.Equals(Main, StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        sql = getTierQueryForSaveTierItem(sid, WorkGroupCode, tierCode, TierItemCode, tierItemCodeDes, Role,
                //                                          en.emplayeeCode, strSequence, CreatedBy, CreatedOn,
                //                                          Resolution, Requester, HeadShift, AVPSale, SVPSale);
                //        listSql.Add(sql);
                //    }
                //    else if (en.type.Equals(Participant, StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        sql = getTierQueryForSaveParticipant(sid, WorkGroupCode, tierCode, TierItemCode, Role, en.emplayeeCode, CreatedBy, CreatedOn);

                //        listSql.Add(sql);
                //    }
                //}
            }

            if (listSql.Count > 0)
            {
                dbservice.executeSQLForFocusone(listSql);
            }
        }
        public void saveTierOwnerMappingRole(string SID, string TierCode, string Tier, List<entityParticipantDynamic> en, string CREATED_BY)
        {
            string CREATED_ON = Validation.getCurrentServerStringDateTime();
            List<string> listsql = new List<string>();
            List<entityParticipantDynamic> en_Old = getTierOwnerMappingRole(SID, TierCode, Tier);

            en.ForEach(r =>
            {
                String sql = "";
                if (en_Old.Where(w => w.OwnerGroupCode.Equals(r.OwnerGroupCode)).Count() == 0)
                {
                    sql = @"insert into ERPW_TIER_OWNER_MAPPING_ROLE (
                               SID
                              ,TierCode
                              ,Tier
                              ,OwnerGroupCode
                              ,[Role]
                              ,CREATED_BY
                              ,CREATED_ON
                            ) VALUES (
                               '" + SID + @"' 
                              ,'" + TierCode + @"'
                              ,'" + Tier + @"'
                              ,'" + r.OwnerGroupCode + @"'
                              ,'" + r.RoleCode + @"'
                              ,'" + CREATED_BY + @"'
                              ,'" + CREATED_ON + @"'
                            )";
                }
                else
                {
                    sql = @"update ERPW_TIER_OWNER_MAPPING_ROLE SET
                              [Role] = '" + r.RoleCode + @"'
                              ,UPDATED_BY = '" + CREATED_BY + @"'
                              ,UPDATED_ON = '" + CREATED_ON + @"'
                            WHERE SID = '" + SID + @"'
                              AND TierCode = '" + TierCode + @"'
                              AND Tier = '" + Tier + @"'
                              AND OwnerGroupCode = '" + r.OwnerGroupCode + @"'";
                }

                listsql.Add(sql);
            });

            var en_delete = en_Old.Where(w => !en.Select(s => s.OwnerGroupCode).Contains(w.OwnerGroupCode)).ToList();
            en_delete.ForEach(r =>
            {
                string sql = @"delete from ERPW_TIER_OWNER_MAPPING_ROLE
                                WHERE SID = '" + SID + @"'
                                  AND TierCode = '" + TierCode + @"'
                                  AND Tier = '" + Tier + @"'
                                  AND OwnerGroupCode = '" + r.OwnerGroupCode + @"'";
                listsql.Add(sql);
            });

            dbservice.executeSQLForFocusone(listsql);
        }

        public List<entityParticipantDynamic> getTierOwnerMappingRole(string SID, string TierCode, string Tier)
        {
            string sql = @"select OwnerGroupCode, [Role] as RoleCode
                        from ERPW_TIER_OWNER_MAPPING_ROLE WITH (NOLOCK) 
                        where SID = '" + SID + @"' 
                          AND TierCode = '" + TierCode + @"'
                          AND Tier = '" + Tier + @"'";

            DataTable dt = dbservice.selectDataFocusone(sql);
            string strJson = JsonConvert.SerializeObject(dt);
            List<entityParticipantDynamic> en = JsonConvert.DeserializeObject<List<entityParticipantDynamic>>(strJson);
            return en;
        }

        public int getMaxSequenceMasterItem(string sid, string WorkGroupCode, string TierCode)
        {
            string sql = @"select Isnull(max(convert(int,[sequence])), 0)as sequence 
                             from [Link_Tier_Master_Item] WITH (NOLOCK) 
                            where SID='" + sid + @"' 
                              and [WorkGroupCode] ='" + WorkGroupCode + @"'
                              and [TierCode] ='" + TierCode + @"'
                              and [sequence] <>''";

            DataTable dtSequence = dbservice.selectDataFocusone(sql);

            int sequence = 0;
            int.TryParse(dtSequence.Rows[0]["sequence"].ToString(), out sequence);

            return sequence;
        }

        public String getAutoGenerateTierMasterCode(string sid, string WorkGroupCode, int repalceFirstString, int NextID = 1)
        {
            string sql = @"select MAX(ISNULL(tierCode, 0)) as ItemNo 
                             from Link_Tier_Master  WITH (NOLOCK) 
                            where SID='" + sid + @"' 
                              and [WorkGroupCode] ='" + WorkGroupCode + @"'";
             
            DataTable dtCode = dbservice.selectDataFocusone(sql);

            int nTierCode = 0;
            if (dtCode != null && dtCode.Rows.Count > 0)
            {
                string TierCode = "";
                if (dtCode.Rows[0]["ItemNo"].ToString().Length >= 2)
                {
                    TierCode = dtCode.Rows[0]["ItemNo"].ToString().Remove(0, repalceFirstString);
                    int.TryParse(TierCode, out nTierCode);
                }
                else
                {
                    int.TryParse(dtCode.Rows[0]["ItemNo"].ToString(), out nTierCode);
                }
            }

            return "TC" + (nTierCode + NextID).ToString().PadLeft(7, '0');
        }

        public void UpdateDescriptionTierMaster(string sid, string WorkGroupCode, string TierCode, string TierName, string Created_By)
        {
            string sql = @" UPDATE [Link_Tier_Master]
                            SET [TierName] = '" + TierName + @"'
                            ,[Created_By] = '" + Created_By + @"'
                            ,[Created_On] = '" + Validation.getCurrentServerStringDateTime() + @"'
                            WHERE [SID] = '" + sid + @"'
                            AND [WorkGroupCode] = '" + WorkGroupCode + @"'
                            AND  [TierCode] = '" + TierCode + @"' ";

           dbservice.executeSQLForFocusone(sql);
        }

        public void DeleteTierMaster(string sid, string WorkGroupCode,string TierGroup, string TierCode)
        {
            List<string> deleteTierMaster = new List<string>();
            string sql = @"
                  DELETE FROM [Link_Tier_Master]
                  WHERE SID ='"+sid+@"'
	              and WorkGroupCode ='"+WorkGroupCode+@"'
	              and TierGroupCode='"+TierGroup+@"'
	              and TierCode='"+TierCode+@"'";

            deleteTierMaster.Add(sql);

            sql = @"
                 DELETE FROM [Link_Tier_Master_Item]
                 WHERE SID='" + sid + @"'
                 and WorkGroupCode ='" + WorkGroupCode + @"'
                 and TierCode = '" + TierCode + @"' 

                 DELETE FROM [Link_Tier_Master_Item_Participant]
                 WHERE SID='" + sid + @"'
                 and WorkGroupCode ='" + WorkGroupCode + @"'
                 and TierCode = '" + TierCode + @"'";
            
            deleteTierMaster.Add(sql);
            dbservice.executeSQLForFocusone(deleteTierMaster);
        }

        public bool IsCheckTierMasterUserWorkingForDelete(string sid,string WorkGroupCode,string TierCode)
        {
            bool check = false;
            string sql = @" SELECT count(*) as UseWork
                              FROM [CRM_Equipment_Mapping_Customer]
                             WHERE SID='" + sid + @"' 
                               AND WorkgroupCode='" + WorkGroupCode + @"'
                               AND TierCode='" + TierCode + @"'";

            DataTable dt = dbservice.selectDataFocusone(sql);

            string strNumber = dt.Rows[0]["UseWork"].ToString();
            int usenumber = 0;
            int.TryParse(strNumber, out usenumber);
            if (usenumber <= 0)
            {
                check = true;
            }

            return check;
        }
        #endregion

        #region Tier Item
        public DataTable searchTierMasterItem(string sid, string WorkGroupCode, string TierCode, string Tier, string TierDescription, string Role)
        {
            string sql = @"
                       SELECT item.[SID]
                        ,item.[WorkGroupCode]
		                ,ms.TierGroupCode
		                ,gp.TierGroupDescription
                        ,item.[TierCode]
		                ,ms.TierName
                        ,item.[Tier]
                        ,item.[TierDescription]
                        ,item.[Role]
                        ,item.[DefaultMain]
                        ,ISNULL(item.Resolution,0) as Resolution
                        ,ISNULL(item.Requester,0) as Requester
                        ,ISNULL(item.HeadShift,0) as HeadShift
                        ,ISNULL(item.AVPSale,0) as AVPSale
                        ,ISNULL(item.SVPSale,0) as SVPSale
                        ,ISNULL(item.StatusReporter,'') as StatusReporter
                        ,Isnull(emd.FirstName_TH,item.[Created_By]) as NameDefaultMain
                        ,ISNULL(item.DynamicOwner,'FALSE') as DynamicOwner
	                    ,emd.LastName_TH as LastNameDefaultMain
                       ,emd.UPDATED_ON as LastUpdateOnEmployee
                        ,ISNULL(REPLACE( STUFF((
	                        SELECT ' |'+ (p.DefaultParticipant+','+emp.UPDATED_ON+','+emp.FirstName_TH +' '+LastName_TH)
	                        from Link_Tier_Master_Item_Participant p
							left join master_employee emP
							on (item.SID = p.SID) and p.SID = emP.SID and p.DefaultParticipant=emp.EmployeeCode  

	                        WHERE (item.SID = p.SID) 
	                        and (item.WorkGroupCode = p.WorkGroupCode) 
	                        and (item.TierCode = p.TierCode) 
	                        and (item.Tier = p.Tier)
	                        and (item.Role = p.Role)
	                        FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
	                        ,1,2,'') , ' ',''),'')
                          AS DefaultParticipant

                        ,item.[sequence]
                        ,item.[Created_By]
                        ,item.[Created_On]
	                    ,Isnull(em.FirstName_TH,item.[Created_By]) as name
                ,em.LastName_TH
                ,case ISNULL(item.[Created_On],'')
                when '' then ''
                else CONVERT(VARCHAR(10), CAST(SUBSTRING(item.[Created_On],1,8) AS DATETIME), 103)
	                +' '+CONVERT(VARCHAR(8), Stuff(Stuff(SUBSTRING(item.[Created_On],9,6), 3, 0, ':'), 6, 0, ':'), 8)
                end Create_ON_Des 

                FROM [dbo].[Link_Tier_Master_Item] item

                left join master_employee em
                on em.SID= item.SID and item.Created_By=em.EmployeeCode

                left join master_employee emd
                on emd.SID= item.SID and item.DefaultMain=emd.EmployeeCode

                left join [Link_Tier_Master] ms
                on item.SID = ms.SID and item.TierCode = ms.TierCode

                left join [Link_TierGroup_Master] gp
                on item.SID = gp.SID and item.TierCode = ms.TierCode and ms.TierGroupCode = gp.TierGroupCode

                    where item.SID='" + sid + @"' 

                ";

            if (!String.IsNullOrEmpty(WorkGroupCode))
            {
                sql += " and item.WorkGroupCode='" + WorkGroupCode + "' ";
            }
            if (!String.IsNullOrEmpty(TierCode))
            {
                sql += " and item.TierCode ='" + TierCode + "'  ";
            }
            if (!String.IsNullOrEmpty(Tier))
            {
                sql += "  and item.Tier='" + Tier + "' ";
            }
            if (!String.IsNullOrEmpty(TierDescription))
            {
                sql += " and item.TierDescription LIKE '%" + TierDescription + "%' ";
            }
            if (!String.IsNullOrEmpty(Role))
            {
                sql += "  and item.Role = '" + Role + "' ";
            }

            sql += @" order by item.WorkGroupCode,TierCode
	                ,case ISNULL(item.[sequence],'999')
                        when '999' then 999
                        else item.[sequence]
                        end  ASC ";
            return dbservice.selectDataFocusone(sql);
        }


        public void UpdateTierItemForSelectNewRole(string sid, string WorkGroupCode ,string tierGroupCode ,string tierCode, string tierItemCode,string tierItemCodeDes,
                                                   string Role,string Sequence, List<TierService.entityParticipant> enParticipant ,string CreatedBy,
                                                   int Resolution, int Requester, int HeadShift, int AVPSale, int SVPSale, bool DynamicOwner)
        {
            string CreatedOn = Validation.getCurrentServerStringDateTime();
            List<String> listUpdate = new List<string>();
            String sql = "";
            sql = getQueryeDeleteRoleOldForAddNewRole(sid,WorkGroupCode,tierCode,tierItemCode);
            listUpdate.Add(sql);
            
            sql = getTierQueryForSaveTierItem(
                sid, WorkGroupCode, tierCode, tierItemCode, tierItemCodeDes, 
                Role, "", Sequence, CreatedBy, CreatedOn,
                Resolution, Requester, HeadShift, AVPSale, SVPSale, DynamicOwner
            );
            listUpdate.Add(sql);

            //foreach (entityParticipant en in enParticipant)
            //{
            //    if (en.type.Equals(Main, StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        sql = getTierQueryForSaveTierItem(
            //            sid, WorkGroupCode, tierCode, tierItemCode, tierItemCodeDes, 
            //            Role, en.emplayeeCode, Sequence, CreatedBy, CreatedOn,
            //            Resolution, Requester, HeadShift, AVPSale, SVPSale, DynamicOwner
            //        );
            //        listUpdate.Add(sql);
            //    }

            //    else if (en.type.Equals(Participant, StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        sql = getTierQueryForSaveParticipant(sid, WorkGroupCode, tierCode, tierItemCode, Role, en.emplayeeCode, CreatedBy, CreatedOn);

            //        listUpdate.Add(sql);
            //    }
            //}

            if (listUpdate.Count > 0)
            {
                dbservice.executeSQLForFocusone(listUpdate);
            }
        }

        private String deleteTierItem;
        public String getQueryeDeleteRoleOldForAddNewRole(string sid, string WorkGroupCode, string TierCode, string Tier)
        {
            String sql = @"

                 DELETE FROM [Link_Tier_Master_Item]
                 WHERE SID='"+sid+ @"'
                 and WorkGroupCode ='"+WorkGroupCode+ @"'
                 and TierCode = '"+TierCode+ @"' 
                 and Tier='" + Tier + @"' ;

                 DELETE FROM [Link_Tier_Master_Item_Participant]
                 WHERE SID='" + sid + @"'
                 and WorkGroupCode ='" + WorkGroupCode + @"'
                 and TierCode = '" + TierCode + @"' 
                 and Tier='" + Tier + @"'  ;

            ";
            deleteTierItem = sql;
            return sql;
        }

        public void DeleteTierItemInTierMaster()
        {
            if (String.IsNullOrEmpty(deleteTierItem))
            {
                throw new Exception(" item for delete is null ");
            }
            dbservice.executeSQLForFocusone(deleteTierItem);
        }

        public void UpdateSequenceForTierItem(String sid,String WorkGroupCode, String Created_By,List<TierService.entityTierItemForSequence> enItemSequence)
        {
            string sql = "";
            string CreatedOn = Validation.getCurrentServerStringDateTime(); 
            foreach (TierService.entityTierItemForSequence en in enItemSequence)
            {
                  sql += @"
                    UPDATE [dbo].[Link_Tier_Master_Item]
                       SET [sequence] = '"+en.sequence+@"'
                          ,[Created_By] = '"+Created_By+@"'
                          ,[Created_On] = '"+CreatedOn+@"'
                     WHERE [SID] = '" + sid + @"'
                    and [WorkGroupCode] = '" +WorkGroupCode+@"'
                    and [TierCode] = '"+en.TierCode+@"'
                    and [Tier] = '"+en.Tier+@"'  ";
            }
            if (String.IsNullOrEmpty(sql))
            {
                throw new Exception(" List entity is not valid or null ");
                return;
            }
            dbservice.executeSQLForFocusone(sql);
        }

        public List<entityParticipant> GetAllTierItemEmployeeDetail(string sid, string CompanyCode)
        {
            string sql = @"
                            Select emp.* 
	                            , c.FirstName + ' ' + c.LastName as FullName_EN
	                            , c.FirstName_TH + ' ' + c.LastName_TH as FullName_TH
	                            , c.UPDATED_ON as UpdateOnEmployee
                            from
                            (
	                            SELECT TierCode, Tier ,[Role] ,[DefaultMain] as [EmpCode]
		                            ,isnull([DynamicOwner], 'FALSE') as [DynamicOwner]
		                            ,'Main' as EmpType
		                            , '' as RoleDescript
	                            FROM [Link_Tier_Master_Item] WITH (NOLOCK) 

	                            where sid = '" + sid + @"'
		                            and DefaultMain != ''

	                            Union All

	                            select TierCode, Tier, [Role], DefaultParticipant as [EmpCode]
		                            ,'FALSE' as [DynamicOwner]
		                            ,'Participant' as EmpType
		                            , '' as RoleDescript

	                            from [dbo].[Link_Tier_Master_Item_Participant] WITH (NOLOCK) 
	                            where sid = '" + sid + @"'

	                            union

	                            SELECT item.TierCode, item.Tier ,item.[Role]
		                            , b.EmployeeCode as [EmpCode]
		                            ,isnull([DynamicOwner], 'FALSE') as [DynamicOwner]
		                            , case when b.MainDelegate = 'TRUE' then 'Main' else 'Participant' end EmpType
		                            , a.HierarchyDesc as RoleDescript

	                            FROM [Link_Tier_Master_Item] as item WITH (NOLOCK) 
	                            inner join dbo.ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY a WITH (NOLOCK) 
		                            on item.SID = a.Sid
		                            and item.Role = a.HierarchyCode
	                            inner join ERPW_ACCOUNTABILITY_LINK_PROJECT_CHARACTER_MAPPING_PERSON b WITH (NOLOCK) 
		                            on a.HierarchyCode = b.HierarchyCode
		                            and a.Sid = b.SID

	                            where item.sid = '" + sid + @"'
		                            and item.[Role] != ''
		                            and (item.DynamicOwner is null or item.DynamicOwner = 'FALSE')

	                            union all 

	                            SELECT item.TierCode, item.Tier ,item.[Role]
		                            , b.EmployeeCode as [EmpCode]
		                            ,isnull([DynamicOwner], 'FALSE') as [DynamicOwner]
		                            , case when b.MainDelegate = 'TRUE' then 'Main' else 'Participant' end EmpType
		                            , a.HierarchyDesc as RoleDescript

	                            FROM [Link_Tier_Master_Item] as item WITH (NOLOCK) 
	                            inner join  ERPW_TIER_OWNER_MAPPING_ROLE _owner WITH (NOLOCK) 
		                            on _owner.[SID] = item.[SID]
		                            and _owner.TierCode = item.TierCode
		                            and _owner.Tier = item.Tier

	                            inner join dbo.ERPW_ACCOUNTABILITY_MASTER_CONF_HIERARCHY a WITH (NOLOCK) 
		                            on item.SID = a.Sid
		                            and _owner.[Role] = a.HierarchyCode
	                            inner join ERPW_ACCOUNTABILITY_LINK_PROJECT_CHARACTER_MAPPING_PERSON b WITH (NOLOCK) 
		                            on a.HierarchyCode = b.HierarchyCode
		                            and a.Sid = b.SID

	                            where item.sid = '" + sid + @"'
		                            and item.[Role] = ''
		                            and item.DynamicOwner = 'TRUE'
                            ) as Emp

                            inner join master_employee c
                                on c.SID = '" + sid + @"'
                                and c.CompanyCode = '" + CompanyCode + @"'
                                and Emp.EmpCode = c.EmployeeCode

                            ";

            DataTable dtResult = dbservice.selectDataFocusone(sql);
            string strJson = JsonConvert.SerializeObject(dtResult);
            List<entityParticipant> enParti = JsonConvert.DeserializeObject<List<entityParticipant>>(strJson);
            return enParti;
        }
        #endregion

        #region Darft Query Tier Master
        public String getTierQueryForSaveTierMaster(string sid,string WorkGroupCode,
            string TierCode, string TierName, string TierGroupCode, 
            string Created_By, string Created_On, string PriorityCode = "")
        { 
            string sql = @"
                        INSERT INTO [dbo].[Link_Tier_Master]
                        (
                            [SID]
                            ,[WorkGroupCode]
                            ,[TierCode]
                            ,[TierName]
                            ,[TierGroupCode]
                            ,[Created_By]
                            ,[Created_On]
                            ,[PriorityCode]
                        )
                        VALUES
                        (
                            '" + sid+@"'
                            ,'"+WorkGroupCode+@"'
                            ,'"+TierCode+@"'
                            ,'"+TierName+@"'
                            ,'"+TierGroupCode+@"'
                            ,'"+Created_By+@"'
                            ,'" + Created_On + @"'
                            ,'" + PriorityCode + @"'
                        ); ";
            return sql;
        }

        public String getTierQueryForSaveTierItem(string sid, string WorkGroupCode, string TierCode, string TierItem, string TierItemDescription,
                                                  string Role, string DefaultMain, string sequence, string emplayeeCode, string CreatedOn,
                                                  int Resolution, int Requester, int HeadShift, int AVPSale, int SVPSale, bool DynamicOwner)
        {
            string sql = @"
                        INSERT INTO [dbo].[Link_Tier_Master_Item]
                            ([SID]
                            ,[WorkGroupCode]
                            ,[TierCode]
                            ,[Tier]
                            ,[TierDescription]
                            ,[Role]
                            ,[DefaultMain]
                            ,[sequence]
                            ,[Created_By]
                            ,[Created_On]
                            ,[Resolution]
                            ,[Requester]
                            ,[HeadShift]
                            ,[AVPSale]
                            ,[SVPSale]
                            ,DynamicOwner
                        )
                         VALUES
                        (
                            '" + sid + @"'
                            ,'" + WorkGroupCode + @"'
                            ,'" + TierCode + @"'
                            ,'" + TierItem + @"'
                            ,'" + TierItemDescription + @"'
                            ,'" + Role + @"'
                            ,'" + DefaultMain + @"'
                            ,'" + sequence + @"'
                            ,'" + emplayeeCode + @"'
                            ,'" + CreatedOn + @"'
                            ," + Resolution + @"
                            ," + Requester + @"
                            ," + HeadShift + @"
                            ," + AVPSale + @"
                            ," + SVPSale + @"
                            ,'" + DynamicOwner.ToString().ToUpper() + @"'
                        ); ";
                return sql;
        }

        public String getTierQueryForSaveParticipant(string sid, string WorkGroupCode, string TierCode, string Tier, string Role,
                                                     string DefaultParticipant, string emplayeeCode, string CreatedOn)
        {
            string sql = @"
                        INSERT INTO [dbo].[Link_Tier_Master_Item_Participant]
                                   ([SID]
                                   ,[WorkGroupCode]
                                   ,[TierCode]
                                   ,[Tier]
                                   ,[Role]
                                   ,[DefaultParticipant]
                                   ,[Created_By]
                                   ,[Created_On])
                             VALUES
                                   ('"+sid+@"'
                                   ,'"+WorkGroupCode+@"'
                                   ,'"+TierCode+@"'
                                   ,'"+Tier+@"'
                                   ,'"+Role+@"'
                                   ,'"+DefaultParticipant+@"'
                                   ,'"+emplayeeCode+@"'
                                   ,'" + CreatedOn + "'); ";
            return sql;
        }
        #endregion darft Query Tier Master

        #region search Role
        public DataTable searchRoleCriteriaData(string sid, string WorkGroupCode)
        {
            string sql = @"
                    select distinct Person.WorkGroupCode, Hierarchy.HierarchyCode, 
                    HGroup.HIERARCHYGROUPNAME + ' --> ' + HType.HIERARCHYTYPENAME + ' --> ' + Hierarchy.HierarchyDesc as HierarchyDesc
                   
                    from LINK_PROJECT_CHARACTER Project
                    left join SYS_HIERARCHYGROUP_MASTER HGroup
                        on Project.HierarchyGroup = HGroup.HIERARCHYGROUPCODE
                    left join SYS_HIERARCHYTYPE_MASTER HType 
                        on Project.HierarchyType = HType.HIERARCHYTYPECODE 
                        and Project.HierarchyGroup = HType.HIERARCHYGROUPCODE 
                    inner join LINK_PROJECT_CHARACTER_MAPPING_PERSON Person
                        on Project.CharacterCode = Person.CharacterCode
                        and Project.SID = Person.SID
                        and Project.WorkGroupCode = Person.WorkGroupCode
                    left join ep_master_conf_hierarchy Hierarchy
                        on Hierarchy.HierarchyCode = Person.HierarchyCode
                        and Project.SID = Hierarchy.Sid

                    where Project.SID='"+sid+@"' 
                        and Project.WorkGroupCode = '"+WorkGroupCode+@"'
                    order by HierarchyDesc
             ";
           return dbservice.selectDataFocusone(sql);
        }

        public DataTable searchPeopleForRoleSelect(string sid, string HierarchyCode)
        {
            string sql = @"
                        SELECT  p.[SID]
                              , p.[WorkGroupCode]
                              , p.[EmployeeCode]
                              ,Isnull(em.FirstName_TH,p.[EmployeeCode]) as name
	                          ,em.LastName_TH
                              , p.[HierarchyCode]
                              , p.[CharacterCode]
                              ,'' as DefaultMain
                              ,'' as DefaultParticipant
                          FROM [LINK_PROJECT_CHARACTER_MAPPING_PERSON] p
 
                          left join master_employee em
                        on em.SID= p.SID and p.EmployeeCode=em.EmployeeCode

                         where p.SID='" + sid + "' and p.HierarchyCode='" + HierarchyCode + "' ";
           return dbservice.selectDataFocusone(sql);
        }

        public DataTable SearchTierItemRoleSelected(string sid, string WorkGroupCode, string TierCode, string Tier, string Role)
        {
            string sql = @"
                        SELECT 
                        item.[SID]
                        ,item.[WorkGroupCode]
                        ,item.[DefaultMain] as EmployeeCode
                        ,Isnull(em.FirstName_TH,item.DefaultMain) as name
                        ,em.LastName_TH
                        ,[Role] as HierarchyCode
                        , '' as CharacterCode
                        ,'active' as DefaultMain
                        ,'' as DefaultParticipant
                        FROM [Link_Tier_Master_Item] item WITH (NOLOCK) 

                        left join master_employee em WITH (NOLOCK) 
                        on em.SID= item.SID and item.DefaultMain=em.EmployeeCode

                        where item.[SID]='" + sid+@"' 
                        and item.[WorkGroupCode]='"+WorkGroupCode+@"'
                        and item.[TierCode]='"+TierCode+@"'
                        and item.[Tier]='"+Tier+@"'
                        and item.[Role] = '"+Role+ @"'

                        UNION

                        SELECT pc.[SID]
                        ,pc.[WorkGroupCode]
                        ,pc.[DefaultParticipant] as EmployeeCode
                        ,Isnull(em.FirstName_TH,pc.DefaultParticipant) as name
                        ,em.LastName_TH
                        ,pc.[Role] as HierarchyCode
                        , '' as CharacterCode
                        ,'' as DefaultMain
                        ,'active' as DefaultParticipant
                        FROM [Link_Tier_Master_Item_Participant] pc WITH (NOLOCK) 

                        left join master_employee em WITH (NOLOCK) 
                        on em.SID= pc.SID and pc.DefaultParticipant=em.EmployeeCode

                        where pc.[SID]='" + sid + @"' 
                        and pc.[WorkGroupCode]='" + WorkGroupCode + @"'
                        and pc.[TierCode]='" + TierCode + @"'
                        and pc.[Tier]='" + Tier + @"'
                        and pc.[Role] = '" + Role + @"'  
                            
                        order by DefaultMain desc ";
            return dbservice.selectDataFocusone(sql);
        }
        #endregion

        public void CopyRemark(string oldObjectLink, string newObjectLink)
        {
            string sql = "SELECT * FROM [dbo].[SNA_ACTIVITY_REMARK]  WITH (NOLOCK) WHERE [OBJECTLINK] = '" + oldObjectLink + "'";

            DataTable dt = dbservice.selectDataFocusone(sql);
            dt.Columns.Remove("REMARK_SEQ");
            dt.AcceptChanges();

            DataTable dtSave = dt.Clone();
            dtSave.TableName = "SNA_ACTIVITY_REMARK";                       

            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtSave.NewRow();

                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName == "OBJECTLINK")
                    {
                        drNew["OBJECTLINK"] = newObjectLink;
                    }
                    else
                    {
                        drNew[dc.ColumnName] = dr[dc.ColumnName].ToString();
                    }
                }                               

                dtSave.Rows.Add(drNew);
            }

            dbservice.SaveTransactionForFocusone(dtSave);

            #region update ref code reply comment
            string sql_reply = @"select a.MESSAGE_TYPE, a.REMARK_MESSAGE, a.CREATE_ON
	                            , a.REMARK_SEQ as REMARK_SEQ
	                            , c.MESSAGE_TYPE, c.REMARK_MESSAGE, a.CREATE_ON
	                            , c.REMARK_SEQ as New_REMARK_SEQ
                            from SNA_ACTIVITY_REMARK a WITH (NOLOCK) 
                            inner join SNA_ACTIVITY_REMARK b WITH (NOLOCK) 
	                            on a.RefCode = b.REMARK_SEQ
                            inner join SNA_ACTIVITY_REMARK c WITH (NOLOCK) 
	                            on a.OBJECTLINK = c.OBJECTLINK
	                            and b.SID = b.SID
	                            and b.COMPANYCODE = c.COMPANYCODE 
	                            and b.MESSAGE_TYPE = c.MESSAGE_TYPE
	                            and b.REMARK_MESSAGE = c.REMARK_MESSAGE

                            where a.OBJECTLINK = '" + newObjectLink + @"'
	                            and a.RefCode <> ''
	                            and a.RefCode Is not null ";
            DataTable dtReply = dbservice.selectDataFocusone(sql_reply);

            List<string> sqlUpdate = new List<string>();
            foreach (DataRow dr in dtReply.Rows)
            {
                sqlUpdate.Add(@"update SNA_ACTIVITY_REMARK set RefCode = '" + dr["New_REMARK_SEQ"] + "' where REMARK_SEQ = '" + dr["REMARK_SEQ"] + "'");
            }
            dbservice.executeSQLForFocusone(sqlUpdate);
            #endregion
        }

        public DataTable getOwnerServiceMaster(string SID, string CompanyCode)
        {
            string sql = @"select *
                            from ERPW_OWNER_GROUP  WITH (NOLOCK) 
                            where SID = '" + SID + @"'
                            AND CompanyCode = '" + CompanyCode + @"'
                            order by OwnerGroupCode asc";
            return dbservice.selectDataFocusone(sql);
        }
    }
}