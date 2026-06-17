using AutoMapper;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models.Business;
using PRN232.LMS.Services.Models.Request;
using PRN232.LMS.Services.Models.Response;

namespace PRN232.LMS.Services.Mapper;

public class AppProfile : Profile
{
    public AppProfile()
    {
        // ── Global Type Converters ───────────────────────────────────────────
        CreateMap<DateTimeOffset, DateTime>().ConvertUsing(d => d.UtcDateTime);
        CreateMap<DateTimeOffset?, DateTime?>().ConvertUsing(d => d.HasValue ? d.Value.UtcDateTime : null);
        CreateMap<DateTimeOffset?, DateTime>().ConvertUsing(d => d.HasValue ? d.Value.UtcDateTime : default);
        
        CreateMap<DateTime, DateTimeOffset>().ConvertUsing(d => new DateTimeOffset(d, TimeSpan.Zero));
        CreateMap<DateTime?, DateTimeOffset?>().ConvertUsing(d => d.HasValue ? new DateTimeOffset(d.Value, TimeSpan.Zero) : null);
        CreateMap<DateTime, DateTimeOffset?>().ConvertUsing(d => new DateTimeOffset(d, TimeSpan.Zero));

        // ── Entity → Business ────────────────────────────────────────────────
        CreateMap<Semester, SemesterBusiness>();
        CreateMap<Course, CourseBusiness>();
        CreateMap<Subject, SubjectBusiness>();
        CreateMap<Student, StudentBusiness>();
        CreateMap<Enrollment, EnrollmentBusiness>();
        CreateMap<User, UserBusiness>();

        // ── Business → Response ──────────────────────────────────────────────
        CreateMap<SemesterBusiness, BasicSemesterResponse>();
        CreateMap<SemesterBusiness, SemesterResponse>();

        CreateMap<CourseBusiness, BasicCourseResponse>();
        CreateMap<CourseBusiness, CourseResponse>();

        CreateMap<SubjectBusiness, SubjectResponse>();

        CreateMap<StudentBusiness, BasicStudentResponse>();
        CreateMap<StudentBusiness, StudentResponse>();

        CreateMap<EnrollmentBusiness, BasicEnrollmentResponse>();
        CreateMap<EnrollmentBusiness, EnrollmentResponse>();

        // ── Business → Entity ────────────────────────────────────────────────
        CreateMap<SemesterBusiness, Semester>();
        CreateMap<CourseBusiness, Course>();
        CreateMap<SubjectBusiness, Subject>();
        CreateMap<StudentBusiness, Student>();
        CreateMap<EnrollmentBusiness, Enrollment>();

        // ── Request → Business ───────────────────────────────────────────────
        CreateMap<CreateSemesterRequest, SemesterBusiness>();
        CreateMap<UpdateSemesterRequest, SemesterBusiness>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateCourseRequest, CourseBusiness>();
        CreateMap<UpdateCourseRequest, CourseBusiness>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateSubjectRequest, SubjectBusiness>();
        CreateMap<UpdateSubjectRequest, SubjectBusiness>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateStudentRequest, StudentBusiness>();
        CreateMap<UpdateStudentRequest, StudentBusiness>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateEnrollmentRequest, EnrollmentBusiness>();
        CreateMap<UpdateEnrollmentRequest, EnrollmentBusiness>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}