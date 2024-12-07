using System.ComponentModel.DataAnnotations;

namespace Application_books.Dtos.Membresia
{
    public class MembresiaEditDto
    {
        [Required]
        public string TipoMembresia { get; set; } // Puede ser actualizado a un tipo diferente.
    }
}
