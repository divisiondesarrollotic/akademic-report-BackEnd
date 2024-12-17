using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.DocentesDto
{
    public class FiltroDocentesDto
    {
        public string? Filtro { get; set; }
        public int? elementosPorPagina { get; set; }
        public int? paginaActual { get; set; }
        public int? idPrograma { get; set; }
    }
}

