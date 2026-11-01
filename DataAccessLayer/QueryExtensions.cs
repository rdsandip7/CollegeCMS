using System.Linq.Expressions;
using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

internal static class QueryExtensions
{
    /// <summary>
    /// Orders by the column named in <paramref name="query"/>, falling back to the first
    /// entry in <paramref name="sortColumns"/> when the name is missing or unknown.
    /// </summary>
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> source,
        ListQuery query,
        IReadOnlyDictionary<string, Expression<Func<T, object?>>> sortColumns)
    {
        var key = sortColumns.TryGetValue(query.SortField, out var selected) ? selected : sortColumns.First().Value;
        return query.Descending ? source.OrderByDescending(key) : source.OrderBy(key);
    }

    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, ListQuery query)
    {
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var total = await source.CountAsync();
        var pageCount = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
        var page = Math.Clamp(query.Page, 1, pageCount);

        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedResult<T> { Items = items, PageNumber = page, PageSize = pageSize, TotalCount = total };
    }
}
