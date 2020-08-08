using System;
using Teleport;

namespace teleport
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(args[1]);
            server.Start();
            
        }
    }
}
