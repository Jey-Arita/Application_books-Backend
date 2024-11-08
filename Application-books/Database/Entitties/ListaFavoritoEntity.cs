using Application_books.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application_books.Database.Entitties
{
    [Table("lista_favorito", Schema = "dbo")]
    public class ListaFavoritoEntity : BaseEntity
    {
        [Column("id_usuario")]
        public string IdUsuario { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        public virtual UserEntity Usuario { get; set; }


        [Column("id_libro")]
        public Guid IdLibro { get; set; }
        [ForeignKey(nameof(IdLibro))]
        public virtual LibroEntity Libro { get; set; }
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }
    }
}
