using Course.Service.Common;
using Course.Service.Models.Request;
using Course.Service.Models.Response;
using Course.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Course.Service.Controllers;
[ApiController]
[Route("api/enrollments")]
[Authorize]
public class EnrollmentsController(EnrollmentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters param)
        => Ok(ApiResponse<PagedResult<object>>.Ok(await service.GetAllAsync(param)));
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var d = await service.GetByIdAsync(id);
        return d is null ? NotFound(ApiResponse<EnrollmentResponse>.Fail("Not found")) : Ok(ApiResponse<EnrollmentResponse>.Ok(d));
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest req)
    {
        var (result, error) = await service.CreateAsync(req);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : StatusCode(201, ApiResponse<EnrollmentResponse>.Ok(result!));
    }
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] UpdateEnrollmentRequest req)
        => await service.PatchAsync(id, req) ? Ok(ApiResponse<object>.Ok(null!, "Updated")) : NotFound(ApiResponse<object>.Fail("Not found"));
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
        => await service.DeleteAsync(id) ? Ok(ApiResponse<object>.Ok(null!, "Deleted")) : NotFound(ApiResponse<object>.Fail("Not found"));
}
