using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models.Business;
using PRN232.LMS.Services.Models.Request;
using PRN232.LMS.Services.Models.Response;

namespace PRN232.LMS.Services.Services;

public class SemesterService(GenericRepository<Semester> repo, IMapper mapper)
    : CrudServiceBase<Semester, SemesterBusiness, SemesterResponse, CreateSemesterRequest, UpdateSemesterRequest>(repo, mapper)
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