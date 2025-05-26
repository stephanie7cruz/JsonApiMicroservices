using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ProductService.Services;
using ProductService.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductService.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void GetById_ShouldReturnCorrectProduct()
        {
            // Arrange
            var service = new Services.ProductService();
            var producto = new Product { Id = 1, Nombre = "Cerveza", Precio = 2.5M };
            service.Create(producto);

            // Act
            var resultado = service.GetById(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Cerveza", resultado?.Nombre);
            Assert.Equal(2.5M, resultado?.Precio);
        }
    }
}
