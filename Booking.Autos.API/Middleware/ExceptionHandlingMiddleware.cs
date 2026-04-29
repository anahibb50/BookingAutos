using Booking.Autos.API.Models.Common;
using Booking.Autos.Business.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ApiErrorResponse("Errores de validación", validationEx.Errors);
                    break;

                case NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    response = new ApiErrorResponse(notFoundEx.Message);
                    break;

                case UnauthorizedBusinessException unauthorizedEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = new ApiErrorResponse(unauthorizedEx.Message);
                    break;

                case BusinessException businessEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ApiErrorResponse(businessEx.Message);
                    break;

                case DbUpdateException dbUpdateEx when IsLocalizacionNombreCiudadConstraintViolation(dbUpdateEx):
                    statusCode = HttpStatusCode.Conflict;
                    response = new ApiErrorResponse("Ya existe una localización con ese nombre en la ciudad seleccionada.");
                    break;

                case DbUpdateException dbUpdateEx when IsUniqueConstraintViolation(dbUpdateEx):
                    statusCode = HttpStatusCode.Conflict;
                    response = new ApiErrorResponse("Ya existe un registro con un valor único duplicado.");
                    break;

                case DbUpdateException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ApiErrorResponse("No se pudo guardar la información. Verifica los datos e inténtalo nuevamente.");
                    break;

                default:
                    response = new ApiErrorResponse("Ocurrió un error interno. Intenta nuevamente más tarde.");
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException exception)
        {
            return exception.InnerException is SqlException sqlException
                && (sqlException.Number == 2601 || sqlException.Number == 2627);
        }

        private static bool IsLocalizacionNombreCiudadConstraintViolation(DbUpdateException exception)
        {
            var errorText = exception.InnerException?.Message ?? exception.Message;

            return errorText.Contains("UQ_Localizacion_Nombre_Ciudad", StringComparison.OrdinalIgnoreCase);
        }
    }
}
