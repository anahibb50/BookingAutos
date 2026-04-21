using Booking.Autos.Business.DTOs.Auth;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
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
        private readonly IUsuarioAppDataService _usuarioDataService;
        private readonly IUsuarioRolDataService _usuarioRolDataService;
        private readonly IConfiguration _config;

        public AuthService(
            IUsuarioAppDataService usuarioDataService,
            IUsuarioRolDataService usuarioRolDataService,
            IConfiguration config)
        {
            _usuarioDataService = usuarioDataService;
            _usuarioRolDataService = usuarioRolDataService;
            _config = config;
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<LoginResponse> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ValidationException(new List<string> { "El username es obligatorio." });

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException(new List<string> { "La contraseña es obligatoria." });

            // 🔍 USUARIO
            var usuario = await _usuarioDataService
                .GetByUsernameAsync(request.Username, cancellationToken);

            if (usuario == null)
                throw new UnauthorizedBusinessException("Usuario o contraseña incorrectos.");

            if (!usuario.Activo)
                throw new UnauthorizedBusinessException("El usuario está inactivo.");

            // 🔐 PASSWORD (temporal)
            if (usuario.PasswordHash != request.Password)
                throw new UnauthorizedBusinessException("Usuario o contraseña incorrectos.");

            // 🔥 ROLES (CORRECTO)
            var roles = await _usuarioRolDataService
                .GetRolesByUsuarioAsync(usuario.Id, cancellationToken);

            // 🔥 TOKEN
            var (token, expira) = GenerarJwt(usuario.Id, usuario.Username, roles);

            return new LoginResponse
            {
                Token = token,
                Tipo = "Bearer",
                ExpiraEn = expira,

                IdUsuario = usuario.Id,
                Username = usuario.Username,
                Correo = usuario.Correo ?? "",

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

            return await Task.FromResult(true);
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ValidationException(new List<string> { "Refresh token inválido." });

            var nuevoToken = Guid.NewGuid().ToString();

            return new LoginResponse
            {
                Token = nuevoToken,
                Tipo = "Bearer",
                ExpiraEn = DateTime.UtcNow.AddHours(1)
            };
        }

        // =========================
        // LOGOUT
        // =========================
        public async Task<bool> LogoutAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            return await Task.FromResult(true);
        }

        // =========================
        // JWT
        // =========================
        private (string token, DateTime expira) GenerarJwt(
            int idUsuario,
            string username,
            List<string> roles)
        {
            var secret = _config["JwtSettings:SecretKey"];
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expira = DateTime.UtcNow.AddHours(1);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, idUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var rol in roles)
                claims.Add(new Claim(ClaimTypes.Role, rol));

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expira,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expira);
        }
    }
}