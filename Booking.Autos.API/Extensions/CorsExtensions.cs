using Microsoft.Extensions.Configuration;

namespace Booking.Autos.API.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsExtension(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(origins!)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }

        // 🔥 ESTE TE FALTA
        public static WebApplication UseCorsExtension(this WebApplication app)
        {
            app.UseCors("CorsPolicy");
            return app;
        }
    }
}