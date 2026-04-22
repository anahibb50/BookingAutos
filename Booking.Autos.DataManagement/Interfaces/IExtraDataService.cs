using Booking.Autos.DataManagement.Models.Extras;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IExtraDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<ExtraDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ExtraDataModel>> GetActivosAsync(
            CancellationToken cancellationToken = default);

        Task<ExtraDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ExtraDataModel>> GetByNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<ExtraDataModel> CreateAsync(
            ExtraDataModel model,
            CancellationToken cancellationToken = default);

        Task<ExtraDataModel> UpdateAsync(
            ExtraDataModel model,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // OPERACIÓN ESPECIAL
        // =========================

        Task<bool> UpdatePrecioAsync(
            int id,
            decimal nuevoPrecio,
            CancellationToken cancellationToken = default);

     
    }
}