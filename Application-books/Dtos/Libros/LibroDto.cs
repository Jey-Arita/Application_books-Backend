﻿using Application_books.Database.Entitties;
using Application_books.Dtos.Autor;
using System.ComponentModel.DataAnnotations;

namespace Application_books.Dtos.Libros
{
    public class LibroDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string IdGenero { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UrlImg {  get; set; }
        public string UrlPdf { get; set; }
        public Guid IdAutor { get; set; }
        public double Promedio {  get; set; }
    }
}
