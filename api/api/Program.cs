
using Data.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Interfaces;
using DataAccess.Base;
using Business.Base;
using Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Business.BusinessObjects;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using DataAccess.DataAccessObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("UnoOnBoarding");
builder.Services.AddDbContext<UnoOnBoardingContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IGenericDataAccessObject, GenericDataAccessObject>();
builder.Services.AddScoped<IGenericBusinessObject, GenericBusinessObject>();
builder.Services.AddScoped<IUserBusinessObject, UserBusinessObject>();
builder.Services.AddScoped<IApiBusinessObject, ApiBusinessObject>();
builder.Services.AddScoped<IUserDataAccessObject, UserDataAccessObject>();
builder.Services.AddScoped<ISensorBusinessObject, SensorBusinessObject>();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Uno OnBoarding API",
        Version = "1.0"
    });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name="Authorization",
        Type=SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
        ClockSkew = TimeSpan.Zero 
    };
});

var app = builder.Build();

var serviceScopeFactory = (IServiceScopeFactory)app.Services.GetService(typeof(IServiceScopeFactory))!;

using (var scope = serviceScopeFactory.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<UnoOnBoardingContext>();
    dbContext.Database.EnsureCreated();
}
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UNO OnBoarding v1");
    });


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseCors(myAllowSpecificOrigins);

app.MapControllers();

app.Run();
