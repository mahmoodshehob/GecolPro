namespace GecolPro.BusinessRules.Interfaces
{
    public interface ISendMessage
    {
        public Task SendGecolMessage(string receiver, string message, string ConversationID);

        public Task SendGecolMessageTest(string receiver, string message, string ConversationID);


        public Task<(bool, object)> SendGecolMessageWR(string receiver, string message, string ConversationID);
    }
}
