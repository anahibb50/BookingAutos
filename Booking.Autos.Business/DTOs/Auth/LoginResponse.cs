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

        // 🔥 ROLES (CLAVE)
        public List<string> Roles { get; set; } = new();
    }
}