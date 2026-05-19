namespace PRN232.LMS.Services.Common;

public class PagedResult<T>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<T> Items { get; set; } = [];
}
