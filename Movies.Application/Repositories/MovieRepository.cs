﻿using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

            var tx = conn.BeginTransaction();

            var result = await conn.ExecuteAsync(
                
                new CommandDefinition("""
                    insert into movies (id, slug, title, yearofrelease)
                    values (@Id, @Slug, @Title, @YearOfRelease)
                    """, movie, cancellationToken: token
                )
            );

            if (result > 0)
            {

                foreach (var genre in movie.Genres)
                {

                    await conn.ExecuteAsync(

                        new CommandDefinition(

                         """
                         insert into genres (movieId, name)
                         values (@MovieId, @Name)
                         """,
                         new { MovieId = movie.Id, Name = genre },
                         cancellationToken: token

                         )
                     );

                }
              
            }

            tx.Commit();

            return result > 0;


        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {

            var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
            var tx = conn.BeginTransaction();

            await conn.ExecuteAsync(new CommandDefinition("""
                delete from genres where movieid = @id
                """, new { id }, cancellationToken: token));

            var result = await conn.ExecuteAsync(new CommandDefinition("""
                delete from movies where id = @id
                """, new { id }, cancellationToken: token));

            tx.Commit();

            return result > 0;

        }

        public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)

        {
            using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
            var movies_result = await conn.QueryAsync(new CommandDefinition(
                    """
                    select m.*, STRING_AGG(g.name, ',') AS genres from movies m left join genres g on m.id = g.movieid group by m.id, m.title;
                    """, cancellationToken: token
                )
             );

            return movies_result.Select(x => new Movie
            {
                Id = x.id,
                Title = x.title,
                YearOfRelease = x.yearofrelease,
                Genres = Enumerable.ToList(x.genres.Split(','))
            });
        }

        public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        {

            using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
            var movie_result = await conn.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(

                """
                select id, slug, title, yearofrelease from movies where id = @id
                """
                , new { id }, cancellationToken: token
                ));

            if (movie_result is null)
            {
                return null;
            }

            var genres = await conn.QueryAsync<string>(
                new CommandDefinition(
                    
                    """
                    select name from genres where movieid = @id
                    """
                    , new { id }, cancellationToken: token
                    
                    )
                );

            foreach(var genre in genres)
            {
                movie_result.Genres.Add(genre);
            }
            return movie_result;



        }

        public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        {


            using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
            var movie_result = await conn.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(

                """
                select id, slug, title, yearofrelease from movies where slug = @slug
                """
                , new { slug }, cancellationToken: token
                ));

            if (movie_result is null)
            {
                return null;
            }

            var genres = await conn.QueryAsync<string>(
                new CommandDefinition(

                    """
                    select name from genres where movieid = @id
                    """
                    , new { movie_result.Id }
                    , cancellationToken: token

                    )
                );

            foreach (var genre in genres)
            {
                movie_result.Genres.Add(genre);
            }
            return movie_result;

        }

        public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
        {
            using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

                using var tx = conn.BeginTransaction();

            await conn.ExecuteAsync(new CommandDefinition("""
                delete from genres where movieid = @id
                """, new {id = movie.Id}, cancellationToken: token));

            foreach(var genre in movie.Genres)
            {
                await conn.ExecuteAsync(new CommandDefinition("""
                    insert into genres (movieid, name) values (@id, @name)
                    """, new {id = movie.Id, name = genre}, cancellationToken: token));
            }

            var result = await conn.ExecuteAsync(new CommandDefinition("""
                update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease
                where id = @Id
                """, movie, cancellationToken: token));

            tx.Commit();

            return result > 0;

        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

            var result = await conn.QuerySingleOrDefaultAsync(new CommandDefinition("""
                select 1 from movies where id = @id
                """, new {id}, cancellationToken: token));

            if (result is not null)
            {
                return true;
            }
            else { return false; }
        }

        
    }
}
