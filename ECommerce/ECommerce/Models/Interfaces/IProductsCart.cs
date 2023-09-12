namespace ECommerce.Models.Interfaces
{
    public interface IProductsCart
    {
        Task<bool> AddProductToCartAsync(int cartId, int productId, int quantity);

    }
}
