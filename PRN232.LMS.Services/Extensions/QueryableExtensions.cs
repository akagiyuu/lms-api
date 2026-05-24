using System.Dynamic;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search)) return query;

        var stringProps = typeof(T)
            .GetProperties()
            .Where(x => x.PropertyType == typeof(string))
            .Select(x => $"{x.Name}.ToLower().Contains(@0)")
            .ToList();
        if (stringProps.Count == 0) return query;

        return query.Where(string.Join(" OR ", stringProps), search.ToLower());
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort)) return query;

        var orders = sort
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.StartsWith('-') ? $"{x[1..]} descending" : x);

        return query.OrderBy(string.Join(", ", orders));
    }

    public static IQueryable<T> ApplyExpand<T>(
        this IQueryable<T> query,
        string? expand,
        IReadOnlyDictionary<string, Func<IQueryable<T>, IQueryable<T>>> expandMap)
    {
        if (string.IsNullOrWhiteSpace(expand)) return query;

        foreach (var token in expand.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (expandMap.TryGetValue(token, out var includeFn))
                query = includeFn(query);
        }

        return query;
    }

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int size)
    {
        page = page < 1 ? 1 : page;
        size = size < 1 ? 10 : size;
        return query.Skip((page - 1) * size).Take(size);
    }

    public static Common.PagedResult<object> ToPagedResult<T>(
        this IEnumerable<T> source,
        int totalItems,
        QueryParameters param)
    {
        var page = param.Page < 1 ? 1 : param.Page;
        var size = param.Size < 1 ? 10 : param.Size;

        List<object> items;

        if (string.IsNullOrWhiteSpace(param.Fields))
        {
            items = source.Cast<object>().ToList();
        }
        else
        {
            var fields = param.Fields
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            items = source.Select(item =>
            {
                IDictionary<string, object?> obj = new ExpandoObject();

                foreach (var prop in typeof(T).GetProperties())
                {
                    if (fields.Contains(prop.Name))
                        obj[prop.Name] = prop.GetValue(item);
                }

                return (object)obj;
            }).ToList();
        }

        return new Common.PagedResult<object>
        {
            Page = page,
            Size = size,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)size),
            Items = items
        };
    }
}
