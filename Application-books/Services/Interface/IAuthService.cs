using Application_books.Dtos.Auth;
using Application_books.Dtos.Common;
using System.Security.Claims;

namespace Application_books.Services.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto);
        Task<ResponseDto<LoginResponseDto>> RegisterAsync(RegisterDto dto);
        Task<ResponseDto<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
        ClaimsPrincipal GetTokenPrincipal(string token);
    }
}
