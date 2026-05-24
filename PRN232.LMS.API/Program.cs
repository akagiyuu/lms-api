using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Mapper;
using PRN232.LMS.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddAutoMapper(cfg => { }, typeof(AppProfile));

builder.Services.AddScoped<SemesterService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<EnrollmentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();