using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace COMP7081_SIP
{
    class SIPProxyServer
    {
        private const int PORTNUM = 5060;
        private const String StartMessage = "Starting Server...";
        private const String RECEIVELOGFILE = "ReceiveLogFile";
        private const String SENDLOGFILE = "SendLogFile";
        private const String REGISTRARLOGFILE = "RegistrarLogFile";
        private const String SYSTEMLOGFILE = "SystemLogFile";

        protected UdpClient udpClient; // The udpClient that will send / receive UDP packets for us.
        private IPEndPoint ipEndPoint; // The socket that we'll listen on.
        private Timer timer = new Timer(900000); // System log timer.

        /// <summary>
        /// Constructor for our SIPProxyServer. Initializes the socket for the program.
        /// </summary>
        public SIPProxyServer()
        {
            // Create the UdpClient that we can send/receive with.
            udpClient = new UdpClient(PORTNUM);

            // Create the a listening socket on PORTNUM.
            ipEndPoint = new IPEndPoint(IPAddress.Any, PORTNUM);

            Logger.logToFile(SYSTEMLOGFILE, "Server initialized.");

            // Start a timer to constantly update every 
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timerElapsed);
        }

        /// <summary>
        /// Event handler for when the timer has elapsed (print a new log in SYSTEMLOG).
        /// </summary>
        /// <param name="sender">timer</param>
        /// <param name="e">eventargs</param>
        void timerElapsed(object sender, ElapsedEventArgs e)
        {
            Logger.logToFile(SYSTEMLOGFILE, "Server is still running.");
        }

        /// <summary>
        /// Run method for the program that constantly reads in from the socket and handles the incoming packets according to SIP.
        /// </summary>
        public void run()
        {
            byte[] receivedBytes; // Used to store the bytestream that we get from the socket.
            Message receivedMessage; // User to store the message object that is parsed from the byte stream.

            // Display a system startup message.
            System.Console.WriteLine(StartMessage);
            Logger.logToFile(SYSTEMLOGFILE, "Server has started running.");

            // Keep reading bytes from the socket.
            while (true)
            {
                // Block on receive until we get a message.
                receivedBytes = udpClient.Receive(ref ipEndPoint);

                // Convert the bytes to a string.
                String receivedString = Encoding.ASCII.GetString(receivedBytes);
                
                // Disregard all incoming jaK packets.
                if (!receivedString.Equals("jaK\0"))
                {
                    receivedMessage = new Message(receivedString); // Create a message object out of the string.

                    Logger.logToFile(RECEIVELOGFILE, receivedString); // Log any received data.

                    // If the message is a REGISTER message, try to register the user.
                    if (receivedMessage.type.Equals("REGISTER"))
                    {
                        // Add the user to the registrar (will replace if already exists).
                        Registrar.addUser(receivedMessage.contactUser, receivedMessage.contactIP);

                        Logger.logToFile(REGISTRARLOGFILE, "Adding record: " + receivedMessage + "@" + receivedMessage.contactIP); // Log whenever we add a user.
                        
                        // Get the successful registration message.
                        byte[] sendBytes = Encoding.ASCII.GetBytes(receivedMessage.getRegistrationSuccessMessage());

                        // Send this message back to the user that wants to register.
                        udpClient.Send(sendBytes, sendBytes.Length, receivedMessage.contactIP, PORTNUM);
                    }
                    // Otherwise, check if the user is registered and forward it.
                    else
                    {
                        // Check if the user is registered before forwarding.
                        if (Registrar.checkUser(receivedMessage.contactUser) != null)
                        {
                            String ip; // This will be the ip that we will send the packet to.

                            // Check to see whether the packet needs to be forwarded, or sent back.
                            if (receivedMessage.type.Equals("INVITE") || receivedMessage.type.Equals("CANCEL") || receivedMessage.type.Equals("BYE") || receivedMessage.type.Equals("OPTIONS"))
                            {
                                // Attempt to retrieve the IP for the user.
                                ip = Registrar.checkUser(receivedMessage.toUser);

                                // Check to see if they are special invite packets that need to be forwarded.
                                if (receivedMessage.isTrying || receivedMessage.isDialogEstablishment || receivedMessage.isOK || receivedMessage.isRinging)
                                {
                                    ip = Registrar.checkUser(receivedMessage.fromUser);
                                }
                            }
                            else
                            {
                                // Attempt to retrieve the IP for the user.
                                ip = Registrar.checkUser(receivedMessage.fromUser);
                            }

                            // If the ip is null, don't send a packet as the user was not found in the registrar.
                            if (ip != null)
                            {
                                // Forward the packet to the recipient that has been chosen.
                                udpClient.Send(receivedBytes, receivedBytes.Length, ip, PORTNUM);

                                // Log any data that we sent.
                                Logger.logToFile(SENDLOGFILE, receivedString);
                            }
                        }
                    }

                    // Write output to the console to show new messages.
                    System.Console.WriteLine("-----NEW MESSAGE-----");
                    System.Console.WriteLine("Type: " + receivedMessage.type);
                    System.Console.WriteLine("From: " + receivedMessage.fromUser + "@" + receivedMessage.fromIP);
                    System.Console.WriteLine("To: " + receivedMessage.toUser + "@" + receivedMessage.toIP);
                }
            }
        }
    }
}
