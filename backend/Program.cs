using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PublicTransportNavigator;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.Repositories;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Services;
using StackExchange.Redis;
using System.Text;

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

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
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

builder.Services.AddSingleton<IPathFinderManager, PathFinderManager>();


builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IBusStopRepository, BusStopRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITimetableRepository, TimetableRepository>();
builder.Services.AddScoped<IUserFavouriteBusStopRepository, UserFavouriteBusStopRepository>();
builder.Services.AddScoped<IBusTypeRepository, BusTypeRepository>();
builder.Services.AddScoped<IBusSeatRepository, BusSeatRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IReservedSeatRepository, ReservedSeatRepository>();
builder.Services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
builder.Services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});


builder.Services.AddScoped<RedisCacheService>();
builder.Services.AddHostedService<ReservationCleanup>();
builder.Services.AddHostedService<FetchDataService>();


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

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.Run();


//wsl
//user: wiktoria
//sudo service redis-server start
