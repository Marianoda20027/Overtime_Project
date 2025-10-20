using System.Text; 
using System.Security.Claims;
using Microsoft.OpenApi.Models; 
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using api.Data; 
using api.BusinessLogic.Services;
using api.BusinessLogic.Services.Reports;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// ===== Config JWT =====
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

// ===== Servicios =====
builder.Services.AddControllers();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<IEmailService, SMTPService>();
builder.Services.AddScoped<SMTPService>();
builder.Services.AddScoped<ReportsService>();
QuestPDF.Settings.License = LicenseType.Community;






// EF Core (SQL Server). Se apoya en "ConnectionStrings:Default" en appsettings.json
builder.Services.AddDbContext<OvertimeContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
    

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite cualquier origen
              .AllowAnyMethod()  // Permite cualquier método (GET, POST, etc.)
              .AllowAnyHeader(); // Permite cualquier cabecera
    });
});

// Auth + JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

// Swagger con JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Introduce tu token JWT con el formato: **Bearer {token}**",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// ===== Migrar/crear BD al iniciar (idempotente) =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OvertimeContext>();
    try
    {
        db.Database.Migrate(); // crea la BD si no existe y aplica migraciones pendientes
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[DB MIGRATION ERROR] {ex.Message}");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
