using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsBoard.Data
{
    public class MailSettings
    {
        public string FromEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
