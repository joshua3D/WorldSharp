namespace WorldSharpDLL.Elements
{
    public class World
    {
        public float Version { get { return Config.Version; } set {} }
        public Tile[][] Tiles { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public World() { }
    }
}
