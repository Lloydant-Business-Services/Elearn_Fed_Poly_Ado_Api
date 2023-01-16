using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class ResponseModel
    {
        public long StatusCode { get; set; } = StatusCodes.Status200OK;
        public string Message { get; set; } = "success";
        public bool Status { get; set; }
    }
}
