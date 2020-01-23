using Agape.Lib.DBService;
using Agape.Lib.KMV2.Core.service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class LinkGalleryMaterial
    {

        private DBService dbService = new DBService();

        public DataTable getGalleryMaterial(string sid, string itemnumber, string filecategory)
        {
            string sql = "select *, KeyFileObject as key_object_link from master_mm_item_picture_for_link where sid = '" + sid + "' and ItmNumber = '" + itemnumber + "' and file_category = '" + filecategory + "' ";
            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public void addGalleryMaterial(string sid, string itemnumber, string keyfileobject, string itemno, string filename, string fileextension, string filesize, string description
            , string filepath, string logicalname, string filecategory, string fileurl)
        {
            string sql = "insert into master_mm_item_picture_for_link(SID, ItmNumber, KeyFileObject, ItemNo, file_name, file_extension, file_size, description, file_path, logical_name, file_category, file_url ) " +
                         "VALUES ('" + sid + "','" + itemnumber + "','" + keyfileobject + "','" + itemno + "','" + filename + "','" + fileextension + "',0,'" + description + "','" + filepath + "','" + logicalname + "','" + filecategory + "','" + fileurl + "' ) ";


            dbService.executeSQLForFocusone(sql);
        }

        public string getMatMaxItemNo(string sid, string itemnumber, string filecategory)
        {
            string result = "0";

            string sql = "SELECT ISNULL(MAX(ItemNo), 0) AS maxItemNo FROM [master_mm_item_picture_for_link] " +
                "WHERE sid='" + sid + "' " +
                "AND ItmNumber='" + itemnumber + "' " +
                "AND file_category='" + filecategory + "' ";
            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["maxItemNo"].ToString();
            }

            return result;
        }

        public void deleteGalleryMaterial(string sid, string itemnumber, string keyfileobject)
        {
            string sql = "SELECT * FROM master_mm_item_picture_for_link " +
                "WHERE sid='" + sid + "' " +
                "AND ItmNumber='" + itemnumber + "' " +
                "AND KeyFileObject='" + keyfileobject + "' ";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                dt.TableName = "master_mm_item_picture_for_link";
                dt.PrimaryKey = new DataColumn[] { 
                    dt.Columns["sid"], 
                    dt.Columns["ItmNumber"], 
                    dt.Columns["KeyFileObject"], 
                    dt.Columns["Itemno"]
                };

                if (!string.IsNullOrEmpty(dt.Rows[0]["file_path"].ToString()))
                {
                    KMFileManager.deleteFileWithSID(dt.Rows[0]["file_path"].ToString(), sid);
                }

                dt.Rows[0].Delete();

                dbService.SaveTransactionForFocusone(dt);
            }
        }

        public void setSequenceGalleryMaterial(string sid, string itemnumber, string[] keyfileobject)
        {
            string sql = "SELECT * FROM master_mm_item_picture_for_link " +
                "WHERE sid='" + sid + "' " +
                "AND ItmNumber='" + itemnumber + "' ";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                dt.TableName = "master_mm_item_picture_for_link";
                dt.PrimaryKey = new DataColumn[] { 
                    dt.Columns["sid"], 
                    dt.Columns["ItmNumber"], 
                    dt.Columns["KeyFileObject"]
                };

                for (int i = 0; i < keyfileobject.Length; i++)
                {
                    DataRow[] drr = dt.Select("KeyFileObject='" + keyfileobject[i] + "'");

                    if (drr.Length > 0)
                    {
                        drr[0]["ItemNo"] = (i + 1).ToString().PadLeft(3, '0');
                    }
                }

                dbService.SaveTransactionForFocusone(dt);
            }
        }

        public void deleteGalleryMaterialV2(string sid, string itemnumber, List<string> listdelete)
        {
            string sql = "";
            
            for (int i = 0; listdelete.Count > i; i++)
            {
                sql += "delete master_mm_item_picture_for_link where SID='" + sid + "' and ItmNumber='" + itemnumber + "' and file_name='" + listdelete[i] + "' ";
            }

            dbService.executeSQLForFocusone(sql);
        }
    }
}