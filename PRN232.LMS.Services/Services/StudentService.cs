using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class StudentService(GenericRepository<Student> repo, IMapper mapper): CrudServiceBase<Student, StudentResponse, CreateStudentRequest, UpdateStudentRequest>(repo, mapper)
{
    protected override Expression<Func<Student,bool>> KeyPredicate(int id)
        => x => x.StudentId == id;

    protected override IReadOnlyDictionary<string, Func<IQueryable<Student>, IQueryable<Student>>> ExpandMap
        => new Dictionary<string, Func<IQueryable<Student>, IQueryable<Student>>>
        {
            ["enrollments"] = q => q.Include(x => x.Enrollments)
        };

    protected override IQueryable<Student> BuildIdQuery()
        => _repo.Query()
            .Include(x => x.Enrollments);
}