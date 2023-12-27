using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchIT.Common;
using WatchIT.Common.Genres.Request;
using WatchIT.Common.Genres.Response;
using WatchIT.Common.Movies.Request;
using WatchIT.WebAPI.Database;
using WatchIT.WebAPI.Services;

namespace WatchIT.WebAPI.Services.Genres
{
    public interface IGenresService
    {
        Task<ApiResponse<IEnumerable<GenreResponse>>> GetGenres();
        Task<ApiResponse<GenreResponse>> GetGenre(int id);
        Task<ApiResponse<int>> AddGenre(GenrePostPutRequest data);
        Task<ApiResponse> ModifyGenre(int id, GenrePostPutRequest data);
        Task<ApiResponse> DeleteGenre(int id);
    }



    public class GenresService : IGenresService
    {
        #region FIELDS

        private DatabaseContext _database;

        #endregion



        #region CONSTRUCTORS

        public GenresService(IDbContextFactory<DatabaseContext> database)
        {
            _database = database.CreateDbContext();
        }

        #endregion



        #region PUBLIC METHODS

        public async Task<ApiResponse<IEnumerable<GenreResponse>>> GetGenres()
        {
            List<GenreResponse> genres = new List<GenreResponse>();
            foreach (Genre genre in _database.Genre)
            {
                genres.Add(new GenreResponse
                {
                    Id = genre.Id,
                    Name = genre.Name,
                    Description = genre.Description,
                });
            }
            return new ApiResponse<IEnumerable<GenreResponse>>
            {
                Success = true,
                Data = genres
            };
        }

        public async Task<ApiResponse<GenreResponse>> GetGenre(int id)
        {
            Genre? genre = await _database.Genre.FirstOrDefaultAsync(x => x.Id == id);

            if (genre is null)
            {
                return new ApiResponse<GenreResponse>
                {
                    Success = false,
                    Message = $"Genre with id {id} was not found"
                };
            }

            return new ApiResponse<GenreResponse>
            {
                Success = true,
                Data = new GenreResponse
                {
                    Id = genre.Id,
                    Name = genre.Name,
                    Description = genre.Description
                }
            };
        }

        public async Task<ApiResponse<int>> AddGenre(GenrePostPutRequest data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = "Name must be provided"
                };
            }

            Genre genre = new Genre();
            GenrePostPutPropertySet(genre, data);
            _database.Genre.Add(genre);

            await _database.SaveChangesAsync();

            return new ApiResponse<int>
            {
                Data = genre.Id,
                Success = true
            };
        }

        public async Task<ApiResponse> ModifyGenre(int id, GenrePostPutRequest data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Name must be provided"
                };
            }

            Genre? genre = _database.Genre.FirstOrDefault(x => x.Id == id);

            if (genre is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Genre with id {id} was not found"
                };
            }

            GenrePostPutPropertySet(genre, data);
            await _database.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true
            };
        }

        public async Task<ApiResponse> DeleteGenre(int id)
        {
            Genre? genre = _database.Genre.FirstOrDefault(x => x.Id == id);

            if (genre is null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Genre with id {id} was not found"
                };
            }

            _database.Genre.Attach(genre);
            _database.Genre.Remove(genre);

            await _database.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true
            };
        }

        #endregion



        #region PRIVATE METHODS

        private void GenrePostPutPropertySet(Genre genre, GenrePostPutRequest data)
        {
            genre.Name = data.Name;
            if (!string.IsNullOrWhiteSpace(data.Description))
            {
                genre.Description = data.Description;
            }
        }

        #endregion
    }
}
