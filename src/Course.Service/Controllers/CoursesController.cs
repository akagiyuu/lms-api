using Course.Service.Common;
using Course.Service.Models.Request;
using Course.Service.Models.Response;
using Course.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Course.Service.Controllers;
[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesController(CourseService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters param)
        => Ok(ApiResponse<PagedResult<object>>.Ok(await service.GetAllAsync(param)));
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var d = await service.GetByIdAsync(id);
        return d is null ? NotFound(ApiResponse<CourseResponse>.Fail("Not found")) : Ok(ApiResponse<CourseResponse>.Ok(d));
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequest req)
        => StatusCode(201, ApiResponse<CourseResponse>.Ok(await service.CreateAsync(req)));
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] UpdateCourseRequest req)
        => await service.PatchAsync(id, req) ? Ok(ApiResponse<object>.Ok(null!, "Updated")) : NotFound(ApiResponse<object>.Fail("Not found"));
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
        => await service.DeleteAsync(id) ? Ok(ApiResponse<object>.Ok(null!, "Deleted")) : NotFound(ApiResponse<object>.Fail("Not found"));
}
