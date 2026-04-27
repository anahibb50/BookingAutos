using Booking.Autos.API.Extensions;
using Booking.Autos.API.Middleware;
using Booking.Autos.API.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 🔥 SERVICES
// ============================================================

// Controllers
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Unifica los 400 automáticos de ApiController con el formato de error del proyecto.
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors
                        .Select(e => NormalizarMensajeValidacion(kvp.Key, e.ErrorMessage))
                        .ToArray()
                );

            var response = new ApiErrorResponse("Errores de validación", errors);
            return new BadRequestObjectResult(response);
        };
    });

static string NormalizarMensajeValidacion(string key, string? rawMessage)
{
    var message = string.IsNullOrWhiteSpace(rawMessage) ? "Valor inválido." : rawMessage;
    var field = ObtenerNombreCampo(key);

    if (message.Contains("The request field is required.", StringComparison.OrdinalIgnoreCase))
        return $"El campo {field} es obligatorio.";

    if (message.Contains("could not be converted to System.Byte", StringComparison.OrdinalIgnoreCase))
        return $"El campo {field} debe ser un número entre 0 y 255.";

    return message;
}

static string ObtenerNombreCampo(string key)
{
    if (string.IsNullOrWhiteSpace(key))
        return "valor";

    var cleaned = key.Trim();

    if (cleaned.StartsWith("$.", StringComparison.Ordinal))
        cleaned = cleaned[2..];

    var segments = cleaned.Split('.', StringSplitOptions.RemoveEmptyEntries);
    var field = segments[^1];

    if (string.IsNullOrWhiteSpace(field))
        return "valor";

    return char.ToLower(field[0], CultureInfo.InvariantCulture) + field[1..];
}

// 🔥 EXTENSIONS (LAS QUE YA CREASTE)
builder.Services.AddProjectServices(builder.Configuration);     // DI (Business + Data + DbContext)
builder.Services.AddSwaggerExtension();                         // Swagger + JWT
builder.Services.AddApiVersioningExtension();                   // Versionado
builder.Services.AddAuthenticationExtension(builder.Configuration); // JWT Auth
builder.Services.AddCorsExtension(builder.Configuration);       // CORS

// ============================================================
// 🔥 BUILD APP
// ============================================================

var app = builder.Build();

// ============================================================
// 🔥 MIDDLEWARE PIPELINE
// ============================================================

// 🔥 Swagger SOLO en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension();
}

// 🔥 Middleware global de errores (MUY IMPORTANTE)
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// 🔥 CORS
app.UseCorsExtension();

// 🔥 AUTH
app.UseAuthentication();
app.UseAuthorization();

// 🔥 Controllers
app.MapControllers();

// ============================================================
// 🔥 RUN
// ============================================================

app.Run();