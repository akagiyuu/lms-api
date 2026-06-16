using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models.Request;
using PRN232.LMS.Services.Models.Response;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers;

/// <summary>Students API — v1 and v2 supported. v2 adds GET /count.</summary>
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/students")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class StudentsController(StudentService service) : ControllerBase
{
    private readonly StudentService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<object>>>> GetAll(
        [FromQuery] QueryParameters param,
        [FromHeader(Name = "X-Request-Id")] string? requestId = null)
    {
        var data = await _service.GetAllAsync(param);
        return Ok(ApiResponse<PagedResult<object>>.Ok(data));
    }

    [HttpGet("{id:int}", Name = "GetStudentById")]
    [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<StudentResponse>>> GetById([FromRoute] int id)
    {
        var data = await _service.GetByIdAsync(id);
        return data is null ? NotFound(ApiResponse<StudentResponse>.Fail("Student not found"))
                            : Ok(ApiResponse<StudentResponse>.Ok(data));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<StudentResponse>>> Create(
        [FromBody] CreateStudentRequest request,
        [FromServices] IValidator<CreateStudentRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(ApiResponse<object>.Fail("Validation failed.", errors));
        }
        var data = await _service.CreateAsync(request);
        return StatusCode(201, ApiResponse<StudentResponse>.Ok(data));
    }

    [HttpPatch("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Patch([FromRoute] int id, [FromBody] UpdateStudentRequest request)
        => await _service.PatchAsync(id, request)
            ? Ok(ApiResponse<object>.Ok(null!, "Student updated"))
            : NotFound(ApiResponse<object>.Fail("Student not found"));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete([FromRoute] int id)
        => await _service.DeleteAsync(id)
            ? Ok(ApiResponse<object>.Ok(null!, "Student deleted"))
            : NotFound(ApiResponse<object>.Fail("Student not found"));

    // ── v2 only ──────────────────────────────────────────────────────────────

    /// <summary>[v2] Returns total student count.</summary>
    [HttpGet("count")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<int>>> GetCount()
    {
        var paged = await _service.GetAllAsync(new QueryParameters { Size = 1 });
        return Ok(ApiResponse<int>.Ok(paged.TotalItems, "Total student count"));
    }
}