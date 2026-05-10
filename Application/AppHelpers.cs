using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Application
{
    internal static class AppHelpers
    {

        public static event EventHandler<bool> ResultSetDns;
        public static void SaveToImage(this InkCanvas inkCanvas, string filePath, ImageFormat format)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)inkCanvas.RenderSize.Width,
                (int)inkCanvas.RenderSize.Height,
                96,
                96,
                PixelFormats.Pbgra32);

            rtb.Render(inkCanvas);

            BitmapEncoder encoder = null;
            if (format == ImageFormat.Png)
            {
                encoder = new PngBitmapEncoder();
            }
            else
            {
                throw new ArgumentException("فرمت تصویر پشتیبانی نمی‌شود.");
            }

            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(fs);
            }
        }

    }
}