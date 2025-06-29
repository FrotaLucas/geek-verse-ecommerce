using System.Drawing.Printing;
using System.Reflection.Metadata.Ecma335;

namespace GeekVerse.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var serviceResponse = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Featured && p.Visible && !p.Deleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted)).ToListAsync()
            };

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await _context.Products.Where(p => !p.Deleted)
                    .Include(p => p.Variants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.ProductType)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            Product product = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await _context.Products.Include(p => p.Variants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.ProductType)
                    .FirstOrDefaultAsync(p => p.Id == productId && !p.Deleted);
            }
            else
            {
                //op1
                product = await _context.Products
               .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
               .ThenInclude(pt => pt.ProductType)
               .FirstOrDefaultAsync(p => p.Id == productId && p.Visible && !p.Deleted);
            }

            //op2
            //var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }

            else
            {
                response.Success = true;
                response.Data = product;
            }

            return response;
        }
        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Visible && !p.Deleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted)).ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>();

            var products = await _context.Products
            .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) && p.Visible && !p.Deleted)
            .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
            .ToListAsync();

            if (products == null || products.Count == 0)
            {
                response.Success = false;
                response.Message = "List of products not found";
                return response;
            }
            response.Data = products;
            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySeachText(searchText);
            List<string> result = new List<string>();

            foreach (var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                if (product.Description != null)
                {
                    var pontuaction = product.Description.Where(char.IsPunctuation)
                    .Distinct().ToArray();

                    var words = product.Description.Split()
                    .Select(s => s.Trim(pontuaction));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }


            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
        {
            var maxProductsPerPage = 2f;
            var pageCounts = Math.Ceiling((await FindProductsBySeachText(searchText)).Count / maxProductsPerPage);
            var products = await _context.Products
                              .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                                  p.Description.ToLower().Contains(searchText.ToLower())
                                  && p.Visible && p.Deleted)
                              .Include(p => p.Variants)
                              .Skip((page - 1) * (int)maxProductsPerPage)
                              .Take((int)maxProductsPerPage)
                              .ToListAsync();

            ServiceResponse<ProductSearchResult> response = new ServiceResponse<ProductSearchResult>()
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCounts
                }
            };


            return response;
        }

        private async Task<List<Product>> FindProductsBySeachText(string searchText)
        {
            return await _context.Products.Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                            ||
                            p.Description.ToLower().Contains(searchText.ToLower())
                            && p.Visible && !p.Deleted
                            )
                            .Include(p => p.Variants)
                            .ToListAsync();
        }

        public async Task<ServiceResponse<Product>> CreateProduct(Product product)
        {
            foreach (var variante in product.Variants)
            {
                //provisorio
                variante.ProductType = null;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ServiceResponse<Product>
            {
                Data = product
            };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            var dbProduct = await _context.Products.FindAsync(productId);
            if (dbProduct == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Product now found.",
                    Success = false
                };
            }

            dbProduct.Deleted = true;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);

            if (dbProduct == null)
            {
                return new ServiceResponse<Product>
                {
                    Message = "Product now found.",
                    Success = false
                };
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.Visible = product.Visible;
            dbProduct.Deleted = product.Deleted;
            dbProduct.Featured = product.Featured;

            foreach (var variant in product.Variants)
            {
                //CRIANDO NOVA VARIANTE
                if (variant.IsNew == true)
                {
                    _context.ProductVariant.Add(variant);
                    await _context.SaveChangesAsync();

                    continue;
                }

                var availableProducts = await _context.ProductVariant.Where(v => v.ProductId == variant.ProductId &&
                    v.Deleted == false).ToListAsync();

                //ATUALIZANDO VARIANTE ANTIGA
                var variantToBeUpdated = availableProducts.Find(v => v.ProductTypeId == variant.ProductTypeId);

                //VARIANT AINDA nao ta no BD
                if (variantToBeUpdated == null)
                {
                    var newVariant = new ProductVariant();
                    //remover toda variant do db que nao esta na product.Variants
                    var rangeToBeRemoved = availableProducts
                        .Where(dbVariant => !product.Variants
                            .Any(v => v.ProductTypeId.Equals(dbVariant.ProductTypeId)))
                        .ToList();

                    if (rangeToBeRemoved != null && rangeToBeRemoved.Any() ) {
                        _context.RemoveRange(rangeToBeRemoved);
                        await _context.SaveChangesAsync();
                    }


                    newVariant = variant;
                    newVariant.ProductType = null;
                    _context.ProductVariant.Add(newVariant);
                    _context.SaveChanges();

                    continue;
                }

                //variant esta no BD
                variantToBeUpdated.OriginalPrice = variant.OriginalPrice;
                variantToBeUpdated.Price = variant.Price;
                variantToBeUpdated.Deleted = variant.Deleted;
                variantToBeUpdated.Visible = variant.Visible;
            }

            //dbvariante tem escopo restrito no loop. Esse save nao deveria esta dentro do foreach ?
            await _context.SaveChangesAsync();

            //certo seria retornar dbProduct com o estado mais atual do que foi feito no Banco de Dados. Nao??
            return new ServiceResponse<Product>
            {
                Data = product,
            };
        }
    }

}
