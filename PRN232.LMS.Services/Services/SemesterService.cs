using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Extensions;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class SemesterService(GenericRepository<Semester> repo, IMapper mapper)
    : CrudServiceBase<Semester, SemesterResponse, CreateSemesterRequest, UpdateSemesterRequest>(repo, mapper)
{
    protected override Expression<Func<Semester, bool>> KeyPredicate(int id)
        => x => x.SemesterId == id;

    protected override IReadOnlyDictionary<string, Func<IQueryable<Semester>, IQueryable<Semester>>> ExpandMap
        => new Dictionary<string, Func<IQueryable<Semester>, IQueryable<Semester>>>
        {
            ["courses"] = q => q.Include(x => x.Courses)
        };

    protected override IQueryable<Semester> BuildIdQuery()
        => _repo.Query().Include(x => x.Courses);
}