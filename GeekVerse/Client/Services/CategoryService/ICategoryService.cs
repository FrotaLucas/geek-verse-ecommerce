namespace GeekVerse.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        //PRECISO DE DEFINIR COMO TASK TODA VEZ QUE O METODO INTERNAMENTE USA AWAI
        event Action OnChange;
        List<Category> Categories { get; set; }
        List<Category> AdminCategories { get; set; }
        Task GetCategories();
        Task GetAdminCategories();

        Task AddCategory( Category category);
        Task UpdateCategory( Category category );   
        Task DeleteCategory (int categoryId);
        Category CreateNewCategory();

        //pq esta errado essa notacao ????????
        //Task<Category> GetByIdAsync(int id);

    }
}
