using DataLayer.Dtos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Email.Interface
{
    public interface IEmailService
    {
        //Task EmailFormatter(EmailDto sendEmailDto);
        Task<bool> EmailFormatter(EmailDto dto);
    }
}
