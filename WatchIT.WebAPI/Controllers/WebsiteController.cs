using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchIT.Common;
using WatchIT.Common.Website.AuthBackground.Request;
using WatchIT.Common.Website.AuthBackground.Response;
using WatchIT.WebAPI.Attributes;
using WatchIT.WebAPI.Services.Website;

namespace WatchIT.WebAPI.Controllers
{
    [ApiController]
    [Route("website")]
    public class WebsiteController : ControllerBase
    {
        #region FIELDS

        private IWebsiteAuthBackgroundService _authBackgroundService;

        #endregion



        #region CONSTRUCTORS

        public WebsiteController(IWebsiteAuthBackgroundService authBackgroundService)
        {
            _authBackgroundService = authBackgroundService;
        }

        #endregion



        #region METHODS

        [HttpGet]
        [Route("auth-background/random")]
        [AllowAnonymous]
        public async Task<ApiResponse<AuthBackgroundResponse>> GetRandomAuthBackground()
        {
            return await _authBackgroundService.GetRandomAuthBackground();
        }

        [HttpGet]
        [Route("auth-background/{auth_background_id}")]
        [AllowAnonymous]
        public async Task<ApiResponse<AuthBackgroundResponse>> GetAuthBackground([FromRoute(Name = "auth_background_id")] short id)
        {
            return await _authBackgroundService.GetAuthBackground(id);
        }

        [HttpGet]
        [Route("auth-background")]
        [AllowAnonymous]
        public ApiResponse<IEnumerable<AuthBackgroundResponse>> GetAuthBackgrounds()
        {
            return _authBackgroundService.GetAuthBackgrounds();
        }

        [HttpPost]
        [Route("auth-background")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse<short>> AddAuthBackground([FromBody]AuthBackgroundPostPutRequest data)
        {
            return await _authBackgroundService.AddAuthBackground(data);
        }

        [HttpPut]
        [Route("auth-background/{auth_background_id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> ModifyAuthBackground([FromRoute(Name = "auth_background_id")] short id, [FromBody]AuthBackgroundPostPutRequest data)
        {
            return await _authBackgroundService.ModifyAuthBackground(id, data);
        }

        [HttpDelete]
        [Route("auth-background/{auth_background_id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> DeleteAuthBackground([FromRoute(Name = "auth_background_id")] short id)
        {
            return await _authBackgroundService.DeleteAuthBackground(id);
        }

        #endregion
    }
}
