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
        public String filePath { get; set; }
        public String FileName { get; set; }
        public bool isAlive = false;
        private TcpListener Listener { get; set; }
        private long fileSize = -1;
        public string hash;
        public int Clients = 0;
        private int row = -1;
        const int chunkSize = 1024 * 1024;



        private async void ClientHandler(TcpClient client)
        {
            FileStream fileStream = File.OpenRead(filePath);
            Progressbar pb = new Progressbar("c1", fileSize);
            pb.Start();
            Clients++;
            updateCli();
            byte[] name = new byte[1024];
            name = Encoding.UTF8.GetBytes($"{FileName}");
            client.GetStream().Write(name);
            client.GetStream().Flush();
            var buffer = new byte[chunkSize];
            int bytesRead;
            int totalBytesReaded = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                totalBytesReaded += bytesRead;
                client.GetStream().Write(buffer);
                pb.Change(totalBytesReaded);
                Array.Clear(buffer, 0, buffer.Length);

            }
            client.Close();
            fileStream.Close();
            Clients--;
            updateCli();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void updateCli()
        {
            if (row == -1) row = Console.CursorTop;
            Console.CursorTop = row;
            Console.CursorLeft = 0;
            Console.Write(new String(' ', Console.WindowLeft));
            Console.CursorLeft = 0;
            if (Clients == 0) Console.WriteLine("Waiting for Connections...");
            else Console.WriteLine($"Active clients [{Clients}]");
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

            }

        }

        public Server(string file)
        {
            filePath = file;
            FileName = Path.GetFileName(file);
            fileSize = (new FileInfo(file)).Length;
            Listener = new TcpListener(1100);
            isAlive = true;

        }
    }
}
