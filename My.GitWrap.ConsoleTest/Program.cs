using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace My.GitWrap.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string authFile = "auth.xml";
            XDocument doc = null;
            string password = null;
            try
            {
                doc = XDocument.Load(authFile);
                password = doc.Root.Element("password").Value;
            }
            catch (IOException)
            {
                Console.WriteLine("Enter password:");
                password = Console.ReadLine();
                doc = new XDocument(
                    new XElement("auth", 
                        new XElement("password", password)));
                doc.Save(authFile);
            }
            Git git = new Git();
            string local = @"C:\Users\user\Desktop\Новая папка (7)";
            //git.Clone("https://github.com/DevInCube/EdgeServerTest.git", local);
            git.SetUser("DevInCube", "devincube@gmail.com", password);
            git.Connect(local);
            Console.WriteLine(String.Format("Connected to {0}", local));
            Console.WriteLine("Press any key to pull...");
           // Console.ReadKey(true);
            git.Pull();
            Console.WriteLine("Press any key to push...");
            //Console.ReadKey(true);
            git.Push();
            Console.WriteLine("Press any key to exit...");
            //Console.ReadKey(true);
        }
    }
}
