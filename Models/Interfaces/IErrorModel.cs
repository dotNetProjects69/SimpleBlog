using System.Net;

namespace SimpleBlog.Models.Interfaces
{
    public interface IErrorModel
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public void SetErrorInfo(HttpStatusCode statusCode, string message);
        public bool StatusCodeIsOk();
        public bool StatusCodeIsNotOk();
    }
}