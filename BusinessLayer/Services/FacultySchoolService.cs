using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class FacultySchoolService : Repository<FacultySchool>, IFacultyService
    {
        ResponseModel response = new ResponseModel();
        public FacultySchoolService(ELearnContext context) : base(context) { }

        public async Task<ResponseModel> AddFacultySchool(FacultyDto model)
        {
            try
            {
                FacultySchool facultySchool = new FacultySchool();
                var f_slug = Utility.GenerateSlug(model.Name);
                //var doesExist = await _context.FACULTY_SCHOOL.Where(f => f.slug == f_slug).FirstOrDefaultAsync();
                //if (doesExist != null)
                //{
                //    response.StatusCode = StatusCodes.Status208AlreadyReported;
                //    response.Message = "faculty/school already exist";
                //    return response;
                //}
                   
                facultySchool.Name = model.Name;
                facultySchool.slug = f_slug;
                facultySchool.Active = true;
                facultySchool.DateCreated = DateTime.Now;
               _context.Add(facultySchool);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseModel> UpdateFacultySchool(FacultyDto model)
        {
            try
            {
                FacultySchool facultySchool = await _context.FACULTY_SCHOOL.Where(f => f.Id == model.Id).FirstOrDefaultAsync();
                if (facultySchool == null)
                    throw new NullReferenceException("Faculty not found");
                var f_slug = Utility.GenerateSlug(model.Name);
                facultySchool.Name = model.Name;
                facultySchool.slug = f_slug;
                facultySchool.Active = true;
                _context.Update(facultySchool);
                await _context.SaveChangesAsync();
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseModel> DeleteFacultySchool(long id)
        {
            try
            {
                FacultySchool facultySchool = await _context.FACULTY_SCHOOL.Where(f => f.Id == id).FirstOrDefaultAsync();
                if(facultySchool != null)
                {
                    facultySchool.Active = facultySchool.Active ? false : true;
                    _context.Update(facultySchool);
                    await _context.SaveChangesAsync();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Message = "deleted";
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
