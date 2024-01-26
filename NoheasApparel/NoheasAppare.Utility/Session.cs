using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoheasApparel.Utility
{
    public static class Session
    {
        public static void SetOBJ(this ISession session, string key, object val)
        {
            session.SetString(key, JsonConvert.SerializeObject(val));
        }

        public static T GetOBJ<T>(this ISession session, string key)
        {
            var val = session.GetString(key);
            return val == null ? default(T) : JsonConvert.DeserializeObject<T>(val);
        }
    }
}
