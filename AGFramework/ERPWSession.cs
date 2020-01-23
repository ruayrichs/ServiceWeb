using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ERPWebMaster.AGFramework
{
    public class ERPWSession
    {
        public static Object Session(string name)
        {
            if (System.Web.HttpContext.Current == null)
                return null;
            if (!(System.Web.HttpContext.Current.Handler is IRequiresSessionState || System.Web.HttpContext.Current.Handler is IReadOnlySessionState))
                return null;
            return System.Web.HttpContext.Current.Session[name];
        }

        public static void Session(string name, Object o)
        {
            System.Web.HttpContext.Current.Session[name] = o;
        }

        
    }
}