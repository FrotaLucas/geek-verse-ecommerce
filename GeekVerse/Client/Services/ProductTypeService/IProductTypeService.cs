namespace GeekVerse.Client.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        event Action OnChange;

        public List<ProductType> ProductTypes { get; set; }
        public Task GetProductTypes();

        public Task AddProductType(ProductType productType);

        public Task UpdateProductType(ProductType productType);

        public ProductType CreateNewProductType();
    }
}
