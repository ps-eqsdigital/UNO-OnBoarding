using Data.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Interfaces;
using DataAccess.Base;
using Business.Base;
using Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Business.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Uno OnBoarding API",
        Version = "1.0"
    });
});

var connectionString = builder.Configuration.GetConnectionString("UnoOnBoarding");
builder.Services.AddDbContext<UnoOnBoardingContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IGenericDataAccessObject, GenericDataAccessObject>();
builder.Services.AddScoped<IGenericBusinessObject, GenericBusinessObject>();
builder.Services.AddScoped<IUserBusinessObject, UserBusinessObject>();

var app = builder.Build();

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using (var scope = serviceScopeFactory.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<UnoOnBoardingContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UNO OnBoarding v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
