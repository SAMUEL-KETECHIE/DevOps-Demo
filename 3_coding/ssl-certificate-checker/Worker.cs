using Microsoft.Extensions.Options;
using SslCertificateChecker.Abstractions;
using SslCertificateChecker.Models;

namespace SslCertificateChecker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICertificateHelper _certificateHelper;
        private readonly AppConfigs _appConfigs;
        public Worker(ILogger<Worker> logger,
            ICertificateHelper certificateHelper,
            IOptions<AppConfigs> options
            )
        {
            _logger = logger;
            _certificateHelper = certificateHelper;
            _appConfigs = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("SSL Certificate Checker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                if (DateTime.Now.Hour == _appConfigs.ExecHour)
                {
                    var domains = _appConfigs.Domains?.Split(',');
                    if (domains?.Any()==true)
                        foreach (var domain in domains)
                        {
                            await _certificateHelper.CheckDomainSSLExpiry(domain);
                        }
                }
                //delay for an hour
                await Task.Delay(3600000, stoppingToken);
            }
        }
    }
}