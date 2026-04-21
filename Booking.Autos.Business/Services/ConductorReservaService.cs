using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Services
{
    public class ConductorReservaService : IConductorReservaService
    {
        private readonly IConductorReservaDataService _dataService;

        public ConductorReservaService(IConductorReservaDataService dataService)
        {
            _dataService = dataService;
        }

        // =========================
        // ACTUALIZAR (CREATE + UPDATE + DELETE 🔥)
        // =========================

        public async Task<ConductorReservaDetalleResponse> CrearAsync(
            int idReserva,
            CrearConductorReservaDetalleRequest request,
            CancellationToken ct = default)
        {
            // =========================
            // VALIDACIONES
            // =========================

            if (idReserva <= 0)
                throw new ValidationException(new List<string> { "IdReserva inválido." });

            if (request.IdConductor <= 0)
                throw new ValidationException(new List<string> { "IdConductor inválido." });

            if (string.IsNullOrWhiteSpace(request.Rol))
                throw new ValidationException(new List<string> { "El rol es obligatorio." });

            // =========================
            // VALIDAR DUPLICADO
            // =========================

            var existe = await _dataService.ExistsAsync(idReserva, request.IdConductor, ct);

            if (existe)
                throw new BusinessException("El conductor ya está asignado a la reserva.");

            // =========================
            // MAPEAR (USA TU MAPPER 🔥)
            // =========================

            var model = ConductorReservaBusinessMapper.ToDataModel(request);

            model.IdReserva = idReserva;

            // 🔥 opcional (si tu modelo lo tiene)
            model.CreadoPorUsuario = "SYSTEM";
            model.ServicioOrigen = "API";

            // =========================
            // GUARDAR
            // =========================

            var creado = await _dataService.CreateAsync(model, ct);

            // =========================
            // RESPONSE
            // =========================

            return ConductorReservaBusinessMapper.ToResponse(creado);
        }

        public async Task<ConductorReservaDetalleResponse> ActualizarAsync(
            ActualizarConductorReservaDetalleRequest request,
            CancellationToken ct = default)
        {
            if (request.IdConductor <= 0)
                throw new ValidationException(new List<string> { "IdConductor inválido." });

            // 🔥 obtener existente
            var existente = await _dataService
                .GetAsync(request.IdReserva, request.IdConductor, ct);

            // =========================
            // 🔥 ELIMINAR
            // =========================
            if (request.Eliminar)
            {
                if (existente is null)
                    throw new NotFoundException("ConductorReserva", request.IdConductor);

                await _dataService.DeleteAsync(request.IdReserva, request.IdConductor, ct);

                return new ConductorReservaDetalleResponse
                {
                    IdReserva = request.IdReserva,
                    IdConductor = request.IdConductor,
                    Estado = "ELI"
                };
            }

            // =========================
            // 🔥 CREAR
            // =========================
            if (existente is null)
            {
                var nuevo = ConductorReservaBusinessMapper.ToDataModel(
                    new CrearConductorReservaDetalleRequest
                    {
                        IdConductor = request.IdConductor,
                        Rol = request.Rol,
                        EsPrincipal = request.EsPrincipal,
                        Observaciones = request.Observaciones
                    });

                nuevo.IdReserva = request.IdReserva;
                nuevo.CreadoPorUsuario = "SYSTEM";
                nuevo.ServicioOrigen = "API";

                var creado = await _dataService.CreateAsync(nuevo, ct);

                return ConductorReservaBusinessMapper.ToResponse(creado);
            }

            // =========================
            // 🔥 UPDATE
            // =========================
            var model = ConductorReservaBusinessMapper.ToDataModel(request);

            model.IdReserva = request.IdReserva;

            // 🔥 conservar datos importantes
            model.Estado = existente.Estado;
            model.FechaRegistroUtc = existente.FechaRegistroUtc;
            model.CreadoPorUsuario = existente.CreadoPorUsuario;
            model.ServicioOrigen = existente.ServicioOrigen;

            // 🔥 auditoría
            model.ModificadoPorUsuario = "SYSTEM";
            model.ModificacionIp = "127.0.0.1";

            var actualizado = await _dataService.UpdateAsync(model, ct);

            return ConductorReservaBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // REMOVER DIRECTO
        // =========================
        public async Task RemoverAsync(
            int idReserva,
            int idConductor,
            CancellationToken ct = default)
        {
            var existe = await _dataService.ExistsAsync(idReserva, idConductor, ct);

            if (!existe)
                throw new NotFoundException("ConductorReserva", $"{idReserva}-{idConductor}");

            await _dataService.DeleteAsync(idReserva, idConductor, ct);
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IReadOnlyList<ConductorReservaDetalleResponse>> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetByReservaAsync(idReserva, ct);

            return ConductorReservaBusinessMapper.ToResponseList(list);
        }

        public async Task<IReadOnlyList<ConductorReservaDetalleResponse>> ObtenerPorConductorAsync(
            int idConductor,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetByConductorAsync(idConductor, ct);

            return ConductorReservaBusinessMapper.ToResponseList(list);
        }

        public async Task<ConductorReservaDetalleResponse?> ObtenerAsync(
            int idReserva,
            int idConductor,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetAsync(idReserva, idConductor, ct);

            return model is null ? null : ConductorReservaBusinessMapper.ToResponse(model);
        }

        public async Task<ConductorReservaDetalleResponse?> ObtenerPrincipalPorReservaAsync(
            int idReserva,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetPrincipalByReservaAsync(idReserva, ct);

            return model is null ? null : ConductorReservaBusinessMapper.ToResponse(model);
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExisteAsync(
            int idReserva,
            int idConductor,
            CancellationToken ct = default)
        {
            return await _dataService.ExistsAsync(idReserva, idConductor, ct);
        }
    }
}