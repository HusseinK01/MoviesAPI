using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<Movie> _Movievalidator;
        private readonly IRatingsRepository _ratingsRepository;

        public MovieService(IMovieRepository movieRepository, IValidator<Movie> movievalidator, IRatingsRepository ratingsRepository)
        {
            _movieRepository = movieRepository;
            _Movievalidator = movievalidator;
            _ratingsRepository = ratingsRepository;
        }

        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            await _Movievalidator.ValidateAndThrowAsync(movie, cancellationToken : token);
            return await _movieRepository.CreateAsync(movie, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _movieRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetAllAsync(userId, token);
        }

        public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetByIdAsync( id, userId, token);
        }

        public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetBySlugAsync(slug,userId, token);
        }

        public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
        {
            await _Movievalidator.ValidateAndThrowAsync(movie, cancellationToken: token);
            var movieExits = await _movieRepository.ExistsByIdAsync(movie.Id, token);
            if (!movieExits)
            {
                return null;
            }
            await _movieRepository.UpdateAsync(movie ,token);

            if (!userId.HasValue)
            {
                movie.Rating = await _ratingsRepository.GetRatingAsync(movie.Id, token);
                return movie;
            }

            var ratings = await _ratingsRepository.GetRatingAsync(movie.Id, userId.Value, token);
            movie.Rating = ratings.rating;
            movie.UserRating = ratings.userRating;


            return movie;
        }
    }
}
