using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Utility.Internet
{
    public class EmailClient : IDisposable
    {
        private string _server;
        private int _port;
        private string _username;
        private string _password;
        public EmailClient(string server, 
            int port, 
            string username,
            string password)
        {
            _server = server;
            _port = port;
            _username = username;
            _password = password;
        }
        public void SendHtmlEmail(string to, List<string> cc, string subject, string body)
        {
            var fromAddress = new MailAddress(_username);
            var toAddress = new MailAddress(to);
            using (var smtp = new SmtpClient
            {
                Host = _server,
                Port = _port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_username, _password)
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                })
                {
                    foreach (string address in cc)
                    {
                        message.CC.Add(address);
                    }
                    smtp.Send(message);
                }
            }
        }

        public void Dispose()
        {
            //Clean up email setting information
            _server = string.Empty;
            _port = 0;
            _username = string.Empty;
            _password = string.Empty;
        }
    }
}
