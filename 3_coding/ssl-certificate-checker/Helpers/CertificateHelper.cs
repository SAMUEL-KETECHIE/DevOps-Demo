using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SslCertificateChecker.Abstractions;
using SslCertificateChecker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SslCertificateChecker.Helpers
{
    public class CertificateHelper : ICertificateHelper
    {

        private readonly ILogger<ICertificateHelper> _logger;
        private readonly AppConfigs _appConfigs;

        public CertificateHelper(
            ILogger<ICertificateHelper> logger,
            IOptions<AppConfigs> options
            )
        {
            _logger = logger;
            _appConfigs = options.Value;
        }
        public async Task<string> CheckDomainSSLExpiry(string domain)
        {
            // Create an HttpClientHandler object and set to use default credentials
            HttpClientHandler handler = new()
            {
                // Set custom server validation callback
                ServerCertificateCustomValidationCallback = ServerCertificateValidationCallback
            };

            HttpClient client = new(handler);

            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://{domain}");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Read {responseBody.Length} characters");
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "An error occurred executing {MethodName}- {Input} - {Trace}\n",
                     nameof(CheckDomainSSLExpiry), new { domain }, e.StackTrace);
                return e.Message;
            }
        }


        private bool ServerCertificateValidationCallback(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {

            // It is possible inpect the certificate provided by server
            var validationMessage = $"Domain Url: {requestMessage.RequestUri}\n" +
                $"<br />Effective date: {certificate?.GetEffectiveDateString()}\n" +
                $"<br />Expiry date: {certificate?.GetExpirationDateString()}\n" +
                //$"<br />Issuer: {certificate?.Issuer}\n" +
                $"<br />Subject: {certificate?.Subject}";

            _logger.LogInformation(validationMessage);

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var expirationDate = DateTime.Parse(certificate.GetExpirationDateString(), CultureInfo.InvariantCulture);
            var daysRemaining = expirationDate - DateTime.Today;
            var envCertCheckFromDaysMore = Environment.GetEnvironmentVariable("CERT_CHECK_FROM_DAYS_MORE");
            var certCheckFromDaysMore = !string.IsNullOrWhiteSpace(envCertCheckFromDaysMore) ? int.Parse(envCertCheckFromDaysMore) : _appConfigs.CertCheckFromDaysMore;
            if (daysRemaining < TimeSpan.FromDays(certCheckFromDaysMore))
            {
                var expiryMessage = $"{daysRemaining.Days} day(s) left for SSL certificate for {requestMessage.RequestUri} to expire.\n\n";
                _logger.LogInformation(expiryMessage);

                if (_appConfigs.IsTeamsEnabled)
                    _ = SendNotificationToTeams($"{expiryMessage}\n<br />\n{validationMessage}\n", $"{requestMessage.RequestUri}", _appConfigs.WebHookUrl);
            }
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            else
            {
                _logger.LogError(new Exception("Cert policy errors: " + sslPolicyErrors.ToString()), "An error occurred executing {MethodName}- {Input} - {Trace}\n",
                     nameof(ServerCertificateValidationCallback), new { requestMessage }, "Cert policy errors: " + sslPolicyErrors.ToString());
                return false;
            }
        }


        public async Task<string> SendNotificationToTeams(string requestMessage,string subTitle, string? webHookUrl = null)
        {
            using var httpClient = new HttpClient();
            var requestPayload = new
            {
                title="SSL Certificate Expiry",
                subtitle= $"<h3>{subTitle}</h3>",
                text = requestMessage
            };

            var response = await httpClient.PostAsync(webHookUrl, new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json"));
            return await response.Content.ReadAsStringAsync();
        }

    }
}
