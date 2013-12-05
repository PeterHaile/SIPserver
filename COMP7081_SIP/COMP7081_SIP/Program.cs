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
            // Read in from socket

            SIPProxyServer sipProxyServer = new SIPProxyServer();
            sipProxyServer.run();

            // Determine whether it's register or not


            // Send out of socket.



        }
    }
}
