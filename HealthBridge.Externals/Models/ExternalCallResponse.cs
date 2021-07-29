using System;
using System.Collections.Generic;
using System.Text;

namespace HealthBridge.External.Service.Models
{
    public class ExternalCallResponse<T>
    {
        public ExternalCallResponse()
        {

        }
        public ExternalCallResponse(T responseData,string errorMessage,bool success)
        {
            Response = responseData;
            Success = success;
            ErrorMessage = errorMessage;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public T Response { get; set; }
    }
}
