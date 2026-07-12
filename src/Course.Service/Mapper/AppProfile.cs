using AutoMapper;
using Course.Service.Models;
using Course.Service.Models.Request;
using Course.Service.Models.Response;
namespace Course.Service.Mapper;
public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<DateTime, DateTimeOffset>().ConvertUsing(d => new DateTimeOffset(d, TimeSpan.Zero));
        CreateMap<DateTimeOffset, DateTime>().ConvertUsing(d => d.UtcDateTime);
        CreateMap<DateTimeOffset?, DateTime?>().ConvertUsing(d => d.HasValue ? d.Value.UtcDateTime : null);
        CreateMap<DateTime?, DateTimeOffset?>().ConvertUsing(d => d.HasValue ? new DateTimeOffset(d.Value, TimeSpan.Zero) : null);
        CreateMap<DateTimeOffset?, DateTime>().ConvertUsing(d => d.HasValue ? d.Value.UtcDateTime : default);
        // Semester
        CreateMap<SemesterEntity, SemesterResponse>();
        CreateMap<CreateSemesterRequest, SemesterEntity>();
        CreateMap<UpdateSemesterRequest, SemesterEntity>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        // Subject
        CreateMap<SubjectEntity, SubjectResponse>();
        CreateMap<CreateSubjectRequest, SubjectEntity>();
        CreateMap<UpdateSubjectRequest, SubjectEntity>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        // Course
        CreateMap<CourseEntity, CourseResponse>();
        CreateMap<CreateCourseRequest, CourseEntity>();
        CreateMap<UpdateCourseRequest, CourseEntity>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        // Enrollment
        CreateMap<EnrollmentEntity, EnrollmentResponse>();
        CreateMap<CreateEnrollmentRequest, EnrollmentEntity>();
        CreateMap<UpdateEnrollmentRequest, EnrollmentEntity>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
