using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/semesters")]
public class SemestersController(SemesterService service) : ControllerBase
{
    private readonly SemesterService _service = service;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<object>>>> GetAll([FromQuery] QueryParameters param)
    {
        var data = await _service.GetAllAsync(param);
        return Ok(ApiResponse<PagedResult<object>>.Ok(data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<SemesterResponse>>> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id);
        return data is null ? NotFound(ApiResponse<SemesterResponse>.Fail("Semester not found")) : Ok(ApiResponse<SemesterResponse>.Ok(data));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SemesterResponse>>> Create(CreateSemesterRequest request)
    {
        var data = await _service.CreateAsync(request);
        return StatusCode(201, ApiResponse<SemesterResponse>.Ok(data, "Semester created"));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Patch(int id, [FromBody] UpdateSemesterRequest request)
        => await _service.PatchAsync(id, request)
            ? Ok(ApiResponse<object>.Ok(null!, "Semester updated"))
            : NotFound(ApiResponse<object>.Fail("Semester not found"));

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        => await _service.DeleteAsync(id)
            ? Ok(ApiResponse<object>.Ok(null!, "Semester deleted"))
            : NotFound(ApiResponse<object>.Fail("Semester not found"));
}