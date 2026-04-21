using Booking.Autos.API.Extensions;
using Booking.Autos.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 🔥 SERVICES
// ============================================================

// Controllers
builder.Services.AddControllers();

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