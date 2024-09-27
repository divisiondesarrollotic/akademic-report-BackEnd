namespace AkademicReport.Dto.PeriodoDto
{
    public class PeriodoGetDto
    {
        public int Id { get; set; }
        public string Periodo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool estado { get; set; }
        public bool EstadoPosgrado { get; set; }
    }
}
