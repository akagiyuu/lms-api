using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Student.Service.Common;
using Student.Service.Models;
using Student.Service.Models.Request;
using Student.Service.Models.Response;
namespace Student.Service.Services;
public class StudentService(AppDbContext db, IMapper mapper)
{
    public async Task<PagedResult<object>> GetAllAsync(QueryParameters param)
    {
        var query = db.Students.AsQueryable();
        if (!string.IsNullOrWhiteSpace(param.Search))
            query = query.Where(s => s.FullName.ToLower().Contains(param.Search.ToLower())
                                  || s.Email.ToLower().Contains(param.Search.ToLower()));
        var total = await query.CountAsync();
        var page  = param.Page < 1 ? 1 : param.Page;
        var size  = param.Size < 1 ? 10 : param.Size;
        var data  = await query.Skip((page - 1) * size).Take(size).ToListAsync();
        var items = mapper.Map<List<StudentResponse>>(data);
        return new PagedResult<object>
        {
            Page = page, Size = size, TotalItems = total,
            TotalPages = (int)Math.Ceiling(total / (double)size),
            Items = items.Cast<object>().ToList()
        };
    }
    public async Task<StudentResponse?> GetByIdAsync(int id)
    {
        var s = await db.Students.FindAsync(id);
        return s is null ? null : mapper.Map<StudentResponse>(s);
    }
    public async Task<bool> ExistsAsync(int id)
        => await db.Students.AnyAsync(s => s.StudentId == id);
    public async Task<StudentResponse> CreateAsync(CreateStudentRequest request)
    {
        var student = new StudentEntity
        {
            FullName    = request.FullName,
            Email       = request.Email,
            DateOfBirth = request.DateOfBirth!.Value.UtcDateTime
        };
        db.Students.Add(student);
        await db.SaveChangesAsync();
        return mapper.Map<StudentResponse>(student);
    }
    public async Task<bool> PatchAsync(int id, UpdateStudentRequest request)
    {
        var student = await db.Students.FindAsync(id);
        if (student is null) return false;
        if (request.FullName    is not null) student.FullName    = request.FullName;
        if (request.Email       is not null) student.Email       = request.Email;
        if (request.DateOfBirth is not null) student.DateOfBirth = request.DateOfBirth.Value.UtcDateTime;
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var student = await db.Students.FindAsync(id);
        if (student is null) return false;
        db.Students.Remove(student);
        await db.SaveChangesAsync();
        return true;
    }
}
