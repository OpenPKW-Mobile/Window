using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Utils
{
    public static class JsonHelper
    {
        /// <summary>
        /// Serializacja obiektu do formatu JSON.
        /// </summary>
        /// <typeparam name="T">Typ obiektu</typeparam>
        /// <param name="instance">Obiekt do serializacji</param>
        /// <returns>JSON</returns>
        public static string ToJson<T>(T instance)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var tempStream = new MemoryStream())
            {
                serializer.WriteObject(tempStream, instance);
                return Encoding.UTF8.GetString(tempStream.ToArray(), 0, tempStream.ToArray().Length);
            }
        }

        /// <summary>
        /// Deserializacja JSON do postaci obiektu.
        /// </summary>
        /// <typeparam name="T">Typ obiektu</typeparam>
        /// <param name="json">JSON</param>
        /// <returns>Obiekt</returns>
        public static T FromJson<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var tempStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T)serializer.ReadObject(tempStream);
            }
        }
    }
}
