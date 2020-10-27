using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WorldSharpDLL.IO
{
    public static class FileHandling
    {
        const string JPEG_CODEC = "image/jpeg";
        const string DEFAULT_FILENAME_IMG = "img";
        const string DEFAULT_FILENAME_JSON = "world";
        const string EXT_BMP = ".bmp";
        const string EXT_GIF = ".gif";
        const string EXT_JPG = ".jpeg";
        const string EXT_JSON = ".json";
        const string EXT_GZIP = ".gz";

        /// <summary>
        /// Will try to write the provided json string as a .json
        /// </summary>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        /// <param name="bitmap">The json string being written</param>
        /// <param name="directory">The Directory where the file will be written, defaults to CurrentDirectory</param>
        /// <param name="fileName">The file name for the json, defaults to 'world'</param>
        public static bool TryWriteJson(string json, string directory = "", string fileName = DEFAULT_FILENAME_JSON) 
        {
            if (json.Length < 1)
                return false;

            if (!fileName.EndsWith(EXT_JSON))
            {
                fileName += EXT_JSON;
            }

            if (string.IsNullOrEmpty(directory)) 
            {
                directory = Directory.GetCurrentDirectory() + @"\";
            }

            var path = directory + fileName;

            if (Directory.Exists(directory)) 
            {
                try
                {
                    File.WriteAllText(path, json);
                }
                catch { return false; }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Will try to write the provided jsonutf8 byte array as a .json
        /// </summary>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        /// <param name="jsonUtf8">The byte array containing the serialized json data</param>
        /// <param name="directory">The Directory where the file will be written, defaults to CurrentDirectory</param>
        /// <param name="fileName">The file name for the json, defaults to 'world'</param>
        public static bool TryWriteJsonUTF8(byte[] jsonUtf8, string directory = "", string fileName = DEFAULT_FILENAME_JSON)
        {
            if (jsonUtf8 == null)
                return false;

            if (!fileName.EndsWith(EXT_JSON))
            {
                fileName += EXT_JSON;
            }

            if (string.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory() + @"\";
            }

            var path = directory + fileName;

            if (Directory.Exists(directory))
            {
                try
                {
                    File.WriteAllBytes(path, jsonUtf8);
                }
                catch { return false; }

                return true;
            }

            return false;
        }

        public static bool TryWriteJsonUTF8GZipped(byte[] jsonUtf8GZipped, string directory = "", string fileName = DEFAULT_FILENAME_JSON)
        {
            if (jsonUtf8GZipped == null)
                return false;

            var ext = $"{EXT_JSON}{EXT_GZIP}";

            if (!fileName.EndsWith(ext))
            {
                fileName += ext;
            }

            if (string.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory() + @"\";
            }

            var path = directory + fileName;

            if (Directory.Exists(directory))
            {
                try
                {
                    File.WriteAllBytes(path, jsonUtf8GZipped);
                }
                catch { return false; }

                return true;
            }

            return false;
        }


        /// <summary>
        /// Will try to write the provided Bitmap as a .bmp
        /// </summary>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        /// <param name="bitmap">The Bitmap you wish to write</param>
        /// <param name="directory">The Directory where the Bitmap will be written, defaults to CurrentDirectory</param>
        /// <param name="fileName">The file name for the Bitmap, defaults to 'img'</param>
        public static bool TryWriteImageToBitmap(Bitmap bitmap, string directory = "", string fileName = DEFAULT_FILENAME_IMG)
        {
            if (bitmap == null)
                return false;

            fileName = fileName.Length > 0 ? fileName : DEFAULT_FILENAME_IMG;

            if (!fileName.EndsWith(EXT_BMP))
            {
                fileName += EXT_BMP;
            }

            if (string.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory() + @"\";
            }

            var path = directory + fileName;

            if (Directory.Exists(directory))
            {
                try
                {
                    bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                catch { return false; }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Will try to write the provided Bitmap as a .jpg
        /// </summary>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        /// <param name="bitmap">The Bitmap you wish to write</param>
        /// <param name="directory">The Directory where the Bitmap will be written, defaults to CurrentDirectory</param>
        /// <param name="fileName">The file name for the Bitmap, defaults to 'img'</param>
        /// <param name="quality">The level of quality for the encoded jpeg. This value is clamped to 1-100</param>
        public static bool TryWriteImageToJpeg(Bitmap bitmap, string directory = "", string fileName = DEFAULT_FILENAME_IMG, decimal quality = 100L)
        {
            if (bitmap == null)
                return false;

            fileName = fileName.Length > 0 ? fileName : DEFAULT_FILENAME_IMG;

            if (!fileName.EndsWith(EXT_JPG))
            {
                fileName += EXT_JPG;
            }

            if (string.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory() + @"\";
            }

            var path = directory + fileName;

            if (Directory.Exists(directory))
            {
                const decimal MAX_QUALITY = 100L;
                const decimal MIN_QUALITY = 1L;

                quality = quality > MAX_QUALITY ? MAX_QUALITY : quality;
                quality = quality < MIN_QUALITY ? MIN_QUALITY : quality;

                ImageCodecInfo myImageCodecInfo = GetMimeType(JPEG_CODEC);
                EncoderParameter myEncoderParameter = new EncoderParameter(Encoder.Quality, 100L);
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = myEncoderParameter;

                try
                {
                    bitmap.Save(path, myImageCodecInfo, myEncoderParameters);
                }
                catch { return false; }

                return true;
            }

            return false;
        }

        private static ImageCodecInfo GetMimeType(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
