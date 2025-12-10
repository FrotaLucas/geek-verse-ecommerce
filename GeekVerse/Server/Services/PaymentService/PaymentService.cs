using GeekVerse.Server.Services.AuthService;
using GeekVerse.Server.Services.CartService;
using GeekVerse.Server.Services.OrderService;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Configuration;

namespace GeekVerse.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        private readonly string _webHookSecret;
        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService, IConfiguration configuration)
        {
            _webHookSecret = configuration["Stripe:WebhookSecret"];

            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
            _authService = authService;
            _cartService = cartService;
            _orderService = orderService;
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();

            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> {"DE","FR","IT","ES","EG"}
                },
                PaymentMethodTypes = new List<string>
                {
                    "card",
                    "sepa_debit",
                    "paypal"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7003/order-success",
                CancelUrl = "https://localhost:7003/cart",
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        //deposi de clicar finaliza sua compra e clica em Pay, o stripe gera um evento chamado
        //CheckoutSessionCompleted. O ClI do stripe precisa esta logado e escutando esse evento para
        //entao a api FulfillOrder ser chamada e o metodo PlaceOrder dentro do corpo dela tb ser trigado.
        //Placeholder vai salvar uma ordem de compra na tabela Orders.
        //Depois de uar o comando stripe login, pecisamos instruir o CLI para ficar escutando a os eventos que acontecem
        //usar comando stripe listen --forward-to https://localhost:7003/api/payment
        //Depois do evento CheckoutSessionCompleted ser trigado, o Stripe enviar um Post Request para meu servidor
        //poder chamar minha api FulfillOrder
        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        _webHookSecret
                    );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };
            }
        }
    }
}
