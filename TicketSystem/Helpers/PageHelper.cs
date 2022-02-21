using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Helpers
{
    public static class PageHelper
    {
        public static int GetPages<T>(this IEnumerable<T> source, int page_count)
        {
            // source 為0時會算出count=0
            int count= source.Count() % page_count == 0 ? source.Count() / page_count : source.Count() / page_count + 1;
            return count == 0 ? 1 : count;
        }
        public static IEnumerable<T> GetPages<T>(this IEnumerable<T> source, int page_count, int index)
        {
            return source.Skip((index - 1) * page_count).Take(page_count);
        }

    }
}
