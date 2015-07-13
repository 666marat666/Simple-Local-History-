using Newtonsoft.Json;
using SimpleLocalHistory.Core.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleLocalHistory.Providers
{
    public class RepositoryProvider : IRepositoryProvider
    {
        string pathToRepo = "";
        ISettingsProvider settings;
        List<FileInRepo> repository;

        public string Name;
        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public event ChangedEventHandler OnChanged;

        public RepositoryProvider(ISettingsProvider settingsProvider)
        {
            settings = settingsProvider;
            var dir = Directory.CreateDirectory(settings.GetStrSettingByName("PathToRepository", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "repo")));
            pathToRepo = dir.FullName;
            Name = new DirectoryInfo(pathToRepo).Name;
            LoadRepositoryInfo(pathToRepo);
        }        

        public string GetPathToRepository()
        {
            return pathToRepo;
        }

        public void DeleteUnusedFiles()
        {
            throw new NotImplementedException();
        }

        public bool AddFileToRepository(string pathToFile, string comment = "")
        {
            try
            {
                FileInRepo file = GetFileIfExist(pathToFile);
                if (file == null)
                {
                    file = new FileInRepo(pathToFile, comment);
                    file.AssignToRepository(this);
                    repository.Add(file);
                    File.Copy(pathToFile, Path.Combine(pathToRepo, file.NameInRepo));

                    OnChanged(this, new EventArgs());

                    return true;
                }
                else
                {
                    FileInRepo newVersionOfFile = FileInRepo.GetPartialCopy(file);
                    newVersionOfFile.Version += 1;
                    newVersionOfFile.Comment = comment;
                    newVersionOfFile.NameInRepo = newVersionOfFile.GenerateUniqueNameForFile();
                    newVersionOfFile.AssignToRepository(this);
                    repository.Add(newVersionOfFile);
                    File.Copy(pathToFile, Path.Combine(pathToRepo, newVersionOfFile.NameInRepo));

                    OnChanged(this, new EventArgs());

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }



        public bool RemoveFileFromRepository(FileInRepo file)
        {
            File.Delete(Path.Combine(pathToRepo, file.NameInRepo));
            repository.Remove(file);
            OnChanged(this, new EventArgs());
            return true;
        }

        
        public FileInRepo SearchFileInRepository(ISearchProvider searchProvider, Query query)
        {
            return null;
        }

        public void Save()
        {
            SaveRepositoryInfo(pathToRepo);
        }

        #region Load And Save Repo
        void LoadRepositoryInfo(string pathToRepository)
        {
            if (File.Exists(Path.Combine(pathToRepository, settings.GetStrSettingByName("RepositoryInfoFileName", "repo.info"))))
            {
                //deserealize
                string json = File.ReadAllText(Path.Combine(pathToRepository, settings.GetStrSettingByName("RepositoryInfoFileName", "repo.info")));
                
                repository = JsonConvert.DeserializeObject<List<FileInRepo>>(json);
            }
            else
            {
                repository = new List<FileInRepo>();
            }
        }

        bool SaveRepositoryInfo(string pathToRepository)
        {
            if (File.Exists(Path.Combine(pathToRepository, settings.GetStrSettingByName("RepositoryInfoFileName", "repo.info"))))
            {
                File.Delete(Path.Combine(pathToRepository, settings.GetStrSettingByName("RepositoryInfoFileName", "repo.info")));
            }
            try
            {
                var json = JsonConvert.SerializeObject(repository, Formatting.Indented);
                File.WriteAllText(Path.Combine(pathToRepository, settings.GetStrSettingByName("RepositoryInfoFileName", "repo.info")),
                                    json);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Repository Helper
        FileInRepo GetFileIfExist(string pathToFile)
        {
            var a1 = repository.Where(x => x.FullPath == pathToFile).OrderBy(x => x.Version);
            return a1.LastOrDefault();
        }

        public void RepositoryRescan()
        {
            List<FileInRepo> filesForRemoving = new List<FileInRepo>();
            for(int i=0; i<repository.Count;i++)
            {
                if (!File.Exists(Path.Combine(pathToRepo, repository[i].NameInRepo)))
                {
                    filesForRemoving.Add(repository[i]);
                }
                else repository[i].AssignToRepository(this);
            }

            repository.RemoveAll(filesForRemoving.Contains);
            OnChanged(this, new EventArgs());
        }

        public string BackupRepository()
        { 
            return JsonConvert.SerializeObject(repository, Formatting.None);
        }

        public bool RestoreRepository(string json)
        {
            try
            {
                this.repository = JsonConvert.DeserializeObject<List<FileInRepo>>(json);
                OnChanged(this, new EventArgs());
                return true;
            }
            catch
            {
                OnChanged(this, new EventArgs());
                return false;
            }
        }
        #endregion

        #region Repository Basic Actions

        public List<FileInRepo> GetAllItems()
        {
            return repository;
        }

        public BindingList<FileInRepo> GetSimpleQueriedItemAsDataSource(string textForSearch)
        {
            textForSearch = textForSearch.ToLower();
            var list = new BindingList<FileInRepo>(repository.Where(x => x.Name.ToLower().Contains(textForSearch) || x.FullPath.ToLower().Contains(textForSearch) || x.Comment.ToLower().Contains(textForSearch)).ToList());
            return list;
        }

        public BindingList<FileInRepo> GetAllItemAsDataSource()
        {
            //List<FileInRepo> copyOfrepo = new List<FileInRepo>();

            //foreach (var file in repository)
            //{
            //    copyOfrepo.Add(file.GetCopyOfFile());
            //}

            var list = new BindingList<FileInRepo>(repository);
            return list;
        }

        public void RestoreFile(FileInRepo file)
        {
            File.Copy(Path.Combine(pathToRepo, file.NameInRepo), file.FullPath, true);
        }

        #endregion

        ~RepositoryProvider()
        {
            this.Save();
        }
    }
}
