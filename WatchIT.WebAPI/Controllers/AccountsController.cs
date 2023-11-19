using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatchIT.Common;
using WatchIT.Common.Accounts.Request;
using WatchIT.Common.Accounts.Response;
using WatchIT.WebAPI.Services.AccountsService;

namespace WatchIT.WebAPI.Controllers
{
    [ApiController]
    [Route("accounts")]
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
        [Route("register")]
        [AllowAnonymous]
        public async Task<ApiResponse> Register([FromBody]RegisterRequest request)
        {
            return await _accountsService.Register(request);
        }

        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public async Task<ApiResponse<AuthenticateResponse>> Authenticate([FromBody]AuthenticateRequest request)
        {
            return await _accountsService.Authenticate(request);
        }

        [HttpPost]
        [Route("authenticate-refresh")]
        [Authorize(AuthenticationSchemes = "refresh")]
        public async Task<ApiResponse<AuthenticateResponse>> AuthenticateRefresh()
        {
            return await _accountsService.AuthenticateRefresh(User.Claims);
        }

        [HttpDelete]
        [Route("logout")]
        [Authorize(AuthenticationSchemes = "refresh")]
        public async Task<ApiResponse> Logout()
        {
            return await _accountsService.Logout(User.Claims);
        }

        [HttpDelete]
        [Route("logout-from-all-devices")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ApiResponse> LogoutFromAllDevices()
        {
            return await _accountsService.LogoutFromAllDevices(User.Claims);
        }

        #endregion
    }
}
