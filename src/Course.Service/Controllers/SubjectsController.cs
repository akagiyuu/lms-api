using Course.Service.Common;
using Course.Service.Models.Request;
using Course.Service.Models.Response;
using Course.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Course.Service.Controllers;
[ApiController]
[Route("api/subjects")]
[Authorize]
public class SubjectsController(SubjectService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters param)
        => Ok(ApiResponse<PagedResult<object>>.Ok(await service.GetAllAsync(param)));
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var d = await service.GetByIdAsync(id);
        return d is null ? NotFound(ApiResponse<SubjectResponse>.Fail("Not found")) : Ok(ApiResponse<SubjectResponse>.Ok(d));
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubjectRequest req)
        => StatusCode(201, ApiResponse<SubjectResponse>.Ok(await service.CreateAsync(req)));
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] UpdateSubjectRequest req)
        => await service.PatchAsync(id, req) ? Ok(ApiResponse<object>.Ok(null!, "Updated")) : NotFound(ApiResponse<object>.Fail("Not found"));
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
        => await service.DeleteAsync(id) ? Ok(ApiResponse<object>.Ok(null!, "Deleted")) : NotFound(ApiResponse<object>.Fail("Not found"));
}
