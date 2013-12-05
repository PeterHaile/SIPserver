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

        public Message(String receivedString)
        {
            String[] messageParts = receivedString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            String[] contact = messageParts[(int)headerFieldIndex.Contact].Split(new string[] { ":", "<", "@", ">", ";" }, StringSplitOptions.RemoveEmptyEntries);
            String[] to = messageParts[(int)headerFieldIndex.To].Split(new string[] { "<", ":", "@", ">" }, StringSplitOptions.RemoveEmptyEntries);

            type = messageParts[(int) headerFieldIndex.Type].Split(' ')[0];
            user = contact[3];
            ip = contact[4];

            toUser = to[3];
            toIP = to[4];
        }
    }
}
