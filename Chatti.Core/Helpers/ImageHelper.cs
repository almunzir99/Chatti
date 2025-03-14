using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Core.Helpers
{
    public static class ImageHelper
    {

        public static string GenerateThumbnail(string imagePath, int width = 70, int height = 70)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Image file not found.", imagePath);
            }
            using var inputStream = File.OpenRead(imagePath);
            using var original = SKBitmap.Decode(inputStream);
            if (original == null)
            {
                throw new Exception("Failed to load image.");
            }
            var resized = original.Resize(new SKImageInfo(width,height), SKSamplingOptions.Default);
            if (resized == null)
            {
                throw new Exception("Failed to resize image.");
            }
            var directory = Path.GetDirectoryName(imagePath);
            var filenameWithoutExt = Path.GetFileNameWithoutExtension(imagePath);
            var extension = Path.GetExtension(imagePath);
            var thumbnailPath = Path.Combine(directory!, $"{filenameWithoutExt}_thumbnail{extension}");

            using var outputStream = File.OpenWrite(thumbnailPath);
            resized.Encode(SKEncodedImageFormat.Jpeg, 80).SaveTo(outputStream);
            return thumbnailPath;
        }
    }
}
