namespace WorldSharpDLL.Elements
{
    public class Tile
    {
        public enum Topography
        {
            Undefined,
            Water,
            Beach,
            Grass,
            Hills,
            Mountains,
            Ice
        }

        public Topography Terrain { get; set; }
        public float Elevation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Tile() { }
    }
}
