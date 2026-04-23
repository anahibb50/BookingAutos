using Booking.Autos.DataManagement.Models.Categorias;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface ICategoriaDataService
    {
        Task<IEnumerable<CategoriaDataModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CategoriaDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<CategoriaDataModel> CreateAsync(CategoriaDataModel model, CancellationToken cancellationToken = default);
        Task<CategoriaDataModel> UpdateAsync(CategoriaDataModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNombreAsync(string nombre, CancellationToken cancellationToken = default);
    }
}
