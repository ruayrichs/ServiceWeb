using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class JSONUtil
{
    public static string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<Dictionary<string, object>> rows =
          new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName.Trim(), dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

    public static string GetJson(Object obj)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        return serializer.Serialize(obj);
    }
}