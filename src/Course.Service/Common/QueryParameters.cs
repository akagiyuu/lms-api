namespace Course.Service.Common;
public class QueryParameters
{
    public string? Search { get; set; }
    public string? Sort   { get; set; }
    public int     Page   { get; set; } = 1;
    public int     Size   { get; set; } = 10;
    public string? Fields { get; set; }
}
public class PagedResult<T>
{
    public int     Page       { get; set; }
    public int     Size       { get; set; }
    public int     TotalItems { get; set; }
    public int     TotalPages { get; set; }
    public List<T> Items      { get; set; } = [];
}
