using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs.Auth;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
