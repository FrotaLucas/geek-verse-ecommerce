namespace GeekVerse.Client.Services.CartService
{
    public interface ICartService 
    {

        //PRECISO DE DEFINIR COMO TASK TODA VEZ QUE O METODO INTERNAMENTE USA AWAIT
        //pq nao tem a palavra public em nenhuma das propriedades???????????? ?
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartProductResponse>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeId);
        Task UpdateQuantity(CartProductResponse product);
        Task StoreCartItems(bool emptyLocalCart);
        Task GetCartItemsCount();
      
    }
}
