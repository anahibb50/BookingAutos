using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Vehiculos;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class VehiculoDataMapper
    {
        // 🔁 Entity → DataModel
        public static VehiculoDataModel ToDataModel(VehiculoEntity entity)
        {
            return new VehiculoDataModel
            {
                Id = entity.id_vehiculo,
                Guid = entity.vehiculo_guid,

                CodigoInterno = entity.codigo_interno_vehiculo,
                Placa = entity.placa_vehiculo,

                IdMarca = entity.id_marca,
                IdCategoria = entity.id_categoria,
                IdLocalizacion = entity.localizacion_actual,

                Modelo = entity.modelo_vehiculo,
                AnioFabricacion = entity.anio_fabricacion,
                Color = entity.color_vehiculo,
                TipoCombustible = entity.tipo_combustible,
                TipoTransmision = entity.tipo_transmision,

                CapacidadPasajeros = entity.capacidad_pasajeros,
                CapacidadMaletas = entity.capacidad_maletas,
                NumeroPuertas = entity.numero_puertas,
                AireAcondicionado = entity.aire_acondicionado,

                PrecioBaseDia = entity.precio_base_dia,
                KilometrajeActual = entity.kilometraje_actual,

                Observaciones = entity.observaciones_generales,
                ImagenUrl = entity.imagen_referencial_url,

                Estado = entity.estado_vehiculo,
                EsEliminado = entity.es_eliminado,

                OrigenRegistro = entity.origen_registro,

                FechaRegistroUtc = entity.fecha_registro_utc,
                CreadoPorUsuario = entity.creado_por_usuario,

                ModificadoPorUsuario = entity.modificado_por_usuario,
                FechaModificacionUtc = entity.fecha_modificacion_utc,
                ModificadoDesdeIp = entity.modificado_desde_ip,

                FechaInhabilitacionUtc = entity.fecha_inhabilitacion_utc,
                MotivoInhabilitacion = entity.motivo_inhabilitacion,

                RowVersion = entity.row_version
            };
        }

        // 🔁 DataModel → Entity
        public static VehiculoEntity ToEntity(VehiculoDataModel model)
        {
            return new VehiculoEntity
            {
                id_vehiculo = model.Id,
                vehiculo_guid = model.Guid,

                codigo_interno_vehiculo = model.CodigoInterno,
                placa_vehiculo = model.Placa,

                id_marca = model.IdMarca,
                id_categoria = model.IdCategoria,
                localizacion_actual = model.IdLocalizacion,

                modelo_vehiculo = model.Modelo,
                anio_fabricacion = model.AnioFabricacion,
                color_vehiculo = model.Color,
                tipo_combustible = model.TipoCombustible,
                tipo_transmision = model.TipoTransmision,

                capacidad_pasajeros = model.CapacidadPasajeros,
                capacidad_maletas = model.CapacidadMaletas,
                numero_puertas = model.NumeroPuertas,
                aire_acondicionado = model.AireAcondicionado,

                precio_base_dia = model.PrecioBaseDia,
                kilometraje_actual = model.KilometrajeActual,

                observaciones_generales = model.Observaciones,
                imagen_referencial_url = model.ImagenUrl,

                estado_vehiculo = model.Estado,
                es_eliminado = model.EsEliminado,

                origen_registro = model.OrigenRegistro,

                fecha_registro_utc = model.FechaRegistroUtc,
                creado_por_usuario = model.CreadoPorUsuario,

                modificado_por_usuario = model.ModificadoPorUsuario,
                fecha_modificacion_utc = model.FechaModificacionUtc,
                modificado_desde_ip = model.ModificadoDesdeIp,

                fecha_inhabilitacion_utc = model.FechaInhabilitacionUtc,
                motivo_inhabilitacion = model.MotivoInhabilitacion,

                row_version = model.RowVersion
            };
        }

        // 🔥 Helper lista
        public static List<VehiculoDataModel> ToDataModelList(IEnumerable<VehiculoEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}