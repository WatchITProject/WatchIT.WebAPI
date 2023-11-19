using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchIT.Common;
using WatchIT.Common.Accounts.Request;
using WatchIT.Common.Accounts.Response;
using WatchIT.Common.Movies.Request;
using WatchIT.WebAPI.Database;
using WatchIT.WebAPI.Services.Common;

namespace WatchIT.WebAPI.Services.MoviesService
{
    public class MoviesService : IMoviesService
    {
        #region FIELDS

        private DatabaseContext _database;

        #endregion



        #region CONSTRUCTORS

        public MoviesService(IDbContextFactory<DatabaseContext> database) 
        { 
            _database = database.CreateDbContext();
        }

        #endregion



        #region PUBLIC METHODS

        public async Task<ApiResponse<int>> AddMovie(MoviePostPutRequest request)
        {
            string? check = CheckMoviePostPutRequest(request);
            if (check != null) 
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = check
                };
            }

            Movie movie = new Movie();
            MoviePostPutPropertySet(movie, request);
            _database.Movies.Add(movie);

            await _database.SaveChangesAsync();

            return new ApiResponse<int>
            { 
                Data = movie.Id, 
                Success = true
            };
        }

        public async Task<ApiResponse> ModifyMovie(int id, MoviePostPutRequest request)
        {
            string? check = CheckMoviePostPutRequest(request);
            if (check != null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = check
                };
            }

            Movie? movie = _database.Movies.FirstOrDefault(x => x.Id == id);

            if (movie is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Movie with id {id} was not found"
                };
            }

            MoviePostPutPropertySet(movie, request);
            await _database.SaveChangesAsync();

            return new ApiResponse
            { 
                Success = true
            };
        }

        #endregion



        #region PRIVATE METHODS

        private void MoviePostPutPropertySet(Movie movie, MoviePostPutRequest request)
        {
            movie.Title = request.Title;
            movie.ReleaseDate = request.ReleaseDate.ToDateTime(new TimeOnly(0));
            movie.Length = request.Length;
            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                movie.Description = request.Description;
            }
            if (string.IsNullOrWhiteSpace(request.OriginalTitle))
            {
                movie.OriginalTitle = request.OriginalTitle;
            }
        }

        private string? CheckMoviePostPutRequest(MoviePostPutRequest request)
        {
            Check<MoviePostPutRequest>[] checks = new Check<MoviePostPutRequest>[]
            {
                new Check<MoviePostPutRequest>
                {
                    CheckAction = new Predicate<MoviePostPutRequest>((req) => string.IsNullOrWhiteSpace(req.Title)),
                    Message = "Title must be provided"
                },
                new Check<MoviePostPutRequest>
                {
                    CheckAction = new Predicate<MoviePostPutRequest>((req) => req.Length <= 0),
                    Message = "Length must be more than 0"
                },
            };

            foreach (Check<MoviePostPutRequest> check in checks)
            {
                if (check.CheckAction.Invoke(request))
                {
                    return check.Message;
                }
            }

            return null;
        }

        #endregion
    }
}
