using Newtonsoft.Json;
using SimpleLocalHistory.Core.Abstract;
using SimpleLocalHistory.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace SimpleLocalHistory.Service.Implementation
{
    public class RepositoryService : IService
    {
        public string GetAllVersionsOfFile(string filePath)
        {
            filePath = HttpUtility.UrlDecode(filePath);
            return JsonConvert.SerializeObject(
                Program.repoProvider.GetAllItems().Where(x => x.FullPath == filePath).ToList()
                );
        }

        public string RestoreLastVersionOfFile(string filePath)
        {
            filePath = HttpUtility.UrlDecode(filePath);
            try
            {
                Program.repoProvider.RestoreFile(Program.repoProvider.GetAllItems().Where(x => x.FullPath == filePath).LastOrDefault());
                return JsonConvert.SerializeObject(new ServiceResult(ServiceStatus.Success, "File restored."));
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new ServiceResult(ServiceStatus.Error, ex.Message));
            }
        }

        public string RemoveAllVersionsOfFile(string filePath)
        {
            filePath = HttpUtility.UrlDecode(filePath);
            try
            {
                var files = Program.repoProvider.GetAllItems().Where(x => x.FullPath == filePath).ToList();
                foreach (var file in files)
                {
                    Program.repoProvider.RemoveFileFromRepository(file);
                }
                return JsonConvert.SerializeObject(new ServiceResult(ServiceStatus.Success, "All files was deleted."));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ServiceResult(ServiceStatus.Error, ex.Message));
            }
        }

        public string AddFile(string filePath, string comment = "new version")
        {
            try
            {
                Program.repoProvider.AddFileToRepository(filePath, comment);
                return JsonConvert.SerializeObject(new ServiceResult(ServiceStatus.Success, "File has been added."));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ServiceResult(ServiceStatus.Error, ex.Message));
            }
        }


        public string GetLastVersionOfFile(string filePath)
        {
            filePath = HttpUtility.UrlDecode(filePath);
            return JsonConvert.SerializeObject(
                Program.repoProvider.GetAllItems().Where(x => x.FullPath == filePath).ToList()
                );
        }


        public string RestoreSpecificVersionOfItem(string filePath, int version)
        {
            filePath = HttpUtility.UrlDecode(filePath);
            try
            {
                var file = Program.repoProvider.GetAllItems().Where(x => x.FullPath == filePath && x.Version == version).FirstOrDefault();
                Program.repoProvider.RestoreFile(file);
                return JsonConvert.SerializeObject(new ServiceResult(ServiceStatus.Success, "File restored."));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ServiceResult(ServiceStatus.Error, ex.Message));
            }

        }
    }
}
