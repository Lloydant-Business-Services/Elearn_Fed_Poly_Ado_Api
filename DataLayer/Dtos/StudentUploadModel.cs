using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class StudentUploadModel
    {
        public string MatricNumber { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Othername { get; set; }
        public string email { get; set; }
    }

    public class InstructorUploadModel
    {
        public string MatricNumber { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Othername { get; set; }
        public string email { get; set; }
    }
}
