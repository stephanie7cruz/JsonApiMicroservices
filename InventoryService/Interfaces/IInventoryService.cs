using InventoryService.Models;

namespace InventoryService.Interfaces
{
    public interface IInventoryService
    {
        Inventory? GetByProductoId(int productoId);
        Inventory? ActualizarCantidad(int productoId, int cantidadComprada);
    }
}
