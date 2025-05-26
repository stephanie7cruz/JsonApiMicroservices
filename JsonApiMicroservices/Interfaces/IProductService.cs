using ProductService.Models;

namespace ProductService.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll(int page, int pageSize);
        Product? GetById(int id);
        Product Create(Product product);
        Product? Update(int id, Product product);
        bool Delete(int id);
    }
}
