using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class DeleteRecordDto
    {
        public long RecordId { get; set; }
        public bool Delete { get; set; }
    }
}
