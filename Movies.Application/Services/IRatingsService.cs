using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    public interface IRatingsService
    {

        public Task<bool> RateMovieAsync(Guid movieId, Guid userId, int rating, CancellationToken token = default);
        public Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default);

    }
}
