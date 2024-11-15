﻿using Application_books.Database;
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

        public CalificacionesServices(ApplicationbooksContext context, IMapper mapper, IAuthService authService) 
        {
            this._context = context;
            this._mapper = mapper;
            this._authService = authService;
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
        public async Task<ResponseDto<CalificacionDto>> GetCalificacionByAsync(Guid id)
        {
            var calificacionEntity = await _context.Calificaciones.FirstOrDefaultAsync(c => c.Id == id);
            if (calificacionEntity == null)
            {
                return new ResponseDto<CalificacionDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "No se encontro registro."
                };
            }
            var calificacionDto = _mapper.Map<CalificacionDto>(calificacionEntity);
            return new ResponseDto<CalificacionDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro obtenido correctamente.",
                Data = calificacionDto,
            };
        }
        public async Task<ResponseDto<CalificacionDto>> CreateAsync(CalificacionCreateDto dto)
        {
            var calificacionEntity = _mapper.Map<CalificacionEntity>(dto);

            //var userIdString = _authService.GetUserId();
            //var userIdString = _authService.GetUserId();

            _context.Calificaciones.Add(calificacionEntity);
            await _context.SaveChangesAsync();

            var calificacionDto = _mapper.Map<CalificacionDto>(calificacionEntity);

            return new ResponseDto<CalificacionDto>
            {
                StatusCode = 201,
                Status = true,
                Message = "Registro creado coreectamente.",
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
