using BusinessLayer.Interface;
using BusinessLayer.Services.PaystackTransaction.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaystackService _paystackService;
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaystackService paystackService, IPaymentService paymentService)
        {
            _paystackService = paystackService;
            _paymentService = paymentService;
        }
        [HttpPost("InitializeTransaction")]
        public async Task<IActionResult> InitializeTransaction(long userId, long setupId)
        {
            return Ok(await _paystackService.InitializeTransaction(userId, setupId));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SetupPayment(PaymentSetup setup)
        {
            return Ok(await _paymentService.SetupPayment(setup));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PaymentSetupList()
        {
            return Ok(await _paymentService.PaymentSetupList());
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ToggleSetup(long Id)
        {
            return Ok(await _paymentService.ToggleSetup(Id));
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdatePayment(string reference)
        {
            return Ok(await _paymentService.UpdatePayment(reference));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> AllPaymentList()
        {
            return Ok(await _paymentService.AllPaymentList());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DefaultPaymentSetup()
        {
            return Ok(await _paymentService.DefaultPaymentSetup());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteSetup(long setupId)
        {
            return Ok(await _paymentService.DeletePaymentSetup(setupId));
        }
    }
}
