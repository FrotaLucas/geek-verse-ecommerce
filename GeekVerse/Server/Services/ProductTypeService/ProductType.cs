namespace GeekVerse.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _context.ProductType.ToListAsync();

            return new ServiceResponse<List<ProductType>>
            {
                Data = productTypes
            };

        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            //evitar de eviar product type para o front com editing = false e IsNew = false
            productType.Editing = productType.IsNew = false;
            _context.ProductType.Add(productType);
            await  _context.SaveChangesAsync();

            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            var dbProductType = await _context.ProductType.FindAsync(productType.Id);

            if (dbProductType == null)
            {
                return new ServiceResponse<List<ProductType>>()
                {
                    Success = false,
                    Message = "Product Type not found."
                };
            }

            dbProductType.Name = productType.Name;
            await _context.SaveChangesAsync();

            return await GetProductTypes();

        }
    }
}
