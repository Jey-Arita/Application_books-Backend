using Application_books.Database.Entitties;
using Application_books.Database;
using Application_books.Dtos.Calificacion;
using Application_books.Dtos.Common;
using Application_books.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application_books.Dtos.Comentarios;

namespace Application_books.Services
{
    public class ComentariosService : IComentariosServices
    {
        private readonly ApplicationbooksContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ComentariosService(ApplicationbooksContext context, IMapper mapper, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._authService = authService;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto<List<ComentarioDto>>> GetComentarioListAsync()
        {
            var comentarioEntity = await _context.Comentarios.ToListAsync();
            var ComentariosDtos = _mapper.Map<List<ComentarioDto>>(comentarioEntity);

            return new ResponseDto<List<ComentarioDto>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Lista de comentarios obtenida correctamente.",
                Data = ComentariosDtos,
            };
        }
        public async Task<ResponseDto<List<ComentarioDto>>> GetComentarioByAsync(Guid id)
        {
            // Cargar los comentarios principales junto con las respuestas y el usuario de cada respuesta
            var comentarioEntities = await _context.Comentarios
                .Where(c => c.IdLibro == id) // Filtrar por el ID del libro
                .Include(c => c.Usuario) // Incluir el usuario que hizo el comentario principal
                .Include(c => c.Respuestas) // Incluir las respuestas de cada comentario
                .ThenInclude(r => r.Usuario) // Incluir el usuario de cada respuesta
                .ToListAsync();

            // Verificar si no se encontraron comentarios
            if (!comentarioEntities.Any())
            {
                return new ResponseDto<List<ComentarioDto>>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "No se encontraron registros."
                };
            }

            // Mapear los comentarios y sus respuestas a DTOs
            var comentarioDtos = _mapper.Map<List<ComentarioDto>>(comentarioEntities);

            return new ResponseDto<List<ComentarioDto>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Comentarios obtenidos correctamente.",
                Data = comentarioDtos,
            };
        }

        public async Task<ResponseDto<ComentarioDto>> CreateAsync(ComentarioCreateDto dto)
        {
            // Extraer el ID del usuario desde el token
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto<ComentarioDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Usuario no autenticado."
                };
            }

            // Buscar el usuario en la tabla User para obtener su nombre
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ResponseDto<ComentarioDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "El usuario no existe."
                };
            }

            // Mapea el DTO a la entidad de comentario
            var comentarioEntity = _mapper.Map<ComentarioEntity>(dto);
            comentarioEntity.IdUsuario = userId; // Asocia el ID del usuario
            comentarioEntity.NombreUsuario = user.FirstName + user.LastName; // Asocia el nombre del usuario

            // Si el DTO incluye un IdComentarioPadre, significa que este comentario es una respuesta
            if (dto.IdComentarioPadre.HasValue)
            {
                var comentarioPadre = await _context.Comentarios.FindAsync(dto.IdComentarioPadre.Value);
                if (comentarioPadre == null)
                {
                    return new ResponseDto<ComentarioDto>
                    {
                        StatusCode = 404,
                        Status = false,
                        Message = "El comentario al que intentas responder no existe."
                    };
                }

                // Asigna el IdComentarioPadre al nuevo comentario
                comentarioEntity.IdComentarioPadre = dto.IdComentarioPadre;
            }

            // Agrega el comentario (o respuesta) a la base de datos
            _context.Comentarios.Add(comentarioEntity);
            await _context.SaveChangesAsync();

            // Mapea la entidad a DTO para la respuesta
            var comentarioDto = _mapper.Map<ComentarioDto>(comentarioEntity);

            return new ResponseDto<ComentarioDto>
            {
                StatusCode = 201,
                Status = true,
                Message = "Comentario creado correctamente.",
                Data = comentarioDto,
            };
        }


        public async Task<ResponseDto<ComentarioDto>> EditAsync(ComentarioEditDto dto, Guid id)
        {
            var comentarioEntity = await _context.Comentarios.FirstOrDefaultAsync(e => e.Id == id);
            if (comentarioEntity is null)
            {
                return new ResponseDto<ComentarioDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = $"El registro no se encontro"
                };
            }
            _mapper.Map(dto, comentarioEntity);

            _context.Comentarios.Update(comentarioEntity);
            await _context.SaveChangesAsync();

            var calificacionDto = _mapper.Map<ComentarioDto>(comentarioEntity);
            return new ResponseDto<ComentarioDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro editado correctamente.",
                Data = calificacionDto
            };
        }
        public async Task<ResponseDto<ComentarioDto>> DeleteAsync(Guid id)
        {
            var comentarioEntity = await _context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (comentarioEntity == null)
            {
                return new ResponseDto<ComentarioDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = $"No se encontro el registro"
                };
            }

            _context.Comentarios.Remove(comentarioEntity);
            await _context.SaveChangesAsync();
            return new ResponseDto<ComentarioDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro borrado correctamente"
            };
        }
    }
}
