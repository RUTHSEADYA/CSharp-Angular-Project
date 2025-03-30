using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

using Microsoft.Extensions.Hosting;
using data;
using Microsoft.Extensions.Logging;


namespace service.Service
{

    public class PatientScoreUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PatientScoreUpdaterService> _logger;

        public PatientScoreUpdaterService(IServiceScopeFactory scopeFactory, ILogger<PatientScoreUpdaterService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                    var waitingPatients = dbContext.patientFileList
                        .Where(p => p.Status == "Waiting")
                        .ToList();

                    foreach (var patient in waitingPatients)
                    {
                        patient.UpdateFinalScore(); 
                    }

                    await dbContext.SaveChangesAsync();
                }


                _logger.LogInformation("עדכון ניקוד הושלם. המתנה ל-2 דקות...");
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}