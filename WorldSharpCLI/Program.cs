using System;
using System.Drawing;
using WorldSharpDLL.Drawing;
using WorldSharpDLL.Generators;
using WorldSharpDLL.IO;

namespace WorldSharpCLI
{
    class Program
    {
        public static readonly Random Random = new Random();
        static void Main(string[] args)
        {
            var generator = new Generator();

            var world = generator.GenerateWorld(80, 32, Random.Next(int.MaxValue));

            byte[] jsonUtf8GZipped = world.ToJsonUTF8GZipped(true);

            // write to json
            if (FileHandling.TryWriteJsonUTF8GZipped(jsonUtf8GZipped))
            {
                Console.WriteLine("Success Writing jsonUtf8");
            }

            // build a texture
            if (TextureBuilder.TryGenerateBitmap(world, out Bitmap bitmap))
            {
                Console.WriteLine("Success Building Bitmap");

                // write to .bmp
                if (FileHandling.TryWriteImageToBitmap(bitmap))
                {
                    Console.WriteLine("Success Writing Bitmap");
                }
            }
        }
    }
}
