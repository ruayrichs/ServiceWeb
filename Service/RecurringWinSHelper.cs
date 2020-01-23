using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Web;

namespace ServiceWeb.Service
{
    public class RecurringWinSHelper
    {
        public const String SERVICENAME_RECURRING = "Link Job Schedule Service_";

        private RecurringWinSHelper()
        {

        }

        public static void restartService(String sid)
        {
            String serviceName = SERVICENAME_RECURRING + sid;

            processStopService(serviceName);

            processStartService(serviceName);
        }

        public static void startService(String sid)
        {
            String serviceName = SERVICENAME_RECURRING + sid;
            processStartService(serviceName);
        }

        public static void stopService(String sid)
        {
            String serviceName = SERVICENAME_RECURRING + sid;
            processStopService(serviceName);
        }

        private static void processStartService(String serviceName)
        {
            ServiceController svcController = new ServiceController(serviceName);

            if (svcController == null)
            {
                return;
            }

            try
            {
                if ((svcController.Status != ServiceControllerStatus.Running) && (svcController.Status != ServiceControllerStatus.StartPending))
                {
                    svcController.Start();
                }
            }
            catch (Exception ex)
            {
                // ABD add log 
                throw ex;
            }
        }

        private static void processStopService(String serviceName)
        {
            ServiceController svcController = new ServiceController(serviceName);

            if (svcController == null)
            {
                return;
            }

            try
            {
                if ((svcController.Status == ServiceControllerStatus.Running) && (svcController.CanStop))
                {
                    svcController.Stop();
                    svcController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
            }
            catch (Exception ex)
            {
                // ABD add log 
                throw ex;
            }

        }
    }
}