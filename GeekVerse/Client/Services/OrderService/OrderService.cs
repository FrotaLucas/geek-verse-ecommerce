using Microsoft.AspNetCore.Components;

namespace GeekVerse.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authStateProvider;
        public OrderService(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient)
        {
            _authStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
            _http = httpClient;

        }
        //SUSBSTITUIR ESSE METODO IsUserAuthenticated por servico do AuthService !! Injetar AuthService 
        //por dependencias aqui no construtor dessa classe
        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
        public async Task<string> PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                //Depois que o PlaceOrder eh chamado,a tabela CartItems do banco de dados eh zerada 

                //Verificar metodo _context.CartItems.RemoveRange dentro da api PlaceOrder
                //var response = await _http.PostAsync("api/order", null);

                var response = await _http.PostAsync("api/payment/checkout", null);
                string url = await response.Content.ReadAsStringAsync();
                return url;
            }
            else
            {
                //_navigationManager.NavigateTo("login");
                return "login";
            }
        }

        public async Task<List<OrderOverviewResponse>> GetOrders()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponse>>>("api/order");

            return response.Data;
        }

        public async Task<OrderDetailsResponse> GetOrderDetails(int orderId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsResponse>>($"api/order/{orderId}");
            return response.Data;
        }
    }
}
