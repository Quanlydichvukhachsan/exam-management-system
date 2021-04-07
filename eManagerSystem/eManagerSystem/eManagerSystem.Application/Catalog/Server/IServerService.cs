using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eManagerSystem.Application.Catalog.Server
{
   public interface IServerService
    {
         void Connect();
         void Send(string filePath);

        void Receive(object obj);

        void Close();

        byte[] GetFilePath(string filePath);

        object Deserialize(byte[] data);


    }
}
