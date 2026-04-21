namespace Booking.Autos.API.Models.Common
{
    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = "Se produjeron uno o más errores.";

        public IDictionary<string, string[]>? Errors { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ApiErrorResponse(string message, IDictionary<string, string[]>? errors = null)
        {
            Message = message;
            Errors = errors;
        }
    }
}