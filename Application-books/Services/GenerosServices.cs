using Application_books.Database;
using Application_books.Dtos.Common;
using Application_books.Dtos.Generos;
using Application_books.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application_books.Services
{
    public class GenerosServices : IGeneroService
    {
        private ApplicationbooksContext _booksContext;
        private IMapper _mapper;

        public GenerosServices(ApplicationbooksContext booksContext, IMapper mapper)
        {
            this._booksContext = booksContext;
            this._mapper = mapper;
        }

        public async Task<ResponseDto<List<GenerosDto>>> GetGenerosListAsync()
        {
            // Obtener la lista de géneros de la base de datos
            var generosEntity = await _booksContext.Generos.ToListAsync();

            // Mapear las entidades al DTO
            var generosDto = _mapper.Map<List<GenerosDto>>(generosEntity);

            // Retornar la respuesta
            return new ResponseDto<List<GenerosDto>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Lista de géneros obtenida correctamente.",
                Data = generosDto
            };
        }

    }
}
