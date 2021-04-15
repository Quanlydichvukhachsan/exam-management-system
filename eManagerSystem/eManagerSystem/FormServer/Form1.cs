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
using LinqToExcel;
using eManagerSystem.Application;
using eManagerSystem.Application.Catalog.Server;
namespace FormServer
{
    public partial class Form1 : Form
    {


        IServerService _server;
        private List<PC> listUser = new List<PC>();
        public List<string> listIP = new List<string>();
        List<Students> _students;
        List<StudentFromExcel> studentFromExcels;
        private Color ColorRed = Color.FromArgb(255, 95, 79);
        private Color ColorGreen = Color.FromArgb(54, 202, 56);

        int counter = 0;
        System.Timers.Timer countdown;
        public Form1(IServerService server)
        {
            _server = server;
            _server.EventActiveHandler += _server_EventActiveHandler;
         
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            countdown = new System.Timers.Timer();
            countdown.Interval = 1000;
            countdown.Elapsed += Countdown_Elapsed;

        }

   
        private void _server_EventActiveHandler(object sender, ServerService.ActiveEventArgs args)
        {
            UpdateUserControll(args.mssv);
        }

        private void Countdown_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            counter -= 1;
            int minute = counter / 60;
            int second = counter % 60;
            lblTimeleft.Text = minute + " : " + second;
            if (counter == 0)
            {
                countdown.Stop();
               
            }
        }
        private void UpdateUserControll(string mssv)
        {
            foreach(var items in listUser)
            {
                items.MSSV = mssv;
                items.ColorUser = ColorGreen;   
                         
             }
            LoadDisPlayUser();

        }
       

        private void Form1_Load(object sender, EventArgs e)
        {
            _server.Connect();
            
        }

        private void cmdBatDauLamBai_Click(object sender, EventArgs e)
        {
          counter =  _server.BeginExam(txtThoiGianLamBai.Text,this.counter,countdown);

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
        private void AddListUser(string clientIP)
        {     
                    int index = 0;    
                    index++;
                    PC pC = new PC();
                    pC.clientIP = clientIP;
                    pC.pcName = index.ToString();
                    pC.ColorUser = ColorRed;
                    listUser.Add(pC);
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

        private void cmdNhapVungIP_Click(object sender, EventArgs e)
        {
            FormSetIp formSetIp = new FormSetIp(listIP);
            formSetIp.EventUpdateHandler += FormSetIp_EventUpdateHandler;
            formSetIp.Show();
        }

        private void FormSetIp_EventUpdateHandler(object sender, FormSetIp.UpdateEventArgs args)
        {
           string IpClient = args.IPClient;
            _server.SetIpUser(args._listIP);    
            AddListUser(IpClient);
            LoadDisPlayUser();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            oFile.ShowDialog();
            if(oFile.FileName != string.Empty)
            {
                string file = oFile.FileName;
                string exetention = Path.GetExtension(file);
                if(exetention.ToLower() ==".xls" || exetention.ToLower().Equals(".xlsx"))
                {
                    var excel = new ExcelQueryFactory(file);
                    var students = from s in excel.Worksheet<StudentFromExcel>("Sheet1")
                                  select s;
                    studentFromExcels = new List<StudentFromExcel>();
                    foreach (var item in students)
                    {
                        studentFromExcels.Add(item);
                    }
                    if (studentFromExcels.Count > 0)
                    {
                        SendStudentsFromExcel(studentFromExcels);
                    }
                  


                }
            }
        }
        private void SendStudentsFromExcel(List<StudentFromExcel> StudentFromExcels)
        {
            _server.SendUserFromFile("Send UserFromExcel", StudentFromExcels);
        }
    }
}
