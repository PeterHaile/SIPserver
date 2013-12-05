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
        private const int PORTNUM = 8080;

        protected UdpClient udpClient;
        private IPEndPoint ipEndPoint;

        public SIPProxyServer()
        {
            // Create the UdpClient that we can send/receive with.
            udpClient = new UdpClient(PORTNUM);
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            // Create the socket we'll listen on.
            ipEndPoint = new IPEndPoint(IPAddress.Any, PORTNUM);
            System.Console.WriteLine("Constructor done");
        }

        public void run()
        {
            byte[] received_bytes;

            while (true)
            {
                System.Console.WriteLine("HELLO");
                received_bytes = udpClient.Receive(ref ipEndPoint);
                System.Console.WriteLine("Recieved from: " + ipEndPoint);
                System.Console.WriteLine(Encoding.ASCII.GetString(received_bytes));
            }
        }
    }
}
