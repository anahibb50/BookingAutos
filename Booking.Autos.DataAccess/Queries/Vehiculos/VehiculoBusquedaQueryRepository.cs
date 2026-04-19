using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Queries.Vehiculos
{
    public class VehiculoBusquedaQueryRepository
    {
        private readonly BookingAutoDbContext _context;

        public VehiculoBusquedaQueryRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehiculoEntity>> ExecuteAsync(
            int idLocalizacion,
            DateTime fechaInicio,
            DateTime fechaFin,
            int? idCategoria,
            int? idMarca,
            string? transmision,
            CancellationToken ct = default)
        {
            var query = _context.Vehiculos
                .AsNoTracking()
                .Include(v => v.Marca)
                .Include(v => v.Categoria)
                .Include(v => v.Localizacion)
                .Where(v =>
                    !v.es_eliminado &&
                    v.localizacion_actual == idLocalizacion &&
                    v.estado_vehiculo == "DIS"
                );

            if (idCategoria.HasValue)
                query = query.Where(v => v.id_categoria == idCategoria.Value);

            if (idMarca.HasValue)
                query = query.Where(v => v.id_marca == idMarca.Value);

            if (!string.IsNullOrEmpty(transmision))
                query = query.Where(v => v.tipo_transmision == transmision);

            // 🔥 FILTRO DE DISPONIBILIDAD (CLAVE DEL CONTRATO)
            query = query.Where(v =>
                !_context.Reservas.Any(r =>
                    r.id_vehiculo == v.id_vehiculo &&
                    r.estado_reserva != "CAN" &&
                    !r.es_eliminado &&
                    (
                        (fechaInicio >= r.fecha_inicio && fechaInicio < r.fecha_fin) ||
                        (fechaFin > r.fecha_inicio && fechaFin <= r.fecha_fin) ||
                        (fechaInicio <= r.fecha_inicio && fechaFin >= r.fecha_fin)
                    )
                )
            );

            return await query.ToListAsync(ct);
        }
    }
}