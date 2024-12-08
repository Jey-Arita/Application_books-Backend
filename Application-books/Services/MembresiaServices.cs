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
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<MembresiaDto>> GetMembresiaByUserAsync()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Usuario no autenticado."
                };
            }

            var membresiaEntity = await _context.Membresias
                .FirstOrDefaultAsync(m => m.IdUsuario == userId);

            if (membresiaEntity == null)
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "No se encontró la membresía para el usuario proporcionado."
                };
            }

            // Verificar y actualizar el estado de la membresía
            ExpiracionMembresia(membresiaEntity);

            // Recalcular los días restantes dinámicamente
            membresiaEntity.DiasRestantes = CalcularDiasRestantes(membresiaEntity.FechaFin);

            // Guardar cambios si hay actualizaciones
            _context.Membresias.Update(membresiaEntity);
            await _context.SaveChangesAsync();

            var membresiaDto = _mapper.Map<MembresiaDto>(membresiaEntity);

            return new ResponseDto<MembresiaDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Membresía obtenida correctamente.",
                Data = membresiaDto
            };
        }

        public async Task<ResponseDto<MembresiaDto>> CreateOrUpdateMembresiaAsync(MembresiaCreateDto dto)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 400,
                    Status = false,
                    Message = "Usuario no autenticado."
                };
            }

            // Buscar membresía existente del usuario
            var existingMembresia = await _context.Membresias
                .FirstOrDefaultAsync(m => m.IdUsuario == userId);

            if (existingMembresia != null)
            {
                // Extender membresía existente
                if (existingMembresia.FechaFin.HasValue && existingMembresia.FechaFin.Value > DateTime.Now)
                {
                    existingMembresia.FechaFin = CalcularExtension(existingMembresia.FechaFin.Value, dto.TipoMembresia);
                }
                else
                {
                    // Reactivar membresía si está vencida
                    _mapper.Map(dto, existingMembresia);
                    existingMembresia = CalcularMembresia(existingMembresia, dto.TipoMembresia);
                }

                // Actualización del estado si estaba expirada
                ExpiracionMembresia(existingMembresia);

                _context.Membresias.Update(existingMembresia);
                await _context.SaveChangesAsync();

                var updatedMembresiaDto = _mapper.Map<MembresiaDto>(existingMembresia);
                return new ResponseDto<MembresiaDto>
                {
                    StatusCode = 200,
                    Status = true,
                    Message = "Membresía actualizada correctamente.",
                    Data = updatedMembresiaDto
                };
            }

            // Crear nueva membresía si no existe ninguna
            var newMembresia = _mapper.Map<MembresiaEntity>(dto);
            newMembresia.IdUsuario = userId;
            newMembresia = CalcularMembresia(newMembresia, dto.TipoMembresia);

            _context.Membresias.Add(newMembresia);
            await _context.SaveChangesAsync();

            var newMembresiaDto = _mapper.Map<MembresiaDto>(newMembresia);
            return new ResponseDto<MembresiaDto>
            {
                StatusCode = 201,
                Status = true,
                Message = "Membresía creada correctamente.",
                Data = newMembresiaDto
            };
        }

        private DateTime CalcularExtension(DateTime fechaActual, string tipoMembresia)
        {
            return tipoMembresia switch
            {
                "Prueba" => fechaActual.AddDays(7),
                "Premium" => fechaActual.AddMonths(1),
                _ => fechaActual // No se extiende para tipos no válidos
            };
        }

        private MembresiaEntity CalcularMembresia(MembresiaEntity entity, string tipoMembresia)
        {
            switch (tipoMembresia)
            {
                case "Gratis":
                    entity.FechaInicio = DateTime.Now;
                    entity.FechaFin = null;
                    entity.DiasRestantes = 0;
                    break;

                case "Prueba":
                    entity.FechaInicio = DateTime.Now;
                    entity.FechaFin = DateTime.Now.AddDays(7);
                    break;

                case "Premium":
                    entity.FechaInicio = DateTime.Now;
                    entity.FechaFin = DateTime.Now.AddMonths(1);
                    break;

                default:
                    throw new ArgumentException("Tipo de membresía no válida.");
            }

            entity.DiasRestantes = CalcularDiasRestantes(entity.FechaFin);
            return entity;
        }

        private void ExpiracionMembresia(MembresiaEntity entity)
        {
            if (entity.FechaFin.HasValue && entity.FechaFin.Value <= DateTime.Now)
            {
                entity.TipoMembresia = "Gratis";
                entity.FechaFin = null;
                entity.DiasRestantes = 0;
            }
        }

        private int CalcularDiasRestantes(DateTime? fechaFin)
        {
            if (!fechaFin.HasValue || fechaFin.Value <= DateTime.Now)
                return 0;

            return (fechaFin.Value - DateTime.Now).Days;
        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
        }
    }
}