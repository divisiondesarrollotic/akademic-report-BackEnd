namespace AkademicReport.Service
{
    public class ServiceResponseReporte<T>
    {
        public int? Status { get; set; }
        public T? Data { get; set; }
        public int totalRecinto { get; set; }
    }
}
