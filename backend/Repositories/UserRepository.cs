using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PublicTransportNavigator.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PublicTransportNavigatorContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private const string TokenExpirationTime = "TokenExpirationInMinutes";
        private readonly int? _tokenExpirationInMinutes;

        public UserRepository(PublicTransportNavigatorContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            try
            {
                _tokenExpirationInMinutes = Initialize();
            }
            catch (Exception ex)
            {
            }
        }
        public async Task<LoginResponseDTO> Login(LoginDTO login)
        {
            if (_tokenExpirationInMinutes == null)
            {
                throw new Exception($"Value of {TokenExpirationTime} field is not correctly specified");
            }
            var user = await (
                    from u in _context.Users
                    where u.Name == login.Login
                    select u)
                .ToListAsync();
            if (user == null) throw new UnauthorizedAccessException("Bad credentials");
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user[0].Password))
                throw new UnauthorizedAccessException("Bad credentials");
            var expirationTime = DateTime.UtcNow.AddMinutes(_tokenExpirationInMinutes.Value);
            var token = GenerateJwtToken(user[0], expirationTime);
            
            return new LoginResponseDTO
            {
                User = _mapper.Map<UserDTO>(user[0]),
                Token = token,
                ExpirationTime = new DateTimeOffset(expirationTime).ToUnixTimeSeconds(),
            };
        }
        public async Task<RegisterResponseDTO> Register(RegisterUserDTO registerUser)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
                var user = new User
                {
                    Name = registerUser.UserName,
                    Surname = registerUser.UserSurname,
                    Password = hashedPassword,
                    LastModified = DateTime.UtcNow
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                var response = new RegisterResponseDTO
                {
                    Message = "Registration successful"
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new RegisterResponseDTO
                {
                    Message = "Registration failed"
                };
                return response;
            }
        }

        private string GenerateJwtToken(User user, DateTime expirationTime)
        {
            if (_tokenExpirationInMinutes == null)
            {
                throw new Exception($"Value of {TokenExpirationTime} field is not correctly specified");
            }
            var jwtSection = _configuration.GetSection("Jwt");
            if (!jwtSection.Exists()
                || string.IsNullOrEmpty(_configuration["Jwt:Key"])
                || string.IsNullOrEmpty(_configuration["Jwt:Audience"])
                || string.IsNullOrEmpty(_configuration["Jwt:Issuer"]))
            {
                throw new Exception("JWT configuration properties are not provided");
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,  DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expirationTime,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private int Initialize()
        {
            if (!int.TryParse(_configuration[$"Jwt:{TokenExpirationTime}"], out var result))
                throw new FormatException();
            return result;
        }
    }
}
