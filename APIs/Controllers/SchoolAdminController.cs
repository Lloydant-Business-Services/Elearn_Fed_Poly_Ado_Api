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
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace APIs.Controllers
{
    //[AuthorizeRole(ElearnRole.SCHOOLADMIN, ElearnRole.INSTRUCTOR, ElearnRole.HOD)]
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolAdminController : ControllerBase
    {
        private readonly ISchoolAdminService _service;
        private readonly IConfiguration _configuration;
        private readonly string key;
        public SchoolAdminController(ISchoolAdminService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
            key = _configuration.GetValue<string>("AppSettings:Key");

        }
        [HttpPost("StudentExcelUpload")]
        public async Task<ExcelSheetUploadAggregation> UploadStudentData(IFormFile file, long departmentId)
        {
            ExcelSheetUploadAggregation uploadAggregation = new ExcelSheetUploadAggregation();
            try
            {
                long size = file.Length;
                //
                if (size > 0)
                {
                    List<StudentUploadModel> studentList = new List<StudentUploadModel>();

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
                                StudentUploadModel studentDetail = new StudentUploadModel();
                                int serialNumber = Convert.ToInt32(worksheet.Cells[i, 1].Value);
                                studentDetail.MatricNumber = worksheet.Cells[i, 2].Value != null ? worksheet.Cells[i, 2].Value.ToString() : " ";
                                studentDetail.Surname = worksheet.Cells[i, 3].Value != null ? worksheet.Cells[i, 3].Value.ToString() : " ";
                                studentDetail.Firstname = worksheet.Cells[i, 4].Value != null ? worksheet.Cells[i, 4].Value.ToString() : " ";
                                studentDetail.Othername = worksheet.Cells[i, 5].Value != null ? worksheet.Cells[i, 5].Value.ToString() : " ";
                                studentDetail.email = worksheet.Cells[i, 6].Value != null ? worksheet.Cells[i, 6].Value.ToString() : " ";

                                studentList.Add(studentDetail);
                            }

                            if (studentList?.Count() > 0)
                            {

                                uploadAggregation = await _service.ProcessStudentUpload(studentList, departmentId);
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

        [HttpPost("InstructorUpload")]
        public async Task<ExcelSheetUploadAggregationInstructor> UploadInstructorData(IFormFile file, long departmentId)
        {
            ExcelSheetUploadAggregationInstructor uploadAggregation = new ExcelSheetUploadAggregationInstructor();
            try
            {
                long size = file.Length;
                //
                if (size > 0)
                {
                    List<InstructorUploadModel> instructorList = new List<InstructorUploadModel>();

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
                                InstructorUploadModel studentDetail = new InstructorUploadModel();
                                int serialNumber = Convert.ToInt32(worksheet.Cells[i, 1].Value);
                                studentDetail.Surname = worksheet.Cells[i, 2].Value != null ? worksheet.Cells[i, 2].Value.ToString() : " ";
                                studentDetail.Firstname = worksheet.Cells[i, 3].Value != null ? worksheet.Cells[i, 3].Value.ToString() : " ";
                                studentDetail.email = worksheet.Cells[i, 4].Value != null ? worksheet.Cells[i, 4].Value.ToString() : " ";

                                instructorList.Add(studentDetail);
                            }

                            if (instructorList?.Count() > 0)
                            {

                                uploadAggregation = await _service.ProcessStaffUpload(instructorList, departmentId);
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

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstitutionUsersDto>> GetAllStudents() => await _service.GetAllStudents();
        [HttpGet("[action]")]
        public async Task<DetailCountDto> InstitutionDetailCount() => await _service.InstitutionDetailCount();
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstitutionUsersDto>> GetStudentsDepartmentId(long DepartmentId) => await _service.GetStudentsDepartmentId(DepartmentId);
        [HttpPost("[action]")]
        public async Task<bool> DeleteStudent(long studentPersonId) => await _service.DeleteStudent(studentPersonId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GeneralAudit>> GetAudits() => await _service.GetAudits();
        [HttpPost("[action]")]
        public async Task<bool> AddSingleStudent(StudentUploadModel student, long departmentId) => await _service.AddSingleStudent(student, departmentId);
    }
}
