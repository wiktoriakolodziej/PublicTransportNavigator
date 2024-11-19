using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task<UserDTO> Register(RegisterUserDTO registerUser);

        Task<LoginResponseDTO> Login (LoginDTO login);
    }
}
