using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class EnrollmentsController(EnrollmentService service) : ControllerBase
{
    private readonly EnrollmentService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<object>>>> GetAll([FromQuery] QueryParameters param)
    {
        var data = await _service.GetAllAsync(param);
        return Ok(ApiResponse<PagedResult<object>>.Ok(data));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<EnrollmentResponse>>> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id);
        return data is null ? NotFound(ApiResponse<EnrollmentResponse>.Fail("Enrollment not found"))
                            : Ok(ApiResponse<EnrollmentResponse>.Ok(data));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<EnrollmentResponse>>> Create(CreateEnrollmentRequest request)
    {
        var data = await _service.CreateAsync(request);
        return StatusCode(201, ApiResponse<EnrollmentResponse>.Ok(data));
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Patch(int id, UpdateEnrollmentRequest request)
        => await _service.PatchAsync(id, request)
            ? Ok(ApiResponse<object>.Ok(null!, "Enrollment updated"))
            : NotFound(ApiResponse<object>.Fail("Enrollment not found"));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        => await _service.DeleteAsync(id)
            ? Ok(ApiResponse<object>.Ok(null!, "Enrollment deleted"))
            : NotFound(ApiResponse<object>.Fail("Enrollment not found"));
}