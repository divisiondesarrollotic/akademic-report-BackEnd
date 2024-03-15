namespace AkademicReport.Service
{
    public class ServiceResponseData<T>
    {
        public int? Status { get; set; }
        public T? Data { get; set; }
      
    }
}
