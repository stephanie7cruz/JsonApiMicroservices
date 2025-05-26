using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private static readonly List<Product> _products = new();
        private static int _nextId = 1;

        public IEnumerable<Product> GetAll(int page, int pageSize)
        {
            return _products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public Product Create(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }

        public Product? Update(int id, Product updated)
        {
            var existing = GetById(id);
            if (existing == null) return null;

            existing.Nombre = updated.Nombre;
            existing.Precio = updated.Precio;
            return existing;
        }

        public bool Delete(int id)
        {
            var product = GetById(id);
            if (product == null) return false;

            _products.Remove(product);
            return true;
        }
    }
}

