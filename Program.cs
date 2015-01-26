using System;
using Ionic;
using QueryMaster;
using System.Threading;
using System.Diagnostics;

namespace SQSC
{
    class Program
    {

        public static string ip; 
        public static ushort port; 
        public static string pName; 
        public static string pPath; 

        public static int count = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Server IP:");
            ip = Console.ReadLine(); //must be 127.0.0.1 for autorestarts
            Console.WriteLine("Server Query Port (as default = GamePort+1):");
            string sport = Console.ReadLine();
            port = Convert.ToUInt16(sport); //server port
            Console.WriteLine("Process name without .exe:");
            pName = Console.ReadLine(); //process name
            Console.WriteLine("Path to server for autorestart:");
            pPath = @Console.ReadLine(); //path to server
            while (true)
            {
                Console.WriteLine(DateTime.Now + " Trying to connect...");
                Server server = ServerQuery.GetServerInstance(EngineType.Source, ip, port);

                Thread.Sleep(1000);
                try
                {
                    ServerInfo info = server.GetInfo();
                    Console.WriteLine(DateTime.Now + " Connected!");
                    Thread.Sleep(1000);
                    Console.WriteLine(DateTime.Now + " Players:" + info.Players);
                    Thread.Sleep(1000);
                    Console.WriteLine(DateTime.Now + " Dispose connect.");
                    count = 0;
                }
                catch
                {
                    Console.WriteLine(DateTime.Now + " Can't connect!");
                    Thread.Sleep(1000);
                    Restart();
                }

                server.Dispose();
                Thread.Sleep(1000);
            }
        }
        static void Restart()
        {
            if (count == 10)
            {
                Console.WriteLine(DateTime.Now + " Closing crashed server...");
                System.Diagnostics.Process[] etc = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process anti in etc)
                    if (anti.ProcessName.ToLower().Contains(pName.ToLower())) anti.Kill();
                Thread.Sleep(5000);
                Console.WriteLine(DateTime.Now + " Trying to start");
                Process.Start(pPath);
                Thread.Sleep(60000);
                try
                {
                    string namewf = "WerFault";
                    System.Diagnostics.Process[] werfault = System.Diagnostics.Process.GetProcesses();
                    foreach (System.Diagnostics.Process anti in werfault)
                        if (anti.ProcessName.ToLower().Contains(namewf.ToLower())) anti.Kill();
                }
                catch { }
                count = 0;
                Thread.Sleep(60000);
            }
            else
            {
                Console.WriteLine(DateTime.Now + " Attempt to check: " + count);
                Thread.Sleep(1000);
                count++;
            }
        }
    }

}
