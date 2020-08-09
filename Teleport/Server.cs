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
        public String FileName { get; set; }
        public bool isAlive = false;
        private TcpListener Listener { get; set; }
        private byte[] fileBytes;


        private async void ClientHandler(TcpClient client)
        {
            Progressbar pb = new Progressbar("c1",fileBytes.Length);
            pb.Start();
            //int maxsize = 500000;
            //int abscount = 0;
            //int count = 0;
            //byte[] package = new byte[maxsize];
            //foreach(byte b in fileBytes)
            //{
            //    pb.Change(abscount);
            //    package[count] = b;
            //    count++;
            //    abscount++;
            //    if (count >= maxsize || abscount == fileBytes.Length)
            //    {
            //        client.GetStream().Write(package);
            //        count = 0;
            //    }

            //}
            //pb.ChangePercent(100);
            byte[] name = new byte[1024];
            name = Encoding.UTF8.GetBytes(FileName);
            client.GetStream().Write(name);
            client.GetStream().Flush();
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
            FileName = file;
            Listener = new TcpListener(1100);
            isAlive = true;
            fileBytes = File.ReadAllBytes("./" + file);
        }
    }
}
