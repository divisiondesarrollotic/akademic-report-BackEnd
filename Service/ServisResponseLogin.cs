namespace AkademicReport.Service
{
    public class ServisResponseLogin<T, I>
    {
        public int Status { get; set; }
        public (T, I)? Message { get; set; }
        public bool LoginResetPassword { get; set; }
    }
}
