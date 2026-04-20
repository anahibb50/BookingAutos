namespace Booking.Autos.Business.DTOs.Vehiculo
{
    public class CrearVehiculoRequest
    {
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

        public string? Observaciones { get; set; }

        public int KilometrajeActual { get; set; }

        public bool AireAcondicionado { get; set; }


        // 💰
        public decimal PrecioBaseDia { get; set; }

        // 📝
        public string? ImagenUrl { get; set; }
    }
}