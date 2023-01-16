using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ISessionSemesterService
    {
        Task<ResponseModel> SetSessionSemester(long sessionId, long semesterId, long userId);
        Task<GetSessionSemesterDto> GetActiveSessionSemester();
        Task<List<GetSessionSemesterDto>> GetALLSessionSemester();
    }
}
