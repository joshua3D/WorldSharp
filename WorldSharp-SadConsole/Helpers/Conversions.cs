using WorldSharpDLL.Elements;
using Console = SadConsole.Console;
using Color = Microsoft.Xna.Framework.Color;

namespace WorldSharp_SadConsole.Helpers
{
    public static class Conversions
    {
        public static bool TryWorldToConsole(World world, out Console console) 
        {
            console = null;

            if (world != null) 
            {
                console = new Console(world.Width, world.Height);

                Drawing.DrawWorldOnConsole(console, world);
            }

            return console != null;
        }
    }
}
