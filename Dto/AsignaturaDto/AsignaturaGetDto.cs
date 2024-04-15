namespace AkademicReport.Dto.AsignaturaDto
{
    public class AsignaturaGetDto
    {
        public int? Id { get; set; }
        public int? Id_concepto { get; set; }
        public string? NombreConcepto { get; set; }
        public string? Codigo { get; set; }
        public List<TipoCargaDto>? TiposCargas { get; set; }
        public List<string>? Modalida { get; set; }
        public string? Nombre { get; set; }
        public string? Horas { get; set; }
        public string? Descripcion { get; set; }
    }
}
