﻿using Chatti.Core.Enums;

namespace Chatti.Entities.Messages
{
    public class MessageAttachment
    {
        public required string FileName { get; set; }
        public required string AttachmentPath { get; set; }
        public string? Thumbnail { get; set; }
        public MimeType Type { get; set; }
    }
}
