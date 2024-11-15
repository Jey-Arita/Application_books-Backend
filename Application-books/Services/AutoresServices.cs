﻿using Application_books.Database;
using Application_books.Database.Entitties;
using Application_books.Dtos.Autor;
using Application_books.Dtos.Common;
using Application_books.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application_books.Services
{
    public class AutoresServices : IAutorServices
    {
        private readonly ApplicationbooksContext _context;
        private readonly IMapper _mapper;

        public AutoresServices(ApplicationbooksContext context, IMapper mapper) 
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<ResponseDto<List<AutorDto>>> GetAutorListAsync()
        {
            var autorEntity = await _context.Autores.Include(a => a.Libros).ToListAsync();
            var autorDtos = _mapper.Map<List<AutorDto>>(autorEntity);

            return new ResponseDto<List<AutorDto>>
            {
                StatusCode = 200,
                Status = true,
                Message = "Lista de autores obtenida correctamente.",
                Data = autorDtos,
            };
        }
        public async Task<ResponseDto<AutorDto>> GetAutorByAsync(Guid id)
        {
            var autorEntity = await _context.Autores.Include(a => a.Libros).FirstOrDefaultAsync(c => c.Id == id);
            if (autorEntity == null)
            {
                return new ResponseDto<AutorDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = "No se encontro registro."
                };
            }
            var autorDto = _mapper.Map<AutorDto>(autorEntity);
            return new ResponseDto<AutorDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro obtenido correctamente.",
                Data = autorDto,
            };
        }
        public async Task<ResponseDto<AutorDto>> CreateAsync(AutorCreateDto dto)
        {
            var autorEntity = _mapper.Map<AutorEntity>(dto);
            _context.Autores.Add(autorEntity);
            await _context.SaveChangesAsync();

            var autorDto = _mapper.Map<AutorDto>(autorEntity);

            return new ResponseDto<AutorDto>
            {
                StatusCode = 201,
                Status = true,
                Message = "Registro creado coreectamente.",
                Data = autorDto,
            };
        }
        public async Task<ResponseDto<AutorDto>> EditAsync(AutorEditDto dto, Guid id)
        {
            var autorEntity = await _context.Autores.FirstOrDefaultAsync(e => e.Id == id);
            if (autorEntity is null)
            {
                return new ResponseDto<AutorDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = $"El registro no se encontro"
                };
            }
            _mapper.Map(dto, autorEntity);
            _context.Autores.Update(autorEntity);
            await _context.SaveChangesAsync();

            var autorDto = _mapper.Map<AutorDto>(autorEntity);
            return new ResponseDto<AutorDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro editado correctamente.",
                Data = autorDto
            };
        }
        public async Task<ResponseDto<AutorDto>> DeleteAsync(Guid id)
        {
            var autorEntity = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autorEntity == null)
            {
                return new ResponseDto<AutorDto>
                {
                    StatusCode = 404,
                    Status = false,
                    Message = $"No se encontro el registro"
                };
            }

            _context.Autores.Remove(autorEntity);
            await _context.SaveChangesAsync();
            return new ResponseDto<AutorDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro borrado correctamente"
            };
        }
       
    }
}
