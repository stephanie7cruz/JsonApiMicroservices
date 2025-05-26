using InventoryService.Interfaces;
using InventoryService.Models;

namespace InventoryService.Services
{
    public class InventoryService : IInventoryService
    {
        private static readonly List<Inventory> _inventario = new()
        {
            new Inventory { ProductoId = 1, Cantidad = 50 },
            new Inventory { ProductoId = 2, Cantidad = 25 }
        };

        public Inventory? GetByProductoId(int productoId)
        {
            return _inventario.FirstOrDefault(i => i.ProductoId == productoId);
        }

        public Inventory? ActualizarCantidad(int productoId, int cantidadComprada)
        {
            var item = GetByProductoId(productoId);
            if (item == null) return null;

            item.Cantidad = Math.Max(0, item.Cantidad - cantidadComprada);
            return item;
        }
    }
}
