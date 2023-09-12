using ECommerce.Data;
using ECommerce.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Services
{
    public class CartService : ICart
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            // Attempt to retrieve the user's cart from the database.
            var cart = await _context.carts
                .Include(c => c.productsCarts)
                .ThenInclude(pc => pc.product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // If the cart does not exist, create a new one.
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                // Add the new cart to the context and save changes.
                _context.carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }
}
