using Microsoft.EntityFrameworkCore;
using UsersTasks.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UsersTasks.Repositories;
using UsersTasks.Services;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Authentication with JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var keyString = jwtSettings["Key"];
if (string.IsNullOrEmpty(keyString))
{
    throw new InvalidOperationException("JWT Key is not configured.");
}
var key = Encoding.UTF8.GetBytes(keyString);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers(
);

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UsersTasks API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {your JWT token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
