
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace service.Service
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";  
        private readonly int _smtpPort = 587; 
        private readonly string _emailFrom = "rachelis360@gmail.com"; 
        private readonly string _emailPassword = "wict vmav roms pzoh"; 
        



        public async Task SendEmailWithAttachmentAsync(string toEmail, string pdfFileName, byte[] pdfBytes)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Hospital System", _emailFrom));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = pdfFileName; // שם הקובץ ככותרת המייל

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.Attachments.Add(pdfFileName, pdfBytes, ContentType.Parse("application/pdf"));

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                    await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailFrom, _emailPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בשליחת המייל עם מצורף: {ex.Message}");
                throw new InvalidOperationException("שגיאה בשליחת המייל עם מצורף", ex);
            }
        }





























        //public async Task SendEmailAsync(string toEmail, string subject, string body)
        //{
        //    try
        //    {
        //        var message = new MimeMessage();
        //        message.From.Add(new MailboxAddress("Hospital System", _emailFrom));
        //        message.To.Add(new MailboxAddress("", toEmail));
        //        message.Subject = subject;

        //        var bodyBuilder = new BodyBuilder
        //        {
        //            HtmlBody = body
        //        };
        //        message.Body = bodyBuilder.ToMessageBody();

        //        using (var client = new SmtpClient())
        //        {
        //            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        //            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
        //            await client.AuthenticateAsync(_emailFrom, _emailPassword);
        //            await client.SendAsync(message);
        //            await client.DisconnectAsync(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"שגיאה בשליחת המייל: {ex.Message}");
        //        throw new InvalidOperationException("שגיאה בשליחת המייל", ex);
        //    }
        //}
    }
}