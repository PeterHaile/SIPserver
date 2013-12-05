using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMP7081_SIP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new SIP Proxy server.
            SIPProxyServer sipProxyServer = new SIPProxyServer();
            sipProxyServer.run();
        }
    }
}
