﻿using Chatti.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Core.Helpers
{
    public static class MIMETypeHelper
    {
        public static MimeType GetMimeType(string extension)
        {
            var imgExtensions = new List<string>() { ".png", ".jpeg", ".jpg", ".jif", ".webp", ".svg" };
            var documentsExtensions = new List<string>() { ".doc", ".docx", ".rtf", ".csv", ".xlsx", ".xlsx" };
            var pdfExtensions = new List<string>() { ".pdf" };
            var audioExtensions = new List<string>() { ".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma", ".m4a", ".opus", ".aiff", ".alac" };

            if (imgExtensions.Contains(extension.ToLower()))
                return MimeType.IMAGE;
            else if (documentsExtensions.Contains(extension.ToLower()))
                return MimeType.DOCUMENT;
            else if (pdfExtensions.Contains(extension.ToLower()))
                return MimeType.PDF;
            else if (audioExtensions.Contains(extension.ToLower()))
                return MimeType.AUDIO;
            else
                return MimeType.File;

        }
    }
}
