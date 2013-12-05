using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Reflection;


namespace COMP7081_SIP
{
    public class Registrar
    {
        private static char[] delim = { ',' };
        private static string localPath = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath + "\\RegisteredUsers.csv";
        public static void addUser(string name, string ip)
        {
            try
            {
                Hashtable allUsers = getUserData();
                if (allUsers != null)
                {
                    if (!allUsers.ContainsKey(name))
                    {
                        using (FileStream registeredUsers = new FileStream(localPath, FileMode.Append))
                        using (StreamWriter write = new StreamWriter(registeredUsers))
                        {
                            write.WriteLine(name + "," + ip);
                        }
                    }
                    else
                    {
                        allUsers[name] = ip;
                        using (FileStream registeredUsers = new FileStream(localPath, FileMode.Create))
                        using (StreamWriter write = new StreamWriter(registeredUsers))
                        {
                            foreach (DictionaryEntry pair in allUsers)
                            {
                                write.WriteLine(pair.Key.ToString() + "," + pair.Value.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.Data.ToString());
            }
        }

        public static string checkUser(string name)
        {
            Hashtable allUsers = getUserData();
            return (allUsers != null && allUsers.ContainsKey(name)) ? allUsers[name].ToString() : null;
        }

        private static Hashtable getUserData()
        {
            try
            {
                string line;
                Hashtable data = new Hashtable();
                using (FileStream registeredUsers = new FileStream(localPath, FileMode.OpenOrCreate))
                {
                    using (StreamReader reader = new StreamReader(registeredUsers))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] commaSeperatedValue = line.Split(delim);
                            if (commaSeperatedValue.Length == 2)
                            {
                                data.Add(commaSeperatedValue[0], commaSeperatedValue[1]);
                            }
                        }
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.Data.ToString());
            }
            return null;
        }
    }
}
