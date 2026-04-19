using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Models.Categorias;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface ICategoriaDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<CategoriaDataModel>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<CategoriaDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<CategoriaDataModel> CreateAsync(CategoriaDataModel model, CancellationToken cancellationToken = default);

        Task<CategoriaDataModel> UpdateAsync(CategoriaDataModel model, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByNombreAsync(string nombre, CancellationToken cancellationToken = default);
    }
}