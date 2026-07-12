using AutoMapper;
using Course.Service.Common;
using Course.Service.Models;
using Course.Service.Models.Request;
using Course.Service.Models.Response;
using Microsoft.EntityFrameworkCore;
using Student.Service.Grpc;
namespace Course.Service.Services;
public class EnrollmentService(AppDbContext db, IMapper mapper, StudentGrpc.StudentGrpcClient studentClient)
{
    public async Task<PagedResult<object>> GetAllAsync(QueryParameters param)
    {
        var query = db.Enrollments.AsQueryable();
        var total = await query.CountAsync();
        var page  = param.Page < 1 ? 1 : param.Page;
        var size  = param.Size < 1 ? 10 : param.Size;
        var data  = await query.Skip((page - 1) * size).Take(size).ToListAsync();
        return new PagedResult<object>
        {
            Page = page, Size = size, TotalItems = total,
            TotalPages = (int)Math.Ceiling(total / (double)size),
            Items = mapper.Map<List<EnrollmentResponse>>(data).Cast<object>().ToList()
        };
    }
    public async Task<EnrollmentResponse?> GetByIdAsync(int id)
    {
        var e = await db.Enrollments.FindAsync(id);
        return e is null ? null : mapper.Map<EnrollmentResponse>(e);
    }
    public async Task<(EnrollmentResponse? result, string? error)> CreateAsync(CreateEnrollmentRequest req)
    {
        // Verify student exists via gRPC
        var reply = await studentClient.StudentExistsAsync(new StudentRequest { StudentId = req.StudentId!.Value });
        if (!reply.Exists)
            return (null, $"Student with id {req.StudentId} not found.");
        var e = mapper.Map<EnrollmentEntity>(req);
        db.Enrollments.Add(e);
        await db.SaveChangesAsync();
        return (mapper.Map<EnrollmentResponse>(e), null);
    }
    public async Task<bool> PatchAsync(int id, UpdateEnrollmentRequest req)
    {
        var e = await db.Enrollments.FindAsync(id);
        if (e is null) return false;
        mapper.Map(req, e); await db.SaveChangesAsync(); return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var e = await db.Enrollments.FindAsync(id);
        if (e is null) return false;
        db.Enrollments.Remove(e); await db.SaveChangesAsync(); return true;
    }
}
