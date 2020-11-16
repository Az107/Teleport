using System;
using Teleport;

namespace teleport
{
    class Program
    {
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
                Server server = new Server(args[0]);
                server.Start();
            }
            
        }
    }
}
    