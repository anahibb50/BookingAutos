namespace Booking.Autos.API.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;

        public string Message { get; set; } = "Operación exitosa";

        public T? Data { get; set; }

        public ApiResponse(T data, string message = "Operación exitosa")
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Ok(T data, string message = "Operación exitosa")
        {
            return new ApiResponse<T>(data, message);
        }
    }
}