using Application_books.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application_books.Database.Entitties
{
    [Table("calificacion", Schema = "dbo")]
    public class CalificacionEntity : BaseEntity
    {
        [Column("id_libro")]
        public Guid IdLibro { get; set; }
        [ForeignKey(nameof(IdLibro))]
        public virtual LibroEntity Libro { get; set; }

        [Column("id_usuario")]
        public string IdUsuario { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        public virtual UserEntity Usuario { get; set; }

        [Range(1, 5)]
        [Column("puntuacion")]
        public int Puntuacion { get; set; }
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }

    }
}
