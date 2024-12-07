using Application_books.Database;
using Application_books.Database.Entitties;
using Application_books.Dtos.Autor;
using Application_books.Dtos.Common;
using Application_books.Dtos.ListaFavoritos;
using Application_books.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application_books.Services
{
    public class ListaFavoritosServices : IListaFavoritoServices
    {
        private readonly ApplicationbooksContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ListaFavoritosServices(ApplicationbooksContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<List<ListaFavoritoDto>>> GetListaFavoritoListAsync()
        {
            var listaFavEntity = await _context.ListaFavoritos.ToListAsync();
            var listaFavDto = _mapper.Map<List<ListaFavoritoDto>>(listaFavEntity);

            return new ResponseDto<List<ListaFavoritoDto>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Lista de Favoritos obtenida correctamente.",
                Data = listaFavDto,
            };
        }

        public async Task<ResponseDto<ListaFavoritoDto>> CreateAsync(ListaFavoritoCreateDto dto)
        {
            // Obtener el ID del usuario autenticado desde el token
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto<ListaFavoritoDto>
                {
                    StatusCode = 401,
                    Status = false,
                    Message = "No autorizado. No se pudo obtener el ID del usuario.",
                    Data = null,
                };
            }

            // Validar si ya existe como favorito para el usuario
            var existeFavorito = await _context.ListaFavoritos
                .AnyAsync(f => f.IdUsuario == userId && f.IdLibro == dto.IdLibro); // Cambiar IdElemento según corresponda

            if (existeFavorito)
            {
                return new ResponseDto<ListaFavoritoDto>
                {
                    StatusCode = 409,
                    Status = false,
                    Message = "El elemento ya está en tu lista de favoritos.",
                    Data = null,
                };
            }

            // Mapear el DTO a la entidad
            var listaFavEntity = _mapper.Map<ListaFavoritoEntity>(dto);
            listaFavEntity.IdUsuario = userId; // Asignar el ID del usuario a la entidad

            // Guardar en la base de datos
            _context.ListaFavoritos.Add(listaFavEntity);
            await _context.SaveChangesAsync();

            // Mapear la entidad creada al DTO de respuesta
            var listaFavDto = _mapper.Map<ListaFavoritoDto>(listaFavEntity);

            return new ResponseDto<ListaFavoritoDto>
            {
                StatusCode = 201,
                Status = true,
                Message = "Registro creado correctamente.",
                Data = listaFavDto,
            };
        }


        public async Task<ResponseDto<ListaFavoritoDto>> EditAsync(ListaFavoritoEditDto dto, Guid id)
        {
            var listaFavEntity = await _context.ListaFavoritos.FirstOrDefaultAsync(e => e.Id == id);
            if (listaFavEntity is null)
            {
                return new ResponseDto<ListaFavoritoDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = $"El registro registro"
                };
            }
            _mapper.Map<ListaFavoritoEditDto, ListaFavoritoEntity>(dto, listaFavEntity);
            _context.ListaFavoritos.Update(listaFavEntity);
            await _context.SaveChangesAsync();

            var listaFavDto = _mapper.Map<ListaFavoritoDto>(listaFavEntity);
            return new ResponseDto<ListaFavoritoDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro editado correctamente.",
                Data = listaFavDto
            };
        }

        public async Task<ResponseDto<ListaFavoritoDto>> DeleteAsync(Guid id)
        {
            var listaFavEntity = await _context.ListaFavoritos.FirstOrDefaultAsync(x => x.Id == id);
            if (listaFavEntity == null)
            {
                return new ResponseDto<ListaFavoritoDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = $"No se encontro el registro"
                };
            }

            _context.ListaFavoritos.Remove(listaFavEntity);
            await _context.SaveChangesAsync();
            return new ResponseDto<ListaFavoritoDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro borrado correctamente"
            };
        }
    }
}
