using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMP7081_SIP
{
    class Message
    {
        enum headerFieldIndex { Type, Via, From, To, CallID, CSeq, Contact, MaxForwards, UserAgent, Expires, ContentLength };

        public String type { get; private set; }
        public String toUser { get; private set; }
        public String toIP { get; private set; }
        public String fromUser { get; private set; }
        public String fromIP { get; private set; }
        public String contactUser { get; private set; }
        public String contactIP { get; private set; }
        public String branch { get; private set; }
        private String[] messageParts;
        private String[] via;
        public Boolean isTrying { get; private set; }
        public Boolean isDialogEstablishment { get; private set; }
        public Boolean isOK { get; private set; }
        public Boolean isRinging { get; private set; }

        public Message(String receivedString)
        {
            isTrying = false;
            isDialogEstablishment = false;
            isOK = false;
            isRinging = false;

            messageParts = receivedString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            String[] contact = messageParts[(int)headerFieldIndex.Contact].Split(new string[] { ":", "<", "@", ">", ";" }, StringSplitOptions.RemoveEmptyEntries);
            String[] cSeq = messageParts[(int)headerFieldIndex.CSeq].Split(' ');
            String[] to = messageParts[(int)headerFieldIndex.To].Split(new string[] { "<", ":", "@", ">" }, StringSplitOptions.RemoveEmptyEntries);
            String[] from = messageParts[(int)headerFieldIndex.From].Split(new string[] { "<", ":", "@", ">" }, StringSplitOptions.RemoveEmptyEntries);
            via = messageParts[(int)headerFieldIndex.Via].Split(new string[] { " ", ";", "=", ":"}, StringSplitOptions.RemoveEmptyEntries);

            String statusLine = messageParts[0];

            type = cSeq[cSeq.Length - 1];
            
            // If this is a trying message
            if (statusLine.Equals("SIP/2.0 100 Trying"))
            {
                isTrying = true;
            }
            else if (statusLine.Equals("SIP/2.0 101 Dialog Establishement"))
            {
                isDialogEstablishment = true;
            }
            else if (statusLine.Equals("SIP/2.0 200 OK"))
            {
                isOK = true;
            }
            else if (statusLine.Equals("SIP/2.0 180 Ringing"))
            {
                isRinging = true;
            }

            contactUser = from[3];
            contactIP = via[2];

            branch = via[6];
            toUser = to[3];
            toIP = to[4];
            fromUser = from[3];
            fromIP = from[4];
        }

        public string getRegistrationSuccessMessage()
        {
            String response = "SIP/2.0 200 Registration successful\r\n"
                            + via[0] + ";"
                            + "rport=5060" + ";"
                            + "received=" + contactUser
                            + "branch=" + via[3] + "\r\n"
                            + messageParts[(int)headerFieldIndex.From] + "\r\n"
                            + messageParts[(int)headerFieldIndex.To] + "\r\n"
                            + messageParts[(int)headerFieldIndex.CallID] + "\r\n"
                            + messageParts[(int)headerFieldIndex.CSeq] + "\r\n"
                            + messageParts[(int)headerFieldIndex.Contact] + ";"
                            + messageParts[(int)headerFieldIndex.Expires] + ";"
                            //+ "q=0.00\r\n"
                            //+ "Server: Flexisip/0.6.0 (sofia-sip-nta/2.0)" + "\r\n"
                            + messageParts[(int)headerFieldIndex.ContentLength] + "\r\n\r\n";
            return response;
        }
    }
}
