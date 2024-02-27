using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Authentication;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Shared;
using SimpleBlog.Validators.Base;
using SimpleBlog.Validators.ValidatorType;
using System.Net;
using System.Runtime.CompilerServices;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;
using static SimpleBlog.Controllers.Extensions.Sql.TemplateBuilder;
using static SimpleBlog.Shared.GlobalParams;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Controllers
{
    public class SignUpController : Controller
    {

        private readonly ILogger<SignUpController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHandler _sessionHandler;
        private readonly string _signUpPagePath;

        public SignUpController(ILogger<SignUpController> logger,
                                IConfiguration configuration,
                                ISessionHandler? sessionHandler = null)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHandler = sessionHandler ?? new SessionHandler();
            _signUpPagePath = "/Views/Authentication/SignUp.cshtml";

        }

        public IActionResult Index(SignUpModel? model = null)
        {
            model ??= new();
			return View(_signUpPagePath, model);
		}

        public IActionResult Register(SignUpModel? model)
        {
            //TODO Test it!
            model ??= new ();
            model.Error = CheckAndSetError(model);
            model.Surname ??= string.Empty;
            if (model.Error.StatusCode != HttpStatusCode.OK)
                return Index(model);
            SetNewGuid(model);
            AddNewAccount(model);
            AddAccountTable(model);
            SetAccountId(model);

            return RedirectToAction("Index", "Posts");
        }

        private IErrorModel CheckAndSetError(SignUpModel model)
        {
            ValidationChain<IAccountModelPart> chain = new();
            chain
                .SetNext(new ModelHasNoBlankFields())
                .SetNext(new NicknameMustNotUsed())
                .SetNext(new NicknameCharsIsCorrect())
                .SetNext(new EmailMustNotExist())
                .SetNext(new PasswordConfirmIsCorrectly())
                .SetNext(new PasswordLengthEnough())
                .SetNext(new PasswordHasNoGaps());

            return chain.Validate(model);
        }

        private void AddAccountTable(SignUpModel model)
        {
            string template =
                "(Id  integer primary key autoincrement," +
                "Title text, Body text," +
                "CreatedAt text," +
                "UpdatedAt text)";
            CreateTable(model.UserId.ToString(),
                template, 
                GetAccountsDataPath());
        }

        private void AddNewAccount(SignUpModel model)
        {
            string tableName = "AuthData";
            string template = 
                CreateTemplate("Name", "Surname", "DateOfBirth",
                               "Email", "Password", "UserID", "NickName");
            string values = 
                CreateValuesForm(model.Name,
                                 model.Surname ?? string.Empty,
                                 model.DateOfBirth.ToString(),
                                 model.Email,
                                 model.Password,
                                 model.UserId.ToString(),
                                 model.Nickname);
            AddAccount(tableName, template, values, GetAccountsDataPath());
        }

        private static void SetNewGuid(SignUpModel model)
        {
            model.UserId = Guid.NewGuid();
        }

        private protected virtual void SetAccountId(SignUpModel model)
        {
            _sessionHandler.SessionOwnerId = model.UserId.ToString();
        }
    }
}
