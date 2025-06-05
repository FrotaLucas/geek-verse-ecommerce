using GeekVerse.Server.Services.AuthService;
using GeekVerse.Server.Services.CartService;
using GeekVerse.Server.Services.OrderService;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Stripe;
using Stripe.Checkout;

namespace GeekVerse.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        string secret = "whsec_1895f159d619cbb590aef94de2e7c9bb1d5bf9a7ddd731bcb0dc5d63a6f4eef4";
        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51PfkfMHzpG9NkVZrBe9m6UDc44hNR0yjjF9IvHxwtVXPIItPHccny73NMcDrzEWVjgM9bb7Uimei8O6nwP74vXqq00gTnlyt6N";
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
                        secret
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
