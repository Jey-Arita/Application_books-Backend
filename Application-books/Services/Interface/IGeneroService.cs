using Application_books.Dtos.Comentarios;
using Application_books.Dtos.Common;
using Application_books.Dtos.Generos;

namespace Application_books.Services.Interface
{
    public interface IGeneroService
    {
        Task<ResponseDto<List<GenerosDto>>> GetGenerosListAsync();
    }
}
