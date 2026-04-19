using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Paises;
using Booking.Autos.DataManagement.Mappers;

namespace Booking.Autos.DataManagement.Services
{
    public class PaisDataService : IPaisDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaisDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<PaisDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Paises.GetAllAsync(ct);

            return PaisDataMapper.ToDataModelList(entities);
        }

        public async Task<PaisDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Paises.GetByIdAsync(id, ct);

            return entity == null
                ? null
                : PaisDataMapper.ToDataModel(entity);
        }

        public async Task<PaisDataModel?> GetByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Paises.GetAllAsync(ct);

            var entity = entities.FirstOrDefault(x =>
                x.nombre_pais == nombre &&
                !x.es_eliminado);

            return entity == null
                ? null
                : PaisDataMapper.ToDataModel(entity);
        }

        public async Task<PaisDataModel?> GetByCodigoIsoAsync(string codigoIso, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Paises.GetAllAsync(ct);

            var entity = entities.FirstOrDefault(x =>
                x.codigo_iso == codigoIso &&
                !x.es_eliminado);

            return entity == null
                ? null
                : PaisDataMapper.ToDataModel(entity);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<PaisDataModel> CreateAsync(PaisDataModel model, CancellationToken ct = default)
        {
            var entity = PaisDataMapper.ToEntity(model);

            entity.pais_guid = Guid.NewGuid();
            entity.fecha_creacion = DateTime.UtcNow;
            entity.fecha_actualizacion = DateTime.UtcNow;
            entity.es_eliminado = false;

            await _unitOfWork.Paises.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return PaisDataMapper.ToDataModel(entity);
        }

        public async Task<PaisDataModel> UpdateAsync(PaisDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Paises.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("País no encontrado");

            existing.nombre_pais = model.Nombre;
            existing.codigo_iso = model.CodigoIso;

            existing.fecha_actualizacion = DateTime.UtcNow;

            await _unitOfWork.Paises.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return PaisDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Paises.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            // 🔥 Validar si tiene ciudades
            var tieneCiudades = await TieneCiudadesAsociadasAsync(id, ct);

            if (tieneCiudades)
                throw new Exception("No se puede eliminar el país porque tiene ciudades asociadas");

            entity.es_eliminado = true;
            entity.fecha_eliminacion = DateTime.UtcNow;

            await _unitOfWork.Paises.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Paises.GetAllAsync(ct);

            return entities.Any(x =>
                x.nombre_pais == nombre &&
                !x.es_eliminado);
        }

        public async Task<bool> ExistsByCodigoIsoAsync(string codigoIso, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Paises.GetAllAsync(ct);

            return entities.Any(x =>
                x.codigo_iso == codigoIso &&
                !x.es_eliminado);
        }

        public async Task<bool> TieneCiudadesAsociadasAsync(int idPais, CancellationToken ct = default)
        {
            var ciudades = await _unitOfWork.Ciudades.GetAllAsync(ct);

            return ciudades.Any(x =>
                x.id_pais == idPais &&
                !x.es_eliminado);
        }
    }
}