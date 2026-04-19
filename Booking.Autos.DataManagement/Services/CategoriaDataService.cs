using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Categorias;
using Booking.Autos.DataManagement.Mappers;

namespace Booking.Autos.DataManagement.Services
{
    public class CategoriaDataService : ICategoriaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<CategoriaDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Categorias.GetAllAsync();

            return entities.Select(CategoriaDataMapper.ToDataModel);
        }

        public async Task<CategoriaDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Categorias.GetByIdAsync(id);

            if (entity == null)
                return null;

            return CategoriaDataMapper.ToDataModel(entity);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<CategoriaDataModel> CreateAsync(CategoriaDataModel model, CancellationToken ct = default)
        {
            var entity = CategoriaDataMapper.ToEntity(model);

            // 🔥 Generar GUID si no viene
            entity.categoria_guid = Guid.NewGuid();
            entity.fecha_creacion = DateTime.UtcNow;
            entity.fecha_actualizacion = DateTime.UtcNow;
            entity.es_eliminado = false;

            await _unitOfWork.Categorias.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return CategoriaDataMapper.ToDataModel(entity);
        }

        public async Task<CategoriaDataModel> UpdateAsync(CategoriaDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Categorias.GetByIdAsync(model.Id);

            if (existing == null)
                throw new Exception("Categoría no encontrada");

            // 🔥 Actualizar solo campos editables
            existing.nombre_categoria = model.Nombre;
            existing.fecha_actualizacion = DateTime.UtcNow;

            _unitOfWork.Categorias.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(ct);

            return CategoriaDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Categorias.GetByIdAsync(id);

            if (entity == null)
                return false;

            // 🔥 BORRADO LÓGICO (importantísimo)
            entity.es_eliminado = true;
            entity.fecha_eliminacion = DateTime.UtcNow;

            _unitOfWork.Categorias.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var categorias = await _unitOfWork.Categorias.GetAllAsync();

            return categorias.Any(x => x.nombre_categoria == nombre && !x.es_eliminado);
        }
    }
}