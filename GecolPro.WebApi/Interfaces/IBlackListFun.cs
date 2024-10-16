namespace GecolPro.WebApi.Interfaces
{
    public interface IBlackListFun
    {
        public Task SyncBlackList(List<string> blackMsisdnList);
        public Task<List<string>> GetBlackList();
        public Task SyncDeleteBlackList(string phoneNumber);

    }
}
