using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PRN232.LMS.API.Common;
using PRN232.LMS.API.Middleware;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Mapper;
using PRN232.LMS.Services.Services;
using PRN232.LMS.Services.Validation;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers: JSON + XML + 406 on unsupported Accept ─────────────────────
builder.Services
    .AddControllers(opt =>
    {
        opt.ReturnHttpNotAcceptable = true;     // returns 406 for unsupported Accept
        opt.RespectBrowserAcceptHeader = true;
        opt.Filters.Add(new ProducesAttribute("application/json", "application/xml"));
    })
    .AddNewtonsoftJson()
    .AddXmlSerializerFormatters();              // supports application/xml

// ── Database ─────────────────────────────────────────────────────────────────
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

// ── Repositories & AutoMapper ────────────────────────────────────────────────
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddAutoMapper(cfg => { }, typeof(AppProfile));

// ── Validation: DataAnnotations response factory + FluentValidation ───────────
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

        return new BadRequestObjectResult(new ApiResponse<object>
        {
            Success = false,
            Message = "One or more validation errors occurred.",
            Errors  = errors
        });
    };
});

// Validators are injected and called manually (FluentValidation v12 removed AspNetCore auto-validation)
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentRequestValidator>();

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SemesterService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<EnrollmentService>();

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtSecret = builder.Configuration["Jwt:Secret"]!;
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

// ── API Versioning ────────────────────────────────────────────────────────────
builder.Services
    .AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
    })
    .AddApiExplorer(opt =>
    {
        opt.GroupNameFormat           = "'v'VVV";
        opt.SubstituteApiVersionInUrl = true;
    });

// ── Swagger / OpenAPI with JWT support ────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // One Swagger doc per API version
    foreach (var version in new[] { "v1", "v2" })
        c.SwaggerDoc(version, new OpenApiInfo { Title = "LMS API", Version = version });

    // JWT Bearer scheme
    var bearerScheme = new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT token (without 'Bearer ' prefix)."
    };
    c.AddSecurityDefinition("Bearer", bearerScheme);

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});
builder.Services.AddSwaggerGenNewtonsoftSupport();

// ────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    // Show a dropdown for each version in Swagger UI
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var desc in provider.ApiVersionDescriptions)
        opt.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                            $"LMS API {desc.GroupName.ToUpper()}");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();