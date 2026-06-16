using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models.Response;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers;

/// <summary>Nested resource: students enrolled in a specific course.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/courses/{courseId:int}/students")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CourseStudentsController(EnrollmentService enrollmentService) : ControllerBase
{
    private readonly EnrollmentService _enrollmentService = enrollmentService;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<object>>>> GetStudents(
        int courseId,
        [FromQuery] QueryParameters param,
        [FromHeader(Name = "X-Request-Id")] string? requestId = null)
    {
        var data = await _enrollmentService.GetStudentsByCourseAsync(courseId, param);
        return Ok(ApiResponse<PagedResult<object>>.Ok(data));
    }
}
