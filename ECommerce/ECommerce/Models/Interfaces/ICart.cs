namespace ECommerce.Models.Interfaces
{
    public interface ICart
    {
          Task<Cart> GetOrCreateCartAsync(string userId);
        Task<List<ProductsCart>> GetProductsInCartAsync(string userId);
        Task<int> GetCartItemCountAsync(string username);

    }
}
