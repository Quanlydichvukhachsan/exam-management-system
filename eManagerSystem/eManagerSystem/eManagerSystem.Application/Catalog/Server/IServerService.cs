
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static eManagerSystem.Application.Catalog.Server.ServerService;

namespace eManagerSystem.Application.Catalog.Server
{
   public interface IServerService
    {
        event UpdateHandler EventUpdateHandler;
        void Connect();
         void SendFile(string filePath);
        void SendUser(string option,List<Students> students);

        void SendSubject(string subject);

        void Receive(object obj);

        void Close();

        byte[] GetFilePath(string filePath);

        object Deserialize(byte[] data);

        List<Students> ReadAll(int gradeId);

        IEnumerable<Grade> getAllGrade();

        IEnumerable<Subject> getAllSubject();


    }
}
