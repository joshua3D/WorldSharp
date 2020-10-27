using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WorldSharpDLL.Elements;

namespace WorldSharpDLL.Drawing
{
    public static class TextureBuilder
    {
        public static bool TryGenerateBitmap(World world, out Bitmap bitmap)
        {
            bitmap = null;

            if (world == null || world.Tiles == null)
                return false;

            try
            {
                int? width = world?.Tiles?.Length;
                int? height = world?.Tiles[0]?.Length;

                if(width != null && height != null) 
                {
                    int w = (int)width;
                    int h = (int)height;

                    bitmap = new Bitmap(w,h);

                    //http://csharpexamples.com/fast-image-processing-c/
                    //https://github.com/MangoDevx/SmartPixel/blob/master/Services/FormService.cs

                    BitmapData bitMapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / (int)8;
                    int byteCount = bitMapData.Stride * bitmap.Height;
                    byte[] pixels = new byte[byteCount];
                    IntPtr ptrFirstPixel = bitMapData.Scan0;
                    Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
                    int heightInPixels = bitMapData.Height;
                    int widthInBytes = bitMapData.Width * bytesPerPixel;

                    var tiles = world.Tiles;

                    Parallel.For(0, heightInPixels, y =>
                    {
                        int j = y * bitMapData.Stride;

                        for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                        {
                            int xi = (x / bytesPerPixel);

                            pixels[j + x + 3] = tiles[xi][y].A;
                            pixels[j + x + 2] = tiles[xi][y].R;
                            pixels[j + x + 1] = tiles[xi][y].G;
                            pixels[j + x] = tiles[xi][y].B;
                        }
                    });

                    // copy modified bytes back
                    Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
                    bitmap.UnlockBits(bitMapData);
                } 
            }
            catch { }

            return (bitmap != null);
        }
    }
}
