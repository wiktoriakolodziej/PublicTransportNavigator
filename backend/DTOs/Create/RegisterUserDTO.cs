namespace PublicTransportNavigator.DTOs.Create
{
    public class RegisterUserDTO
    {
        public required string UserName { get; set; }
        public required string UserSurname { get; set; }
        public required string Password { get; set; }
    }
}
