using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

using teleport;

namespace Teleport
{
    class Server
    {
        public String file { get; set; }
        public bool isAlive = false;
        private TcpListener Listener { get; set; }
        private byte[] fileBytes;


        private async void ClientHandler(TcpClient client)
        {
            client.GetStream().Write(fileBytes);
            client.Close();
        }


        public void Start()
        {
            Listener.Start();
            while (isAlive)
            {
                Task<TcpClient> clientTask = Listener.AcceptTcpClientAsync();
                clientTask.Wait();
                Task clientTask2 = new Task(() => ClientHandler(clientTask.Result));
                clientTask2.Start();
                
                //clientTask.Start();
            }

        }

        public Server(string file)
        {
            Listener = new TcpListener(1100);
            isAlive = true;
            fileBytes = File.ReadAllBytes("./" + file);
        }
    }
}
