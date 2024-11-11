namespace Application_books.Dtos.Comentarios
{
    public class ComentarioDto
    {
        public Guid Id { get; set; }
        public Guid IdLibro { get; set; }
        public Guid IdUsuario { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreUsuario { get; set; }
        public Guid? IdComentarioPadre { get; set; }  // Para identificar el comentario padre
        public List<ComentarioDto> Respuestas { get; set; } = new List<ComentarioDto>();
    }
}
