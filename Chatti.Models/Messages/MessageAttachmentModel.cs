namespace Chatti.Models.Messages
{
    public class MessageAttachmentModel
    {
        public required string FileName { get; set; }
        public required string AttachmentPath { get; set; }
        public required string Type { get; set; }
        public double SizeInKB { get; set; }
        public string? Thumbnail { get; set; }

    }
}
