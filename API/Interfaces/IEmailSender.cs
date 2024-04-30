using API.Utility;

namespace API.Interfaces
{
    public interface IEmailSender
    {
        Task Send(EmailMetadata emailMetadata);
    }
}
