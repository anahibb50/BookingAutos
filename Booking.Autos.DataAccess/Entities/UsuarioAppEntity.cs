
namespace Booking.Autos.DataAccess.Entities
{
    public class UsuarioAppEntity
    {
        public int id_usuario { get; set; }

        public Guid usuario_guid { get; set; }

        public string username { get; set; }

        public string correo { get; set; }

        public string password_hash { get; set; }

        public string password_salt { get; set; }

        public string estado_usuario { get; set; }

        public bool es_eliminado { get; set; }

        public bool activo { get; set; }

        public DateTime fecha_registro_utc { get; set; }

        public string creado_por_usuario { get; set; }

        public string? modificado_por_usuario { get; set; }

        public DateTime? fecha_modificacion_utc { get; set; }

        public byte[] row_version { get; set; }

        public int? id_cliente { get; set; }

        // Navegación
        public virtual ClienteEntity Cliente { get; set; }

        public virtual ICollection<UsuarioRolEntity> UsuariosRoles { get; set; }
    }
}