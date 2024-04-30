namespace API.Utility
{
    public class EmailMetadata
    {
        public string ToAdress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Attachment { get; set; }

        public EmailMetadata(string toAdress, string subject, string body, string attachment = "")
        {
            ToAdress = toAdress;
            Subject = subject;
            Body = body;
            Attachment = attachment;

        }
    }
}
