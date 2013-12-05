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
                        Registrar.addUser(receivedMessage.contactUser, receivedMessage.contactIP);
                        
                        // Get the successful registration message.
                        byte[] sendBytes = Encoding.ASCII.GetBytes(receivedMessage.getRegistrationSuccessMessage());

                        // Send this message back to the user that wants to register.
                        udpClient.Send(sendBytes, sendBytes.Length, receivedMessage.contactIP, PORTNUM);
                        System.Console.WriteLine("Added user");
                    }
                    // Otherwise, check if the user is registered and forward it.
                    else
                    {
                        // Check if the user is registered before forwarding.
                        if (Registrar.checkUser(receivedMessage.contactUser) != null)
                        {
                            String ip;

                            if (receivedMessage.type.Equals("INVITE") || receivedMessage.type.Equals("CANCEL") || receivedMessage.type.Equals("BYE") || receivedMessage.type.Equals("OPTIONS"))
                            {
                                ip = Registrar.checkUser(receivedMessage.toUser);
                            }
                            else
                            {
                                ip = Registrar.checkUser(receivedMessage.fromUser);
                            }

                            udpClient.Send(receivedBytes, receivedBytes.Length, ip, PORTNUM);
                        }
                    }

                    System.Console.WriteLine("Type:" + receivedMessage.type);
                    System.Console.WriteLine("Type:" + receivedMessage.contactUser);
                    System.Console.WriteLine("Type:" + receivedMessage.contactIP);
                    System.Console.WriteLine("Type:" + receivedMessage.toIP);
                    System.Console.WriteLine("Type:" + receivedMessage.toUser);
                }
            }
        }
    }
}
