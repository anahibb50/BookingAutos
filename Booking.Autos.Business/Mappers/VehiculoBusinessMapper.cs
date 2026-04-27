using Booking.Autos.Business.DTOs.Vehiculo;
using Booking.Autos.DataManagement.Models.Vehiculos;

namespace Booking.Autos.Business.Mappers
{
    public static class VehiculoBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static VehiculoDataModel ToDataModel(CrearVehiculoRequest request)
        {
            return new VehiculoDataModel
            {

                Placa = request.Placa,

                IdMarca = request.IdMarca,
                IdCategoria = request.IdCategoria,
                IdLocalizacion = request.IdLocalizacion,

                Modelo = request.Modelo,
                AnioFabricacion = request.AnioFabricacion,
                Color = request.Color,
                TipoCombustible = request.TipoCombustible,
                TipoTransmision = request.TipoTransmision,

                CapacidadPasajeros = request.CapacidadPasajeros,
                CapacidadMaletas = request.CapacidadMaletas,
                NumeroPuertas = request.NumeroPuertas,
                AireAcondicionado = request.AireAcondicionado,

                PrecioBaseDia = request.PrecioBaseDia,
                KilometrajeActual = request.KilometrajeActual,

                Observaciones = request.Observaciones,
                ImagenUrl = request.ImagenUrl,

                // 🔥 estado inicial
                Estado = "ACT", // disponible
                EsEliminado = false,

                // 🔥 auditoría mínima
                FechaRegistroUtc = DateTime.UtcNow,
                OrigenRegistro = "WEB",      // ✅ campo NOT NULL que faltaba
                CreadoPorUsuario = "SISTEMA"
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static VehiculoDataModel ToDataModel(ActualizarVehiculoRequest request)
        {
            return new VehiculoDataModel
            {
                Id = request.Id,


                Placa = request.Placa,

                IdMarca = request.IdMarca,
                IdCategoria = request.IdCategoria,
                IdLocalizacion = request.IdLocalizacion,

                Modelo = request.Modelo,
                AnioFabricacion = request.AnioFabricacion,
                Color = request.Color,
                TipoCombustible = request.TipoCombustible,
                TipoTransmision = request.TipoTransmision,

                CapacidadPasajeros = request.CapacidadPasajeros,
                CapacidadMaletas = request.CapacidadMaletas,
                NumeroPuertas = request.NumeroPuertas,
                AireAcondicionado = request.AireAcondicionado,

                PrecioBaseDia = request.PrecioBaseDia,
                KilometrajeActual = request.KilometrajeActual,

                Observaciones = request.Observaciones,
                ImagenUrl = request.ImagenUrl,

                // 🔥 auditoría
                FechaModificacionUtc = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static VehiculoResponse ToResponse(VehiculoDataModel model)
        {
            return new VehiculoResponse
            {
                Id = model.Id,
                Guid = model.Guid,

                CodigoInterno = model.CodigoInterno,
                Placa = model.Placa,

                IdMarca = model.IdMarca,
                IdCategoria = model.IdCategoria,
                IdLocalizacion = model.IdLocalizacion,

                Modelo = model.Modelo,
                AnioFabricacion = model.AnioFabricacion,
                Color = model.Color,
                TipoCombustible = model.TipoCombustible,
                TipoTransmision = model.TipoTransmision,

                CapacidadPasajeros = model.CapacidadPasajeros,
                CapacidadMaletas = model.CapacidadMaletas,
                NumeroPuertas = model.NumeroPuertas,
                AireAcondicionado = model.AireAcondicionado,

                PrecioBaseDia = model.PrecioBaseDia,
                KilometrajeActual = model.KilometrajeActual,

                Observaciones = model.Observaciones,
                ImagenUrl = model.ImagenUrl,

                Estado = model.Estado
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<VehiculoResponse> ToResponseList(IEnumerable<VehiculoDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}