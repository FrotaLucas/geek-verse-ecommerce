using GeekVerse.Shared;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace GeekVerse.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;
        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public List<Product> AdminProducts { get; set; } = new List<Product>();

        public string Message { get; set; } = "Loading products ...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action ProductsChanged;


        public async Task GetAdminProducts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
            AdminProducts = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts.Count == 0)
                Message = "No products found.";
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            //pq essa tem return e a de baixo nao tem ???
            return result;
        }
        public async Task GetProducts(string? categoryUrl = null)
        {
            //var result = (categoryUrl == null) ? await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") :
            var result = (categoryUrl == null) ? await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") :
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            
            if (result != null && result.Data != null)
                Products = result.Data; 

            if (Products.Count == 0  || result.Data == null)
            {
                Products = new List<Product>();
                Message = "No products found";
            }

            CurrentPage = 1;
            PageCount = 0;
            //chamada de evento quando metodo GetProducts for acionado
            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            //rever controller. Esta mandando lista de Product e nao de string!!!!!!
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggentions/{searchText}");
            return result.Data;
        }


        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            //pq somente se eu adicionar um breakpoint a atualizacao de Products com evento funciona?
            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/product/search/{searchText}/{page}");

            if (result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (result == null || Products.Count == 0) Message = "No products found.";
            ProductsChanged?.Invoke();
        }
        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _http.PostAsJsonAsync("api/product/admin", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Product>>()).Data;

            return newProduct;
        }

        public async Task DeleteProduct(Product product)
        {
            var result = await _http.DeleteAsync($"api/product/{product.Id}");
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await _http.PutAsJsonAsync("api/product/admin", product);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>();
            return content.Data;
            //return (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data; 
        }
    }
}
