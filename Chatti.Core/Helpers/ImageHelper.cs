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

        public static string GenerateThumbnail(string imagePath, int width = 150, int height = 150)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Image file not found.", imagePath);
            }
            using var inputStream = File.OpenRead(imagePath);
            using var original = SKBitmap.Decode(inputStream);

            // Resize the image (keeping aspect ratio)
            var resizedImage = ResizeImage(original, width, height);

            // Apply blur effect
            using var blurred = new SKBitmap(width, height);
            using var canvas = new SKCanvas(blurred);
            var paint = new SKPaint
            {
                ImageFilter = SKImageFilter.CreateBlur(10, 10)
            };
            canvas.DrawBitmap(resizedImage, 0, 0, paint);
            // Convert to compressed format
            using var image = SKImage.FromBitmap(blurred);
            var directory = Path.GetDirectoryName(imagePath);
            var filenameWithoutExt = Path.GetFileNameWithoutExtension(imagePath);
            var extension = Path.GetExtension(imagePath);
            var thumbnailPath = Path.Combine(directory!, $"{filenameWithoutExt}_thumbnail{extension}");
            using var outputStream = File.OpenWrite(thumbnailPath);
            image.Encode(SKEncodedImageFormat.Jpeg, 80).SaveTo(outputStream);
            return thumbnailPath;
        }
        private static SKBitmap ResizeImage(SKBitmap original, int maxWidth, int maxHeight)
        {
            float ratioX = (float)maxWidth / original.Width;
            float ratioY = (float)maxHeight / original.Height;
            float ratio = Math.Min(ratioX, ratioY);
            int newWidth = (int)(original.Width * ratio);
            int newHeight = (int)(original.Height * ratio);

            var resized = original.Resize(new SKImageInfo(newWidth, newHeight), SKSamplingOptions.Default);
            return resized ?? original; 
        }
    }
}
