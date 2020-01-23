using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class UploadGalleryService
    {
        DBService dbService = new DBService();

        private static UploadGalleryService _instance;

        public static UploadGalleryService getInstance()
        {
            if (_instance == null)
            {
                _instance = new UploadGalleryService();
            }
            return _instance;
        }

        public void EditKMDesc(String sid, String keyObjectLink, String newDesc)
        {
            String sqlUpdate = "update  [km_attach_list] " +
                " set description = '" + newDesc + "' " +
                " where 1=1 and sid = '" + sid + "'  and key_object_link = '" + keyObjectLink + "' ";

            dbService.executeSQLForFocusone(sqlUpdate);
        }

        public DataTable GetKMFile(String sid, String objectLink, String fileCategory)
        {
            String sql = "SELECT TOP 1000 [sid] ,[business_type] ,[key_object_link] ,[item_no] " +
              " ,[file_name] ,[file_extension] ,[file_size] ,[doc_type]  ,[doc_number]  " +
              " ,[doc_year] ,[file_path] ,[logical_name] ,[file_category], description  " +
              " FROM  [km_attach_list] " +
              " where 1=1 and sid = '" + sid + "' and doc_number = '" + objectLink + "' ";
            if (!String.IsNullOrEmpty(fileCategory))
            {
                sql += " and file_category = '" + fileCategory + "' ";
            }
            sql += " order by [item_no] ";
            DataTable result = dbService.selectDataFocusone(sql);


            return result;
        }

        public string GetPathKMFile(String sid, String objectLink, String keyObjectLink)
        {
            String sql = "SELECT [file_path] " +
              " FROM  [km_attach_list] " +
              " where 1=1 and sid = '" + sid + "' and doc_number = '" + objectLink + "' and key_object_link = '" + keyObjectLink + "' ";
            DataTable result = dbService.selectDataFocusone(sql);

            if (result.Rows.Count == 1)
            {
                return result.Rows[0]["file_path"].ToString();
            }
            return "";
        }

        public void DeleteKMFile(String sid, String objectLink, String keyObjectLink, String ServerPath)
        {
            String sqlDelete = "delete from [km_attach_list] " +
                " where 1=1 and sid = '" + sid + "' and doc_number = '" + objectLink + "' and key_object_link = '" + keyObjectLink + "' ";

            dbService.executeSQLForFocusone(sqlDelete);

            if (File.Exists(ServerPath))
            {
                File.Delete(ServerPath);
            }
        }

        public DataTable GetFeedSocialShareFile(string AobjectLink, string FileType)
        {
            return GetFeedSocialShareFile(AobjectLink, FileType, null);
        }

        public DataTable GetFeedSocialShareFile(string AobjectLink, string FileType, string[] SEQ)
        {
            string sql = "select * from SNA_ACTIVITY_SPECIAL_FILE where AOBJECTLINK = '" + AobjectLink + "'";
            if (!string.IsNullOrEmpty(FileType))
            {
                sql += "  and FileType = '" + FileType + "' ";
            }
            if (SEQ != null && SEQ.Length > 0)
            {
                sql += "  and SEQ in (" + string.Join(",", SEQ) + ") ";
            }
            return dbService.selectData(sql);
        }

        public DataTable GetFeedActivityRawData(string SNAID, string AobjectLink)
        {
            string sql = "select * from SNA_" + SNAID
                + "_TIMEATTENDANCE where AOBJECTLINK = '" + AobjectLink + "'";
            return dbService.selectData(sql);
        }

        public void DeleteFeedSocialShareFile(string AobjectLink, string rootDirectory, string[] FileSEQ)
        {
            List<string> ListSql = new List<string>();
            DataTable dtSpecialFile = GetFeedSocialShareFile(AobjectLink, "", FileSEQ);
            foreach (DataRow dr in dtSpecialFile.Rows)
            {
                ListSql.Add("delete from SNA_ACTIVITY_SPECIAL_FILE where AOBJECTLINK = '" + AobjectLink +
                    "' and SEQ = '" + Convert.ToString(dr["SEQ"]) + "'");

                string serverPath = rootDirectory + Convert.ToString(dr["PhysicalFilePath"]).Replace("/", "\\");
                if (File.Exists(serverPath))
                {
                    File.Delete(serverPath);
                }
            }
            dbService.executeSQL(ListSql);
        }

        public void UpdateBlogData(string SNAID, string AOBJECTLINK, string Subject, String projectCode, String subprojectCode)
        {
            string sql = "update SNA_" + SNAID + "_TIMEATTENDANCE set ";
            sql += " JOBDESCRIPTION = '" + Subject + "' ";
            sql += " , PROJECTCODE = '" + projectCode + "' ";
            sql += " , SUBPROJECT = '" + subprojectCode + "' ";
            sql += " where AOBJECTLINK = '" + AOBJECTLINK + "'";

            dbService.executeSQL(sql);
        }

        public void DeleteFeed(string SNAID, string SID, string AobjectLink, string rootDirectory)
        {
            DeleteFeedSocialShareFile(
                AobjectLink,
                rootDirectory,
                null
            );

            List<string> ListSql = new List<string>();
            ListSql.Add("delete from SNA_" + SNAID + "_TIMEATTENDANCE where AOBJECTLINK = '" + AobjectLink + "'");
            ListSql.Add("delete from SNA_" + SNAID + "_TIMEATTENDANCE_PARTICIPANT where AOBJECTLINK = '" + AobjectLink + "'");
            ListSql.Add("delete from SNA_ACTIVITY_REMARK where OBJECTLINK = '" + AobjectLink + "'");
            ListSql.Add("delete from SNA_ACTIVITY_REMARK_CONTROL where AOBJECTLINK = '" + AobjectLink + "'");
            ListSql.Add("delete from SNA_SYSTEM_MESSAGE where XAOBJECTLINK = '" + AobjectLink + "'");
            ListSql.Add("delete from SNA_SYSTEM_MESSAGE_FAVORITE where AOBJECTLINK = '" + AobjectLink + "'");
            ListSql.Add("delete from SNA_ACTIVITY_KIDS_MAPPING where AOBJECTLINK = '" + AobjectLink + "'");
            ListSql.Add("delete from SNA_ACTIVITY_PRIVILEDGE where AOBJECTLINK = '" + AobjectLink + "'");
            ListSql.Add("delete from SNA_ACTIVITY_PRIVILEDGE_PARTY where AOBJECTLINK = '" + AobjectLink + "'");
            dbService.executeSQL(ListSql);
        }

        public string getKMMaxItemNo(string sid, string businessType, string docType, string docNumber, string docYear)
        {
            string result = "0";

            string sql = "SELECT ISNULL(MAX(item_no), 0) AS maxItemNo FROM [km_attach_list] " +
                "WHERE sid='" + sid + "' " +
                "AND business_type='" + businessType + "' " +
                "AND doc_type='" + docType + "' " +
                "AND doc_number='" + docNumber + "' " +
                "AND doc_year='" + docYear + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["maxItemNo"].ToString();
            }

            return result;
        }
    }
}