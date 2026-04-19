using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Ciudades;
using Booking.Autos.DataManagement.Mappers;

namespace Booking.Autos.DataManagement.Services
{
    public class CiudadDataService : ICiudadDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CiudadDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<CiudadDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Ciudades.GetAllAsync();

            return entities.Select(CiudadDataMapper.ToDataModel);
        }

        public async Task<CiudadDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Ciudades.GetByIdAsync(id);

            if (entity == null)
                return null;

            return CiudadDataMapper.ToDataModel(entity);
        }

        public async Task<IEnumerable<CiudadDataModel>> GetByPaisAsync(int idPais, CancellationToken ct = default)
        {
            var ciudades = await _unitOfWork.Ciudades.GetAllAsync(ct);

            return ciudades
                .Where(x => x.id_pais == idPais)
                .Select(CiudadDataMapper.ToDataModel);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<CiudadDataModel> CreateAsync(CiudadDataModel model, CancellationToken ct = default)
        {
            var entity = CiudadDataMapper.ToEntity(model);

            entity.ciudad_guid = Guid.NewGuid();
            entity.fecha_creacion = DateTime.UtcNow;
            entity.fecha_actualizacion = DateTime.UtcNow;
            entity.es_eliminado = false;

            await _unitOfWork.Ciudades.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return CiudadDataMapper.ToDataModel(entity);
        }

        public async Task<CiudadDataModel> UpdateAsync(CiudadDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Ciudades.GetByIdAsync(model.Id);

            if (existing == null)
                throw new Exception("Ciudad no encontrada");

            existing.nombre_ciudad = model.Nombre;
            existing.codigo_postal = model.CodigoPostal;
            existing.id_pais = model.IdPais;
            existing.estado_ciudad = model.Estado;
            existing.origen_registro = model.OrigenRegistro;

            existing.fecha_actualizacion = DateTime.UtcNow;

            await _unitOfWork.Ciudades.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(ct);

            return CiudadDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Ciudades.GetByIdAsync(id);

            if (entity == null)
                return false;

            entity.es_eliminado = true;
            entity.fecha_eliminacion = DateTime.UtcNow;

            await _unitOfWork.Ciudades.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByNombreAsync(string nombre, int idPais, CancellationToken ct = default)
        {
            var ciudades = await _unitOfWork.Ciudades.GetAllAsync(ct);

            return ciudades.Any(x =>
                x.nombre_ciudad == nombre &&
                x.id_pais == idPais &&
                !x.es_eliminado);
        }
    }
}