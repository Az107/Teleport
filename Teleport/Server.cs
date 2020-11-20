using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace Teleport
{
    public class Server
    {

        public Exception NoFileException;
        public delegate void NewClientd(String ip);
        public event NewClientd ClientConnectedEvent;
        public event NewClientd ClientDisconnectedEvent;




        public String filePath { get; set; }
        public String FileName { get; set; }
        public bool isAlive = false;
        private TcpListener Listener { get; set; }
        private long fileSize = -1;
        public string hash;
        public int Clients = 0;

        const int chunkSize = 1024 * 1024;
        private int Port = 1100;

        private CancellationTokenSource cts = new CancellationTokenSource();    

        private  void ClientHandler(TcpClient client)
        {
            ClientConnectedEvent?.Invoke(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            FileStream fileStream = File.OpenRead(filePath);
            Clients++;
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
                Array.Clear(buffer, 0, buffer.Length);

            }
            client.Close();
            fileStream.Close();
            Clients--;
            ClientDisconnectedEvent?.Invoke(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        public void Stop(){
            isAlive = false;
            int timeOut = 10;
            while(Clients != 0 && timeOut != 0){
                Thread.Sleep(100);
                timeOut --;
            }
            cts.Cancel();
        }



        public void Start(bool inBackground = true){
            if (inBackground) Task.Run(Start);
            else Start();
        }
        private  void Start()
        {

            if (string.IsNullOrEmpty(filePath)) throw NoFileException;
            isAlive = true;
            Listener.Start();
            while (isAlive)
            {
                Task<TcpClient> clientTask = Listener.AcceptTcpClientAsync();
                clientTask.Wait(cts.Token);
                Task clientTask2 = new Task(() => ClientHandler(clientTask.Result),cts.Token);
                clientTask2.Start();

            }              

        }

        public void AddFile(String file){
            filePath = file;
            FileName = Path.GetFileName(file);
            fileSize = (new FileInfo(file)).Length;

        }


        public Server(string file)
        {
            AddFile(file);
            Listener = new TcpListener(IPAddress.Any,Port);

        }
    }
}
