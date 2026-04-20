using Booking.Autos.Business.DTOs.Auth;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Booking.Autos.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Usuario) || string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException(new List<string> { "El usuario y la contraseña son obligatorios." });

            var usuarioDB = await _unitOfWork.UsuariosApp
                .GetByUserNameAsync(request.Usuario, cancellationToken);

            if (usuarioDB == null)
                throw new UnauthorizedBusinessException("Usuario o contraseña incorrectos.");

            if (!usuarioDB.Activo)
                throw new UnauthorizedBusinessException("El usuario está inactivo.");

            if (usuarioDB.PasswordHash != request.Password)
                throw new UnauthorizedBusinessException("Usuario o contraseña incorrectos.");

            // 🔥 roles
            var roles = usuarioDB.UsuarioRoles?
                .Where(r => r.Rol != null)
                .Select(r => r.Rol.Nombre)
                .Distinct()
                .ToList() ?? new List<string>();

            // 🔥 TOKEN (método separado ✔)
            var token = GenerarTokenJWT(usuarioDB, roles);

            return new LoginResponse
            {
                Token = token,
                Tipo = "Bearer",
                ExpiraEn = 3600,

                Usuario = usuarioDB.UserName,
                NombreCompleto = usuarioDB.NombreCompleto,
                Correo = usuarioDB.CorreoElectronico,
                Roles = roles
            };
        }

        // =========================
        // VALIDAR TOKEN
        // =========================
        public async Task<bool> ValidarTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            // 🔥 validación básica (puedes mejorar luego con JWT real)
            return await Task.FromResult(true);
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ValidationException(new List<string> { "Refresh token inválido." });

            // 🔥 en producción validarías el refreshToken

            var nuevoToken = Guid.NewGuid().ToString();

            return new LoginResponse
            {
                Token = nuevoToken,
                Tipo = "Bearer",
                ExpiraEn = 3600
            };
        }

        // =========================
        // LOGOUT
        // =========================
        public async Task<bool> LogoutAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            // 🔥 aquí puedes implementar blacklist
            return await Task.FromResult(true);
        }

        // =========================
        // 🔥 GENERAR JWT (SEPARADO ✔)
        // =========================
        private string GenerarTokenJWT(UsuarioAppEntity usuario, List<string> roles)
        {
            var jwtSecret = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico ?? ""),
                new Claim("nombreCompleto", usuario.NombreCompleto ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}