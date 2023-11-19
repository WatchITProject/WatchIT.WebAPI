using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WatchIT.Common;
using WatchIT.Common.Movies.Request;
using WatchIT.WebAPI.Services.Common.Attributes;
using WatchIT.WebAPI.Services.MoviesService;

namespace WatchIT.WebAPI.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MoviesController : ControllerBase
    {
        #region FIELDS

        private IMoviesService _moviesService;

        #endregion



        #region CONSTRUCTORS

        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        #endregion



        #region METHODS

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse<int>> AddMovie([FromBody] MoviePostPutRequest request)
        {
            return await _moviesService.AddMovie(request);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> ModifyMovie([FromRoute]int id, [FromBody]MoviePostPutRequest request)
        {
            return await _moviesService.ModifyMovie(id, request);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> DeleteMovie([FromRoute]int id)
        {
            return null;
        }

        [HttpPost]
        [Route("{movie_id}/genre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> AddGenre([FromRoute(Name = "movie_id")]int movieId, [FromQuery(Name = "genre_id")][BindRequired]int genreId)
        {
            return null;
        }

        [HttpDelete]
        [Route("{movie_id}/genre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> DeleteGenre([FromRoute(Name = "movie_id")]int movieId, [FromQuery(Name = "genre_id")][BindRequired]int genreId)
        {
            return null;
        }

        #endregion
    }
}
