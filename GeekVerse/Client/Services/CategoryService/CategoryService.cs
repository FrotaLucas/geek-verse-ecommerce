namespace GeekVerse.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }
        //recebe da tabela Categories quem tem coluna Deleted false ou 1 e Visible true ou 0
        public List<Category> Categories { get; set; } = new List<Category>();
        //recebe da tabela Categories quem tem coluna Deleted false ou 1
        public List<Category> AdminCategories { get; set; } = new List<Category>();

        public event Action OnChange;

        public async Task AddCategory(Category category)
        {
            var response = await _http.PostAsJsonAsync("api/category/admin", category);
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            //mesmo chamando a funcao GetCategories logo abaixo que atualiza o Categories, ainda assim preciso invocar o OnChange depois??
            //R: Acho que sim, pq o front precisa saber que tem um novo item na barra de menu para se adcionar.
            //Aqui ele atualiza o Categories, mas o html que ta consumindo ele precisa saber disso tbm.
            //Esse arquivo CategoryService.cs eh como se fosse um backend do frontend. O Categories atualiza, mas o frontend
            //que consome o Categories precisa saber disso.
            await GetCategories();
            OnChange.Invoke();
        }

        public Category CreateNewCategory()
        {
            var newCategory = new Category
            {
                IsNew = true,
                Editing = true,
            };

            //eh adicionado ao banco de dados tbm ? PQ NAO PRECISO DE AWAIT ?
            AdminCategories.Add(newCategory);
            OnChange.Invoke();
            return newCategory;
        }

        public async Task DeleteCategory(int categoryId)
        {
            //o metodo delete altera a coluna do Deleted no BD, logo tanto AdminCategories como Categories mudam
            var response = await _http.DeleteAsync($"api/category/admin/{categoryId}");
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

        public async Task GetAdminCategories()
        {
            //so mostra da tabela Category quem na coluna Deleted ta como false ou 1
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category/admin");
            if( response !=null && response.Data != null )
            {
                AdminCategories = response.Data;
            }
        }

        //Pq essa funcao nao precisa retornar tipo serviceResponse<List<Category>? ??????
        public async Task GetCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("/api/category");
            if (response != null && response.Data != null)
            {
                Categories = response.Data;
            }
        }

        public async Task UpdateCategory(Category category)
        {
            var response = await _http.PutAsJsonAsync("api/category/admin", category);
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;

            await GetCategories();
            OnChange.Invoke();
        }
    }
}
