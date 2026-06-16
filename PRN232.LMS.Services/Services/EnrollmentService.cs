using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Extensions;
using PRN232.LMS.Services.Models.Business;
using PRN232.LMS.Services.Models.Request;
using PRN232.LMS.Services.Models.Response;

namespace PRN232.LMS.Services.Services;

public class EnrollmentService(GenericRepository<Enrollment> repo, IMapper mapper)
    : CrudServiceBase<Enrollment, EnrollmentBusiness, EnrollmentResponse, CreateEnrollmentRequest, UpdateEnrollmentRequest>(repo, mapper)
{
    protected override Expression<Func<Enrollment, bool>> KeyPredicate(int id)
        => x => x.EnrollmentId == id;

    protected override IReadOnlyDictionary<string, Func<IQueryable<Enrollment>, IQueryable<Enrollment>>> ExpandMap
        => new Dictionary<string, Func<IQueryable<Enrollment>, IQueryable<Enrollment>>>
        {
            ["student"] = q => q.Include(x => x.Student),
            ["course"]  = q => q.Include(x => x.Course)
        };

    protected override IQueryable<Enrollment> BuildIdQuery()
        => _repo.Query()
            .Include(x => x.Student)
            .Include(x => x.Course);

    // Nested resource: students enrolled in a course
    public async Task<PagedResult<object>> GetStudentsByCourseAsync(int courseId, QueryParameters param)
    {
        var total = await _repo.Query().CountAsync(e => e.CourseId == courseId);
        var students = await _repo.Query()
            .Where(e => e.CourseId == courseId)
            .Include(e => e.Student)
            .ApplyPaging(param.Page, param.Size)
            .Select(e => e.Student)
            .ToListAsync();

        var businesses = _mapper.Map<List<StudentBusiness>>(students);
        var responses  = _mapper.Map<List<BasicStudentResponse>>(businesses);
        return responses.ToPagedResult(total, param);
    }
}