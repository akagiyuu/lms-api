using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models.Business;
using PRN232.LMS.Services.Models.Request;
using PRN232.LMS.Services.Models.Response;

namespace PRN232.LMS.Services.Services;

public class CourseService(GenericRepository<Course> repo, IMapper mapper)
    : CrudServiceBase<Course, CourseBusiness, CourseResponse, CreateCourseRequest, UpdateCourseRequest>(repo, mapper)
{
    protected override Expression<Func<Course, bool>> KeyPredicate(int id)
        => x => x.CourseId == id;

    protected override IReadOnlyDictionary<string, Func<IQueryable<Course>, IQueryable<Course>>> ExpandMap
        => new Dictionary<string, Func<IQueryable<Course>, IQueryable<Course>>>
        {
            ["semester"] = q => q.Include(x => x.Semester),
            ["enrollments"] = q => q.Include(x => x.Enrollments).ThenInclude(e => e.Student),
        };

    protected override IQueryable<Course> BuildIdQuery()
        => _repo.Query()
            .Include(x => x.Semester)
            .Include(x => x.Enrollments)
                .ThenInclude(e => e.Student);
}