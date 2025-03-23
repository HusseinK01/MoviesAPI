using Dapper;
using Movies.Application.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    class RatingsRepository : IRatingsRepository
    {

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public RatingsRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
        {
            var conn = await _dbConnectionFactory.CreateConnectionAsync();

            var tx = conn.BeginTransaction();

            var result = await conn.ExecuteAsync(new CommandDefinition("""

                delete from ratings where userid = @userId and movieid = @movieId

                """, new {userId, movieId}));
            tx.Commit();

            return result > 0;
        }

        public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default)
        {
            var conn = await _dbConnectionFactory.CreateConnectionAsync();
            var result = await conn.QuerySingleOrDefaultAsync(new CommandDefinition("""
                select round(avg(r.rating),1) as Rating from ratings r where r.movieid = @movieId group by r.movieid
                """, new { movieId }, cancellationToken: token));

            return result;
        }

        public async Task<(float? rating, int? userRating)> GetRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
        {
            var conn = await _dbConnectionFactory.CreateConnectionAsync();

            var result = await conn.QuerySingleOrDefaultAsync(new CommandDefinition("""
                select round(avg(r.rating),1) as Rating, 
                (select rr.rating from ratings rr where rr.userid = @userId and rr.movieid = @movieId) as UserRating 
                from ratings r where r.movieid = @movieId group by r.movieid
                """, new {userId, movieId}, cancellationToken: token));

            return result;
        }

        public async Task<bool> RateMovieAsync(Guid movieId, Guid userId, int rating, CancellationToken token = default)
        {
            var conn = await _dbConnectionFactory.CreateConnectionAsync();

            var result = await conn.ExecuteAsync(new CommandDefinition("""

                insert into ratings (rating, movieId, userId) values (@rating, @movieId, @userId)
                on conflict (movieId,userId) do update set rating = @rating 

                """, new { rating, movieId, userId}));

            return result > 0;
        }


    }
}
