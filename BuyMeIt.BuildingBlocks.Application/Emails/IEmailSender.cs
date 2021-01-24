using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Application.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
