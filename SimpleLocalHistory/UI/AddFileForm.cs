using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleLocalHistory.UI
{
    public partial class AddFileForm : Form
    {
        public AddFileForm()
        {
            InitializeComponent();
        }

        public AddFileForm(string pathToFile)
        {
            InitializeComponent();
            textBox1.Text = pathToFile;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Program.repoProvider.AddFileToRepository(textBox1.Text, textBox2.Text);
                this.Close();
            }
        }

        private void AddFileForm_Load(object sender, EventArgs e)
        {

        }
    }
}
