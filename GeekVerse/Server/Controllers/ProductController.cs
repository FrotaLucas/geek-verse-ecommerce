using GeekVerse.Server.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekVerse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
        {
            var response = await _productService.GetAdminProducts();

            return Ok(response);
        }
        
        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
        {
            var response = await _productService.CreateProduct(product);

            return Ok(response);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
        {
            var response = await _productService.UpdateProduct(product);

            return Ok(response);
        } 
        
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProduct(id);

            return Ok(response);
        }

        //nao esta mais sendo usado, pq temos a api product/feature
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            var product = await _productService.GetProductAsync(productId);

            return Ok(product);
        }

        [HttpGet("category/{categoryUrl}")]  //QUEM DO CLIENTE ESTA CHAMANDO ESS API ?
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductByCategory(string categoryUrl)
        {
            var response = await _productService.GetProductsByCategory(categoryUrl);

            return Ok(response);
        }
        
        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResult>>> SearchProduct(string searchText, int page = 1)
        {
            var response = await _productService.SearchProducts(searchText, page);

            return Ok(response);
        }

        [HttpGet("searchsuggentions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchText)
        {
            //ATENCAO. O RETORNO Do BD nao eh <List<Product> mas sim <List<striing>. Nao eh um erro????
            var response = await _productService.GetProductSearchSuggestions(searchText);

            return Ok(response);
        }


        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            var products = await _productService.GetFeaturedProducts();

            return Ok(products);
        }
    }
}
