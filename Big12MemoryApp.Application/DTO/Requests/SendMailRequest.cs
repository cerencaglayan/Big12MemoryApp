
namespace Big12MemoryApp.Application.DTO.Requests
{
    public class SendMailRequest
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}