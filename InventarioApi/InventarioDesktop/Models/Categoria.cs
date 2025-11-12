using System.ComponentModel.DataAnnotations;

namespace InventarioDesktop.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }
}