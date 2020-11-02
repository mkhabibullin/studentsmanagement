using SM.Application.Common.Models;
using System.Linq;

namespace SM.Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, PagingQuery page)
        {
            var pageNum = page.Page.HasValue ? page.Page.Value : 0;
            var pageSize = page.PageSize.HasValue ? page.PageSize.Value : 0;

            return query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
