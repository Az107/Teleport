using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace teleport{
    class Sender{
        IPAddress Reciver {get; set;}
        String file {get; set;}

        public Sender(string fileName,string ReciverAdr){
            Reciver = IPAddress.Parse(ReciverAdr);
            file = fileName;
            
        }

    }

}