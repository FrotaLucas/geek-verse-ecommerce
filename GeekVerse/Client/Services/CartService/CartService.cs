using GeekVerse.Shared;
using Blazored.LocalStorage;

namespace GeekVerse.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly IAuthService _authService;

        public CartService(ILocalStorageService localStorage, HttpClient http, IAuthService authService)
        {
            _localStorage = localStorage;   
            _http = http;
            _authService = authService;
        }
        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await _authService.IsAuthenticated())
            {
                Console.WriteLine("User is authenticated");
                await _http.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId &&
                x.ProducTypetId == cartItem.ProducTypetId);

                if (sameItem == null)
                {
                    cart.Add(cartItem);
                }
                else
                {
                    //sameItem.Quantity += cartItem.Quantity;
                    sameItem.Quantity = sameItem.Quantity + 1;
                }
                await _localStorage.SetItemAsync("cart", cart);
            }

            await GetCartItemsCount();
        }

        //busca no banco de dados infos como preco e Product Type para expor no componente Cart.razor 
        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            //As APIs api/cart e a api/cart/products fazem a mesma coisa. Elas entregam uma lista de CartProductResponse.
            //a diferenca eh que api/cart usa o Id do usuario por baixo dos panos, ja a outra nao.
            if (await _authService.IsAuthenticated())
            {
                var responsee = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("api/cart");
                return responsee.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                {
                    return new List<CartProductResponse>();
                }
                //estou uando esse metodo pq a api eh do tipo Post. Precisava ser
                //post pra receber o parametro cartItems?
                var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
                var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

                return cartProducts.Data;
            }
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            if(await _authService.IsAuthenticated())
            {
                await _http.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if(cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == productId && x.ProducTypetId == productTypeId);
           
                if(cartItem != null)
                {
                    cart.Remove(cartItem); 
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
        }
        //salva no banco de dados a lista de produtos
        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            //nao deveria aqui ser cartItemsCount ao inves de cart ? Quando Login.razor eh chamado o usuario
            //esta autenticado e por conseguencia deveria salvar itens personalizados do carrinho no cartItemsCount 
            //???????????????????????
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (localCart == null)
                return;

            await _http.PostAsJsonAsync("api/cart", localCart);

            if(emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        //essa funcao tem return ?
        public async Task UpdateQuantity(CartProductResponse product)
        {
            if(await _authService.IsAuthenticated())
            {
                var request = new CartItem()
                {
                    ProductId = product.ProductId,
                    ProducTypetId = product.ProductTypeId,
                    Quantity = product.Quantity
                };
                await _http.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == product.ProductId
                && x.ProducTypetId == product.ProductTypeId);

                if (cartItem != null)
                {   //Interessante. Ao mudar item cartItem que pertence a lista.
                    //automaticamente a lista cart eh atualizada
                    cartItem.Quantity = product.Quantity;
                    //pq precisa desse _localStorage aqui ?
                    await _localStorage.SetItemAsync("cart", cart);
                }

            }
        }

        //extrai do banco de dados os items de carrinho, conta quantos tem e salva no localSotorage em cartItemsCount.
        //Descobre o Id do User usando o HttpContext
        public async Task GetCartItemsCount()
        {
            //todos os metodos dessa classe precisam chamar o GetCartItemsCount pq ele tem o INVOKE que atualiza
            //tudo.

            if (await _authService.IsAuthenticated())
            {
                var cartItems = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = cartItems.Data;
                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cartCount = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cartCount != null ? cartCount.Count : 0);
            }
          
            OnChange.Invoke();
        }
    }
}
