using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchIT.Common;
using WatchIT.Common.Website.AuthBackground.Request;
using WatchIT.Common.Website.AuthBackground.Response;
using WatchIT.WebAPI.Database;

namespace WatchIT.WebAPI.Services.Website
{
    public interface IWebsiteAuthBackgroundService
    {
        Task<ApiResponse<short>> AddAuthBackground(AuthBackgroundPostPutRequest data);
        Task<ApiResponse> ModifyAuthBackground(short id, AuthBackgroundPostPutRequest data);
        Task<ApiResponse> DeleteAuthBackground(short id);
        Task<ApiResponse<AuthBackgroundResponse>> GetRandomAuthBackground();
        Task<ApiResponse<AuthBackgroundResponse>> GetAuthBackground(short id);
        ApiResponse<IEnumerable<AuthBackgroundResponse>> GetAuthBackgrounds();
    }



    public class WebsiteAuthBackgroundService : IWebsiteAuthBackgroundService
    {
        #region FIELDS

        private DatabaseContext _database;

        #endregion



        #region CONSTRUCTORS

        public WebsiteAuthBackgroundService(IDbContextFactory<DatabaseContext> database)
        {
            _database = database.CreateDbContext();
        }

        #endregion



        #region METHODS

        public async Task<ApiResponse<short>> AddAuthBackground(AuthBackgroundPostPutRequest data)
        {
            AuthBackgroundImage image = new AuthBackgroundImage
            {
                Description = data.Description,
                Image = data.Image,
                ContentType = data.ContentType,
            };

            await _database.AuthBackgroundImage.AddAsync(image);
            await _database.SaveChangesAsync();

            return new ApiResponse<short> 
            { 
                Data = image.Id,
                Success = true 
            };
        }

        public async Task<ApiResponse> ModifyAuthBackground(short id, AuthBackgroundPostPutRequest data)
        {
            AuthBackgroundImage? image = await _database.AuthBackgroundImage.FirstOrDefaultAsync(x => x.Id == id);

            if (image is null)
            {
                return new ApiResponse
                {
                    Message = $"Auth background image with id {id} does not exist",
                    Success = false
                };
            }

            image.Description = data.Description;
            image.Image = data.Image;
            image.ContentType = data.ContentType;

            await _database.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true
            };
        }

        public async Task<ApiResponse> DeleteAuthBackground(short id)
        {
            AuthBackgroundImage? image = await _database.AuthBackgroundImage.FirstOrDefaultAsync(x => x.Id == id);

            if (image is null)
            {
                return new ApiResponse
                {
                    Message = $"Auth background image with id {id} already does not exist",
                    Success = true
                };
            }

            _database.AuthBackgroundImage.Attach(image);
            _database.AuthBackgroundImage.Remove(image);
            await _database.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true
            };
        }

        public async Task<ApiResponse<AuthBackgroundResponse>> GetRandomAuthBackground()
        {
            IEnumerable<short> ids = _database.AuthBackgroundImage.Select(x => x.Id);

            if (!ids.Any())
            {
                return new ApiResponse<AuthBackgroundResponse>
                {
                    Message = $"AuthBackground list is empty",
                    Success = false
                };
            }

            int index = Random.Shared.Next(ids.Count());

            return await GetAuthBackground(ids.ElementAt(index));
        }

        public async Task<ApiResponse<AuthBackgroundResponse>> GetAuthBackground(short id)
        {
            AuthBackgroundImage? image = await _database.AuthBackgroundImage.FirstOrDefaultAsync(x => x.Id == id);

            if (image is null)
            {
                return new ApiResponse<AuthBackgroundResponse>
                {
                    Message = $"Auth background image with id {id} does not exist",
                    Success = false
                };
            }

            return new ApiResponse<AuthBackgroundResponse>
            {
                Data = new AuthBackgroundResponse
                {
                    Id = image.Id,
                    Description = image.Description,
                    Image = image.Image,
                    ContentType = image.ContentType,
                },
                Success = true,
            };
        }

        public ApiResponse<IEnumerable<AuthBackgroundResponse>> GetAuthBackgrounds()
        {
            List<AuthBackgroundResponse> authBackgroundResponses = new List<AuthBackgroundResponse>();
            foreach (AuthBackgroundImage image in _database.AuthBackgroundImage)
            {
                authBackgroundResponses.Add(new AuthBackgroundResponse
                {
                    Id = image.Id,
                    Description = image.Description,
                    Image = image.Image,
                    ContentType = image.ContentType,
                });
            }

            return new ApiResponse<IEnumerable<AuthBackgroundResponse>>
            {
                Data = authBackgroundResponses,
                Success = true,
            };
        }

        #endregion
    }
}
