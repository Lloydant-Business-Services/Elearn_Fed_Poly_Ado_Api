using BusinessLayer.Infrastructure;
using BusinessLayer.Interface;
using BusinessLayer.Services.Email.Interface;
using DataLayer.Dtos;
using DataLayer.Enums;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class SchoolAdminService : ISchoolAdminService
    {
        private readonly ELearnContext _context;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly IEmailService _emailService;
        private readonly string defaultPassword = "1234567";

        public SchoolAdminService(ELearnContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");
            _emailService = emailService;


        }

        public async Task<ExcelSheetUploadAggregation> ProcessStudentUpload(IEnumerable<StudentUploadModel> studentList, long departmentId)
        {
            ExcelSheetUploadAggregation uploadAggregation = new ExcelSheetUploadAggregation();
            List<StudentUploadModel> failedUploads = new List<StudentUploadModel>();
            uploadAggregation.SuccessfullUpload = 0;
            uploadAggregation.FailedUpload = 0;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if(studentList.Count() > 0)
                {
                    foreach (StudentUploadModel student in studentList)
                    {
                        var surname = student.Surname.Trim();
                        var firstname = student.Firstname.Trim();
                        var othername = student.Othername.Trim();
                        var matNo = student.MatricNumber.Trim();
                        var email = student.email.Trim();
                        StudentUploadModel failedUploadSingle = new StudentUploadModel();

                        if(string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(matNo))
                        {
                            continue;
                        }
                        var studentPerson = await GetStudentPersonBy(matNo);
                        var personExist = await GetPersonByEmail(email);
                        if (studentPerson == null && personExist == null)
                        {
                            Person person = new Person()
                            {
                                Surname = surname,
                                Firstname = firstname,
                                Othername = othername != null ? othername : null,
                                Email = email
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
                                DepartmentId = departmentId,
                                
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
                            if(person.Email != null)
                            {
                                EmailDto emailDto = new EmailDto()
                                {
                                    ReceiverEmail = person.Email,
                                    ReceiverName = person.Firstname,
                                    Password = defaultPassword,
                                    RegNumber = user.Username,
                                    Subject = "Account Creation Notification",
                                    NotificationCategory = EmailNotificationCategory.AccountAdded
                                    
                                };
                                await _emailService.EmailFormatter(emailDto);
                            }
                           
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
                   await transaction.DisposeAsync();
                    uploadAggregation.FailedStudentUploads = failedUploads;
                    
                }
                return uploadAggregation;
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }


        public async Task<ExcelSheetUploadAggregationInstructor> ProcessStaffUpload(IEnumerable<InstructorUploadModel> staffList, long departmentId)
        {
            ExcelSheetUploadAggregationInstructor uploadAggregation = new ExcelSheetUploadAggregationInstructor();
            List<InstructorUploadModel> failedUploads = new List<InstructorUploadModel>();
            uploadAggregation.SuccessfullUpload = 0;
            uploadAggregation.FailedUpload = 0;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (staffList.Count() > 0)
                {
                    foreach (InstructorUploadModel student in staffList)
                    {
                        var surname = student.Surname.Trim();
                        var firstname = student.Firstname.Trim();
                        //var othername = student.Othername.Trim();
                        var email = student.email.Trim();
                        InstructorUploadModel failedUploadSingle = new InstructorUploadModel();

                        if (string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(email))
                        {
                            continue;
                        }
                        var personExist = await GetPersonByEmail(email);
                        if (personExist == null)
                        {
                            Person person = new Person()
                            {
                                Surname = surname,
                                Firstname = firstname,
                                Othername = null,
                                Email = email
                            };
                            _context.Add(person);
                            await _context.SaveChangesAsync();


                           

                            Utility.CreatePasswordHash(defaultPassword, out byte[] passwordHash, out byte[] passwordSalt);
                            User user = new User()
                            {
                                Username = email,
                                PersonId = person.Id,
                                RoleId = (int)UserRole.Instructor,
                                PasswordHash = passwordHash,
                                PasswordSalt = passwordSalt,
                                IsVerified = true,
                                Active = true

                            };
                            _context.Add(user);
                            await _context.SaveChangesAsync();

                            InstructorDepartment instructorDepartment = new InstructorDepartment()
                            {
                                DepartmentId = departmentId,
                                UserId = user.Id,
                                Active = true
                            };
                            _context.Add(instructorDepartment);
                            await _context.SaveChangesAsync();

                            uploadAggregation.SuccessfullUpload += 1;
                            if (person.Email != null)
                            {
                                EmailDto emailDto = new EmailDto()
                                {
                                    ReceiverEmail = person.Email,
                                    ReceiverName = person.Firstname,
                                    Password = defaultPassword,
                                    RegNumber = user.Username,
                                    Subject = "Account Creation Notification",
                                    NotificationCategory = EmailNotificationCategory.AccountAdded

                                };
                                await _emailService.EmailFormatter(emailDto);
                            }

                        }
                        //Already exists
                        else
                        {
                            failedUploadSingle.Surname = surname;
                            failedUploadSingle.Firstname = firstname;
                            failedUploadSingle.MatricNumber = email;
                            failedUploads.Add(failedUploadSingle);
                            uploadAggregation.FailedUpload += 1;
                        }

                    }
                    await transaction.CommitAsync();
                    uploadAggregation.FailedInstructorUploads = failedUploads;

                }
                return uploadAggregation;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public async Task<bool> AddSingleStudent(StudentUploadModel student, long departmentId)
        {
            ExcelSheetUploadAggregation uploadAggregation = new ExcelSheetUploadAggregation();
            List<StudentUploadModel> failedUploads = new List<StudentUploadModel>();
            uploadAggregation.SuccessfullUpload = 0;
            uploadAggregation.FailedUpload = 0;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
               
                        var surname = student.Surname.Trim();
                        var firstname = student.Firstname.Trim();
                        var othername = student.Othername.Trim();
                        var matNo = student.MatricNumber.Trim();
                        var email = student.email.Trim();
                        StudentUploadModel failedUploadSingle = new StudentUploadModel();

                       
                        var studentPerson = await GetStudentPersonBy(matNo);
                        var studentPersonByEmail = await GetPersonByEmail(email);
                
                        if (studentPerson == null && studentPersonByEmail == null)
                        {
                            Person person = new Person()
                            {
                                Surname = surname,
                                Firstname = firstname,
                                Othername = othername != null ? othername : null,
                                Email = email
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
                                DepartmentId = departmentId,

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

                            if (person.Email != null)
                            {
                                EmailDto emailDto = new EmailDto()
                                {
                                    ReceiverEmail = person.Email,
                                    ReceiverName = person.Firstname,
                                    Password = defaultPassword,
                                    RegNumber = user.Username,
                                    Subject = "Account Creation Notification",
                                    NotificationCategory = EmailNotificationCategory.AccountAdded

                                };
                                await _emailService.EmailFormatter(emailDto);
                                await transaction.CommitAsync();

                            }
                            return true;
                        }
                       

                return false;
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

        public async Task<Person> GetPersonByEmail(string email)
        {
            try
            {
                return await _context.PERSON.Where(x => x.Email.ToLower() == email.ToLower())
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<GetInstitutionUsersDto>> GetAllStudents()
        {
            return await _context.STUDENT_PERSON.Where(a => a.Id > 0 && a.Active)
                .Include(p => p.Person)
                .Select(f => new GetInstitutionUsersDto
                {
                    FullName = f.Person.Surname + " " + f.Person.Firstname + " " + f.Person.Othername,
                    MatricNumber = f.MatricNo,
                    PersonId = f.PersonId,
                    StudentPersonId = f.Id
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<GetInstitutionUsersDto>> GetStudentsDepartmentId(long DepartmentId)
        {
            return await _context.STUDENT_PERSON.Where(a => a.DepartmentId == DepartmentId && a.Active)
                .Include(p => p.Person)
                .Select(f => new GetInstitutionUsersDto
                {
                    FullName = f.Person.Surname + " " + f.Person.Firstname + " " + f.Person.Othername,
                    MatricNumber = f.MatricNo,
                    PersonId = f.PersonId,
                    StudentPersonId = f.Id

                })
                .ToListAsync();
        }
        public async Task<bool> DeleteStudent(long studentPersonId)
        {
            try
            {
                var student = await _context.STUDENT_PERSON.Where(a => a.Id == studentPersonId && a.Active).FirstOrDefaultAsync();
                var user = await _context.USER.Where(a => a.PersonId == student.PersonId).FirstOrDefaultAsync();
                student.Active = false;
                user.Active = false;
                _context.Update(student);
                _context.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<GeneralAudit>> GetAudits()
        {
            return await _context.GENERAL_AUDIT.OrderBy(x => x.ActionTime).Include(x => x.User).ThenInclude(x => x.Person).Where(x => x.Id > 0).ToListAsync(); 
        }
        public async Task<DetailCountDto> InstitutionDetailCount()
        {
            DetailCountDto countDto = new DetailCountDto();
            var studentCount = await _context.STUDENT_PERSON.Where(d => d.Id > 0).CountAsync();
            var instructorCount = await _context.COURSE_ALLOCATION.Where(d => d.Id > 0).CountAsync();
            var departmentCount = await _context.DEPARTMENT.Where(d => d.Id > 0).CountAsync();
            countDto.AllDepartments = departmentCount;
            countDto.AllStudents = studentCount;
            countDto.AllInstructors = instructorCount;
            return countDto;
        }

        //public async Task<CourseMaterialDto> GetCourseMaterialsBy(long departmentId, long courseId, long instructorId)
        //{
        //    SchoolAdminStatsDto summary = new SchoolAdminStatsDto();
        //    var courseMaterials = await _context.COURSE_CONTENT.Where(x => x.CourseTopic.CourseAllocation.Instructor)
        //}
    }
}
