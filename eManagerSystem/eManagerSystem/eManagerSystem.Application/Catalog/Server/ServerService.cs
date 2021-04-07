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
        public string Message = "";
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
        public void Send(string message)
        {
            foreach( Socket client in clientList)
            {
                if (message != String.Empty)
                {
                    client.Send(Serialize(message));
                    SetMessage(message);
                }
            }
           
        }
        private void SetMessage(string message)
        {

            Message = message;
        }

        public string GetMessage()
        {
            return Message;
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
        private void AddMessage( string message)
        {
            //
        }

        public void Close()
        {
            server.Close();
        }

        private byte[] Serialize(object data)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(memoryStream, data);
            return memoryStream.ToArray();
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
