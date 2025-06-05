using GeekVerse.Shared;

namespace GeekVerse.Client.Services.ProductService
{
    public interface IProductService
    {

        //PRECISO DE DEFINIR COMO TASK TODA VEZ QUE O METODO INTERNAMENTE USA AWAIT
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        List<Product> AdminProducts { get; set; }
        string Message {  get; set; }
        int CurrentPage { get; set; }
        string LastSearchText { get; set; }
        public int PageCount { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int productId);
        Task SearchProducts( string searchText, int page);
        Task<List<string>> GetProductSearchSuggestions(string searchText);

        Task GetAdminProducts();

        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Product product);
    }
}
