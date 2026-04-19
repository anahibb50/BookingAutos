using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Localizaciones;
using Booking.Autos.DataManagement.Mappers;


namespace Booking.Autos.DataManagement.Services
{
    public class LocalizacionDataService : ILocalizacionDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocalizacionDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<LocalizacionDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.LocalizacionListQueries.GetAllAsync(ct);

            return LocalizacionDataMapper.ToDataModelList(entities);
        }

        public async Task<LocalizacionDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.LocalizacionDetalleQueries.GetByIdAsync(id, ct);

            return entity == null
                ? null
                : LocalizacionDataMapper.ToDataModel(entity);
        }

        public async Task<IEnumerable<LocalizacionDataModel>> GetByCiudadAsync(int idCiudad, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.LocalizacionListQueries.GetAllAsync(ct);

            return entities
                .Where(x => x.id_ciudad == idCiudad)
                .Select(LocalizacionDataMapper.ToDataModel);
        }

        public async Task<LocalizacionDataModel?> GetByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.LocalizacionListQueries.GetAllAsync(ct);

            var entity = entities.FirstOrDefault(x =>
                x.nombre_localizacion == nombre);

            return entity == null
                ? null
                : LocalizacionDataMapper.ToDataModel(entity);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<LocalizacionDataModel> CreateAsync(LocalizacionDataModel model, CancellationToken ct = default)
        {
            var entity = LocalizacionDataMapper.ToEntity(model);

            entity.localizacion_guid = Guid.NewGuid();
            entity.fecha_registro_utc = DateTime.UtcNow;
            entity.es_eliminado = false;

            await _unitOfWork.Localizaciones.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return LocalizacionDataMapper.ToDataModel(entity);
        }

        public async Task<LocalizacionDataModel> UpdateAsync(LocalizacionDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Localizaciones.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("Localización no encontrada");

            existing.codigo_localizacion = model.Codigo;
            existing.nombre_localizacion = model.Nombre;
            existing.id_ciudad = model.IdCiudad;

            existing.direccion_localizacion = model.Direccion;
            existing.telefono_contacto = model.Telefono;
            existing.correo_contacto = model.Correo;
            existing.horario_atencion = model.HorarioAtencion;
            existing.zona_horaria = model.ZonaHoraria;

            existing.estado_localizacion = model.Estado;

            existing.modificado_por_usuario = model.ModificadoPorUsuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_desde_ip = model.ModificadoDesdeIp;

            existing.origen_registro = model.OrigenRegistro;
            existing.motivo_inhabilitacion = model.MotivoInhabilitacion;

            await _unitOfWork.Localizaciones.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return LocalizacionDataMapper.ToDataModel(existing);
        }

        public async Task<bool> InhabilitarAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Localizaciones.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.estado_localizacion = "INA";
            entity.fecha_inhabilitacion_utc = DateTime.UtcNow;

            await _unitOfWork.Localizaciones.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByNombreEnCiudadAsync(string nombre, int idCiudad, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.LocalizacionListQueries.GetAllAsync(ct);

            return entities.Any(x =>
                x.nombre_localizacion == nombre &&
                x.id_ciudad == idCiudad);
        }
    }
}