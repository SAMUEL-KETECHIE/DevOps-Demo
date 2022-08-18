using SslCertificateChecker;
using SslCertificateChecker.Abstractions;
using SslCertificateChecker.Helpers;
using SslCertificateChecker.Models;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services) =>
    {
        services.Configure<AppConfigs>(context.Configuration.GetSection("AppConfigs"));

        services.AddTransient<ICertificateHelper, CertificateHelper>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
