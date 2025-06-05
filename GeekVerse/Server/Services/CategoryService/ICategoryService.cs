namespace GeekVerse.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategories();

        Task<ServiceResponse<List<Category>>> GetAdminCategories();
        Task<ServiceResponse<List<Category>>> AddCategories(Category category);

        Task<ServiceResponse<List<Category>>> UpdateCategory(Category category);

        Task<ServiceResponse<List<Category>>> DeleteCategory(int id);
    }
}
