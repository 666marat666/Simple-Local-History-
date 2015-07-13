using SimpleLocalHistory.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleLocalHistory.Core.Abstract
{
    public class FileInRepo
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public int Version { get; set; }
        public string Comment { get; set; }

        [System.ComponentModel.Browsable(false)] //TODO
        public string NameInRepo { get; set; }
        [System.ComponentModel.Browsable(false)] //TODO hides column in data grid view, bad practice because has relation with ui
        public string FullPathInRepo { get; set; }

        public FileInRepo()
        {
            Name = "";
            FullPath = "";
            Version = 0;
            Comment = "";
            NameInRepo = "";
            FullPathInRepo = "";
        }

        public FileInRepo(string pathToFile, string commentToFile = "")
        {
            Name = Path.GetFileName(pathToFile);
            FullPath = pathToFile;
            Version = 0;
            Comment = commentToFile;
            NameInRepo = GetHashForFile() + Path.GetExtension(pathToFile);
        }

        string GetHashForFile()
        {
            return HashHelper.CalculateMD5Hash(Name + FullPath + Version.ToString());
        }

        public string GenerateUniqueNameForFile()
        {
            return GetHashForFile() + Path.GetExtension(FullPath);
        }

        public static FileInRepo GetPartialCopy(FileInRepo original)
        {
            FileInRepo result = new FileInRepo(original.FullPath);
            result.Version = original.Version;
            return result;
        }

        public FileInRepo GetFullCopy()
        {
            FileInRepo tmp = new FileInRepo();
            tmp.Name = this.Name;
            tmp.FullPath = this.FullPath;
            tmp.Comment = this.Comment;
            tmp.NameInRepo = this.NameInRepo;
            tmp.Version = this.Version;
            tmp.FullPathInRepo = this.FullPathInRepo;
            return tmp;
        }

        public void AssignToRepository(IRepositoryProvider repository)
        {
            this.FullPathInRepo = Path.Combine(repository.GetPathToRepository(), this.NameInRepo);
        }

        public override string ToString()
        {
            return String.Format("name: {0} version: {1} comment: {2} \nfullName: {3}", Name, Version.ToString(), Comment, FullPath);
        }
    }
}
