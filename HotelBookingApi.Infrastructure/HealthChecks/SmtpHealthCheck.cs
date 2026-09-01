using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Infrastructure.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace HotelBookingApi.Infrastructure.HealthChecks;

public class SmtpHealthCheck : IHealthCheck
{
    private readonly SmtpSettings _smtpSettings;

    public SmtpHealthCheck(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var tcpClient = new TcpClient();
            
            var connectTask = tcpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port);
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

            var completedTask = await Task.WhenAny(connectTask, timeoutTask);
            if (completedTask == timeoutTask)
            {
                return HealthCheckResult.Unhealthy("SMTP connection timeout.");
            }
            
            if (connectTask.IsFaulted)
            {
                return HealthCheckResult.Unhealthy($"SMTP connection failed: {connectTask.Exception?.Message}");
            }

            return HealthCheckResult.Healthy("SMTP connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Exception while checking SMTP health: {ex.Message}");
        }
    }
}
