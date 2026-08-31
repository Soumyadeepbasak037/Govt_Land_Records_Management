using govt_land_service.Service;
using govt_land_service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using govt_land_service.DTO;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key not configured.");




builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
)
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

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });




// Add services to the container.
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();


app.MapControllers();
//Auth Routes
app.MapPost("/login", async (
    govt_land_service.DTO.LoginRequestDTO request,
    IAuthService authService) =>
{
    var token = await authService.LoginAsync(
        request.Email,
        request.Password
    );

    if (token is null)
    {

        return Results.Json(new { message = "You are not authorized to access this resource." },
                            statusCode: StatusCodes.Status401Unauthorized);


    }
    return Results.Json(new { token = token }, statusCode: StatusCodes.Status200OK);
});

app.MapPost("/register", async (
    RegisterRequestDTO request,
    IAuthService authService) =>
{
    var userId = await authService.RegisterAsync(request);
    if (userId is null)
    {
        return Results.Json(new { message = "Registration failed." },
                            statusCode: StatusCodes.Status400BadRequest);
    }
    return Results.Json(new { userId = userId }, statusCode: StatusCodes.Status201Created);
});
app.Run();
