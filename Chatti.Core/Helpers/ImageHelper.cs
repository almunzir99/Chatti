using SkiaSharp;
using PuppeteerSharp;
using PDFiumSharp;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.Arm;
using System.Drawing;
using System.Drawing.Imaging;
namespace Chatti.Core.Helpers
{
    public static class ImageHelper
    {

        public static string GenerateThumbnail(string imagePath, int width = 150, int height = 150, int blurSigma = 10)
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
                ImageFilter = SKImageFilter.CreateBlur(blurSigma, blurSigma)
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
        public static string GeneratePdfSnapshotAsync(string pdfPath, int dpi = 72)
        {
            using var doc = new PdfDocument(pdfPath);
            using var page = doc.Pages[0];

            var width = (int)(page.Width * dpi / 72);
            var height = (int)(page.Height * dpi / 72);

            using var bitmap = new PDFiumBitmap(width, height, true);
            page.Render(bitmap);

            var directory = Path.GetDirectoryName(pdfPath);
            var filenameWithoutExt = Path.GetFileNameWithoutExtension(pdfPath);
            var outputPath = Path.Combine(directory!, $"{filenameWithoutExt}_snapshot.png");
            using var outputStream = File.OpenWrite(outputPath);
            bitmap.Save(outputStream);
            outputStream.Flush();
            outputStream.Close();
            // Optionally, you can also create a thumbnail
            var thumbnailPath = GenerateThumbnail(outputPath, width, height, 1);
            // delete the original snapshot if you only want the thumbnail
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

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
