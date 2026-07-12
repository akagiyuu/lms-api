using Grpc.Core;
using Student.Service.Services;
namespace Student.Service.Grpc;
public class StudentGrpcService(StudentService studentService) : StudentGrpc.StudentGrpcBase
{
    public override async Task<StudentReply> GetStudentById(StudentRequest request, ServerCallContext context)
    {
        var student = await studentService.GetByIdAsync(request.StudentId);
        if (student is null) return new StudentReply { Found = false };
        return new StudentReply
        {
            StudentId   = student.StudentId,
            FullName    = student.FullName,
            Email       = student.Email,
            DateOfBirth = student.DateOfBirth.ToString("O"),
            Found       = true
        };
    }
    public override async Task<ExistsReply> StudentExists(StudentRequest request, ServerCallContext context)
    {
        var exists = await studentService.ExistsAsync(request.StudentId);
        return new ExistsReply { Exists = exists };
    }
}
