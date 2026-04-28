namespace Booking.Autos.Business.DTOs.Auth
{
    public class LoginResponse
    {
        // 🔐 Token JWT
        public string Token { get; set; }

        public string Tipo { get; set; } = "Bearer";

        public DateTime ExpiraEn { get; set; }

        // 👤 Info usuario
        public int IdUsuario { get; set; }

        public string Username { get; set; }

        public string Correo { get; set; }

        /// <summary>Cliente vinculado en <c>seguridad.USUARIOAPP.id_cliente</c> (reservas lo requieren).</summary>
        public int? IdCliente { get; set; }

        // 🔥 ROLES (CLAVE)
        public List<string> Roles { get; set; } = new();
    }
}