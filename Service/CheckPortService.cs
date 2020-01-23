using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace ServiceWeb.Service
{
    public class CheckPortService
    {
        private DBService db = new DBService();

        public DataTable getDataPort()
        {
            string sql = @"select * from ERPW_PORT_SERVICE";
            return db.selectDataFocusone(sql);
        }

        public bool CheckPortStatus(string IP, int Port)
        {
            bool status = false;
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(IP, Convert.ToInt32(Port));
                    status = true;
                    Console.WriteLine("Port open");
                }
                catch (Exception ex)
                {
                    status = false;
                }
            }
            return status;
        }
    }
}