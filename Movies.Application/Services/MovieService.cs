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

        public MovieService(IMovieRepository movieRepository, IValidator<Movie> movievalidator)
        {
            _movieRepository = movieRepository;
            _Movievalidator = movievalidator;
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

        public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
        {
            return _movieRepository.GetAllAsync(token);
        }

        public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _movieRepository.GetByIdAsync(id, token);
        }

        public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            return _movieRepository.GetBySlugAsync(slug, token);
        }

        public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default)
        {
            await _Movievalidator.ValidateAndThrowAsync(movie, cancellationToken: token);
            var movieExits = await _movieRepository.ExistsByIdAsync(movie.Id, token);
            if (!movieExits)
            {
                return null;
            }
            await _movieRepository.UpdateAsync(movie, token);
            return movie;
        }
    }
}
