using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories;

public class GenericRepository<T>(AppDbContext context) where T : class
{
    private readonly AppDbContext _context = context;
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

    public async Task RemoveAsync(T data)
    {
        _set.Remove(data);
        await _context.SaveChangesAsync();
    }
}
