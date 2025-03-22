using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Movies.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IRatingsRepository, RatingsRepository>();
            services.AddSingleton<IRatingsService, RatingService>();
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddSingleton<IMovieService, MovieService>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
            return services;
        }

        public static IServiceCollection AddNpgDB(this IServiceCollection services, string connection_string)
        {

            services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connection_string));
            services.AddSingleton<DbInitializer>();
            return services;

        }
    }
}
