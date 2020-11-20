using System;
using Teleport;

namespace teleport
{
    class Program
    {
        private static int row = -1;
        private static Server server;
        private static void updateCli(String clientAddress)
        {
            //Progressbar pb = new Progressbar("c1", fileSize);
            if (row == -1) row = Console.CursorTop;
            Console.CursorTop = row;
            Console.CursorLeft = 0;
            Console.Write(new String(' ', Console.WindowLeft));
            Console.CursorLeft = 0;
            if (server.Clients == 0) Console.WriteLine("Waiting for Connections...");
            else Console.WriteLine($"Active clients [{server.Clients}]");
        }
        static void Main(string[] args)
        {
            if (args.Length is 0 or >2){
                Console.WriteLine("use:");
                Console.WriteLine($"\tTo send: {System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName} [file to send]");
                Console.WriteLine($"\tTo recive: {System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName} -r [sender ip]");

            }
            else if (args[0] == "-r")
            {
                Reciver reciver = new Reciver(args[1]);
            }
            else
            {
                updateCli("");
                server = new Server(args[0]);
                server.ClientConnectedEvent += updateCli;
                server.Start();
            }
            
        }
    }
}
    