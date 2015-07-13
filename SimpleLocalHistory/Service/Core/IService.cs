using SimpleLocalHistory.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SimpleLocalHistory.Service.Core
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json)]
        string GetAllVersionsOfFile(string filePath);

        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json)]
        string GetLastVersionOfFile(string filePath);

        [OperationContract]
        [WebInvoke(Method = "GET",//POST
                    ResponseFormat = WebMessageFormat.Json)]
        string RestoreLastVersionOfFile(string filePath);

        [OperationContract]
        [WebInvoke(Method = "GET",//POST
                    ResponseFormat = WebMessageFormat.Json)]
        string RestoreSpecificVersionOfItem(string filePath, int version);

        [OperationContract]
        [WebInvoke(Method = "GET",//POST
                    ResponseFormat = WebMessageFormat.Json)]
        string RemoveAllVersionsOfFile(string filePath);

        [OperationContract]
        [WebInvoke(Method = "GET",//POST
                    ResponseFormat = WebMessageFormat.Json)]
        string AddFile(string filePath, string comment = "new version");

    }
}
