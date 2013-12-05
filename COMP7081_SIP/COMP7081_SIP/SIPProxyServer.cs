using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace COMP7081_SIP
{
    class SIPProxyServer
    {
        private const int PORTNUM = 5060;

        protected UdpClient listenSocket;
        private IPEndPoint endPoint;

        public SIPProxyServer()
        {
            // Create the UdpClient that we can send/receive with.
            listenSocket = new UdpClient(PORTNUM);

            // Create the socket we'll listen on.
            endPoint = new IPEndPoint(IPAddress.Any, PORTNUM);
        }

        public void run()
        {
            received_byte
        }
    }
}
