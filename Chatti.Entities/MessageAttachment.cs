using Chatti.Core.Enums;

namespace Chatti.Entities
{
    public class MessageAttachment
    {
        public required string FileName { get; set; }
        public required string AttachmentPath { get; set; }
        public MimeType Type { get; set; }
    }
}
