using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SslCertificateChecker.Abstractions
{
    public interface ICertificateHelper
    {
        Task<string> CheckDomainSSLExpiry(string domain);
    }
}
