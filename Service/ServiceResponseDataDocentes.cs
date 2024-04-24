namespace AkademicReport.Service
{
    public class ServiceResponseDataDocentes<T>
    {
        public int? Status { get; set; }
        public T? Data { get; set; }
        public int? Total { get; set; } = 0;
    }
}
