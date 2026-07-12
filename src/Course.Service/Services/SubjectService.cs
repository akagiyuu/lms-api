using AutoMapper;
using Course.Service.Common;
using Course.Service.Models;
using Course.Service.Models.Request;
using Course.Service.Models.Response;
using Microsoft.EntityFrameworkCore;
namespace Course.Service.Services;
public class SubjectService(AppDbContext db, IMapper mapper)
{
    public async Task<PagedResult<object>> GetAllAsync(QueryParameters param)
    {
        var query = db.Subjects.AsQueryable();
        var total = await query.CountAsync();
        var page  = param.Page < 1 ? 1 : param.Page;
        var size  = param.Size < 1 ? 10 : param.Size;
        var data  = await query.Skip((page - 1) * size).Take(size).ToListAsync();
        return new PagedResult<object>
        {
            Page = page, Size = size, TotalItems = total,
            TotalPages = (int)Math.Ceiling(total / (double)size),
            Items = mapper.Map<List<SubjectResponse>>(data).Cast<object>().ToList()
        };
    }
    public async Task<SubjectResponse?> GetByIdAsync(int id)
    {
        var e = await db.Subjects.FindAsync(id);
        return e is null ? null : mapper.Map<SubjectResponse>(e);
    }
    public async Task<SubjectResponse> CreateAsync(CreateSubjectRequest req)
    {
        var e = mapper.Map<SubjectEntity>(req);
        db.Subjects.Add(e); await db.SaveChangesAsync();
        return mapper.Map<SubjectResponse>(e);
    }
    public async Task<bool> PatchAsync(int id, UpdateSubjectRequest req)
    {
        var e = await db.Subjects.FindAsync(id);
        if (e is null) return false;
        mapper.Map(req, e); await db.SaveChangesAsync(); return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var e = await db.Subjects.FindAsync(id);
        if (e is null) return false;
        db.Subjects.Remove(e); await db.SaveChangesAsync(); return true;
    }
}
