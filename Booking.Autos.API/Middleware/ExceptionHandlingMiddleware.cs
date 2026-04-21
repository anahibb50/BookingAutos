using Booking.Autos.API.Models.Common;
using Booking.Autos.Business.Exceptions;
using System.Net;
using System.Text.Json;

namespace Booking.Autos.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiErrorResponse("Error interno del servidor");
            var statusCode = HttpStatusCode.InternalServerError;

            switch (ex)
            {
                // 🔴 VALIDACIONES
                // ✅ ESTO ES LO CORRECTO
                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ApiErrorResponse(
                        "Errores de validación",
                        validationEx.Errors
                    );
                    break;

                // 🔐 AUTH
                case UnauthorizedBusinessException unauthorizedEx:
                    statusCode = HttpStatusCode.Unauthorized;

                    response = new ApiErrorResponse(
                        unauthorizedEx.Message
                    );
                    break;

                // ❗ OTROS
                default:
                    response = new ApiErrorResponse(
                        ex.Message, // 🔥 mensaje real
                        new List<string> { ex.StackTrace ?? "Sin stacktrace" } // 🔥 detalle
                    );
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}