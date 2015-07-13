using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalHistoryNPPPlugin.Service.Core
{
    [Serializable]
    public enum ServiceStatus
    { 
        Success = 1,
        Error = 0,
        None = -1
    }

    [Serializable]
    public class ServiceResult
    {        
        public ServiceStatus Status { get; set; }
        
        public string Message { get; set; }

        public ServiceResult()
        {
            Status = ServiceStatus.None;
            Message = "";
        }

        public ServiceResult(ServiceStatus status, string message = "")
        {
            Status = status;
            Message = message;
        }

        //public static explicit operator ServiceResult(string jsonString)
        //{
        //    ServiceResult result = new ServiceResult(ServiceStatus.Error, "");
        //    checked
        //    {
        //        result = JsonConvert.DeserializeObject<ServiceResult>(jsonString);
        //    }
        //    return result;
        //}

        public static implicit operator ServiceResult(string jsonString)
        {
            ServiceResult result = new ServiceResult(ServiceStatus.Error, "");
            checked
            {
                result = JsonConvert.DeserializeObject<ServiceResult>(jsonString);
            }
            return result;
        } 
    }
}
