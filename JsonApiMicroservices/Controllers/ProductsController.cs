using Microsoft.AspNetCore.Mvc;
using ProductService.Interfaces;
using ProductService.Models;
using ProductService.Services;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos los productos con paginación.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var productos = _service.GetAll(page, pageSize);
            return Ok(new
            {
                data = productos.Select(p => new
                {
                    type = "product",
                    id = p.Id,
                    attributes = new { p.Nombre, p.Precio }
                })
            });
        }

        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var producto = _service.GetById(id);
            if (producto == null)
                return NotFound(new { errors = new[] { new { detail = "Producto no encontrado" } } });

            return Ok(new
            {
                data = new
                {
                    type = "product",
                    id = producto.Id,
                    attributes = new { producto.Nombre, producto.Precio }
                }
            });
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            var created = _service.Create(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new
            {
                data = new
                {
                    type = "product",
                    id = created.Id,
                    attributes = new { created.Nombre, created.Precio }
                }
            });
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            var updated = _service.Update(id, product);
            if (updated == null)
                return NotFound(new { errors = new[] { new { detail = "Producto no encontrado" } } });

            return Ok(new
            {
                data = new
                {
                    type = "product",
                    id = updated.Id,
                    attributes = new { updated.Nombre, updated.Precio }
                }
            });
        }

        /// <summary>
        /// Elimina un producto.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var eliminado = _service.Delete(id);
            if (!eliminado)
                return NotFound(new { errors = new[] { new { detail = "Producto no encontrado" } } });

            return NoContent();
        }
    }
}
