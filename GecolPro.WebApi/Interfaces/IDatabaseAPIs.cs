namespace GecolPro.WebApi.Interfaces
{
    public interface IDatabaseAPIs
    {
        public Task<bool> IsMeterExist(string meter);

        public Task<bool> CreateNewMeter(string meter, string at, string tt);

        public Task<bool> SaveDcblRequest(string MSISDN, string Amount, string Status, string TransactionId);

        public Task<bool> SaveGecolRequest(string MSISDN, string Amount, string Status, string Token, string UniqueNumber, string TotalTax);
    }
}
