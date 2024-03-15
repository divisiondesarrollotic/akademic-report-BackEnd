namespace AkademicReport.Service
{
    public class ServicesResponseMessage<T>
    {
        public int? Status { get; set; }
        public T? Message { get; set; }
    }
}
