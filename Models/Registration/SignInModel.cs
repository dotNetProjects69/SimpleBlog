using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SimpleBlog.Models.Registration
{
    public class SignInModel
    {
        private string _nickName;
        private string _email;
        private string _password;
        private HttpStatusCode _statusCode;

        public SignInModel()
        {
            _nickName = string.Empty;
            _email = string.Empty;
            _password = string.Empty;
            _statusCode = HttpStatusCode.OK;
        }

        public string NickName { get => _nickName; set => _nickName = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
        public HttpStatusCode StatusCode { get => _statusCode; set => _statusCode = value; }
    }
}
