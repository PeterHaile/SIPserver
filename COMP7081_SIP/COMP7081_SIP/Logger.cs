using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace COMP7081_SIP
{
    public class Logger
    {
        private static string localPath = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath + "\\Logs\\";
        
        public static void logToFile(string logFileName, string logEntry)
        {
            if(!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            using (FileStream registeredUsers = (System.IO.File.Exists(localPath + logFileName) 
                ? new FileStream(localPath + logFileName, FileMode.Append) : new FileStream(localPath + logFileName, FileMode.OpenOrCreate)))
            using (StreamWriter write = new StreamWriter(registeredUsers))
            {
                write.WriteLine("[" + DateTime.Now.ToString() + "]\n" + logEntry);
            }
        }
    }
}
