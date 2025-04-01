namespace AkademicReport.Dto.CargaDto
{
    public class NotaCargaIrregularDto
    {
        public int? Id { get; set; }
        public int? IdPeriodo { get; set; }
        public string? Nota { get; set; }
        public string? Cedula { get; set; }
        public int? IdTipoReporte { get; set; }
        public int? IdTipoReporteIrregular { get; set; }
    }
}
