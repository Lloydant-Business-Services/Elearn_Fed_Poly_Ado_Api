using BusinessLayer.Helpers;
using BusinessLayer.Interface;
using BusinessLayer.Services.PaystackTransaction.Interface;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PayStackDotNetSDK.Models.Banks;
using PayStackDotNetSDK.Models.SubAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.PaystackTransaction
{
    public class PaystackService : IPaystackService
    {
        public string _secretKey { get; set; } = "sk_test_aa314329dfa7e67f451260d712418dfa7b36e11b";
        //public string _secretKey { get; set; } = "sk_test_1ec0456a42753f0171919cb2247a8a2dd33121e4";
        public static string BaseUrl { get; set; } = "https://api.paystack.co/";

        private readonly ELearnContext _context;
        private readonly IPaymentService _paymentService;

        //private string _secretKey;
        public PaystackService(ELearnContext context, IPaymentService paymentService)
        {
            //_secretKey = secretKey;
            _context = context;
            _paymentService = paymentService;
        }
        public async Task<PaymentInitalizationResponseModel> InitializeTransaction(long userId, long setupId)
        {
            //string url = GetUrl("initialize");
            var getUser = await _context.USER.Where(x => x.Id == userId).Include(x => x.Person).FirstOrDefaultAsync();
            var activeSessionSemester = await _context.SESSION_SEMESTER.Where(x => x.Active).FirstOrDefaultAsync();
            var isGenerated = await _context.STUDENT_PAYMENT.Where(x => x.PersonId == getUser.PersonId && x.SessionSemesterId == activeSessionSemester.Id && x.PaymentSetupId == setupId).FirstOrDefaultAsync();
            var defaultSetup = await _context.PAYMENT_SETUP.Where(x => x.Active).FirstOrDefaultAsync();
            if (defaultSetup == null) return null;
            if(isGenerated != null)
            {
                SubData subData = new SubData()
                {
                    authorization_url = "https://checkout.paystack.com/" + isGenerated.SystemCode,
                };
                PaymentInitalizationResponseModel model = new PaymentInitalizationResponseModel()
                {
                    data = subData,
                    message = "success"
                };
                return model;
            }
            var values = new Dictionary<string, string>
                {
                   { "email", getUser.Person.Email },
                   { "amount", $"{defaultSetup.Amount}" },
                   { "personId", $"{getUser.PersonId}" },
                   { "setupId", $"{setupId}" },
                };
            string url = GetUrl("transaction/initialize");
            var content = await BaseClient.PostEntities(url, GetContent(values), _secretKey);
            var res = JsonConvert.DeserializeObject<PaymentInitalizationResponseModel>(content);
            if (isGenerated == null && res.status)
            {
                await _paymentService.CreateStudentPayment(userId, res.data.reference, res.data.access_code, defaultSetup.Id, defaultSetup.Amount);
            }
            return res;
        }

        static string GetUrl(String url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return BaseUrl;
            return $"{BaseUrl}{url}";
        }

        //static string GetUrl(String url)
        //{
        //    if (string.IsNullOrWhiteSpace(url))
        //        return "transaction";
        //    return $"transaction/{url}";
        //}

        static String GetContent(Dictionary<string, string> values)
        {
            return JsonConvert.SerializeObject(values);
        }

        public async Task<PaymentInitalizationResponseModel> InitializeTransaction()
        {
            TransactionRequestDto data = new TransactionRequestDto();
            data.amount = 20000;
            data.email = "miracleoghenemado@gmail.com";

            data.split.type = "percentage";
            data.split.bearer_type = "account";

            data.split.subaccounts.Add(new Subaccount { subaccount = "ACCT_idbp4bdch3gleav", share = 40 });
            data.split.subaccounts.Add(new Subaccount { subaccount = "ACCT_mhqnr5zwmo6etz9", share = 20 });

            string url = GetUrl("transaction/initialize");
            var content = await BaseClient.PostEntities(url, JsonConvert.SerializeObject(data), this._secretKey);
            return JsonConvert.DeserializeObject<PaymentInitalizationResponseModel>(content);
        }

        public async Task<SubAccountResponseModel> CreateSubAccount(PayStackSubaccount subAccount)
        {
            string url = GetUrl("subaccount");
            var content = await BaseClient.PostEntities(url, JsonConvert.SerializeObject(subAccount), this._secretKey);
            return JsonConvert.DeserializeObject<SubAccountResponseModel>(content);
        }

        public async Task<BankListResponseModel> ListBanks()
        {
            var client = HttpConnection.CreateClient(this._secretKey);

            var response = await client.GetAsync("https://api.paystack.co/bank");

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BankListResponseModel>(json);
        }
    }

}
