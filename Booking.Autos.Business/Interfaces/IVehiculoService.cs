using Booking.Autos.Business.DTOs.Vehiculo;
using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.Business.Interfaces
{
    public interface IVehiculoService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<VehiculoResponse> CrearAsync(
            CrearVehiculoRequest request,
            CancellationToken cancellationToken = default);

        Task<VehiculoResponse> ActualizarAsync(
            ActualizarVehiculoRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS (CATÁLOGO)
        // =========================

        Task<VehiculoResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<VehiculoResponse?> ObtenerPorPlacaAsync(
            string placa,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // 🔥 PAGINADO
        Task<DataPagedResult<VehiculoResponse>> BuscarAsync(
            VehiculoFiltroRequest request,
            CancellationToken cancellationToken = default);

        // =========================
        // FILTROS
        // =========================

        Task<IReadOnlyList<VehiculoResponse>> ObtenerPorMarcaAsync(
            int idMarca,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ObtenerPorCategoriaAsync(
            int idCategoria,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ListarDisponiblesAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ObtenerPorRangoPrecioAsync(
            decimal min,
            decimal max,
            CancellationToken cancellationToken = default);

        // =========================
        // DISPONIBILIDAD 🔥🔥
        // =========================

        Task<bool> VerificarDisponibilidadAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken cancellationToken = default);

        // =========================
        // OPERACIONES ESPECIALES 🔥
        // =========================

        Task<bool> ActualizarKilometrajeAsync(
            int id,
            int nuevoKilometraje,
            CancellationToken cancellationToken = default);

        Task<bool> ActualizarEstadoAsync(
            int id,
            string estado,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorPlacaAsync(
            string placa,
            CancellationToken cancellationToken = default);
    }
}