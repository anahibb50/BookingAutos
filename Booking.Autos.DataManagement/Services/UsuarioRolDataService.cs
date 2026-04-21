using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.UsuarioApp;
using Booking.Autos.DataManagement.Mappers;

namespace Booking.Autos.DataManagement.Services
{
    public class UsuarioRolDataService : IUsuarioRolDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioRolDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<UsuarioRolDataModel>> GetByUsuarioAsync(
            int idUsuario,
            CancellationToken ct = default)
        {
            var entities = await _unitOfWork.UsuariosRoles
                .GetByUsuarioAsync(idUsuario, ct);

            return UsuarioRolDataMapper.ToDataModelList(entities);
        }

        public async Task<IEnumerable<UsuarioRolDataModel>> GetByRolAsync(
            int idRol,
            CancellationToken ct = default)
        {
            var entities = await _unitOfWork.UsuariosRoles
                .GetByRolAsync(idRol, ct);

            return UsuarioRolDataMapper.ToDataModelList(entities);
        }

        public async Task<UsuarioRolDataModel?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.UsuariosRoles
                .GetByIdAsync(id);

            return entity == null
                ? null
                : UsuarioRolDataMapper.ToDataModel(entity);
        }

        // 🔥 CLAVE PARA AUTH
        public async Task<List<string>> GetRolesByUsuarioAsync(
            int idUsuario,
            CancellationToken ct = default)
        {
            return await _unitOfWork.UsuariosRoles
                .GetRolesByUsuarioAsync(idUsuario, ct);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AssignAsync(
            UsuarioRolDataModel model,
            CancellationToken ct = default)
        {
            // 🔥 validación por combinación (aquí sí tiene sentido)
            var exists = await _unitOfWork.UsuariosRoles
                .ExistsAsync(model.IdUsuario, model.IdRol, ct);

            if (exists)
                throw new Exception("El usuario ya tiene este rol");

            var entity = UsuarioRolDataMapper.ToEntity(model);

            await _unitOfWork.UsuariosRoles.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            UsuarioRolDataModel model,
            CancellationToken ct = default)
        {
            var entity = UsuarioRolDataMapper.ToEntity(model);

            await _unitOfWork.UsuariosRoles.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _unitOfWork.UsuariosRoles.DeleteAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            var entity = await _unitOfWork.UsuariosRoles
                .GetByIdAsync(id);

            return entity != null;
        }

        public async Task<bool> ExistsAsync(
            int idUsuario,
            int idRol,
        CancellationToken ct = default)
            {
                return await _unitOfWork.UsuariosRoles
                    .ExistsAsync(idUsuario, idRol, ct);
            }
    }
}