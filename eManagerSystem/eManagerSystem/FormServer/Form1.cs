using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using eManagerSystem.Application.Catalog.Server;
namespace FormServer
{
    public partial class Form1 : Form
    {
        ServerService server = new ServerService();
        public Form1()
        {
           
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
          
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server.Connect();
        }

        private void cmdBatDauLamBai_Click(object sender, EventArgs e)
        {
            server.Send(tbMessage.Text);
            lsvMessage.Items.Add(new ListViewItem() { Text = server.GetMessage() });
            
         
        }
    }
}
