using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MovieRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> CreateAsync(Movie movie)
        {
            using var conn = await _dbConnectionFactory.CreateConnectionAsync();

            var tx = conn.BeginTransaction();

            var result = await conn.ExecuteAsync(
                
                new CommandDefinition("""
                    insert into movies (id, slug, title, yearofrelease)
                    values (@Id, @Slug, @Title, @YearOfRelease)
                    """, movie
                )
            );

            if (result > 0)
            {
                
                foreach(var genre in movie.Genres)
                {

                    await conn.ExecuteAsync(

                        new CommandDefinition(

                         """
                         insert into genres (movieId, name)
                         values (@MovieId, @Name)
                         """,
                         new { MovieId = movie.Id, Name = genre }

                         )
                     );

                }

              
            }

            tx.Commit();

            return result > 0;




        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {

            throw new NotImplementedException();


        }

        public async Task<IEnumerable<Movie>> GetAllAsync()

        {
            throw new NotImplementedException();

        }

        public async Task<Movie?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();


        }

        public async Task<Movie?> GetBySlugAsync(string slug)
        {
            throw new NotImplementedException();

        }

        public async Task<bool> UpdateAsync(Movie movie)
        {
            throw new NotImplementedException();

        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        
    }
}
