using System.Diagnostics;
using System.Text;
using kimbumjun.Data;
using kimbumjun.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// (1) Cors
const string corsapp = "corsapp";
builder.Services.AddCors(p => p.AddPolicy(corsapp, x => { x.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://localhost:5555");
// $ dotnet publish --configuration Release -o "/Users/<User>/Web/Publish/api.kimbumjun.com"
// Db Inject Database config
builder.Services.AddDbContext<CSharpDbContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnectionString")));

// Authentication (인증)
builder.Services.AddIdentity<AppUser, IdentityRole>(x => { }).AddEntityFrameworkStores<CSharpDbContext>();

try
{
    // JWT Configuration
    // builder.Services.Configure<JWTConfig>(app.Configuration.GetSection("JWTConfig"));
    builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig"));

    builder.Services.AddAuthentication(x =>
    {
        // 추가 수정된 부분
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        // JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["JWTConfig:Key"]);
        var issuer = builder.Configuration["JWTConfig:Issuer"];
        var audience = builder.Configuration["JWTConfig:Audience"];
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ValidIssuer = issuer,
            ValidAudience = audience
        };
    });
}
catch (InvalidOperationException ex)
{
    throw new Exception(ex.Message);
}
catch (Exception ex)
{
    Debug.WriteLine(ex.Message);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(options);
app.UseStaticFiles();

// (2) Cors
app.UseCors(corsapp);

// Add Atuthentication 
app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapControllers();
app.Run();
