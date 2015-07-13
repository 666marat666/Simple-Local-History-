using SimpleLocalHistory.Core.Abstract;
using SimpleLocalHistory.Providers;
using SimpleLocalHistory.UI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleLocalHistory.UI
{
    public partial class MainForm : Form
    {
          
        public MainForm()
        {
            InitializeComponent();
            Program.repoProvider.OnChanged += repoProvider_OnChanged;
        }

        void repoProvider_OnChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(RefreshData));
            }
            else
                RefreshData();
        }

        private void addTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.repoProvider.AddFileToRepository(Clipboard.GetText( TextDataFormat.Text ), "new version");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Program.repoProvider.GetAllItemAsDataSource();
        }

        void RefreshData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Program.repoProvider.GetAllItemAsDataSource();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInRepo selectedFile = GetSelectedFileInDataGrid(dataGridView1);
            if (selectedFile != null)
                Program.repoProvider.RestoreFile(selectedFile);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Program.repoProvider.GetSimpleQueriedItemAsDataSource(textBox1.Text);
        }

        private void fromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFileForm form = new AddFileForm();
            form.ShowDialog(this);
            RefreshData();
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (FileList.Count() > 0)
            {
                AddFileForm form = new AddFileForm(FileList[0]);
                form.ShowDialog(this);
                RefreshData();
            }
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInRepo selectedFile = GetSelectedFileInDataGrid(dataGridView1);
            if (selectedFile != null)
                Program.repoProvider.RemoveFileFromRepository(selectedFile);
            RefreshData();
        }

        private void showDiffsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiffEditorHelper diffHelper = new DiffEditorHelper(SimpleLocalHistory.Program.settings);
            FileInRepo selectedFile = GetSelectedFileInDataGrid(dataGridView1);
            if (selectedFile != null)
                diffHelper.ShowDiffs(selectedFile, Program.repoProvider.GetPathToRepository());

        }

        #region Help Methods
        public FileInRepo GetSelectedFileInDataGrid(DataGridView dataGridView)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows.Count < 2)
            {
                var file = dataGridView1.SelectedRows[0].DataBoundItem as FileInRepo;
                return file;
            }
            else if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedCells.Count < 2)
            {
                var file = dataGridView1.SelectedCells[0].OwningRow.DataBoundItem as FileInRepo;
                return file;
            }
            return null;
        }
        #endregion

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            RefreshData();
        }

        private void rescanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.repoProvider.RepositoryRescan();
        }

        private void backupRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.settings.AddSetting(Program.repoProvider.Name, Program.repoProvider.BackupRepository());
        }

        private void restoreRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.repoProvider.RestoreRepository(Program.settings.GetStrSettingByName(Program.repoProvider.Name, ""));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
