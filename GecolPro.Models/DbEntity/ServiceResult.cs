namespace GecolPro.Models.DbEntity
{
    public class ServiceResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public ServiceResult(bool status, string message)
        {
            Status = status;
            Message = message;
        }

    }
}
