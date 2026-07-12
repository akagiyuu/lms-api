using AutoMapper;
using Student.Service.Models;
using Student.Service.Models.Response;
namespace Student.Service.Mapper;
public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<DateTime, DateTimeOffset>().ConvertUsing(d => new DateTimeOffset(d, TimeSpan.Zero));
        CreateMap<StudentEntity, StudentResponse>();
    }
}
