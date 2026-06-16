using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models.Request;
using PRN232.LMS.Services.Models.Response;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/subjects")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class SubjectsController(SubjectService service) : ControllerBase
{
    private readonly SubjectService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<object>>>> GetAll(
        [FromQuery] QueryParameters param,
        [FromHeader(Name = "X-Request-Id")] string? requestId = null)
    {
        var data = await _service.GetAllAsync(param);
        return Ok(ApiResponse<PagedResult<object>>.Ok(data));
    }

    [HttpGet("{id:int}", Name = "GetSubjectById")]
    [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SubjectResponse>>> GetById([FromRoute] int id)
    {
        var data = await _service.GetByIdAsync(id);
        return data is null ? NotFound(ApiResponse<SubjectResponse>.Fail("Subject not found"))
                            : Ok(ApiResponse<SubjectResponse>.Ok(data));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SubjectResponse>>> Create([FromBody] CreateSubjectRequest request)
    {
        var data = await _service.CreateAsync(request);
        return StatusCode(201, ApiResponse<SubjectResponse>.Ok(data));
    }

    [HttpPatch("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Patch([FromRoute] int id, [FromBody] UpdateSubjectRequest request)
        => await _service.PatchAsync(id, request)
            ? Ok(ApiResponse<object>.Ok(null!, "Subject updated"))
            : NotFound(ApiResponse<object>.Fail("Subject not found"));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete([FromRoute] int id)
        => await _service.DeleteAsync(id)
            ? Ok(ApiResponse<object>.Ok(null!, "Subject deleted"))
            : NotFound(ApiResponse<object>.Fail("Subject not found"));
}