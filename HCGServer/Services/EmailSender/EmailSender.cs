using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HCGServer.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        protected ILogger _logger { get; }

        public EmailSender(ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public async Task SendEmail(string _reciver, string _subject, string _message, string _sender = null)
        {
            Task T2;
            RestClient Client = new RestClient();
            Client.BaseUrl = new Uri(EmailProperties.BaseUrl);
            Client.Authenticator = new HttpBasicAuthenticator("api", EmailProperties.PrivateApiKey);

            _sender = _sender ?? EmailProperties.PRName;
            _sender = $"{_sender} <{EmailProperties.UserName}@{EmailProperties.DomainName}>";

            for (int i = 0; i < 3; ++i) {
                RestRequest Request = new RestRequest();
                Request.AddParameter("from", _sender);
                Request.AddParameter("domain", EmailProperties.DomainName, ParameterType.UrlSegment);
                Request.AddParameter("subject", _subject);
                Request.Resource = "{domain}/messages";
                Request.AddParameter("text", _message);
                Request.AddParameter("to", _reciver);
                Request.Method = Method.POST;

                try {
                    IRestResponse Response = Client.Execute(Request);
                    if (Response.StatusCode != System.Net.HttpStatusCode.OK) {
                        T2 = WaitFor(TimeSpan.FromSeconds(Math.Pow(i++, 2) * 10));
                        if (i < 2) { _logger.LogWarning($"Couldn't send Email to {_reciver}, going to retry {(2 - i)} more time.");
                        } else { _logger.LogError($"Couldn't send Email to {_reciver}, going to abort the process."); }
                    } else { break; }
                } catch (Exception ex) {
                    T2 = WaitFor(TimeSpan.FromSeconds(Math.Pow(i++, 2) * 10));
                    if (i < 2) { _logger.LogWarning($"Couldn't send Email to {_reciver}, going to retry {(2 - i)} more time.", ex);
                    } else { _logger.LogWarning($"Couldn't send Email to {_reciver}, going to abort the process.", ex);  }
                }
                await T2;
            }
        }

        private static async Task<TimeSpan> WaitFor(TimeSpan delay)
        {
            await Task.Delay(delay);
            return delay;
        }
    }

    public static class EmailProperties
    {
        public static string PRName { get; set; }
        public static string BaseUrl { get; set; }
        public static string UserName { get; set; }
        public static string DomainName { get; set; }
        public static string PublicApiKey { get; set; }
        public static string PrivateApiKey { get; set; }

        public static Task Load()
        {
            JObject _DBC = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + @"/Secrets/APIs/Mailgun.json"));

            PRName = (string)_DBC["PRName"];
            BaseUrl = (string)_DBC["BaseUrl"];
            UserName = (string)_DBC["UserName"];
            DomainName = (string)_DBC["DomainName"];
            PublicApiKey = (string)_DBC["ApiKey"]["PublicApiKey"];
            PrivateApiKey = (string)_DBC["ApiKey"]["PrivateApiKey"];

            return Task.FromResult(0);
        }
    }
}
