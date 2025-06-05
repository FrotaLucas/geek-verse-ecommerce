using GeekVerse.Server.Services.AuthService;
using GeekVerse.Server.Services.CartService;
using System.Security.Claims;

namespace GeekVerse.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        public readonly DataContext _context;
        public readonly ICartService _cartService;
        public readonly IAuthService _authService;
        public OrderService(DataContext context, ICartService cartService, IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsResponse>();

            var order = await _context.Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();
            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found";
                return response;
            }

            var orderDetailsResponse = new OrderDetailsResponse()
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductResponse>()
            };

            order.OrderItems.ForEach(item => orderDetailsResponse.Products.Add(new OrderDetailsProductResponse
            {
                ProductId = item.ProductId,
                Title = item.Product.Title,
                ImageUrl = item.Product.ImageUrl,
                ProductType = item.ProductType.Name,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
            })
            );

            response.Data = orderDetailsResponse;
            return response;

        }

        public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewResponse>>();
            var orders = await _context.Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse()
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title}" +
                " and " + $"{o.OrderItems.Count} more" : $"{o.OrderItems.First().Product.Title}",
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl,

            }));

            response.Data = orderResponse;

            return response;
        }

        //private int GetUserId() => int.Parse(_httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
        {

            //Console.WriteLine("UserId" + _authService.GetUserId());

            var products = (await _cartService.GetDbCartProducts(userId)).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();

            //criando uma lista de OrderItems
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity,

            }));

            //criando uma Order com UsedId e fazendo a conexao com OrderItems. 
            //Como a conexao estabelecida, ao salvar na tabela Orders, a tabela OrderItems automaticamente
            //tambem vai ser preenchida!!
            var order = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            await _context.Orders.AddAsync(order);

            //depois de fazer pedido, uma ordem eh criada na tabela Orders e o carrinho eh entao deletado da tabela CarttItems
            _context.CartItems.RemoveRange(_context.CartItems
                .Where(ci => ci.UserId == userId));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true
            };
        }
    }
}
