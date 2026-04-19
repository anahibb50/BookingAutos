using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IUsuarioAppRepository
    {
        // =========================
        // CONSULTAS BÁSICAS
        // =========================

        Task<IEnumerable<UsuarioAppEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<UsuarioAppEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<UsuarioAppEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        // =========================
        // AUTENTICACIÓN
        // =========================

        Task<UsuarioAppEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

        Task<UsuarioAppEntity?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default);

        // =========================
        // RELACIONES
        // =========================

        Task<UsuarioAppEntity?> GetByClienteIdAsync(int idCliente, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(UsuarioAppEntity usuario, CancellationToken cancellationToken = default);

        Task UpdateAsync(UsuarioAppEntity usuario, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);

        Task<bool> ExistsByCorreoAsync(string correo, CancellationToken cancellationToken = default);
    }
}