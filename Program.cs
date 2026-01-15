using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RateNowApi.Data;
using RateNowApi.Middleware;
using RateNowApi.Services;
using RateNowApi.Services.Interfaces;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.IdentityModel.Logging;
IdentityModelEventSource.ShowPII = true;



var builder = WebApplication.CreateBuilder(args);

#region Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=movietracker.db"));
#endregion

#region Services (DI)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMoviesService, MoviesService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<ISeriesesService, SeriesesService>();
builder.Services.AddScoped<IWatchListService, WatchListService>();
#endregion

builder.Services.AddHttpContextAccessor();

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
});
#endregion

#region JWT Authentication
#region JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"]
    ?? throw new Exception("Jwt:Key is missing in appsettings.json");

var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.IncludeErrorDetails = true;

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("JWT AUTH FAILED: " + ctx.Exception.Message);
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            

            ValidIssuer = issuer,
            ValidAudience = audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.FromMinutes(1)
            
        };
    });
#endregion

#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("adminOnly", policy =>
        policy.RequireRole("admin"));
});
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region Swagger + JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RateNowApi",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token giriniz"
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
            Array.Empty<string>()
        }
    });
});
#endregion

var app = builder.Build();

#region Database Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
#endregion

#region Middleware
app.UseMiddleware<ExceptionMiddleware>();
#endregion


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();