using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerce.Data;
using ECommerce.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Services
{
    public class ProductService  : IProduct
    {
        IConfiguration _configration;
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context, IConfiguration configration)
        { 
            _context = context;
            _configration = configration;

        }

        public async Task<Product> Create(Product product  )
        {

         
            _context.Entry(product).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return product;
        }


        public async Task<Uri> GetFile(IFormFile file)
        {
            if (file == null)
            {
                Uri defaultImg = new Uri("https://faststorestorage.blob.core.windows.net/images/DefaultIMG.png");
                return defaultImg;
            }
            BlobContainerClient container = new BlobContainerClient(_configration.GetConnectionString("AzureBlob"), "images");
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
            };
            if (!blob.Exists())
            {
                await blob.UploadAsync(stream, options);
            }
            return blob.Uri;

        }
    }
}
