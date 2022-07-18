//using MailKit.Net.Smtp;
using MimeKit;
using NuGet.Packaging;
using System.Linq;
using System.Net.Mail;

namespace File_Sharing_App.Helper.Mail
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _config;

        public MailHelper(IConfiguration config)
        {
            this._config = config;
        }
        /*
                public void SendMail(InputMailMessage model)
                {
                    var emailMessage = CreateEmailMessage(model);
                    Send(emailMessage);
                }

                private MimeMessage CreateEmailMessage(InputMailMessage model)
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("email", _config.GetValue<string>("Mail:From")));
                    emailMessage.To.Add(new MailboxAddress(_config.GetValue<string>("Mail:Sender"),model.Email));
                    emailMessage.Subject = model.Subject;
                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = model.Body };
                    emailMessage.HtmlBody.Append('a');

                    return emailMessage;
                }
                private void Send(MimeMessage mailMessage)
                {
                    using (var client = new SmtpClient())
                    {
                        try
                        {
                            client.Connect(_config.GetValue<string>("Mail:Host"), _config.GetValue<int>("Mail:Port"), true);
                            client.AuthenticationMechanisms.Remove("XOAUTH2");
                            client.Authenticate(_config.GetValue<string>("Mail:From"), _config.GetValue<string>("Mail:PWD"));
                            client.Send(mailMessage);
                        }
                        catch
                        {
                            //log an error message or throw an exception or both.
                            throw;
                        }
                        finally
                        {
                            client.Disconnect(true);
                            client.Dispose();
                        }
                    }
                }
        */

        public void SendMail(InputMailMessage model)
        {
            using (SmtpClient client = new SmtpClient(_config.GetValue<string>("Mail:Host"), _config.GetValue<int>("Mail:Port")))
            {
                var msg = new MailMessage();
                //var msg = new MailMessage(_config.GetValue<string>("Mail:From"), model.Email);
                // msg.To.Add(_config.GetValue<string>("Mail:From"));
                msg.To.Add(model.Email);
                msg.Subject = model.Subject;
                msg.Body = model.Body;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(_config.GetValue<string>("Mail:From"), _config.GetValue<string>("Mail:Sender"), System.Text.Encoding.UTF8);
                //msg.From = new MailAddress($"{model.Email}", _config.GetValue<string>("Mail:Sender"), System.Text.Encoding.UTF8);
                //client.Credentials = new System.Net.NetworkCredential(model.Email,model.Password);
                /*
                client.UseDefaultCredentials = true;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                */
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(_config.GetValue<string>("Mail:From"), _config.GetValue<string>("Mail:PWD"));
                client.Send(msg);


            }
        }

        public void SendMailToAdmin(InputMailMessage model)
        {
            using (SmtpClient client = new SmtpClient(_config.GetValue<string>("Mail:Host"), _config.GetValue<int>("Mail:Port")))
            {
                var msg = new MailMessage();
                msg.To.Add(_config.GetValue<string>("Mail:From"));
                msg.Subject = model.Subject;
                msg.Body = model.Body;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(model.Email, _config.GetValue<string>("Mail:Sender"), System.Text.Encoding.UTF8);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(_config.GetValue<string>("Mail:From"), _config.GetValue<string>("Mail:PWD"));
                client.Send(msg);
            }
        }
    }
}
