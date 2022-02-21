using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Helpers
{
    public static class SessionExtetionHelper
    {
        public static void SetObject<T>(this ISession session,string key,T value)
        {
            string json = JsonConvert.SerializeObject(value);
            session.SetString(key,json);           
        }
        public static T GetObject<T>(this ISession session,string key)
        {
            string json = session.GetString(key);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
