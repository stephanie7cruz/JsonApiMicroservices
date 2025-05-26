using Microsoft.AspNetCore.Mvc;
using InventoryService.Models;
using InventoryService.Services;
using InventoryService.Interfaces;
using InventoryService.Clients;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/inventories")]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _service;
        private readonly ProductClient _productClient;

        public InventoriesController(IInventoryService service, ProductClient productClient)
        {
            _service = service;
            _productClient = productClient;
        }

        /// <summary>
        /// Obtiene la cantidad disponible para un producto.
        /// </summary>
        [HttpGet("{productoId}")]
        public IActionResult GetByProductId(int productoId)
        {
            var inventario = _service.GetByProductoId(productoId);
            if (inventario == null)
                return NotFound(new { errors = new[] { new { detail = "Inventario no encontrado" } } });

            return Ok(new
            {
                data = new
                {
                    type = "inventory",
                    id = inventario.ProductoId,
                    attributes = new { inventario.Cantidad }
                }
            });
        }

        /// <summary>
        /// Actualiza la cantidad después de una compra.
        /// </summary>
        [HttpPut("{productoId}")]
        public IActionResult UpdateCantidad(int productoId, [FromBody] int cantidadComprada)
        {
            var actualizado = _service.ActualizarCantidad(productoId, cantidadComprada);
            if (actualizado == null)
                return NotFound(new { errors = new[] { new { detail = "Inventario no encontrado o producto no existe" } } });

            Console.WriteLine($"[EVENTO] Cambio de inventario para producto {productoId}: {actualizado.Cantidad}");

            return Ok(new
            {
                data = new
                {
                    type = "inventory",
                    id = actualizado.ProductoId,
                    attributes = new { actualizado.Cantidad }
                }
            });
        }

        /// <summary>
        /// Obtiene el detalle completo: producto + cantidad disponible.
        /// </summary>
        [HttpGet("{productoId}/detalle")]
        public async Task<IActionResult> GetProductoConInventario(int productoId)
        {
            var inventario = _service.GetByProductoId(productoId);
            if (inventario == null)
                return NotFound(new { errors = new[] { new { detail = "Inventario no encontrado" } } });

            var producto = await _productClient.GetProductByIdAsync(productoId);
            if (producto == null)
                return NotFound(new { errors = new[] { new { detail = "Producto no encontrado en ProductService" } } });

            return Ok(new
            {
                data = new
                {
                    type = "inventory-product",
                    id = producto.Id,
                    attributes = new
                    {
                        producto.Nombre,
                        producto.Precio,
                        inventario.Cantidad
                    }
                }
            });
        }
    }
}

