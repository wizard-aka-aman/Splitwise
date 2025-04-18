namespace Splitwise.Model.Chat
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
