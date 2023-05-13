using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace nibss_orchad_azure.Helpers
{
    
    public class Mailer
    {
       

        
        private string Smtpssl { get; set; }
        private string Smtpusername { get; set; }
        private string Smtppassword { get; set; }
        private string Smtpserver { get; set; }
        private string Smtpport { get; set; }

        public Mailer(IConfiguration configuration)
        {
           
            Smtpssl = configuration.GetValue<string>("smtp_ssl");
            Smtpusername = configuration.GetValue<string>("smtp_user");
            Smtppassword = configuration.GetValue<string>("smtp_password");
            Smtpserver = configuration.GetValue<string>("smtp_host");
            Smtpport = configuration.GetValue<string>("smtp_port");
        }


        public bool Sendmail(string msg, string subject, string @from,  string to, string copy, string filePath)
        {

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            bool result = false;
            string server;
            try
            {
                server = Smtpserver;
                if(@from == null)
                {
                    @from = Smtpusername;
                }
                MailAddress fromAddress = new MailAddress(@from, "NIBSS");

                smtpClient.Host = server;
                int port;
                if (Int32.TryParse(Smtpport, out port))
                {
                    smtpClient.Port = port;
                }
                else
                {
                    smtpClient.Port = 587;
                }



                System.Net.NetworkCredential smtpUserInfo = new System.Net.NetworkCredential(Smtpusername, Smtppassword);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = smtpUserInfo;



                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;



                if (Smtpssl.ToUpper() == "TRUE")
                {
                    smtpClient.EnableSsl = true;
                }
                else
                {
                    smtpClient.EnableSsl = false;
                }

               
                message.From = fromAddress;

              
                if (filePath != null)
                {
                    ContentType contentType = new ContentType();
                    contentType.MediaType = MediaTypeNames.Application.Octet;
                    contentType.Name = Path.GetFileName(filePath);
                    Attachment data = new Attachment(filePath, contentType);
                    message.Attachments.Add(data);
                    data.Name = Path.GetFileName(filePath);
                }

              
                if (copy != null)
                {
                    message.CC.Add(copy);
                }
                message.To.Add(to);
                message.Subject = subject;


                message.IsBodyHtml = true;

              
                message.Body = msg;
               
                smtpClient.Send(message);
                result = true;

                message.Dispose();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return result;
        }

    }
}
