using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SafeCity.Repository;
using SafeCity.Repository.Case;
using SafeCity.Repository.CrisisRepo;
using SafeCity.Repository.Patrol;
using SafeCity.Services.Auth;
using SafeCity.Services.Case;
using SafeCity.Services.Crisis;
using SafeCity.Services.Dispatch;
using SafeCity.Services.IncidentService;
using SafeCity.Services.PatrolService;
using System.Text;
using SafeCity.Repository.Response;
using SafeCity.Services.Response;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<SafeCity.Domain.Data.SafeCityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("SafeCity")
    )
);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        var errors = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e=>e.ErrorMessage)
            .ToList();
 
        return new BadRequestObjectResult(new
        {
            messages = errors
        });
    };
});
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<SafeCity.Repository.IUserRepository, SafeCity.Repository.UserRepository>();
builder.Services.AddScoped<SafeCity.Services.IUserService, SafeCity.Services.UserService>();
builder.Services.AddScoped<SafeCity.Services.Auth.IAuthService, SafeCity.Services.Auth.AuthService>();
builder.Services.AddScoped<SafeCity.Services.Audit.IAuditService, SafeCity.Services.Audit.AuditService>();
builder.Services.AddScoped<SafeCity.Repository.Audit.IAuditRepository, SafeCity.Repository.Audit.AuditRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDispatchService, DispatchService>();
builder.Services.AddScoped<IDispatchRepository, DispatchRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<ICrisisRepository, CrisisRepository>();
builder.Services.AddScoped<ICrisisService, CrisisService>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IPatrolRepository, PatrolRepository>();
builder.Services.AddScoped<IPatrolService, PatrolService>();
builder.Services.AddScoped<IResponseRepository, ResponseRepository>();
builder.Services.AddScoped<IResponseService, ResponseService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authentication using Bearer scheme"
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", doc), new List<string>() }
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();