using LocalHistoryNPPPlugin.Providers;
using LocalHistoryNPPPlugin.Service.Core;
using Newtonsoft.Json;
using NppPluginNET;
using SimpleLocalHistory.UI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Windows.Forms;

namespace LocalHistoryNPPPlugin
{
    public partial class frmMyDlg : Form
    {
        public frmMyDlg()
        {
            InitializeComponent();
            
            
            using (ChannelFactory<IService> cf = new ChannelFactory<IService>(new WebHttpBinding(), SettingsProvider.GetStrSettingByName("ServiceUri", @"http://localhost:8181")))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                IService channel = cf.CreateChannel();
                if (
                    JsonConvert.DeserializeObject<List<FileResult>>(
                    channel.GetAllVersionsOfFile(PluginBase.GetCurrentPath())
                    ) == null ||
                    JsonConvert.DeserializeObject<List<FileResult>>(
                    channel.GetAllVersionsOfFile(PluginBase.GetCurrentPath())
                    ).Count == 0
                    )
                {
                    ServiceResult result = (ServiceResult)channel.AddFile(PluginBase.GetCurrentPath(), "original");

                    if (result.Status == ServiceStatus.Error)
                        MessageBox.Show(result.Message);
                }
            }

            RefreshDataInDataGrid(dataGridView1);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);            
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void commitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ChannelFactory<IService> cf = new ChannelFactory<IService>(new WebHttpBinding(), SettingsProvider.GetStrSettingByName("ServiceUri", @"http://localhost:8181")))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                IService channel = cf.CreateChannel();
                string comment = Microsoft.VisualBasic.Interaction.InputBox("Pleae specify comment:", "Commit", "new version");
                ServiceResult result = (ServiceResult)channel.AddFile(PluginBase.GetCurrentPath(), comment);
                
                if(result.Status == ServiceStatus.Error)
                    MessageBox.Show(result.Message);
            }

            RefreshDataInDataGrid(dataGridView1);
        }

        private void getAllVersionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataInDataGrid(dataGridView1);
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ChannelFactory<IService> cf = new ChannelFactory<IService>(new WebHttpBinding(), SettingsProvider.GetStrSettingByName("ServiceUri", @"http://localhost:8181")))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                IService channel = cf.CreateChannel();

                var file = GetSelectedFileInDataGrid(dataGridView1);


                ServiceResult result = (ServiceResult)channel.RestoreSpecificVersionOfItem(PluginBase.GetCurrentPath(), file.Version);

                if (result.Status == ServiceStatus.Error)
                    MessageBox.Show(result.Message);

                PluginBase.RefreshWindow(PluginBase.GetCurrentPath());//TODO: todo
            }
        }

        #region Help Methods
        public FileResult GetSelectedFileInDataGrid(DataGridView dataGridView)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows.Count < 2)
            {
                var file = dataGridView1.SelectedRows[0].DataBoundItem as FileResult;
                return file;
            }
            else if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedCells.Count < 2)
            {
                var file = dataGridView1.SelectedCells[0].OwningRow.DataBoundItem as FileResult;
                return file;
            }
            return null;
        }

        public void RefreshDataInDataGrid(DataGridView dataGridView)
        {
            List<FileResult> results;
            using (ChannelFactory<IService> cf = new ChannelFactory<IService>(new WebHttpBinding(), SettingsProvider.GetStrSettingByName("ServiceUri", @"http://localhost:8181")))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                IService channel = cf.CreateChannel();
                results = JsonConvert.DeserializeObject<List<FileResult>>(
                    channel.GetAllVersionsOfFile(PluginBase.GetCurrentPath())
                    );
                if (results != null && results.Count > 0)
                {
                    dataGridView.DataSource = new BindingList<FileResult>(results);
                }
                else dataGridView.DataSource = null;
            }
        }
        #endregion

        private void showDiffsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var file = GetSelectedFileInDataGrid(dataGridView1);
            if (file != null)
                DiffEditorHelper.ShowDiffs(file);
            else MessageBox.Show("Please select file in repository.");
        }

        private void removeAllVersionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ChannelFactory<IService> cf = new ChannelFactory<IService>(new WebHttpBinding(), SettingsProvider.GetStrSettingByName("ServiceUri", @"http://localhost:8181")))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                IService channel = cf.CreateChannel();

                var file = GetSelectedFileInDataGrid(dataGridView1);


                ServiceResult result = (ServiceResult)channel.RemoveAllVersionsOfFile(file.FullPath);

                if (result.Status == ServiceStatus.Error)
                    MessageBox.Show(result.Message);

                RefreshDataInDataGrid(dataGridView1);
            }
        }
    }

    public class FileResult
    {
        public string Name { get; set; }
        public int Version { get; set; }
        public string Comment { get; set; }

        [System.ComponentModel.Browsable(false)] //TODO
        public string FullPath { get; set; }
        [System.ComponentModel.Browsable(false)] //TODO
        public string NameInRepo { get; set; }
        [System.ComponentModel.Browsable(false)] //TODO hides column in data grid view, bad practice because has relation with ui
        public string FullPathInRepo { get; set; }


        public FileResult()
        {           
        }
    }
}
