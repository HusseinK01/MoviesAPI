using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class MovieValidator : AbstractValidator<Movie>
    {
        private readonly IMovieRepository _movieRepository;
        public MovieValidator(IMovieRepository movieService)
        {
            _movieRepository = movieService;

            RuleFor(x => x.Id)
            .NotEmpty();

            RuleFor(x => x.Genres)
                .NotEmpty();

            RuleFor(x => x.YearOfRelease)
                .GreaterThanOrEqualTo(1900)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Slug)
                .MustAsync(ValidateSlug)
                .WithMessage("This Movie already exists in the system");
        }

        private async Task<bool> ValidateSlug(Movie movie ,string slug, CancellationToken token)
        {

            var existingMovie = await _movieRepository.GetBySlugAsync(slug);

            if (existingMovie is not null)
            {
                //movie exists
                //ids match= means we are going for an update, A-okay!
                //ids don't match= means identical slug for different movies, No-way!
                return existingMovie.Id == movie.Id;
            }

            //
            return existingMovie is null;
            
        }
    }
}
