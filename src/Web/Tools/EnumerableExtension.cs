using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Web.Tools
{
    public static class EnumerableExtension
    {
        public static async Task<IList<T>> ToListAsync<T>(this IEnumerable<T> list) 
        {
            if (list is IQueryable<T> query) 
            {
                return await EntityFrameworkQueryableExtensions.ToListAsync(query);
            }

            return list.ToList();
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IEnumerable<T> list)
        {
            if (list is IQueryable<T> query)
            {
                return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query);
            }

            return list.FirstOrDefault();
        }
    }
}
