﻿using Movies.Application.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    public interface IRatingsRepository
    {
        public Task<bool> RateMovieAsync(Guid movieId, Guid userId, int rating, CancellationToken token = default);
        public Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default);
        public Task<(float? rating, int? userRating)> GetRatingAsync(Guid movieId,Guid userId, CancellationToken token = default);



    }
}
