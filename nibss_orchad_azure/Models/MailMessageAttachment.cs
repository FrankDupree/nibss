using System.Net.Mail;
using MailMessage = OrchardCore.Email.MailMessage;

namespace nibss_orchad_azure.Models
{
    public class MailMessageAttachment : MailMessage
    {
        public AttachmentCollection Attachments { get; set; }

    }
}
