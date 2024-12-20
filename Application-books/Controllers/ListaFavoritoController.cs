﻿using Application_books.Constants;
using Application_books.Dtos.Autor;
using Application_books.Dtos.Common;
using Application_books.Dtos.ListaFavoritos;
using Application_books.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application_books.Controllers
{
    [Route("api/listafavoritos")]
    [ApiController]
    public class ListaFavoritoController : Controller
    {
        private readonly IListaFavoritoServices _listaFavoritoServices;

        public ListaFavoritoController(IListaFavoritoServices listaFavoritoServices)
        {
            this._listaFavoritoServices = listaFavoritoServices;
        }

        [HttpGet()]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<ActionResult<ResponseDto<List<ListaFavoritoDto>>>> GetAll()
        {
            var response = await _listaFavoritoServices.GetListaFavoritoListAsync();
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<ActionResult<ResponseDto<ListaFavoritoDto>>> Create(ListaFavoritoCreateDto dto)
        {
            var respose = await _listaFavoritoServices.CreateAsync(dto);
            return StatusCode(respose.StatusCode, respose);
        }

        [HttpGet("is-favorito/{idLibro}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<IActionResult> IsFavorito(Guid idLibro)
        {
            var result = await _listaFavoritoServices.IsLibroFavoritoAsync(idLibro);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<ActionResult<ResponseDto<ListaFavoritoDto>>> Delete(Guid id)
        {
            var response = await _listaFavoritoServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
