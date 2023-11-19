using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchIT.Common.Movies.Request;
using WatchIT.Common;

namespace WatchIT.WebAPI.Services.MoviesService
{
    public interface IMoviesService
    {
        Task<ApiResponse<int>> AddMovie(MoviePostPutRequest request);
        Task<ApiResponse> ModifyMovie(int id, MoviePostPutRequest request);
    }
}
