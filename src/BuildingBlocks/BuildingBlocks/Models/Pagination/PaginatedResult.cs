namespace BuildingBlocks.Models.Pagination;
public class PaginatedResult<TEntity>
    (int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    where TEntity : class
{
    public IEnumerable<TEntity> Data { get; } = data;
    public int PageIndex { get; } = pageIndex;
    public int TotalPages => (int)Math.Ceiling((double)Count / PageSize);
    public long Count { get; } = count;
    public int PageSize { get; } = pageSize;

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
