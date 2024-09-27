namespace AkademicReport.Dto.PeriodoDto
{
    public class PeriodoUpdateDto
    {
        public int? Id { get; set; }
        public string Periodo { get; set; } = null!;
        public string? Descripcion { get; set; }

        public bool estado { get; set; }
    }   
}
