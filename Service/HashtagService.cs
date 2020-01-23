using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceWeb.Service
{
    public class HashtagService
    {
        DBService db = new DBService();

        private static HashtagService _instance;

        public static HashtagService getInstance()
        {
            if (_instance == null)
            {
                _instance = new HashtagService();
            }
            return _instance;
        }

        public DataTable getlistHashtag(string SID, string SEQ)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("select * ");
            sql.AppendLine("from SNA_HASHTAG");
            sql.AppendLine("where SID = '" + SID + "'");
            if (!string.IsNullOrEmpty(SEQ))
            {
                sql.AppendLine("and SEQ = '" + SEQ + "'");
            }
            sql.AppendLine("order by SEQ  asc");

            return db.selectData(sql.ToString());
        }

        public void AddHashTag(string sid, string hashtag, string created_by, string creaed_on)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("insert into SNA_HASHTAG");
            sql.AppendLine("(SID, Hashtag, created_by, created_on)");
            sql.AppendLine("values(");
            sql.AppendLine("'" + sid + "',");
            sql.AppendLine("'" + hashtag + "',");
            sql.AppendLine("'" + created_by + "',");
            sql.AppendLine("'" + creaed_on + "')");

            db.executeSQL(sql.ToString());
        }

        public void saveHashtagActivity(string SID, string Aobject, string SEQ)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("insert into SNA_ACTIVITY_HASHTAG");
            sql.AppendLine("(SID, AOBJECTLINK, HashtagSEQ)");
            sql.AppendLine("values('" + SID + "', ");
            sql.AppendLine("'" + Aobject + "', ");
            sql.AppendLine("'" + SEQ + "')");

            db.executeSQL(sql.ToString());
        }

        public DataTable getRefHashtag(string SID, string SNAID, string itemType, string Linkid)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("select distinct atg.HashtagSEQ, tag.Hashtag");
            sql.AppendLine("from SNA_" + SNAID + "_TIMEATTENDANCE atd");
            sql.AppendLine("inner join SNA_ACTIVITY_HASHTAG atg");
            sql.AppendLine("on atd.AOBJECTLINK = atg.AOBJECTLINK ");
            sql.AppendLine("left join SNA_HASHTAG tag");
            sql.AppendLine("on atg.HashtagSEQ = tag.SEQ");
            sql.AppendLine("where atd.ItemType = '" + itemType + "'");
            sql.AppendLine("and atd.CREATED_BY = '" + Linkid + "'");

            return db.selectData(sql.ToString());
        }

        public DataTable getHashtagByAobject(string SID, string AobjectLink)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("select atg.HashtagSEQ, tag.Hashtag");
            sql.AppendLine("from SNA_ACTIVITY_HASHTAG atg");
            sql.AppendLine("inner join SNA_HASHTAG tag");
            sql.AppendLine("on tag.SEQ = atg.HashtagSEQ");
            sql.AppendLine("where atg.SID = '" + SID + "' and ");
            sql.AppendLine("atg.AOBJECTLINK = '" + AobjectLink + "'");

            return db.selectData(sql.ToString());
        }

        public DataTable searchHashtagbySEQ(string sid, string SNAID, string seq)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("select tag.Hashtag, tag.SEQ, atd.* ");
            sql.AppendLine("from SNA_ACTIVITY_HASHTAG aht");
            sql.AppendLine("inner join SNA_HASHTAG tag");
            sql.AppendLine("on aht.HashtagSEQ = tag.SEQ");
            sql.AppendLine("inner join SNA_'" + SNAID + "'_TIMEATTENDANCE atd");
            sql.AppendLine("on aht.AOBJECTLINK = atd.AOBJECTLINK");
            sql.AppendLine("and atd.ItemType = 'CAT_PR'");
            sql.AppendLine("where aht.SID = '" + sid + "' and aht.HashtagSEQ = '10'");

            return db.selectData(sql.ToString());
        }
    }
}