using GeekVerse.Server.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.BillingPortal;

namespace GeekVerse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout"), Authorize]
        public async Task<ActionResult<string>> CreateCheckoutSession()
        {
            var session = await _paymentService.CreateCheckoutSession();
            //Essa Url aqui representa a rota de pagamento do stripe. Ali o Cliente consome essa api, pega a string e joga no navegador
            return Ok(session.Url);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder()
        {
          var response = await _paymentService.FulfillOrder(Request);
          if(!response.Success)
          {
            return BadRequest(response.Message);
          }
            return Ok(response);
            
        }
    }
}
