using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using CarRental.Api.data;
using CarRental.Api.Interfaces;
using CarRental.Api.Models;
using CarRental.Api.Repositories;
using CarRental.Api.Services;
using CarRental.Api.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Accept enums as strings
                // Prevent object cycles during serialization (e.g., Booking -> Car -> Bookings -> ...)
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });


builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();




builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
})
;
// 2. Add Identity
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
        NameClaimType = ClaimTypes.NameIdentifier,
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var hasAuth = context.Request.Headers.ContainsKey("Authorization");
            Console.WriteLine($"[JwtEvents] MessageReceived. Has Authorization header: {hasAuth}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var nameId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"[JwtEvents] Token validated. NameIdentifier: {nameId}");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"[JwtEvents] Authentication failed: {context.Exception?.Message}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"[JwtEvents] OnChallenge: {context.Error} - {context.ErrorDescription}");
            return Task.CompletedTask;
        }
    };

});

// Cookie redirect configuration moved after Identity registration to ensure it is applied correctly.
builder.Services.AddAuthorization();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Ensure Identity cookie redirection returns 401/403 for APIs (avoid redirect to login page)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// Post-configure authentication options so JwtBearer is the default challenge and authenticate scheme
builder.Services.PostConfigure<AuthenticationOptions>(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

builder.Services.AddCors(options =>
        {
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? throw new InvalidOperationException("Allowed origins are not configured.");

            options.AddPolicy("DefaultCors", policy =>
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.ShowSidebar = true;
        options.Theme = ScalarTheme.BluePlanet;
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedUsers.SeedDefaultAdminUserAsync(userManager, roleManager);
}
app.UseStaticFiles();

app.UseCors("DefaultCors");

app.UseHttpsRedirection();
app.UseRouting();

// Debug middleware to log Authorization header presence (do not log full token)
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var hasAuth = context.Request.Headers.ContainsKey("Authorization");
    var authVal = context.Request.Headers["Authorization"].FirstOrDefault();
    Console.WriteLine($"[AuthDebug] Path: {path} | HasAuthHeader: {hasAuth} | AuthHeaderLen: {(authVal?.Length ?? 0)}");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();





app.Run();


