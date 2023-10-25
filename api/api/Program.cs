
using Data.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Interfaces;
using DataAccess.Base;
using Business.Base;
using Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Business.BusinessObjects;

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

var app = builder.Build();

var serviceScopeFactory = (IServiceScopeFactory)app.Services.GetService(typeof(IServiceScopeFactory));

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
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(myAllowSpecificOrigins);

app.MapControllers();

app.Run();
