// -----------------------------------------------------------------------
// <copyright file="JsonSerialization.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.Serialization
{
    using System.IO;
    using Newtonsoft.Json;

    /// <summary>
    /// JSON serialization class.
    /// </summary>
    public class JsonSerialization
    {
        /// <summary>
        /// JSON serialize settings
        /// </summary>
        private static readonly JsonSerializerSettings JSONSettings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore, TypeNameHandling = TypeNameHandling.All, ObjectCreationHandling = ObjectCreationHandling.Replace };

        /// <summary>
        /// Serializes the passed in object to a JSON string.
        /// </summary>
        /// <param name="input">Object to serialize.</param>
        /// <returns>Serializes data.</returns>
        public static string SerializeObjectToString(object input)
        {
            return JsonConvert.SerializeObject(input, Formatting.Indented, JSONSettings);
        }

        /// <summary>
        /// Serializes the passed in object to a JSON string and save to the passed in file.
        /// </summary>
        /// <param name="input">Object to serialize.</param>
        /// <param name="filename">Filename to serialize the object to.</param>
        public static void SerializeObjectToFile(object input, string filename)
        {
            using (TextWriter writer = File.CreateText(filename))
            {
                writer.Write(SerializeObjectToString(input));
            }
        }

        /// <summary>
        /// Deserialize the passed in string into the requested object type.
        /// </summary>
        /// <typeparam name="T">Deserialized output type.</typeparam>
        /// <param name="input">Input JSON to deserialize.</param>
        /// <returns>Deserialized object instance.</returns>
        public static T DeserializeObjectFromString<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, JSONSettings);
        }

        /// <summary>
        /// Deserialize the passed in file into the requested object type.
        /// </summary>
        /// <typeparam name="T">Deserialized output type.</typeparam>
        /// <param name="filename">Input file to deserialize.</param>
        /// <returns>Deserialized object instance.</returns>
        public static T DeserializeObjectFromFile<T>(string filename)
        {
            FileInfo info = new FileInfo(filename);
            if (info.Exists)
            {
                using (TextReader reader = File.OpenText(filename))
                {
                    return DeserializeObjectFromString<T>(reader.ReadToEnd());
                }
            }

            throw new FileNotFoundException(string.Format("File \"{0}\"  was not found.", filename));
        }
    }
}