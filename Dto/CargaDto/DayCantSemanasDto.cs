using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.CargaDto
{
    public class DayCantSemanasDto
    {

        [Required]
        public int Mes { get; set; }
        [Required]
        public int CantSemanas { get; set; }
        public int? Monto { get; set; }


    }
}
    