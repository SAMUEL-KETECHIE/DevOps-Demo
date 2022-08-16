using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SslCertificateChecker.Models
{
    public class AppConfigs
    {
        public string? WebHookUrl { get; set; }
        public string? Domains { get; set; }
        public bool IsTeamsEnabled { get; set; }
        public int ExecHour { get; set; }
        public int CertCheckFromDaysMore { get; set; }
        public int ExecMinute { get; set; }
    }
}
