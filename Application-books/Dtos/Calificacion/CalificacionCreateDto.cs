using System.ComponentModel.DataAnnotations;

namespace Application_books.Dtos.Calificacion
{
    public class CalificacionCreateDto
    {
        [Display(Name = "IdLibro")]
        [Required(ErrorMessage = "El Id es requerido.")]
        public Guid IdLibro { get; set; }
        [Display(Name = "IdLibro")]
        [Required(ErrorMessage = "El Id es requerido.")]

        public int Puntuacion { get; set; }
    }
}
