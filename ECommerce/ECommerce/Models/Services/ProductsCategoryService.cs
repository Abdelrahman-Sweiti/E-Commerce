using ECommerce.Data;
using ECommerce.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Services
{
    public class ProductsCategoryService : IProductsCategory
    {

        private readonly ApplicationDbContext _context;


        public ProductsCategoryService(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<List<ProductsCategory>> GetAllProductsForCategory(int categoryId)
        {
            var productsForCategory = await _context.productsCategories
                .Include(pc => pc.product)
                .Where(pc => pc.CategoryId == categoryId)
                .ToListAsync();

            return productsForCategory;
        }


    }
}
