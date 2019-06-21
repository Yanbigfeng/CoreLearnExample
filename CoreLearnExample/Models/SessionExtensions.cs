using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CoreLearnExample.Models
{
    public static class SessionExtensions
    {

        //官方序列化扩展session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

        //使用流文件扩展session
        public static void SetStream<T>(this ISession session, string key, T value)
        {
            System.Runtime.Serialization.IFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            string result = string.Empty;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bf.Serialize(ms, value);
                byte[] byt = new byte[ms.Length];
                byt = ms.ToArray();
                result = System.Convert.ToBase64String(byt);
                ms.Flush();
                session.SetString(key, result);
            }
        }

        public static T GetStream<T>(this ISession session, string key)
        {
            T obj = default(T);
            var value = session.GetString(key);
            if (!string.IsNullOrEmpty(value)) {
                System.Runtime.Serialization.IFormatter bf = new BinaryFormatter();
                byte[] byt = Convert.FromBase64String(value);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byt, 0, byt.Length))
                {
                    obj = (T)bf.Deserialize(ms);
                }
            }
            return obj;
        }
    }
}
