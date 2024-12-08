using Application_books.Database;
using Application_books.Database.Entitties;
using Application_books.Dtos.Calificacion;
using Application_books.Dtos.Common;
using Application_books.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application_books.Services
{
    public class CalificacionesServices : ICalificacionesServices
    {
        private readonly ApplicationbooksContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CalificacionesServices(ApplicationbooksContext context, IMapper mapper, IAuthService authService, IHttpContextAccessor httpContextAccessor) 
        {
            this._context = context;
            this._mapper = mapper;
            this._authService = authService;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto<List<CalificacionDto>>> GetCalificacionesListAsync()
        {
            var calificacionEntity = await _context.Calificaciones.ToListAsync();
            var CalificacionDtos = _mapper.Map<List<CalificacionDto>>(calificacionEntity);

            return new ResponseDto<List<CalificacionDto>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Lista de calificacion obtenida correctamente.",
                Data = CalificacionDtos,
            };
        }

        public async Task<ResponseDto<CalificacionDto>> GetCalificacionByAsync(Guid idLibro)
        {
            // Extraer el ID del usuario desde el token
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Usuario no autenticado.",
                    Data = null
                };
            }

            // Buscar el usuario en la base de datos para validar su existencia
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "El usuario no existe.",
                    Data = null
                };
            }

            // Buscar si el usuario ya tiene una calificación para este libro
            var existingCalificacion = await _context.Calificaciones
                .FirstOrDefaultAsync(c => c.IdUsuario == userId && c.IdLibro == idLibro);

            if (existingCalificacion != null)
            {
                // Mapear la calificación al DTO
                var calificacionDto = _mapper.Map<CalificacionDto>(existingCalificacion);

                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 200,
                    Status = true,
                    Message = "El usuario ya ha calificado este libro.",
                    Data = calificacionDto
                };
            }

            return new ResponseDto<CalificacionDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "El usuario no ha calificado este libro.",
                Data = null
            };
        }


        public async Task<ResponseDto<CalificacionDto>> CreateAsync(CalificacionCreateDto dto)
        {
            // Extraer el ID del usuario desde el token
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Usuario no autenticado."
                };
            }

            // Buscar el usuario en la base de datos para validar su existencia
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "El usuario no existe."
                };
            }

            // Buscar el libro relacionado
            var libro = await _context.Libros.FindAsync(dto.IdLibro);
            if (libro == null)
            {
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "El libro no existe."
                };
            }

            // Verificar si el usuario ya calificó el libro
            var existingCalificacion = await _context.Calificaciones
                .FirstOrDefaultAsync(c => c.IdUsuario == userId && c.IdLibro == dto.IdLibro);

            if (existingCalificacion != null)
            {
                // Actualizar la calificación existente
                existingCalificacion.Puntuacion = dto.Puntuacion;

                _context.Calificaciones.Update(existingCalificacion);
                await _context.SaveChangesAsync();

                var updatedDto = _mapper.Map<CalificacionDto>(existingCalificacion);
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 200,
                    Status = true,
                    Message = "Calificación actualizada correctamente.",
                    Data = updatedDto,
                };
            }

            // Crear una nueva calificación si no existe
            var calificacionEntity = _mapper.Map<CalificacionEntity>(dto);
            calificacionEntity.IdUsuario = userId;

            _context.Calificaciones.Add(calificacionEntity);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            var calificacionDto = _mapper.Map<CalificacionDto>(calificacionEntity);

            return new ResponseDto<CalificacionDto>
            {
                StatusCode = 201,
                Status = true,
                Message = "Calificación creada correctamente.",
                Data = calificacionDto,
            };
        }


        public async Task<ResponseDto<CalificacionDto>> EditAsync(CalificacionEditDto dto, Guid id)
        {
            var calificacionEntity = await _context.Calificaciones.FirstOrDefaultAsync(e => e.Id == id);
            if (calificacionEntity is null)
            {
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = $"El registro no se encontro"
                };
            }
            _mapper.Map(dto, calificacionEntity);
           
            _context.Calificaciones.Update(calificacionEntity);
            await _context.SaveChangesAsync();

            var calificacionDto = _mapper.Map<CalificacionDto>(calificacionEntity);
            return new ResponseDto<CalificacionDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro editado correctamente.",
                Data = calificacionDto
            };
        }
        public async Task<ResponseDto<CalificacionDto>> DeleteAsync(Guid id)
        {
            var calificacionEntity = await _context.Calificaciones.FirstOrDefaultAsync(x => x.Id == id);
            if (calificacionEntity == null)
            {
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = $"No se encontro el registro"
                };
            }

            _context.Calificaciones.Remove(calificacionEntity);
            await _context.SaveChangesAsync();
            return new ResponseDto<CalificacionDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro borrado correctamente"
            };
        }
    }
}
