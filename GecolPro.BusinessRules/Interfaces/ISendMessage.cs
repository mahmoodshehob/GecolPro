namespace GecolPro.BusinessRules.Interfaces
{
    public interface ISendMessage
    {
        public Task SendGecolMessage(string receiver, string message, string ConversationID);

    }
}
