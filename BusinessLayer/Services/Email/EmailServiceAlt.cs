using BusinessLayer.Services.Email.Interface;
using DataLayer.Dtos;
using DataLayer.Enums;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Mailgun;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Email
{
    public class EmailServiceAlt : IEmailServiceAlt
    {
        string _template = "PayPlat.Logic.Templates.VerificationTemplate.cshtml";
        string _configurationtemplate = "PayPlat.Logic.Templates.ConfigurationTemplate.cshtml";

        string _liveLectureTemplate = "BusinessLayer.Templates.general.cshtml";
      

        private readonly IFluentEmail _email;
        private readonly IConfiguration _configuration;
        private readonly string frontEndUrl;

        public EmailServiceAlt([FromServices] IFluentEmail email, IConfiguration configuration)
        {
            _email = email;

            _configuration = configuration;
            //var sender = new MailgunSender(_configuration.GetValue<string>("MailGun:domain"),
            //_configuration.GetValue<string>("MailGun:apiKey"));
            var sender = new MailgunSender("nrf.lloydant.com", "key-8540f3ef6a66cdaf8d9121f11c99aa6b");
            _email.Sender = sender;
            frontEndUrl = _configuration.GetValue<string>("Url:frontUrl");


        }

        public async Task<bool> SendMail(SendEmailDTO sendEmailDto, string template, int templateType)
        {
            string parsedTemplate = string.Empty;
            parsedTemplate = template;
            //if (!string.IsNullOrEmpty(template))
            //{

            //    if (templateType==(int)EmailCategory.InstitutionConfiguration || templateType == (int)EmailCategory.CollectionKey)
            //    {
            //_addUsertemplate = template;
            // parsedTemplate = ParseTemplate(_addUsertemplate);
            //    }

            //    else
            //    {
            //        _template = template;
            //        parsedTemplate = ParseTemplate(_template);
            //    }



            //}
            if (!string.IsNullOrEmpty(parsedTemplate))
            {
                try
                {
                    SendResponse response = await _email
                    .SetFrom(sendEmailDto.SenderEmail, sendEmailDto.SenderName)
                    .To(sendEmailDto.ReceiverEmail)
                    .Subject(sendEmailDto.Subject)
                    .UsingTemplate(parsedTemplate, sendEmailDto)
                    .SendAsync();
                    if (response.Successful)
                        return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            return false;

        }

        private string ParseTemplate(string path)
        {
            string result;
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(path))
                using (var reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return result;
        }

        public async Task<bool> EmailFormatter(SendEmailDTO sendEmailDto)
        {
            bool returnValue = false;
            switch (sendEmailDto.EmailCategory)
            {
                case EmailNotificationCategory.LiveLectureAlert:

                    sendEmailDto.Subject = "SignUp";
                    returnValue = await SendMail(sendEmailDto, ParseTemplate(_liveLectureTemplate), 0);
                    //sendEmailDto.Body = string.Format("You have successfully sign-up on, KulPay Platform. Kindly activate your account using "+ sendEmailDto.AccessCode +" to continue.");
                    //sendEmailDto.Subject = "Sign-Up Notification";
                    ////sendEmailDto.CTAUrl = frontEndUrl + "VerifyEmail?guid=" + sendEmailDto.VerificationGuid + "&type=" + sendEmailDto.EmailCategory;
                    //sendEmailDto.ButtonText = "Verify";
                    break;
               
                default:
                    break;
            }

            //if (sendEmailDto.EmailCategory == (int)EmailCategory.InstitutionConfiguration || sendEmailDto.EmailCategory == (int)EmailCategory.CollectionKey)
            //    return await SendMail(sendEmailDto, _configurationtemplate, sendEmailDto.EmailCategory);
            //else
            return returnValue;
        }

    }

}
