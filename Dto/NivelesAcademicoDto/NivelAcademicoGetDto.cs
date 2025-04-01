namespace AkademicReport.Dto.NivelesAcademicoDto
{
    public class NivelAcademicoGetDto
    {
        public int Id { get; set; }
        public string Nivel { get; set; } = null!;
        public int PagoHora { get; set; }
        public int? IdPrograma { get; set; }

    }
}
