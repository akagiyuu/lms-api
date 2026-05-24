using System.Linq.Expressions;
using AutoMapper;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class SubjectService(GenericRepository<Subject> repo, IMapper mapper) : CrudServiceBase<Subject, SubjectResponse, CreateSubjectRequest, UpdateSubjectRequest>(repo, mapper)
{
    protected override Expression<Func<Subject, bool>> KeyPredicate(int id)
       => x => x.SubjectId == id;
}