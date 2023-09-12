namespace ECommerce.Models.Interfaces
{
    public interface ICart
    {
          Task<Cart> GetOrCreateCartAsync(string userId);

    }
}
