using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
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


        public async Task<Category> Create(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return category;

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

        public async Task<Category> GetFile(IFormFile file, Category category)
        {
            BlobContainerClient container = new BlobContainerClient(_configration.GetConnectionString("StorageConnection"), "images");
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
            };

            if (!await blob.ExistsAsync())
            {
                await blob.UploadAsync(stream, options);
            }

            category.CategoryCover = blob.Uri.ToString();

            return category;
        }


        public async Task<Product> GetFile(IFormFile file, Product product)
        {
            BlobContainerClient container = new BlobContainerClient(_configration.GetConnectionString("StorageConnection"), "images");
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
            };

            if (!await blob.ExistsAsync())
            {
                await blob.UploadAsync(stream, options);
            }

            product.ImageUri = blob.Uri.ToString();

            return product;
        }
    }
}
