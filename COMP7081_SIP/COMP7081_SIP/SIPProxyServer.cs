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
        private const String StartMessage = "Starting Server...";

        protected UdpClient udpClient;
        private IPEndPoint ipEndPoint;
        
        public SIPProxyServer()
        {
            // Create the UdpClient that we can send/receive with.
            udpClient = new UdpClient(PORTNUM);

            // Create the socket we'll listen on.
            ipEndPoint = new IPEndPoint(IPAddress.Any, PORTNUM);
        }

        public void run()
        {
            byte[] receivedBytes; // Used to store the bytestream we get from the socket.
            Message receivedMessage;

            System.Console.WriteLine(StartMessage);

            while (true)
            {
                // Block on receive until we get a message.
                receivedBytes = udpClient.Receive(ref ipEndPoint);

                String receivedString = Encoding.ASCII.GetString(receivedBytes); // Convert the bytes to a string.
                
                if (!receivedString.Equals("jaK\0"))
                {
                    receivedMessage = new Message(receivedString);

                    // If the message is a REGISTER message, try to register the user.
                    if (receivedMessage.type.Equals("REGISTER"))
                    {
                        // Add the user to the registrar (will replace if already exists).
                        Registrar.addUser(receivedMessage.user, receivedMessage.ip);
                        System.Console.WriteLine("Added user");
                    }
                    // Otherwise, check if the user is registered and forward it.
                    else
                    {
                        // Check if the user is registered before forwarding.
                        if (Registrar.checkUser(receivedMessage.user) != null)
                        {
                            udpClient.Send(receivedBytes, receivedBytes.Length, receivedMessage.toIP, PORTNUM);
                        }
                    }

                    System.Console.WriteLine("Type:" + receivedMessage.type);
                    System.Console.WriteLine("Type:" + receivedMessage.user);
                    System.Console.WriteLine("Type:" + receivedMessage.ip);
                    System.Console.WriteLine("Type:" + receivedMessage.toIP);
                    System.Console.WriteLine("Type:" + receivedMessage.toUser);
                }
            }
        }
    }
}
