using DataAccess.IRepositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace Business.BackgroundJobService
{
    public class BackgroundJobService
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public BackgroundJobService(IServiceScopeFactory scopeFactory, HealthCheckService healthCheckService, IEmailSender emailSender, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _healthCheckService = healthCheckService;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task HealthCheck()
        {
            var healthCheckResult = await _healthCheckService.CheckHealthAsync();
            var result = healthCheckResult.Entries.ToDictionary(e => e.Key, e => new
            {
                Result = e.Value.Status.ToString(),
                IsSuccess = e.Value.Status == HealthStatus.Healthy,
                Message = e.Value.Exception?.Message ?? e.Value.Description
            });

            StringBuilder emailContent = new StringBuilder();
            bool hasUnhealthyServices = false;

            foreach (var entry in result)
            {
                if (!entry.Value.IsSuccess)
                {
                    hasUnhealthyServices = true;
                    string unhealthyMessage = $"Unhealthy Service Detected: {entry.Key}<br>Result: {entry.Value.Result}<br>IsSuccess: {entry.Value.IsSuccess}<br>Message: {entry.Value.Message}<br><br>";
                    emailContent.AppendLine(unhealthyMessage);
                }
                else
                {
                    string healthyMessage = $"Healthy Service Detected: {entry.Key}<br>Result: {entry.Value.Result}<br>IsSuccess: {entry.Value.IsSuccess}<br>Message: {entry.Value.Message}<br><br>";
                }
            }

            if (hasUnhealthyServices)
            {
                string emailBody = emailContent.ToString();
                await _emailSender.SendEmailAsync(_configuration["EmailSettings:SupportEmail"], "Health Check Report", emailBody);
            }
        }

        public async Task CancelOrders()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                await orderRepository.CancelOrders();
            }
        }
    }
}
