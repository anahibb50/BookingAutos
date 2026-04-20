namespace Booking.Autos.Business.DTOs.Auth
{
    public class LoginRequest
    {
        // 👤 Puede ser username o email
        public string Username { get; set; }=null!;

        public string Password { get; set; }=null!;
    }
}