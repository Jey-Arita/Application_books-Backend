using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application_books.Database.Entities;

namespace Application_books.Database.Entitties
{
    [Table("comentario", Schema = "dbo")]
    public class ComentarioEntity : BaseEntity
    {
        [Column("id_libro")]
        public Guid IdLibro { get; set; }
        [ForeignKey(nameof(IdLibro))]
        public virtual LibroEntity Libro { get; set; }

        [Column("id_usuario")]
        public string IdUsuario { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        public virtual UserEntity Usuario { get; set; }

        [StringLength(200)]
        [Column("comentario")]
        public string Comentario { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }
    }
}
