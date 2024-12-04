using Application_books.Constants;
using Application_books.Dtos.Comentarios;
using Application_books.Dtos.Common;
using Application_books.Dtos.Generos;
using Application_books.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application_books.Controllers
{
    [Route("api/genero")]
    [ApiController]
    public class GeneroController: ControllerBase
    {
        private readonly IGeneroService _generoService;

        public GeneroController(
            IGeneroService generoService
            )
        {
            this._generoService = generoService;
        }

        [HttpGet]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.SUSCRIPTOR}")]
        public async Task<ActionResult<ResponseDto<List<GenerosDto>>>> GetAll()
        {
            var response = await _generoService.GetGenerosListAsync();
            return StatusCode(response.StatusCode, response);
        }

    }
}
