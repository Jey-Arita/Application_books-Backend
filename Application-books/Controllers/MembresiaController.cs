using Application_books.Constants;
using Application_books.Dtos.Common;
using Application_books.Dtos.Membresia;
using Application_books.Services;
using Application_books.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application_books.Controllers
{
    [Route("api/membresia")]
    [ApiController]
    public class MembresiaController : Controller
    {
        private readonly IMembresiaServicio _membresiaServicio;

        public MembresiaController(IMembresiaServicio membresiaServicio) 
        {
            this._membresiaServicio = membresiaServicio;
        }
        [HttpGet("count")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}")]
        public async Task<ActionResult<ResponseDto<MembresiaCountDto>>> GetMembresiasCount()
        {
            var response = await _membresiaServicio.GetMembresiasCountAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<MembresiaDto>>> GetById()
        {
            var response = await _membresiaServicio.GetMembresiaByUserAsync();
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ResponseDto<MembresiaDto>>> Create(MembresiaCreateDto dto)
        {
            var response = await _membresiaServicio.CreateOrUpdateMembresiaAsync(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
