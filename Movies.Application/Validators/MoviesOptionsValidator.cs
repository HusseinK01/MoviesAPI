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
        }
    }
}
