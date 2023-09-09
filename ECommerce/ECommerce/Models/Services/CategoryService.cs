using ECommerce.Data;
using ECommerce.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        IConfiguration _configration;

        public CategoryService(ApplicationDbContext context, IConfiguration configration)
        {
            _context = context;
              _configration = configration;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.categories.Include(x => x.productsCategories).ThenInclude(y => y.product).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.categories.FirstOrDefaultAsync(x=>x.Id==id);
            return category;
        }

        public async Task<Product> AddProductToCategories(int categoryId, Product product)
        {

           
            _context.Entry(product).State = EntityState.Added;

            await _context.SaveChangesAsync();
            ProductsCategory categoryProduct = new ProductsCategory()
            {
                ProductId = product.Id,
                CategoryId = categoryId
            };

            _context.Entry(categoryProduct).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return product;
        }


    }
}
