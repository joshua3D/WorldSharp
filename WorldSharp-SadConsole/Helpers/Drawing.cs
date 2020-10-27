using Microsoft.Xna.Framework;
using WorldSharpDLL.Elements;
using Console = SadConsole.Console;

namespace WorldSharp_SadConsole.Helpers
{
    public static class Drawing
    {
        public static void DrawWorldOnConsole(Console console, World world) 
        {
            if (console == null || world == null)
                return;

            int width = world.Width;
            int height = world.Height;

            var tiles = world.Tiles;

            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    Tile tile = tiles[x][y];

                    byte r = tile.R;
                    byte g = tile.G;
                    byte b = tile.B;
                    byte a = tile.A;

                    console.SetGlyph(x, y, 219, new Color(r, g, b, a));
                }
            }
        }
    }
}
