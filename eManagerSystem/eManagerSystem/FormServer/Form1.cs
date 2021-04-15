using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using eManagerSystem.Application;
using eManagerSystem.Application.Catalog.Server;
namespace FormServer
{
    public partial class Form1 : Form
    {


        IServerService _server;
        private List<PC> listUser = new List<PC>();
        List<Students> _students;
        private Color ColorRed = Color.FromArgb(255, 95, 79);
        private Color ColorGreen = Color.FromArgb(54, 202, 56);
        public Form1(IServerService server)
        {
            _server = server;
            _server.EventUpdateHandler += _server_EventUpdateHandler;
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;


        }
        private void UpdateUserControll(string mssv)
        {
            foreach(var items in listUser)
            {
                if (items.MSSV == mssv)
                {
                    items.ColorUser = ColorGreen;   
                    
                } 
             }
            LoadDisPlayUser();

        }
        

        private void _server_EventUpdateHandler(object sender, ServerService.UpdateEventArgs args)
        {
            UpdateUserControll(args.mssv);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _server.Connect();
            
        }

        private void cmdBatDauLamBai_Click(object sender, EventArgs e)
        {
         


        }


        private OpenFileDialog openFileDialog1;
        // them de thi
        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    var PathName = openFileDialog1.FileName;
                    lstDeThi.Items.Add(PathName);
                    _server.SendFile(PathName);


                }
                catch(Exception er)
                {
                    throw er;
                 //   MessageBox.Show("Loi mo file");

                }
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            IEnumerable<Grade> grades = _server.getAllGrade();
            Form2 form2 = new Form2(grades, _server);
            form2.EventUpdateHandler += Form2_EventUpdateHandler;
            form2.Show();
        }

        private void Form2_EventUpdateHandler(object sender, Form2.UpdateEventArgs args)
        {
            _students = args.studentsDelegate;
           // AddListUser(_students);
          //  LoadDisPlayUser();
            _server.SendUser("Send User", _students);
        }

        private void LoadDisPlayUser()
        {
            flowLayoutContainer.Controls.Clear();
            for (int i = 0; i < listUser.Count; i++)
            {
                flowLayoutContainer.Controls.Add(listUser[i]);

            }
        }
        private void AddListUser(List<Students> students)
        {
            if (students.Count > 0)
            {
                int index = 0;
                foreach (var items in students)
                {
                    index++;
                    PC pC = new PC();
                    pC.MSSV = items.MSSV.ToString();
                    pC.pcName = index.ToString();
                    pC.ColorUser = ColorRed;
                    listUser.Add(pC);
                }
            }

        }

        private void cbCbonMonThi_Click(object sender, EventArgs e)
        {
        
            IEnumerable<Subject> subjects = _server.getAllSubject();
            cbCbonMonThi.DataSource = subjects;
            cbCbonMonThi.DisplayMember = "SubjectName";
            cbCbonMonThi.ValueMember = "SubjectId";
        }

        private void cmdChapNhan_Click(object sender, EventArgs e)
        {
            if(cbCbonMonThi.Text != string.Empty)
            {
                _server.SendSubject(cbCbonMonThi.Text);
            }
          
        }
    }
}
