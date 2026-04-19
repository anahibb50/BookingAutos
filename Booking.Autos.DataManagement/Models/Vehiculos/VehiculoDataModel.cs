namespace Booking.Autos.DataManagement.Models.Vehiculos
{
    public class VehiculoDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string CodigoInterno { get; set; }

        public string Placa { get; set; }

        // 🔗 Relaciones
        public int IdMarca { get; set; }

        public int IdCategoria { get; set; }

        public int IdLocalizacion { get; set; }

        // 🚗 Información del vehículo
        public string Modelo { get; set; }

        public short AnioFabricacion { get; set; }

        public string Color { get; set; }

        public string TipoCombustible { get; set; }

        public string TipoTransmision { get; set; }

        public byte CapacidadPasajeros { get; set; }

        public byte CapacidadMaletas { get; set; }

        public byte NumeroPuertas { get; set; }

        public bool AireAcondicionado { get; set; }

        // 💰 Operativo
        public decimal PrecioBaseDia { get; set; }

        public int KilometrajeActual { get; set; }

        // 📝 Extras
        public string? Observaciones { get; set; }

        public string? ImagenUrl { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        public bool EsEliminado { get; set; }

        // 🧾 Auditoría
        public string OrigenRegistro { get; set; }

        public DateTime FechaRegistroUtc { get; set; }

        public string CreadoPorUsuario { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificadoDesdeIp { get; set; }

        public DateTime? FechaInhabilitacionUtc { get; set; }

        public string? MotivoInhabilitacion { get; set; }

        // 🔒 Concurrencia
        public byte[] RowVersion { get; set; }
    }
}