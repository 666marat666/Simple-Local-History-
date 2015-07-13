using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLocalHistory.Service.Core
{
    [Serializable]
    public enum ServiceStatus
    { 
        Success = 1,
        Error = 0
    }

    [Serializable]
    public class ServiceResult
    {        
        public ServiceStatus Status { get; set; }
        
        public string Message { get; set; }

        public ServiceResult(ServiceStatus status, string message = "")
        {
            Status = status;
            Message = message;
        }
    }
}
