using FluentValidation;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class MoviesOptionsValidator : AbstractValidator<MoviesOptions>
    {
        public MoviesOptionsValidator()
        {
            RuleFor(x => x.YearOfRelease).
            GreaterThanOrEqualTo(1900);

            RuleFor(x => x.SortField).Must(f => SortFields.Contains(f, StringComparer.OrdinalIgnoreCase)||f is null).
                WithMessage("Sort field must be title or year of release");

            RuleFor(x => x.Size)
                .InclusiveBetween(1, 25)
                .WithMessage("You can get between 1 and 25 movies per page");

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);
        }
        private static readonly string[] SortFields =
        {
            "yearofrelease", "title"
        };
    }
}
