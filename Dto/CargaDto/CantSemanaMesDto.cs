using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.CargaDto
{
    public class CantSemanaMesDto
    {
        public MesGetDto? MesObj { get; set; }
        public int CantSemanas { get; set; }
        public decimal Monto { get; set; }
    }
}
