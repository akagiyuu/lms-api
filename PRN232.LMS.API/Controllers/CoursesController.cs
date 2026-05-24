using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController(CourseService service) : ControllerBase
{
    private readonly CourseService _service = service;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<object>>>> GetAll([FromQuery] QueryParameters param)
    {
        var data = await _service.GetAllAsync(param);
        return Ok(ApiResponse<PagedResult<object>>.Ok(data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CourseResponse>>> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id);
        return data is null ? NotFound(ApiResponse<CourseResponse>.Fail("Course not found"))
                            : Ok(ApiResponse<CourseResponse>.Ok(data));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CourseResponse>>> Create(CreateCourseRequest request)
    {
        var data = await _service.CreateAsync(request);
        return StatusCode(201, ApiResponse<CourseResponse>.Ok(data));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Patch(int id, UpdateCourseRequest request)
        => await _service.PatchAsync(id, request)
            ? Ok(ApiResponse<object>.Ok(null!, "Course updated"))
            : NotFound(ApiResponse<object>.Fail("Course not found"));

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        => await _service.DeleteAsync(id)
            ? Ok(ApiResponse<object>.Ok(null!, "Course deleted"))
            : NotFound(ApiResponse<object>.Fail("Course not found"));
}