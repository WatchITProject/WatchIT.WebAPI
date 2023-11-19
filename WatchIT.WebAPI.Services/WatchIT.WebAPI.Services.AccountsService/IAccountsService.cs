using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WatchIT.Common;
using WatchIT.Common.Accounts.Request;
using WatchIT.Common.Accounts.Response;

namespace WatchIT.WebAPI.Services.AccountsService
{
    public interface IAccountsService
    {
        Task<ApiResponse> Register(RegisterRequest request);
        Task<ApiResponse<AuthenticateResponse>> Authenticate(AuthenticateRequest request);
        Task<ApiResponse<AuthenticateResponse>> AuthenticateRefresh(IEnumerable<Claim> claims);
        Task<ApiResponse> Logout(IEnumerable<Claim> claims);
        Task<ApiResponse> LogoutFromAllDevices(IEnumerable<Claim> claims);
    }
}
