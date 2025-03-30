using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Database
{
    public interface IDbConnectionFactory
    {

        public Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);

    }
    public class NpgsqlConnectionFactory : IDbConnectionFactory 
    {
        private readonly string _connetionString;


        public NpgsqlConnectionFactory(string connetionString)
        {
            _connetionString = connetionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
        {
           


            var connection = new NpgsqlConnection(_connetionString);
            await connection.OpenAsync(token);
            return connection;
            
        }
    }
}
