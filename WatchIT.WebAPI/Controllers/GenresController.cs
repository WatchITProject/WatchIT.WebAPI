using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using WatchIT.Common.Movies.Request;
using WatchIT.Common;
using WatchIT.WebAPI.Services.Genres;
using WatchIT.WebAPI.Services.Movies;
using Microsoft.AspNetCore.Authorization;
using WatchIT.WebAPI.Attributes;
using WatchIT.Common.Genres.Request;
using WatchIT.Common.Genres.Response;

namespace WatchIT.WebAPI.Controllers
{
    [ApiController]
    [Route("genres")]
    public class GenresController : ControllerBase
    {
        #region FIELDS

        private IGenresService _genresService;

        #endregion



        #region CONSTRUCTORS

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        #endregion



        #region METHODS

        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<IEnumerable<GenreResponse>>> GetGenres()
        {
            return await _genresService.GetGenres();
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<ApiResponse<GenreResponse>> GetGenre([FromRoute]int id)
        {
            return await _genresService.GetGenre(id);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse<int>> AddGenre([FromBody]GenrePostPutRequest data)
        {
            return await _genresService.AddGenre(data);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> ModifyGenre([FromRoute]int id, [FromBody]GenrePostPutRequest data)
        {
            return await _genresService.ModifyGenre(id, data);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> DeleteGenre([FromRoute]int id)
        {
            return await _genresService.DeleteGenre(id);
        }

        #endregion
    }
}
