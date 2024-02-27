using System.Net;
using SimpleBlog.Models.Interfaces;

namespace SimpleBlog.Models
{
    public class ErrorModel : IErrorModel
    {
        public ErrorModel()
        {
            StatusCode = HttpStatusCode.OK;
            Message = string.Empty;
        }

        public ErrorModel(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public HttpStatusCode StatusCode { get; private set; }

        public string Message { get; private set; }

        public void SetErrorInfo(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public bool StatusCodeIsOk()
        {
            return StatusCode == HttpStatusCode.OK;
        }

        public bool StatusCodeIsNotOk()
        {
            return !StatusCodeIsOk();
        }
    }
}
