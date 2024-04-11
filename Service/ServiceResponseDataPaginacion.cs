namespace AkademicReport.Service
{
    public class ServiceResponseDataPaginacion<T>
    {
        public int? Status { get; set; }
        public T? Data { get; set; }
        public int? TotalPaginas { get; set; }
        public int? TotalRegistros { get; set; }
    }
}
