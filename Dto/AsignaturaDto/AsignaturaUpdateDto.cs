namespace AkademicReport.Dto.AsignaturaDto
{
    public class AsignaturaUpdateDto
    {
        public int Id { get; set; }
        public int? IdConcepto { get; set; }
        public string? Codigo1 { get; set; }
        public string? Modalida { get; set; }
        public string? Nombre { get; set; }
        public string? Horas { get; set; }
        public string? Descripcion { get; set; }
    }
}
