using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class SubAdminDelegationDto
    {
        public string Name { get; set; }
        public long UserId { get; set; }
        public List<GetDelegationsDto> DelegationsDtos { get; set; }
    }
  
    public class GetDelegationsDto
    {
        public long DelegationId { get; set; }
        public TaskDelegations TaskId { get; set; }
    }
}
