using ProductService.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate(); // Aplica migraciones pendientes

            if (context.Products.Any()) return; 

            var productos = new List<Product>
            {
                new Product { Nombre = "Cerveza", Precio = 3500 },
                new Product { Nombre = "Malta", Precio = 2500 },
                new Product { Nombre = "Agua con gas", Precio = 2000 }
            };

            context.Products.AddRange(productos);
            context.SaveChanges();
        }
    }
}
