using API.Interfaces;
using API.Utility;
using FluentEmail.Core;

namespace API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmail _fluentEmail;
        public EmailSender(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }
        public async Task Send(EmailMetadata emailMetadata)
        {
            if (string.IsNullOrWhiteSpace(emailMetadata.Attachment))
            {
                await _fluentEmail.To(emailMetadata.ToAdress)
                    .Subject(emailMetadata.Subject)
                    .Body(emailMetadata.Body)
                    .SendAsync();

            } 
            else
            {
                await _fluentEmail.To(emailMetadata.ToAdress)
                    .Subject(emailMetadata.Subject)
                    .UsingTemplateFromFile(emailMetadata.Attachment,
                    new
                    {
                        Name = emailMetadata.ToAdress,
                        Link = emailMetadata.Body
                    })
                    .SendAsync();
            }
        }
    }
}
