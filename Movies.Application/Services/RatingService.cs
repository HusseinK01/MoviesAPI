using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    class RatingService : IRatingsService
    {

        private readonly IRatingsRepository _ratingsRepository;
        private readonly IMovieRepository _movieRepository;

        public RatingService(IRatingsRepository ratingsRepository, IMovieRepository movieRepository)
        {
            _ratingsRepository = ratingsRepository;
            _movieRepository = movieRepository;
        }

        public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
        {

            var movieExists = await _movieRepository.ExistsByIdAsync(movieId);

            if (!movieExists) { return false; }

            return await _ratingsRepository.DeleteRatingAsync(movieId, userId, token);
        }

        public async Task<bool> RateMovieAsync(Guid movieId, Guid userId, int rating, CancellationToken token = default)
        {
            
            if (rating <= 0 || rating > 5)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure( "Rating", "Rating must be between 1 and 5")
                });
            }

            var movieExists = await _movieRepository.ExistsByIdAsync(movieId);

            if (!movieExists) { return false; }

            return await _ratingsRepository.RateMovieAsync(movieId, userId, rating, token);
        }

        
    }
}
