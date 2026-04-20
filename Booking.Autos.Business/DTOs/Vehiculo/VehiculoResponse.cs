namespace Booking.Autos.Business.DTOs.Vehiculo
{
    public class VehiculoResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string CodigoInterno { get; set; }
        public string Placa { get; set; }

        // 🔗 Relaciones
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public int IdLocalizacion { get; set; }

        // 🚗 Info
        public string Modelo { get; set; }
        public short AnioFabricacion { get; set; }
        public string Color { get; set; }

        public string TipoCombustible { get; set; }
        public string TipoTransmision { get; set; }

        public byte CapacidadPasajeros { get; set; }
        public byte CapacidadMaletas { get; set; }
        public byte NumeroPuertas { get; set; }

        public bool AireAcondicionado { get; set; }

        // 💰
        public decimal PrecioBaseDia { get; set; }

        public int KilometrajeActual { get; set; }

        // 📝
        public string? Observaciones { get; set; }
        public string? ImagenUrl { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }
    }
}