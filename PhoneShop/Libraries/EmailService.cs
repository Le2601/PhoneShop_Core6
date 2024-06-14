using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace PhoneShop.Libraries
{
    public class EmailService
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            // Tạo message email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("PhoneShop_NNLE", "nnle2000047@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            // Thêm nội dung email
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            // Cấu hình SmtpClient
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls); //ket noi SMTP
                client.Authenticate("ngoclewill2002@gmail.com", "xrkq zfvc vfeb yegz"); 

                // Gửi email
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
