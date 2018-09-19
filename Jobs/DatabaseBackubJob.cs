using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atut.Models;
using Atut.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Atut.Jobs
{
    public class DatabaseBackgroundJob : BackgroundJob
    {
        public DatabaseBackgroundJob(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = ScopeFactory.CreateScope())
                {
                    var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    //                    var vehicles = databaseContext.Vehicles.ToList();
                    //                    Console.WriteLine("[IncomingEthTxService] Service is Running");

                    await emailService.SendEmailAsync(
                        "michalorzechowski123@gmail.com",
                        "Test",
                        $"Test -> {DateTime.Now}"
                    );

                    await Task.Delay(60000, stoppingToken);
                }
            }
        }
    }
}
