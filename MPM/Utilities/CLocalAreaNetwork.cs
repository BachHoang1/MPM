// author: hoan chau
// purpose: check local network for ip addresses that might be set up as a client

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace MPM.Utilities
{
    class CLocalAreaNetwork
    {                                           
        public string GetMyIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            string sLocalBaseNetworkAddress = "192.168.1.2";  // initialize
            for (int i = 0; i < ipHostInfo.AddressList.Length; i++)
            {
                if (ipHostInfo.AddressList[i].ToString().Contains("."))  // must be ipv4
                {                    
                    sLocalBaseNetworkAddress = ipHostInfo.AddressList[i].ToString();
                    break;
                }
            }

            return sLocalBaseNetworkAddress;
        }
    }
}
