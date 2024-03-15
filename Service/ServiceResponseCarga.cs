namespace AkademicReport.Service
{
    public class ServiceResponseCarga<T, I>
    {
        public int Status { get; set; }
        public (T, I)? Data { get; set; }
    }
}
