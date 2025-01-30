using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GameStore.Presentation.HealthChecks
{
    public class HealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("GameStore is healthy!"));
        }
    }
}