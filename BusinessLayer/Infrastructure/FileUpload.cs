using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Infrastructure
{
    public class FileUpload : IFileUpload
    {
        public async Task<string> GetNoteUploadLink(IFormFile file, string filePath, string directory, string givenFileName)
        {

            var noteUrl = string.Empty;
            //Define allowed property of the uploaded file

            var validFileSize = (1024 * 1024);//1mb
            List<string> validFileExtension = new List<string>();
            validFileExtension.Add(".pdf");
            validFileExtension.Add(".doc");
            validFileExtension.Add(".docx");
            validFileExtension.Add(".xlx");
            validFileExtension.Add(".xlxs");
            validFileExtension.Add(".docx");
            validFileExtension.Add(".ppt");
            if (file.Length > 0)
            {

                var extType = Path.GetExtension(file.FileName);
                var fileSize = file.Length;
                if (fileSize <= validFileSize)
                {

                    if (validFileExtension.Contains(extType))
                    {
                        string fileName = string.Format("{0}{1}", givenFileName + "_" + DateTime.Now.Millisecond, extType);
                        //create file path if it doesnt exist
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fullPath = Path.Combine(filePath, fileName);
                        noteUrl = Path.Combine(directory, fileName);
                        //Delete if file exist
                        FileInfo fileExists = new FileInfo(fullPath);
                        if (fileExists.Exists)
                        {
                            fileExists.Delete();
                        }

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        return noteUrl = noteUrl.Replace('\\', '/');





                    }
                    else
                    {
                        throw new BadImageFormatException("File format is not supported");
                    }
                }
            }
            return noteUrl;
        }

        //public async Task<string> UploadDocument(IFormFile file, string filePath, string directory, string givenFileName)
       
    }
}
