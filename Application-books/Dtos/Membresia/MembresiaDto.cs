using Application_books.Database.Entitties;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application_books.Database.Entities;

namespace Application_books.Dtos.Membresia
{
    public class MembresiaDto
    {
        public Guid Id { get; set; }
        public string IdUsuario { get; set; }
        public string TipoMembresia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int DiasRestantes { get; set; }
        public bool ActivaMembresia => FechaFin.HasValue && FechaFin.Value > DateTime.Now;
    }
}
