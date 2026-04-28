using Booking.Autos.Business.DTOs.Auth;
using Booking.Autos.Business.DTOs.Usuario;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Clientes;
using Booking.Autos.DataManagement.Models.UsuarioApp;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;

namespace Booking.Autos.Business.Services
{
    public class AuthService : IAuthService
    {
        /// <summary>Claim personalizado para que el front lea el cliente vinculado desde el JWT.</summary>
        public const string JwtClaimIdCliente = "id_cliente";

        private readonly IUsuarioAppDataService _usuarioDataService;
        private readonly IUsuarioRolDataService _usuarioRolDataService;
        private readonly IRolDataService _rolDataService;
        private readonly IClienteDataService _clienteDataService;
        private readonly ICiudadDataService _ciudadDataService;
        private readonly IConfiguration _config;

        public AuthService(
            IUsuarioAppDataService usuarioDataService,
            IUsuarioRolDataService usuarioRolDataService,
            IRolDataService rolDataService,
            IClienteDataService clienteDataService,
            ICiudadDataService ciudadDataService,
            IConfiguration config)
        {
            _usuarioDataService = usuarioDataService;
            _usuarioRolDataService = usuarioRolDataService;
            _rolDataService = rolDataService;
            _clienteDataService = clienteDataService;
            _ciudadDataService = ciudadDataService;
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
                throw new UnauthorizedBusinessException("No se encuentra el usuario.");

            if (!usuario.Activo)
                throw new UnauthorizedBusinessException("El usuario está inactivo.");

            // 🔐 PASSWORD
            if (!VerificarPassword(request.Password, usuario.PasswordHash, usuario.PasswordSalt))
                throw new UnauthorizedBusinessException("Usuario o contraseña incorrectos.");

            // 🔥 ROLES (CORRECTO)
            var roles = await _usuarioRolDataService
                .GetRolesByUsuarioAsync(usuario.Id, cancellationToken);

            // 🔥 TOKEN (incluye id_cliente cuando el usuario está vinculado a personas.clientes)
            var (token, expira) = GenerarJwt(usuario.Id, usuario.Username, roles, usuario.IdCliente);

            return new LoginResponse
            {
                Token = token,
                Tipo = "Bearer",
                ExpiraEn = expira,

                IdUsuario = usuario.Id,
                Username = usuario.Username,
                Correo = usuario.Correo ?? "",
                IdCliente = usuario.IdCliente,

                Roles = roles
            };
        }

        // =========================
        // REGISTER (CLIENTE)
        // =========================
        public async Task<LoginResponse> RegisterAsync(
            CrearUsuarioRequest request,
            CancellationToken cancellationToken = default)
        {
            var erroresFormato = AuthValidator.ValidarFormatoRegistroCliente(request);
            if (erroresFormato.Count > 0)
                throw new ValidationException(erroresFormato.ToList());

            NormalizarCamposRegistro(request);

            if (await _usuarioDataService.ExistsByUsernameAsync(request.Username, cancellationToken))
                throw new ValidationException(new List<string> { "Ya existe un usuario con ese nombre de usuario." });

            if (await _usuarioDataService.ExistsByCorreoAsync(request.Correo, cancellationToken))
                throw new ValidationException(new List<string> { "Ya existe un usuario registrado con ese correo electrónico." });

            var idCliente = await ResolverIdClienteParaRegistroAsync(request, cancellationToken);

            var rolCliente = await _rolDataService.GetByNombreAsync("CLIENTE", cancellationToken);
            if (rolCliente == null || !rolCliente.Activo || rolCliente.Estado != "ACT")
                throw new ValidationException(new List<string> { "El rol CLIENTE no existe o está inactivo." });

            UsuarioAppDataModel usuarioCreado;
            try
            {
                usuarioCreado = await _usuarioDataService.CreateAsync(
                    new UsuarioAppDataModel
                    {
                        Username = request.Username,
                        Correo = request.Correo,
                        IdCliente = idCliente,
                        Estado = "ACT",
                        Activo = true,
                        EsEliminado = false,
                        FechaRegistroUtc = DateTime.UtcNow,
                        CreadoPorUsuario = "self-register"
                    },
                    request.Password,
                    cancellationToken);
            }
            catch (Exception ex) when (
                string.Equals(ex.Message, "El username ya existe", StringComparison.Ordinal)
                || string.Equals(ex.Message, "El correo ya existe", StringComparison.Ordinal))
            {
                throw new ValidationException(new List<string>
                {
                    string.Equals(ex.Message, "El username ya existe", StringComparison.Ordinal)
                        ? "Ya existe un usuario con ese nombre de usuario."
                        : "Ya existe un usuario registrado con ese correo electrónico."
                });
            }

            await _usuarioRolDataService.AssignAsync(
                new UsuarioRolDataModel
                {
                    IdUsuario = usuarioCreado.Id,
                    IdRol = rolCliente.Id,
                    Estado = "ACT",
                    Activo = true,
                    EsEliminado = false,
                    FechaRegistroUtc = DateTime.UtcNow,
                    CreadoPorUsuario = "self-register"
                },
                cancellationToken);

            var roles = await _usuarioRolDataService.GetRolesByUsuarioAsync(usuarioCreado.Id, cancellationToken);
            var (token, expira) = GenerarJwt(usuarioCreado.Id, usuarioCreado.Username, roles, usuarioCreado.IdCliente);

            return new LoginResponse
            {
                Token = token,
                Tipo = "Bearer",
                ExpiraEn = expira,
                IdUsuario = usuarioCreado.Id,
                Username = usuarioCreado.Username,
                Correo = usuarioCreado.Correo ?? "",
                IdCliente = usuarioCreado.IdCliente,
                Roles = roles
            };
        }

