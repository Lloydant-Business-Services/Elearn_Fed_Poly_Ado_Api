using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Email.Interface
{
    public interface IEmailServiceAlt
    {
        Task<bool> SendMail(SendEmailDTO sendEmailDto, string template, int templateType);
        Task<bool> EmailFormatter(SendEmailDTO sendEmailDto);
    }
}
