

namespace Booking.Autos.DataAccess.Entities
{
    public class RolEntity
    {
        public int id_rol { get; set; }

        public Guid rol_guid { get; set; }

        public string nombre_rol { get; set; }

        public string? descripcion_rol { get; set; }

        public string estado_rol { get; set; }

        public bool es_eliminado { get; set; }

        public bool activo { get; set; }

        public DateTime fecha_registro_utc { get; set; }

        public string creado_por_usuario { get; set; }

        public string? modificado_por_usuario { get; set; }

        public DateTime? fecha_modificacion_utc { get; set; }

        public byte[] row_version { get; set; }

        // Navegación
        public virtual ICollection<UsuarioRolEntity> UsuariosRoles { get; set; }
    }
}