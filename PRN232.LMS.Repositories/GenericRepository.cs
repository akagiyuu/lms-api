using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PRN232.LMS.Repositories;

public class GenericRepository<T, TContext>(TContext context) where T : class where TContext : DbContext
{
    private readonly TContext _context = context;
    private readonly DbSet<T> _set = context.Set<T>();

    public IQueryable<T> Query() => _set.AsQueryable();

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        => _set.FirstOrDefaultAsync(predicate);

    public async Task AddAsync(T data)
    {
        await _set.AddAsync(data);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T data)
    {
        _set.Update(data);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T data)
    {
        _set.Remove(data);
        await _context.SaveChangesAsync();
    }
}
