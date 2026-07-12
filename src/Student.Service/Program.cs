using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Student.Service.Grpc;
using Student.Service.Mapper;
using Student.Service.Middleware;
using Student.Service.Services;
var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddGrpc();
var connStr = Environment.GetEnvironmentVariable("DATABASE_URL")
              ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connStr));
builder.Services.AddAutoMapper(cfg => {}, typeof(AppProfile).Assembly);
builder.Services.AddScoped<StudentService>();
var jwtSecret = builder.Configuration["Jwt:Secret"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    });
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "Student Service", Version = "v1" });
    var bearer = new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Name = "Authorization", Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer", BearerFormat = "JWT", In = Microsoft.OpenApi.ParameterLocation.Header
    };
    c.AddSecurityDefinition("Bearer", bearer);
    c.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", doc)] = new List<string>()
    });
});
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging(opt => { opt.MessageTemplate = "{RequestMethod} {RequestPath} -> {StatusCode} in {Elapsed:0}ms"; });
app.UseSwagger();
app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Service v1"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<StudentGrpcService>();
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
app.Run();
