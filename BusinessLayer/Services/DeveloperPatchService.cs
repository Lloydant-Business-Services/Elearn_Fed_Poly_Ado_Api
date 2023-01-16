using BusinessLayer.Infrastructure;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class DeveloperPatchService : IDeveloperPatchService
    {
        private readonly ELearnContext _context;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly string defaultPassword = "1234567";

        public DeveloperPatchService(ELearnContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }

        public async Task<ExcelSheetUploadAggregation> ProcessAPIData(IEnumerable<StudentUploadModel> studentList, long departmentId)
        {
            ExcelSheetUploadAggregation uploadAggregation = new ExcelSheetUploadAggregation();
            List<StudentUploadModel> failedUploads = new List<StudentUploadModel>();
            uploadAggregation.SuccessfullUpload = 0;
            uploadAggregation.FailedUpload = 0;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (studentList.Count() > 0)
                {
                    foreach (StudentUploadModel student in studentList)
                    {
                        var surname = student.Surname.Trim();
                        var firstname = student.Firstname.Trim();
                        var othername = student.Othername.Trim();
                        var matNo = student.MatricNumber.Trim();
                        StudentUploadModel failedUploadSingle = new StudentUploadModel();



                        var studentPerson = await GetStudentPersonBy(matNo);
                        if (studentPerson == null)
                        {
                            Person person = new Person()
                            {
                                Surname = surname,
                                Firstname = firstname,
                                Othername = othername != null ? othername : null
                            };
                            _context.Add(person);
                            await _context.SaveChangesAsync();

                            var mat_no_slug = Utility.GenerateSlug(matNo);
                            StudentPerson _student_person = new StudentPerson()
                            {
                                MatricNo = matNo,
                                PersonId = person.Id,
                                MatricNoSlug = mat_no_slug,
                                Active = true,
                                DepartmentId = departmentId
                            };
                            _context.Add(_student_person);
                            await _context.SaveChangesAsync();

                            Utility.CreatePasswordHash(defaultPassword, out byte[] passwordHash, out byte[] passwordSalt);
                            User user = new User()
                            {
                                Username = matNo,
                                PersonId = person.Id,
                                RoleId = (int)UserRole.Student,
                                PasswordHash = passwordHash,
                                PasswordSalt = passwordSalt,
                                IsVerified = true,
                                Active = true

                            };
                            _context.Add(user);
                            await _context.SaveChangesAsync();

                            uploadAggregation.SuccessfullUpload += 1;
                        }
                        //Already exists
                        else
                        {
                            failedUploadSingle.Surname = surname;
                            failedUploadSingle.Firstname = firstname;
                            failedUploadSingle.Othername = othername;
                            failedUploadSingle.MatricNumber = matNo;
                            failedUploads.Add(failedUploadSingle);
                            uploadAggregation.FailedUpload += 1;
                        }
                    }
                    await transaction.CommitAsync();
                    uploadAggregation.FailedStudentUploads = failedUploads;

                }
                return uploadAggregation;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
        public async Task<StudentPerson> GetStudentPersonBy(string MatricNo)
        {
            try
            {
                var matNoSlug = Utility.GenerateSlug(MatricNo);
                return await _context.STUDENT_PERSON.Where(x => x.MatricNoSlug == matNoSlug).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<StudentUploadModel>> MockAPIData()
        {
            List<StudentUploadModel> mockResponse = new List<StudentUploadModel>();
            mockResponse.Add(new StudentUploadModel { Surname = "Oghenekevwe", Firstname="Akpos" , Othername = "Eliot", MatricNumber ="ABSU/AC/00212"});
            mockResponse.Add(new StudentUploadModel { Surname = "Oghenechavwuko", Firstname="Miracle" , Othername = "Godspeed", MatricNumber ="ABSU/AC/00213"});
            mockResponse.Add(new StudentUploadModel { Surname = "Anyanwu", Firstname="Ifeanyi" , Othername = "Michael", MatricNumber ="ABSU/AC/00214"});
            mockResponse.Add(new StudentUploadModel { Surname = "Chimaobi", Firstname="Okeke" , Othername = "Festus", MatricNumber ="ABSU/AC/00215"});
            mockResponse.Add(new StudentUploadModel { Surname = "Nwaogu", Firstname="Jason" , Othername = "Kingdom", MatricNumber ="ABSU/AC/00216"});
            mockResponse.Add(new StudentUploadModel { Surname = "Nnebedum", Firstname="Kosi" , Othername = "Maryann", MatricNumber ="ABSU/AC/00217"});
            mockResponse.Add(new StudentUploadModel { Surname = "Asiwaju", Firstname="Femi" , Othername = "Joe", MatricNumber ="ABSU/AC/00218"});
            mockResponse.Add(new StudentUploadModel { Surname = "Ihetu", Firstname="Ikechukwu" , Othername = "Emmanuel", MatricNumber ="ABSU/AC/00218"});
            mockResponse.Add(new StudentUploadModel { Surname = "Ojo", Firstname="Promise" , Othername = "Oghenevwaire", MatricNumber ="ABSU/AC/00219"});
            mockResponse.Add(new StudentUploadModel { Surname = "Oghenemado", Firstname="Nelson" , Othername = "Onome", MatricNumber ="ABSU/AC/00219"});

            return mockResponse;
        }
    }
}
