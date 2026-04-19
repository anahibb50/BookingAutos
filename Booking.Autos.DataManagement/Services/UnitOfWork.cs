using Booking.Autos.DataAccess;
using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Queries.Categorias;
using Booking.Autos.DataAccess.Queries.Extras;
using Booking.Autos.DataAccess.Queries.Localizaciones;
using Booking.Autos.DataAccess.Queries.Vehiculos;
using Booking.Autos.DataAccess.Repositories;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Booking.Autos.DataManagement.Interfaces;

namespace Booking.Autos.DataManagement.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookingAutoDbContext _context;

    // =========================
    // REPOSITORIES
    // =========================

    public IClienteRepository Clientes { get; }
    public IUsuarioAppRepository UsuariosApp { get; }
    public IVehiculoRepository Vehiculos { get; }
    public IReservaRepository Reservas { get; }
    public IReservaExtraRepository ReservasExtras { get; }
    public IConductorRepository Conductores { get; }
    public IConductorReservaRepository ConductoresReservas { get; }
    public IExtraRepository Extras { get; }
    public IFacturaRepository Facturas { get; }
    public ILocalizacionRepository Localizaciones { get; }
    public IMarcaRepository Marcas { get; }
    public IPaisRepository Paises { get; }
    public ICategoriaRepository Categorias { get; }
    public ICiudadRepository Ciudades { get; }

    public CategoriaListQueryRepository CategoriaListQueryRepository { get; }
    public ExtraListQueryRepository ExtraQueries { get; }

    public LocalizacionListQueryRepository LocalizacionListQueries { get; }
    public LocalizacionDetalleQueryRepository LocalizacionDetalleQueries { get; }

    public VehiculoBusquedaQueryRepository VehiculoBusquedaQueries { get; }
    public VehiculoDetalleQueryRepository VehiculoDetalleQueries { get; }
    public VehiculoDisponibilidadQueryRepository VehiculoDisponibilidadQueries { get; }

    // =========================
    // CONSTRUCTOR
    // =========================

    public UnitOfWork(BookingAutoDbContext context)
    {
        _context = context;

        Clientes = new ClienteRepository(_context);
        UsuariosApp = new UsuarioAppRepository(_context);
        Vehiculos = new VehiculoRepository(_context);
        Reservas = new ReservaRepository(_context);
        ReservasExtras = new ReservaExtraRepository(_context);
        Conductores = new ConductorRepository(_context);
        ConductoresReservas = new ConductorReservaRepository(_context);
        Extras = new ExtraRepository(_context);
        Facturas = new FacturaRepository(_context);
        Localizaciones = new LocalizacionRepository(_context);
        Marcas = new MarcaRepository(_context);
        Paises = new PaisRepository(_context);
        Categorias = new CategoriaRepository(_context);
        Ciudades = new CiudadRepository(_context);
        CategoriaListQueryRepository = new CategoriaListQueryRepository(_context);
        ExtraQueries = new ExtraListQueryRepository(_context);

        LocalizacionListQueries = new LocalizacionListQueryRepository(_context);
        LocalizacionDetalleQueries = new LocalizacionDetalleQueryRepository(_context);

        VehiculoBusquedaQueries = new VehiculoBusquedaQueryRepository(_context);
        VehiculoDetalleQueries = new VehiculoDetalleQueryRepository(_context);
        VehiculoDisponibilidadQueries = new VehiculoDisponibilidadQueryRepository(_context);
    }

    // =========================
    // SAVE
    // =========================

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    // =========================
    // DISPOSE
    // =========================

    public void Dispose()
    {
        _context.Dispose();
    }
}