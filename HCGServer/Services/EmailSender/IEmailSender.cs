using System.Threading.Tasks;

namespace HCGServer.Services.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmail(string reciver, string subject, string message, string sender = null);
    }
}
