
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
         void SendFile(string filePath);
        void SendUser(string option,List<Students> students);

        void Receive(object obj);

        void Close();

        byte[] GetFilePath(string filePath);

        object Deserialize(byte[] data);

        List<Students> ReadAll(int gradeId);

        IEnumerable<Grade> getAllGrade();
    }
}
