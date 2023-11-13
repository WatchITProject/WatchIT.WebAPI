using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchIT.WebAPI.Services.AccountsService;
using WatchIT.WebAPI.Services.AccountsService.Request;
using WatchIT.WebAPI.Services.AccountsService.Response;

namespace WatchIT.WebAPI.Controllers
{
    [ApiController]
    [Route("Accounts")]
    public class AccountsController : ControllerBase
    {
        #region FIELDS

        private IAccountsService _accountsService;

        #endregion



        #region CONSTRUCTORS

        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        #endregion



        #region METHODS

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public void Register([FromBody]RegisterRequest request)
        {
            _accountsService.Register(request);
        }

        [HttpGet]
        [Route("Authenticate")]
        [AllowAnonymous]
        public AuthenticateResponse Authenticate(string email_or_username, string password)
        {
            return _accountsService.Authenticate(email_or_username, password);
        }

        #endregion
    }
}
