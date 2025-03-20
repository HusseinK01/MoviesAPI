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

        public async Task<bool> CreateAsync(Movie movie)
        {
            await _Movievalidator.ValidateAndThrowAsync(movie);
            return await _movieRepository.CreateAsync(movie);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _movieRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return _movieRepository.GetAllAsync();
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            return _movieRepository.GetByIdAsync(id);
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            return _movieRepository.GetBySlugAsync(slug);
        }

        public async Task<Movie?> UpdateAsync(Movie movie)
        {
            await _Movievalidator.ValidateAndThrowAsync(movie);
            var movieExits = await _movieRepository.ExistsByIdAsync(movie.Id);
            if (!movieExits)
            {
                return null;
            }
            await _movieRepository.UpdateAsync(movie);
            return movie;
        }
    }
}
