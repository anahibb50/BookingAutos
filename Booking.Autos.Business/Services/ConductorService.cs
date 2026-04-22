using Booking.Autos.Business.DTOs.Conductor;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Conductores;

namespace Booking.Autos.Business.Services
{
    public class ConductorService : IConductorService
    {
        private readonly IConductorDataService _conductorDataService;

        public ConductorService(IConductorDataService conductorDataService)
        {
            _conductorDataService = conductorDataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<ConductorResponse> CrearAsync(
            CrearConductorRequest request,
            CancellationToken cancellationToken = default)
        {
            // 🔥 VALIDACIONES CENTRALIZADAS (AQUÍ ESTÁ LA CLAVE)
            var errors = ConductorValidator.ValidarCreacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            // 🔍 VALIDACIONES DE NEGOCIO (BD)
            if (await _conductorDataService.ExistsByIdentificacionAsync(request.NumeroIdentificacion, cancellationToken))
                throw new ValidationException(new List<string>
                {
                    "Ya existe un conductor con esa identificación."
                });

                    if (await _conductorDataService.ExistsByLicenciaAsync(request.NumeroLicencia, cancellationToken))
                        throw new ValidationException(new List<string>
                {
                    "Ya existe un conductor con esa licencia."
                });

            // 🔹 MAPEO
            var model = ConductorBusinessMapper.ToDataModel(request);

            // 🔹 CAMPOS AUTOMÁTICOS
            model.Estado = "ACT";
            model.EsEliminado = false;
            model.CreadoPorUsuario = "SYSTEM";
            model.OrigenRegistro = "API";

            // 🔹 GUARDAR
            var creado = await _conductorDataService.CreateAsync(model, cancellationToken);

            // 🔹 RESPUESTA
            return ConductorBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<ConductorResponse> ActualizarAsync(
            ActualizarConductorRequest request,
            CancellationToken cancellationToken = default)
        {
            // 🔥 VALIDACIONES CENTRALIZADAS
            var errors = ConductorValidator.ValidarActualizacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            // 🔍 EXISTENCIA
            var existente = await _conductorDataService
                .GetByIdAsync(request.Id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Conductor", request.Id);

            // 🔍 VALIDACIONES DE NEGOCIO (DUPLICADOS)
            if (await _conductorDataService.ExistsByIdentificacionAsync(
                    request.NumeroIdentificacion, cancellationToken)
                && existente.NumeroIdentificacion != request.NumeroIdentificacion)
            {
                throw new ValidationException(new List<string>
            {
                "Ya existe otro conductor con esa identificación."
            });
            }

            if (await _conductorDataService.ExistsByLicenciaAsync(
                    request.NumeroLicencia, cancellationToken)
                && existente.NumeroLicencia != request.NumeroLicencia)
            {
                throw new ValidationException(new List<string>
            {
                "Ya existe otro conductor con esa licencia."
            });
            }

            // 🔹 MAPEO
            var model = ConductorBusinessMapper.ToDataModel(request);

            // 🔥 CONSERVAR DATOS IMPORTANTES
            model.Guid = existente.Guid;
            model.Codigo = existente.Codigo;
            model.FechaRegistroUtc = existente.FechaRegistroUtc;
            model.EsEliminado = existente.EsEliminado;
            model.CreadoPorUsuario = existente.CreadoPorUsuario;
            model.OrigenRegistro = existente.OrigenRegistro;

            // 🔥 AUDITORÍA
            model.ModificadoPorUsuario = "SYSTEM";
            model.ModificadoDesdeIp = "127.0.0.1";

            // 🔹 ACTUALIZAR
            var actualizado = await _conductorDataService.UpdateAsync(model, cancellationToken);

            return ConductorBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // ELIMINACIÓN LÓGICA
        // =========================
        public async Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default)
        {
            var existente = await _conductorDataService
                .GetByIdAsync(id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Conductor", id);

            existente.EsEliminado = true;
            existente.FechaInhabilitacionUtc = DateTime.UtcNow;
            existente.MotivoInhabilitacion = "Eliminación lógica";
            existente.ModificadoPorUsuario = usuario;

            await _conductorDataService.UpdateAsync(existente, cancellationToken);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<ConductorResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var conductor = await _conductorDataService
                .GetByIdAsync(id, cancellationToken);

            if (conductor is null)
                throw new NotFoundException("Conductor", id);

            return ConductorBusinessMapper.ToResponse(conductor);
        }

        // =========================
        // POR IDENTIFICACIÓN
        // =========================
        public async Task<ConductorResponse?> ObtenerPorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default)
        {
            var conductor = await _conductorDataService
                .GetByIdentificacionAsync(identificacion, cancellationToken);

            if (conductor is null)
                throw new NotFoundException("Conductor con identificación", identificacion);

            return ConductorBusinessMapper.ToResponse(conductor);
        }

        // =========================
        // POR LICENCIA
        // =========================
        public async Task<ConductorResponse> ObtenerPorLicenciaAsync(
            string numeroLicencia,
            CancellationToken cancellationToken = default)
        {
            var conductor = await _conductorDataService
                .GetByLicenciaAsync(numeroLicencia, cancellationToken);

            if (conductor is null)
                throw new NotFoundException("Conductor con licencia", numeroLicencia);

            return ConductorBusinessMapper.ToResponse(conductor);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<ConductorResponse>> ListarAsync(
            CancellationToken cancellationToken = default)
        {
            var conductores = await _conductorDataService
                .GetAllAsync(cancellationToken);

            return conductores
                .Select(ConductorBusinessMapper.ToResponse)
                .ToList();
        }

        // =========================
        // BUSCAR (PAGINADO)
        // =========================
        public async Task<DataPagedResult<ConductorResponse>> BuscarAsync(
            ConductorFiltroRequest request,
            CancellationToken cancellationToken = default)
        {
            var filtro = new ConductorFiltroDataModel
            {
                NumeroIdentificacion = request.NumeroIdentificacion,
                NumeroLicencia = request.NumeroLicencia,
                Nombre = request.Nombre,
                Estado = request.Estado,
                EdadMin = request.EdadMin,
                EdadMax = request.EdadMax,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _conductorDataService
                .BuscarAsync(filtro, cancellationToken);

            return new DataPagedResult<ConductorResponse>
            {
                Items = result.Items.Select(ConductorBusinessMapper.ToResponse).ToList(),
                TotalRecords = result.TotalRecords,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }

        // =========================
        // VALIDACIONES
        // =========================
        public async Task<bool> ExistePorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default)
        {
            return await _conductorDataService
                .ExistsByIdentificacionAsync(identificacion, cancellationToken);
        }

        public async Task<bool> ExistePorLicenciaAsync(
            string numeroLicencia,
            CancellationToken cancellationToken = default)
        {
            return await _conductorDataService
                .ExistsByLicenciaAsync(numeroLicencia, cancellationToken);
        }
    }
}