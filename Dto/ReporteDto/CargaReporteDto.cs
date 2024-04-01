namespace AkademicReport.Dto.ReporteDto
{
    public class CargaReporteDto
    {
        public string Periodo { get; set; } = null!;
        public int curricular { get; set; }
        public string codigo_asignatura { get; set; } = null!;
        public string nombre_asignatura { get; set; } = null!;
        public int id { get; set; }
        public int seccion { get; set; }
        public string Horario_dia { get; set; } = null!;
        public string Horario_inicio { get; set; } = null!;
        public string Horario_final { get; set; } = null!;
        public int credito { get; set; }
        public int precio_hora { get; set; }
        public int pago_asignatura { get; set; }
        public string recinto { get; set; } = null!;
    }
}
