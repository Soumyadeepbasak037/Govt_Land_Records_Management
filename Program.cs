using govt_land_service.DTO;
using govt_land_service.DTO.ProjectDTO;
using govt_land_service.Models;
using govt_land_service.Service;
using govt_land_service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

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
            ),
            RoleClaimType = ClaimTypes.Role
        };
    });




// Add services to the container.
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();


// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

//Roles Routes

app.MapPost("/roles/CreateRole", async (
    CreateRoleRequestDTO request,
    IRoleService roleService
    ) =>
{
    var roleIDs = await roleService.CreateRole(request.roleNames);
    return Results.Json(new {roleids =  roleIDs}, statusCode: StatusCodes.Status201Created);
}
);

app.MapPost("/roles/GetRoleById", async(
       GetRolesRequestDTO request,
       IRoleService roleService
    ) =>
{
    RolesModel response = await roleService.GetRolesByUserIdAsync(request.userId);
    if (response is null)
    {
        return Results.Json(new { message = "Error occured during fetching Roles" },statusCode: StatusCodes.Status500InternalServerError);
    }
    return Results.Json(response);
});

app.MapPost("/roles/AssignRolesByUserId", async (AssignOrRemoveRolesByUserIdRequestDTO request, IRoleService roleService) =>
{
    RolesResponseDTO response = await roleService.AssignRolesToUser(request.userId, request.roleIds);
    if (response is null) {
        return Results.Json(new { message = "Couldn't assign role to user" });
    }
    return Results.Json<RolesResponseDTO>(response);
});

//Location Service Routes

app.MapGet("/location/getStates", async (ILocationService locationService) => {
    LocationData location = await locationService.GetStates();
    if (location is null) {
        return Results.Json(new { message = "No States currently in db" });
    }
    return Results.Json(location);
});

app.MapPost("/location/InsertDistricts", async (InsertDistrictsRequest request,ILocationService locationService)=>{

    int affectedRows = await locationService.InsertDistricts(request.state_id,request.districts);
    if(affectedRows == -1)
    {
        return Results.Json(new { message = "Districts couldnt be inserted" });

    }
    return Results.Json(new {message = $"{affectedRows} districts were newly inserted"});
});

//Project Routes
app.MapPost("/projects/new", async (CreateProjectDTO request, IProjectService service) => {
    ProjectResponseDTO response = await service.CreateProjectAsync(request);
    return Results.Json<ProjectResponseDTO>(response);
});
app.Run();
