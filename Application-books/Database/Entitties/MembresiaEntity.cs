using Application_books.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application_books.Database.Entitties
{
    [Table("membresia", Schema = "dbo")]
    public class MembresiaEntity : BaseEntity
    {
        [Column("id_usuario")]
        public string IdUsuario { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        public virtual UserEntity Usuario { get; set; }

        [Column("activa_membresia")]
        public bool ActivaMembresia { get; set; }

        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        [Column("fecha_fin")]
        public DateTime? FechaFin { get; set; }

        [Column("fecha_cancelacion")]
        public DateTime? FechaCancelacion { get; set; }
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }
    }
}
