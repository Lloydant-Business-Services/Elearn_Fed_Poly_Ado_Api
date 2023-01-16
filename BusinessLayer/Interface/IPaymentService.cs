using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IPaymentService
    {
        Task<bool> SetupPayment(PaymentSetup setup);
        Task<IEnumerable<PaymentSetup>> PaymentSetupList();
        Task<bool> ToggleSetup(long Id);
        Task<bool> CreateStudentPayment(long userId, string paymentReference, string accessCode, long setupId, int amount);
        Task<bool> UpdatePayment(string paymentReference);
        Task<IEnumerable<SystemPaymentDto>> AllPaymentList();
        Task<PaymentSetup> DefaultPaymentSetup();
        Task<bool> VerifyPayment(long userId);
        Task<bool> DeletePaymentSetup(long setupId);

    }       
}
