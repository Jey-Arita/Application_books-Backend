using Application_books.Database.Entitties;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application_books.Dtos.Calificacion
{
    public class CalificacionDto
    {
        public string Id { get; set; }
        public string IdLibro { get; set; }
        public string IdUsuario { get; set; }
        public int Puntuacion { get; set; }
    }
}
