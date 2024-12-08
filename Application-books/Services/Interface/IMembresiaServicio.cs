using Application_books.Dtos.Common;
using Application_books.Dtos.Membresia;
using Application_books.Dtos.Usuarios;

namespace Application_books.Services.Interface
{
    public interface IMembresiaServicio
    {
        Task<ResponseDto<MembresiaDto>> GetMembresiaByUserAsync();
        Task<ResponseDto<MembresiaDto>> CreateOrUpdateMembresiaAsync(MembresiaCreateDto dto);
       // Task<ResponseDto<MembresiaDto>> EditMembresiaAsync(MembresiaEditDto dto);
    }
}
