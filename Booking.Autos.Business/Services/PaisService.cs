using Booking.Autos.Business.DTOs.Catalogos.Pais;
using Booking.Autos.Business.DTOs.Pais;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;

namespace Booking.Autos.Business.Services
{
    public class PaisService : IPaisService
    {
        private readonly IPaisDataService _dataService;

        public PaisService(IPaisDataService dataService)
        {
            _dataService = dataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<PaisResponse> CrearAsync(
            CrearPaisRequest request,
            CancellationToken ct = default)
        {
            var nombre = request.Nombre?.Trim();
            var codigoIso = request.CodigoIso?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ValidationException(new List<string>
                {
                    "El nombre es obligatorio."
                });

            if (await _dataService.ExistsByNombreAsync(nombre, ct))
                throw new ValidationException(new List<string>
                {
                    "Ya existe un país con ese nombre."
                });

            if (!string.IsNullOrWhiteSpace(codigoIso))
            {
                if (await _dataService.ExistsByCodigoIsoAsync(codigoIso, ct))
                    throw new ValidationException(new List<string>
                    {
                        "El código ISO ya está registrado."
                    });
            }

            request.Nombre = nombre;
            request.CodigoIso = codigoIso;

            var model = PaisBusinessMapper.ToDataModel(request);

            var creado = await _dataService.CreateAsync(model, ct);

            return PaisBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<PaisResponse> ActualizarAsync(
            ActualizarPaisRequest request,
            CancellationToken ct = default)
        {
            var nombre = request.Nombre?.Trim();
            var codigoIso = request.CodigoIso?.Trim();

            if (request.Id <= 0)
                throw new ValidationException(new List<string>
                {
                    "Id inválido."
                });

            var existente = await _dataService.GetByIdAsync(request.Id, ct);

            if (existente is null)
                throw new NotFoundException("Pais", request.Id);

            // 🔥 validar nombre
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ValidationException(new List<string>
                {
                    "El nombre es obligatorio."
                });

            if (!string.Equals(existente.Nombre?.Trim(), nombre, StringComparison.OrdinalIgnoreCase))
            {
                if (await _dataService.ExistsByNombreAsync(nombre, ct))
                    throw new ValidationException(new List<string>
                    {
                        "Ya existe un país con ese nombre."
                    });
            }

            // 🔥 validar ISO
            if (!string.IsNullOrWhiteSpace(codigoIso) &&
                !string.Equals(existente.CodigoIso?.Trim(), codigoIso, StringComparison.OrdinalIgnoreCase))
            {
                if (await _dataService.ExistsByCodigoIsoAsync(codigoIso, ct))
                    throw new ValidationException(new List<string>
                    {
                        "El código ISO ya está registrado."
                    });
            }

            request.Nombre = nombre;
            request.CodigoIso = codigoIso;

            var model = PaisBusinessMapper.ToDataModel(request);

            // 🔥 conservar datos importantes
            model.Guid = existente.Guid;
            model.FechaCreacion = existente.FechaCreacion;
            model.EsEliminado = existente.EsEliminado;

            var actualizado = await _dataService.UpdateAsync(model, ct);

            return PaisBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // ELIMINACIÓN LÓGICA
        // =========================
        public async Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken ct = default)
        {
            var existente = await _dataService.GetByIdAsync(id, ct);

            if (existente is null)
                throw new NotFoundException("Pais", id);

            // 🔥 REGLA CLAVE
            var tieneCiudades = await _dataService
                .TieneCiudadesAsociadasAsync(id, ct);

            if (tieneCiudades)
                throw new ValidationException(new List<string>
                {
                    "No se puede eliminar el país porque tiene ciudades asociadas."
                });

            existente.EsEliminado = true;
            existente.FechaEliminacion = DateTime.UtcNow;

            await _dataService.UpdateAsync(existente, ct);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<PaisResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetByIdAsync(id, ct);

            if (model is null)
                throw new NotFoundException("Pais", id);

            return PaisBusinessMapper.ToResponse(model);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<PaisResponse>> ListarAsync(
            CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            return PaisBusinessMapper.ToResponseList(list);
        }

        // =========================
        // POR NOMBRE
        // =========================
        public async Task<PaisResponse?> ObtenerPorNombreAsync(
            string nombre,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetByNombreAsync(nombre, ct);

            return model is null
                ? null
                : PaisBusinessMapper.ToResponse(model);
        }

        // =========================
        // POR ISO
        // =========================
        public async Task<PaisResponse?> ObtenerPorCodigoIsoAsync(
            string codigoIso,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetByCodigoIsoAsync(codigoIso, ct);

            return model is null
                ? null
                : PaisBusinessMapper.ToResponse(model);
        }

        // =========================
        // VALIDACIONES
        // =========================
        public async Task<bool> ExistePorNombreAsync(
            string nombre,
            CancellationToken ct = default)
        {
            return await _dataService.ExistsByNombreAsync(nombre, ct);
        }

        public async Task<bool> ExistePorCodigoIsoAsync(
            string codigoIso,
            CancellationToken ct = default)
        {
            return await _dataService.ExistsByCodigoIsoAsync(codigoIso, ct);
        }

        public async Task<bool> TieneCiudadesAsociadasAsync(
            int idPais,
            CancellationToken ct = default)
        {
            return await _dataService
                .TieneCiudadesAsociadasAsync(idPais, ct);
        }
    }
}
