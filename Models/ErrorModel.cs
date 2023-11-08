using System.Net;

namespace SimpleBlog.Models
{
    public class ErrorModel
    {
        private HttpStatusCode _statusCode;
        private string _message;

        public ErrorModel()
        {
            _statusCode = HttpStatusCode.OK;
            _message = string.Empty;
        }

        public ErrorModel(HttpStatusCode statusCode, string message)
        {
            _statusCode = statusCode;
            _message = message;
        }

        public HttpStatusCode StatusCode { get => _statusCode; set => _statusCode = value; }
        public string Message { get => _message; set => _message = value; }
    }
}
