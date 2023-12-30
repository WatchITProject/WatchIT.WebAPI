using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WatchIT.Common;
using WatchIT.Common.Movies.Request;
using WatchIT.Common.Movies.Response;
using WatchIT.WebAPI.Attributes;
using WatchIT.WebAPI.Services.Movies;

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

        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResponse<MovieResponse>> GetMovie([FromRoute]int id)
        {
            return await _moviesService.GetMovie(id);
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<MovieResponse>>> GetMovies()
        {
            return await _moviesService.GetMovies();
        }

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
            return await _moviesService.DeleteMovie(id);
        }

        [HttpPost]
        [Route("{movie_id}/genre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> AddGenre([FromRoute(Name = "movie_id")]int movieId, [FromQuery(Name = "genre_id")][BindRequired]int genreId)
        {
            return await _moviesService.AddGenre(movieId, genreId);
        }

        [HttpDelete]
        [Route("{movie_id}/genre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequiresClaim("admin", "True")]
        public async Task<ApiResponse> DeleteGenre([FromRoute(Name = "movie_id")]int movieId, [FromQuery(Name = "genre_id")][BindRequired]int genreId)
        {
            return await _moviesService.DeleteGenre(movieId, genreId);
        }

        #endregion
    }
}
