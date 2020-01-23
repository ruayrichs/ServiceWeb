using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

public static class ObjectUtil
{
    public static T Copy<T>(T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable", "source");
        }

        if (Object.ReferenceEquals(source, null))
        {
            return default(T);
        }
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using (stream)
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }

    public static decimal ToDecimal(object _in)
    {
        decimal _rs = 0;
        if (_in != null)
        {
            decimal.TryParse(_in.ToString(), out _rs);
        }
        return _rs;
    }

    public static string Err(string _in)
    {
        string _rs = _in.Replace("'", "");
        _rs = _rs.Replace("\r", "");
        _rs = _rs.Replace("\n", "<br/>");
        return _rs;
    }

    public static string StripTagsCharArray(string source)
    {
        char[] array = new char[source.Length];
        int arrayIndex = 0;
        bool inside = false;

        for (int i = 0; i < source.Length; i++)
        {
            char let = source[i];
            if (let == '<')
            {
                inside = true;
                continue;
            }
            if (let == '>')
            {
                inside = false;
                continue;
            }
            if (!inside)
            {
                array[arrayIndex] = let;
                arrayIndex++;
            }
        }
        return new string(array, 0, arrayIndex);
    }

    public static string PrepareCodeAndDescription(string code, string desc)
    {
        string resultDesc = code;

        if (!string.IsNullOrEmpty(desc))
        {
            resultDesc += resultDesc == "" ? "" : " : ";
            resultDesc += desc;
        }

        return resultDesc;
    }

    public static DateTime ConvertDateTimeDBToDateTime(string dateTime)
    {
        if (string.IsNullOrEmpty(dateTime) || dateTime.Length != 14)
        {
            dateTime = Agape.FocusOne.Utilities.Validation.getCurrentServerStringDateTime();
        }

        DateTime dt = new DateTime(
                    Convert.ToInt32(dateTime.Substring(0, 4)),
                    Convert.ToInt32(dateTime.Substring(4, 2)),
                    Convert.ToInt32(dateTime.Substring(6, 2)),
                    Convert.ToInt32(dateTime.Substring(8, 2)),
                    Convert.ToInt32(dateTime.Substring(10, 2)),
                    Convert.ToInt32(dateTime.Substring(12, 2)));

        return dt;
    }
}
