using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Service.Common;
using Student.Service.Models.Request;
using Student.Service.Models.Response;
using Student.Service.Services;
namespace Student.Service.Controllers;
[ApiController]
[Route("api/students")]
[Authorize]
public class StudentsController(StudentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters param)
        => Ok(ApiResponse<PagedResult<object>>.Ok(await service.GetAllAsync(param)));
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<StudentResponse>.Fail("Student not found"))
            : Ok(ApiResponse<StudentResponse>.Ok(data));
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentRequest request)
        => StatusCode(201, ApiResponse<StudentResponse>.Ok(await service.CreateAsync(request)));
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] UpdateStudentRequest request)
        => await service.PatchAsync(id, request)
            ? Ok(ApiResponse<object>.Ok(null!, "Student updated"))
            : NotFound(ApiResponse<object>.Fail("Student not found"));
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
        => await service.DeleteAsync(id)
            ? Ok(ApiResponse<object>.Ok(null!, "Student deleted"))
            : NotFound(ApiResponse<object>.Fail("Student not found"));
}
