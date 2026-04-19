using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Autos.DataAccess.Entities
{
    public class LocalizacionEntity
    {

        public int id_localizacion { get; set; }

        // --- CAMPOS PRINCIPALES ---
        public Guid localizacion_guid { get; set; }

        public string codigo_localizacion { get; set; } // VARCHAR(20)

        public string nombre_localizacion { get; set; } // VARCHAR(100)

        public int id_ciudad { get; set; }

        public string direccion_localizacion { get; set; } // VARCHAR(200)

        public string telefono_contacto { get; set; } // VARCHAR(20)

        public string correo_contacto { get; set; } // VARCHAR(120)

        public string horario_atencion { get; set; } // VARCHAR(120)

        public string zona_horaria { get; set; } // VARCHAR(50)

        // --- ESTADO / CICLO DE VIDA ---
        public string estado_localizacion { get; set; } // CHAR(3) - 'ACT' o 'INA'

        public bool es_eliminado { get; set; }

        // --- AUDITORÍA ---
        public DateTime fecha_registro_utc { get; set; } // DATETIME2(0)

        public string creado_por_usuario { get; set; } // VARCHAR(100)

        public string? modificado_por_usuario { get; set; } // VARCHAR(100)

        public DateTime? fecha_modificacion_utc { get; set; }

        public string? modificado_desde_ip { get; set; } // VARCHAR(45)

        public DateTime? fecha_inhabilitacion_utc { get; set; }

        // --- INTEGRACIÓN / ORIGEN ---
        public string origen_registro { get; set; } // VARCHAR(20)

        public string? motivo_inhabilitacion { get; set; } // VARCHAR(200)

        // --- CONCURRENCIA ---

        public byte[] row_version { get; set; }

        public virtual CiudadEntity Ciudad { get; set; }

        public virtual ICollection<VehiculoEntity> Vehiculos { get; set; }
    }
}