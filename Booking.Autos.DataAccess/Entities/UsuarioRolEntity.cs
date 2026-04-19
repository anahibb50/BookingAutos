namespace Booking.Autos.DataAccess.Entities
{
    public class UsuarioRolEntity
    {
        public int id_usuario_rol { get; set; }

        public int id_usuario { get; set; }

        public int id_rol { get; set; }

        public string estado_usuario_rol { get; set; }

        public bool es_eliminado { get; set; }

        public bool activo { get; set; }

        public DateTime fecha_registro_utc { get; set; }

        public string creado_por_usuario { get; set; }

        public string? modificado_por_usuario { get; set; }

        public DateTime? fecha_modificacion_utc { get; set; }

        public byte[] row_version { get; set; }

        // Navegación
        public virtual UsuarioAppEntity UsuarioApp { get; set; }

        public virtual RolEntity Rol { get; set; }
    }
}