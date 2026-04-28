using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.UsuarioApp;
using Booking.Autos.DataManagement.Mappers;
using System.Security.Cryptography;
using System.Text;

namespace Booking.Autos.DataManagement.Services
{
    public class UsuarioAppDataService : IUsuarioAppDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioAppDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<UsuarioAppDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.UsuariosApp.GetAllAsync(ct);

            return UsuarioAppDataMapper.ToDataModelList(entities);
        }

        public async Task<UsuarioAppDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.UsuariosApp.GetByIdAsync(id, ct);

            return entity == null ? null : UsuarioAppDataMapper.ToDataModel(entity);
        }

        public async Task<UsuarioAppDataModel?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.UsuariosApp.GetAllAsync(ct);

            var entity = entities.FirstOrDefault(x =>
                x.username == username && !x.es_eliminado);

            return entity == null ? null : UsuarioAppDataMapper.ToDataModel(entity);
        }

        public async Task<UsuarioAppDataModel?> GetByCorreoAsync(string correo, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.UsuariosApp.GetAllAsync(ct);

            var entity = entities.FirstOrDefault(x =>
                x.correo == correo && !x.es_eliminado);

            return entity == null ? null : UsuarioAppDataMapper.ToDataModel(entity);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<UsuarioAppDataModel> CreateAsync(
            UsuarioAppDataModel model,
            string passwordPlano,
            CancellationToken ct = default)
        {
            if (await ExistsByUsernameAsync(model.Username, ct))
                throw new Exception("El username ya existe");

            if (await ExistsByCorreoAsync(model.Correo, ct))
                throw new Exception("El correo ya existe");

            var entity = UsuarioAppDataMapper.ToEntity(model);

            entity.usuario_guid = Guid.NewGuid();
            entity.fecha_registro_utc = DateTime.UtcNow;
            entity.es_eliminado = false;
            entity.activo = true;
            entity.estado_usuario = "ACT";

            // 🔐 HASH PASSWORD
            var salt = GenerateSalt();
            entity.password_salt = salt;
            entity.password_hash = HashPassword(passwordPlano, salt);

            await _unitOfWork.UsuariosApp.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return UsuarioAppDataMapper.ToDataModel(entity);
        }

        public async Task<UsuarioAppDataModel> UpdateAsync(
            UsuarioAppDataModel model,
            CancellationToken ct = default)
        {
            var existing = await _unitOfWork.UsuariosApp.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("Usuario no encontrado");

            existing.username = model.Username;
            existing.correo = model.Correo;
            existing.id_cliente = model.IdCliente;

            existing.estado_usuario = model.Estado;
            existing.activo = model.Activo;

            existing.modificado_por_usuario = model.ModificadoPorUsuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _unitOfWork.UsuariosApp.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return UsuarioAppDataMapper.ToDataModel(existing);
        }

        // =========================
        // ESTADO
        // =========================

        public async Task<bool> ActivarAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.UsuariosApp.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.activo = true;
            entity.estado_usuario = "ACT";
            entity.fecha_modificacion_utc = DateTime.UtcNow;

            await _unitOfWork.UsuariosApp.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> DesactivarAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.UsuariosApp.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.activo = false;
            entity.estado_usuario = "INA";
            entity.fecha_modificacion_utc = DateTime.UtcNow;

            await _unitOfWork.UsuariosApp.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.UsuariosApp.GetAllAsync(ct);

            return entities.Any(x =>
                x.username == username &&
                !x.es_eliminado);
        }

        public async Task<bool> ExistsByCorreoAsync(string correo, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.UsuariosApp.GetAllAsync(ct);

            return entities.Any(x =>
                x.correo == correo &&
                !x.es_eliminado);
        }

        // =========================
        // 🔐 SEGURIDAD
        // =========================

        private static string GenerateSalt()
        {
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes($"{password}:{salt}");
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}