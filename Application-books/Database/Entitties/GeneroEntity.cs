using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application_books.Database.Entitties
{
    [Table("genero", Schema = "dbo")]
    public class GeneroEntity: BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }
    }
}
