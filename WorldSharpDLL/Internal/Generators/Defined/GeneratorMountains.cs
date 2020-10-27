using System.Drawing;
using static WorldSharpDLL.Elements.Tile;

namespace WorldSharpDLL.Generators
{
    public class GeneratorMountains : Generator
    {
        protected override Topography ComputeTypography(float value, object args = null)
        {
            if (value <= 0.65)
            {
                return Topography.Grass;
            }
            else if (value <= 0.80f)
            {
                return Topography.Hills;
            }
            else if (value <= 0.94f)
            {
                return Topography.Mountains;
            }
            else if (value > 0.94f)
            {
                return Topography.Ice;
            }
            return Topography.Water;
        }

        protected override Color ComputeColor(Topography topography, object args = null)
        {
            switch (topography)
            {
                case Topography.Grass:
                    return ColorTranslator.FromHtml(PALETTE_GRS);
                case Topography.Hills:
                    return ColorTranslator.FromHtml(PALETTE_HIL);
                case Topography.Mountains:
                    return ColorTranslator.FromHtml(PALETTE_MNT);
                case Topography.Ice:
                    return ColorTranslator.FromHtml(PALETTE_ICE);
                default:
                    return ColorTranslator.FromHtml(PALETTE_GRS);
            };
        }
    }
}
