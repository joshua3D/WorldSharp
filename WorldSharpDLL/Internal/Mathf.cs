using System.Drawing;

namespace WorldSharpDLL
{
    public static class Mathf
    {
        public static float Lerp(float a, float b, float t)
        {
            return (a * (1.0f - t)) + (b * t);
        }

        public static Color LerpColor(Color start, Color next, float t)
        {
            int r, g, b, a;

            r = (int)Lerp(start.R, next.R, t);
            g = (int)Lerp(start.G, next.G, t);
            b = (int)Lerp(start.B, next.B, t);
            a = (int)Lerp(start.A, next.A, t);

            return Color.FromArgb(a, r, g, b);
        }

        public static decimal Map(this decimal value, decimal fromSource, decimal toSource, decimal fromTarget, decimal toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static float ClampRange(float floor, float ceiling, ref float num)
        {
            num = num < floor ? floor : num;
            num = num > ceiling ? ceiling : num;
            return num;
        }
    }
}
