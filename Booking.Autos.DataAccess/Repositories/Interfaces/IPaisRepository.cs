using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IPaisRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<PaisEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<PaisEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<PaisEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        Task<PaisEntity?> GetByNombreAsync(string nombrePais, CancellationToken cancellationToken = default);

        Task<PaisEntity?> GetByCodigoIsoAsync(string codigoIso, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(PaisEntity pais, CancellationToken cancellationToken = default);

        Task UpdateAsync(PaisEntity pais, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================


        Task<bool> TieneCiudadesAsociadasAsync(int idPais, CancellationToken cancellationToken = default);
    }
}