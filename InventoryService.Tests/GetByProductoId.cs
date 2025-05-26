using InventoryService.Services;  
using Xunit;

namespace InventoryService.Tests
{
    public class InventoryServiceTests
    {
        private readonly InventoryService.Services.InventoryService _service;

        public InventoryServiceTests()
        {
            _service = new InventoryService.Services.InventoryService();
        }

        [Fact]
        public void GetByProductoId_ExistingId_ReturnsInventory()
        {
            var result = _service.GetByProductoId(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.ProductoId);
            Assert.Equal(50, result.Cantidad);
        }


        [Fact]
        public void ActualizarCantidad_ProductNotFound_ReturnsNull()
        {
            var result = _service.ActualizarCantidad(999, 5);
            Assert.Null(result);
        }
    }
}
