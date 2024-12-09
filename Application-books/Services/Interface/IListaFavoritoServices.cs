using Application_books.Dtos.Common;
using Application_books.Dtos.ListaFavoritos;

namespace Application_books.Services.Interface
{
    public interface IListaFavoritoServices
    {
        Task<ResponseDto<ListaFavoritoDto>> CreateAsync(ListaFavoritoCreateDto dto);
        Task<ResponseDto<bool>> IsLibroFavoritoAsync(Guid idLibro);
        Task<ResponseDto<ListaFavoritoDto>> DeleteAsync(Guid id);
        Task<ResponseDto<List<ListaFavoritoDto>>> GetListaFavoritoListAsync();
    }
}
