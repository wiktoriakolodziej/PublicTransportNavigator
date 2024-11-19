using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Repositories;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using PublicTransportNavigator.Dijkstra.AStar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        configure =>
        {
            configure.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<PublicTransportNavigatorContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "time-span",
        Example = new OpenApiString("02:30:00")
    });
});
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSingleton<IPathFinderManager, PathFinderManager<Node>>();


builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IBusStopRepository, BusStopRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITimetableRepository, TimetableRepository>();
builder.Services.AddScoped<IUserFavouriteBusStopRepository, UserFavouriteBusStopRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
