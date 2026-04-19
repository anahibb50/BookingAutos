using Booking.Autos.DataAccess.Repositories.Interfaces;
using Booking.Autos.DataAccess.Queries.Categorias;
using Booking.Autos.DataAccess.Queries.Extras;
using Booking.Autos.DataAccess.Queries.Localizaciones;
using Booking.Autos.DataAccess.Queries.Vehiculos;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        // 🔗 Repositories (CRUD)
        IClienteRepository Clientes { get; }
        IConductorRepository Conductores { get; }
        IConductorReservaRepository ConductoresReservas { get; }
        IExtraRepository Extras { get; }
        IFacturaRepository Facturas { get; }
        ILocalizacionRepository Localizaciones { get; }
        IMarcaRepository Marcas { get; }
        IPaisRepository Paises { get; }
        IReservaRepository Reservas { get; }
        IReservaExtraRepository ReservasExtras { get; }
        IUsuarioAppRepository UsuariosApp { get; }
        IVehiculoRepository Vehiculos { get; }
        ICategoriaRepository Categorias { get; }
        ICiudadRepository Ciudades { get; }

        // 🔍 QueryRepositories (LECTURA)
        CategoriaListQueryRepository CategoriaListQueryRepository { get; }
        ExtraListQueryRepository ExtraQueries { get; }

        LocalizacionListQueryRepository LocalizacionListQueries { get; }
        LocalizacionDetalleQueryRepository LocalizacionDetalleQueries { get; }

        VehiculoBusquedaQueryRepository VehiculoBusquedaQueries { get; }
        VehiculoDetalleQueryRepository VehiculoDetalleQueries { get; }
        VehiculoDisponibilidadQueryRepository VehiculoDisponibilidadQueries { get; }

        // 💾 Persistencia
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}