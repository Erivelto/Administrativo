﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
			MailMessage mail = new MailMessage();

			mail.From = new MailAddress("contato@contfy.com.br");
			mail.To.Add(email);
			mail.Subject =subject;
			mail.Body = message;
			mail.IsBodyHtml = true;

			SmtpClient smtp = new SmtpClient();
			smtp.Host = "smtp.gmail.com";
			smtp.EnableSsl = true;
			smtp.Port = 587;
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new NetworkCredential("contato@contfy.com.br", "erivelto33");
			try
			{
				smtp.Send(mail);
			}
			catch (Exception)
			{

				throw;
			}
			return Task.CompletedTask;
        }
    }
}
