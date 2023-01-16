using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BusinessLayer.Services.Email.Interface;
using DataLayer.Dtos;
using DataLayer.Enums;
using DataLayer.Model;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace BusinessLayer.Services.Email

{
    public class EmailService : IEmailService
    {
        string _template = "LiteHR.Services.Email.default.cshtml";
        string _templateAlt = "APIs.EmailTemplates.passwordReset.cshtml";
        IWebHostEnvironment HostingEnvironment { get; }

        private readonly IFluentEmail _email;
        private readonly IConfiguration _configuration;
        private readonly string frontEndUrl;

        public EmailService([FromServices] IFluentEmail email, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _email = email;

            _configuration = configuration;
            var sender = new MailgunSender("nrf.lloydant.com", "key-8540f3ef6a66cdaf8d9121f11c99aa6b");
            _email.Sender = sender;
            frontEndUrl = _configuration.GetValue<string>("Url:frontUrl");
            HostingEnvironment = hostingEnvironment;


        }

       
        public async Task<bool> SendOneTimePassword(EmailDto dto)
        {
            try
            {
                dto.MailGunTemplate = "otptemplate";
                return await SendMail(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> PasswordReset(EmailDto dto)
        {
            try
            {
                dto.MailGunTemplate = "passwordupdate";
                return await SendMail(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> LiveLectureAlert(EmailDto dto)
        {
            try
            {
                dto.MailGunTemplate = "elearnlivelecture";
                return await SendMail(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> AccountAddedALert(EmailDto dto)
        {
            try
            {
                dto.MailGunTemplate = "accountadded";
                return await SendMail(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> EmailFormatter(EmailDto dto)
        {
            switch (dto.NotificationCategory)
            {
                case EmailNotificationCategory.OTP:
                    return await SendOneTimePassword(dto);
                case EmailNotificationCategory.PasswordReset:
                    return await PasswordReset(dto);
                case EmailNotificationCategory.LiveLectureAlert:
                    return await LiveLectureAlert(dto);
                case EmailNotificationCategory.AccountAdded:
                    return await AccountAddedALert(dto);
                default:
                    throw new Exception("Inavild Email Category");
            }

        }


        private async Task<bool> SendMail(EmailDto dto)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3/");
            client.Authenticator =
                new HttpBasicAuthenticator("api", "key-8540f3ef6a66cdaf8d9121f11c99aa6b");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "nrf.lloydant.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Elearn NG <mailgun@elearnng.com>");
            request.AddParameter("to", dto.ReceiverEmail);
            request.AddParameter("subject", dto.Subject);
            request.AddParameter("v:user", dto.ReceiverName);
            request.AddParameter("template", dto.MailGunTemplate);
            request.AddParameter("v:message", dto.message);
            request.AddParameter("v:regNumber", dto.RegNumber);
            request.AddParameter("v:generatedPassword", dto.Password);

            request.Method = Method.POST;
            var stat = client.Execute(request);
            if(stat.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

    
        //public async Task SendMail(EmailDto sendEmailDto, string template)
        //{
        //    try
        //    {
        //        string pathToEmailFile = $"{HostingEnvironment.WebRootPath}/EmailTemplates/LoginAlert.html";
        //        if (!string.IsNullOrEmpty(template))
        //        {
        //            _template = template;
        //        }
        //        var templateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "default.html");

        //        var parsedTemplate = ParseTemplate(_template);
        //        //var parsedTemplate = _template;
        //        if (!string.IsNullOrEmpty(parsedTemplate))
        //        {
        //            try
        //            {
        //                var sendStatus = await _email
        //               .SetFrom(sendEmailDto.SenderEmail, sendEmailDto.SenderName)
        //               .To(sendEmailDto.ReceiverEmail)
        //               .Subject(sendEmailDto.Subject)
        //               .UsingTemplate(parsedTemplate, sendEmailDto)
        //               .SendAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }


        //}

        //private string ParseTemplate(string path)
        //{
        //    string result;
        //    var assembly = Assembly.GetExecutingAssembly();
        //    using (var stream = assembly.GetManifestResourceStream(path))
        //    using (var reader = new StreamReader(stream))
        //    {
        //        result = reader.ReadToEnd();
        //    }

        //    return result;
        //}

    }
}
