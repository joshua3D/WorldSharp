using System.IO;
using System.IO.Compression;
using System.Text.Json;
using WorldSharpDLL.Elements;

namespace WorldSharpDLL.IO
{
    public static class Serialization 
    {
        public static string ToJson(this World world) 
        {
            string json;
            json = JsonSerializer.Serialize(world);
            return json;
        }

        public static byte[] ToJsonUTF8(this World world, bool writeIndented = false)
        {
            byte[] jsonUtf8Bytes;
            var options = new JsonSerializerOptions
            {
                WriteIndented = writeIndented
            };
            jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes<World>(world, options);
            return jsonUtf8Bytes;
        }

        public static byte[] ToJsonUTF8GZipped(this World world, bool writeIndented = false)
        {
            byte[] jsonUtf8Bytes = ToJsonUTF8(world, writeIndented);
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        public static World FromJsonUTF8Gzipped(byte[] jsonGzipped)
        {
            using (var compressedStream = new MemoryStream(jsonGzipped))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                var jsonUtf8Bytes = resultStream.ToArray();
                var utf8Reader = new Utf8JsonReader(jsonUtf8Bytes);
                World world = JsonSerializer.Deserialize<World>(ref utf8Reader);
                return world;
            }
        }
    }
}
