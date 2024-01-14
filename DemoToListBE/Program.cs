using DemoToListBE.Configuration;
using DemoToListBE.Data;
using DemoToListBE.Data.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mime;
using System.Text;
using Hellang.Middleware.ProblemDetails;
using System;
using DemoToListBE.ExceptionHandle.Map;
using DemoToListBE.Data.Mapper;
using DemoToListBE.Logic.Service.AppService;
using DemoToListBE.Data.Repository;
using System.Security.Claims;

var AllowedSpecificOrigins = "_allowedSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(options => ProblemDetailsMap.ProlemDetailsMapConfiguration(options, builder.Environment));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(new[] { "https://thankful-smoke-0fc162610.3.azurestaticapps.net", "http://localhost:3000", "https://localhost:3000", "http://localhost:8080", "http://localhost:4200", "http://127.0.0.1:3000" })
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.Configure<ConnectionStrings>(
    builder.Configuration.GetSection(ConnectionStrings.ConnectionStringName));
IConfiguration config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();
builder.Services.AddSingleton(config);
builder.Services.Configure<JwtConfig>(
    builder.Configuration.GetSection(JwtConfig.JwtConfigName));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddCookie(cookieOptions =>
{
    cookieOptions.Cookie.Name = "Todo_jwt_token";
    cookieOptions.Cookie.Path = "/";
    cookieOptions.Cookie.HttpOnly = true;
    cookieOptions.Cookie.IsEssential = true;
}).AddJwtBearerConfiguration(
    Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]));


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>().SQLServerDb));

builder.Services.AddIdentityCore<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    //Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 2;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

builder.Services.AddScoped<IApplicationToDoListService, ApplicationService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseProblemDetails();
app.UseHttpsRedirection();

app.UseCors(AllowedSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
