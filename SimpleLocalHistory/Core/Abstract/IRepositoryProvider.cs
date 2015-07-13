using SimpleLocalHistory.Providers;
using System;
namespace SimpleLocalHistory.Core.Abstract
{
    public interface IRepositoryProvider
    {
        bool AddFileToRepository(string pathToFile, string comment = "");
        string GetPathToRepository();
        bool RemoveFileFromRepository(SimpleLocalHistory.Core.Abstract.FileInRepo file);
        SimpleLocalHistory.Core.Abstract.FileInRepo SearchFileInRepository(SimpleLocalHistory.Core.Abstract.ISearchProvider searchProvider, Query query);
    }
}
