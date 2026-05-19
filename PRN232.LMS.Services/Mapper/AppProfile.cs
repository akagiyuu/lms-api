using AutoMapper;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Mapper;

public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<Semester, BasicSemesterResponse>();
        CreateMap<Semester, SemesterResponse>();

        CreateMap<Course, BasicCourseResponse>();
        CreateMap<Course, CourseResponse>();

        CreateMap<Subject, SubjectResponse>();

        CreateMap<Student, BasicStudentResponse>();
        CreateMap<Student, StudentResponse>();

        CreateMap<Enrollment, BasicEnrollmentResponse>();
        CreateMap<Enrollment, EnrollmentResponse>();

        CreateMap<CreateSemesterRequest, Semester>();
        CreateMap<UpdateSemesterRequest, Semester>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateCourseRequest, Course>();
        CreateMap<UpdateCourseRequest, Course>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateSubjectRequest, Subject>();
        CreateMap<UpdateSubjectRequest, Subject>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateStudentRequest, Student>();
        CreateMap<UpdateStudentRequest, Student>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateEnrollmentRequest, Enrollment>();
        CreateMap<UpdateEnrollmentRequest, Enrollment>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}