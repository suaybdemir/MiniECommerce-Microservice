using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniECommerce.Gateway.YARP.Context;
using MiniECommerce.Gateway.YARP.Dtos;
using MiniECommerce.Gateway.YARP.Models;
using MiniECommerce.Gateway.YARP.Services;
using System.Text;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "default",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"))
);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (builder.Configuration.GetSection("JWT:SecretKey").Value ?? "")),
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


//Register
app.MapPost("/auth/register", async (
    
    ApplicationDbContext context,
    RegisterDto request,
    CancellationToken cancellationToken) =>
{
    bool isUserNameExists = await context.Users.AnyAsync(p=>p.UserName==request.UserName,cancellationToken);

    if(isUserNameExists)
    {
        return Results.BadRequest(Result<string>.Failure("This username already exists!"));
    }

    User user = new()
    {
        UserName = request.UserName,
        Password = request.Password
    };

    context.Add(user);

    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(Result<string>.Succeed("Successfully signed up!"));
});

//Login
app.MapPost("/auth/login", async (
    
    ApplicationDbContext context,
    LoginDto request,
    CancellationToken cancellationToken) =>
{
    User? user = await context.Users.FirstOrDefaultAsync(p=>p.UserName==request.UserName,cancellationToken);

    if(user is null)
    {
        return Results.BadRequest(Result<string>.Failure("User couldnt found!"));
    }

    JwtProvider jwtProvider = new(builder.Configuration);

    //token creation
    string token = jwtProvider.CreateToken(user);

    return Results.Ok(Result<string>.Succeed(token));
});

app.UseCors("default");

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

using(var scoped = app.Services.CreateScope())
{
    var srv = scoped.ServiceProvider;
    var context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}



app.Run();