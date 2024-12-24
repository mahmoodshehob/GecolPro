namespace ZGecolPro.SmppClient.Services.IServices
{
    public interface ILoggers
    {
        public Task LogInfoAsync(string message);

        public Task LogErrorAsync(string message);

        public Task LogDebugAsync(string message);



    }
}
