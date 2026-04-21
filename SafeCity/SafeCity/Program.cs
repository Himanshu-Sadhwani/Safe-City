using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SafeCity.Hubs;
using SafeCity.Repository;
using SafeCity.Repository.Audit;
using SafeCity.Repository.Case;
using SafeCity.Repository.Compliance;
using SafeCity.Repository.CrisisRepo;
using SafeCity.Repository.Patrol;
using SafeCity.Repository.Response;
using SafeCity.Services.Audit;
using SafeCity.Services.Auth;
using SafeCity.Services.Case;
using SafeCity.Services.Compliance;
using SafeCity.Services.Crisis;
using SafeCity.Services.Dispatch;
using SafeCity.Services.IncidentService;
using SafeCity.Services.PatrolService;
using SafeCity.Services.Resource;
using SafeCity.Services.Response;
using SafeCity.Services.Notification;
using System.Text;
using System.ComponentModel.Design;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddDbContext<SafeCity.Domain.Data.SafeCityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("SafeCity")
    )
);
builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<SafeCity.Repository.IUserRepository, SafeCity.Repository.UserRepository>();
builder.Services.AddScoped<SafeCity.Services.IUserService, SafeCity.Services.UserService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IComplianceRepository, ComplianceRepository>();
builder.Services.AddScoped<IComplianceService, ComplianceService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDispatchService, DispatchService>();
builder.Services.AddScoped<IDispatchRepository, DispatchRepository>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<ICrisisRepository, CrisisRepository>();
builder.Services.AddScoped<ICrisisService, CrisisService>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IPatrolRepository, PatrolRepository>();
builder.Services.AddScoped<IPatrolService, PatrolService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<SafeCity.Repository.FieldReport.IFieldReportRepository, SafeCity.Repository.FieldReport.FieldReportRepository>();
builder.Services.AddScoped<SafeCity.Services.FieldReport.IFieldReportService, SafeCity.Services.FieldReport.FieldReportService>();
builder.Services.AddScoped<SafeCity.Services.Resource.IResourceService,SafeCity.Services.Resource.ResourceService>();
builder.Services.AddScoped<SafeCity.Services.Resource.IResourceService, SafeCity.Services.Resource.ResourceService>();
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

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var error = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return new BadRequestObjectResult(new
        {
            message = error
        });
    };
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

app.MapHub<NotificationHub>("/hubs/notifications");

app.MapControllers();

app.UseStaticFiles();

app.Run();