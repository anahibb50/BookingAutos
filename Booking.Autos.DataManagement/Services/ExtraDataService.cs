using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Extras;
using Booking.Autos.DataManagement.Mappers;

namespace Booking.Autos.DataManagement.Services
{
    public class ExtraDataService : IExtraDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExtraDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<ExtraDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Extras.GetAllAsync(ct);

            return ExtraDataMapper.ToDataModelList(entities);
        }

        public async Task<IEnumerable<ExtraDataModel>> GetActivosAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Extras.GetAllAsync(ct);

            return entities
                .Where(x => x.estado_extra == "ACT" && !x.es_eliminado)
                .Select(ExtraDataMapper.ToDataModel);
        }

        public async Task<ExtraDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Extras.GetByIdAsync(id, ct);

            return entity == null
                ? null
                : ExtraDataMapper.ToDataModel(entity);
        }

        public async Task<IEnumerable<ExtraDataModel>> GetByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Extras.GetAllAsync(ct);

            return entities
                .Where(x => x.nombre_extra.Contains(nombre) && !x.es_eliminado)
                .Select(ExtraDataMapper.ToDataModel);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<ExtraDataModel> CreateAsync(ExtraDataModel model, CancellationToken ct = default)
        {
            var entity = ExtraDataMapper.ToEntity(model);

            entity.extra_guid = Guid.NewGuid();
            entity.fecha_registro_utc = DateTime.UtcNow;
            entity.es_eliminado = false;

            await _unitOfWork.Extras.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ExtraDataMapper.ToDataModel(entity);
        }

        public async Task<ExtraDataModel> UpdateAsync(ExtraDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Extras.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("Extra no encontrado");

            existing.codigo_extra = model.Codigo;
            existing.nombre_extra = model.Nombre;
            existing.descripcion_extra = model.Descripcion;
            existing.valor_fijo = model.ValorFijo;

            existing.estado_extra = model.Estado;

            existing.modificado_por_usuario = model.ModificadoPorUsuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_desde_ip = model.ModificadoDesdeIp;

            existing.origen_registro = model.OrigenRegistro;
            existing.motivo_inhabilitacion = model.MotivoInhabilitacion;

            await _unitOfWork.Extras.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ExtraDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Extras.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.es_eliminado = true;
            entity.fecha_inhabilitacion_utc = DateTime.UtcNow;
            entity.estado_extra = "INA";

            await _unitOfWork.Extras.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        // =========================
        // OPERACIÓN ESPECIAL
        // =========================

        public async Task<bool> UpdatePrecioAsync(int id, decimal nuevoPrecio, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Extras.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.valor_fijo = nuevoPrecio;
            entity.fecha_modificacion_utc = DateTime.UtcNow;

            await _unitOfWork.Extras.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }


    }
}