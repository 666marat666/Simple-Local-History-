using SimpleLocalHistory.Core.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleLocalHistory.UI.Helpers
{
    public class DiffEditorHelper
    {
        string pathToDiffEditor;
        public DiffEditorHelper(ISettingsProvider settings)
        { 
            //WinMergeU C:\Folder\File.txt C:\Folder2
            pathToDiffEditor = settings.GetStrSettingByName("PathToDiffEditor", @"C:\Program Files (x86)\WinMerge\WinMergeU.exe");
        }

        public void ShowDiffs(FileInRepo fileFromRepo, string pathToRepo)
        {
            System.Diagnostics.Process.Start(pathToDiffEditor, fileFromRepo.FullPath + " " + Path.Combine(pathToRepo,fileFromRepo.NameInRepo));
        }
    }
}
