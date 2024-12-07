using System.ComponentModel.DataAnnotations;

namespace Application_books.Dtos.Membresia
{
    public class MembresiaCreateDto
    {
        [Required]
        public string TipoMembresia { get; set; } // Puede ser "Gratis", "Premium", etc.
    }
}
