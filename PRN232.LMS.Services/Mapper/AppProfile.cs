using AutoMapper;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Mapper;

public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<Semester, SemesterResponse>().ReverseMap();
        CreateMap<CreateSemesterRequest, Semester>();
        CreateMap<UpdateSemesterRequest, Semester>();

        CreateMap<Subject, SubjectResponse>().ReverseMap();
        CreateMap<CreateSubjectRequest, Subject>();
        CreateMap<UpdateSubjectRequest, Subject>();

        CreateMap<Course, CourseResponse>().ReverseMap();
        CreateMap<CreateCourseRequest, Course>();
        CreateMap<UpdateCourseRequest, Course>();

        CreateMap<Student, StudentResponse>().ReverseMap();
        CreateMap<CreateStudentRequest, Student>();
        CreateMap<UpdateStudentRequest, Student>();

        CreateMap<Enrollment, EnrollmentResponse>().ReverseMap();
    }
}