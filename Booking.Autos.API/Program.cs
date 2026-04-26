using Booking.Autos.API.Extensions;
using Booking.Autos.API.Middleware;
using Booking.Autos.API.Models.Common;
using Microsoft.AspNetCore.Mvc;

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
                        .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                            ? "Valor inválido."
                            : e.ErrorMessage)
                        .ToArray()
                );

            var response = new ApiErrorResponse("Errores de validación", errors);
            return new BadRequestObjectResult(response);
        };
    });

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