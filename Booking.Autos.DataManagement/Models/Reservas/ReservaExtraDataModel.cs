public class ReservaExtraDataModel
{
    public int Id { get; set; }
    public Guid Guid { get; set; }

    public int IdReserva { get; set; }
    public int IdExtra { get; set; }

    public int Cantidad { get; set; }

    public decimal ValorUnitario { get; set; }
    public decimal Subtotal { get; set; }

    public string Estado { get; set; }
    public bool EsEliminado { get; set; }

    // 🔥 AUDITORÍA (FALTABA)
    public string? CreadoPorUsuario { get; set; }
    public string? ModificadoPorUsuario { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public DateTime? FechaEliminacion { get; set; }
}