using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IUsuarioRolRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<UsuarioRolEntity>> GetByUsuarioAsync(
            int idUsuario,
            CancellationToken ct = default);

        Task<IEnumerable<UsuarioRolEntity>> GetByRolAsync(
            int idRol,
            CancellationToken ct = default);

        Task<UsuarioRolEntity?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        // 🔥 para validar duplicados
        Task<bool> ExistsAsync(
            int idUsuario,
            int idRol,
            CancellationToken ct = default);

        // 🔥 CLAVE PARA AUTH
        Task<List<string>> GetRolesByUsuarioAsync(
            int idUsuario,
            CancellationToken ct = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(
            UsuarioRolEntity entity,
            CancellationToken ct = default);

        Task UpdateAsync(
            UsuarioRolEntity entity,
            CancellationToken ct = default);

        Task DeleteAsync(
            int id,
            CancellationToken ct = default);
    }
}