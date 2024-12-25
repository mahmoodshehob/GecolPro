using GecolPro.Models.Gecol;

namespace GecolPro.Models.DbEntity
{
    public class Meter : CustVendDetail
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string? at { get; set; }
        public string? tt { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
        public bool IsActive { get; set; } = true;

    }
}
