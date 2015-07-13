
using LocalHistoryNPPPlugin;
using LocalHistoryNPPPlugin.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleLocalHistory.UI.Helpers
{
    public static class DiffEditorHelper
    {
        static string pathToDiffEditor;
        static DiffEditorHelper()
        { 
            //WinMergeU C:\Folder\File.txt C:\Folder2
            pathToDiffEditor = SettingsProvider.GetStrSettingByName("PathToDiffEditor", @"C:\Program Files (x86)\WinMerge\WinMergeU.exe");
        }

        public static void ShowDiffs(FileResult fileFromRepo)
        {
            if (File.Exists(pathToDiffEditor))
                System.Diagnostics.Process.Start(pathToDiffEditor, fileFromRepo.FullPath + " " + fileFromRepo.FullPathInRepo);
            else
                throw new Exception("WinMerge doesn't exist or you need to specify path in PathToDiffEditor setting");
        }
    }
}
