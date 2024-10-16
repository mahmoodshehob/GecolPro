namespace GecolPro.WebApi.Interfaces
{
    public interface ISendMessage
    {
        public Task SendGecolMessage(string? sender, string receiver, string message, string ConversationID);

    }
}
