using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace COMMONS
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            string stringValue = JsonConvert.SerializeObject(value);
            var valueByte = Encoding.Unicode.GetBytes(stringValue);
            session.Set(key, valueByte);

            //session.SetObject(key,JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            byte[] valueByte;
            var result = session.TryGetValue(key, out valueByte);
            if (!result || valueByte.Length == 0) return default(T);

            var stringValue = Encoding.Unicode.GetString(valueByte);
            var value = JsonConvert.DeserializeObject<T>(stringValue);
            return value;
        }

        //public static T GetObject<T>(this ISession session, string key)
        //{
        //    var value = session.GetString(key);
        //    return value == null ? default(T) :
        //                          JsonConvert.DeserializeObject<T>(value);
        //}
    }
}
