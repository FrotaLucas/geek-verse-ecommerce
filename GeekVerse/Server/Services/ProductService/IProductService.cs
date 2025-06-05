namespace GeekVerse.Server.Services.ProductService
{
        public interface IProductService
    {
            Task<ServiceResponse<List<Product>>> GetProductsAsync();
            Task<ServiceResponse<Product>> GetProductAsync(int productId);
            Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
            Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page);
            Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText);
            Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
            Task<ServiceResponse<List<Product>>> GetAdminProducts();

            //esses servicos vinheram depois
            Task<ServiceResponse<Product>> CreateProduct(Product product);
            Task<ServiceResponse<bool>> DeleteProduct(int productId);
            Task<ServiceResponse<Product>> UpdateProduct(Product product);


        }
   
}
