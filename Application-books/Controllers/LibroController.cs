using Application_books.Constants;
using Application_books.Dtos.Common;
using Application_books.Dtos.Libros;
using Application_books.Dtos.Membresia;
using Application_books.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application_books.Controllers
{
    [Route("api/libros")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ILibrosServices _librosServices;
        private readonly IMembresiaServicio _membresiaServicio;

        public LibroController(ILibrosServices librosServices, IMembresiaServicio membresiaServicio)
        {
            this._librosServices = librosServices;
            this._membresiaServicio = membresiaServicio;
        }

        [HttpGet]
        [Authorize(Roles = $"{RolesConstant.SUSCRIPTOR},{RolesConstant.ADMIN}")]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<LibroDto>>>>> PaginationList(string searchTerm, int page = 1) 
        {
            var response = await _librosServices.GetLibroListAsync(searchTerm, page);

           var responses = await _membresiaServicio.GetMembresiaByUserAsync();

            return StatusCode(response.StatusCode, new
            {
                response.Status,
                response.Message,
                response.Data,
            });
        }

        [HttpGet("destacados")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<List<LibroDto>>>> GetAll() 
        {
                var response = await _librosServices.GetLibroListDestacadosAsync();
                return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrSubscriberAndPremium")]
        public async Task<ActionResult<ResponseDto<LibroDto>>> Get(Guid id)
        {
            var response = await _librosServices.GetLibroByAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("genero/{generoId}")]
        [Authorize(Policy = "AdminOrSubscriberAndPremium")]
        public async Task<ActionResult<ResponseDto<List<LibroDto>>>> GetLibrosByGenero(Guid generoId)
        {
            var response = await _librosServices.GetLibrosByGeneroAsync(generoId);
            return StatusCode(response.StatusCode, response);
        }



        [HttpPost]
        [Authorize(Roles = $"{RolesConstant.ADMIN}")]
        public async Task<ActionResult<ResponseDto<LibroDto>>> Create(LibroCreateDto dto)
        {
            var respose = await _librosServices.CreateAsync(dto);
            return StatusCode(respose.StatusCode, respose);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}")]
        public async Task<ActionResult<ResponseDto<LibroDto>>> Edit(LibroEditDto dto, Guid id)
        {
            var responde = await _librosServices.EditAsync(dto, id);
            return StatusCode(responde.StatusCode, responde);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}")]
        public async Task<ActionResult<ResponseDto<LibroDto>>> Delete(Guid id)
        {
            var response = await _librosServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }  
}
 
