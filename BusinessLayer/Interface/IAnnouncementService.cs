using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAnnouncementService
    {
        Task<int> AddAnnouncement(AddAnnouncementDto dto);
        Task<IEnumerable<GetAnnouncementDto>> GetAnnouncement(long departmentId);
    }
}
