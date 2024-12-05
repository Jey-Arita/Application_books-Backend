using Microsoft.AspNetCore.Mvc;
using Application_books.Services.Interface;
using Application_books.Constants;
using Application_books.Dtos.Common;
using Application_books.Dtos.Generos;
using Microsoft.AspNetCore.Authorization;
using Application_books.Dtos.Libros;
using Application_books.Services;
using Application_books.Dtos.Dashboard;

namespace Application_books.Controllers
{
    [Route("api/estadistica")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("totales")]
        //[Authorize(Roles = $"{RolesConstant.ADMIN}")]
        public async Task<ActionResult<ResponseDto<List<DashboardSummaryDto>>>> GetDashboardSummary()
        {
            var response = await _dashboardService.GetDashboardSummaryAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
