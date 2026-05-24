using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController(StudentService service) : Controller
{
    private readonly StudentService _service = service;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<object>>>> GetAll([FromQuery] QueryParameters param)
    {
        var data = await _service.GetAllAsync(param);
        return Ok(ApiResponse<PagedResult<object>>.Ok(data));
    }

    [HttpGet("{id:int}")]

    public async Task<ActionResult<ApiResponse<StudentResponse>>> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id);
        return data is null ? NotFound(ApiResponse<StudentResponse>.Fail("Student not found"))
                            : Ok(ApiResponse<StudentResponse>.Ok(data));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StudentResponse>>> Create(CreateStudentRequest request)
    {
        var data = await _service.CreateAsync(request);
        return StatusCode(201, ApiResponse<StudentResponse>.Ok(data));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Patch(int id, UpdateStudentRequest request)
        => await _service.PatchAsync(id, request)
            ? Ok(ApiResponse<object>.Ok(null!, "Student updated"))
            : NotFound(ApiResponse<object>.Fail("Student not found"));

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        => await _service.DeleteAsync(id)
            ? Ok(ApiResponse<object>.Ok(null!, "Student deleted"))
            : NotFound(ApiResponse<object>.Fail("Student not found"));
}