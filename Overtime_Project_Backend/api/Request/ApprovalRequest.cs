public class ApprovalRequest
{
    public string Comments { get; set; }  // Comentarios de aprobación
    public decimal Cost { get; set; }     // Costo calculado para las horas extra
}

public class RejectRequest
{
    public string Reason { get; set; }    // Razón del rechazo
    public string Comments { get; set; }  // Comentarios adicionales
}
