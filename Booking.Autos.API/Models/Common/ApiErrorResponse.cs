namespace Booking.Autos.API.Models.Common
{
    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "Se produjeron uno o más errores.";
        public IDictionary<string, string[]>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // ✅ Constructor principal
        public ApiErrorResponse(string message, IDictionary<string, string[]>? errors = null)
        {
            Message = message;
            Errors = errors;
        }

        // ✅ Constructor para lista simple de errores
        public ApiErrorResponse(string message, IEnumerable<string> errors)
        {
            Message = message;
            Errors = new Dictionary<string, string[]>
        {
            { "errors", errors.ToArray() }
        };
        }
    }
}