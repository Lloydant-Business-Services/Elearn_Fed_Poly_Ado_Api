using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ELearnContext _context;

        public PaymentService(ELearnContext context)
        {
            _context = context;
        }


        public async Task<bool> SetupPayment(PaymentSetup setup)
        {
            try
            {
                _context.Add(setup);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeletePaymentSetup(long setupId)
        {
            try
            {
                var setup = await _context.PAYMENT_SETUP.Where(x => x.Id == setupId).FirstOrDefaultAsync();
                _context.Remove(setup);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<PaymentSetup>> PaymentSetupList()
        {
            try
            {
                return await _context.PAYMENT_SETUP.ToListAsync();
         
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaymentSetup> DefaultPaymentSetup()
        {
            try
            {
                return await _context.PAYMENT_SETUP.Where(x => x.Active).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ToggleSetup(long Id)
        {
            try
            {
                var toggleActive = await _context.PAYMENT_SETUP.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (!toggleActive.Active)
                {
                    toggleActive.Active = true;
                    var setups = await _context.PAYMENT_SETUP.Where(x => x.Active && x.Id != toggleActive.Id).ToListAsync();
                    if (setups.Any())
                    {
                        foreach (var item in setups)
                        {
                            item.Active = false;
                            _context.Update(item);
                        }

                        _context.Update(toggleActive);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                      return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateStudentPayment(long userId, string paymentReference, string accessCode, long setupId, int amount)
        {
            try
            {
                var getUser = await _context.USER.Where(x => x.Id == userId).Include(x => x.Person).FirstOrDefaultAsync();
                var activeSessionSemester = await _context.SESSION_SEMESTER.Where(x => x.Active).FirstOrDefaultAsync();
                StudentPerson studentPerson = await _context.STUDENT_PERSON.Where(x => x.PersonId == getUser.Person.Id).FirstOrDefaultAsync(); 
             
                StudentPayment payment = new StudentPayment()
                {
                    SessionSemesterId = activeSessionSemester.Id,
                    PaymentSetupId = setupId,
                    IsPaid = false,
                    PersonId = getUser.PersonId,
                    Created = DateTime.Now,
                    DepartmentId = studentPerson.DepartmentId,
                    SystemPaymentReference = paymentReference,
                    Amount = amount,
                    SystemCode = accessCode
                };
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> VerifyPayment(long userId)
        {
            try
            {
                var getUser = await _context.USER.Where(x => x.Id == userId).Include(x => x.Person).FirstOrDefaultAsync();
                var activeSessionSemester = await _context.SESSION_SEMESTER.Where(x => x.Active).FirstOrDefaultAsync();
                var defaultSetup = await _context.PAYMENT_SETUP.Where(x => x.Active).FirstOrDefaultAsync();
                StudentPayment payment = await _context.STUDENT_PAYMENT.Where(x => x.SessionSemesterId == activeSessionSemester.Id && x.PersonId == getUser.PersonId && x.PaymentSetupId == defaultSetup.Id).FirstOrDefaultAsync();


                return payment?.IsPaid == true ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdatePayment(string paymentReference)
        {
            try
            {
                StudentPayment payment = await _context.STUDENT_PAYMENT.Where(x => x.SystemPaymentReference == paymentReference).FirstOrDefaultAsync();
                payment.IsPaid = true;
                payment.DatePaid = DateTime.Now;
                _context.Update(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SystemPaymentDto>> AllPaymentList()
        {
            try
            {
                return await _context.STUDENT_PAYMENT.Include(x => x.Person).Include(x => x.Department)
                    .Select(f => new SystemPaymentDto()
                    {
                        Id = f.Id,
                        Created = f.Created,
                        DatePaid = f.DatePaid,
                        Fullname = f.Person.Surname + " " + f.Person.Firstname,
                        Department = f.Department.Name,
                        Amount = f.Amount,
                        SystemPaymentReference = f.SystemPaymentReference

                    })
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
