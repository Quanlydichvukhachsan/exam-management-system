using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace eManagerSystem.Application.Catalog.Server
{
   public class ServerService
    {
        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;
      
        public void Connect()
        {
            clientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, 9999);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            server.Bind(IP);

            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        clientList.Add(client);
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    IP = new IPEndPoint(IPAddress.Any, 9999);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
              
            });
            Listen.IsBackground = true;
            Listen.Start();
                

        }
        public void Send(string filePath)
        {
            foreach( Socket client in clientList)
            {
                if (filePath != String.Empty)
                {
                  
                    client.Send(Serialize(filePath));
                   
                }
            }
           
        }
       

        public void  Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string message = (string)Deserialize(data);
                  
                }
              
            }
            catch
            {
                clientList.Remove(client);
                client.Close();
            }


        }
     

        public void Close()
        {
            server.Close();
        }

        private byte[] Serialize(string filePath)
        {
            var name = Path.GetFileName(filePath);
            byte[] fNameByte = Encoding.ASCII.GetBytes(name);
            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] serverData = new byte[4 + fNameByte.Length + fileData.Length];
            byte[] fNameLength = BitConverter.GetBytes(fNameByte.Length);
            fNameLength.CopyTo(serverData, 0);
            fNameByte.CopyTo(serverData, 4+ fNameByte.Length);

            return serverData;

        }

        private object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Deserialize(stream);
            return stream;
        }
     
    }
}
