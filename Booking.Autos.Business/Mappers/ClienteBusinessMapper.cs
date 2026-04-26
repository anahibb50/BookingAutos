using Booking.Autos.Business.DTOs.Cliente;
using Booking.Autos.DataManagement.Models.Clientes;

namespace Booking.Autos.Business.Mappers
{
    public static class ClienteBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static ClienteDataModel ToDataModel(CrearClienteRequest request)
        {
            return new ClienteDataModel
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,

                RazonSocial = request.RazonSocial,

                TipoIdentificacion = request.TipoIdentificacion,
                Identificacion = request.Identificacion,

                IdCiudad = request.IdCiudad,
                Direccion = request.Direccion,

                Genero = request.Genero,

                Telefono = request.Telefono,
                Email = request.Email,

                // 🔥 ESTADO INICIAL
                Estado = "ACT",

                // 🔥 AUDITORÍA
                FechaRegistroUtc = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static ClienteDataModel ToDataModel(ActualizarClienteRequest request)
        {
            return new ClienteDataModel
            {
                Id = request.Id,

                Nombre = request.Nombre,
                Apellido = request.Apellido,

                RazonSocial = request.RazonSocial,

                TipoIdentificacion = request.TipoIdentificacion,
                Identificacion = request.Identificacion,

                IdCiudad = request.IdCiudad,
                Direccion = request.Direccion,

                Genero = request.Genero,

                Telefono = request.Telefono,
                Email = request.Email,

                // 🔥 AUDITORÍA
                FechaModificacionUtc = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static ClienteResponse ToResponse(ClienteDataModel model)
        {
            return new ClienteResponse
            {
                Id = model.Id,
                Guid = model.Guid,

                Nombre = model.Nombre,
                Apellido = model.Apellido,
                RazonSocial = model.RazonSocial,

                TipoIdentificacion = model.TipoIdentificacion,
                Identificacion = model.Identificacion,

                IdCiudad = model.IdCiudad,
                Direccion = model.Direccion,
                Genero = model.Genero,

                Telefono = model.Telefono,
                Email = model.Email,

                Estado = model.Estado
            };
        }

        // 🔥 LISTA (PRO)
        public static List<ClienteResponse> ToResponseList(IEnumerable<ClienteDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}