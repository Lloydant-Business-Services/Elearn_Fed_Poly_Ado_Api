using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IFileUpload
    {
        Task<string> GetNoteUploadLink(IFormFile file, string filePath, string directory, string givenFileName);
    }
}
