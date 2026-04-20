using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.UsuarioApp;
using Booking.Autos.DataManagement.Mappers;

namespace Booking.Autos.DataManagement.Services
{
    public class RolDataService : IRolDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<RolDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Roles.GetAllAsync(ct);

            return RolDataMapper.ToDataModelList(entities);
        }

        public async Task<RolDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Roles.GetByIdAsync(id, ct);

            return entity == null ? null : RolDataMapper.ToDataModel(entity);
        }

        public async Task<RolDataModel?> GetByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Roles.GetByNombreAsync(nombre, ct);

            return entity == null ? null : RolDataMapper.ToDataModel(entity);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<RolDataModel> CreateAsync(RolDataModel model, CancellationToken ct = default)
        {
            if (await ExistsByNombreAsync(model.Nombre, ct))
                throw new Exception("El rol ya existe");

            var entity = RolDataMapper.ToEntity(model);

            await _unitOfWork.Roles.AddAsync(entity, ct);

            return RolDataMapper.ToDataModel(entity);
        }

        public async Task<RolDataModel> UpdateAsync(RolDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Roles.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("Rol no encontrado");

            existing.nombre_rol = model.Nombre;
            existing.descripcion_rol = model.Descripcion;
            existing.estado_rol = model.Estado;
            existing.activo = model.Activo;

            existing.modificado_por_usuario = model.ModificadoPorUsuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _unitOfWork.Roles.UpdateAsync(existing, ct);

            return RolDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Roles.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            await _unitOfWork.Roles.DeleteAsync(id, ct);

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var roles = await _unitOfWork.Roles.GetAllAsync(ct);

            return roles.Any(x =>
                x.nombre_rol == nombre &&
                !x.es_eliminado);
        }
    }
}