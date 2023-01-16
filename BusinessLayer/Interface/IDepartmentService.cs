using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IDepartmentService:IRepository<Department>
    {
        Task<ResponseModel> AddDepartment(DepartmentDto model);
        Task<ResponseModel> UpdateDepartment(DepartmentDto model);
        Task<ResponseModel> DeleteDepartment(long id);
        Task<IEnumerable<GetDepartmentHeadDto>> GetDepartmentHeadsByFaculty(long facultyId);
        Task<GetDepartmentHeadDto> GetDepartmentHeadByDepartmentId(long departmentId);
        Task<ResponseModel> AssignDepartmentHead(AddDepartmentHeadDto dto);
        Task<IEnumerable<GetDepartmentHeadDto>> GetAllDepartmentHeads();
        Task<IEnumerable<DepartmentDto>> GetDepartmentsByFacultyId(long facultyId, bool isAdmin);
    }
}
