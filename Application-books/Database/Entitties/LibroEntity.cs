﻿using Application_books.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application_books.Database.Entitties
{
    [Table("libros_book", Schema = "dbo")]
    public class LibroEntity : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column("titulo")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(2000)]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("id_genero")]
        public Guid IdGenero { get; set; }
        [ForeignKey(nameof(IdGenero))]
        public virtual GeneroEntity Genero { get; set; }

        [StringLength(50)]
        [Column("created_time")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(500)]
        [Column("img_libro")]
        public string UrlImg { get; set; }

        [Required]
        [StringLength(500)]
        [Column("pdf_libro")]
        public string UrlPdf { get; set; }

        [Required]
        [Column("id_autor")]
        public Guid IdAutor {  get; set; }
        [ForeignKey(nameof(IdAutor))]
        public virtual AutorEntity Autor { get; set; }

        public virtual IEnumerable<CalificacionEntity> Calificaciones { get; set; }
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }

    }
}
