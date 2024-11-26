using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task<RegisterResponseDTO> Register(RegisterUserDTO registerUser);

        Task<LoginResponseDTO> Login (LoginDTO login);
    }
}
