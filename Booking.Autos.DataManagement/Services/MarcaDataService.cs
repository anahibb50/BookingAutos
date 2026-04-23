using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Mappers;
using Booking.Autos.DataManagement.Models.Marcas;

namespace Booking.Autos.DataManagement.Services
{
    public class MarcaDataService : IMarcaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarcaDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MarcaDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Marcas.GetAllAsync(ct);
            return MarcaDataMapper.ToDataModelList(entities);
        }

        public async Task<MarcaDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Marcas.GetByIdAsync(id, ct);
            return entity == null ? null : MarcaDataMapper.ToDataModel(entity);
        }

        public async Task<MarcaDataModel?> GetByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Marcas.GetAllAsync(ct);

            var entity = entities.FirstOrDefault(x =>
                x.nombre_marca == nombre &&
                !x.es_eliminado);

            return entity == null ? null : MarcaDataMapper.ToDataModel(entity);
        }

        public async Task<MarcaDataModel> CreateAsync(MarcaDataModel model, CancellationToken ct = default)
        {
            var entity = MarcaDataMapper.ToEntity(model);

            entity.marca_guid = Guid.NewGuid();
            entity.fecha_creacion = DateTime.UtcNow;
            entity.fecha_actualizacion = DateTime.UtcNow;
            entity.es_eliminado = false;

            await _unitOfWork.Marcas.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return MarcaDataMapper.ToDataModel(entity);
        }

        public async Task<MarcaDataModel> UpdateAsync(MarcaDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Marcas.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("Marca no encontrada");

            existing.nombre_marca = model.Nombre;
            existing.es_eliminado = model.EsEliminado;
            existing.fecha_eliminacion = model.FechaEliminacion;
            existing.fecha_actualizacion = DateTime.UtcNow;

            await _unitOfWork.Marcas.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return MarcaDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Marcas.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.es_eliminado = true;
            entity.fecha_eliminacion = DateTime.UtcNow;

            await _unitOfWork.Marcas.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> ExistsByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Marcas.GetAllAsync(ct);
            return entities.Any(x => x.nombre_marca == nombre && !x.es_eliminado);
        }
    }
}
