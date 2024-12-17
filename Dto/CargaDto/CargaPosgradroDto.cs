using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.CargaDto
{
    public class CargaPosgradroDto
    {
        public int? Id { get; set; }
        public string Periodo { get; set; } = null!;
        public string Recinto { get; set; } = null!;
        public string CodAsignatura { get; set; } = null!;
        [Required]
        public int IdCodigo { get; set; }
        public string NombreAsignatura { get; set; } = null!;
        public string CodUniversitas { get; set; } = null!;
        public int? Modalidad { get; set; }
        public int Dias { get; set; }
        public string? HoraInicio { get; set; }
        public string? MinutoInicio { get; set; }
        public string? HoraFin { get; set; }
        public string? MinutoFin { get; set; }
        public int Credito { get; set; }
        public string NombreProfesor { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public int? DiaMes { get; set; }
        public int? IdMes { get; set; }
        public string? Anio { get; set; }
        public int? IdPrograma { get; set; }
        public int? IdConceptoPosgrado { get; set; }
        [Required]
        public int? idTipoCarga { get; set; }
        public int? idUsuario { get; set; }

    }
}
