﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.GitWrap.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Git git = new Git();            
            string local = @"C:\Users\DevInCube\Desktop\GitTest";
            //git.Clone("https://github.com/DevInCube/EdgeServerTest.git", local);
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            git.SetUser("DevInCube", "devincube@gmail.com", password);
            git.Connect(local);
            Console.WriteLine(String.Format("Connected to {0}", local));
            Console.WriteLine("Press any key to pull...");
            Console.ReadKey(true);
            git.Pull();
            Console.WriteLine("Press any key to push...");
            Console.ReadKey(true);
            git.Push();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}