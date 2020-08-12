using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Teleport
{
    class Reciver
    {
        string filename { get; set;  }
       

        public Reciver(string ip)
        {
            TcpClient client = new TcpClient();
            client.Connect(ip, 1100);
            NetworkStream stream = client.GetStream();
            byte[] nameByte = new byte[1024];
            stream.Read(nameByte,0,1024);
            filename = Encoding.UTF8.GetString(nameByte).TrimEnd('\0');
            Console.WriteLine($"Downloading {filename}");
            try
            {
                var file = File.Create("./" + filename);
                file.Close();
                FileStream fs = File.OpenWrite("./" + filename);
                stream.Flush();
                stream.CopyTo(fs);
                fs.Close();
                stream.Close();
            }catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
            client.Close();


        }
    }
}
