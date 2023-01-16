using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetInstitutionUsersDto
    {
        public long PersonId { get; set; }
        public long StudentPersonId { get; set; }
        public string FullName { get; set; }
        public string MatricNumber { get; set; }
        public string email { get; set; }

    }
}
