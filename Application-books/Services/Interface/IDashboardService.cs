using Application_books.Dtos.Autor;
using Application_books.Dtos.Common;
using Application_books.Dtos.Dashboard;

namespace Application_books.Services.Interface
{
    public interface IDashboardService
    {
        Task<ResponseDto<DashboardSummaryDto>> GetDashboardSummaryAsync();

    }
}
