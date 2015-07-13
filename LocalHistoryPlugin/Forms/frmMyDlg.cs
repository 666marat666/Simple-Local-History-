using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LocalHistoryPlugin
{
    public partial class frmMyDlg : Form
    {
        public frmMyDlg()
        {
            InitializeComponent();
        }

        private void frmMyDlg_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show(
                JsonConvert.SerializeObject(new Dictionary<string, string>() { { "test", "testVal" } })
                );
        }
    }
}
