using System;
using Teleport;

namespace teleport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "-r")
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
    