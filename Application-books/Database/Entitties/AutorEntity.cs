using Application_books.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application_books.Database.Entitties
{
    [Table("autor", Schema = "dbo")]
    public class AutorEntity : BaseEntity
    {
        [Required]
        [StringLength(50)]
        [Column("autor")]
        public string NombreAutor { get; set; }

        [StringLength(1000)]
        [Column("bibliografia")]
        public string Bibliografia { get; set; }

        [Required]
        [StringLength(500)]
        [Column("img_autor")]
        public string UrlImg { get; set; }

        public ICollection<LibroEntity> Libros { get; set; } = new List<LibroEntity>();
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }
    }
}
