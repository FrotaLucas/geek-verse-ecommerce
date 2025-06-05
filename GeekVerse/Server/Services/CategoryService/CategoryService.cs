namespace GeekVerse.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        readonly DataContext _context;
        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Category>>> AddCategories(Category category)
        {
            //o que significa essa linha de codigo ?
            //R: Eh um chain assignment operator. Primeiro category.IsNew recebe false, e ai category.Editing recebe tbm false.
            category.Editing = category.IsNew = false;

            await _context.Categories.AddAsync(category);
            _context.SaveChanges();

            //pq esse retorno usando a outra funcao  ?
            return await GetAdminCategories();

        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
            var category = await GetCategoryById(id);
            if(category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            //altera no banco de dados
            category.Deleted = true;
            await _context.SaveChangesAsync();

            return await GetAdminCategories();

        }

        private async Task<Category> GetCategoryById(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
          //0 - false and 1 - true
          var categories = await _context.Categories
                .Where(c => !c.Deleted).ToListAsync();

          return new ServiceResponse<List<Category>> { 
            Data = categories
          };


        }

        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            //1 no bando de dados eh false e 0 eh true
            var categories = await _context.Categories
                .Where( c => !c.Deleted && c.Visible) //pra dar true && true Deleted eh 0 e Visible 1
                .ToListAsync();

            return new ServiceResponse<List<Category>>()
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            var dbCategory = await GetCategoryById(category.Id);

            if (dbCategory == null)
            {
                return new ServiceResponse<List<Category>>()
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible  = category.Visible;

            await _context.SaveChangesAsync();
            //pq nao precisa atualizar Deleted tbm ? essa prop ta no bando de dados tbm
            //dbCategory.Deleted  = category.Deleted;

            return await GetAdminCategories();  
        }
    }
}
