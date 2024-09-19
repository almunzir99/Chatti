using Chatti.Core.Enums;

namespace Chatti.Entities
{
    public class Attachment : EntityBase
    {
        public required string FileName { get; set; }
        public required string AttachmentPath { get; set; }
        public MimeType Type { get; set; }
    }
}
