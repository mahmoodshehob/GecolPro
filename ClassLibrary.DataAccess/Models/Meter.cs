namespace ClassLibrary.DataAccess.Models
{
    public class Meter
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string? at { get; set; }
        public string? tt { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
        public bool IsActive { get; set; } = true;

    }
}
