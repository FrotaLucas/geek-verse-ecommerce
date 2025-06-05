namespace GeekVerse.Client.Services.OrderService
{
    public interface IOrderService
    {

        //PRECISO DE DEFINIR COMO TASK TODA VEZ QUE O METODO INTERNAMENTE USA AWAIT
        Task<string> PlaceOrder();

        Task<List<OrderOverviewResponse>> GetOrders();

        Task<OrderDetailsResponse> GetOrderDetails(int orderId);
    }
}
