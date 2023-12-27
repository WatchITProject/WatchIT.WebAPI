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
using WatchIT.WebAPI.Services;

namespace WatchIT.WebAPI.Services.Movies
{
    public interface IMoviesService
    {
        Task<ApiResponse<int>> AddMovie(MoviePostPutRequest request);
        Task<ApiResponse> ModifyMovie(int id, MoviePostPutRequest request);
        Task<ApiResponse> DeleteMovie(int id);
        Task<ApiResponse> AddGenre(int movieId, int genreId);
        Task<ApiResponse> DeleteGenre(int movieId, int genreId);
    }



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
            if (check is not null) 
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = check
                };
            }

            Media media = new Media
            {
                Title = request.Title,
                OriginalTitle = request.OriginalTitle,
                Description = request.Description,
                Length = request.Length,
                ReleaseDate = request.ReleaseDate.ToDateTime(new TimeOnly(0))
            };
            await _database.Media.AddAsync(media);
            await _database.SaveChangesAsync();

            MediaMovie movie = new MediaMovie
            {
                MediaId = media.Id
            };
            await _database.MediaMovie.AddAsync(movie);
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

            MediaMovie? movie = await _database.MediaMovie.FirstOrDefaultAsync(x => x.Id == id);

            if (movie is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Movie with id {id} was not found"
                };
            }

            Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == movie.MediaId);

            if (media is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Movie with id {id} was found, but linked media with id {movie.MediaId} was not found"
                };
            }

            media.Title = request.Title;
            media.OriginalTitle = request.OriginalTitle;
            media.Description = request.Description;
            media.Length = request.Length;
            media.ReleaseDate = request.ReleaseDate.ToDateTime(new TimeOnly(0));

            await _database.SaveChangesAsync();

            return new ApiResponse
            { 
                Success = true
            };
        }

        public async Task<ApiResponse> DeleteMovie(int id)
        {
            MediaMovie? movie = await _database.MediaMovie.FirstOrDefaultAsync(x => x.Id == id);

            if (movie is null)
            {
                return new ApiResponse
                {
                    Success = true,
                    Message = $"Movie with id {id} was not found"
                };
            }

            Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == movie.MediaId);

            if (media is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Movie with id {id} was found, but linked media with id {movie.MediaId} was not found"
                };
            }

            IEnumerable<GenreMedia> genreMedia = _database.GenreMedia.Where(x => x.MediaId == media.Id);

            _database.GenreMedia.AttachRange(genreMedia);
            _database.GenreMedia.RemoveRange(genreMedia);

            await _database.SaveChangesAsync();

            _database.Media.Attach(media);
            _database.Media.Remove(media);

            await _database.SaveChangesAsync();

            _database.MediaMovie.Attach(movie);
            _database.MediaMovie.Remove(movie);

            await _database.SaveChangesAsync();

            return new ApiResponse
            { 
                Success = true 
            };
        }

        public async Task<ApiResponse> AddGenre(int movieId, int genreId)
        {
            MediaMovie? movie = await _database.MediaMovie.FirstOrDefaultAsync(x => x.Id == movieId);

            if (movie is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Movie with id {movieId} was not found"
                };
            }

            Task<Media?> mediaTask = _database.Media.FirstOrDefaultAsync(x => x.Id == movie.MediaId);
            Task<Genre?> genreTask = _database.Genre.FirstOrDefaultAsync(x => x.Id == genreId);

            await Task.WhenAll(mediaTask, genreTask);

            Media? media = mediaTask.Result;
            Genre? genre = genreTask.Result;

            if (media is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Movie with id {movieId} was found, but linked media with id {movie.MediaId} was not found"
                };
            }

            if (genre is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Genre with id {genreId} was not found"
                };
            }

            GenreMedia? genreMedia = await _database.GenreMedia.FirstOrDefaultAsync(x => x.MediaId == media.Id && x.GenreId == genreId);

            if (genreMedia is not null)
            {
                return new ApiResponse
                {
                    Success = true,
                    Message = $"Genre with id {genreId} is already added to movie with id {movieId} (media: {media.Id})"
                };
            }

            genreMedia = new GenreMedia
            {
                GenreId = genreId,
                MediaId = media.Id,
            };
            await _database.GenreMedia.AddAsync(genreMedia);
            await _database.SaveChangesAsync();

            return new ApiResponse 
            { 
                Success = true
            };
        }

        public async Task<ApiResponse> DeleteGenre(int movieId, int genreId)
        {
            MediaMovie? movie = await _database.MediaMovie.FirstOrDefaultAsync(x => x.Id == movieId);

            if (movie is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Movie with id {movieId} was not found"
                };
            }

            GenreMedia? genreMedia = await _database.GenreMedia.FirstOrDefaultAsync(x => x.MediaId == movie.MediaId && x.GenreId == genreId);

            if (genreMedia is null)
            {
                return new ApiResponse
                {
                    Success = true,
                    Message = $"There is no genre with id {genreId} linked to movie with id {movieId} (media: {movie.MediaId})"
                };
            }

            _database.GenreMedia.Attach(genreMedia);
            _database.GenreMedia.Remove(genreMedia);

            await _database.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true
            };
        }

        #endregion



        #region PRIVATE METHODS

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
