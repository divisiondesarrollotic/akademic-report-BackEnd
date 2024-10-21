using AkademicReport.Dto.ReporteDto;

namespace AkademicReport.Service
{
    public class ServiceResponseReporte<T>
    {
        public int? Status { get; set; }
        public MontosPosgradosDto? MontoTotalesPorMes { get; set; }
        public int totalRecinto { get; set; }
        public string? Message { get; set; }
        public int Anio { get; set; }
        public T? Data { get; set; }
       
       
    }
}
