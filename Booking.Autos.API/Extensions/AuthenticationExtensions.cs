using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Diagnostics;

namespace Booking.Autos.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationExtension(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["SecretKey"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey!)
                    ),

                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                // 🔥 AQUÍ LOS LOGS
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        Debug.WriteLine("🔥 TOKEN HEADER: " + context.Request.Headers["Authorization"]);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Debug.WriteLine("💣 ERROR JWT: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Debug.WriteLine("✅ TOKEN VALIDADO CORRECTAMENTE");
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}