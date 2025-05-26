using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService.Models
{
    public class Inventory
    {
        /// <summary>
        /// Identificador único del inventario (clave primaria, autogenerado).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ID del producto asociado (debe existir en ProductService).
        /// </summary>
        [Required(ErrorMessage = "El ProductoId es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ProductoId debe ser mayor a cero.")]
        public int ProductoId { get; set; }

        /// <summary>
        /// Cantidad disponible del producto en inventario.
        /// </summary>
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser negativa.")]
        public int Cantidad { get; set; }
    }
}

