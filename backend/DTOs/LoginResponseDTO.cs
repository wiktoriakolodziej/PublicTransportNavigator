using PublicTransportNavigator.Models;

namespace PublicTransportNavigator.DTOs
{
    public class LoginResponseDTO
    {
        public required UserDTO User { get; set; }
        public required string Token { get; set; }
        public required long ExpirationTime { get; set; }
    }
}
