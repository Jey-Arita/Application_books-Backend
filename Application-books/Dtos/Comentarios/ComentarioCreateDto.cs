using System.ComponentModel.DataAnnotations;

namespace Application_books.Dtos.Comentarios
{
    public class ComentarioCreateDto
    {
        [Display(Name = "IdLibro")]
        [Required(ErrorMessage = "El Id es requerido.")]
        public Guid IdLibro { get; set; }
        [Display(Name = "IdUsuario")]
        [Required(ErrorMessage = "El Id es requerido.")]
        public string Comentario { get; set; }
        public Guid? IdComentarioPadre { get; set; }
    }
}
