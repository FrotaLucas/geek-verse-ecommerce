using GeekVerse.Server.Services.OrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekVerse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //[HttpPost]
        //public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
        //{
        //    var result = await _orderService.PlaceOrder();
        //    return Ok(result);
        //}

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponse>>>> GetOrders()
        {
            var response = await _orderService.GetOrders();
            return Ok(response);
        }

        [HttpGet("{orderId}")]   
        public async Task<ActionResult<ServiceResponse<OrderDetailsResponse>>> GetOrderDetails(int orderId )
        {
            var response = await _orderService.GetOrderDetails(orderId);
            return Ok(response);
        }
    }
}
