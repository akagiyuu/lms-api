using AutoMapper;
using Course.Service.Common;
using Course.Service.Models;
using Course.Service.Models.Request;
using Course.Service.Models.Response;
using Microsoft.EntityFrameworkCore;
namespace Course.Service.Services;
public class SemesterService(AppDbContext db, IMapper mapper)
{
    public async Task<PagedResult<object>> GetAllAsync(QueryParameters param)
    {
        var query = db.Semesters.AsQueryable();
        var total = await query.CountAsync();
        var page  = param.Page < 1 ? 1 : param.Page;
        var size  = param.Size < 1 ? 10 : param.Size;
        var data  = await query.Skip((page - 1) * size).Take(size).ToListAsync();
        return new PagedResult<object>
        {
            Page = page, Size = size, TotalItems = total,
            TotalPages = (int)Math.Ceiling(total / (double)size),
            Items = mapper.Map<List<SemesterResponse>>(data).Cast<object>().ToList()
        };
    }
    public async Task<SemesterResponse?> GetByIdAsync(int id)
    {
        var e = await db.Semesters.FindAsync(id);
        return e is null ? null : mapper.Map<SemesterResponse>(e);
    }
    public async Task<SemesterResponse> CreateAsync(CreateSemesterRequest req)
    {
        var e = mapper.Map<SemesterEntity>(req);
        db.Semesters.Add(e); await db.SaveChangesAsync();
        return mapper.Map<SemesterResponse>(e);
    }
    public async Task<bool> PatchAsync(int id, UpdateSemesterRequest req)
    {
        var e = await db.Semesters.FindAsync(id);
        if (e is null) return false;
        mapper.Map(req, e); await db.SaveChangesAsync(); return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var e = await db.Semesters.FindAsync(id);
        if (e is null) return false;
        db.Semesters.Remove(e); await db.SaveChangesAsync(); return true;
    }
}
