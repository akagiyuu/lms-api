using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class EnrollmentService(GenericRepository<Enrollment> repo, IMapper mapper) : CrudServiceBase<Enrollment, EnrollmentResponse, CreateEnrollmentRequest, UpdateEnrollmentRequest>(repo, mapper)
{
    protected override Expression<Func<Enrollment, bool>> KeyPredicate(int id)
        => x => x.EnrollmentId == id;

    protected override IReadOnlyDictionary<string, Func<IQueryable<Enrollment>, IQueryable<Enrollment>>> ExpandMap
        => new Dictionary<string, Func<IQueryable<Enrollment>, IQueryable<Enrollment>>>
        {
            ["student"] = q => q.Include(x => x.Student),
            ["course"] = q => q.Include(x => x.Course)
        };

    protected override IQueryable<Enrollment> BuildIdQuery()
        => _repo.Query()
            .Include(x => x.Student)
            .Include(x => x.Course);
}