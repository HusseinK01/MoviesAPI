using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.Application.Database;

namespace Movies.API.Health
{
    public class DatabaseHealth : IHealthCheck
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<DatabaseHealth> _logger;
        public const string Name = "Database Check";

        public DatabaseHealth(IDbConnectionFactory dbConnectionFactory, ILogger<DatabaseHealth> logger)
        {
            this._dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try {
                _ = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
                return HealthCheckResult.Healthy();
            
            }
            catch (Exception e){


                string errorMessage = "Database is unhealthy";

                _logger.LogError(errorMessage, e);
                return HealthCheckResult.Unhealthy(errorMessage,e);

            
            }
        }
    }
}
