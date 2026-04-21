using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Services;

using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Services;


using Booking.Autos.DataAccess.Context;

using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ============================================================
            // 🔥 DB CONTEXT
            // ============================================================
            services.AddDbContext<BookingAutoDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // ============================================================
            // 🔥 UNIT OF WORK
            // ============================================================
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ============================================================
            // 🔥 BUSINESS SERVICES
            // ============================================================
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IVehiculoService, VehiculoService>();
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IExtraService, ExtraService>();
            services.AddScoped<IFacturaService, FacturaService>();
            services.AddScoped<ILocalizacionService, LocalizacionService>();
            services.AddScoped<IMarcaService, MarcaService>();
            services.AddScoped<IPaisService, PaisService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioRolService, UsuarioRolService>();
            services.AddScoped<IConductorReservaService, ConductorReservaService>();
            services.AddScoped<IReservaExtraService, ReservaExtraService>();
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<ICiudadService, CiudadService>();
            services.AddScoped<IConductorService, ConductorService>();

            // ============================================================
            // 🔥 DATA SERVICES
            // ============================================================
            services.AddScoped<IVehiculoDataService, VehiculoDataService>();
            services.AddScoped<IReservaDataService, ReservaDataService>();
            services.AddScoped<IExtraDataService, ExtraDataService>();
            services.AddScoped<IFacturaDataService, FacturaDataService>();
            services.AddScoped<ILocalizacionDataService, LocalizacionDataService>();
            services.AddScoped<IMarcaDataService, MarcaDataService>();
            services.AddScoped<IPaisDataService, PaisDataService>();
            services.AddScoped<IRolDataService, RolDataService>();
            services.AddScoped<IUsuarioRolDataService, UsuarioRolDataService>();
            services.AddScoped<IConductorReservaDataService, ConductorReservaDataService>();
            services.AddScoped<IReservaExtraDataService, ReservaExtraDataService>();
            services.AddScoped<IUsuarioAppDataService, UsuarioAppDataService>();
            services.AddScoped<ICategoriaDataService, CategoriaDataService>();
            services.AddScoped<IClienteDataService, ClienteDataService>();
            services.AddScoped<ICiudadDataService, CiudadDataService>();
            services.AddScoped<IConductorDataService, ConductorDataService>();

            return services;
        }
    }
}