namespace GecolPro.WebApi.Interfaces
{
    public interface ISendMessage
    {
        public Task SendGecolMessage(string receiver, string message, string ConversationID);

    }
}
