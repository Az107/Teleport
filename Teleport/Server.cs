using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Teleport
{
    class Server
    {
        public string filePath { get; set; }
        public String FileName { get; set; }
        public bool isAlive = false;
        private TcpListener Listener { get; set; }
        private byte[] fileBytes;
        public string hash;
        private bool lowMemoryMode = false;
        public int Clients = 0;
        private int row = -1;

   

        private async void ClientHandler(TcpClient client)
        {
            //Progressbar pb = new Progressbar("c1",fileBytes.Length);
            //pb.Start();
            Clients++;
            updateCli();
            byte[] name = new byte[1024];
            name = Encoding.UTF8.GetBytes($"{FileName}");
            client.GetStream().Write(name);
            client.GetStream().Flush();
            client.GetStream().Write(fileBytes);
            client.Close();
            Clients--;
            updateCli();
        }

        private void updateCli()
        {
            if (row == -1) row = Console.CursorTop;
            Console.CursorTop = row;
            Console.CursorLeft = 0;
            Console.Write(new String(' ',Console.WindowLeft));
            Console.CursorLeft = 0;
            if (Clients == 0) Console.WriteLine("Waiting for Connections...");
            else   Console.WriteLine($"Active clients [{Clients}]");
        }

        public void Start()
        {
            Listener.Start();
            updateCli();
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

            FileName = Path.GetFileName(file);
            Listener = new TcpListener(1100);
            isAlive = true;
            filePath = file;
            fileBytes = File.ReadAllBytes(file);
            Console.WriteLine("OK");

        }
    }
}
