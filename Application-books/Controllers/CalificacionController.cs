﻿using Application_books.Constants;
using Application_books.Dtos.Calificacion;
using Application_books.Dtos.Common;
using Application_books.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application_books.Controllers
{
    [Route("api/calificacion")]
    [ApiController]
    public class CalificacionController : Controller
    {
        private readonly ICalificacionesServices _calificacionesServices;

        public CalificacionController(ICalificacionesServices calificacionesServices) 
        {
            this._calificacionesServices = calificacionesServices;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrSubscriberAndPremium")]
        public async Task<ActionResult<ResponseDto<List<CalificacionDto>>>> GetAll()
        {
            var response = await _calificacionesServices.GetCalificacionesListAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<ActionResult<ResponseDto<CalificacionDto>>> Get(Guid id)
        {
            var response = await _calificacionesServices.GetCalificacionByAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<ActionResult<ResponseDto<CalificacionDto>>> Create(CalificacionCreateDto dto)
        {
            var respose = await _calificacionesServices.CreateAsync(dto);
            return StatusCode(respose.StatusCode, respose);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<ActionResult<ResponseDto<CalificacionDto>>> Edit(CalificacionEditDto dto, Guid id)
        {
            var responde = await _calificacionesServices.EditAsync(dto, id);
            return StatusCode(responde.StatusCode, responde);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<ActionResult<ResponseDto<CalificacionDto>>> Delete(Guid id)
        {
            var response = await _calificacionesServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
