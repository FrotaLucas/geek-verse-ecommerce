namespace GeekVerse.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _http;
        public ProductTypeService(HttpClient http)
        {
            _http = http;  
        }
        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public event Action OnChange;

        public async Task AddProductType(ProductType productType)
        {
            var response = await _http.PostAsJsonAsync("api/producttype", productType);
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;

            OnChange.Invoke();

        }

        public ProductType CreateNewProductType()
        {
            var newProductTyp = new ProductType { IsNew = true, Editing = true };
            ProductTypes.Add(newProductTyp);
            OnChange.Invoke();

            return newProductTyp;
        }

        public async Task GetProductTypes()
        {
            var response = await _http
                .GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");

            ProductTypes = response.Data;           
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response = await _http.PutAsJsonAsync("api/producttype", productType);
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;

            OnChange.Invoke();  
        }

    }
}
