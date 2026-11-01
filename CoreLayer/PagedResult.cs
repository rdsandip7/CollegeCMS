namespace CoreLayer;

/// <summary>Paging details without the item type, so one pager view can render any list.</summary>
public interface IPagedResult
{
    int PageNumber { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int PageCount { get; }
    bool HasPrevious { get; }
    bool HasNext { get; }
}

/// <summary>One page of a sorted, filtered list, plus what the UI needs to render a pager.</summary>
public class PagedResult<T> : IPagedResult
{
    public required IReadOnlyList<T> Items { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }

    public int PageCount => TotalCount == 0 ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < PageCount;
}

/// <summary>Search, sort and paging options shared by every list page.</summary>
public record ListQuery(string? Search = null, string? Sort = null, int Page = 1, int PageSize = 10)
{
    public bool Descending => Sort?.EndsWith("_desc", StringComparison.Ordinal) == true;
    public string SortField => Sort is null ? string.Empty : Sort.Replace("_desc", string.Empty);
}
