using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using WorldSharp_SadConsole.Helpers;
using WorldSharpDLL.Generators;
using Console = SadConsole.Console;

namespace WorldSharp_SadConsole
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 32;

        public static readonly Random Random = new Random();
        public static Generator Generator = new Generator();
        public static Console GameConsole = null;

        private static float _delta = default;

        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime time) 
        {
            const float DELAY = 1000;

            _delta += time.ElapsedGameTime.Milliseconds;

            if (_delta > DELAY) 
            {
                _delta = 0f;

                var world = Generator.GenerateWorld(Width, Height, Random.Next(int.MaxValue));

                Drawing.DrawWorldOnConsole(GameConsole, world);
            };
        }

        private static void Init()
        {
            var world = Generator.GenerateWorld(Width, Height, Random.Next(int.MaxValue));

            if(Conversions.TryWorldToConsole(world, out SadConsole.Console console)) 
            {
                // Any startup code for your game. We will use an example console for now
                GameConsole = console;

                SadConsole.Global.CurrentScreen = console;
            }
        }
    }
}