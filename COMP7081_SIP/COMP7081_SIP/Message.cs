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
        public String user { get; private set; }
        public String ip { get; private set; }
        public String branch { get; private set; }

        public Message(String receivedString)
        {
            String[] messageParts = receivedString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            String[] contact = messageParts[(int)headerFieldIndex.Contact].Split(new string[] { ":", "<", "@", ">", ";" }, StringSplitOptions.RemoveEmptyEntries);
            String[] to = messageParts[(int)headerFieldIndex.To].Split(new string[] { "<", ":", "@", ">" }, StringSplitOptions.RemoveEmptyEntries);
            String[] via = messageParts[(int)headerFieldIndex.Via].Split(new string[] { ";", "=" }, StringSplitOptions.RemoveEmptyEntries);

            type = messageParts[(int) headerFieldIndex.Type].Split(' ')[0];
            user = contact[3];
            ip = contact[4];
            branch = via[3];
            toUser = to[3];
            toIP = to[4];
        }

        public string registrationSuccessMessage()
        {
            //String response = "SIP/2.0 200 Registration successful\r\n"
              //              + 
            return "";
        }
    }
}
