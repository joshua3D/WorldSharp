using System;
using System.Drawing;
using WorldSharpDLL.Elements;
using Noise = FastNoiseLite;
using Topography = WorldSharpDLL.Elements.Tile.Topography;

namespace WorldSharpDLL.Generators
{
    public class Generator
    {
        //https://davidmathlogic.com/colorblind/#%23D81B60-%231E88E5-%23FFC107-%23004D40
        // default palette chosen to aid visibility to color-blind
        // water, beach, grass, hill, mountain, ice

        protected const string PALETTE_WTR = "#4B0092";
        protected const string PALETTE_BCH = "#FEFE62";
        protected const string PALETTE_GRS = "#E1BE6A";
        protected const string PALETTE_HIL = "#40B0A6";
        protected const string PALETTE_MNT = "#D41159";
        protected const string PALETTE_ICE = "#FFFFFF";

        protected Noise NoiseProvider { get; set; }

        public struct Settings
        {
            public Noise.FractalType FractalType { get; set; }
            public Noise.NoiseType NoiseType { get; set; }
            public float Frequency { get; set; }
            public int Octaves { get; set; }
        }

        protected readonly Random SeedRandom = new Random();

        private Settings _defaultSettings = new Settings
        {
            FractalType = Noise.FractalType.None,
            NoiseType = Noise.NoiseType.OpenSimplex2,
            Frequency = 0.075f,
            Octaves = 3,
        };

        protected virtual Settings DefaultSettings
        {
            get
            {
                return _defaultSettings;
            }
            set 
            {
                _defaultSettings = value;
            }
        }

        public Generator() { }

        public Generator(Settings defaultSettings) 
        {
            _defaultSettings = defaultSettings;
        }

        public virtual World GenerateWorld(int width, int height, int? seed = null)
        {
            return GenerateWorld(width, height, DefaultSettings, seed);
        }

        public virtual World GenerateWorld(int width, int height, Settings? generatorSettings, int? seed = null)
        {
            if (generatorSettings == null)
                _ = DefaultSettings;

            if (seed == null)
                seed = 1337;
       
            var settings = (Settings)generatorSettings;
 
            var fractalType = settings.FractalType;
            var noiseType = settings.NoiseType;
            var frequency = settings.Frequency;
            var octaves = settings.Octaves;
            var generationSeed = (int)seed;

            NoiseProvider = new Noise(generationSeed);

            NoiseProvider.SetNoiseType(noiseType);
            NoiseProvider.SetFractalType(fractalType);
            NoiseProvider.SetFractalOctaves(octaves);
            NoiseProvider.SetFrequency(frequency);

            var world = new World()
            {
                Tiles = new Tile[width][],
                Width = width,
                Height = height
            };

            const float offset = 1.0f;

            for (int x = 0; x < world.Width; x += 1)
            {
                world.Tiles[x] = new Tile[height];

                for (int y = 0; y < world.Height; y += 1)
                {
                    Tile tile = new Tile()
                    {
                        X = x,
                        Y = y
                    };

                    // FastNoise range is (-1.0 ---> 1.0)
                    // Add offset to make range (0.0 ---> 2.0)
                    tile.Elevation = (NoiseProvider.GetNoise(x, y) + offset);

                    // ReMap (0.0 ---> 2.0) to (0.0 ---> 1.0)
                    tile.Elevation = (float)Mathf.Map((decimal)tile.Elevation, 0.0M, 2.0M, 0.0M, 1.0M);

                    tile.Terrain = ComputeTypography(tile.Elevation);

                    Color color = ComputeColor(tile.Terrain);

                    tile.R = color.R;
                    tile.G = color.G;
                    tile.B = color.B;
                    tile.A = color.A;

                    world.Tiles[x][y] = tile;
                }
            }

            return world;
        }

        protected virtual Topography ComputeTypography(float elevation, object args = null) 
        {
            // the most basic approach to computing topography is to compare elevation
            // the values set here are arbitrary, and chosen for the simplest output

            if (elevation <= 0.10f)
            {
                return Topography.Water;
            }
            else if (elevation <= 0.20f)
            {
                return Topography.Beach;
            }
            else if (elevation <= 0.70f)
            {
                return Topography.Grass;
            }
            else if (elevation <= 0.80f)
            {
                return Topography.Hills;
            }
            else if (elevation <= 0.93f)
            {
                return Topography.Mountains;
            }
            else 
            {
                return Topography.Ice;
            }
        }

        protected virtual Color ComputeColor(Topography topography, object args = null) 
        {
            switch (topography)
            {
                case Topography.Water:
                    return ColorTranslator.FromHtml(PALETTE_WTR);
                case Topography.Beach:
                    return ColorTranslator.FromHtml(PALETTE_BCH);
                case Topography.Grass:
                    return ColorTranslator.FromHtml(PALETTE_GRS);
                case Topography.Hills:
                    return ColorTranslator.FromHtml(PALETTE_HIL);
                case Topography.Mountains:
                    return ColorTranslator.FromHtml(PALETTE_MNT);
                case Topography.Ice:
                    return ColorTranslator.FromHtml(PALETTE_ICE);
                default:
                    return ColorTranslator.FromHtml(PALETTE_WTR);
            };
        }
    }
}
