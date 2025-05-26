using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Si ya hay datos, no insertes de nuevo
            if (context.Inventories.Any()) return;

            var inventarios = new Inventory[]
            {
            new Inventory { ProductoId = 1, Cantidad = 50 },
            new Inventory { ProductoId = 2, Cantidad = 75 },
            new Inventory { ProductoId = 3, Cantidad = 100 }
            };

            context.Inventories.AddRange(inventarios);
            context.SaveChanges();
        }
    }

}

