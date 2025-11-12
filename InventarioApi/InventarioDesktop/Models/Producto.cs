using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarioDesktop.Models
{
    public class Producto
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        
        public int CategoriaId { get; set; }
        
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        
        public int Stock { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}