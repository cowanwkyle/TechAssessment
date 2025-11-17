using System;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Web.Extensions;

public class PagedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public PagedList()
    {
        PageIndex = 1;
        TotalPages = 1;
    }
    public PagedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PagedList<T>> CreateAsync(List<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        var result = new PagedList<T>(items, count, pageIndex, pageSize);
        return await Task.FromResult(result);
    }
}
