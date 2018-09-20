using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
                try
                {
                    var http = (HttpWebRequest)WebRequest.Create(_settings.Value.Url);
                    http.GetResponse();
                }
                catch (Exception)
                {
                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
