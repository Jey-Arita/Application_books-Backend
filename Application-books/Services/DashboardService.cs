using Application_books.Database;
using Application_books.Dtos.Common;
using Application_books.Dtos.Dashboard;
using Application_books.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application_books.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationbooksContext _booksContext;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;

        public DashboardService(ApplicationbooksContext booksContext, IMapper mapper, IConfiguration configuracion)
        {
            this._booksContext = booksContext;
            this._mapper = mapper;
            PAGE_SIZE = configuracion.GetValue<int>("PageSize");
        }
        public async Task<ResponseDto<DashboardSummaryDto>> GetDashboardSummaryAsync()
        {
            var summary = new DashboardSummaryDto
            {
                TotalLibros = await _booksContext.Libros.CountAsync(),
                TotalUsuarios = await _booksContext.Users.CountAsync(),
                TotalAutores = await _booksContext.Autores.CountAsync(),
                TotalGeneros = await _booksContext.Generos.CountAsync()
            };

            // No necesitas mapear a lista, ya que es solo un objeto
            var summaryDto = _mapper.Map<DashboardSummaryDto>(summary);

            return new ResponseDto<DashboardSummaryDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Resumen del dashboard obtenido correctamente.",
                Data = summaryDto
            };
        }

    }
}
