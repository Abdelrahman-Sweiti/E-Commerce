namespace ECommerce.Models.Interfaces
{
    public interface ICategory
    {
        Task<List<Category>> GetCategories();

        Task<Product> AddProductToCategories(int categoriesId, Product product  );

    }
}
