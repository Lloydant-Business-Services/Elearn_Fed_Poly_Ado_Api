using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IFacultyService:IRepository<FacultySchool>
    {
        Task<ResponseModel> UpdateFacultySchool(FacultyDto model);
        Task<ResponseModel> AddFacultySchool(FacultyDto model);
        Task<ResponseModel> DeleteFacultySchool(long id);
    }
}
