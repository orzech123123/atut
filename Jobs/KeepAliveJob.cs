using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Atut.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Atut.Jobs
{
    public class KeepAliveJob : BackgroundJob
    {
        private readonly IOptions<KeepAliveSettings> _settings;

        public KeepAliveJob(IServiceScopeFactory scopeFactory, IOptions<KeepAliveSettings> settings) : base(scopeFactory)
        {
            _settings = settings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = ScopeFactory.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    try
                    {
                        var http = (HttpWebRequest)WebRequest.Create(_settings.Value.Url);
                        http.GetResponse();
                    }
                    catch (Exception e)
                    {
                        await emailService.SendEmailAsync(
                            "michalorzechowski123@gmail.com",
                            "Test - wysypalem sie",
                            e.Message
                        );
                    }

                    await Task.Delay(30000, stoppingToken);
                }
            }
        }
    }
}
