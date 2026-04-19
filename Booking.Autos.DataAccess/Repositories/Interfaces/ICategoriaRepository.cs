using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        // Lectura
        Task<IReadOnlyList<CategoriaEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CategoriaEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<CategoriaEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);
        Task<CategoriaEntity?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default);

        // Escritura
        Task AddAsync(CategoriaEntity categoria, CancellationToken cancellationToken = default);
        Task UpdateAsync(CategoriaEntity categoria, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        // Validaciones
        Task<bool> ExistsByNombreAsync(string nombre, CancellationToken cancellationToken = default);
        Task<bool> TieneVehiculosAsociadosAsync(int id, CancellationToken cancellationToken = default);
    }
}