        private static void NormalizarCamposRegistro(CrearUsuarioRequest request)
        {
            request.Username = request.Username.Trim();
            request.Correo = request.Correo.Trim();
            request.Password = request.Password.Trim();
            request.Identificacion = request.Identificacion.Trim();

            request.Nombre = string.IsNullOrWhiteSpace(request.Nombre) ? null : request.Nombre.Trim();
            request.Apellido = string.IsNullOrWhiteSpace(request.Apellido) ? null : request.Apellido.Trim();
            request.TipoIdentificacion = string.IsNullOrWhiteSpace(request.TipoIdentificacion)
                ? null
                : request.TipoIdentificacion.Trim();
            request.Direccion = string.IsNullOrWhiteSpace(request.Direccion) ? null : request.Direccion.Trim();
            request.Genero = string.IsNullOrWhiteSpace(request.Genero) ? null : request.Genero.Trim();
            request.Telefono = string.IsNullOrWhiteSpace(request.Telefono) ? null : request.Telefono.Trim();
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
            List<string> roles,
            int? idCliente = null)
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

            if (idCliente.HasValue)
                claims.Add(new Claim(JwtClaimIdCliente, idCliente.Value.ToString(CultureInfo.InvariantCulture)));

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expira,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expira);
        }

        private static bool VerificarPassword(string passwordPlano, string passwordHash, string? passwordSalt)
        {
            // Compatibilidad temporal: si hay data vieja en texto plano.
            if (passwordHash == passwordPlano)
                return true;

            if (string.IsNullOrWhiteSpace(passwordSalt))
                return false;

            var hashCalculado = HashPassword(passwordPlano, passwordSalt);
            return hashCalculado == passwordHash;
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes($"{password}:{salt}");
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private async Task<int> ResolverIdClienteParaRegistroAsync(
            CrearUsuarioRequest request,
            CancellationToken cancellationToken)
        {
            var identificacion = request.Identificacion!;
            var clienteExistente = await _clienteDataService.GetByIdentificacionAsync(identificacion, cancellationToken);

            // Si el cliente ya existe: solo se vincula al usuario nuevo (no se modifican datos del cliente).
            if (clienteExistente is not null)
                return clienteExistente.Id;

            if (string.IsNullOrWhiteSpace(request.Nombre) || string.IsNullOrWhiteSpace(request.Apellido))
                throw new ValidationException(new List<string> { "Para crear cliente nuevo debes enviar nombre y apellido." });

            if (!request.IdCiudad.HasValue)
                throw new ValidationException(new List<string> { "Para crear cliente nuevo debes enviar idCiudad." });

            var ciudad = await _ciudadDataService.GetByIdAsync(request.IdCiudad.Value, cancellationToken);
            if (ciudad is null)
                throw new ValidationException(new List<string> { "La ciudad enviada no existe." });

            var tipoIdentificacion = string.IsNullOrWhiteSpace(request.TipoIdentificacion)
                ? "CEDULA"
                : request.TipoIdentificacion.Trim().ToUpperInvariant();

            var genero = string.IsNullOrWhiteSpace(request.Genero)
                ? "N"
                : request.Genero.Trim().ToUpperInvariant();

            if (genero.Length > 1)
                genero = genero[..1];

            var clienteCreado = await _clienteDataService.CreateAsync(
                new ClienteDataModel
                {
                    Nombre = request.Nombre!,
                    Apellido = request.Apellido!,
                    TipoIdentificacion = tipoIdentificacion,
                    Identificacion = identificacion,
                    IdCiudad = request.IdCiudad.Value,
                    Direccion = request.Direccion?.Trim(),
                    Genero = genero,
                    Telefono = request.Telefono?.Trim(),
                    Email = request.Correo.Trim(),
                    Estado = "ACT",
                    CreadoPorUsuario = "self-register",
                    ServicioOrigen = "API"
                },
                cancellationToken);

            return clienteCreado.Id;
        }
    }
}