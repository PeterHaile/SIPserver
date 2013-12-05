using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMP7081_SIP
{
    class Message
    {
        String invite;
        String via;
        String to;
        String from;
        String callid;
        String cseq;
        String contact;
        String contactType;
        String contentLength;

        public Message(byte[] bytes)
        {
            System.Console.Out.WriteLine("Hello");
        }
    }
}
