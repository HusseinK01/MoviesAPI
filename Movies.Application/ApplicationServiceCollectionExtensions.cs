using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;
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
            services.AddSingleton<IMovieRepository, MovieRepository>();
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
