using DataLayer.Model;
using PayStackDotNetSDK.Models.Banks;
using PayStackDotNetSDK.Models.SubAccounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.PaystackTransaction.Interface
{
    public interface IPaystackService
    {
        Task<PaymentInitalizationResponseModel> InitializeTransaction(long userId, long setupId);
        Task<PaymentInitalizationResponseModel> InitializeTransaction();
        Task<SubAccountResponseModel> CreateSubAccount(PayStackSubaccount subAccount);
        Task<BankListResponseModel> ListBanks();
    }
}
