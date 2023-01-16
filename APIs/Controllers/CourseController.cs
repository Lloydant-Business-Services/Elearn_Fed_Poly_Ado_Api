using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APIs.Middleware;
using BusinessLayer.Infrastructure;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpPost("AddCourses")]
        public async Task<ResponseModel> PostCourse(AddCourseDto courseDto) => await _service.AddCourse(courseDto);
        [HttpPut("UpdateCourse")]
        public async Task<ResponseModel> UpdateCourseDetail(AddCourseDto dto) => await _service.UpdateCourseDetail(dto);
        [HttpGet("GetAllocatedCourses")]
        public async Task<IEnumerable<AddCourseDto>> GetStudentCourses() => await _service.GetStudentCourses();
        [HttpGet("GetAllocatedCoursesByDepartmentAlt")]
        public async Task<IEnumerable<AddCourseDto>> GetStudentCoursesByDepartment(long departmentId) => await _service.GetStudentCoursesByDepartment(departmentId);
        [HttpGet("GetAllCourses")]
        public async Task<IEnumerable<AllCoursesDto>> GetCourses() => await _service.GetCourses();
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetDepartmentCourseDto>> GetDepartmentalCourses(long departmentId) => await _service.GetDepartmentalCourses(departmentId);
        [HttpGet("[action]")]
        public async Task<AllCoursesDto> GetCourseByCourseId(long courseId) => await _service.GetCourseByCourseId(courseId);
        [HttpGet("[action]")]
        public async Task<AllCoursesDto> AllocatedCourseByAllocationId(long courseAllocationId) => await _service.AllocatedCourseByAllocationId(courseAllocationId);
        [AuthorizeRole(ElearnRole.SCHOOLADMIN, ElearnRole.INSTRUCTOR, ElearnRole.HOD, ElearnRole.SERVICE_ADMIN)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetDepartmentCourseDto>> GetAllocatedCoursesByDepartment(long departmentId) => await _service.GetAllocatedCoursesByDepartment(departmentId);

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetDepartmentCourseDto>> GetAllocatedServiceCourses() => await _service.GetAllocatedServiceCourses();
        [HttpPost("BulkCourseUpload")]
        public async Task<CoursePloadSheetAggregation> UploadCourse(IFormFile file, long userId)
        {
            CoursePloadSheetAggregation uploadAggregation = new CoursePloadSheetAggregation();
            try
            {
                long size = file.Length;
                //
                if (size > 0)
                {
                    List<CourseUploadModel> courseList = new List<CourseUploadModel>();

                    var filePath = Path.GetTempFileName();
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                        ExcelPackage package = new ExcelPackage(stream);
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null)
                        {
                            //two rows space from the top to allow for the headers
                            int totalRows = worksheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {
                                CourseUploadModel courseDetail = new CourseUploadModel();
                                int serialNumber = Convert.ToInt32(worksheet.Cells[i, 1].Value);
                                courseDetail.CourseTitle = worksheet.Cells[i, 2].Value != null ? worksheet.Cells[i, 2].Value.ToString() : " ";
                                courseDetail.CourseCode = worksheet.Cells[i, 3].Value != null ? worksheet.Cells[i, 3].Value.ToString() : " ";

                                courseList.Add(courseDetail);
                            }

                            if (courseList?.Count() > 0)
                            {

                                uploadAggregation = await _service.ProcessBulkCourseUpload(courseList, userId);
                            }
                        }
                        else
                        {
                            return uploadAggregation;
                        }
                    }

                    return uploadAggregation;
                }

                throw new NullReferenceException("Invalid Upload Sheet");
            }
            catch (Exception ex) { throw ex; }
        }

        //[HttpPost("[action]")]
        //public async Task<ResponseModel> AllocateCourse(AllocateCourseDto dto) => await _service.AllocateCourse(dto);
    }
}
