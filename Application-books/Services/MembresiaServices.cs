using Application_books.Database;
using Application_books.Database.Entitties;
using Application_books.Dtos.Common;
using Application_books.Dtos.Membresia;
using Application_books.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application_books.Services
{
    public class MembresiaServices : IMembresiaServicio
    {
        private readonly ApplicationbooksContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MembresiaServices(ApplicationbooksContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto<MembresiaDto>> GetMembresiaByUserAsync()
        {
            // Extraer el ID del usuario desde el token
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

            if (userId == null)
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Usuario no autenticado."
                };
            }

            // Buscar la membresía asociada al usuario
            var membresiaEntity = await _context.Membresias
                .FirstOrDefaultAsync(m => m.IdUsuario == userId); // Usamos IdCliente para asociarlo con el usuario

            // Si no se encuentra la membresía
            if (membresiaEntity == null)
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "No se encontró la membresía para el usuario proporcionado."
                };
            }

            // Llamar al método para verificar la expiración de la membresía
            ExpiracionMembresia(membresiaEntity);

            // Mapear la entidad a un DTO para enviarlo
            var membresiaDto = _mapper.Map<MembresiaDto>(membresiaEntity);

            // Retornar la respuesta con la membresía encontrada
            return new ResponseDto<MembresiaDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Membresía obtenida correctamente.",
                Data = membresiaDto,
            };
        }

        public async Task<ResponseDto<MembresiaDto>> CreateMembresiaAsync(MembresiaCreateDto dto)
        {
            // Extraer el ID del usuario desde el token
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

            if (userId == null)
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Usuario no autenticado."
                };
            }

            // Verificar si el usuario ya tiene una membresía activa
            var existingMembresia = await _context.Membresias
                .FirstOrDefaultAsync(m => m.IdUsuario == userId && m.FechaFin > DateTime.Now); // Membresía activa

            if (existingMembresia != null)
            {
                // Si el usuario ya tiene una membresía activa, proceder con la actualización
                // Mapear los datos de dto a la entidad existente
                _mapper.Map(dto, existingMembresia);
                existingMembresia = CalcularMembresia(existingMembresia, dto.TipoMembresia);

                // Actualizar la membresía en la base de datos
                _context.Membresias.Update(existingMembresia);
                await _context.SaveChangesAsync();

                // Mapear a DTO
                var membresiaDto = _mapper.Map<MembresiaDto>(existingMembresia);

                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 200,
                    Status = true,
                    Message = "Membresía actualizada correctamente.",
                    Data = membresiaDto
                };
            }
            else
            {
                // Si el usuario no tiene una membresía activa, crear una nueva
                var membresiaEntity = _mapper.Map<MembresiaEntity>(dto);
                membresiaEntity.IdUsuario = userId;  // Asociar el ID de usuario

                // Calcular la fecha de inicio y fin según el tipo de membresía
                membresiaEntity = CalcularMembresia(membresiaEntity, dto.TipoMembresia);

                // Agregar la nueva membresía al contexto
                _context.Membresias.Add(membresiaEntity);
                await _context.SaveChangesAsync();

                // Mapear a DTO
                var membresiaDto = _mapper.Map<MembresiaDto>(membresiaEntity);

                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 201,
                    Status = true,
                    Message = "Membresía creada correctamente.",
                    Data = membresiaDto
                };
            }
        }


        public async Task<ResponseDto<MembresiaDto>> EditMembresiaAsync(MembresiaEditDto dto)
        {
            // Extraer el ID del usuario desde el token
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

            if (userId == null)
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Usuario no autenticado."
                };
            }

            // Buscar la membresía asociada al usuario
            var membresiaEntity = await _context.Membresias
                .FirstOrDefaultAsync(m => m.IdUsuario == userId);

            if (membresiaEntity is null)
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "El registro no fue encontrado."
                };
            }

            // Validar si el tipo de membresía es "Premium", "Gratis" o "Prueba"
            if (dto.TipoMembresia != "Premium" && dto.TipoMembresia != "Gratis" && dto.TipoMembresia != "Prueba")
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Solo se puede actualizar membresías de tipo 'Premium', 'Gratis' o 'Prueba'."
                };
            }

            // Mapear los datos de dto a la entidad
            _mapper.Map(dto, membresiaEntity);

            // Calcular la membresía, si es necesario
            membresiaEntity = CalcularMembresia(membresiaEntity, dto.TipoMembresia);

            // Actualizar la membresía en la base de datos
            _context.Membresias.Update(membresiaEntity);
            await _context.SaveChangesAsync();

            // Mapear la entidad a un DTO para la respuesta
            var membresiaDto = _mapper.Map<MembresiaDto>(membresiaEntity);

            return new ResponseDto<MembresiaDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro editado correctamente.",
                Data = membresiaDto
            };
        }

        private MembresiaEntity CalcularMembresia(MembresiaEntity entity, string tipoMembresia)
        {
            switch (tipoMembresia)
            {
                case "Gratis":
                    entity.FechaInicio = DateTime.Now;
                    entity.FechaFin = DateTime.Now;
                    entity.DiasRestantes = 0;
                    break;

                case "Prueba":
                    entity.FechaInicio = DateTime.Now;
                    entity.FechaFin = DateTime.Now.AddDays(7);
                    entity.DiasRestantes = 7;
                    break;

                case "Premium":
                    entity.FechaInicio = DateTime.Now;
                    entity.FechaFin = DateTime.Now.AddMonths(1);
                    entity.DiasRestantes = (entity.FechaFin - DateTime.Now).Value.Days;
                    break;

                case "":
                    entity.TipoMembresia = "Gratis";
                    entity.FechaInicio = DateTime.Now;
                    entity.FechaFin = DateTime.Now.AddMonths(1);
                    entity.DiasRestantes = (entity.FechaFin - DateTime.Now).Value.Days;
                    break;


                default:
                    throw new ArgumentException("Tipo de membresía no válida.");
            }

            return entity;
        }

        private void ExpiracionMembresia(MembresiaEntity entity)
        {
            if (entity.FechaFin.HasValue && entity.FechaFin.Value <= DateTime.Now)
            {
                entity.TipoMembresia = "Gratis";
                entity.FechaFin = null;
            }
        }
    }
}
