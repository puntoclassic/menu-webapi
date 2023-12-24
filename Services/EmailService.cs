using MenuWebapi.Models.Options;
using MenuWebapi.Models.EmailModel;
using MimeKit;
using FluentEmail.Core;
using Microsoft.Extensions.Options;
using FluentEmail.Razor;
using System.Net;
using System.Net.Mail;
using FluentEmail.Core.Models;
using MenuWebapi.Models.Entities;

namespace MenuWebapi.Services
{
    public class EmailService
    {
        private EmailOptionsModel emailOptionsModel;


        public EmailService(IOptions<EmailOptionsModel> _emailOptionsModel)
        {
            this.emailOptionsModel = _emailOptionsModel.Value;


        }
        public void SendPasswordResetEmail(PasswordResetModel passwordResetModel)
        {
            var template = File.ReadAllText($"{Directory.GetCurrentDirectory()}/EmailTemplates/AccountResetPassword.cshtml");

            Email.DefaultRenderer = new RazorRenderer(Directory.GetCurrentDirectory());

            var email = Email
                .From(emailOptionsModel.Username)
                .To(passwordResetModel.Email)
                .Subject("Reset della password")
                .UsingTemplate(template, passwordResetModel);

            FluentEmail.Smtp.SmtpSender sender = new FluentEmail.Smtp.SmtpSender(new SmtpClient
            {
                Host = emailOptionsModel.Host!,
                Credentials = new NetworkCredential
                {
                    Password = emailOptionsModel.Password,
                    UserName = emailOptionsModel.Username,
                },
                Port = emailOptionsModel.Port,
                EnableSsl = emailOptionsModel.Secure,
            });

            SendResponse response = sender.Send(email);

            if (response.Successful)
            {
                Console.WriteLine("The email was sent successfully");
            }
            else
            {
                Console.WriteLine("The email could not be sent. Check the errors: ");
                foreach (string error in response.ErrorMessages)
                {
                    Console.WriteLine(error);
                }
            }
        }

        public void SendOrderCreatedEmail(Order order)
        {
            var template = File.ReadAllText($"{Directory.GetCurrentDirectory()}/EmailTemplates/OrderCreated.cshtml");

            Email.DefaultRenderer = new RazorRenderer(Directory.GetCurrentDirectory());

            var email = Email
                .From(emailOptionsModel.Username)
                .To(order.User!.Email)
                .Subject("Ordine creato")
                .UsingTemplate(template, order);

            FluentEmail.Smtp.SmtpSender sender = new FluentEmail.Smtp.SmtpSender(new SmtpClient
            {
                Host = emailOptionsModel.Host!,
                Credentials = new NetworkCredential
                {
                    Password = emailOptionsModel.Password,
                    UserName = emailOptionsModel.Username,
                },
                Port = emailOptionsModel.Port,
                EnableSsl = emailOptionsModel.Secure,
            });

            SendResponse response = sender.Send(email);

            if (response.Successful)
            {
                Console.WriteLine("The email was sent successfully");
            }
            else
            {
                Console.WriteLine("The email could not be sent. Check the errors: ");
                foreach (string error in response.ErrorMessages)
                {
                    Console.WriteLine(error);
                }
            }
        }

        public void SendOrderPaidEmail(Order order)
        {
            var template = File.ReadAllText($"{Directory.GetCurrentDirectory()}/EmailTemplates/OrderPaid.cshtml");

            Email.DefaultRenderer = new RazorRenderer(Directory.GetCurrentDirectory());

            var email = Email
                .From(emailOptionsModel.Username)
                .To(order.User!.Email)
                .Subject("Ordine pagato")
                .UsingTemplate(template, order);

            FluentEmail.Smtp.SmtpSender sender = new FluentEmail.Smtp.SmtpSender(new SmtpClient
            {
                Host = emailOptionsModel.Host!,
                Credentials = new NetworkCredential
                {
                    Password = emailOptionsModel.Password,
                    UserName = emailOptionsModel.Username,
                },
                Port = emailOptionsModel.Port,
                EnableSsl = emailOptionsModel.Secure,
            });

            SendResponse response = sender.Send(email);

            if (response.Successful)
            {
                Console.WriteLine("The email was sent successfully");
            }
            else
            {
                Console.WriteLine("The email could not be sent. Check the errors: ");
                foreach (string error in response.ErrorMessages)
                {
                    Console.WriteLine(error);
                }
            }
        }

        public void SendAccountCreatedEmail(AccountCreatedModel accountCreatedModel)
        {
            var template = File.ReadAllText($"{Directory.GetCurrentDirectory()}/EmailTemplates/AccountActivationCode.cshtml");

            Email.DefaultRenderer = new RazorRenderer(Directory.GetCurrentDirectory());

            var email = Email
                .From(emailOptionsModel.Username)
                .To(accountCreatedModel.Email)
                .Subject("Attiva il tuo account")
                .UsingTemplate(template, accountCreatedModel);

            FluentEmail.Smtp.SmtpSender sender = new FluentEmail.Smtp.SmtpSender(new SmtpClient
            {
                Host = emailOptionsModel.Host!,
                Credentials = new NetworkCredential
                {
                    Password = emailOptionsModel.Password,
                    UserName = emailOptionsModel.Username,
                },
                UseDefaultCredentials = false,
                Port = emailOptionsModel.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = emailOptionsModel.Secure,
            });

            SendResponse response = sender.Send(email);

            if (response.Successful)
            {
                Console.WriteLine("The email was sent successfully");
            }
            else
            {
                Console.WriteLine("The email could not be sent. Check the errors: ");
                foreach (string error in response.ErrorMessages)
                {
                    Console.WriteLine(error);
                }
            }

        }
    }
}



