using ProtoBuf;
using System.IO;
using System.IO.Compression;

namespace Falcon.Caching
{
    /// <summary>
    /// Chứa các hàm tiện ích
    /// </summary>
    public class CacheUtil
    {
        /// <summary>
        /// GZip mảng byte
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] raw)
        {
            using (var memory = new MemoryStream())
            {
                using (var compressor = new GZipStream(memory, CompressionMode.Compress))
                {
                    compressor.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        /// <summary>
        /// unzip mảng byte
        /// </summary>
        /// <param name="gzip"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (var stream = new MemoryStream(gzip))
            using (var decoder = new GZipStream(stream, CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = decoder.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        /// <summary>
        /// Serialize Sử dụng protobuf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] SerializeProtobuf<T>(T input)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, input);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserialize Sử dụng protobuf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeserializeProtobuf<T>(byte[] source)
        {
            using (var ms = new MemoryStream(source))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }

        public static bool IsGZipHeader(byte[] arr)
        {
            return arr.Length >= 2 &&
                arr[0] == 31 &&
                arr[1] == 139;
        }

        public static bool IsSnappyHeader(byte[] arr)
        {
            return arr.Length >= 11 &&
                arr[0] == 255 &&
                arr[1] == 6 &&
                arr[2] == 0 &&
                arr[3] == 0 &&
                arr[4] == 115 &&
                arr[5] == 78 &&
                arr[6] == 97 &&
                arr[7] == 80 &&
                arr[8] == 112 &&
                arr[9] == 89 &&
                arr[10] == 0;
        }
    }
}
