﻿using iChat.Api.Constants;
using iChat.Api.Contract;
using iChat.Api.Helpers;
using iChat.Api.Models;
using iChat.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace iChat.Api.Services {
    public class EmailService : IEmailService {
        private readonly AppSettings _appSettings;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmailService(IOptions<AppSettings> appSettings, IHostingEnvironment hostingEnvironment) {
            _appSettings = appSettings.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task SendUserInvitationEmailAsync(UserInvitationData data) {
            if (string.IsNullOrWhiteSpace(data.ReceiverAddress)) {
                throw new ArgumentException("Cannot send to empty email address.");
            }
            var body = ConstructUserInvitationEmailBody(data);

            using (MailMessage mailMessage = new MailMessage(new MailAddress(_appSettings.GmailUserName), new MailAddress(data.ReceiverAddress))) {
                mailMessage.Subject = $"Join iChat";
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                var smtp = new SmtpClient {
                    Host = _appSettings.GmailHost,
                    Port = Int32.Parse(_appSettings.GmailPort),
                    EnableSsl = Boolean.Parse(_appSettings.GmailSsl),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_appSettings.GmailUserName, _appSettings.GmailPassword)
                };
                await smtp.SendMailAsync(mailMessage);
            }
        }

        private string ConstructUserInvitationEmailBody(UserInvitationData data) {
            var emailTemplatePath = Path.Combine(iChatConstants.EmailTemplatePath, "UserInvitationEmail.htm");
            var joinUrl = $"{_appSettings.FrontEndUrl}/user/acceptinvitation" +
                $"?email={data.ReceiverAddress}&code={data.InvitationCode}" +
                $"&workspaceName={data.WorkspaceName}";

            var body = string.Empty;
            if (!string.IsNullOrEmpty(emailTemplatePath)) {
                using (StreamReader reader = new StreamReader(emailTemplatePath)) {
                    body = reader.ReadToEnd();
                }
            }
            body = body.Replace("{WorkspaceName}", HttpUtility.HtmlEncode(data.WorkspaceName));
            body = body.Replace("{InviterName}", HttpUtility.HtmlEncode(data.InviterName));
            body = body.Replace("{InviterEmail}", HttpUtility.HtmlEncode(data.InviterEmail));
            body = body.Replace("{JoinUrl}", joinUrl);
            return body;
        }
    }
}